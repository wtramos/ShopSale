namespace Shop.UIForms.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using ShopSale.Common.Models;
    using ShopSale.Common.Services;
    using System.Collections.Generic;

    public class ProductsViewModel : BaseViewModel
    {
        private ApiService _apiService;
        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return this._products; }
            set { this.SetValue(ref this._products, value); }
        }

        public ProductsViewModel()
        {
            this._apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this._apiService.GetListAsync<Product>(
                "https://shopsalewebsite.azurewebsites.net",
                "/api",
                "/Products");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts);

        }
    }
}