using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SchoolsMailing.ViewModels
{
    public class Orders : PageViewModel
    {
        //public static IMobileServiceTable<Company> allCompanies = App.MobileService.GetTable<Company>();
        //private MobileServiceCollection<Orders, Orders> items;
        //private IMobileServiceTable<Orders> companyTable = App.MobileService.GetTable<Orders>();

        public Orders(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //allCompanies = App.MobileService.GetTable<Company>().ToCollectionAsync;
            //getData();
        }

        private ObservableCollection<Orders> _allItems;
        public ObservableCollection<Orders> allItems
        {
            get
            {
                return _allItems;
            }
            set
            {
                if(_allItems != value)
                {
                    _allItems = value;
                }
            }
        }
        //private async Task getData()
        //{

        //    MobileServiceInvalidOperationException exception = null;
        //    try
        //    {
        //        allItems = await companyTable
        //            .ToCollectionAsync();
        //    }
        //    catch (MobileServiceInvalidOperationException e)
        //    {
        //        exception = e;
        //    }

        //    if (exception != null)
        //    {
        //        //await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
        //        Debug.Write("error has occured");
        //        Debug.Write(exception);
        //    }
        //    else
        //    {
        //        //this.ButtonSave.IsEnabled = true;
        //    }

            
        //}
    }
}
