using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.Login
{
    public static class ControlHelper
    {
        private static readonly ResourceDictionary _resourceDic = new ResourceDictionary()
        {
            Source = new Uri(@"pack://application:,,,/WPF.Login;component/Resources/CommonDic.xaml")
        };

        /// <summary>
        /// 设置控件的Placeholder呈现效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SetPlaceholder(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                var box = sender as TextBox;
                if (!string.IsNullOrEmpty(box.Text))
                    box.Background = Brushes.Transparent;
                else
                    box.Background = _resourceDic["OrigpasswordBurshHelper"] as VisualBrush;
            }
            else if (sender is PasswordBox)
            {
                var box = sender as PasswordBox;
                if (!string.IsNullOrEmpty(box.Password))
                    box.Background = Brushes.Transparent;
                else
                    box.Background = _resourceDic["OrigpasswordBurshHelper"] as VisualBrush;
            }
        }

        /// 获得指定元素的父元素  
        /// <summary>  
        /// <typeparam name="T">指定页面元素</typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>  
        /// 获得指定元素的所有子元素  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        /// <summary>  
        /// 查找子元素  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="obj"></param>  
        /// <param name="name"></param>  
        /// <returns></returns>  
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;


            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);


                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
    }
}
