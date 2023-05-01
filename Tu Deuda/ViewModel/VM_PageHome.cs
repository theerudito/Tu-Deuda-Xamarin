using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
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

        private ObservableCollection<Client> _list_client;

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

        public ObservableCollection<Client> List_Client
        {
            get { return _list_client; }
            set
            {
                _list_client = value;
                OnPropertyChanged();
            }
        }

        #region METHOD.

        public async Task Save_Client()
        {
            if (Valitations() == true)
            {
                var searchName = _dbContext.Clients.Where(c => c.Name == TextName.ToUpper().Trim());

                if (searchName.Count() > 0)
                {
                    await DisplayAlert("info", "Ya Existe Un Registro Con Este Nombre", "ok");
                }
                else
                {
                    var newClient = new Client { Name = TextName.ToUpper().Trim(), Saldo_Inicial = TextValor, Description = TextDescription, Status = _status, Fecha = _dateNow, Imagen = _imageClient };

                    _dbContext.Add(newClient);

                    await _dbContext.SaveChangesAsync();

                    await DisplayAlert("info", "Dato Guargado Con Exito", "ok");

                    await Load_Data();

                    ResetField();
                }
            }
        }

        public async Task Go_Details(Client client)
        {
            await Navigation.PushAsync(new Details(client));
        }

        public async Task Load_Data()
        {
            IsBusy = true;
            try
            {
                var loadData = await _dbContext.Clients.Where(cli => cli.Status == true).ToListAsync();

                List_Client = new ObservableCollection<Client>(loadData);
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

        public async Task getOneClient()
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
                List_Client = new ObservableCollection<Client>(searchingOneClient);
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

        #endregion METHOD.

        public ICommand btnSaveData => new Command(async () => await Save_Client());
        public ICommand btn_goDetail => new Command<Client>(async (cli) => await Go_Details(cli));
        public ICommand btnSearchOneClient => new Command(async () => await getOneClient());
        public ICommand btnLanguage => new Command(Change_Language);
        public ICommand btnConfig => new Command(OpenConfiguration);
    }
}