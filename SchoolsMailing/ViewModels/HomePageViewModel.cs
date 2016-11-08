using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using System.Windows.Input;
using SchoolsMailing.Views;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using System.Diagnostics;
using SchoolsMailing.Models;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;

namespace SchoolsMailing.ViewModels
{
    public class HomePageViewModel : PageViewModel
    {
        //static IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public HomePageViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
       }

        private RelayCommand _getEvents;
        public RelayCommand getEvents
        {
            get
            {
                if (_getEvents == null)
                {
                    _getEvents = new RelayCommand(() =>
                    {

                    });
                }

                return _getEvents;

            }
        }
    }
}
