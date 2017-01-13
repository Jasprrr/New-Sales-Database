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
using Windows.UI.Xaml.Controls;
using Windows.UI;
using SchoolsMailing.DAL;

namespace SchoolsMailing.ViewModels
{
    public class HomePageViewModel : PageViewModel
    {
        //static IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public HomePageViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
        }

        public DateTime selectedDate;

        public void dateInvoked(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.CalendarViewSelectedDatesChangedEventArgs;
            DateTimeOffset selectedDateOffset = arg.AddedDates.FirstOrDefault();
            selectedDate = selectedDateOffset.DateTime.Date;
            if(selectedDate != null)
            {
                GetOrders(selectedDate);
            }
        }

        public void GetOrders(DateTime date)
        {
            emailOrders = DataAccessLayer.GetOrdersEmailByDate(date);
            schoolSendOrders = DataAccessLayer.GetOrdersSchoolSendByDate(date);
            directMailingOrders = DataAccessLayer.GetOrdersDirectMailingByDate(date);
            sharedMailingOrders = DataAccessLayer.GetOrdersSharedMailingByDate(date);
        }

        private bool _paneOpen = false;
        public bool paneOpen
        {
            get { return _paneOpen; }
            set { _paneOpen = !_paneOpen; RaisePropertyChanged("paneOpen"); }
        }

        private List<OrdersEmail> _emailOrders = new List<OrdersEmail>();
        public List<OrdersEmail> emailOrders
        {
            get { return _emailOrders; }
            set { if (_emailOrders != value) { _emailOrders = value; RaisePropertyChanged("emailOrders"); } }
        }
        private List<OrdersSchoolSend> _schoolSendOrders = new List<OrdersSchoolSend>();
        public List<OrdersSchoolSend> schoolSendOrders
        {
            get { return _schoolSendOrders; }
            set { if (_schoolSendOrders != value) { _schoolSendOrders = value; RaisePropertyChanged("schoolSendOrders"); } }
        }
        private List<OrdersDirectMailing> _directMailingOrders = new List<OrdersDirectMailing>();
        public List<OrdersDirectMailing> directMailingOrders
        {
            get { return _directMailingOrders; }
            set { if (_directMailingOrders != value) { _directMailingOrders = value; RaisePropertyChanged("directMailingOrders"); } }
        }
        private List<OrdersSharedMailing> _sharedMailingOrders = new List<OrdersSharedMailing>();
        public List<OrdersSharedMailing> sharedMailingOrders
        {
            get { return _sharedMailingOrders; }
            set { if (_sharedMailingOrders != value) { _sharedMailingOrders = value; RaisePropertyChanged("sharedMailingOrders"); } }
        }

        public void orderPartInvoked(object sender, object parameter)
        {
            long orderID = 0;
            //Get parameter (order part)
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get clicked order part
            var item = arg.ClickedItem;
            //Get the type of order
            Type t = item.GetType();
            //Assign correct conversion
            if (t.Equals(typeof(OrdersData)))
            {
                var order = arg.ClickedItem as OrdersData;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersEmail)))
            {
                var order = arg.ClickedItem as OrdersEmail;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersSchoolSend)))
            {
                var order = arg.ClickedItem as OrdersSchoolSend;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersDirectMailing)))
            {
                var order = arg.ClickedItem as OrdersDirectMailing;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersSharedMailing)))
            {
                var order = arg.ClickedItem as OrdersSharedMailing;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersPrint)))
            {
                var order = arg.ClickedItem as OrdersPrint;
                orderID = order.ID;
            }
            if (t.Equals(typeof(OrdersSurcharge)))
            {
                var order = arg.ClickedItem as OrdersSurcharge;
                orderID = order.ID;
            }

            //Check if order ID is valid
            if (orderID != 0)
            {
                //Navigate to CompanyView
                this.NavigationService.Navigate(typeof(OrderView));
                //Pass ID parameter
                MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<Int64>(orderID, "OrderViewModel"));
            }

        }
    }
}
