using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Model
{
    class HttpRespModel : ObservableObject
    {
        #region Headers
        private ObservableCollection<HeaderModel> headers = null;
        public ObservableCollection<HeaderModel> Headers
        {
            get
            {
                return this.headers ??
                    (this.headers = new ObservableCollection<HeaderModel>());
            }
            private set { this.headers = value; }
        }
        #endregion

        #region RespText
        private string respText = string.Empty;
        public string RespText
        {
            get { return this.respText; }
            set
            {
                this.respText = value;
                this.RaisePropertyChanged("RespText");
            }
        }
        #endregion

        #region Cookies
        private ObservableCollection<HeaderModel> cookies = null;
        public ObservableCollection<HeaderModel> Cookies
        {
            get
            {
                return this.cookies ?? 
                    (this.cookies = new ObservableCollection<HeaderModel>());
            }
            private set { this.cookies = value; }
        }
        #endregion
    }
}
