using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_Config : BaseVM
    {
        public VM_Config(INavigation navigation)
        {
            Navigation = navigation;

            BtnSaveConfig = false;

            getConfig();

            ChangeDataBase();
        }

        #region Properties

        private string urlProyect;
        private string urlKeyProyect;
        private string _dataBase;

        private bool _isVisibleFrameUrlProyect;
        private bool _isVisibleFrameUrlKeyProyect;

        private bool _btnSaveConfig;
        private int _codeAdmin;

        private string Sqlite = "Sqlite";
        private string Web = "Web";
        private string Firebase = "Firebase";
        private string Supabase = "Supabase";

        #endregion Properties

        #region Objects

        public string UrlProyect
        {
            get { return urlProyect; }
            set { urlProyect = value; }
        }

        public string KeyProyect
        {
            get { return urlKeyProyect; }
            set { urlKeyProyect = value; }
        }

        public string DataBase
        {
            get { return _dataBase; }
            set
            {
                _dataBase = value;
                ChangeDataBase();
                OnPropertyChanged();
            }
        }

        public string SelectDataBase
        {
            get { return _dataBase; }
            set
            {
                SetProperty(ref _dataBase, value);
                DataBase = _dataBase;
            }
        }

        public bool IsVisibleFrameUrlProyect
        {
            get { return _isVisibleFrameUrlProyect; }
            set { SetProperty(ref _isVisibleFrameUrlProyect, value); }
        }

        public bool IsVisibleFrameUrlKeyProyect
        {
            get { return _isVisibleFrameUrlKeyProyect; }
            set { SetProperty(ref _isVisibleFrameUrlKeyProyect, value); }
        }

        public bool BtnSaveConfig
        {
            get { return _btnSaveConfig; }
            set { SetProperty(ref _btnSaveConfig, value); }
        }

        public int CodeAdmin
        {
            get { return _codeAdmin; }
            set { SetProperty(ref _codeAdmin, value); }
        }

        private int idDataBase = 1;

        #endregion Objects

        #region Metthods

        public async Task getConfig()
        {
            var _dbCcontext = new Application_Context();

            var searchDatabase = await _dbCcontext.DBApp.Where(c => c.Id == idDataBase).FirstOrDefaultAsync();

            if (searchDatabase != null)
            {
                SelectDataBase = searchDatabase.NameDatabase;
                UrlProyect = searchDatabase.UrlProyect;
                KeyProyect = searchDatabase.KeyProyect;
            }
        }

        public void ChangeDataBase()
        {
            if (Sqlite == SelectDataBase)
            {
                IsVisibleFrameUrlProyect = false;
                IsVisibleFrameUrlKeyProyect = false;
            }
            else if (Web == SelectDataBase)
            {
                IsVisibleFrameUrlProyect = true;
                IsVisibleFrameUrlKeyProyect = false;
            }
            else if (Firebase == SelectDataBase)
            {
                IsVisibleFrameUrlProyect = true;
                IsVisibleFrameUrlKeyProyect = false;
            }
            else if (Supabase == SelectDataBase)
            {
                IsVisibleFrameUrlProyect = true;
                IsVisibleFrameUrlKeyProyect = true;
            }
        }

        public async Task SaveConfig()
        {
            var _dbCcontext = new Application_Context();

            var searchDatabase = await _dbCcontext.DBApp.Where(c => c.Id == idDataBase).FirstOrDefaultAsync();

            if (searchDatabase != null)
            {
                searchDatabase.NameDatabase = SelectDataBase;
                searchDatabase.UrlProyect = UrlProyect;
                searchDatabase.KeyProyect = KeyProyect;
                _dbCcontext.Update(searchDatabase);
                await _dbCcontext.SaveChangesAsync();

                await DisplayAlert("info", "Updated Successfully", "ok");

                BtnSaveConfig = false;
            }
        }

        public async Task MyCode()
        {
            var _dbCcontext = new Application_Context();

            var queryCode = await _dbCcontext.Code_App.Where(c => c.CodeAdmin == CodeAdmin).FirstOrDefaultAsync();

            if (queryCode != null)
            {
                await DisplayAlert("info", "Code Correct", "ok");
                BtnSaveConfig = true;
                CodeAdmin = 0;
            }
            else
            {
                await DisplayAlert("info", "Code Incorrect", "ok");
            }
        }

        #endregion Metthods

        #region Commands

        public ICommand btnSaveConfig => new Command(async () => await SaveConfig());
        public ICommand btnCodeConfig => new Command(async () => await MyCode());

        #endregion Commands
    }
}