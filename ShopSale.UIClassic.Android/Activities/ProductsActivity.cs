namespace ShopSale.UIClassic.Android.Activities
{
    using System.Collections.Generic;
    using Adapters;
    using Common.Models;
    using Common.Services;
    using global::Android.App;
    using global::Android.Content;
    using global::Android.OS;
    using global::Android.Support.V7.App;
    using global::Android.Widget;
    using Helpers;
    using Newtonsoft.Json;

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class ProductsActivity : AppCompatActivity
    {
        private TokenResponse token;
        private string email;
        private ApiService apiService;
        private ListView productsListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ProductsPage);

            this.productsListView = FindViewById<ListView>(Resource.Id.productsListView);

            this.email = Intent.Extras.GetString("email");
            var tokenString = Intent.Extras.GetString("token");
            this.token = JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetListAsync<Product>(
                "https://shopsalewebsite.azurewebsites.net",
                "/api",
                "/Products",
                "bearer",
                this.token.Token);

            if (!response.IsSuccess)
            {
                DiaglogService.ShowMessage(this, "Error", response.Message, "Accept");
                return;
            }

            var products = (List<Product>)response.Result;
            this.productsListView.Adapter = new ProductsListAdapter(this, products);
            this.productsListView.FastScrollEnabled = true;
        }
    }

}