namespace FourWays.FormsTraditional
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            this.MainPage = new NavigationPage(new TipPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
