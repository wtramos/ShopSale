namespace FourWays.Core
{
    using Core.ViewModels;
    using MvvmCross.IoC;
    using MvvmCross.ViewModels;

    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<TipViewModel>();
        }
    }

}
