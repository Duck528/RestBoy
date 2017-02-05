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

namespace RestBoy.Model
{
    public enum JType { Object, Array, File, Number, String };
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
        private bool displayArray = false;
        public bool DisplayArray
        {
            get { return this.displayArray; }
            set
            {
                if (this.displayArray != value)
                {
                    this.displayArray = value;
                    this.RaisePropertyChanged("DisplayArray");
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
            this.DisplayFile = false;
            this.DisplayAdd = false;
            this.DisplayArray = false;
            this.ShutOffValue = false;
            this.DisplayFileName = false;
        }

        public IEnumerable<JType> JsonPropTypes
        {
            get { return Enum.GetValues(typeof(JType)).Cast<JType>(); }
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
                            this.Value = "{ " + this.Childs.Count() + " }";
                            break;

                        case JType.Array:
                            this.DisplayAdd = true;
                            this.Value = "[ " + this.Childs.Count() + " ]";
                            break;

                        case JType.File:
                            this.ShutOffValue = true;
                            this.DisplayFile = true;
                            this.Value = "선택된 파일이 없습니다";
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

        private bool readOnly = false;
        public bool ReadOnly
        {
            get { return this.readOnly; }
            set
            {
                if (this.readOnly != value)
                {
                    this.readOnly = value;
                    this.RaisePropertyChanged("ReadOnly");
                }
            }
        }
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
            if (this.Parent != null || "".Equals(this.Key.Trim()))
                builder.Append("\"").Append(this.Key).Append("\":");

            switch (this.SelectedJsonType)
            {
                case JType.Array:
                    {
                        builder.Append("[");
                        int nRest = this.Childs.Count() - 1;
                        foreach (var model in this.Childs)
                        {
                            builder.Append("\"").Append(model.Value).Append("\"");
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
                        builder.Append("\"").Append(Path.GetFileName(this.Value)).Append("\"");
                        break;
                    }


                case JType.Object:
                    {
                        builder.Append("{");
                        foreach (var model in this.Childs)
                        {
                            builder.Append(model.ToJson());
                        }
                        builder.Append("}");
                        break;
                    }

                case JType.String:
                    {
                        builder.Append("\"").Append(this.Value).Append("\"");
                        break;
                    }

                case JType.Number:
                    {
                        builder.Append("\"").Append(this.Value).Append("\"");
                        break;
                    }
            }

            string json = Regex.Replace(builder.ToString(), "\"\"", "\",\"")
                .Replace("]\"", "],\"").Replace("}\"", "},\"");
            return json;
        }

        #region Constructor
        public JsonModel(JsonModel parent, bool isReadOnly)
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

            this.ReadOnly = isReadOnly;
            this.Parent = parent;
        }
        #endregion
    }
}
