using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF.Login
{
    /// <summary>
    /// 带点位符的文本输入控件
    /// </summary>
    public class PlaceholderComboBox : ComboBox
    {
        #region Fields

        /// <summary>
        /// 占位符的文本框
        /// </summary>
        private readonly ComboBox _placeholderComboBox = new ComboBox();

        /// <summary>
        /// 占位符的画刷
        /// </summary>
        private readonly VisualBrush _placeholderVisualBrush;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 占位符的依赖属性
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            "Placeholder", typeof(string), typeof(PlaceholderComboBox),
            new FrameworkPropertyMetadata("请在此输入", FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 最大长度
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached(
            "MaxLength", typeof(int), typeof(PlaceholderComboBox), 
            new UIPropertyMetadata(OnMaxLengthChanged));

        /// <summary>
        /// 占位符
        /// </summary>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public string MaxLength
        {
            get { return (string)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        #endregion Properties       

        public PlaceholderComboBox()
        {
            _placeholderVisualBrush = new VisualBrush()
            {
                AlignmentX = AlignmentX.Left,
                Stretch = Stretch.None,
                Opacity = 0.3,
                Visual = GetPlaceholder(),
                TileMode = TileMode.None
            };

            Background = _placeholderVisualBrush;
            AddHandler(TextBoxBase.TextChangedEvent, new RoutedEventHandler(PlaceholderComboBox_TextChanged));                       
        }

        private TextBlock GetPlaceholder()
        {
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Placeholder")
            };
            var placeholder = new TextBlock()
            {
                FontStyle = FontStyles.Italic,
                FontSize = 12
            };
            placeholder.SetBinding(TextBlock.TextProperty, binding);
            return placeholder;
        }

        #region Events Handling

        /// <summary>
        /// 文本变化的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaceholderComboBox_TextChanged(object sender, RoutedEventArgs e)
        {
            Background = string.IsNullOrEmpty(Text) ? _placeholderVisualBrush : null;
        }


        private static void OnMaxLengthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var comboBox = obj as ComboBox;
            if (comboBox == null) return;

            comboBox.Loaded += (s, e) =>
            {
                var textBox = FindChild(comboBox, "PART_EditableTextBox",typeof(TextBox));
                if (textBox == null) return;
                textBox.SetValue(TextBox.MaxLengthProperty, args.NewValue);
            };            
        }
        #endregion Events Handling


        public static DependencyObject FindChild(DependencyObject reference, string childName, Type childType)
        {
            DependencyObject foundChild = null;
            if (reference != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    // If the child is not of the request child type child
                    if (child.GetType() != childType)
                    {
                        // recursively drill down the tree
                        foundChild = FindChild(child, childName, childType);
                        if (foundChild != null)
                            break;
                    }
                    else if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = child;
                        break;
                    }
                }
            }
            return foundChild;
        }
    }
}
