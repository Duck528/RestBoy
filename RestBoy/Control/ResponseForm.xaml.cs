using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// ResponseForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResponseForm : UserControl
    {
        public ResponseForm()
        {
            InitializeComponent();

            this.btnBody.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        public void ClickedSettingButton(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            this.ClearSettingButton();

            switch (btn.Name)
            {
                case "btnBody":
                    this.tbkBody.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.btnBody.Foreground = Brushes.Black;
                    break;

                case "btnCookies":
                    this.tbkCookies.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.btnCookies.Foreground = Brushes.Black;
                    break;

                case "btnHeaders":
                    this.tbkHeaders.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.btnHeaders.Foreground = Brushes.Black;
                    break;
            }
        }

        private void ClearSettingButton()
        {
            this.tbkBody.TextDecorations = null;
            this.tbkBody.Foreground = Brushes.Gray;

            this.tbkHeaders.TextDecorations = null;
            this.tbkHeaders.Foreground = Brushes.Gray;

            this.tbkCookies.TextDecorations = null;
            this.tbkCookies.Foreground = Brushes.Gray;
        }

        private TextDecorationCollection GetUnderline(SolidColorBrush color, int thickness)
        {
            var collection = new TextDecorationCollection();
            var strike = new TextDecoration();
            strike.Location = TextDecorationLocation.Underline;

            strike.Pen = new Pen(color, thickness);
            strike.PenThicknessUnit = TextDecorationUnit.FontRecommended;

            collection.Add(strike);
            return collection;
        }

        private void btnBody_Click(object sender, RoutedEventArgs e)
        {
            this.ClickedSettingButton(sender, e);
            this.tbkBody.Foreground = Brushes.Black;

            this.respHeader.Visibility = Visibility.Collapsed;
            this.respBody.Visibility = Visibility.Visible;
        }

        private void btnCookies_Click(object sender, RoutedEventArgs e)
        {
            this.ClickedSettingButton(sender, e);
            this.tbkCookies.Foreground = Brushes.Black;

            this.respHeader.Visibility = Visibility.Collapsed;
            this.respBody.Visibility = Visibility.Collapsed;
        }

        private void btnHeaders_Click(object sender, RoutedEventArgs e)
        {
            this.ClickedSettingButton(sender, e);
            this.tbkHeaders.Foreground = Brushes.Black;

            this.respHeader.Visibility = Visibility.Visible;
            this.respBody.Visibility = Visibility.Collapsed;
        }
    }
}
