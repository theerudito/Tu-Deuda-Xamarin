using Tu_Deuda.Model;
using Tu_Deuda.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tu_Deuda.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Details : ContentPage
    {
        public Details(Client client)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new VM_Details(Navigation, client);
        }
    }
}