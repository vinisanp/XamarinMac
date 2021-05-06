using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SDMobileXF.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //int themeId = Resource.Style.Theme_Splash;
            //switch (App.__nivelDeProjeto)
            //{
            //    case "CSN":
            //        themeId = Resource.Style.Theme_SplashCsn;
            //        break;
            //    case "Suzano":
            //        themeId = Resource.Style.Theme_Splash;
            //        break;
            //}
            //SetTheme(themeId);

            base.OnCreate(savedInstanceState);

            //ScheduleSplashScreen();
            StartActivity(typeof(MainActivity));
        }

        private void ScheduleSplashScreen()
        {
            var splashScreenDuration = 2000;
            Handler handler = new Handler();
            handler.PostDelayed(() =>
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }, splashScreenDuration);

        }
    }
}