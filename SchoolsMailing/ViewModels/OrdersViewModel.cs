using SchoolsMailing.ViewModels.Common;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using GalaSoft.MvvmLight.Command;
using SchoolsMailing.Models;
using System.Collections.ObjectModel;
using SchoolsMailing.DAL;

namespace SchoolsMailing.ViewModels
{
    class OrdersViewModel : PageViewModel
    {
        public OrdersViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //companies = DataAccessLayer.GetAllCompanies2(); //TODO: replace
            //MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp); // register company parameter
        }
        //private RelayCommand _getDataOrders;
        //public RelayCommand getDataOrders
        //{
        //    get
        //    {
        //        if (_getDataOrders == null)
        //        {
        //            _getDataOrders = new RelayCommand(() => {
        //                if (getDataOrders != null)
        //                {
        //                    GetOrders();
        //                }
        //            });
        //        }
        //        return _getDataOrders;
        //    }
        //}
        //private ObservableCollection<OrdersData> _dataOrders;
        //public ObservableCollection<OrdersData> dataOrders
        //{
        //    get { return _dataOrders; }
        //    set { if (_dataOrders != value) { _dataOrders = value; RaisePropertyChanged("dataOrders"); } }
        //}


        //public void GetOrders()
        //{
        //    dataOrders = DataAccessLayer.GetAllData();
        //    RaisePropertyChanged("dataOrders");
        //}
    }
}
