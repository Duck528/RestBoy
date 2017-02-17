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
        public JsonControl()
        {
            InitializeComponent();
        }

        private void btnDelJson_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var model = button.CommandParameter as JsonModel;
            if (model == null)
            {
                return;
            }

            // 너비우선 탐색 방식으로 삭제할 컨트롤을 찾아 삭제한다
            var models = this.tvJson.ItemsSource as ObservableCollection<JsonModel>;
            if (models == null)
            {
                return;
            }

            var queue = new Queue<ObservableCollection<JsonModel>>();
            queue.Enqueue(models[0].Childs);
            while (queue.Count() != 0)
            {
                ObservableCollection<JsonModel> temp = queue.Dequeue();
                var finded = temp.Where(t =>
                {
                    if (t.Id == model.Id)
                        return true;
                    else
                        return false;
                }).FirstOrDefault();

                if (finded != null)
                {
                    bool isDeleted = temp.Remove(finded);
                    if (isDeleted == true)
                    {
                        break;
                    }
                }

                foreach (var jsonModel in temp)
                {
                    queue.Enqueue(jsonModel.Childs);
                }
            }
        }

        private void btnAddJson_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var model = button.CommandParameter as JsonModel;
            if (model == null)
                return;

            if (model.SelectedJsonType == JType.Array)
            {
                model.Childs.Add(new JsonModel(model)
                {
                    HasKey = false,
                    SelectedJsonType = JType.String,
                    ValueBorderThickness = new Thickness(0, 0, 0, 1),
                    KeyBorderThickness = new Thickness(0, 0, 0, 1)
                });
            }
            else
            {
                model.Childs.Add(new JsonModel(model)
                {
                    HasKey = true,
                    SelectedJsonType = JType.String,
                    ValueBorderThickness = new Thickness(0, 0, 0, 1),
                    KeyBorderThickness = new Thickness(0, 0, 0, 1)
                });
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
