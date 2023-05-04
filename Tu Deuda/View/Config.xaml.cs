using Tu_Deuda.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tu_Deuda.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Config : ContentPage
    {
        public Config()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new VM_Config(Navigation);
        }
    }
}