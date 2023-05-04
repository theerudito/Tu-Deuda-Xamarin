using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Tu_Deuda.ViewModel
{
    public class VM_Config : BaseVM
    {
        public VM_Config(INavigation navigation)
        {
            Navigation = navigation;
        }

        #region Properties

        private int codeClient = 1;
        private string urlProyect;
        private string urlKeyProyect;
        private string _dataBase;

        #endregion Properties

        #region Objects

        public int CodeClient
        {
            get { return codeClient; }
            set { codeClient = value; }
        }

        public string UrlProyect
        {
            get { return urlProyect; }
            set { urlProyect = value; }
        }

        public string UrlKeyProyect
        {
            get { return urlKeyProyect; }
            set { urlKeyProyect = value; }
        }

        public string DataBase
        {
            get { return _dataBase; }
            set { _dataBase = value; }
        }

        #endregion Objects

        #region Metthods

        public async Task SaveConfig()
        {
            await DisplayAlert("Configuración", "Configuración guardada", "OK");
        }

        #endregion Metthods

        #region Commands

        public ICommand btnSaveConfig => new Command(async () => await SaveConfig());

        #endregion Commands
    }
}