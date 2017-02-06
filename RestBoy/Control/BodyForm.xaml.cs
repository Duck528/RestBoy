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
    /// BodyForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BodyForm : UserControl
    {
        private string checkedBodyType = string.Empty;
        public string CheckedBodyType
        {
            get { return this.checkedBodyType; }
            private set
            {
                if (this.checkedBodyType.Equals(value) == false)
                {
                    this.checkedBodyType = value;
                }
            }
        }

        public BodyForm()
        {
            InitializeComponent();
        }

        private void Checked_BodyType(object sender, RoutedEventArgs e)
        {
            var btnRadio = sender as RadioButton;
            if (btnRadio == null)
                return;

            string type = btnRadio.Content.ToString();
            this.checkedBodyType = type;
        }
    }
}
