﻿using RestBoy.ViewModel;
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

namespace RestBoy.View
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel mainViewModel = new MainViewModel();
        public static readonly DependencyProperty IsModifiedProperty =
            DependencyProperty.Register("IsModified", typeof(bool), typeof(MainView), new PropertyMetadata());
        public bool IsModified
        {
            get { return (bool)GetValue(IsModifiedProperty); }
            set { throw new Exception("Readonly property"); }
        }

        public MainView()
        {
            InitializeComponent();
            this.DataContext = this.mainViewModel;
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

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.wrapTbxSeaerchLog.Background = Brushes.White;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.wrapTbxSeaerchLog.Background = Brushes.LightGray;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var window = sender as Window;
            if (window == null)
                return;

            if (window.ActualWidth < 800)
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

