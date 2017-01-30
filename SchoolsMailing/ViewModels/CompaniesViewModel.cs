using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.ViewModels.Common;
using SchoolsMailing.Models;
using Windows.Storage;
using System.IO;
using GalaSoft.MvvmLight.Command;
using SchoolsMailing.DAL;
using SchoolsMailing.Views;
using System.Linq;

namespace SchoolsMailing.ViewModels
{
    public class CompaniesViewModel : PageViewModel
    {
        //Get online table
        //private IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public CompaniesViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //initialiseCompanies();
        }

        //private MobileServiceCollection<Company, Company> _companies;
        //public MobileServiceCollection<Company, Company> companies
        //{
        //    get
        //    {
        //        return _companies;
        //    }
        //    set
        //    {
        //        if (_companies != value)
        //        {
        //            _companies = value;
        //            //Refresh binding
        //            RaisePropertyChanged("companies");
        //        }
        //    }
        //}

        //private async Task initialiseCompanies()
        //{
        //    MobileServiceInvalidOperationException exception = null;
        //    try
        //    {
        //        //Turn azure table to bindable collection
        //        companies = await companyTable
        //                      .ToCollectionAsync();
        //    }
        //    catch (MobileServiceInvalidOperationException e)
        //    {
        //        exception = e;
        //    }
        //}
        
        private List<Company> _companies;
        public List<Company> companies
        {
            get
            {
                _companies = DataAccessLayer.getCompanies();
                //companies.GroupBy()
                _companies.GroupBy(x => x.companyInitial);
                return _companies;
            }
        }

        private static string dbPath = string.Empty;
        private static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    //Get db path
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Sales.sqlite");
                }
                return dbPath;
            }
        }

        private RelayCommand _newCompany;
        public RelayCommand newCompany
        {
            get
            {
                if (_newCompany == null)
                {
                    _newCompany = new RelayCommand(() =>
                    {
                        NavigationService.Navigate(typeof(NewCompanyView));
                    });
                }
                return _newCompany;
            }
        }

        //private RelayCommand _refreshCompany;
        //public RelayCommand refreshCompany
        //{
        //    get
        //    {
        //        if (_refreshCompany == null)
        //        {
        //            _refreshCompany = new RelayCommand(async () =>
        //            {
        //                await initialiseCompanies();
        //            });
        //        }

        //        return _refreshCompany;

        //    }
        //}
        
        //Handle company clicked event
        public void companyInvoked(object sender, object parameter)
        {
            //Get any parameters
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get clicked company
            var company = arg.ClickedItem as Company;
            //Get clicked company ID
            string _companyID = company.ID.ToString();
            //Navigate to CompanyView
            this.NavigationService.Navigate(typeof(CompanyView));
            //Pass ID parameter
            MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<Int64>(company.ID, "CompanyViewModel"));
        }

        //public ObservableCollection<Company> GetAllCompanies()
        //{
        //    List<Company> c;

        //    // Create a new connection
        //    using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
        //    {
        //        c = (from p in db.Table<Company>()
        //             select p).ToList();
        //    }
        //    ObservableCollection<Company> model = new ObservableCollection<Company>(c);
        //    return model;
        //}
    }
}
