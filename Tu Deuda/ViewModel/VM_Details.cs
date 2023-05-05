using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Data;
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
            receivedClient = client;
            Navigation = navigation;

            Task.Run(async () => await GetDataBase());
            Load_Data();

            var lang = SecureStorage.GetAsync("LANGUAGE").Result;

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
        private string _dateNow = $"Fecha: {_date} - Hora: {_hour} ";

        private string _name;
        private float _deuda;
        private float _abono;
        private float _valueFinal;
        private string _description;
        private string _credito = "CREDITO";
        private string _debito = "DEBITO";
        private string _type;
        private string _fecha;
        private string _color;

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
            get { return _fecha; }
            set
            {
                SetValue(ref _fecha, value);
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
            var url = ConnectionFirebase.GetFirebaseFireStore();
            FirebaseClient firebase = new FirebaseClient(url.ToString());

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

        // crear un metodo para las alerteas

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
            if (await DisplayAlert("info", "Estas Seguro de querer Reseter sus Valores a 0", "yes", "no"))
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
                        await DisplayAlert("Alert", "You must configure the database", "OK");
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

            await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

            Color = "Black";
            Load_Data();
        }

        public async Task DeleteSupabase()
        {
            await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

            Color = "Black";
            Load_Data();
        }

        public async Task DeleteFirebase()
        {
            // update en firebase
            var url = ConnectionFirebase.GetFirebaseFireStore();
            FirebaseClient firebase = new FirebaseClient(url.ToString());

            receivedClient.Status = false;
            receivedClient.Saldo_Inicial = 0;
            receivedClient.Description = "";
            receivedClient.Name = TextName;
            receivedClient.Fecha = _date;

            await firebase
                .Child("Clients")
                .Child(receivedClient.ClientId)
                .PutAsync(receivedClient);

            await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");
            Color = "Black";
            Load_Data();
        }

        public async Task DeleteWeb()
        {
            await DisplayAlert("info", "Listo Ya No Tienes Deuda", "ok");

            Color = "Black";
            Load_Data();
        }

        #region COMMANDS

        public ICommand btnDelete => new Command(async () => await Delete_Credito());
        public ICommand btngoHome => new Command(async () => await Back_Home());
        public ICommand btnSave_Data => new Command(async () => await SaveData());

        #endregion COMMANDS
    }
}