using GalaSoft.MvvmLight;
using Mvvm.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestBoy.Model
{
    public class HeaderModel : ObservableObject
    {
        public ObservableCollection<HeaderModel> Parent { get; set; }

        #region Constructor
        public HeaderModel(ObservableCollection<HeaderModel> parent)
        {
            this.Parent = parent;
        }
        #endregion

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

        #region DeleteCommand
        private ICommand delKeyValCommand = null;
        public ICommand DelKeyValCommand
        {
            get
            {
                return this.delKeyValCommand ??
                (this.delKeyValCommand = new DelegateCommand(DelKeyVal));
            }
        }
        private void DelKeyVal()
        {
            this.Parent.Remove(this);
        }
        #endregion
    }
}
