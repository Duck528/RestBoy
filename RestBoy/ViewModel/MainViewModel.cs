using GalaSoft.MvvmLight;
using Microsoft.Win32;
using Mvvm.Commands;
using RestBoy.Control;
using RestBoy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RestBoy.ViewModel
{
    class MainViewModel : ObservableObject
    {
        #region Constructor
        public MainViewModel()
        {
            this.methods = new ObservableCollection<string>();
            methods.Add("GET");
            methods.Add("POST");
            methods.Add("PUT");
            methods.Add("DELETE");

            this.SelectedMethod = "GET";

            this.controlMap = new Dictionary<string, UIElement>();

            var headerForm = new HeaderForm();
            headerForm.DataContext = this;
            this.controlMap.Add("header", headerForm);

            var bodyForm = new BodyForm();
            bodyForm.DataContext = this;
            this.controlMap.Add("body", bodyForm);

            var authForm = new AuthForm();
            authForm.DataContext = this;
            this.controlMap.Add("auth", authForm);

            this.JsonModels.Add(new JsonModel(null, true)
            {
                Key = "{  }",
                SelectedJsonType = JType.Object,
                ShutOffValue = true,
                ShutOffDelButton = true
            });
            this.JsonModels[0].Childs.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    { 
                        this.JsonModels[0].Key = "{ " + this.JsonModels[0].Childs.Count() + " }";
                    }

                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        this.JsonModels[0].Key = "{ " + this.JsonModels[0].Childs.Count() + " }";
                    }
                });
        }
        #endregion

        #region Logs
        private ObservableCollection<LogModel> logs = null;
        public ObservableCollection<LogModel> Logs
        {
            get
            {
                return (this.logs) ??
                    (this.logs = new ObservableCollection<LogModel>());
            } 
        }
        #endregion

        #region Methods
        private ObservableCollection<string> methods = null;
        public ObservableCollection<string> Methods
        {
            get
            {
                return (this.methods) ??
                    (this.methods = new ObservableCollection<string>());
            }
        }
        #endregion

        #region SelectedMethod
        private string selectedMethod = string.Empty;
        public string SelectedMethod
        {
            get { return this.selectedMethod; }
            set
            {
                if (selectedMethod != value)
                {
                    this.selectedMethod = value;
                    this.RaisePropertyChanged("SelectedMethod");
                    this.DisplayButtons();
                }
            }
        }
        #endregion

        #region SearchLogWord
        private string searchLogWord = string.Empty;
        public string SearchWord
        {
            get { return this.searchLogWord; }
            set
            {
                if (this.searchLogWord != value)
                {
                    this.searchLogWord = value;
                    this.RaisePropertyChanged("SearchWord");
                }
            }
        }
        #endregion

        #region SearchLogCommand
        private void SearchLog()
        {
            MessageBox.Show("Hello!");
        }

        private bool CanSearchLog()
        {
            // 검색할 데이터가 비어있다면,
            if (this.searchLogWord.Equals(string.Empty))
                return false;

            return true;
        }
        private ICommand searchLogCommand = null;
        public ICommand SearchLogCommand
        {
            get { return (this.searchLogCommand) ??
                    (this.searchLogCommand = new DelegateCommand(SearchLog, CanSearchLog)); }
        }
        #endregion

        #region RequestUri
        private string requestUri = string.Empty;
        public string RequestUri
        {
            get { return this.requestUri; }
            set
            {
                if (this.requestUri != value)
                {
                    this.requestUri = value;
                    this.RaisePropertyChanged("RequestUri");
                }
            }
        }
        #endregion

        #region SendReqCommand
        private ICommand sendCommand = null;
        public ICommand SendCommand
        {
            get
            {
                return this.sendCommand ??
                  (this.sendCommand = new DelegateCommand(SendRequest));
            }
        }
        private void SendRequest()
        {
            this.requestUri = this.requestUri.Trim();
            if ("".Equals(this.requestUri))
            {
                MessageBox.Show("URL을 입력해주세요");
                return;
            }
            // 입력된 Auth를 가져온다 (있다면)

            // 전송 방식(Method)를 가져온다
            string method = this.SelectedMethod;

            // 만약, GET이 아닌 경우 Data의 타입을 가져온다
            if (method.Equals("GET") == false)
            {
                UIElement uiElem = null;
                bool flag = this.controlMap.TryGetValue("body", out uiElem);
                if (flag == false)
                    return;

                var bodyForm = uiElem as BodyForm;
                if (bodyForm == null)
                    return;
            }

            // 입력된 URL Param을 가져온다 (입력된 순서대로)
            var reqParams = from paramControl in this.Parameters
                            orderby paramControl.ParamModel.Order ascending
                            select paramControl.ParamModel;



            // 입력된 Header를 가져온다
            var reqHeaders = from headerControl in this.Headers
                             select headerControl.Header;

            // 입력된 Body 매개변수를 가져온다
            var reqBodies = from bodyControl in this.Bodies
                            select bodyControl.Body;
        }
        #endregion

        #region ParamActivate
        private bool paramDisplay = false;
        public bool ParamDisplay
        {
            get { return this.paramDisplay; }
            set
            {
                this.paramDisplay = !(value);
                this.RaisePropertyChanged("ParamDisplay");
            }
        }
        private void ToggleParamDisplay()
        {
            this.ParamDisplay = this.ParamDisplay;
        }
        private ICommand toggleParamDisplayCommand = null;
        public ICommand ToggleParamDisplayCommand
        {
            get { return (this.toggleParamDisplayCommand) ??
                    (this.toggleParamDisplayCommand = new DelegateCommand(ToggleParamDisplay)); }
        }
        #endregion

        #region ResponseFormActive
        private bool respDisplay = false;
        public bool RespDisplay
        {
            get { return this.respDisplay; }
            set
            {
                this.respDisplay = !(value);
                this.RaisePropertyChanged("RespDisplay");
            }
        }
        private void ToggleRespDisplay()
        {
            this.RespDisplay = this.RespDisplay;
        }
        private ICommand toggleRespDisplayCommand = null;
        public ICommand ToggleRespDisplayCommand
        {
            get { return this.toggleRespDisplayCommand ??
                  (this.toggleRespDisplayCommand = new DelegateCommand(ToggleRespDisplay)); }
        }
        #endregion

        #region AddParamCommand
        private ICommand addParamCommand = null;
        public ICommand AddParamCommand
        {
            get {
                return (this.addParamCommand) ??
                  (this.addParamCommand = new DelegateCommand(AddParam)); }
        }
        private void AddParam()
        {
            int order = this.Parameters.Count();

            var paramModel = new ParamModel(order);
            var param = new ParamControl(paramModel);
            param.DataContext = this;
            this.Parameters.Add(param);
        }

        private ICommand deleteParamCommand = null;
        public ICommand DeleteParamCommand
        {
            get { return (this.deleteParamCommand) ??
                    (this.deleteParamCommand = new DelegateCommand<object>(DeleteParam)); }
        }
        private void DeleteParam(object param)
        {
            var delParam = param as ParamControl;
            if (delParam != null)
            {
                var paramModel = delParam.ParamModel;
                this.Parameters.Remove(delParam);
            }
        }
        #endregion

        #region Params
        private ObservableCollection<ParamControl> parameters = null;
        private UIElement selectedParam = null;
        public UIElement SelectedParam
        {
            get { return this.selectedParam; }
            set
            {
                if (this.selectedParam != value)
                {
                    this.selectedParam = value;
                    this.RaisePropertyChanged("SelectedParam");
                }
            }
        }
        public ObservableCollection<ParamControl> Parameters
        {
            get
            {
                return (this.parameters) ??
                    (this.parameters = new ObservableCollection<ParamControl>());
            }
        }

        private ObservableCollection<string> paramComboVals = new ObservableCollection<string>
        {
            "Text", "File"
        };

        public ObservableCollection<string> ParamComboVals
        {
            get { return this.paramComboVals; }
        }
        #endregion

        #region EnableButtons
        private void DisplayButtons()
        {
            switch (this.selectedMethod)
            {
                case "GET":
                    this.EnableBodyButton = false;
                    break;

                default:
                    this.EnableBodyButton = true;
                    break;
            }
        }
        private bool enableBodyButton = true;
        public bool EnableBodyButton
        {
            get { return this.enableBodyButton; }
            set
            {
                if (this.enableBodyButton != value)
                {
                    this.enableBodyButton = value;
                    this.RaisePropertyChanged("EnableBodyButton");
                }
            }
        }
        #endregion

        #region ButtonClickCommands
        private Dictionary<string, UIElement> controlMap = null;

        private ICommand headerCommand = null;
        public ICommand HeaderCommand
        {
            get
            {
                return this.headerCommand
                    ?? (this.headerCommand = new DelegateCommand<object>(ClickedHeader));
            }
        }
        private void ClickedHeader(object panel)
        {
            var dockpanel = panel as DockPanel;
            if (dockpanel == null)
                return;

            var uiElem = this.controlMap["header"];
            dockpanel.Children.Clear();
            dockpanel.Children.Add(uiElem);

        }

        private ICommand bodyCommand = null;
        public ICommand BodyCommand
        {
            get
            {
                return this.bodyCommand ??
                  (this.bodyCommand = new DelegateCommand<object>(ClickedBody));
            }
        }
        private void ClickedBody(object panel)
        {
            var dockpanel = panel as DockPanel;
            if (dockpanel == null)
                return;

            var uiElem = this.controlMap["body"];
            dockpanel.Children.Clear();
            dockpanel.Children.Add(uiElem);
        }

        private ICommand authCommand = null;
        public ICommand AuthCommand
        {
            get
            {
                return this.authCommand ??
                  (this.authCommand = new DelegateCommand<object>(ClickedAuth));
            }
        }
        private void ClickedAuth(object panel)
        {
            var dockpanel = panel as DockPanel;
            if (dockpanel == null)
                return;

            var uiElem = this.controlMap["auth"];
            dockpanel.Children.Clear();
            dockpanel.Children.Add(uiElem);
        }
        #endregion

        #region Bodies
        private ObservableCollection<BodyControl> bodies = null;
        public ObservableCollection<BodyControl> Bodies
        {
            get
            {
                return this.bodies ??
                    (this.bodies = new ObservableCollection<BodyControl>());
            }
        }
        private ICommand addBodyControlCommand = null;
        public ICommand AddBodyControlCommand
        {
            get {
                return this.addBodyControlCommand ??
                  (this.addBodyControlCommand = new DelegateCommand(AddBodyControl));
            }
        }
        private void AddBodyControl()
        {
            var body = new BodyControl(new BodyModel() { IsChecked = true });
            body.DataContext = this;
            this.Bodies.Add(body);
        }
        private ICommand deleteBodyCommand = null;
        public ICommand DeleteBodyCommand
        {
            get
            {
                return this.deleteBodyCommand ??
                  (this.deleteBodyCommand = new DelegateCommand<object>(DeleteBody));
            }
        }
        private void DeleteBody(object delBody)
        {
            var bodyControl = delBody as BodyControl;
            if (bodyControl == null)
                return;

            this.Bodies.Remove(bodyControl);
        }
        #endregion

        #region Headers
        private ObservableCollection<KeyValueControl> headers = null;
        public ObservableCollection<KeyValueControl> Headers
        {
            get
            {
                return this.headers ?? 
                    (this.headers = new ObservableCollection<KeyValueControl>());
            }
        }

        private ICommand addKeyValCommand = null;
        public ICommand AddKeyValCommand
        {
            get { return this.addKeyValCommand ??
                  (this.addKeyValCommand = new DelegateCommand(AddKeyVal)); }
        }
        private void AddKeyVal()
        {
            var header = new KeyValueControl(new HeaderModel() { IsChecked=true });
            header.DataContext = this;
            this.headers.Add(header);
        }

        private ICommand delKeyValCommand = null;
        public ICommand DelKeyValCommand
        {
            get { return this.delKeyValCommand ??
                  (this.delKeyValCommand = new DelegateCommand<object>(DelKeyVal)); }
        }
        private void DelKeyVal(object control)
        {
            var keyValControl = control as KeyValueControl;
            if (keyValControl == null)
                return;

            this.Headers.Remove(keyValControl);
        }
        #endregion

        #region Response
        private string testText = "12345698";
        public string TestText
        {
            get { return this.testText; }
            private set
            {
                if (this.testText.Equals(value) == false)
                {
                    this.testText = value;
                    this.RaisePropertyChanged("TestText");
                }
            }
        }
        private Dictionary<string, UIElement> respFormMap = null;
        public Dictionary<string, UIElement> RespFormMap
        {
            get { return this.respFormMap; }
        }

        private ObservableCollection<HeaderModel> respHeaders = null;
        public ObservableCollection<HeaderModel> RespHeaders
        {
            get
            {
                return this.respHeaders ??
                  (this.respHeaders = new ObservableCollection<HeaderModel>());
            }
        }
        #endregion

        #region JsonModel
        private ObservableCollection<JsonModel> jsonModels = null;
        public ObservableCollection<JsonModel> JsonModels
        {
            get
            {
                return this.jsonModels ??
                    (this.jsonModels = new ObservableCollection<JsonModel>());
            }
        }
        #endregion


    }
}
