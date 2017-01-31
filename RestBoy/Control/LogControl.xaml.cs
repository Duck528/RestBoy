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
    /// LogControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogControl : UserControl
    {
        private string date = string.Empty;
        private string method = string.Empty;
        private string uri = string.Empty;
        public LogControl(string date, string method, string uri)
        {
            InitializeComponent();
        }

        private void InitializeSetting(string date, string method, string uri)
        {
            this.tbkDate.Text = date;
            this.tbkMethod.Text = method;
            this.tbkUri.Text = uri;

            switch (this.method)
            {
                case "GET":
                    this.tbkMethod.Foreground = new SolidColorBrush(Colors.Green);
                    break;

                case "POST":
                    this.tbkMethod.Foreground = new SolidColorBrush(Colors.Blue);
                    break;

                default:
                    this.tbkMethod.Foreground = new SolidColorBrush(Colors.Red);
                    break;
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }

        public string Uri
        {
            get
            {
                return uri;
            }

            set
            {
                uri = value;
            }
        }
    }
}
