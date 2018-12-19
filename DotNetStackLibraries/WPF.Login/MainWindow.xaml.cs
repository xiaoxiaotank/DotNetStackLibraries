using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.Login
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _emptyPassword = "";
        /// <summary>
        /// 存储账户和密码信息
        /// </summary>
        private readonly static Dictionary<string, string> _accountsDic = new Dictionary<string, string>();

        /// <summary>
        /// 账户加密方式
        /// </summary>
        private readonly static Func<string, string> _accountEncrypt = EncryptHelper.Base64Encrypt;

        /// <summary>
        /// 账户解密方式
        /// </summary>
        private readonly static Func<string, string> _accountDecrypt = EncryptHelper.Base64Decrypt;

        /// <summary>
        /// 同步线程上下文 解决UI阻塞问题（更新UI时使用）
        /// </summary>
        private readonly static TaskScheduler _syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// 账户密码信息文件路径
        /// </summary>
        private readonly static string _filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.dat");

        /// <summary>
        /// ComboBox能够存储的最大账户数量
        /// </summary>
        private readonly static int _maxAccountCount = 8;

        /// <summary>
        /// 用于取消登陆的TokenSource
        /// </summary>
        private static CancellationTokenSource _loginCts = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();

            #region 绑定图片资源
            ImgSetting.DataContext = new ImageViewModel() { Source = ImageResource.Setting };
            ImgClose.DataContext = new ImageViewModel() { Source = ImageResource.Close };
            ImgLogo.DataContext = new ImageViewModel() { Source = ImageResource.Logo };
            #endregion
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 绑定图片资源
            (ControlHelper.GetChildObject<Border>(CmbUserName, "Bg")
                    .FindName("TbOpen") as Image)
                    .DataContext = new ImageViewModel() { Source = ImageResource.Open };
            #endregion

            InitAccountsInfo();
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            e.Handled = true;
        }

        private void Window_Close(object sender, MouseButtonEventArgs e)
        {
            if (((TextBlock)sender).Equals(TbClose))
                this.Close();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var userName = CmbUserName.Text.Trim();
            var password = Password.Password;
            Task.Factory.StartNew(() => Login(userName, password));
        }

        private void CmbUserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userName = string.Empty;
            if (CmbUserName.SelectedItem != null)
            {
                userName = CmbUserName.SelectedItem.ToString();
                if (!string.IsNullOrWhiteSpace(_accountsDic[userName]))
                {
                    Password.Password = _accountsDic[userName];
                    IsRememberPassword.IsChecked = true;
                    return;
                }
            }

            IsRememberPassword.IsChecked = false;
            Password.Password = string.Empty;
        }

        private void Part_Delete_Click(object sender, RoutedEventArgs e)
        {
            var userName = ((Button)sender).DataContext.ToString();
            CmbUserName.Items.Remove(userName);
            _accountsDic.Remove(userName);

            SaveAccountsInfo();
        }

        /// <summary>
        /// 脱机登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOffLine_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void IsAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            IsRememberPassword.IsChecked = true;
        }

        private void IsRememberPassword_Click(object sender, RoutedEventArgs e)
        {
            if (IsRememberPassword.IsChecked == false && IsAutoLogin.IsChecked == true)
                IsAutoLogin.IsChecked = false;
        }

        /// <summary>
        /// 取消登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelLogin_Click(object sender, RoutedEventArgs e)
        {
            _loginCts.Cancel();
        }


        #region 辅助方法
        private void SetPlaceholder(object sender, EventArgs e)
        {
            ControlHelper.SetPlaceholder(sender, e);
        }

        /// <summary>
        /// 初始化所有账户信息
        /// </summary>
        private void InitAccountsInfo()
        {
            var accounts = FileHelper.ReadFileByStream(_filePath, System.IO.FileMode.OpenOrCreate, 1).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (accounts.Length <= 0)
            {
                return;
            }

            var flag = Boolean.Parse(_accountDecrypt(accounts[0]));
            #region 后台线程
            foreach (var account in accounts.Skip(1))
            {
                var decryptInfo = _accountDecrypt(account);
                var accountInfo = decryptInfo.Split();
                var userName = accountInfo[0];
                var password = accountInfo[1];
                if (!CmbUserName.Items.Contains(userName))
                {
                    CmbUserName.Items.Add(userName);
                    if (!_accountsDic.ContainsKey(userName))
                    {
                        _accountsDic[userName] = password;
                    }
                }
            }
            #endregion

            if (flag)
            {
                #region 后台线程
                var decryptInfo = _accountDecrypt(accounts[1]);
                var accountInfo = decryptInfo.Split();
                var userName = accountInfo[0];
                var password = accountInfo[1];
                //初始化自动登录
                IsAutoLogin.IsChecked = true;
                IsRememberPassword.IsChecked = true;
                CmbUserName.SelectedItem = userName;
                Password.Password = _accountsDic[userName];

                Task.Factory.StartNew(() => Login(userName, password));
                #endregion
            }
            else
            {
                CmbUserName.SelectedItem = CmbUserName.Items[CmbUserName.Items.Count - 1];
                Password.Password = _accountsDic[CmbUserName.SelectedItem.ToString()];
                if (!Password.Password.Equals(_emptyPassword))
                    IsRememberPassword.IsChecked = true;
            }
        }

        /// <summary>
        /// 存储账户信息
        /// </summary>
        private void SaveAccountsInfo()
        {
            var datas = new List<string>();
            //需要保存的账户个数
            var takeCount = CmbUserName.Items.Count;
            //文件头标记  指示是否自动登录
            var flag = (bool)(IsAutoLogin.IsChecked);
            datas.Add(_accountEncrypt(flag.ToString()));
            if (flag)
            {
                var autoUserName = (string)(CmbUserName.Items[takeCount - 1]);
                var cryptInfo = _accountEncrypt(autoUserName + " " + _accountsDic[autoUserName]);
                datas.Add(cryptInfo);
                takeCount--;
            }
            for (int i = 0; i < takeCount; i++)
            {
                var userName = CmbUserName.Items[i] as string;
                var cryptInfo = _accountEncrypt(userName + " " + _accountsDic[userName]);
                datas.Add(cryptInfo);
            }
            FileHelper.SaveFileByLine(datas.ToArray(), _filePath, System.IO.FileMode.Create);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void Login(string userName, string password)
        {
            if (!ValidateUserName(userName)) return;
            var updateUI = Task.Factory.StartNew(() => UpdateUIStatus(true));

            updateUI.ContinueWith(task =>
            {
                var verifyUser = Task.Factory.StartNew(() => VerifyUser(userName, password), _loginCts.Token);

                //是否超时
                var isTimeout = false;
                Task.Factory.StartNew(() =>
                {
                    //设定超时时间
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    if (!verifyUser.IsCompleted)
                    {
                        _loginCts.Cancel();
                        Console.WriteLine("登陆超时，请稍候重试！");
                        UpdateUIStatus(false);
                        isTimeout = true;
                    }
                });

                verifyUser.ContinueWith(t =>
                {
                    if (!isTimeout)
                    {
                        //取消登录
                        if (t.IsCanceled)
                            UpdateUIStatus(false);
                        // 登录失败 返回了错误信息
                        else if (t.Exception != null)
                        {
                            Console.WriteLine(t.Exception.InnerException.Message);
                            UpdateUIStatus(false);
                        }
                        //登录成功
                        else
                            Task.Factory.StartNew(() => InitCurrentAccountInfo(t.Result));
                    }
                    _loginCts = new CancellationTokenSource();
                });
            });
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool VerifyUser(string userName, string password)
        {
#warning 测试超时
            Thread.Sleep(1 * 1000);
 
            _loginCts.Token.ThrowIfCancellationRequested();

            return true;
        }

        /// <summary>
        /// 根据是否处于登录状态来更新UI信息
        /// </summary>
        /// <param name="isLogining"></param>
        private void UpdateUIStatus(bool isLogining)
        {
            Task.Factory.StartNew(() =>
            {
                if (isLogining)
                {
                    //LayerPanel.IsEnabled = false;
                    CmbUserName.IsEnabled
                        = Password.IsEnabled
                        = IsRememberPassword.IsEnabled
                        = IsAutoLogin.IsEnabled
                        = BtnOffLine.IsEnabled
                        = false;
                    BtnLogin.Visibility = Visibility.Collapsed;
                    BtnCancelLogin.Visibility = Visibility.Visible;
                }
                else
                {
                    CmbUserName.IsEnabled
                        = Password.IsEnabled
                        = IsRememberPassword.IsEnabled
                        = IsAutoLogin.IsEnabled
                        = BtnOffLine.IsEnabled
                        = true;
                    IsAutoLogin.IsChecked = false;
                    BtnCancelLogin.Visibility = Visibility.Collapsed;
                    BtnLogin.Visibility = Visibility.Visible;
                }
            }, CancellationToken.None, TaskCreationOptions.None, _syncContextTaskScheduler);

        }

        /// <summary>
        /// 初始化当前已登录账户信息
        /// </summary>
        /// <param name="isSuccess"></param>
        private void InitCurrentAccountInfo(bool isSuccess)
        {
            // 登录成功了 保存登录信息
            if (isSuccess)
            {
                SaveCurrentAccountInfo();

                Console.WriteLine("登录成功，已成功初始化账户信息");
            }
        }

        /// <summary>
        /// 保存当前已登录账户信息
        /// </summary>
        private void SaveCurrentAccountInfo()
        {
            Task.Factory.StartNew(() =>
            {
                var userName = CmbUserName.Text;
                var password = Password.Password;

                //2.保存下拉列表中的用户，如果下拉列表中包含此次登录的用户，跳过
                if (!_accountsDic.ContainsKey(userName))
                {
                    CmbUserName.Items.Add(userName);
                    if (CmbUserName.Items.Count > _maxAccountCount)
                        CmbUserName.Items.RemoveAt(0);
                }
                _accountsDic[userName] = (bool)IsRememberPassword.IsChecked ? password : _emptyPassword;
                SaveAccountsInfo();
            }, CancellationToken.None, TaskCreationOptions.None, _syncContextTaskScheduler);

        }

        /// <summary>
        /// 验证用户名是否有效
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("请输入用户名！");
                return false;
            }
            return true;
        }

        #endregion

    }
}
