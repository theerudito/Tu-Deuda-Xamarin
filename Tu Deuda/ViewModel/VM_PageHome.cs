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
using Tu_Deuda.Model;
using Tu_Deuda.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_PageHome : BaseVM
    {
        private Application_Context _dbContext = new Application_Context();
        private Language language = new Language();
        public Command LoadItemsCommand { get; }

        public VM_PageHome(INavigation navigation)
        {
            Navigation = navigation;

            Task.Run(async () => await GetDataBase());

            Task.Run(async () => await Load_Data());

            TextLanguage = "ES";
            SecureStorage.SetAsync("LANGUAGE", TextLanguage);

            var lang = SecureStorage.GetAsync("LANGUAGE").Result;

            if (lang != null)
            {
                Language_ES();
            }

            LoadItemsCommand = new Command(async () => await Load_Data());
        }

        private ObservableCollection<MClient> _list_client;
        private ObservableCollection<MClientSupabase> _list_clientSupabase;

        private static string _hour = DateTime.Now.ToString("HH:mm");
        private static string _date = DateTime.Now.ToString("dd/MM/yyyy");
        private string _dateNow = $"Fecha {_date} - Hour {_hour} ";
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

        public string TextLanguage
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
        private static string hour;
        private static string _fecha;

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

        public string Date
        {
            get { return _fecha; }
            set
            {
                SetValue(ref _fecha, value);
                OnPropertyChanged();
            }
        }

        public string Hour
        {
            get { return hour; }
            set
            {
                SetValue(ref hour, value);
                OnPropertyChanged();
            }
        }

        #endregion Language

        public ObservableCollection<MClient> List_Client
        {
            get { return _list_client; }
            set
            {
                _list_client = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MClientSupabase> List_ClientSupabase
        {
            get { return _list_clientSupabase; }
            set
            {
                _list_clientSupabase = value;
                OnPropertyChanged();
            }
        }

        #region METHOD

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
                        await DisplayAlert("Alert", "You must configure the database", "OK");
                        break;
                }
            }
        }

        public async Task SaveOnSqlite()
        {
            var searchName = _dbContext.Clients.Where(c => c.Name == TextName.ToUpper().Trim());

            if (searchName.Count() > 0)
            {
                await DisplayAlert("info", "Ya Existe Un Registro Con Este Nombre", "ok");
            }
            else
            {
                var newClient = new MClient { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow, Imagen = _imageClient };

                _dbContext.Add(newClient);

                await _dbContext.SaveChangesAsync();

                await DisplayAlert("info", "Dato Guargado Con Exito", "ok");

                await Load_Data();

                ResetField();
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

            // mappear la tabla

            var newClient = new MClientSupabase { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow };
            await supabase.From<MClientSupabase>().Insert(newClient);
            await DisplayAlert("Alert", "Added Successfully", "OK");
            ResetField();
            await Load_Data();
        }

        public async Task SaveOnFirebase()
        {
            var url = ConnectionFirebase.GetFirebaseFireStore();

            FirebaseClient firebase = new FirebaseClient(url.ToString());

            await firebase.Child("Clients").PostAsync(new MClient()
            {
                Name = TextName.ToUpper().Trim(),
                Saldo_Inicial = TextValor,
                Description = TextDescription,
                Status = _status,
                Fecha = _dateNow
            });

            await DisplayAlert("Alert", "Added Successfully", "OK");
            ResetField();
            await Load_Data();
        }

        public async Task SaveOnWeb()
        {
            var url = ConnectionWeb.UrlWeb();

            var client = new HttpClient();

            var newClient = new MClient { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow };

            var json = JsonConvert.SerializeObject(newClient);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(url + "/api/ControllerClient", content);

            await DisplayAlert("Alert", "Added Successfully", "OK");
            ResetField();
            await Load_Data();
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
                        await DisplayAlert("Alert", "You must configure the database", "OK");
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
            await GetDataBase();
        }

        public async Task LoadDataSupabase()
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };
            var supabase = new Supabase.Client(URLProyect, KeyProyect, options);
            await supabase.InitializeAsync();

            var loadDataSupabase = await supabase.From<MClientSupabase>().Get();
            var resul = loadDataSupabase.Models;

            List_ClientSupabase = new ObservableCollection<MClientSupabase>();

            foreach (var item in resul)
            {
                if (item.Status == true)
                {
                    List_ClientSupabase.Add(new MClientSupabase
                    {
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
            var url = ConnectionFirebase.GetFirebaseFireStore();
            FirebaseClient firebase = new FirebaseClient(url.ToString());
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
            await GetDataBase();
        }

        public async Task LoadDataWeb()
        {
            var url = ConnectionWeb.UrlWeb();

            var client = new HttpClient();

            var response = await client.GetStringAsync(url + "/api/ControllerClient");

            var loadDataWeb = JsonConvert.DeserializeObject<List<MClient>>(response);

            List_Client = new ObservableCollection<MClient>();

            if (loadDataWeb != null)
            {
                foreach (var item in loadDataWeb)
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
                await DisplayAlert("Alert", "No se encontraron datos", "OK");
            }
            await GetDataBase();
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
            }
        }

        public async Task LoadOneClientSqlite()
        {
            var searchingOneClient = await _dbContext.Clients
                                .Where(u => u.Name.StartsWith(TextSeaching.ToUpper().Trim()))
                                .ToListAsync();

            if (searchingOneClient == null)
            {
                await DisplayAlert("info", "No se encontraron resultados", "OK");
            }
            else
            {
                List_Client = new ObservableCollection<MClient>(searchingOneClient);
            }
        }

        public async Task LoadOneClientSupabase()
        {
            var searchingOneClient = await _dbContext.Clients
                                .Where(u => u.Name.StartsWith(TextSeaching.ToUpper().Trim()))
                                .ToListAsync();

            if (searchingOneClient == null)
            {
                await DisplayAlert("info", "No se encontraron resultados", "OK");
            }
            else
            {
                List_Client = new ObservableCollection<MClient>(searchingOneClient);
            }
        }

        public async Task LoadOneClientFirebase()
        {
            var url = ConnectionFirebase.GetFirebaseFireStore();
            FirebaseClient firebase = new FirebaseClient(url.ToString());

            var searchingOneClient = await firebase.Child("Clients").OrderBy("Name").EqualTo(TextSeaching.ToUpper().Trim()).OnceAsync<MClient>();

            if (searchingOneClient == null)
            {
                await DisplayAlert("info", "No se encontraron resultados", "OK");
            }
            else
            {
                List_Client = new ObservableCollection<MClient>();

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
                await DisplayAlert("info", "No se encontraron resultados", "OK");
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
                DisplayAlert("Error", "Debes ingresar un nombre", "Ok");
                return false;
            }
            else if (TextValor == 0)
            {
                DisplayAlert("Error", "Debes ingresar un valor", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(TextDescription))
            {
                DisplayAlert("Error", "Debes ingresar una descripcion", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        public void OpenConfiguration()
        {
            Navigation.PushAsync(new Config());
        }

        public void Change_Language()
        {
            var lang = SecureStorage.GetAsync("LANGUAGE").Result;

            if (lang == TextLanguage)
            {
                SecureStorage.SetAsync("LANGUAGE", TextLanguage);
                Language_ES();
            }
            else
            {
                SecureStorage.SetAsync("LANGUAGE", TextLanguage);
                Language_EN();
            }
        }

        public void Language_ES()
        {
            var es = new Language();
            TextLanguage = "ES";
            NameLabel = es.Name = "Nombre:";
            ValueLabel = es.Value = "Valor:";
            Credit = es.Credit = "Deuda:";
            Date = es.Date = "Fecha:";
            Hour = es.Hour = "Hora:";
            DescriptionLabel = es.Description = "Descripcion:";
            AddClientLabel = es.AddClient = "Añadir Cliente";
            SearchClient = es.SearchClient = "Buscar Cliente";
            Flag = ImageSource.FromFile("flag_EN.png");
        }

        public void Language_EN()
        {
            var en = new Language();
            TextLanguage = "EN";
            NameLabel = en.Name = "Name:";
            ValueLabel = en.Value = "Value:";
            Credit = en.Credit = "Saldo:";
            Date = en.Date = "Date:";
            Hour = en.Hour = "Hour:";
            DescriptionLabel = en.Description = "Description:";
            AddClientLabel = en.AddClient = "Add Client";
            SearchClient = en.SearchClient = "Search Client";
            Flag = ImageSource.FromFile("flag_ES.png");
        }

        #endregion METHOD

        public ICommand btnSaveData => new Command(async () => await Save_Client());
        public ICommand btn_goDetail => new Command<MClient>(async (cli) => await Go_Details(cli));
        public ICommand btnSearchOneClient => new Command(async () => await getOneClient());
        public ICommand btnLanguage => new Command(Change_Language);
        public ICommand btnConfig => new Command(OpenConfiguration);
    }
}