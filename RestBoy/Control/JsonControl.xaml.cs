using Microsoft.Win32;
using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// JsonControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class JsonControl : UserControl
    {
        private ObservableCollection<JsonModel> jsonModels = null;
        public ObservableCollection<JsonModel> JsonModels
        {
            get
            {
                return this.jsonModels ?? (this.jsonModels = new ObservableCollection<JsonModel>());
            }
        }
        public JsonControl()
        {
            InitializeComponent();
        }

        private void btnDelJson_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var model = button.CommandParameter as JsonModel;
            var parentModel = model.Parent;
            parentModel.Childs.Remove(model);
        }

        private void btnAddJson_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var model = button.CommandParameter as JsonModel;
            if (model.SelectedJsonType == JType.Array)
            {
                var child = new JsonModel(model, true)
                {
                    DisplayArray = true,
                    SelectedJsonType = JType.Value,
                    ShutOffDelButton = false
                };
                model.Childs.Add(child);
            }
            else
            {
                var child = new JsonModel(model, false)
                {
                    SelectedJsonType = JType.Value
                };
                model.Childs.Add(child);
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var model = button.CommandParameter as JsonModel;
            if (model == null)
                return;

            var openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == true)
            {
                model.Value = openFileDlg.FileName;
            }
        }
    }
}
