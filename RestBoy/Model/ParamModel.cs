using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Model
{
    public class ParamModel : ObservableObject
    {
        #region Key
        private string key = string.Empty;
        public string Key
        {
            get { return this.key; }
            set
            {
                if (this.key != value)
                {
                    this.key = value;
                    this.RaisePropertyChanged("Key");
                }
            }
        }
        #endregion

        #region Value
        private string value = string.Empty;
        public string Value
        {
            get { return this.value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        #endregion

        #region Order
        private int order = -1;
        public int Order
        {
            get { return this.order; }
            set
            {
                if (this.order != value)
                {
                    this.order = value;
                    this.RaisePropertyChanged("Order");
                }
            }
        }
        #endregion

        #region Constructor
        public ParamModel(int order)
        {
            this.Order = order;
        }
        #endregion
    }
}
