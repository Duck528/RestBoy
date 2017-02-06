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
    public class BodyModel : ObservableObject
    {
        public ObservableCollection<BodyModel> Parent { get; set; }
        public BodyModel(ObservableCollection<BodyModel> parent)
        {
            this.Parent = parent;
        }

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
        private string valueType = string.Empty;
        public string ValueType
        {
            get { return this.valueType; }
            set
            {
                if (this.valueType.Equals(value) == false)
                {
                    this.valueType = value;
                    this.RaisePropertyChanged("ValueType");
                }
            }
        }
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

        private string displayFileName = "선택된 파일이 없습니다";
        public string DisplayFileName
        {
            get { return this.displayFileName; }
            set
            {
                if (this.displayFileName.Equals(value) == false)
                {
                    this.displayFileName = value;
                    this.RaisePropertyChanged("DisplayFileName");
                }
            }
        }

        private string filePath = string.Empty;
        public string FilePath
        {
            get { return this.filePath; }
            set
            {
                if (this.filePath.Equals(value) == false)
                {
                    this.filePath = value;
                    this.RaisePropertyChanged("FilePath");
                }
            }
        }

        #region DeleteCommand
        private ICommand deleteBodyCommand = null;
        public ICommand DeleteBodyCommand
        {
            get
            {
                return this.deleteBodyCommand ??
                  (this.deleteBodyCommand = new DelegateCommand(DeleteBody));
            }
        }
        private void DeleteBody()
        {
            this.Parent.Remove(this);
        }
        #endregion
    }
}
