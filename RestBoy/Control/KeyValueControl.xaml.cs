using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace RestBoy.Control
{
    /// <summary>
    /// KeyValueControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyValueControl : UserControl
    {
        private HeaderModel header = null;
        public HeaderModel Header
        {
            get { return this.header; }
            private set { this.header = value; }
        }

        public KeyValueControl(HeaderModel header)
        {
            InitializeComponent();

            this.Header = header;
            if (this.Header.IsChecked == true)
                this.chxCheck.IsChecked = true;
        }

        private void chxCheck_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            bool isChecked = checkbox.IsChecked.Value;
            this.header.IsChecked = isChecked;
        }

        private void tbxKey_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            string text = textbox.Text.Trim();
            textbox.Text = text;
            this.header.Key = text;
        }

        private void tbxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            string text = textbox.Text.Trim();
            textbox.Text = text;
            this.header.Value = text;
        }
    }
}
