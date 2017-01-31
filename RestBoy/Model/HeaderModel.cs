using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBoy.Model
{
    public class HeaderModel : ObservableObject
    {
        #region IsChecked
        private bool isChecked = false;
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.RaisePropertyChanged("IsChecked");
                }
            }
        }
        #endregion

        #region Key
        private string key = string.Empty;
        public string Key
        {
            get { return this.key; }
            set
            {
                if (this.key.Equals(value) == false)
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
                if (this.value.Equals(value) == false)
                {
                    this.value = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        #endregion
    }
}
