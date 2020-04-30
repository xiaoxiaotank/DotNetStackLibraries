using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.MVVM.Login
{
    class ControlAttachProperty
    {
        private static readonly Type _ownerType = typeof(ControlAttachProperty);

        #region Placeholder 占位符
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty
            .RegisterAttached("Placeholder", typeof(string), _ownerType, new FrameworkPropertyMetadata(string.Empty));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }
        #endregion
    }
}
