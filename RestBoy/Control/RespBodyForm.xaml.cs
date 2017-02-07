using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
    /// 아래 사이트를 참고하여 웹 브라우저 스크립트 경고를 막을 수 있었다
    /// http://stackoverflow.com/questions/6138199/wpf-webbrowser-control-how-to-supress-script-errors
    /// </summary>
    [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleServiceProvider
    {
        [PreserveSig]
        int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
    }
    /// <summary>
    /// RespBodyForm.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RespBodyForm : UserControl
    {
        public RespBodyForm()
        {
            InitializeComponent();
            this.wbPreview.Navigated += new NavigatedEventHandler(wbMain_Navigated);
        }

        void wbMain_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(this.wbPreview, true); // make it silent
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

        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }
    }
}
