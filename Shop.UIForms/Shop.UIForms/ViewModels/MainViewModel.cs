namespace Shop.UIForms.ViewModels
{
    using ShopSale.Common.Models;
    public class MainViewModel
    {
        private static MainViewModel _instance;
        public TokenResponse Token { get; set; }
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public MainViewModel()
        {
            _instance = this;
        }
        public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                return new MainViewModel();
            }
            return _instance;
        }
    }
}
