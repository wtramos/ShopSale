namespace Shop.UIForms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using ShopSale.Common.Models;
    

    public class MainViewModel
    {
        private static MainViewModel _instance;
        public TokenResponse Token { get; set; }
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        public MainViewModel()
        {
            _instance = this;
            this.LoadMenus();
        }
        public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                return new MainViewModel();
            }
            return _instance;
        }
        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_info",
                    PageName = "AboutPage",
                    Title = "About"
                },

                new Menu
                {
                    Icon = "ic_phonelink_setup",
                    PageName = "SetupPage",
                    Title = "Setup"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = "Close session"
                }
            };

            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }

    }
}
