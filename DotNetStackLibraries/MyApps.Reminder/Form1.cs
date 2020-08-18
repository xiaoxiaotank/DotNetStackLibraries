using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MyApps.Reminder
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// when closing the session
        /// </summary>
        public const int WM_QUERYENDSESSION = 0x0011;
        /// <summary>
        /// when shutting down the machine
        /// </summary>
        public const int WM_ENDSESSION = 0x0016;
        public const uint SHUTDOWN_NORETRY = 0x00000001;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string reason);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);
        [DllImport("kernel32.dll")]
        static extern bool SetProcessShutdownParameters(uint dwLevel, uint dwFlags);

        private readonly Timer _timer;
        private readonly Dictionary<TimeSpan, bool> _timeSpanDic = new Dictionary<TimeSpan, bool>()
        {
            //[TimeSpan.FromHours(9)] = true,
            //[TimeSpan.FromHours(10)] = true,
            [TimeSpan.FromHours(18.5)] = true, 
            [TimeSpan.FromHours(21)] = true, 
            [TimeSpan.FromHours(21.5)] = true 
        };
        private readonly string tip;

        public Form1()
        {
            InitializeComponent();
            // Define the priority of the application (0x3FF = The higher priority)
            SetProcessShutdownParameters(0x3FF, SHUTDOWN_NORETRY);

            var nowTs = DateTime.Now.TimeOfDay;
            var keys = _timeSpanDic.Keys.Where(k => k <= nowTs).ToList();
            foreach (var key in keys)
            {
                _timeSpanDic[key] = false;
            }

            var tipSb = new StringBuilder(10);
            for (int i = 0; i < 10; i++)
            {
                tipSb.AppendLine("打卡！！！！！！！！！！！！！！！！！！！！！！！");
            }
            tip = tipSb.ToString();
            _timer = new Timer()
            {
                Interval = (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
            };
            _timer.Tick += Timer_Tick;
            Register();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _timer.Start();
            WindowState = FormWindowState.Minimized;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip((int)TimeSpan.FromSeconds(10).TotalMilliseconds, "打卡提醒程序", "打卡提醒程序已启动", ToolTipIcon.Info);
            }
        }

        private static void Register()
        {
            try
            {
                using (var local = Registry.CurrentUser)
                using (var key = local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    // 添加注册表
                    key.SetValue("JJJ_Reminder", Application.ExecutablePath);
                    // 删除注册表
                    //key.DeleteValue("JJJ_Reminder", false);
                    key.Close();
                    local.Close();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入注册表失败");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var nowTs = DateTime.Now.TimeOfDay;
            foreach (var ts in _timeSpanDic.Keys.OrderBy(k => k))
            {
                if(nowTs >= ts && _timeSpanDic[ts])
                {
                    if(MessageBox.Show(tip, "打卡提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                    {
                        _timeSpanDic[ts] = false;
                        break;
                    }
                }
            }
        }
   
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //switch (e.CloseReason)
            //{
            //    case CloseReason.ApplicationExitCall:
            //        break;
            //    case CloseReason.FormOwnerClosing:
            //        break;
            //    case CloseReason.MdiFormClosing:
            //        break;
            //    case CloseReason.None:
            //        break;
            //    case CloseReason.TaskManagerClosing:
            //        break;
            //    // 注销或关机会触发此事件
            //    case CloseReason.UserClosing:
            //        break;
            //    case CloseReason.WindowsShutDown:
            //        break;
            //    default:
            //        break;
            //}

            MessageBox.Show("不允许关闭！");
            e.Cancel = true;
            base.OnFormClosing(e);
        }

    }
}
