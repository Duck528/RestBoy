using Microsoft.Win32;
using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace RestBoy.Control
{
    /// <summary>
    /// BodyControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BodyControl : UserControl
    {
        private BodyModel body = null;
        public BodyModel Body
        {
            get { return this.body; }
        }
        public BodyControl(BodyModel bodyModel)
        {
            InitializeComponent();
            this.body = bodyModel;
            if (body.IsChecked == true)
                this.chxCheck.IsChecked = true;

            this.cbxType.Items.Add("Text");
            this.cbxType.Items.Add("File");
        }
        private void tbxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            string text = textbox.Text.Trim();
            textbox.Text = text;
            this.Body.Value = text;
        }
        private void tbxKey_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            string text = textbox.Text.Trim();
            textbox.Text = text;
            this.Body.Key = text;
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == true)
            {
                string filePath = openFileDlg.FileName;
                this.Body.DisplayFileName = Path.GetFileName(filePath);
                this.Body.FilePath = filePath;
                this.tbkFileName.Text = this.Body.DisplayFileName;
            }
        }
        private void chxCheck_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            bool isChecked = checkbox.IsChecked.Value;
            this.Body.IsChecked = isChecked;
        }

        private void cbxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            if (cbx == null)
                return;

            string type = cbx.SelectedValue.ToString();
            this.body.ValueType = type;
            switch (type)
            {
                case "File":
                    this.tbxValue.Visibility = Visibility.Collapsed;
                    this.spOpenFile.Visibility = Visibility.Visible;
                    break;

                case "Text":
                    this.spOpenFile.Visibility = Visibility.Collapsed;
                    this.tbxValue.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
