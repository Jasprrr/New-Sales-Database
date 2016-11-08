namespace SchoolsMailing.ViewModels.Common
{
    using SchoolsMailing.Common;

    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;
    using SchoolsMailing.ViewModels;

    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            RegisterServiceProviders();
            RegisterViewModels();
        }

        /// <summary>
        /// Gets the main page view model.
        /// </summary>
        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();
        public HomePageViewModel HomePageViewModel => SimpleIoc.Default.GetInstance<HomePageViewModel>();
        public CompaniesViewModel CompaniesViewModel => SimpleIoc.Default.GetInstance<CompaniesViewModel>();
        public NewCompanyViewModel NewCompanyViewModel => SimpleIoc.Default.GetInstance<NewCompanyViewModel>();
        public CompanyViewModel CompanyViewModel => SimpleIoc.Default.GetInstance<CompanyViewModel>();
        //this, name=> { NameProperty = name}
        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<HomePageViewModel>();
            SimpleIoc.Default.Register<NewCompanyViewModel>();
            SimpleIoc.Default.Register<CompaniesViewModel>();
            SimpleIoc.Default.Register<CompanyViewModel>();
        }

        private static void RegisterServiceProviders()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register<NavigationService>();
        }
    }
}
