using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Supabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Data;
using Tu_Deuda.Helpers;
using Tu_Deuda.Model;
using Tu_Deuda.View;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_PageHome : BaseVM
    {
        private Application_Context _dbContext = new Application_Context();

        public Command LoadItemsCommand { get; }

        public VM_PageHome(INavigation navigation)
        {
            Navigation = navigation;

            Language = LocalStorange.GetStorange("language");

            if (Language == "EN") Language_Select();
            else Language_Select();

            Task.Run(async () => await GetDataBase());

            Task.Run(async () => await Load_Data());

            LoadItemsCommand = new Command(async () => await Load_Data());
        }

        private ObservableCollection<MClient> _list_client;

        private static string _labelDate;
        private static string _labelHour;
        private static string _hour = DateTime.Now.ToString("HH:mm");
        private static string _date = DateTime.Now.ToString("dd/MM/yyyy");
        private string _dateNow = $"{_date}";
        private string _nameClient;
        private float _saldoInicial;
        private string _descripcionClient;
        private string _imageClient = "png";
        private bool _status = true;
        private string _searching;
        private ImageSource _flag;
        private string _language;

        public string TextName
        {
            get { return _nameClient; }
            set
            {
                SetValue(ref _nameClient, value);
                OnPropertyChanged();
            }
        }

        public float TextValor
        {
            get { return _saldoInicial; }
            set
            {
                SetValue(ref _saldoInicial, value);
                OnPropertyChanged();
            }
        }

        public string TextDescription
        {
            get { return _descripcionClient; }
            set
            {
                SetValue(ref _descripcionClient, value);
                OnPropertyChanged();
            }
        }

        public string TextDate
        {
            get { return _dateNow; }
            set
            {
                SetValue(ref _dateNow, value);
                OnPropertyChanged(nameof(TextDate));
            }
        }

        public string TextSeaching
        {
            get { return _searching; }
            set
            {
                SetValue(ref _searching, value);
                OnPropertyChanged();
            }
        }

        public ImageSource Flag
        {
            get { return _flag; }
            set
            {
                SetValue(ref _flag, value);
                OnPropertyChanged();
            }
        }

        public string Language
        {
            get { return _language; }
            set
            {
                SetValue(ref _language, value);
                OnPropertyChanged();
            }
        }

        #region Language

        public string _nameLabel;
        public string _valueLabel;
        public string _descriptionLabel;
        public string _addClientLabel;
        public string _searchClientLabel;
        public string _credit;
        private string _labelHeaderName;
        private string _labelHeaderValue;
        private string _labelHeaderDate;
        private string _labelHeaderAction;

        // DATABASE CONFIG
        private static string fetchData;

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

        // LANGUAGE

        public string NameLabel
        {
            get { return _nameLabel; }
            set
            {
                SetValue(ref _nameLabel, value);
                OnPropertyChanged();
            }
        }

        public string ValueLabel
        {
            get { return _valueLabel; }
            set
            {
                SetValue(ref _valueLabel, value);
                OnPropertyChanged();
            }
        }

        public string DescriptionLabel
        {
            get { return _descriptionLabel; }
            set
            {
                SetValue(ref _descriptionLabel, value);
                OnPropertyChanged();
            }
        }

        public string AddClientLabel
        {
            get { return _addClientLabel; }
            set
            {
                SetValue(ref _addClientLabel, value);
                OnPropertyChanged();
            }
        }

        public string SearchClient
        {
            get { return _searchClientLabel; }
            set
            {
                SetValue(ref _searchClientLabel, value);
                OnPropertyChanged();
            }
        }

        public string Credit
        {
            get { return _credit; }
            set
            {
                SetValue(ref _credit, value);
                OnPropertyChanged();
            }
        }

        public string LabelDate
        {
            get => _labelDate;
            set => _labelDate = value;
        }

        public string LabelHour
        {
            get { return _labelHour; }
            set { SetValue(ref _labelHour, value); }
        }

        public string LabelHeaderName
        {
            get { return _labelHeaderName; }
            set
            {
                SetValue(ref _labelHeaderName, value);
                OnPropertyChanged();
            }
        }

        public string LabelHeaderValue
        {
            get { return _labelHeaderValue; }
            set
            {
                SetValue(ref _labelHeaderValue, value);
                OnPropertyChanged();
            }
        }

        public string LabelHeaderDate
        {
            get { return _labelHeaderDate; }
            set
            {
                SetValue(ref _labelHeaderDate, value);
                OnPropertyChanged();
            }
        }

        public string LabelHeaderAction
        {
            get { return _labelHeaderAction; }
            set
            {
                SetValue(ref _labelHeaderAction, value);
                OnPropertyChanged();
            }
        }

        #endregion Language

        public ObservableCollection<MClient> List_Client
        {
            get { return _list_client; }
            set
            {
                SetValue(ref _list_client, value);
                OnPropertyChanged();
            }
        }

        #region METHOD

        public async Task GetDataBase()
        {
            var queryDatabase = _dbContext.DBApp.Find(1);
            if (queryDatabase == null)
            {
                await AlertConfigureDatabase();
                return;
            }
            else
            {
                FetchData = queryDatabase.NameDatabase;
                URLProyect = queryDatabase.UrlProyect;
                KeyProyect = queryDatabase.KeyProyect;
            }
        }

        public async Task Save_Client()
        {
            if (Valitations() == true)
            {
                switch (FetchData)
                {
                    case "Sqlite":
                        await SaveOnSqlite();
                        break;

                    case "Supabase":
                        await SaveOnSupabase();
                        break;

                    case "Firebase":
                        await SaveOnFirebase();
                        break;

                    case "Web":
                        await SaveOnWeb();
                        break;

                    default:
                        await AlertConfigureDatabase();
                        break;
                }
            }
        }

        public async Task SaveOnSqlite()
        {
            var searchName = _dbContext.Clients.Where(c => c.Name == TextName.ToUpper().Trim());

            if (searchName.Count() > 0)
            {
                await AlertExistRecord();
            }
            else
            {
                var newClient = new MClient { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow, Imagen = _imageClient };

                if (newClient.Name.Length > 15)
                {
                    if (Language == "EN")
                    {
                        await Alerts.ShowAlert("info", "maximum 15 letters or 2 words", "Ok");
                    }
                    else
                    {
                        await Alerts.ShowAlert("info", "maximo 15 letras o 2 palabras", "Ok");
                    }
                    return;
                }
                else
                {
                    _dbContext.Add(newClient);
                    await _dbContext.SaveChangesAsync();
                    await AlertShow();
                    await Load_Data();
                    ResetField();
                }
            }
        }

        public async Task SaveOnSupabase()
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };
            var supabase = new Supabase.Client(URLProyect, KeyProyect, options);
            await supabase.InitializeAsync();

            var newClient = new MClientSupabase { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow };
            await supabase.From<MClientSupabase>().Insert(newClient);
            await AlertShow();
            ResetField();
            await Load_Data();
        }

        public async Task SaveOnFirebase()
        {
            FirebaseClient firebase = new FirebaseClient(Connections.urlFirebase().ToString());

            await firebase.Child("Clients").PostAsync(new MClient()
            {
                Name = TextName.ToUpper().Trim(),
                Saldo_Inicial = TextValor,
                Description = TextDescription,
                Status = _status,
                Fecha = _dateNow
            });

            await AlertShow();
            ResetField();
            await Load_Data();
        }

        public async Task SaveOnWeb()
        {
            var client = new HttpClient();

            var newClient = new MClient { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow };

            var json = JsonConvert.SerializeObject(newClient);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(Connections.urlWebApi() + "/api/ControllerClient", content);

            await AlertShow();
            ResetField();
            await Load_Data();
        }

        public async Task AlertShow()
        {
            if (Language == "EN")
            {
                await Alerts.ShowAlert("Alert", "Added Successfully", "OK");
            }
            else
            {
                await Alerts.ShowAlert("Alerta", "Agregado Con Exito", "OK");
            }
        }

        public async Task AlertExistRecord()
        {
            if (Language == "EN")
            {
                await Alerts.ShowAlert("Alert", "There is already a record with this name", "OK");
            }
            else
            {
                await Alerts.ShowAlert("Alerta", "Ya Existe Un Registro Con Este Nombre", "OK");
            }
        }

        public async Task AlertConfigureDatabase()
        {
            if (Language == "EN")
            {
                await Alerts.ShowAlert("Alert", "You must configure the database", "OK");
            }
            else
            {
                await Alerts.ShowAlert("Alerta", "Debes configurar la base de datos", "OK");
            }
        }

        public async Task AlertNoResult()
        {
            if (Language == "EN")
            {
                await Alerts.ShowAlert("Alert", "No results found", "OK");
            }
            else
            {
                await Alerts.ShowAlert("Alerta", "No se encontraron resultados", "OK");
            }
        }

        public async Task Go_Details(MClient client)
        {
            await Navigation.PushAsync(new Details(client));
        }

        public async Task Load_Data()
        {
            IsBusy = true;
            try
            {
                switch (FetchData)
                {
                    case "Sqlite":
                        await LoadDataSqlite();
                        break;

                    case "Supabase":
                        await LoadDataSupabase();
                        break;

                    case "Firebase":
                        await LoadDataFirebase();
                        break;

                    case "Web":
                        await LoadDataWeb();
                        break;

                    default:
                        await AlertConfigureDatabase();
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadDataSqlite()
        {
            var loadDataSqlite = await _dbContext.Clients.Where(cli => cli.Status == true).ToListAsync();
            List_Client = new ObservableCollection<MClient>(loadDataSqlite);
        }

        public async Task LoadDataSupabase()
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var supabase = new Supabase.Client(URLProyect, KeyProyect, options);

            var loadDataSupabase = await supabase.From<MClientSupabase>().Get();

            List<MClientSupabase> result = loadDataSupabase.Models;

            List_Client = new ObservableCollection<MClient>();

            foreach (var item in result)
            {
                if (item.Status == true)
                {
                    List_Client.Add(new MClient
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Saldo_Inicial = item.Saldo_Inicial,
                        Description = item.Description,
                        Status = item.Status,
                        Fecha = item.Fecha
                    });
                }
            }
        }

        public async Task LoadDataFirebase()
        {
            FirebaseClient firebase = new FirebaseClient(Connections.urlFirebase().ToString());
            var loadDataFirebase = await firebase.Child("Clients").OnceAsync<MClient>();

            List_Client = new ObservableCollection<MClient>();

            foreach (var item in loadDataFirebase)
            {
                if (item.Object.Status == true)
                {
                    List_Client.Add(new MClient
                    {
                        ClientId = item.Key,
                        Name = item.Object.Name,
                        Saldo_Inicial = item.Object.Saldo_Inicial,
                        Description = item.Object.Description,
                        Status = item.Object.Status,
                        Fecha = item.Object.Fecha
                    });
                }
            }
        }

        public async Task LoadDataWeb()
        {
            var fetch = new HttpClient();

            var response = await fetch.GetAsync(Connections.urlWebApi() + "/api/ControllerClient");

            List_Client = new ObservableCollection<MClient>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<MClient>>(json);

                foreach (var item in result)
                {
                    if (item.Status == true)
                    {
                        List_Client.Add(new MClient
                        {
                            ClientId = item.ClientId,
                            Name = item.Name,
                            Saldo_Inicial = item.Saldo_Inicial,
                            Description = item.Description,
                            Status = item.Status,
                            Fecha = item.Fecha
                        });
                    }
                }
            }
            else
            {
                if (Language == "EN")
                {
                    await DisplayAlert("Alert", "Error connecting to the API or No Internet", "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "Error al Conectar con la API o Sin Internet", "OK");
                }
            }
        }

        public async Task getOneClient()
        {
            switch (FetchData)
            {
                case "Sqlite":
                    await LoadOneClientSqlite();
                    break;

                case "Supabase":
                    await LoadOneClientSupabase();
                    break;

                case "Firebase":
                    await LoadOneClientFirebase();
                    break;

                case "Web":
                    await LoadOneClientWeb();
                    break;

                default:
                    await AlertConfigureDatabase();
                    break;
            }
        }

        public async Task LoadOneClientSqlite()
        {
            var searchingOneClient = await _dbContext.Clients
                                .Where(u => u.Name.StartsWith(TextSeaching.ToUpper().Trim()))
                                .ToListAsync();

            if (searchingOneClient.Count > 0)
            {
                List_Client = new ObservableCollection<MClient>(searchingOneClient);
            }
            else
            {
                await AlertNoResult();
            }
        }

        public async Task LoadOneClientSupabase()
        {
            var searchingOneClient = await _dbContext.Clients
                                .Where(u => u.Name.StartsWith(TextSeaching.ToUpper().Trim()))
                                .ToListAsync();

            if (searchingOneClient == null)
            {
                await AlertNoResult();
            }
            else
            {
                List_Client = new ObservableCollection<MClient>(searchingOneClient);
            }
        }

        public async Task LoadOneClientFirebase()
        {
            FirebaseClient firebase = new FirebaseClient(Connections.urlFirebase());

            var searchingOneClient = await firebase.Child("Clients").OrderBy("Name").EqualTo(TextSeaching.ToUpper().Trim()).OnceAsync<MClient>();

            if (searchingOneClient == null)
            {
                await AlertNoResult();
            }
            else
            {
                foreach (var item in searchingOneClient)
                {
                    List_Client.Add(new MClient
                    {
                        ClientId = item.Key,
                        Name = item.Object.Name,
                        Saldo_Inicial = item.Object.Saldo_Inicial,
                        Description = item.Object.Description,
                        Status = item.Object.Status,
                        Fecha = item.Object.Fecha
                    });
                }
            }
        }

        public async Task LoadOneClientWeb()
        {
            var searchingOneClient = await _dbContext.Clients
                                .Where(u => u.Name.StartsWith(TextSeaching.ToUpper().Trim()))
                                .ToListAsync();

            if (searchingOneClient == null)
            {
                await AlertNoResult();
            }
            else
            {
                List_Client = new ObservableCollection<MClient>(searchingOneClient);
            }
        }

        public void ResetField()
        {
            TextName = "";
            TextValor = 0;
            TextDescription = "";
        }

        public bool Valitations()
        {
            if (string.IsNullOrEmpty(TextName))
            {
                if (Language == "EN")
                {
                    DisplayAlert("Error", "You must enter a name", "Ok");
                }
                else
                {
                    DisplayAlert("Error", "Debes ingresar un nombre", "Ok");
                }

                return false;
            }
            else if (TextValor == 0)
            {
                if (Language == "EN")
                {
                    DisplayAlert("Error", "You must enter a value", "Ok");
                }
                else
                {
                    DisplayAlert("Error", "Debes ingresar un valor", "Ok");
                }

                return false;
            }
            else if (string.IsNullOrEmpty(TextDescription))
            {
                if (Language == "EN")
                {
                    DisplayAlert("Error", "You must enter a description", "Ok");
                }
                else
                {
                    DisplayAlert("Error", "Debes ingresar una descripcion", "Ok");
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task OpenConfiguration()
        {
            if (ValidationInternet.IsConnected() == true)
            {
                if (Language == "EN")
                {
                    var result = await DisplayAlert("WordInEnglish", "Do you want to see an ad to unlock the configuration?", "Yes", "No");
                    if (result == true)
                    {
                        MyAds.ShowIntertiscal();
                        await Navigation.PushAsync(new Config());
                    }
                }
                else
                {
                    var result = await DisplayAlert("WordInEnglish", "¿Quieres ver un anuncio para desbloquear la configuración?", "SI", "NO");
                    if (result == true)
                    {
                        MyAds.ShowIntertiscal();
                    }
                }
            }
            else
            {
                await Navigation.PushAsync(new Config());

            }
        }

        public void Change_Language()
        {
            if (Language == "EN")
            {
                Flag = ImageSource.FromFile("flag_EN.png");
                LocalStorange.SetStorange("language", "ES");
                Language_Select();
            }
            else
            {
                Flag = ImageSource.FromFile("flag_ES.png");
                LocalStorange.SetStorange("language", "EN");
                Language_Select();
            }
        }

        public void Language_Select()
        {
            Language = LocalStorange.GetStorange("language");
            if (Language == "EN")
            {
                LabelHeaderName = LanguageApp._labelHeaderNameEN;
                LabelHeaderValue = LanguageApp._labelHeaderValueEN;
                LabelHeaderDate = LanguageApp._labelHeaderDateEN;
                LabelHeaderAction = LanguageApp._labelHeaderActionEN;
                NameLabel = LanguageApp._nameTextEN;
                ValueLabel = LanguageApp._valueTextEN;
                Credit = "Deuda:";
                LabelDate = LanguageApp._dateTextEN;
                LabelHour = LanguageApp._hourTextEN;
                DescriptionLabel = LanguageApp._descriptionTextEN;
                AddClientLabel = LanguageApp._add_clientTextEN;
                SearchClient = LanguageApp._seach_ClientTextEN;
                Flag = ImageSource.FromFile("flag_ES.png");
                LabelDate = LanguageApp._dateTextEN;
                LabelHour = LanguageApp._hourTextEN;
            }
            else
            {
                LabelHeaderName = LanguageApp._labelHeaderNameES;
                LabelHeaderValue = LanguageApp._labelHeaderValueES;
                LabelHeaderDate = LanguageApp._labelHeaderDateES;
                LabelHeaderAction = LanguageApp._labelHeaderActionES;
                NameLabel = LanguageApp._nameTextES;
                ValueLabel = LanguageApp._valueTextES;
                Credit = "Deuda:";
                LabelDate = LanguageApp._dateTextES;
                LabelHour = LanguageApp._hourTextES;
                DescriptionLabel = LanguageApp._descriptionTextES;
                AddClientLabel = LanguageApp._add_clientTextES;
                SearchClient = LanguageApp._seach_ClientTextES;
                Flag = ImageSource.FromFile("flag_EN.png");
                LabelDate = LanguageApp._dateTextES;
                LabelHour = LanguageApp._hourTextES;
            }
        }



        #endregion METHOD

        #region COMMAND

        public ICommand btnSaveData => new Command(async () => await Save_Client());
        public ICommand btn_goDetail => new Command<MClient>(async (cli) => await Go_Details(cli));
        public ICommand btnSearchOneClient => new Command(async () => await getOneClient());
        public ICommand btnLanguage => new Command(Change_Language);
        public ICommand btnConfig => new Command(async () => await OpenConfiguration());

        #endregion COMMAND
    }
}