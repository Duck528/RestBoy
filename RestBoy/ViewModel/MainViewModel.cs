using GalaSoft.MvvmLight;
using Microsoft.Win32;
using Mvvm.Commands;
using RestBoy.Control;
using RestBoy.Model;
using RestBoy.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace RestBoy.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Constructor
        public MainViewModel(ObservableCollection<MainViewModel> mainViewModels)
        {
            this.MainViewModels = mainViewModels;

            this.methods = new ObservableCollection<string>();
            methods.Add("GET");
            methods.Add("POST");
            methods.Add("PUT");
            methods.Add("DELETE");

            this.SelectedMethod = "GET";

            this.JsonModels.Add(new JsonModel(null)
            {
                SelectedJsonType = JType.Object,
                ShutOffValue = true,
                ShutOffDelButton = true,
                HasKey = false,
                ValueBorderThickness = new Thickness(0),
                ReadOnlyValue = true
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

            this.RdoFormData = true;
        }
        #endregion

        #region MainViewModels
        private ObservableCollection<MainViewModel> mainViewModels = null;
        public ObservableCollection<MainViewModel> MainViewModels
        {
            get { return this.mainViewModels; }
            private set { this.mainViewModels = value; }
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
        private async void SendRequest()
        {
            this.RespText = "Loading Now";

            this.requestUri = this.requestUri.Trim();
            if ("".Equals(this.requestUri))
            {
                MessageBox.Show("URL을 입력해주세요");
                return;
            }
            bool hasHttp = this.RequestUri.StartsWith("http://", true, null);
            bool hasHttps = this.RequestUri.StartsWith("https://", true, null);
            string httpUri = string.Empty;
            if (hasHttp == false && hasHttps == false)
            {
                httpUri = "http://" + this.RequestUri;
            }
            else
            {
                httpUri = this.RequestUri;
            }

            // 입력된 Auth를 가져온다 (있다면)

            // 입력된 Header를 가져온다
            var reqHeaders = this.Headers;
            var headers = new Dictionary<string, string>();
            foreach (var param in reqHeaders)
            {
                string headerKey = param.Key.Trim();
                string headerValue = param.Value.Trim();
                if ("".Equals(headerKey) || "".Equals(headerValue))
                    continue;
                headers.Add(headerKey, headerValue);
            }

            // 입력된 URL Param을 가져온다 (입력된 순서대로)
            var reqParams = from p in this.ParameterModels orderby p.Order select p;
            string uriWithParam = string.Empty;
            if (reqParams.Count() != 0)
            {
                var paramBuilder = new StringBuilder(httpUri);
                paramBuilder.Append("?");
                foreach (var param in reqParams)
                {
                    if ("".Equals(param.Key.Trim()) || "".Equals(param.Value.Trim()))
                        continue;
                    paramBuilder.Append(param.Key).Append("=").Append(param.Value).Append("&");
                }
                paramBuilder.Remove(paramBuilder.Length - 1, 1);
                uriWithParam = paramBuilder.ToString();
            }
            else
            {
                uriWithParam = httpUri;
            }

            // 전송 방식(Method)를 가져온다
            string method = this.SelectedMethod;

            HttpRespVo res = null;
            var reqHelper = new ReqHttpHelper();
            if (method.Equals("GET"))
                res = await reqHelper.Get(uriWithParam, headers);
            else
            {
                // 입력된 Body 매개변수를 가져온다
                // 만약, FormData가 선택되어져 있다면 bodyControl.Body를 가져오고,
                // 만약, AppJson이 선택되어 있다면 JsonModels에서 Json으로 변환한다
                if (this.RdoFormData == true)
                {
                    var reqBodies = this.Bodies;
                    var postParams = new Dictionary<string, object>();
                    foreach (var bodyModel in reqBodies)
                    {
                        
                        switch (bodyModel.ValueType)
                        {
                            case "File":
                                {
                                    byte[] data = null;
                                    using (var fs = new FileStream(bodyModel.FilePath, FileMode.Open, FileAccess.Read))
                                    {
                                        data = new byte[fs.Length];
                                        fs.Read(data, 0, data.Length);
                                    }
                                    postParams.Add(
                                        bodyModel.Key,
                                        new FileParam(bodyModel.DisplayFileName,
                                        Path.GetExtension(bodyModel.FilePath), 
                                        data,
                                        "application/octet-stream"));
                                }
                                break;

                            case "Text":
                                {
                                    postParams.Add(bodyModel.Key, bodyModel.Value);
                                }
                                break;
                        }
                    }
                    res = await reqHelper.SendMultipart(uriWithParam, method, postParams, headers);
                }
                else if (this.RdoAppJson == true)
                {
                    string text = this.JsonModels[0].ToJson();
                    string filtered = Regex.Replace(text, ",}", "}").Replace(",]", "]").Replace("}\"", "},\"")
                        .Replace(",,", ",").Replace("}{", "},{").Replace("]\"", "],\"");
                    string json = Regex.Replace(filtered, ",$", "");
                    res = await reqHelper.SendApplicationJson(uriWithParam, method, json, headers);
                }
            }

            if (res.IsSuccess == true)
            {
                this.RespText = res.RespText;

                // Set Resp Headers 
                this.RespHeaders.Clear();
                foreach (var key in res.Headers.Keys)
                {
                    this.RespHeaders.Add(new HeaderModel(null)
                    {
                        Key = key,
                        Value = res.Headers[key]
                    });
                }
                this.NumHeaders = this.RespHeaders.Count();

                // Set Cookies
                foreach (var key in res.Cookies.Keys)
                {
                    this.RespCookies.Add(new HeaderModel(null)
                    {
                        Key = key,
                        Value = res.Cookies[key]
                    });
                }
                this.NumCookies = this.RespCookies.Count();

                this.RespStatus = res.StatusCode;
                this.RespStatusMsg = res.StatusMsg;

                var model = this.DeepCopy();
                this.MainViewModels.Add(model);
            }
            else
            {
                this.RespText = res.ErrorMsg;
            }
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
        private ObservableCollection<ParamModel> parameterModels = null;
        public ObservableCollection<ParamModel> ParameterModels
        {
            get { return this.parameterModels ?? (this.parameterModels = new ObservableCollection<ParamModel>()); }
            private set { this.parameterModels = value; }
        }

        private ICommand addParamCommand = null;
        public ICommand AddParamCommand
        {
            get {
                return (this.addParamCommand) ??
                  (this.addParamCommand = new DelegateCommand(AddParam)); }
        }
        private void AddParam()
        {
            int order = this.ParameterModels.Count();

            var paramModel = new ParamModel(order, this.ParameterModels);
            this.ParameterModels.Add(paramModel);
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
        private ICommand headerCommand = null;
        public ICommand HeaderCommand
        {
            get
            {
                return this.headerCommand
                    ?? (this.headerCommand = new DelegateCommand(ClickedHeader));
            }
        }
        private void ClickedHeader()
        {
            this.DisplayAuthForm = false;
            this.DisplayBodyForm = false;
            this.DisplayHeaderForm = true;
        }
        private bool displayHeaderForm = false;
        public bool DisplayHeaderForm
        {
            get { return this.displayHeaderForm; }
            set
            {
                if (this.displayHeaderForm != value)
                {
                    this.displayHeaderForm = value;
                    this.RaisePropertyChanged("DisplayHeaderForm");
                }
            }
        }

        private ICommand bodyCommand = null;
        public ICommand BodyCommand
        {
            get
            {
                return this.bodyCommand ??
                  (this.bodyCommand = new DelegateCommand(ClickedBody));
            }
        }
        private void ClickedBody()
        {
            this.DisplayAuthForm = false;
            this.DisplayHeaderForm = false;
            this.DisplayBodyForm = true;
        }
        private bool displayBodyForm = false;
        public bool DisplayBodyForm
        {
            get { return this.displayBodyForm; }
            set
            {
                if (this.displayBodyForm != value)
                {
                    this.displayBodyForm = value;
                    this.RaisePropertyChanged("DisplayBodyForm");
                }
            }
        }

        private ICommand authCommand = null;
        public ICommand AuthCommand
        {
            get
            {
                return this.authCommand ??
                  (this.authCommand = new DelegateCommand(ClickedAuth));
            }
        }
        private void ClickedAuth()
        {
            this.DisplayHeaderForm = false;
            this.DisplayBodyForm = false;
            this.DisplayAuthForm = true;
        }
        private bool displayAuthForm = false;
        public bool DisplayAuthForm
        {
            get { return this.displayAuthForm; }
            set
            {
                if (this.displayAuthForm != value)
                {
                    this.displayAuthForm = value;
                    this.RaisePropertyChanged("DisplayAuthForm");
                }
            }
        }
        #endregion

        #region Bodies
        private ObservableCollection<BodyModel> bodies = null;
        public ObservableCollection<BodyModel> Bodies
        {
            get
            {
                return this.bodies ??
                    (this.bodies = new ObservableCollection<BodyModel>());
            }
            private set { this.bodies = value;}
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
            var body = new BodyModel(this.Bodies) { IsChecked = true };
            this.Bodies.Add(body);
        }

        #endregion

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

        private ICommand addKeyValCommand = null;
        public ICommand AddKeyValCommand
        {
            get { return this.addKeyValCommand ??
                  (this.addKeyValCommand = new DelegateCommand(AddKeyVal)); }
        }
        private void AddKeyVal()
        {
            var header = new HeaderModel(this.Headers) { IsChecked = true };
            this.Headers.Add(header);
        }


        #endregion

        #region RespHeaders
        private ObservableCollection<HeaderModel> respHeaders = null;
        public ObservableCollection<HeaderModel> RespHeaders
        {
            get
            {
                return this.respHeaders ??
                    (this.respHeaders = new ObservableCollection<HeaderModel>());
            }
            private set { this.respHeaders = value; }
        }
        private int numHeaders = 0;
        public int NumHeaders
        {
            get { return this.numHeaders; }
            set
            {
                if (this.numHeaders != value)
                {
                    this.numHeaders = value;
                    this.RaisePropertyChanged("NumHeaders");
                }
            }
        }
        #endregion

        #region RespStatus
        private int respStatus = -1;
        public int RespStatus
        {
            get { return this.respStatus; }
            set
            {
                if (this.respStatus != value)
                {
                    this.respStatus = value;
                    this.RaisePropertyChanged("RespStatus");
                }
            }
        }
        private string respStatusMsg = string.Empty;
        public string RespStatusMsg
        {
            get { return this.respStatusMsg; }
            set
            {
                if (this.respStatusMsg.Equals(value) == false)
                {
                    this.respStatusMsg = value;
                    this.RaisePropertyChanged("RespStatusMsg");
                }
            }
        }
        #endregion

        #region RespCookies
        private ObservableCollection<HeaderModel> respCookies = null;
        public ObservableCollection<HeaderModel> RespCookies
        {
            get
            {
                return this.respCookies ??
                    (this.respCookies = new ObservableCollection<HeaderModel>());
            }
            private set { this.respCookies = value; }
        }
        private int numCookies = 0;
        public int NumCookies
        {
            get { return this.numCookies; }
            set
            {
                if (this.numCookies != value)
                {
                    this.numCookies = value;
                    this.RaisePropertyChanged("NumHeaders");
                }
            }
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

        #region BodyForm
        private bool rdoFormData = false;
        public bool RdoFormData
        {
            get { return this.rdoFormData; }
            set
            {
                if (this.rdoFormData != value)
                {
                    this.rdoFormData = value;
                    this.RaisePropertyChanged("RdoFormData");
                }
            }
        }

        private bool rdoAppJson = false;
        public bool RdoAppJson
        {
            get { return this.rdoAppJson; }
            set
            {
                if (this.rdoAppJson != value)
                {
                    this.rdoAppJson = value;
                    this.RaisePropertyChanged("RdoAppJson");
                }
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
            private set { this.jsonModels = value; }
        }
        #endregion

        #region WindowSize
        private int winWidth;
        public int WinWidth
        {
            get { return this.winWidth; }
            set
            {
                if (this.winWidth != value)
                {
                    this.winWidth = value;
                    this.RaisePropertyChanged("RespBodyFormWidth");
                }
            }
        }

        private int winHeight;
        public int WinHeight
        {
            get { return this.winHeight; }
            set
            {
                if (this.winHeight != value)
                {
                    this.winHeight = value;
                    this.RaisePropertyChanged("WinHeight");
                }
            }
        }
        #endregion

        #region DeepCopy
        public MainViewModel DeepCopy()
        {
            var model = new MainViewModel(this.MainViewModels);
            var headers = new ObservableCollection<HeaderModel>();
            foreach (var h in this.Headers)
            {
                var headerModel = new HeaderModel(headers);
                headerModel.Key = h.Key;
                headerModel.Value = h.Value;
                headerModel.IsChecked = h.IsChecked;
                headers.Add(headerModel);
            }
            model.Headers = headers;

            var urlParams = new ObservableCollection<ParamModel>();
            foreach (var p in this.ParameterModels)
            {
                var paramModel = new ParamModel(p.Order, urlParams);
                paramModel.Key = p.Key;
                paramModel.Value = p.Value;
                urlParams.Add(paramModel);
            }
            model.ParameterModels = urlParams;

            var multiparts = new ObservableCollection<BodyModel>();
            foreach (var b in this.Bodies)
            {
                var bodyModel = new BodyModel(multiparts);
                bodyModel.Key = b.Key;
                bodyModel.Value = b.Value;
                bodyModel.ValueType = b.ValueType;
                bodyModel.IsChecked = b.IsChecked;
                bodyModel.FilePath = b.FilePath;
                bodyModel.DisplayFileName = b.DisplayFileName;
                multiparts.Add(bodyModel);
            }
            model.Bodies = multiparts;

            var jsonModels = new ObservableCollection<JsonModel>();
            jsonModels.Add(new JsonModel(null)
            {
                SelectedJsonType = JType.Object,
                ShutOffValue = true,
                ShutOffDelButton = true,
                HasKey = false,
                ValueBorderThickness = new Thickness(0),
                ReadOnlyValue = true
            });
            foreach (var jModel in this.JsonModels[0].Childs)
            {
                var copiedJsonModel = jModel.RecursiveDeepCopy();
                jsonModels[0].Childs.Add(copiedJsonModel);
            }
            model.JsonModels = jsonModels;

            var respHeaders = new ObservableCollection<HeaderModel>();
            foreach (var headerModel in this.RespHeaders)
            {
                var hModel = new HeaderModel(null);
                hModel.Key = headerModel.Key;
                hModel.Value = headerModel.Value;
                hModel.IsChecked = headerModel.IsChecked;
                respHeaders.Add(hModel);
            }
            model.RespHeaders = respHeaders;
            model.NumHeaders = respHeaders.Count;

            var respCookies = new ObservableCollection<HeaderModel>();
            foreach (var cookieModel in this.RespCookies)
            {
                var cModel = new HeaderModel(null);
                cModel.Key = cookieModel.Key;
                cModel.Value = cookieModel.Value;
                cModel.IsChecked = cookieModel.IsChecked;
                respCookies.Add(cModel);
            }
            model.RespCookies = respCookies;
            model.numCookies = respCookies.Count;

            model.RequestUri = this.RequestUri;
            model.SelectedMethod = this.SelectedMethod;
            model.RespText = this.RespText;
            model.RespStatus = this.RespStatus;
            model.RespStatusMsg = this.RespStatusMsg;
            model.RdoFormData = this.RdoFormData;
            model.RdoAppJson = this.RdoAppJson;
            model.DisplayAuthForm = this.DisplayAuthForm;
            model.DisplayBodyForm = this.DisplayBodyForm;
            model.DisplayHeaderForm = this.DisplayHeaderForm;
            model.ParamDisplay = this.ParamDisplay;
            model.EnableBodyButton = this.EnableBodyButton;

            return model;
        }
        #endregion
    }
}
