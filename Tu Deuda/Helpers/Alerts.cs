using System.Threading.Tasks;

namespace Tu_Deuda.Helpers
{
    public static class Alerts
    {
        public static async Task ShowAlert(string title, string message, string button)
        {
            await App.Current.MainPage.DisplayAlert(title, message, button);
        }
    }
}