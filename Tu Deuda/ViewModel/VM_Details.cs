using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Data;
using Tu_Deuda.Helpers;
using Tu_Deuda.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_Details : BaseVM
    {
        private Application_Context _dbContext = new Application_Context();
        public MClient receivedClient { get; set; }

        public VM_Details(INavigation navigation, MClient client)
        {
            LanguageUI = LocalStorange.GetStorange("language");

            if (LanguageUI == "EN") LoadLanguage();
            LoadLanguage();

            receivedClient = client;
            Navigation = navigation;

            Task.Run(async () => await GetDataBase());

            Load_Data();

            Color = "Black";
        }

        public void CambioColor()
        {
            if (Timeout.Equals(5, 2))
            {
                Color = "Black";
            }
        }

        private static string _hour = DateTime.Now.ToString("HH:mm");
        private static string _date = DateTime.Now.ToString("dd/MM/yyyy");
        private string _dateNow = $"{_date} - {_hour}";

        private string _name;
        private float _deuda;
        private float _abono;
        private float _valueFinal;
        private string _description;
        private string _credito = "CREDITO";
        private string _debito = "DEBITO";
        private string _type;
        private string _color;
        private string _follow;

        public string TextName
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value);
                OnPropertyChanged();
            }
        }

        public float TextValor
        {
            get { return _deuda; }
            set
            {
                SetProperty(ref _deuda, value);
                OnPropertyChanged(nameof(TextValor));
            }
        }

        public float TextAbono
        {
            get { return _abono; }
            set
            {
                SetProperty(ref _abono, value);
                TextValorFinal = Calculo(receivedClient.Saldo_Inicial, TextAbono);
                Color = "red";
                OnPropertyChanged();
            }
        }

        public float TextValorFinal
        {
            get { return _valueFinal; }
            set
            {
                SetProperty(ref _valueFinal, value);
                OnPropertyChanged(nameof(TextValorFinal));
            }
        }

        public string TextDescription
        {
            get { return _description; }
            set
            {
                SetValue(ref _description, value);
                OnPropertyChanged();
            }
        }

        public string TextDate
        {
            get { return _dateNow; }
            set
            {
                SetValue(ref _dateNow, value);
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get { return _type; }
            set { SetValue(ref _type, value); }
        }

        public string SelectType
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
                Type = _type;

                TextValorFinal = Calculo(receivedClient.Saldo_Inicial, TextAbono);
                Color = "red";
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get { return _color; }
            set { SetValue(ref _color, value); }
        }

        public string Follow
        {
            get { return _follow; }
            set { SetValue(ref _follow, value); }
        }

        // DATABASE CONFIG
        private string fetchData;

        private string _urlProyect;
        private string _urlKeyProyect;

        public string FetchData
        {
            get { return fetchData; }
            set
            {
                SetValue(ref fetchData, value);
                OnPropertyChanged();
            }
        }

        public string URLProyect
        {
            get { return _urlProyect; }
            set
            {
                SetValue(ref _urlProyect, value);
                OnPropertyChanged();
            }
        }

        public string KeyProyect
        {
            get { return _urlKeyProyect; }
            set
            {
                SetValue(ref _urlKeyProyect, value);
                OnPropertyChanged();
            }
        }

        // DATABASE CONFIG

        #region TRADUCTIONES

        private string _languageUI;
        private string _labelPreviousValue;
        private string _labelCurrentValue;
        private string _labelResultFinal;
        private string _labelName;
        private string _labelDescription;
        private static string _labelDate;
        private static string _labelHour;
        private string _labelType;
        private string _labelCredit;
        private string _labelDebit;
        private string _textBtnSave;
        private string _labelnewValue;
        private string _placeholderDescription;
        private string _placeholderValue;
        private string _pickerInfor;

        public string LanguageUI
        {
            get { return _languageUI; }
            set
            {
                SetValue(ref _languageUI, value);
                OnPropertyChanged();
            }
        }

        public string LabelPreviousValue
        {
            get { return _labelPreviousValue; }
            set
            {
                SetValue(ref _labelPreviousValue, value);
                OnPropertyChanged();
            }
        }

        public string LabelCurrentValue
        {
            get { return _labelCurrentValue; }
            set
            {
                SetValue(ref _labelCurrentValue, value);
                OnPropertyChanged();
            }
        }

        public string LabelResultFinal
        {
            get { return _labelResultFinal; }
            set
            {
                SetValue(ref _labelResultFinal, value);
                OnPropertyChanged();
            }
        }

        public string LabelName
        {
            get { return _labelName; }
            set
            {
                SetValue(ref _labelName, value);
                OnPropertyChanged();
            }
        }

        public string LabelDescription
        {
            get { return _labelDescription; }
            set
            {
                SetValue(ref _labelDescription, value);
                OnPropertyChanged();
            }
        }

        public string LabelHour
        {
            get { return _labelHour; }
            set
            {
                SetValue(ref _labelHour, value);
            }
        }

        public string LabelDate
        {
            get { return _labelDate; }
            set
            {
                SetValue(ref _labelDate, value);
            }
        }

        public string LabelType
        {
            get { return _labelType; }
            set
            {
                SetValue(ref _labelType, value);
                OnPropertyChanged();
            }
        }

        public string LabelCredit
        {
            get { return _labelCredit; }
            set
            {
                SetValue(ref _labelCredit, value);
                OnPropertyChanged();
            }
        }

        public string LabelDebit
        {
            get { return _labelDebit; }
            set
            {
                SetValue(ref _labelDebit, value);
                OnPropertyChanged();
            }
        }

        public string TextBtnSave
        {
            get { return _textBtnSave; }
            set
            {
                SetValue(ref _textBtnSave, value);
                OnPropertyChanged();
            }
        }

        public string LabelNewValue
        {
            get { return _labelnewValue; }
            set
            {
                SetValue(ref _labelnewValue, value);
                OnPropertyChanged();
            }
        }

        public string PlaceholderDescription
        {
            get { return _placeholderDescription; }
            set
            {
                SetValue(ref _placeholderDescription, value);
                OnPropertyChanged();
            }
        }

        public string PlaceholderValue
        {
            get { return _placeholderValue; }
            set
            {
                SetValue(ref _placeholderValue, value);
                OnPropertyChanged();
            }
        }

        public string PickerInfor
        {
            get { return _pickerInfor; }
            set
            {
                SetValue(ref _pickerInfor, value);
                OnPropertyChanged();
            }
        }

        #endregion TRADUCTIONES

        public void LoadLanguage()
        {
            if (LanguageUI == "EN")
            {
                LabelPreviousValue = LanguageApp._valuePreviousEN;
                LabelCurrentValue = LanguageApp._valueCurrentEN;
                LabelResultFinal = LanguageApp._resultFinalLabelEN;
                LabelName = LanguageApp._labelNameEN;
                LabelDescription = LanguageApp._labelDescriptionEN;
                LabelDate = LanguageApp._dateTextEN;
                LabelType = LanguageApp._typeTransactionEN;
                LabelCredit = LanguageApp._creditEN;
                LabelDebit = LanguageApp._debitEN;
                TextBtnSave = LanguageApp._btnTextEN;
                LabelNewValue = LanguageApp._newValueLabelEN;
                PlaceholderDescription = LanguageApp._descriptionPlaceholderEN;
                PlaceholderValue = LanguageApp._valueEN;
                PickerInfor = LanguageApp._delectPickerEN;
                LabelHour = LanguageApp._hourTextEN;
                LabelDate = LanguageApp._dateTextEN;
                Follow = LanguageApp._followEN;
            }
            else
            {
                LabelPreviousValue = LanguageApp._valuePreviousES;
                LabelCurrentValue = LanguageApp._valueCurrentES;
                LabelResultFinal = LanguageApp._resultFinalLabelES;
                LabelName = LanguageApp._labelNameES;
                LabelDescription = LanguageApp._labelDescriptionES;
                LabelDate = LanguageApp._dateTextES;
                LabelType = LanguageApp._typeTransactionES;
                LabelCredit = LanguageApp._creditES;
                LabelDebit = LanguageApp._debitES;
                TextBtnSave = LanguageApp._btnTextES;
                LabelNewValue = LanguageApp._newValueLabelES;
                PlaceholderDescription = LanguageApp._descriptionPlaceholderES;
                PlaceholderValue = LanguageApp._valueES;
                PickerInfor = LanguageApp._delectPickerES;
                LabelHour = LanguageApp._hourTextES;
                LabelDate = LanguageApp._dateTextES;
                Follow = LanguageApp._followES;
            }
        }

        public void Load_Data()
        {
            TextDate = receivedClient.Fecha;
            TextName = receivedClient.Name;
            TextValor = receivedClient.Saldo_Inicial;
            TextDescription = receivedClient.Description;

            if (TextAbono != 0)
            {
                TextValor = receivedClient.Saldo_Inicial;
            }
            else
            {
                TextValorFinal = Calculo(receivedClient.Saldo_Inicial, TextAbono);
            }
        }

        public float Calculo(float saldo, float abono)
        {
            if (SelectType == _credito)
            {
                return TextValorFinal = saldo + abono;
            }

            if (SelectType == _debito)
            {
                return TextValorFinal = saldo - abono;
            }

            return TextValorFinal;
        }

        public async Task GetDataBase()
        {
            var queryDatabase = _dbContext.DBApp.Find(1);
            if (queryDatabase == null)
            {
                await DisplayAlert("Alert", "You must configure the database", "OK");
                return;
            }
            else
            {
                FetchData = queryDatabase.NameDatabase;
                URLProyect = queryDatabase.UrlProyect;
                KeyProyect = queryDatabase.KeyProyect;
            }
        }

        public async Task SaveData()
        {
            switch (FetchData)
            {
                case "Sqlite":
                    await UpdateSqlite();
                    break;

                case "Supabase":
                    await UpdateSupabase();
                    break;

                case "Firebase":
                    await UpdateFirebase();
                    break;

                case "Web":
                    await UpdateWeb();
                    break;

                default:
                    await DisplayAlert("Alert", "You must configure the database", "OK");
                    break;
            }
        }

        public async Task UpdateSqlite()
        {
            if (TextValorFinal == 0)
            {
                if (Valitations() == true)
                {
                    receivedClient.Status = false;
                    receivedClient.Saldo_Inicial = 0;
                    receivedClient.Description = "";
                    receivedClient.Name = TextName;
                    receivedClient.Fecha = _date;

                    _dbContext.Update(receivedClient);

                    await _dbContext.SaveChangesAsync();

                    await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

                    await Back_Home();
                }
            }
            else
            {
                if (Valitations() == true)
                {
                    receivedClient.Status = true;
                    receivedClient.Saldo_Inicial = TextValorFinal;
                    receivedClient.Description = TextDescription + "-";
                    receivedClient.Name = TextName;
                    receivedClient.Fecha = _dateNow;

                    _dbContext.Update(receivedClient);

                    await _dbContext.SaveChangesAsync();

                    await DisplayAlert("info", $"Tu Deuda Ahora es de: {TextValorFinal}", "ok");

                    await Back_Home();
                }
            }
        }

        public async Task UpdateSupabase()
        {
            if (TextValorFinal == 0)
            {
                if (Valitations() == true)
                {
                    await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

                    await Back_Home();
                }
            }
            else
            {
                if (Valitations() == true)
                {
                    await DisplayAlert("info", $"Tu Deuda Ahora es de: {TextValorFinal}", "ok");

                    await Back_Home();
                }
            }
        }

        public async Task UpdateFirebase()
        {
            FirebaseClient firebase = new FirebaseClient(Connections.urlFirebase());

            if (TextValorFinal == 0)
            {
                if (Valitations() == true)
                {
                    await firebase.Child("Clients").Child(receivedClient.ClientId).PutAsync(new MClient
                    {
                        Name = TextName,
                        Description = "",
                        Saldo_Inicial = 0,
                        Fecha = _dateNow,
                        Status = false
                    });

                    await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

                    await Back_Home();
                }
            }
            else
            {
                if (Valitations() == true)
                {
                    await firebase.Child("Clients").Child(receivedClient.ClientId).PutAsync(new MClient
                    {
                        Name = TextName,
                        Description = TextDescription + "-",
                        Saldo_Inicial = TextValorFinal,
                        Fecha = _dateNow,
                        Status = true
                    });

                    await DisplayAlert("info", $"Tu Deuda Ahora es de: {TextValorFinal}", "ok");

                    await Back_Home();
                }
            }
        }

        public async Task UpdateWeb()
        {
            var fetch = new HttpClient();

            if (TextValorFinal == 0)
            {
                if (Valitations() == true)
                {
                    await fetch.PutAsync($"{Connections.urlWebApi()}/api/ControllerClient/{receivedClient.Id}",
                        new StringContent(JsonConvert.SerializeObject(new MClient
                        {
                            Name = TextName,
                            Description = "",
                            Saldo_Inicial = 0,
                            Fecha = _dateNow,
                            Status = false
                        }), Encoding.UTF8, "application/json"));

                    await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

                    await Back_Home();
                }
            }
            else
            {
                if (Valitations() == true)
                {
                    await fetch.PutAsync($"{Connections.urlWebApi()}/api/ControllerClient/{receivedClient.Id}",
                       new StringContent(JsonConvert.SerializeObject(new MClient
                       {
                           Name = TextName,
                           Description = TextDescription + "-",
                           Saldo_Inicial = TextValorFinal,
                           Fecha = _dateNow,
                           Status = true
                       }), Encoding.UTF8, "application/json"));

                    await DisplayAlert("info", $"Tu Deuda Ahora es de: {TextValorFinal}", "ok");

                    await Back_Home();
                }
            }
        }

        public bool Valitations()
        {
            if (string.IsNullOrEmpty(TextName))
            {
                DisplayAlert("Error", "Debes ingresar un nombre", "Ok");
                return false;
            }
            else if (TextAbono == 0)
            {
                DisplayAlert("Error", "Debes ingresar un valor", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(TextDescription))
            {
                DisplayAlert("Error", "Debes ingresar una descripcion", "Ok");
                return false;
            }
            else if (SelectType == null)
            {
                DisplayAlert("Error", "Debes selecionar una opcion", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task Back_Home()
        {
            await Navigation.PopToRootAsync();
        }

        public async Task Delete_Credito()
        {
            if (LanguageUI == "EN" ? await DisplayAlert("Alert", "Are you sure you want to delete this credit?", "Yes", "No") :
                await DisplayAlert("Alerta", "¿Estas seguro de eliminar este credito?", "Si", "No"))
            {
                switch (FetchData)
                {
                    case "Sqlite":
                        await DeleteSqlite();
                        break;

                    case "Supabase":
                        await DeleteSupabase();
                        break;

                    case "Firebase":
                        await DeleteFirebase();
                        break;

                    case "Web":
                        await DeleteWeb();
                        break;

                    default:
                        if (LanguageUI == "EN")
                        {
                            await Alerts.ShowAlert("Error", "You must configure the database", "ok");
                        }
                        else
                        {
                            await Alerts.ShowAlert("Error", "Debes Configurar la Base de Datos", "ok");
                        }
                        break;
                }
            }
        }

        public async Task DeleteSqlite()
        {
            receivedClient.Status = false;
            receivedClient.Saldo_Inicial = 0;
            receivedClient.Description = "";
            receivedClient.Name = TextName;
            receivedClient.Fecha = _date;

            _dbContext.Update(receivedClient);

            await _dbContext.SaveChangesAsync();

            await AlertPaymend();

            Color = "Black";
            Load_Data();
        }

        public async Task DeleteSupabase()
        {
            await AlertPaymend();

            Color = "Black";
            Load_Data();
        }

        public async Task DeleteFirebase()
        {
            FirebaseClient firebase = new FirebaseClient(Connections.urlFirebase());

            receivedClient.Status = false;
            receivedClient.Saldo_Inicial = 0;
            receivedClient.Description = "";
            receivedClient.Name = TextName;
            receivedClient.Fecha = _date;

            await firebase
                .Child("Clients")
                .Child(receivedClient.ClientId)
                .PutAsync(receivedClient);

            await AlertPaymend();
            Color = "Black";
            Load_Data();
        }

        public async Task DeleteWeb()
        {
            var fetch = new HttpClient();

            var deleteDeuda = await fetch.DeleteAsync($"{Connections.urlWebApi()}/api/ControllerClient/{receivedClient.Id}");

            if (deleteDeuda.IsSuccessStatusCode)
            {
                await AlertPaymend();
                Color = "Black";
                Load_Data();
            }
            else
            {
                await AlertError();
            }
        }

        public async Task AlertPaymend()
        {
            if (LanguageUI == "EN")
            {
                await Alerts.ShowAlert("info", "Done You Have No Debt", "ok");
            }
            else
            {
                await Alerts.ShowAlert("info", "Listo Ya No Tienes Deuda", "ok");
            }
        }

        public async Task AlertError()
        {
            if (LanguageUI == "EN")
            {
                await Alerts.ShowAlert("Error", "Error to Delete", "ok");
            }
            else
            {
                await Alerts.ShowAlert("Error", "Error al Eliminar", "ok");
            }
        }

        public async Task OpenUrl(string url)
        {
            await Browser.OpenAsync(url);
        }

        #region COMMANDS

        public ICommand btnDelete => new Command(async () => await Delete_Credito());
        public ICommand btngoHome => new Command(async () => await Back_Home());
        public ICommand btnSave_Data => new Command(async () => await SaveData());

        public ICommand btnInstagram => new Command(async () => await OpenUrl("https://www.instagram.com/theerudito/"));
        public ICommand btnGithub => new Command(async () => await OpenUrl("https://github.com/theerudito?tab=repositories"));
        public ICommand btnThreads => new Command(async () => await OpenUrl("https://www.threads.net/@theerudito"));
        public ICommand btnLinkedin => new Command(async () => await OpenUrl("https://www.linkedin.com/in/theerudito/"));

        #endregion COMMANDS
    }
}