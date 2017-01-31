using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Model
{
    class LogModel : ObservableObject
    {
        private string method = string.Empty;
        private string uri = string.Empty;
        private string date = string.Empty;

        public LogModel(string method, string uri, string date)
        {
            this.method = method;
            this.uri = uri;
            this.date = date;
        }

        public LogModel() { }

        public string Method
        {
            get
            {
                return method;
            }

            set
            {
                if (this.method != value)
                {
                    this.method = value;
                    this.RaisePropertyChanged("Method");
                }
                
            }
        }

        public string Uri
        {
            get
            {
                return uri;
            }

            set
            {
                if (this.uri != value)
                {
                    this.uri = value;
                    this.RaisePropertyChanged("Uri");
                }
                
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                if (this.date != value)
                {
                    this.date = value;
                    this.RaisePropertyChanged("Date");
                }
                
            }
        }
    }
}
