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
using SchoolsMailing.DAL;

namespace SchoolsMailing.ViewModels
{
    public class NewOrderViewModel : PageViewModel
    {
        public NewOrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            companies = DataAccessLayer.GetAllCompanies2();
            MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp); // register company parameter
        }

        public void SetUp(NotificationMessage<Int64> obj)
        {
            if (obj.Notification == "OrderViewModel")
            {
                if (obj.Content != 0)
                {
                    //TODO: Get Order Code
                    selectedOrder = DataAccessLayer.GetOrder(obj.Content);
                }
                else
                {
                    //TODO: Set new order code
                    selectedOrder = new Orders();
                }
                companies = DataAccessLayer.GetAllCompanies2();
            }
        }

        private Orders _selectedOrder;
        public Orders selectedOrder
        {
            get { return _selectedOrder; }
            set { if(_selectedOrder != value) { _selectedOrder = value;  RaisePropertyChanged("selectedOrder"); } }
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

        #region Order information
        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> companies
        {
            get { return _companies; }
            set { if(_companies != value) { _companies = value;  RaisePropertyChanged("companies"); } }
        }

        private Company _selectedCompany;
        public Company selectedCompany
        {
            get { return _selectedCompany; }
            set { if(_selectedCompany != value) { _selectedCompany = value;  RaisePropertyChanged("selectedCompany"); } }
        }

        private ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> contacts
        {
            get { return _contacts; }
            set { if(_contacts != value) { _contacts = value;  RaisePropertyChanged("contacts"); } }
        }

        private Contact _selectedContact;
        public Contact selectedContact
        {
            get { return _selectedContact; }
            set { if(_selectedContact != value) { _selectedContact = value;  RaisePropertyChanged("selectedContact"); } }
        }

        private RelayCommand _companyChanged;
        public RelayCommand companyChanged
        {
            get
            {
                if(_companyChanged == null)
                {
                    _companyChanged = new RelayCommand(() =>
                    {
                        contacts = DataAccessLayer.GetContactsByCompany(selectedCompany.ID);
                    });
                }
                return _companyChanged;
            }
        }
        
        #endregion

        #region Orders
        #region Data Order
        private ObservableCollection<Data> _dataOrders;
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

        private RelayCommand _saveData;
        public RelayCommand saveData
        {
            get
            {
                if (_saveData == null)
                {
                    _saveData = new RelayCommand(() =>
                    {
                        dataOrders.Add(newDataOrder);
                        newDataOrder = new Data();
                        NavigationService.GoBack();
                    });
                }

                return _saveData;

            }
        }

        private RelayCommand _cancelData;
        public RelayCommand cancelData
        {
            get
            {
                if (_cancelData == null)
                {
                    _cancelData = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelData;

            }
        }

        private RelayCommand _duplicateData;
        public RelayCommand duplicateData
        {
            get
            {
                if (_duplicateData == null)
                {
                    _duplicateData = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateData;

            }
        }

        private RelayCommand _deleteData;
        public RelayCommand deleteData
        {
            get
            {
                if (_deleteData == null)
                {
                    _deleteData = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteData;

            }
        }
        #endregion

        #region Email Order
        private ObservableCollection<Email> _emailOrders;
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
                        NavigationService.GoBack();
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
                        NavigationService.GoBack();
                    });
                }

                return _cancelEmail;

            }
        }

        private RelayCommand _duplicateEmail;
        public RelayCommand duplicateEmail
        {
            get
            {
                if (_duplicateEmail == null)
                {
                    _duplicateEmail = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateEmail;
            }
        }

        private RelayCommand _deleteEmail;
        public RelayCommand deleteEmail
        {
            get
            {
                if (_deleteEmail == null)
                {
                    _deleteEmail = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteEmail;
            }
        }
        #endregion

        #region Direct Mailing Order
        private ObservableCollection<DirectMailing> _directMailingOrders;
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

        private RelayCommand _saveDirectMailing;
        public RelayCommand saveDirectMailing
        {
            get
            {
                if (_saveDirectMailing == null)
                {
                    _saveDirectMailing = new RelayCommand(() =>
                    {
                        directMailingOrders.Add(newDirectMailingOrder);
                        newDirectMailingOrder = new DirectMailing();
                        NavigationService.GoBack();
                    });
                }

                return _saveDirectMailing;

            }
        }

        private RelayCommand _cancelDirectMailing;
        public RelayCommand cancelDirectMailing
        {
            get
            {
                if (_cancelDirectMailing == null)
                {
                    _cancelDirectMailing = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelDirectMailing;

            }
        }

        private RelayCommand _duplicateDirectMailing;
        public RelayCommand duplicateDirectMailing
        {
            get
            {
                if (_duplicateDirectMailing == null)
                {
                    _duplicateDirectMailing = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateDirectMailing;
            }
        }

        private RelayCommand _deleteDirectMailing;
        public RelayCommand deleteDirectMailing
        {
            get
            {
                if (_deleteDirectMailing == null)
                {
                    _deleteDirectMailing = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteDirectMailing;
            }
        }
        #endregion

        #region Print Order
        private ObservableCollection<Print> _printOrders;
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

        private RelayCommand _savePrint;
        public RelayCommand savePrint
        {
            get
            {
                if (_savePrint == null)
                {
                    _savePrint = new RelayCommand(() =>
                    {
                        printOrders.Add(newPrintOrder);
                        newPrintOrder = new Print();
                        NavigationService.GoBack();
                    });
                }

                return _savePrint;

            }
        }

        private RelayCommand _cancelPrint;
        public RelayCommand cancelPrint
        {
            get
            {
                if (_cancelPrint == null)
                {
                    _cancelPrint = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelPrint;

            }
        }

        private RelayCommand _duplicatePrint;
        public RelayCommand duplicatePrint
        {
            get
            {
                if (_duplicatePrint == null)
                {
                    _duplicatePrint = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicatePrint;
            }
        }

        private RelayCommand _deletePrint;
        public RelayCommand deletePrint
        {
            get
            {
                if (_deletePrint == null)
                {
                    _deletePrint = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deletePrint;
            }
        }
        #endregion

        #region SchoolSend Order
        private ObservableCollection<SchoolSend> _schoolSendOrders;
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

        private RelayCommand _saveSchoolSend;
        public RelayCommand saveSchoolSend
        {
            get
            {
                if (_saveSchoolSend == null)
                {
                    _saveSchoolSend = new RelayCommand(() =>
                    {
                        schoolSendOrders.Add(newSchoolSendOrder);
                        newSchoolSendOrder = new SchoolSend();
                        NavigationService.GoBack();
                    });
                }

                return _saveSchoolSend;

            }
        }

        private RelayCommand _cancelSchoolSend;
        public RelayCommand cancelSchoolSend
        {
            get
            {
                if (_cancelSchoolSend == null)
                {
                    _cancelSchoolSend = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelSchoolSend;

            }
        }

        private RelayCommand _setCredits;
        public RelayCommand setCredits
        {
            get {
                if (_setCredits == null) {
                        _setCredits = new RelayCommand(() => {
                            //switch (newSchoolSendOrder.schoolsendPackage)
                            //{
                            //    case "Ruby":
                            //        break;
                            //}
                            Debug.WriteLine(string.Format(newSchoolSendOrder.schoolsendPackage.ToString()));
                        });
                }
                return _setCredits;
            }
        }

        private RelayCommand _duplicateSchoolSend;
        public RelayCommand duplicateSchoolSend
        {
            get
            {
                if (_duplicateSchoolSend == null)
                {
                    _duplicateSchoolSend = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateSchoolSend;
            }
        }

        private RelayCommand _deleteSchoolSend;
        public RelayCommand deleteSchoolSend
        {
            get
            {
                if (_deleteSchoolSend == null)
                {
                    _deleteSchoolSend = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteSchoolSend;
            }
        }
        #endregion

        #region Shared Mailing Order
        private ObservableCollection<SharedMailing> _sharedMailingOrders;
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

        private RelayCommand _saveSharedMailing;
        public RelayCommand saveSharedMailing
        {
            get
            {
                if (_saveSharedMailing == null)
                {
                    _saveSharedMailing = new RelayCommand(() =>
                    {
                        sharedMailingOrders.Add(newSharedMailingOrder);
                        newSharedMailingOrder = new SharedMailing();
                        NavigationService.GoBack();
                    });
                }

                return _saveSharedMailing;

            }
        }

        private RelayCommand _cancelSharedMailing;
        public RelayCommand cancelSharedMailing
        {
            get
            {
                if (_cancelSharedMailing == null)
                {
                    _cancelSharedMailing = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelSharedMailing;

            }
        }

        private RelayCommand _duplicateSharedMailing;
        public RelayCommand duplicateSharedMailing
        {
            get
            {
                if (_duplicateSharedMailing == null)
                {
                    _duplicateSharedMailing = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateSharedMailing;
            }
        }

        private RelayCommand _deleteSharedMailing;
        public RelayCommand deleteSharedMailing
        {
            get
            {
                if (_deleteSharedMailing == null)
                {
                    _deleteSharedMailing = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteSharedMailing;
            }
        }
        #endregion

        #region Surcharge Order
        private ObservableCollection<Surcharge> _surchargeOrders;
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

        private RelayCommand _saveSurcharge;
        public RelayCommand saveSurcharge
        {
            get
            {
                if (_saveSurcharge == null)
                {
                    _saveSurcharge = new RelayCommand(() =>
                    {
                        surchargeOrders.Add(newSurchargeOrder);
                        newSurchargeOrder = new Surcharge();
                        NavigationService.GoBack();
                    });
                }

                return _saveSurcharge;

            }
        }

        private RelayCommand _cancelSurcharge;
        public RelayCommand cancelSurcharge
        {
            get
            {
                if (_cancelSurcharge == null)
                {
                    _cancelSurcharge = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                    });
                }

                return _cancelSurcharge;

            }
        }

        private RelayCommand _duplicateSurcharge;
        public RelayCommand duplicateSurcharge
        {
            get
            {
                if (_duplicateSurcharge == null)
                {
                    _duplicateSurcharge = new RelayCommand(() =>
                    {
                        Debug.WriteLine("Duplicate");
                    });
                }

                return _duplicateSurcharge;
            }
        }

        private RelayCommand _deleteSurcharge;
        public RelayCommand deleteSurcharge
        {
            get
            {
                if (_deleteSurcharge == null)
                {
                    _deleteSurcharge = new RelayCommand(() =>
                    {
                        Debug.WriteLine("delete");
                    });
                }

                return _deleteSurcharge;
            }
        }
        #endregion
        #endregion
    }
}
