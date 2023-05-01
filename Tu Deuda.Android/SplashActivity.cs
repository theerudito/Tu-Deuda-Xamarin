using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;


namespace Tu_Deuda.Droid
{
    [Activity(Label = "Tu Deuda", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}