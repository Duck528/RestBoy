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
    /// ParamControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ParamControl : UserControl
    {
        private ParamModel paramModel = null;
        public ParamModel ParamModel
        {
            get { return this.paramModel; }
        }
        public ParamControl(ParamModel paramModel)
        {
            InitializeComponent();
            this.paramModel = paramModel;
        }
        public ParamControl()
        {
            InitializeComponent();
        }
    }
}
