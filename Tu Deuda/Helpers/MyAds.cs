using MarcTron.Plugin;

namespace Tu_Deuda.Helpers
{
    public static class MyAds
    {
        public static void ShowIntertiscal()
        {
            var idIntersticial = "ca-app-pub-7633493507240683/8015778047";

            CrossMTAdmob.Current.LoadInterstitial(idIntersticial);
        }

        public static bool IsIntertiscalLoaded()
        {
            return CrossMTAdmob.Current.IsInterstitialLoaded() ? true : false;
        }

        public static void ShowRewardedVideo()
        {
            var idVideo = "ca-app-pub-7633493507240683/6124136868";

            CrossMTAdmob.Current.LoadRewardedVideo(idVideo);
        }

        public static bool IsVideoLoaded()
        {
            return CrossMTAdmob.Current.IsRewardedVideoLoaded();
        }

    }
}
