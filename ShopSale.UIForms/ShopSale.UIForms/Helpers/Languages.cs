namespace ShopSale.UIForms.Helpers
{
	using Interfaces;
	using Resources;
	using Xamarin.Forms;
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public static class Languages
	{
		static Languages()
		{
			var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
			Resource.Culture = ci;
			DependencyService.Get<ILocalize>().SetLocale(ci);
		}

		public static string Accept => Resource.Accept;

		public static string Error => Resource.Error;

		public static string EmailError => Resource.EmailError;

		public static string PasswordError => Resource.PasswordError;

		public static string LoginIncorrect => Resource.LoginIncorrect;

		public static string Login => Resource.Login;

		public static string Email => Resource.Email;

		public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

		public static string Password => Resource.Password;

		public static string Remember => Resource.Remember;

		public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

		public static string RegisterNewUser => Resource.RegisterNewUser;

		public static string FirstName => Resource.FirstName;
		public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;
		public static string LastName => Resource.LastName;
		public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;
		public static string Country => Resource.Country;
		public static string City => Resource.City;
		public static string Address => Resource.Address;
		public static string AddressPlaceHolder => Resource.AddressPlaceHolder;
		public static string TelephonePlaceHolder => Resource.TelephonePlaceHolder;
		public static string Telephone => Resource.Telephone;
		public static string PasswordConfirm => Resource.PasswordConfirm;
		public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;

		public static string ForgotPassword => Resource.ForgotPassword;
		public static string RecoverPassword => Resource.RecoverPassword;
		public static string RecoverEmailPlaceHolder => Resource.RecoverEmailPlaceHolder;

	}
}