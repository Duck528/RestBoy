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
    /// RespBodyForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RespBodyForm : UserControl
    {
        public RespBodyForm()
        {
            InitializeComponent();
        }

        private void btnRaw_Click(object sender, RoutedEventArgs e)
        {
            this.wbPreview.Visibility = Visibility.Collapsed;
            this.tbxRaw.Visibility = Visibility.Visible;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            this.tbxRaw.Visibility = Visibility.Collapsed;
            this.wbPreview.Visibility = Visibility.Visible;
        }
    }
}
