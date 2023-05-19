using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tu_Deuda.ApplicationDB;
using Tu_Deuda.Helpers;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_Config : BaseVM
    {
        public VM_Config(INavigation navigation)
        {
            Language = LocalStorange.GetStorange("language");

            Navigation = navigation;

            BtnSaveConfig = false;

            if (Language == "EN")
            {
                LoadLanguage();
            }
            else
            {
                LoadLanguage();
            }

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


        #region Language
        private string _pickerTitle;
        private string _placeholderUrl;
        private string _placeholderKey;
        private string _entryCode;
        private string _textBtnSave;
        private string _language;

        public string PickerTitle
        {
            get { return _pickerTitle; }
            set { SetProperty(ref _pickerTitle, value); }
        }

        public string PlaceholderUrl
        {
            get { return _placeholderUrl; }
            set { SetProperty(ref _placeholderUrl, value); }
        }

        public string PlaceholderKey
        {
            get { return _placeholderKey; }
            set { SetProperty(ref _placeholderKey, value); }
        }

        public string EntryCode
        {
            get { return _entryCode; }
            set { SetProperty(ref _entryCode, value); }
        }

        public string TextBtnSave
        {
            get { return _textBtnSave; }
            set { SetProperty(ref _textBtnSave, value); }
        }
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }
        #endregion Language

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
                BtnSaveConfig = false;

                if (Language == "EN")
                {
                    await Alerts.ShowAlert("info", "Updated Successfully", "ok");
                }
                else
                {
                    await Alerts.ShowAlert("info", "Actualizado Correctamente", "ok");
                }
            }
        }

        public void LoadLanguage()
        {
            if (Language == "EN")
            {
                PickerTitle = LanguageApp._selectDataBaseEN;
                PlaceholderUrl = LanguageApp._entryUrlProyectEN;
                PlaceholderKey = LanguageApp._entryKeyProyectEN;
                EntryCode = LanguageApp._entryCodePlaceholderEN;
                TextBtnSave = LanguageApp._btnTextEN;
            }
            else
            {
                PickerTitle = LanguageApp._selectDataBaseES;
                PlaceholderUrl = LanguageApp._entryUrlProyectES;
                PlaceholderKey = LanguageApp._entryKeyProyectES;
                EntryCode = LanguageApp._entryCodePlaceholderES;
                TextBtnSave = LanguageApp._btnTextES;
            }

        }

        public async Task MyCode()
        {
            var _dbCcontext = new Application_Context();

            var queryCode = await _dbCcontext.Code_App.Where(c => c.CodeAdmin == CodeAdmin).FirstOrDefaultAsync();

            if (queryCode != null)
            {
                if (Language == "EN")
                {
                    await Alerts.ShowAlert("info", "Code Correct", "ok");
                    BtnSaveConfig = true;
                    CodeAdmin = 0;
                }
                else
                {
                    await Alerts.ShowAlert("info", "Codigo Correcto", "ok");
                    BtnSaveConfig = true;
                    CodeAdmin = 0;
                }
            }
            else
            {
                if (Language == "EN")
                {
                    await Alerts.ShowAlert("info", "Code Incorrect", "ok");
                }
                else
                {
                    await Alerts.ShowAlert("info", "Codigo Incorrecto", "ok");
                }
            }
        }

        #endregion Metthods

        #region Commands

        public ICommand btnSaveConfig => new Command(async () => await SaveConfig());
        public ICommand btnCodeConfig => new Command(async () => await MyCode());

        #endregion Commands
    }
}