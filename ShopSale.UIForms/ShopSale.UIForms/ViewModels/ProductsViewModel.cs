namespace ShopSale.UIForms.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Common.Models;
    using Common.Services;
    
    public class ProductsViewModel : BaseViewModel
    {
        private ApiService _apiService;
        private List<Product> myProducts;
        private ObservableCollection<ProductItemViewModel> _products;
        private bool isRefreshing;

        public ObservableCollection<ProductItemViewModel> Products
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

            this.myProducts = (List<Product>)response.Result;
            this.RefresProductsList();
        }

        public void AddProductToList(Product product)
        {
            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void UpdateProductInList(Product product)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == product.Id).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void DeleteProductInList(int productId)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == productId).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }
            this.RefresProductsList();
        }

        private void RefresProductsList()
        {
            this.Products = new ObservableCollection<ProductItemViewModel>(this.myProducts.Select(p => new ProductItemViewModel
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ImageFullPath = p.ImageFullPath,
                IsAvailabe = p.IsAvailabe,
                LastPurchase = p.LastPurchase,
                LastSale = p.LastSale,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                User = p.User
            })
            .OrderBy(p => p.Name)
            .ToList());
        }
    }
}