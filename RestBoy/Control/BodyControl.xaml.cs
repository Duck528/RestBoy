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
        public BodyControl()
        {
            InitializeComponent();
            this.cbxType.Items.Add("Text");
            this.cbxType.Items.Add("File");
        }
       
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == true)
            {
                var body = this.DataContext as BodyModel;
                string filePath = openFileDlg.FileName;
                body.DisplayFileName = Path.GetFileName(filePath);
                body.FilePath = filePath;
            }
        }

        private void cbxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            if (cbx == null)
                return;

            string type = cbx.SelectedValue.ToString();
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
