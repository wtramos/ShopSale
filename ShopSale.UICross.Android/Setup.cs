namespace ShopSale.UICross.Android
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common;
    using Common.Interfaces;
    using MvvmCross;
    using MvvmCross.Platforms.Android.Core;
    using Services;
   
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<INetworkProvider, NetworkProvider>();
            base.InitializeFirstChance();
        }

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var assemblies = base.GetPluginAssemblies().ToList();
            assemblies.Add(typeof(MvvmCross.Plugin.Visibility.Platforms.Android.Plugin).Assembly);
            return assemblies;
        }
    }

}