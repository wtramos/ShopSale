namespace ShopSale.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using UIForms.Views;
    using Common.Services;
    using Common.Models;
    using Common.Helpers;
    using Newtonsoft.Json;
    using ShopSale.UIForms.Helpers;

    public class LoginViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService _apiService;
        public bool IsRemember { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

        public LoginViewModel()
        {
            this._apiService = new ApiService();
            this.IsEnabled = true;
            this.IsRemember = true;
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, 
                    Languages.EmailError, 
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, 
                    Languages.PasswordError, 
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this._apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.LoginIncorrect,
                    Languages.Accept);
                return;
            }

            var token = (TokenResponse)response.Result;

            var response2 = await this._apiService.GetUserByEmailAsync(
                                url,
                                "/api",
                                "/Account/GetUserByEmail",
                                this.Email,
                                "bearer",
                                token.Token);

            var user = (User)response2.Result;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.User = user;
            mainViewModel.Token = token;
            mainViewModel.UserEmail = this.Email;
            mainViewModel.Products = new ProductsViewModel();

            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Email;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.User = JsonConvert.SerializeObject(user);
            Application.Current.MainPage = new MasterPage();
        }

        public ICommand RegisterCommand => new RelayCommand(this.Register);

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public ICommand RememberPasswordCommand => new RelayCommand(this.RememberPassword);

        private async void RememberPassword()
        {
            MainViewModel.GetInstance().RememberPassword = new RememberPasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RememberPasswordPage());
        }

    }
}
