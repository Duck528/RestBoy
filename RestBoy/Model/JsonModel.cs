using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RestBoy.Model
{
    public enum JType { Object, Array, File, Number, String, Boolean };
    public class JsonModel : ObservableObject
    {
        /// <summary>
        /// DisplayAdd는 SelelctedJsonType이 Object, Array인 경우에 true로 셋팅된다
        /// </summary>
        private bool displayAdd = false;
        public bool DisplayAdd
        {
            get { return this.displayAdd; }
            set
            {
                if (this.displayAdd != value)
                {
                    this.displayAdd = value;
                    this.RaisePropertyChanged("DisplayAdd");
                }
            }
        }

        /// <summary>
        /// DisplayFile는 SelectedJsonType이 File인 경우에 true로 셋팅된다
        /// </summary>
        private bool displayFile = false;
        public bool DisplayFile
        {
            get { return this.displayFile; }
            set
            {
                if (this.displayFile != value)
                {
                    this.displayFile = value;
                    this.RaisePropertyChanged("DisplayFile");
                }
            }
        }

        private bool displayFileName = false;
        public bool DisplayFileName
        {
            get { return this.displayFile; }
            set
            {
                if (this.displayFile != value)
                {
                    this.displayFileName = value;
                    this.RaisePropertyChanged("DisplayFileName");
                }
            }
        }
        private void ShutOffDisplay()
        {
            this.DisplayAdd = false;
            this.HasTextBlockValue = false;
            this.HasTextBoxValue = false;
            this.DisplayFile = false;
        }

        public IEnumerable<JType> JsonPropTypes
        {
            get
            {
                if (this.HasKey == true)
                    return Enum.GetValues(typeof(JType)).Cast<JType>();
                else
                    return Enum.GetValues(typeof(JType)).Cast<JType>()
                        .Where(v => v != JType.File).ToList();
            }
        }

        private JType selectedJsonType = JType.String;
        public JType SelectedJsonType
        {
            get { return this.selectedJsonType; }
            set
            {
                if (this.selectedJsonType != value)
                {
                    this.selectedJsonType = value;
                    this.RaisePropertyChanged("SelectedJsonType");
                    this.ShutOffDisplay();
                    this.Childs.Clear();
                    this.Value = "";
                    switch (this.selectedJsonType)
                    {
                        case JType.Object:
                            this.DisplayAdd = true;
                            this.ReadOnlyValue = true;
                            this.ValueBorderThickness = new Thickness(0);
                            this.Value = "{ " + this.Childs.Count() + " }";
                            this.HasTextBoxValue = true;
                            break;

                        case JType.Array:
                            this.DisplayAdd = true;
                            this.ReadOnlyValue = true;
                            this.ValueBorderThickness = new Thickness(0);
                            this.Value = "[ " + this.Childs.Count() + " ]";
                            this.HasTextBoxValue = true;
                            break;

                        case JType.File:
                            this.ShutOffValue = true;
                            this.DisplayFile = true;
                            this.Value = "선택된 파일이 없습니다";
                            this.HasTextBlockValue = true;
                            break;

                        default:
                            this.ReadOnlyValue = false;
                            this.ValueBorderThickness = new Thickness(0, 0, 0, 1);
                            this.HasTextBoxValue = true;
                            break;
                    }
                }
            }
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

        private string value = null;
        public string Value    
        {
            get { return this.value; ; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }

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

        private ObservableCollection<JsonModel> childs = null;
        public ObservableCollection<JsonModel> Childs
        {
            get { return this.childs; }
            private set { this.childs = value; }
        }

        /// <summary>
        /// Parent - 부모 계층을 가리킨다. 만약 없다면 null을 반환한다
        /// </summary>
        private JsonModel parent = null;
        public JsonModel Parent
        {
            get { return this.parent; }
            private set { this.parent = value; }
        }

        #region HasKeyOption
        private bool hasKey = false;
        public bool HasKey
        {
            get { return this.hasKey; }
            set
            {
                if (this.hasKey != value)
                {
                    this.hasKey = value;
                    this.RaisePropertyChanged("HasKey");
                }
            }
        }
        #endregion

        #region HasTextBoxValueOption
        private bool hasTextBoxValue = true;
        public bool HasTextBoxValue
        {
            get { return this.hasTextBoxValue; }
            set
            {
                if (this.hasTextBoxValue != value)
                {
                    this.hasTextBoxValue = value;
                    this.RaisePropertyChanged("HasTextBoxValue");
                }
            }
        }
        #endregion

        #region HasTextBlockValueOption
        private bool hasTextBlockValue = false;
        public bool HasTextBlockValue
        {
            get { return this.hasTextBlockValue; }
            set
            {
                if (this.hasTextBlockValue != value)
                {
                    this.hasTextBlockValue = value;
                    this.RaisePropertyChanged("HasTextBlockValue");
                }
            }
        }
        #endregion

        #region ReadOnlyValueOption
        private bool readonlyValue = false;
        public bool ReadOnlyValue
        {
            get { return this.readonlyValue; }
            set
            {
                if (this.readonlyValue != value)
                {
                    this.readonlyValue = value;
                    this.RaisePropertyChanged("ReadOnlyValue");
                }
            }
        }
        #endregion

        #region ValueBorderThicknessOption
        private Thickness valueBorderThickness = new Thickness(1);
        public Thickness ValueBorderThickness
        {
            get { return this.valueBorderThickness; }
            set
            {
                if (this.valueBorderThickness != value)
                {
                    this.valueBorderThickness = value;
                    this.RaisePropertyChanged("ValueBorderThickness");
                }
            }
        }
        #endregion

        #region KeyBorderThicknessOption
        private Thickness keyBorderThickness = new Thickness(1);
        public Thickness KeyBorderThickness
        {
            get { return this.keyBorderThickness; }
            set
            {
                if (this.keyBorderThickness != value)
                {
                    this.keyBorderThickness = value;
                    this.RaisePropertyChanged("ValueBorderThickness");
                }
            }
        }
        #endregion

        private bool shutOffValue = false;
        public bool ShutOffValue
        {
            get { return this.shutOffValue; }
            set
            {
                if (this.shutOffValue != value)
                {
                    this.shutOffValue = value;
                    this.RaisePropertyChanged("ShutOffValue");
                }
            }
        }
        private bool shutOffDelButton = false;
        public bool ShutOffDelButton
        {
            get { return this.shutOffDelButton; }
            set
            {
                if (this.shutOffDelButton != value)
                {
                    this.shutOffDelButton = value;
                    this.RaisePropertyChanged("ShutOffDelButton");
                }
            }
        }

        public string ToJson()
        {
            var builder = new StringBuilder();
            // if (this.Parent != null || "".Equals(this.Key.Trim()))
            if (this.HasKey == true)
                builder.Append("\"").Append(this.Key).Append("\":");

            switch (this.SelectedJsonType)
            {
                case JType.Array:
                    {
                        builder.Append("[");
                        int nRest = this.Childs.Count() - 1;
                        foreach (var model in this.Childs)
                        {
                            builder.Append(model.ToJson());
                            if (nRest > 0)
                            {
                                builder.Append(",");
                                nRest -= 1;
                            }
                        }
                        builder.Append("]");
                    }
                    break;

                case JType.File:
                    {
                        builder.Append("\"").Append(Path.GetFileName(this.Value)).Append("\"").Append(",");
                        break;
                    }

                case JType.Object:
                    {
                        builder.Append("{");
                        foreach (var model in this.Childs)
                        {
                            builder.Append(model.ToJson());
                        }
                        builder.Append("}").Append(",");
                        break;
                    }

                case JType.String:
                    {
                        builder.Append("\"").Append(this.Value).Append("\"").Append(",");
                        break;
                    }

                case JType.Number:
                    {
                        builder.Append(this.Value).Append(",");
                        break;
                    }
                case JType.Boolean:
                    {
                        builder.Append(this.Value).Append(",");
                        break;
                    }
            }

            return builder.ToString();
        }

        #region Constructor
        public JsonModel(JsonModel parent)
        {
            this.Childs = new ObservableCollection<JsonModel>();
            this.Childs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
                            e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        switch (this.SelectedJsonType)
                        {
                            case JType.Array:
                                this.Value = "[ " + this.Childs.Count() + " ]";
                                break;

                            case JType.Object:
                                this.Value = "{ " + this.Childs.Count() + " }";
                                break;
                        }
                    }
                });
            this.Parent = parent;
        }
        #endregion
    }
}
