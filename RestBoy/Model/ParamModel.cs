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
    public class ParamModel : ObservableObject
    {
        #region Parent
        public ObservableCollection<ParamModel> Parent { get; set; }
        #endregion

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
        public ParamModel(int order, ObservableCollection<ParamModel> parent)
        {
            this.Order = order;
            this.Parent = parent;
        }
        #endregion

        #region DeleteCommand
        private ICommand deleteParamCommand = null;
        public ICommand DeleteParamCommand
        {
            get
            {
                return (this.deleteParamCommand) ??
                  (this.deleteParamCommand = new DelegateCommand(DeleteParam));
            }
        }
        private void DeleteParam()
        {
            this.Parent.Remove(this);
        }
        #endregion
    }
}
