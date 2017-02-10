using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.DAL;
using SchoolsMailing.Models;
using SchoolsMailing.ViewModels.Common;
using SchoolsMailing.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.ViewModels
{
    public class OrdersViewModel : PageViewModel
    {
        #region Variables
        private List<OrdersData> _dataByOrder;
        public List<OrdersData> dataByOrder
        {
            get { return _dataByOrder; }
            set { if (_dataByOrder != value) { _dataByOrder = value; RaisePropertyChanged("dataByOrder"); } }
        }
        private List<OrdersEmail> _emailByOrder;
        public List<OrdersEmail> emailByOrder
        {
            get { return _emailByOrder; }
            set { if (_emailByOrder != value) { _emailByOrder = value; RaisePropertyChanged("emailByOrder"); } }
        }
        private List<OrdersSchoolSend> _schoolsendByOrder;
        public List<OrdersSchoolSend> schoolsendByOrder
        {
            get { return _schoolsendByOrder; }
            set { if (_schoolsendByOrder != value) { _schoolsendByOrder = value; RaisePropertyChanged("schoolsendByOrder"); } }
        }
        private List<OrdersDirectMailing> _directMailingByOrder;
        public List<OrdersDirectMailing> directMailingByOrder
        {
            get { return _directMailingByOrder; }
            set { if (_directMailingByOrder != value) { _directMailingByOrder = value; RaisePropertyChanged("directMailingByOrder"); } }
        }
        private List<OrdersSharedMailing> _sharedMailingByOrder;
        public List<OrdersSharedMailing> sharedMailingByOrder
        {
            get { return _sharedMailingByOrder; }
            set { if (_sharedMailingByOrder != value) { _sharedMailingByOrder = value; RaisePropertyChanged("sharedMailingByOrder"); } }
        }
        private List<OrdersPrint> _printByOrder;
        public List<OrdersPrint> printByOrder
        {
            get { return _printByOrder; }
            set { if (_printByOrder != value) { _printByOrder = value; RaisePropertyChanged("printByOrder"); } }
        }
        private List<OrdersSurcharge> _surchargeByOrder;
        public List<OrdersSurcharge> surchargeByOrder
        {
            get { return _surchargeByOrder; }
            set { if (_surchargeByOrder != value) { _surchargeByOrder = value; RaisePropertyChanged("surchargeByOrder"); } }
        }

        private List<Orders> _allOrders;
        public List<Orders> allOrders
        {
            get
            {
                _allOrders = DataAccessLayer.getOrders();
                return _allOrders;
            }
        }
        private List<OrdersData> _allDataOrders;
        public List<OrdersData> allDataOrders
        {
            get
            {
                _allDataOrders = DataAccessLayer.getOrdersData();
                return _allDataOrders;
            }
        }
        private List<OrdersEmail> _allEmailOrders;
        public List<OrdersEmail> allEmailOrders
        {
            get
            {
                _allEmailOrders = DataAccessLayer.getOrdersEmails();
                return _allEmailOrders;
            }
        }
        private List<OrdersSchoolSend> _allSchoolSendOrders;
        public List<OrdersSchoolSend> allSchoolSendOrders
        {
            get
            {
                _allSchoolSendOrders = DataAccessLayer.getOrdersSchoolSend();
                return _allSchoolSendOrders;
            }
        }
        private List<OrdersDirectMailing> _allDirectMailingOrders;
        public List<OrdersDirectMailing> allDirectMailingOrders
        {
            get
            {
                _allDirectMailingOrders = DataAccessLayer.getOrdersDirectMailing();
                return _allDirectMailingOrders;
            }
        }
        private List<OrdersSharedMailing> _allSharedMailingOrders;
        public List<OrdersSharedMailing> allSharedMailingOrders
        {
            get
            {
                _allSharedMailingOrders = DataAccessLayer.getOrdersSharedMailing();
                return _allSharedMailingOrders;
            }
        }
        private List<OrdersPrint> _allPrintOrders;
        public List<OrdersPrint> allPrintOrders
        {
            get
            {
                _allPrintOrders = DataAccessLayer.getOrdersPrint();
                return _allPrintOrders;
            }
        }
        private List<OrdersSurcharge> _allSurchargeOrders;
        public List<OrdersSurcharge> allSurchargeOrders
        {
            get
            {
                _allSurchargeOrders = DataAccessLayer.getOrdersSurcharge();
                return _allSurchargeOrders;
            }
        }
        #endregion

        #region Commands
        private RelayCommand _newOrder;
        public RelayCommand newOrder
        {
            get { if (_newOrder == null) { _newOrder = new RelayCommand(() => { if (_newOrder != null) { if (newOrder != null) { NavigationService.Navigate(typeof(OrderView)); } } }); } return _newOrder; }
        }
        #endregion

        public OrdersViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {

        }

        private Orders _selectedOrder;
        public Orders selectedOrder
        {
            get { return _selectedOrder; }
            set { if (_selectedOrder != value) { _selectedOrder = value; RaisePropertyChanged("selectedOrder"); } }
        }

        public void orderInvoked(object sender, object parameter)
        {
            long orderID = 0;
            //Get parameter (order part)
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get clicked order
            var order = arg.ClickedItem as Orders;
            orderID = order.ID;
            
            dataByOrder = DataAccessLayer.getOrdersDataByOrderID(orderID);
            emailByOrder = DataAccessLayer.getOrdersEmailsByOrderID(orderID);
            schoolsendByOrder = DataAccessLayer.getOrdersSchoolSendByOrderID(orderID);
            directMailingByOrder = DataAccessLayer.getOrdersDirectMailingByOrderID(orderID);
            sharedMailingByOrder = DataAccessLayer.getOrdersSharedMailingByOrderID(orderID);
            printByOrder = DataAccessLayer.getOrdersPrintByOrderID(orderID);
            surchargeByOrder = DataAccessLayer.getOrdersSurchargeByOrderID(orderID);
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
            if(orderID != 0) 
            {
                //Navigate to CompanyView
                this.NavigationService.Navigate(typeof(OrderView));
                //Pass ID parameter
                MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<Int64>(orderID, "OrderViewModel"));
            }
            
        }

    }
}