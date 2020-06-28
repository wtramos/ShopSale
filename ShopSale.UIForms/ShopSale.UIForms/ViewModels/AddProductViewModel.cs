namespace ShopSale.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using Common.Models;
    using Common.Services;
    using Plugin.Media;

    public class AddProductViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService _apiService;
        private ImageSource imageSource;
        private MediaFile file;

        public string Image { get; set; }

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

        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }


        public string Name { get; set; }

        public string Price { get; set; }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        public AddProductViewModel()
        {
            this._apiService = new ApiService();
            this.ImageSource = "noImage";
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product price.", "Accept");
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The price must be a number greather than zero.", "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            //TODO: Add image
            var product = new Product
            {
                IsAvailabe = true,
                Name = this.Name,
                Price = price,
                User = new User { UserName = MainViewModel.GetInstance().UserEmail, 
                                  Email = MainViewModel.GetInstance().UserEmail }
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this._apiService.PostAsync(
                url,
                "/api",
                "/Products",
                product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var newProduct = (Product)response.Result;
            MainViewModel.GetInstance().Products.AddProductToList(newProduct);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you take the picture?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }

            if (source == "From Camera")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

    }
}