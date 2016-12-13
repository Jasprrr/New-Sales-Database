using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using SchoolsMailing.Views;
using SchoolsMailing.Models;
using System.Collections.ObjectModel;

namespace SchoolsMailing.ViewModels
{
    public class NewOrderViewModel : PageViewModel
    {
        public NewOrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {

        }

        private RelayCommand _saveEmail;
        public RelayCommand saveEmail
        {
            get
            {
                if (_saveEmail == null)
                {
                    _saveEmail = new RelayCommand(() =>
                    {
                        emailOrders.Add(newEmailOrder);
                        newEmailOrder = new Email();
                    });
                }

                return _saveEmail;

            }
        }

        private RelayCommand _cancelEmail;
        public RelayCommand cancelEmail
        {
            get
            {
                if (_cancelEmail == null)
                {
                    _cancelEmail = new RelayCommand(() =>
                    {
                        
                    });
                }

                return _cancelEmail;

            }
        }

        private string _someBinding;
        public string someBinding
        {
            get { return _someBinding; }
            set { if(_someBinding != value) { _someBinding = value; RaisePropertyChanged("someBinding"); } }
        }

        #region Navigate to new orders
        private RelayCommand _newEmail;
        public RelayCommand newEmail
        {
            get { if (_newEmail == null) { _newEmail = new RelayCommand(() => { NavigationService.Navigate(typeof(NewEmailView)); }); } return _newEmail; }
        }
        private RelayCommand _newData;
        public RelayCommand newData
        {
            get { if (_newData == null) { _newData = new RelayCommand(() => { NavigationService.Navigate(typeof(NewDataView)); }); } return _newData; }
        }
        private RelayCommand _newSchoolSend;
        public RelayCommand newSchoolSend
        {
            get { if (_newSchoolSend == null) { _newSchoolSend = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSchoolSendView)); }); } return _newSchoolSend; }
        }
        private RelayCommand _newDirectMailing;
        public RelayCommand newDirectMailing
        {
            get { if (_newDirectMailing == null) { _newDirectMailing = new RelayCommand(() => { NavigationService.Navigate(typeof(NewDirectMailingView)); }); } return _newDirectMailing; }
        }
        private RelayCommand _newSharedMailing;
        public RelayCommand newSharedMailing
        {
            get { if (_newSharedMailing == null) { _newSharedMailing = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSharedMailingView)); }); } return _newSharedMailing; }
        }
        private RelayCommand _newSurcharge;
        public RelayCommand newSurcharge
        {
            get { if (_newSurcharge == null) { _newSurcharge = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSurchargeView)); }); } return _newSurcharge; }
        }
        private RelayCommand _newPrint;
        public RelayCommand newPrint
        {
            get { if (_newPrint == null) { _newPrint = new RelayCommand(() => { NavigationService.Navigate(typeof(NewPrintView)); }); } return _newPrint; }
        }
        #endregion

        private RelayCommand _cancelNew;
        public RelayCommand cancelNew
        {
            get { if (_cancelNew == null) { _cancelNew = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cancelNew; }
        }

        private string _orderCode;
        public string orderCode
        {
            get { return _orderCode; }
            set { if(_orderCode != value) { _orderCode = value; RaisePropertyChanged("orderCode"); } }
        }

        #region Orders
        #region Data Order
        private ObservableCollection<Data> _dataOrders = new ObservableCollection<Data>();
        public ObservableCollection<Data> dataOrders
        {
            get { return _dataOrders; }
            set { if(_dataOrders != value) { _dataOrders = value; RaisePropertyChanged("dataOrders"); } }
        }

        private Data _newDataOrder = new Data();
        public Data newDataOrder
        {
            get { return _newDataOrder; }
            set { if (_newDataOrder != value) { _newDataOrder = value; RaisePropertyChanged("newDataOrder"); } }
        }
        #endregion

        #region Email Order
        private ObservableCollection<Email> _emailOrders = new ObservableCollection<Email>();
        public ObservableCollection<Email> emailOrders
        {
            get { return _emailOrders; }
            set { if(_emailOrders != value) { _emailOrders = value; RaisePropertyChanged("emailOrders"); } }
        }

        private Email _newEmailOrder = new Email();
        public Email newEmailOrder
        {
            get { return _newEmailOrder; }
            set { if(_newEmailOrder != value) { _newEmailOrder = value; RaisePropertyChanged("newEmailOrder"); } }
        }
        #endregion

        #region Direct Mailing Order
        private ObservableCollection<DirectMailing> _directMailingOrders = new ObservableCollection<DirectMailing>();
        public ObservableCollection<DirectMailing> directMailingOrders
        {
            get { return _directMailingOrders; }
            set { if(_directMailingOrders != value) { _directMailingOrders = value;  RaisePropertyChanged("directMailingOrders"); } }
        }

        private DirectMailing _newDirectMailingOrder = new DirectMailing();
        public DirectMailing newDirectMailingOrder
        {
            get { return _newDirectMailingOrder; }
            set { if(_newDirectMailingOrder != value) { _newDirectMailingOrder = value; RaisePropertyChanged("newDirectMailingOrder"); } }
        }
        #endregion

        #region Print Order
        private ObservableCollection<Print> _printOrders = new ObservableCollection<Print>();
        public ObservableCollection<Print> printOrders
        {
            get { return _printOrders; }
            set { if(_printOrders != value) { _printOrders = value; RaisePropertyChanged("printOrders"); } }
        }

        private Print _newPrintOrder = new Print();
        public Print newPrintOrder
        {
            get { return _newPrintOrder; }
            set { if(_newPrintOrder != value) { _newPrintOrder = value; RaisePropertyChanged("newPrintOrder"); } }
        }
        #endregion

        #region SchoolSend Order
        private ObservableCollection<SchoolSend> _schoolSendOrders = new ObservableCollection<SchoolSend>();
        public ObservableCollection<SchoolSend> schoolSendOrders
        {
            get { return _schoolSendOrders; }
            set { if(_schoolSendOrders != value) { _schoolSendOrders = value;  RaisePropertyChanged("schoolSendOrders"); } }
        }

        private SchoolSend _newSchoolSendOrder = new SchoolSend();
        public SchoolSend newSchoolSendOrder
        {
            get { return _newSchoolSendOrder; }
            set { if(_newSchoolSendOrder != value) { _newSchoolSendOrder = value; RaisePropertyChanged("newSchoolSendOrder"); } }
        }
        #endregion

        #region Shared Mailing Order
        private ObservableCollection<SharedMailing> _sharedMailingOrders = new ObservableCollection<SharedMailing>();
        public ObservableCollection<SharedMailing> sharedMailingOrders
        {
            get { return _sharedMailingOrders; }
            set { if(_sharedMailingOrders != value) { _sharedMailingOrders = value; RaisePropertyChanged("sharedMailingOrders"); } }
        }

        private SharedMailing _newSharedMailingOrder = new SharedMailing();
        public SharedMailing newSharedMailingOrder
        {
            get { return _newSharedMailingOrder; }
            set { if(_newSharedMailingOrder != value) { _newSharedMailingOrder = value; RaisePropertyChanged("newSharedMailingOrder"); } }
        }
        #endregion

        #region Surcharge Order
        private ObservableCollection<Surcharge> _surchargeOrders = new ObservableCollection<Surcharge>();
        public ObservableCollection<Surcharge> surchargeOrders
        {
            get { return _surchargeOrders; }
            set { if(_surchargeOrders != value) { _surchargeOrders = value; RaisePropertyChanged("surchargeOrders"); } }
        }

        private Surcharge _newSurchargeOrder = new Surcharge();
        public Surcharge newSurchargeOrder
        {
            get { return _newSurchargeOrder; }
            set { if(_newSurchargeOrder != value) { _newSurchargeOrder = value; RaisePropertyChanged("newSurchargeOrder"); } }
        }
        #endregion
        #endregion
    }
}
