using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RestBoy.Util
{
    class WebBrowserHelper
    {
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.RegisterAttached(
                "Body",
                typeof(string),
                typeof(WebBrowserHelper), new PropertyMetadata(OnBodyChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(BodyProperty, body);
        }

        private static void OnBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrower = d as WebBrowser;
            if (webBrower != null)
            {
                string doc = Convert.ToString(e.NewValue).Trim();
                if ("".Equals(doc) == false)
                    webBrower.NavigateToString(e.NewValue as string ?? "&nbsp;");
            }
        }
    }
}
