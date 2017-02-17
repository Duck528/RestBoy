using RestBoy.ViewModel;
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
using System.Globalization;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

// Branch Push Test
namespace RestBoy.View
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        private ObservableCollection<MainViewModel> mainViewModels 
            = new ObservableCollection<MainViewModel>();
        public ObservableCollection<MainViewModel> MainViewModels
        {
            get { return this.mainViewModels; }
        }
        private MainViewModel selectedViewModel = null;
        public MainViewModel SelectedViewModel
        {
            get { return this.selectedViewModel; }
            set
            {
                if (this.selectedViewModel != value)
                {
                    MainViewModel beforeMV = this.DataContext as MainViewModel;
                    if (beforeMV == null)
                    {
                        MessageBox.Show("Internal program error");
                        return;
                    }
                    MainViewModel nextMV = value.DeepCopy();
                    this.DataContext = nextMV;

                    nextMV.RdoFormData = beforeMV.RdoFormData;
                    nextMV.RdoAppJson = beforeMV.RdoAppJson;
                    nextMV.RdoRaw = beforeMV.RdoRaw;
                    nextMV.DisplayAuthForm = beforeMV.DisplayAuthForm;
                    nextMV.DisplayBodyForm = beforeMV.DisplayBodyForm;
                    nextMV.DisplayHeaderForm = beforeMV.DisplayHeaderForm;
                    nextMV.ParamDisplay = !beforeMV.ParamDisplay;

                    this.selectedViewModel = value;
                }
            }
        }

        public MainView()
        {
            InitializeComponent();
            var mainViewModel = new MainViewModel(mainViewModels);

            this.DataContext = mainViewModel;
            this.lbxHistories.DataContext = this;
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

        private void ClearSettingButton()
        {
            this.tbkAuth.TextDecorations = null;
            this.tbkAuth.Foreground = Brushes.Gray;

            this.tbkHeaders.TextDecorations = null;
            this.tbkHeaders.Foreground = Brushes.Gray;

            this.tbkBody.TextDecorations = null;
            this.tbkBody.Foreground = Brushes.Gray;
        }

        public void ClickedSettingButton(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            this.ClearSettingButton();

            switch (btn.Name)
            {
                case "btnAuth":
                    this.tbkAuth.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.tbkAuth.Foreground = Brushes.Black;
                    break;

                case "btnHeaders":
                    this.tbkHeaders.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.tbkHeaders.Foreground = Brushes.Black;
                    break;

                case "btnBody":
                    this.tbkBody.TextDecorations = this.GetUnderline(Brushes.Orange, 2);
                    this.tbkBody.Foreground = Brushes.Black;
                    break;
            }
        }

        private void tbxUri_GotFocus(object sender, RoutedEventArgs e)
        {
            this.tbxUri.Background = Brushes.White;
        }

        private void tbxUri_LostFocus(object sender, RoutedEventArgs e)
        {
            this.tbxUri.Background = Brushes.LightGray;
        }

        private void btnDisplayParams_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnDisplayParams.Background == Brushes.White)
                this.btnDisplayParams.Background = Brushes.LightGray;
            else
                this.btnDisplayParams.Background = Brushes.White;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var window = sender as Window;
            if (window == null)
                return;

            if (window.ActualWidth < 1100)
                this.gdAside.Visibility = Visibility.Collapsed;
            else
                this.gdAside.Visibility = Visibility.Visible;
        }

        private void svContents_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var svContents = sender as ScrollViewer;
            if (svContents == null)
                return;

            svContents.ScrollToVerticalOffset(svContents.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}

