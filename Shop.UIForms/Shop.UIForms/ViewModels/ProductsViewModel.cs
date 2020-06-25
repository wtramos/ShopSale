﻿namespace Shop.UIForms.ViewModels
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
        private bool isRefreshing;

        public ObservableCollection<Product> Products
        {
            get { return this._products; }
            set { this.SetValue(ref this._products, value); }
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public ProductsViewModel()
        {
            this._apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this._apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRefreshing = false;

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