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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Text.RegularExpressions;
using SchoolsMailing.Controls;

namespace SchoolsMailing.ViewModels
{
    public class OrderViewModel : PageViewModel
    {
        public OrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            companies = DataAccessLayer.GetAllCompanies2(); //TODO: replace
            MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp); // register company parameter
        }

        public void SetUp(NotificationMessage<Int64> obj)
        {
            if (obj.Notification == "OrderViewModel")
            {
                if (obj.Content != 0) //Get order
                {
                    selectedOrder = DataAccessLayer.GetOrder(obj.Content);
                    emailOrders = DataAccessLayer.GetAllEmailsByOrderID(obj.Content);
                    dataOrders = DataAccessLayer.GetAllDataByOrderID(obj.Content);
                    schoolSendOrders = DataAccessLayer.GetAllSchoolSendsByOrderID(obj.Content);
                    sharedMailingOrders = DataAccessLayer.GetAllSharedMailingsByOrderID(obj.Content);
                    directMailingOrders = DataAccessLayer.GetAllDirectMailingsByOrderID(obj.Content);
                    printOrders = DataAccessLayer.GetAllPrintByOrderID(obj.Content);
                    surchargeOrders = DataAccessLayer.GetAllSurchargesByOrderID(obj.Content);

                    selectedCompany = DataAccessLayer.GetCompanyById(selectedOrder.companyID);
                    selectedContact = DataAccessLayer.GetContactById(selectedOrder.contactID);
                }
                else //Create order
                {
                    selectedOrder = new Orders() { orderDate = DateTime.Now };
                    //New & existing lists
                    emailOrders = new ObservableCollection<Email>();
                    dataOrders = new ObservableCollection<Data>();
                    schoolSendOrders = new ObservableCollection<SchoolSend>();
                    sharedMailingOrders = new ObservableCollection<SharedMailing>();
                    directMailingOrders = new ObservableCollection<DirectMailing>();
                    printOrders = new ObservableCollection<Print>();
                    surchargeOrders = new ObservableCollection<Surcharge>();
                    //Deleted lists 
                    deletedEmailOrders = new ObservableCollection<Email>();
                    deletedDataOrders = new ObservableCollection<Data>();
                    deletedDirectMailingOrders = new ObservableCollection<DirectMailing>();
                    deletedPrintOrders = new ObservableCollection<Print>();
                    deletedSchoolSendOrders = new ObservableCollection<SchoolSend>();
                    deletedSharedMailingOrders = new ObservableCollection<SharedMailing>();
                    deletedSurchargeOrders = new ObservableCollection<Surcharge>();
                }
                companies = DataAccessLayer.GetAllCompanies2();
                schoolsendPacks = DataAccessLayer.GetAllSchoolSendPacks();
                sharedPacks = DataAccessLayer.GetAllSharedPacks();
                pivotIndex = 0;
            }
        }
        
        private int _pivotIndex;
        public int pivotIndex
        {
            get { return _pivotIndex; }
            set { if (_pivotIndex != value) { _pivotIndex = value; RaisePropertyChanged("pivotIndex"); } }
        }

        #region Navigate to new orders
        private RelayCommand _newData;
        public RelayCommand newData
        {
            get { if (_newData == null) {
                    _newData = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(DataView));
                        selectedDataOrder = new Data() { dataStart = DateTime.Now, dataEnd = DateTime.Now };
                        originalDataOrder = new Data();
                    });
                } return _newData;
            }
        }

        private RelayCommand _newEmail;
        public RelayCommand newEmail
        {
            get
            {
                if (_newEmail == null)
                {
                    _newEmail = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(EmailView));
                        selectedEmailOrder = new Email() { emailDate = DateTime.Now };
                        originalEmailOrder = new Email();
                    });
                }
                return _newEmail;
            }
        }
        
        private RelayCommand _newSchoolSend;
        public RelayCommand newSchoolSend
        {
            get { if (_newSchoolSend == null) {
                    _newSchoolSend = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SchoolSendView));
                        selectedSchoolSendOrder = new SchoolSend() { schoolsendStart = DateTime.Now, schoolsendEnd = DateTime.Now.AddYears(1) };
                        originalSchoolSendOrder = new SchoolSend();
                    });
                } return _newSchoolSend;
            }
        }

        private RelayCommand _newDirectMailing;
        public RelayCommand newDirectMailing
        {
            get { if (_newDirectMailing == null) {
                    _newDirectMailing = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(DirectMailingView));
                        selectedDirectMailingOrder = new DirectMailing() { directArtworkDate = DateTime.Now, directDate = DateTime.Now, directDataDate = DateTime.Now, directInsertDate = DateTime.Now };
                        originalDirectMailingOrder = new DirectMailing();
                    });
                } return _newDirectMailing;
            }
        }

        private RelayCommand _newSharedMailing;
        public RelayCommand newSharedMailing
        {
            get { if (_newSharedMailing == null) {
                    _newSharedMailing = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SharedMailingView));
                        selectedSharedMailingOrder = new SharedMailing();
                        originalSharedMailingOrder = new SharedMailing();
                    });
                } return _newSharedMailing;
            }
        }

        private RelayCommand _newPrint;
        public RelayCommand newPrint
        {
            get
            {
                if (_newPrint == null)
                {
                    _newPrint = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(PrintView));
                        selectedPrintOrder = new Print() { printDate = DateTime.Now };
                        originalPrintOrder = new Print();
                    });
                }
                return _newPrint;
            }
        }

        private RelayCommand _newSurcharge;
        public RelayCommand newSurcharge
        {
            get { if (_newSurcharge == null) {
                    _newSurcharge = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SurchargeView));
                        selectedSurchargeOrder = new Surcharge() { surchargeDate = DateTime.Now };
                        originalSurchargeOrder = new Surcharge();
                    });
                } return _newSurcharge;
            }
        }

        private RelayCommand _newSharedPack;
        public RelayCommand newSharedPack
        {
            get
            {
                if (_newSharedPack == null)
                {
                    _newSharedPack = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SharedPackageView));
                        selectedSharedPack = new SharedPack() { packArtworkDate = DateTime.Now, packDate = DateTime.Now, packDeliveryDate = DateTime.Now};
                    });
                }
                return _newSharedPack;
            }
        }
        #endregion

        #region Order information

        private string _orderCode;
        public string orderCode
        {
            get { return _orderCode; }
            set { if (_orderCode != value) { _orderCode = value; RaisePropertyChanged("orderCode"); } }
        }

        private Orders _selectedOrder;
        public Orders selectedOrder
        {
            get { return _selectedOrder; }
            set { if (_selectedOrder != value) { _selectedOrder = value; RaisePropertyChanged("selectedOrder"); } }
        }

        private RelayCommand _saveOrder; //Save order & loops through lists
        public RelayCommand saveOrder
        {
            get
            {
                if (_saveOrder == null)
                {
                    _saveOrder = new RelayCommand(() => {
                        DataAccessLayer.SaveOrder(selectedOrder);
                        long orderID = selectedOrder.ID;
                        //Email
                        foreach (Email e in emailOrders) //Loop through emails
                        {
                            e.orderID = orderID;
                            DataAccessLayer.SaveEmail(e); //Add emails
                        }
                        foreach (Email e in deletedEmailOrders) //Loop through emails
                        {
                            DataAccessLayer.DeleteEmail(e); //Delete emails
                        }
                        //Data
                        foreach (Data d in dataOrders)
                        {
                            d.orderID = orderID;
                            DataAccessLayer.SaveData(d);
                        }
                        foreach (Data d in deletedDataOrders)
                        {
                            DataAccessLayer.DeleteData(d);
                        }
                        //Direct mailing
                        foreach (DirectMailing dm in directMailingOrders)
                        {
                            dm.orderID = orderID;
                            DataAccessLayer.SaveDirectMailing(dm);
                        }
                        foreach (DirectMailing dm in deletedDirectMailingOrders)
                        {
                            DataAccessLayer.DeleteDirectMailing(dm);
                        }
                        //Shared mailing
                        foreach (SharedMailing sm in sharedMailingOrders)
                        {
                            sm.orderID = orderID;
                            DataAccessLayer.SaveSharedMailing(sm);
                        }
                        foreach (SharedMailing sm in deletedSharedMailingOrders)
                        {
                            DataAccessLayer.DeleteSharedMailing(sm);
                        }
                        //Surcharge
                        foreach (Surcharge s in surchargeOrders)
                        {
                            s.orderID = orderID;
                            DataAccessLayer.SaveSurcharge(s);
                        }
                        foreach (Surcharge s in deletedSurchargeOrders)
                        {
                            DataAccessLayer.DeleteSurcharge(s);
                        }
                        //Print
                        foreach (Print p in printOrders)
                        {
                            p.orderID = orderID;
                            DataAccessLayer.SavePrint(p);
                        }
                        foreach (Print p in deletedPrintOrders)
                        {
                            DataAccessLayer.DeletePrint(p);
                        }
                        //SchoolSend
                        foreach (SchoolSend ss in schoolSendOrders)
                        {
                            ss.orderID = orderID;
                            DataAccessLayer.SaveSchoolSend(ss);
                        }
                        foreach (SchoolSend ss in deletedSchoolSendOrders)
                        {
                            DataAccessLayer.DeleteSchoolSend(ss);
                        }
                    });
                }
                return _saveOrder;
            }
        }

        private double _dataCosts = 0;
        public double dataCosts
        {
            get { return _dataCosts; }
            set { if (_dataCosts != value) { _dataCosts = value; RaisePropertyChanged("dataCosts"); } }
        }

        private double _emailCosts = 0;
        public double emailCosts
        {
            get { return _emailCosts; }
            set { if (_emailCosts != value) { _emailCosts = value; RaisePropertyChanged("emailCost"); } }
        }

        private double _schoolSendCosts = 0;
        public double schoolSendCosts
        {
            get { return _schoolSendCosts; }
            set { if (_schoolSendCosts != value) { _schoolSendCosts = value; RaisePropertyChanged("schoolSendCosts"); } }
        }

        private double _directMailingCosts = 0;
        public double directMailingCosts
        {
            get { return _directMailingCosts; }
            set { if (_directMailingCosts != value) { _directMailingCosts = value; RaisePropertyChanged("directMailingCosts"); } }
        }

        private double _sharedMailingCosts = 0;
        public double sharedMailingCosts
        {
            get { return _sharedMailingCosts; }
            set { if (_sharedMailingCosts != value) { _sharedMailingCosts = value; RaisePropertyChanged("sharedMailingCosts"); } }
        }

        private double _printCosts = 0;
        public double printCosts
        {
            get { return _printCosts; }
            set { if (_printCosts != value) { _printCosts = value; RaisePropertyChanged("printCosts"); } }
        }

        private double _surchargeCosts = 0;
        public double surchargeCosts
        {
            get { return _surchargeCosts; }
            set { if (_surchargeCosts != value) { _surchargeCosts = value; RaisePropertyChanged("surchargeCosts"); } }
        }

        public void CalculateCosts()
        {
            emailCosts = 0;
            dataCosts = 0;
            schoolSendCosts = 0;
            directMailingCosts = 0;
            sharedMailingCosts = 0;
            printCosts = 0;
            surchargeCosts = 0;

            foreach (Email p in emailOrders)
            {
                emailCosts = emailCosts + p.emailCost;
                RaisePropertyChanged("emailCosts");
            }
            foreach (Data p in dataOrders)
            {
                dataCosts = dataCosts + p.dataCost;
                RaisePropertyChanged("dataCosts");
            }
            foreach (DirectMailing p in directMailingOrders)
            {
                directMailingCosts = directMailingCosts + p.directCost;
                RaisePropertyChanged("directMailingCosts");
            }
            foreach (SharedMailing p in sharedMailingOrders)
            {
                sharedMailingCosts = sharedMailingCosts + p.sharedCost;
                RaisePropertyChanged("sharedMailingCosts");
            }
            foreach (Print p in printOrders)
            {
                printCosts = printCosts + p.printCost;
                RaisePropertyChanged("printCosts");
            }
            foreach (Surcharge p in surchargeOrders)
            {
                surchargeCosts = surchargeCosts + p.surchargeCost;
                RaisePropertyChanged("surchargeCosts");
            }
            selectedOrder.orderTotal = emailCosts + dataCosts + directMailingCosts + sharedMailingCosts + printCosts + surchargeCosts;
            RaisePropertyChanged("selectedOrder");
        }
        #endregion

        #region Company
        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> companies
        {
            get { return _companies; }
            set { if (_companies != value) { _companies = value; RaisePropertyChanged("companies"); } }
        }

        private Company _selectedCompany;
        public Company selectedCompany
        {
            get { return _selectedCompany; }
            set { if (_selectedCompany != value) { _selectedCompany = value; RaisePropertyChanged("selectedCompany"); } }
        }

        private RelayCommand _companyChanged;
        public RelayCommand companyChanged
        {
            get
            {
                if (_companyChanged == null)
                {
                    _companyChanged = new RelayCommand(() =>
                    {
                        if (selectedCompany != null)
                        {
                            if (selectedCompany.companyInvoiceName != null) { selectedOrder.companyName = selectedCompany.companyInvoiceName.ToString(); }
                            if (selectedCompany.companyInvoiceAddress1 != null) { selectedOrder.companyAddress1 = selectedCompany.companyInvoiceAddress1.ToString(); }
                            if (selectedCompany.companyInvoiceAddress2 != null) { selectedOrder.companyAddress2 = selectedCompany.companyInvoiceAddress2.ToString(); }
                            if (selectedCompany.companyInvoiceCity != null) { selectedOrder.companyCity = selectedCompany.companyInvoiceCity.ToString(); }
                            if (selectedCompany.companyInvoiceCounty != null) { selectedOrder.companyCounty = selectedCompany.companyInvoiceCounty.ToString(); }
                            if (selectedCompany.companyInvoicePostCode != null) { selectedOrder.companyPostCode = selectedCompany.companyInvoicePostCode.ToString(); }

                            contacts = DataAccessLayer.GetContactsByCompany(selectedCompany.ID);
                        }
                    });
                }
                return _companyChanged;
            }
        }
        #endregion

        #region Contact
        private Contact _selectedContact;
        public Contact selectedContact
        {
            get { return _selectedContact; }
            set { if (_selectedContact != value) { _selectedContact = value; RaisePropertyChanged("selectedContact"); } }
        }

        private ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> contacts
        {
            get { return _contacts; }
            set { if (_contacts != value) { _contacts = value; RaisePropertyChanged("contacts"); } }
        }

        private RelayCommand _contactChanged;
        public RelayCommand contactChanged
        {
            get
            {
                if (_contactChanged == null)
                {
                    _contactChanged = new RelayCommand(() =>
                    {
                        if (selectedContact != null)
                        {
                            if (selectedContact.contactTitle != null) { selectedOrder.contactTitle = selectedContact.contactTitle.ToString(); }
                            if (selectedContact.contactForename != null) { selectedOrder.contactForename = selectedContact.contactForename.ToString(); }
                            if (selectedContact.contactSurname != null) { selectedOrder.contactSurname = selectedContact.contactSurname.ToString(); }
                            if (selectedContact.contactTelephone != null) { selectedOrder.contactTelephone = selectedContact.contactTelephone.ToString(); }
                            if (selectedContact.contactEmail != null) { selectedOrder.contactEmail = selectedContact.contactEmail.ToString(); }
                        }
                    });
                }
                return _contactChanged;
            }
        }
        #endregion

        #region Data Order
        private ObservableCollection<Data> _dataOrders;
        public ObservableCollection<Data> dataOrders
        {
            get { return _dataOrders; }
            set { if (_dataOrders != value) { _dataOrders = value; RaisePropertyChanged("dataOrders"); } }
        }
        private ObservableCollection<Data> _deletedDataOrders;
        public ObservableCollection<Data> deletedDataOrders
        {
            get { return _deletedDataOrders; }
            set { if (_deletedDataOrders != value) { _deletedDataOrders = value; RaisePropertyChanged("deletedDataOrders"); } }
        }
        private Data _selectedDataOrder;
        public Data selectedDataOrder
        {
            get { return _selectedDataOrder; }
            set { if (_selectedDataOrder != value) { _selectedDataOrder = value; RaisePropertyChanged("selectedDataOrder"); } }
        }
        private Data _originalDataOrder;
        public Data originalDataOrder
        {
            get { return _originalDataOrder; }
            set { if (_originalDataOrder != value) { _originalDataOrder = value; RaisePropertyChanged("deleteDataOrder"); } }
        }
        public Data rightClickedData = new Data(); //Holds right clicked item for flyout
        private bool _isValidDataStart = false;
        public bool isValidDataStart
        {
            get { return _isValidDataStart; }
            set { if (_isValidDataStart != value) { _isValidDataStart = !_isValidDataStart; RaisePropertyChanged("isValidDataStart"); } }
        }
        private bool _isValidDataEnd = false;
        public bool isValidDataEnd
        {
            get { return _isValidDataEnd; }
            set { if (_isValidDataEnd != value) { _isValidDataEnd = !_isValidDataEnd; RaisePropertyChanged("isValidDataEnd"); } }
        }
        private bool _isValidDataDetails = false;
        public bool isValidDataDetails
        {
            get { return _isValidDataDetails; }
            set { if (_isValidDataDetails != value) { _isValidDataDetails = !_isValidDataDetails; RaisePropertyChanged("isValidDataDetails"); } }
        }
        private bool _isValidDataCost = false;
        public bool isValidDataCost
        {
            get { return _isValidDataCost; }
            set { if (_isValidDataCost != value) { _isValidDataCost = !_isValidDataCost; RaisePropertyChanged("isValidDataCost"); } }
        }
        #endregion

        #region Email Order
        private ObservableCollection<Email> _emailOrders;
        public ObservableCollection<Email> emailOrders
        {
            get { return _emailOrders; }
            set { if (_emailOrders != value) { _emailOrders = value; RaisePropertyChanged("emailOrders"); } }
        }
        private ObservableCollection<Email> _deletedEmailOrders;
        public ObservableCollection<Email> deletedEmailOrders
        {
            get { return _deletedEmailOrders; }
            set { if (_deletedEmailOrders != value) { _deletedEmailOrders = value; RaisePropertyChanged("deletedEmailOrders"); } }
        }
        private Email _selectedEmailOrder;
        public Email selectedEmailOrder
        {
            get { return _selectedEmailOrder; }
            set { if (_selectedEmailOrder != value) { _selectedEmailOrder = value; RaisePropertyChanged("selectedEmailOrder"); } }
        }
        private Email _originalEmailOrder;
        public Email originalEmailOrder
        {
            get { return _originalEmailOrder; }
            set { if (_originalEmailOrder != value) { _originalEmailOrder = value; RaisePropertyChanged("originalEmailOrder"); } }
        }
        public Email rightClickedEmail = new Email(); //Holds right clicked item for flyout
        private bool _isValidEmailDate = false;
        public bool isValidEmailDate
        {
            get { return _isValidEmailDate; }
            set { if (_isValidEmailDate != value) { _isValidEmailDate = !_isValidEmailDate; RaisePropertyChanged("isValidEmailDate"); } }
        }
        private bool _isValidEmailDetails = false;
        public bool isValidEmailDetails
        {
            get { return _isValidEmailDetails; }
            set { if (_isValidEmailDetails != value) { _isValidEmailDetails = !_isValidEmailDetails; RaisePropertyChanged("isValidEmailDetails"); } }
        }
        private bool _isValidEmailAdminCost = false;
        public bool isValidEmailAdminCost
        {
            get { return _isValidEmailAdminCost; }
            set { if (_isValidEmailAdminCost != value) { _isValidEmailAdminCost = !_isValidEmailAdminCost; RaisePropertyChanged("isValidEmailAdminCost"); } }
        }
        private bool _isValidEmailDirectCost = false;
        public bool isValidEmailDirectCost
        {
            get { return _isValidEmailDirectCost; }
            set { if (_isValidEmailDirectCost != value) { _isValidEmailDirectCost = !_isValidEmailDirectCost; RaisePropertyChanged("isValidEmailDirectCost"); } }
        }
        private bool _isValidEmailCost = false;
        public bool isValidEmailCost
        {
            get { return _isValidEmailCost; }
            set { if (_isValidEmailCost != value) { _isValidEmailCost = !_isValidEmailCost; RaisePropertyChanged("isValidEmailCost"); } }
        }
        public DateTime duplicateEmailDate;
        #endregion

        #region Direct Mailing Order
        private ObservableCollection<DirectMailing> _directMailingOrders;
        public ObservableCollection<DirectMailing> directMailingOrders
        {
            get { return _directMailingOrders; }
            set { if (_directMailingOrders != value) { _directMailingOrders = value; RaisePropertyChanged("directMailingOrders"); } }
        }
        private ObservableCollection<DirectMailing> _deletedDirectMailingOrders;
        public ObservableCollection<DirectMailing> deletedDirectMailingOrders
        {
            get { return _deletedDirectMailingOrders; }
            set { if (_deletedDirectMailingOrders != value) { _deletedDirectMailingOrders = value; RaisePropertyChanged("deletedDirectMailingOrders"); } }
        }
        private DirectMailing _selectedDirectMailingOrder;
        public DirectMailing selectedDirectMailingOrder
        {
            get { return _selectedDirectMailingOrder; }
            set { if (_selectedDirectMailingOrder != value) { _selectedDirectMailingOrder = value; RaisePropertyChanged("selectedDirectMailingOrder"); } }
        }
        private DirectMailing _originalDirectMailingOrder;
        public DirectMailing originalDirectMailingOrder
        {
            get { return _originalDirectMailingOrder; }
            set { if (_originalDirectMailingOrder != value) { _originalDirectMailingOrder = value; RaisePropertyChanged("originalDirectMailingOrder"); } }
        }
        public DirectMailing rightClickedDirectMailing = new DirectMailing(); //Holds right clicked item for flyout
        private bool _isValidDirectMailingFulfilmentCost = false;
        public bool isValidDirectMailingFulfilmentCost
        {
            get { return _isValidDirectMailingFulfilmentCost; }
            set { if (_isValidDirectMailingFulfilmentCost != value) { _isValidDirectMailingFulfilmentCost = !_isValidDirectMailingFulfilmentCost; RaisePropertyChanged("isValidDirectMailingFulfilmentCost"); } }
        }
        public bool isValidDirectMailingPrintCost
        {
            get { return _isValidDirectMailingPrintCost; }
            set { if (_isValidDirectMailingPrintCost != value) { _isValidDirectMailingPrintCost = !_isValidDirectMailingPrintCost; RaisePropertyChanged("isValidDirectMailingPrintCost"); } }
        }
        private bool _isValidDirectMailingPostageCost = false;
        public bool isValidDirectMailingPostageCost
        {
            get { return _isValidDirectMailingPostageCost; }
            set { if (_isValidDirectMailingPostageCost != value) { _isValidDirectMailingPostageCost = !_isValidDirectMailingPostageCost; RaisePropertyChanged("isValidDirectMailingPostageCost"); } }
        }
        private bool _isValidDirectMailingPrintCost = false;
        private bool _isValidDirectMailingDate = false;
        public bool isValidDirectMailingDate
        {
            get { return _isValidDirectMailingDate; }
            set { if (_isValidDirectMailingDate != value) { _isValidDirectMailingDate = !_isValidDirectMailingDate; RaisePropertyChanged("isValidDirectMailingDate"); } }
        }
        private bool _isValidDirectMailingDetails = false;
        public bool isValidDirectMailingDetails
        {
            get { return _isValidDirectMailingDetails; }
            set { if (_isValidDirectMailingDetails != value) { _isValidDirectMailingDetails = !_isValidDirectMailingDetails; RaisePropertyChanged("isValidDirectMailingDetails"); } }
        }
        private bool _isValidDirectMailingDeliveryCode = false;
        public bool isValidDirectMailingDeliveryCode
        {
            get { return _isValidDirectMailingDeliveryCode; }
            set { if (_isValidDirectMailingDeliveryCode != value) { _isValidDirectMailingDeliveryCode = !_isValidDirectMailingDeliveryCode; RaisePropertyChanged("isValidDirectMailingDeliveryCode"); } }
        }
        private bool _isValidDirectMailingArtworkDate = false;
        public bool isValidDirectMailingArtworkDate
        {
            get { return _isValidDirectMailingArtworkDate; }
            set { if (_isValidDirectMailingArtworkDate != value) { _isValidDirectMailingArtworkDate = !_isValidDirectMailingArtworkDate; RaisePropertyChanged("isValidDirectMailingArtworkDate"); } }
        }
        private bool _isValidDirectMailingDataDate = false;
        public bool isValidDirectMailingDataDate
        {
            get { return _isValidDirectMailingDataDate; }
            set { if (_isValidDirectMailingDataDate != value) { _isValidDirectMailingDataDate = !_isValidDirectMailingDataDate; RaisePropertyChanged("isValidDirectMailingDataDate"); } }
        }
        private bool _isValidDirectMailingInsertDate = false;
        public bool isValidDirectMailingInsertDate
        {
            get { return _isValidDirectMailingInsertDate; }
            set { if (_isValidDirectMailingInsertDate != value) { _isValidDirectMailingInsertDate = !_isValidDirectMailingInsertDate; RaisePropertyChanged("isValidDirectMailingInsertDate"); } }
        }

        private RelayCommand _setDirectMailingDate;
        public RelayCommand setDirectMailingDate
        {
            get
            {
                if (_setDirectMailingDate == null)
                {
                    _setDirectMailingDate = new RelayCommand(() => {
                        if (setDirectMailingDate != null)
                        {
                            selectedDirectMailingOrder.directArtworkDate = selectedDirectMailingOrder.directDate.AddDays(-14);
                            selectedDirectMailingOrder.directInsertDate = selectedDirectMailingOrder.directDate.AddDays(-5);
                            RaisePropertyChanged("selectedDirectMailingOrder");
                        }
                    });
                }
                return _setDirectMailingDate;
            }
        }
        #endregion

        #region Print Order
        private ObservableCollection<Print> _printOrders;
        public ObservableCollection<Print> printOrders
        {
            get { return _printOrders; }
            set { if (_printOrders != value) { _printOrders = value; RaisePropertyChanged("printOrders"); } }
        }
        private ObservableCollection<Print> _deletedPrintOrders;
        public ObservableCollection<Print> deletedPrintOrders
        {
            get { return _deletedPrintOrders; }
            set { if (_deletedPrintOrders != value) { _deletedPrintOrders = value; RaisePropertyChanged("deletedPrintOrders"); } }
        }
        private Print _selectedPrintOrder;
        public Print selectedPrintOrder
        {
            get { return _selectedPrintOrder; }
            set { if (_selectedPrintOrder != value) { _selectedPrintOrder = value; RaisePropertyChanged("selectedPrintOrder"); } }
        }
        private Print _originalPrintOrder;
        public Print originalPrintOrder
        {
            get { return _originalPrintOrder; }
            set { if (_originalPrintOrder != value) { _originalPrintOrder = value; RaisePropertyChanged("originalPrintOrder"); } }
        }
        public Print rightClickedPrint = new Print(); //Holds right clicked item for flyout
        private bool _isValidPrintCharge = false;
        public bool isValidPrintCharge
        {
            get { return _isValidPrintCharge; }
            set { if (_isValidPrintCharge != value) { _isValidPrintCharge = !_isValidPrintCharge; RaisePropertyChanged("isValidPrintCharge"); } }
        }
        private bool _isValidPrintCost = false;
        public bool isValidPrintCost
        {
            get { return _isValidPrintCost; }
            set { if (_isValidPrintCost != value) { _isValidPrintCost = !_isValidPrintCost; RaisePropertyChanged("isValidPrintCost"); } }
        }
        private bool _isValidPrintDetails = false;
        public bool isValidPrintDetails
        {
            get { return _isValidPrintDetails; }
            set { if (_isValidPrintDetails != value) { _isValidPrintDetails = !_isValidPrintDetails; RaisePropertyChanged("isValidPrintDetails"); } }
        }
        private bool _isValidPrintPrinter = false;
        public bool isValidPrintPrinter
        {
            get { return _isValidPrintPrinter; }
            set { if (_isValidPrintPrinter != value) { _isValidPrintPrinter = !_isValidPrintPrinter; RaisePropertyChanged("isValidPrintPrinter"); } }
        }
        private bool _isValidPrintDate = false;
        public bool isValidPrintDate
        {
            get { return _isValidPrintDate; }
            set { if (_isValidPrintDate != value) { _isValidPrintDate = !_isValidPrintDate; RaisePropertyChanged("isValidPrintDate"); } }
        }
        #endregion

        #region SchoolSend Order
        private ObservableCollection<SchoolSend> _schoolSendOrders;
        public ObservableCollection<SchoolSend> schoolSendOrders
        {
            get { return _schoolSendOrders; }
            set { if (_schoolSendOrders != value) { _schoolSendOrders = value; RaisePropertyChanged("schoolSendOrders"); } }
        }
        private ObservableCollection<SchoolSend> _deletedSchoolSendOrders;
        public ObservableCollection<SchoolSend> deletedSchoolSendOrders
        {
            get { return _deletedSchoolSendOrders; }
            set { if (_deletedSchoolSendOrders != value) { _deletedSchoolSendOrders = value; RaisePropertyChanged("deletedSchoolSendOrders"); } }
        }
        private ObservableCollection<SchoolSendPack> _schoolsendPacks;
        public ObservableCollection<SchoolSendPack> schoolsendPacks
        {
            get { return _schoolsendPacks; }
            set { if (_schoolsendPacks != value) { _schoolsendPacks = value; RaisePropertyChanged("schoolsendPacks"); } }
        }
        private SchoolSendPack _selectedSchoolSendPack;
        public SchoolSendPack selectedSchoolSendPack
        {
            get { return _selectedSchoolSendPack; }
            set { if (_selectedSchoolSendPack != value) { _selectedSchoolSendPack = value; RaisePropertyChanged("selectedSchoolSendPack"); } }
        }
        private SchoolSend _selectedSchoolSendOrder;
        public SchoolSend selectedSchoolSendOrder
        {
            get { return _selectedSchoolSendOrder; }
            set { if (_selectedSchoolSendOrder != value) { _selectedSchoolSendOrder = value; RaisePropertyChanged("selectedSchoolSendOrder"); } }
        }
        private SchoolSend _originalSchoolSendOrder;
        public SchoolSend originalSchoolSendOrder
        {
            get { return _originalSchoolSendOrder; }
            set { if (_originalSchoolSendOrder != value) { _originalSchoolSendOrder = value; RaisePropertyChanged("originalSchoolSendOrder"); } }
        }
        public SchoolSend rightClickedSchoolSend = new SchoolSend(); //Holds right clicked item for flyout
        private bool _isValidSchoolSendStart = false;
        public bool isValidSchoolSendStart
        {
            get { return _isValidSchoolSendStart; }
            set { if (_isValidSchoolSendStart != value) { _isValidSchoolSendStart = !_isValidSchoolSendStart; RaisePropertyChanged("isValidSchoolSendStart"); } }
        }
        private bool _isValidSchoolSendEnd = false;
        public bool isValidSchoolSendEnd
        {
            get { return _isValidSchoolSendEnd; }
            set { if (_isValidSchoolSendEnd != value) { _isValidSchoolSendEnd = !_isValidSchoolSendEnd; RaisePropertyChanged("isValidSchoolSendEnd"); } }
        }
        private bool _isValidSchoolSendPackage = false;
        public bool isValidSchoolSendPackage
        {
            get { return _isValidSchoolSendPackage; }
            set { if (_isValidSchoolSendPackage != value) { _isValidSchoolSendPackage = !_isValidSchoolSendPackage; RaisePropertyChanged("isValidSchoolSendPackage"); } }
        }
        private bool _isValidSchoolSendCost = false;
        public bool isValidSchoolSendCost
        {
            get { return _isValidSchoolSendCost; }
            set { if (_isValidSchoolSendCost != value) { _isValidSchoolSendCost = !_isValidSchoolSendCost; RaisePropertyChanged("isValidSchoolSendCost"); } }
        }

        private RelayCommand _setSchoolSendDate;
        public RelayCommand setSchoolSendDate
        {
            get
            {
                if (_setSchoolSendDate == null)
                {
                    _setSchoolSendDate = new RelayCommand(() => {
                        if (setSchoolSendDate != null)
                        {
                            selectedSchoolSendOrder.schoolsendEnd = selectedSchoolSendOrder.schoolsendStart.AddYears(1);
                            RaisePropertyChanged("selectedSchoolSendOrder");
                        }
                    });
                }
                return _setSchoolSendDate;
            }
        }
        private RelayCommand _setCredits;
        public RelayCommand setCredits
        {
            get {
                if (_setCredits == null) {
                    _setCredits = new RelayCommand(() => {
                        if (selectedSchoolSendPack != null)
                        {
                            selectedSchoolSendOrder.schoolsendPackage = selectedSchoolSendPack.ID;
                            selectedSchoolSendOrder.schoolsendCredits = selectedSchoolSendPack.packCredits;
                            selectedSchoolSendOrder.schoolsendCost = selectedSchoolSendPack.packCost;
                            RaisePropertyChanged("selectedSchoolSendOrder");
                        }
                    });
                }
                return _setCredits;
            }
        }
        #endregion

        #region Shared Mailing Order
        private ObservableCollection<SharedMailing> _sharedMailingOrders;
        public ObservableCollection<SharedMailing> sharedMailingOrders
        {
            get { return _sharedMailingOrders; }
            set { if (_sharedMailingOrders != value) { _sharedMailingOrders = value; RaisePropertyChanged("sharedMailingOrders"); } }
        }
        private ObservableCollection<SharedMailing> _deletedSharedMailingOrders;
        public ObservableCollection<SharedMailing> deletedSharedMailingOrders
        {
            get { return _deletedSharedMailingOrders; }
            set { if (_deletedSharedMailingOrders != value) { _deletedSharedMailingOrders = value; RaisePropertyChanged("deletedSharedMailingOrders"); } }
        }
        private SharedMailing _selectedSharedMailingOrder;
        public SharedMailing selectedSharedMailingOrder
        {
            get { return _selectedSharedMailingOrder; }
            set { if (_selectedSharedMailingOrder != value) { _selectedSharedMailingOrder = value; RaisePropertyChanged("selectedSharedMailingOrder"); } }
        }
        private SharedMailing _originalSharedMailingOrder;
        public SharedMailing originalSharedMailingOrder
        {
            get { return _originalSharedMailingOrder; }
            set { if (_originalSharedMailingOrder != value) { _originalSharedMailingOrder = value; RaisePropertyChanged("originalSharedMailingOrder"); } }
        }
        public SharedMailing rightClickedSharedMailing = new SharedMailing(); //Holds right clicked item for flyout
        private bool _isValidSharedPackage = false;
        public bool isValidSharedPackage
        {
            get { return _isValidSharedPackage; }
            set { if (_isValidSharedPackage != value) { _isValidSharedPackage = !_isValidSharedPackage; RaisePropertyChanged("isValidSharedPackage"); } }
        }
        private bool _isValidSharedDate = false;
        public bool isValidSharedDate
        {
            get { return _isValidSharedDate; }
            set { if (_isValidSharedDate != value) { _isValidSharedDate = !_isValidSharedDate; RaisePropertyChanged("isValidSharedDate"); } }
        }
        private bool _isValidSharedDeliveryDate = false;
        public bool isValidSharedDeliveryDate
        {
            get { return _isValidSharedDeliveryDate; }
            set { if (_isValidSharedDeliveryDate != value) { _isValidSharedDeliveryDate = !_isValidSharedDeliveryDate; RaisePropertyChanged("isValidSharedDeliveryDate"); } }
        }
        private bool _isValidSharedArtworkDate = false;
        public bool isValidSharedArtworkDate
        {
            get { return _isValidSharedArtworkDate; }
            set { if (_isValidSharedArtworkDate != value) { _isValidSharedArtworkDate = !_isValidSharedArtworkDate; RaisePropertyChanged("isValidSharedArtworkDate"); } }
        }
        private bool _isValidSharedCost = false;
        public bool isValidSharedCost
        {
            get { return _isValidSharedCost; }
            set { if (_isValidSharedCost != value) { _isValidSharedCost = !_isValidSharedCost; RaisePropertyChanged("isValidSharedCost"); } }
        }

        private RelayCommand _setPack;
        public RelayCommand setPack
        {
            get
            {
                if (_setPack == null) {
                    _setPack = new RelayCommand(() => {
                        if (selectedSharedPack != null)
                        {
                            selectedSharedMailingOrder.sharedArtworkDate = selectedSharedPack.packArtworkDate;
                            selectedSharedMailingOrder.sharedDate = selectedSharedPack.packDate;
                            selectedSharedMailingOrder.sharedDeliveryDate = selectedSharedPack.packDeliveryDate;
                            selectedSharedMailingOrder.sharedMailingTo = selectedSharedPack.packTo;
                            selectedSharedMailingOrder.sharedCost = selectedSharedPack.packCost;
                            RaisePropertyChanged("selectedSharedMailingOrder");
                        }
                    });
                }
                return _setPack;
            }
        }
        #endregion

        #region Surcharge Order
        private ObservableCollection<Surcharge> _surchargeOrders;
        public ObservableCollection<Surcharge> surchargeOrders
        {
            get { return _surchargeOrders; }
            set { if (_surchargeOrders != value) { _surchargeOrders = value; RaisePropertyChanged("surchargeOrders"); } }
        }
        private ObservableCollection<Surcharge> _deletedSurchargeOrders;
        public ObservableCollection<Surcharge> deletedSurchargeOrders
        {
            get { return _deletedSurchargeOrders; }
            set { if (_deletedSurchargeOrders != value) { _deletedSurchargeOrders = value; RaisePropertyChanged("deletedSurchargeOrders"); } }
        }
        private Surcharge _selectedSurchargeOrder;
        public Surcharge selectedSurchargeOrder
        {
            get { return _selectedSurchargeOrder; }
            set { if (_selectedSurchargeOrder != value) { _selectedSurchargeOrder = value; RaisePropertyChanged("selectedSurchargeOrder"); } }
        }
        private Surcharge _originalSurchargeOrder;
        public Surcharge originalSurchargeOrder
        {
            get { return _originalSurchargeOrder; }
            set { if (_originalSurchargeOrder != value) { _originalSurchargeOrder = value; RaisePropertyChanged("originalSurchargeOrder"); } }
        }
        public Surcharge rightClickedSurcharge = new Surcharge(); //Holds right clicked item for flyout
        private bool _isValidSurchargeDate = false;
        public bool isValidSurchargeDate
        {
            get { return _isValidSurchargeDate; }
            set { if (_isValidSurchargeDate != value) { _isValidSurchargeDate = !_isValidSurchargeDate; RaisePropertyChanged("isValidSurchargeDate"); } }
        }
        private bool _isValidSurchargeDetails = false;
        public bool isValidSurchargeDetails
        {
            get { return _isValidSurchargeDetails; }
            set { if (_isValidSurchargeDetails != value) { _isValidSurchargeDetails = !_isValidSurchargeDetails; RaisePropertyChanged("isValidSurchargeDetails"); } }
        }
        private bool _isValidSurchargeCost = false;
        public bool isValidSurchargeCost
        {
            get { return _isValidSurchargeCost; }
            set { if (_isValidSurchargeCost != value) { _isValidSurchargeCost = !_isValidSurchargeCost; RaisePropertyChanged("isValidSurchargeCost"); } }
        }
        #endregion

        #region School Send Package
        private ObservableCollection<SharedPack> _sharedPacks;
        public ObservableCollection<SharedPack> sharedPacks
        {
            get { return _sharedPacks; }
            set { if (_sharedPacks != value) { _sharedPacks = value; RaisePropertyChanged("sharedPacks"); } }
        }
        private SharedPack _selectedSharedPack;
        public SharedPack selectedSharedPack
        {
            get { return _selectedSharedPack; }
            set { if (_selectedSharedPack != value) { _selectedSharedPack = value; RaisePropertyChanged("selectedSharedPack"); } }
        }

        private bool _isValidPackageName = false;
        public bool isValidPackageName
        {
            get { return _isValidPackageName; }
            set { if (_isValidPackageName != value) { _isValidPackageName = !_isValidPackageName; RaisePropertyChanged("isValidPackageName"); } }
        }
        private bool _isValidPackageTo = false;
        public bool isValidPackageTo
        {
            get { return _isValidPackageTo; }
            set { if (_isValidPackageTo != value) { _isValidPackageTo = !_isValidPackageTo; RaisePropertyChanged("isValidPackageTo"); } }
        }
        private bool _isValidPackageDate = false;
        public bool isValidPackageDate
        {
            get { return _isValidPackageDate; }
            set { if (_isValidPackageDate != value) { _isValidPackageDate = !_isValidPackageDate; RaisePropertyChanged("isValidPackageDate"); } }
        }
        private bool _isValidPackageArtworkDate = false;
        public bool isValidPackageArtworkDate
        {
            get { return _isValidPackageArtworkDate; }
            set { if (_isValidPackageArtworkDate != value) { _isValidPackageArtworkDate = !_isValidPackageArtworkDate; RaisePropertyChanged("isValidPackageArtworkDate"); } }
        }
        private bool _isValidPackageDeliveryDate = false;
        public bool isValidPackageDeliveryDate
        {
            get { return _isValidPackageDeliveryDate; }
            set { if (_isValidPackageDeliveryDate != value) { _isValidPackageDeliveryDate = !_isValidPackageDeliveryDate; RaisePropertyChanged("isValidPackageDeliveryDate"); } }
        }
        private bool _isValidPackageCost = false;
        public bool isValidPackageCost
        {
            get { return _isValidPackageCost; }
            set { if (_isValidPackageCost != value) { _isValidPackageCost = !_isValidPackageCost; RaisePropertyChanged("isValidPackageCost"); } }
        }
        private bool _isValidPackageMaxInserts = false;
        public bool isValidPackageMaxInserts
        {
            get { return _isValidPackageMaxInserts; }
            set { if (_isValidPackageMaxInserts != value) { _isValidPackageMaxInserts = !_isValidPackageMaxInserts; RaisePropertyChanged("isValidPackageMaxInserts"); } }
        }
        #endregion

        #region Save
        public void SaveOrderPart(String orderPart)
        {
            switch (orderPart)
            {
                case "Data":
                    if (ValidateDataOrder(selectedDataOrder))
                    {
                        dataOrders.Remove(originalDataOrder);
                        dataOrders.Add(selectedDataOrder);
                        NavigationService.GoBack();
                        pivotIndex = 1;
                        CalculateCosts();
                    }
                    break;

                case "Email":
                    if (validateEmailOrder(selectedEmailOrder))
                    {
                        emailOrders.Remove(originalEmailOrder);
                        emailOrders.Add(selectedEmailOrder);
                        pivotIndex = 2;
                        NavigationService.GoBack();
                        CalculateCosts();
                    }
                    break;

                case "SchoolSend":
                    if (validateSchoolSendOrder(selectedSchoolSendOrder))
                    {
                        schoolSendOrders.Remove(originalSchoolSendOrder);
                        schoolSendOrders.Add(selectedSchoolSendOrder);
                        NavigationService.GoBack();
                        pivotIndex = 3;
                        CalculateCosts();
                    }
                    break;

                case "DirectMailing":
                    if (validateDirectMailingOrder(selectedDirectMailingOrder))
                    {
                        directMailingOrders.Remove(originalDirectMailingOrder);
                        directMailingOrders.Add(selectedDirectMailingOrder);
                        NavigationService.GoBack();
                        pivotIndex = 4;
                        CalculateCosts();
                    }
                    break;

                case "SharedMailing":
                    if (validateSharedMailingOrder(selectedSharedMailingOrder))
                    {
                        sharedMailingOrders.Remove(originalSharedMailingOrder);
                        sharedMailingOrders.Add(selectedSharedMailingOrder);
                        NavigationService.GoBack();
                        pivotIndex = 5;
                        CalculateCosts();
                    }
                    break;

                case "Print":
                    if (validatePrintOrder(selectedPrintOrder))
                    {
                        printOrders.Remove(originalPrintOrder);
                        printOrders.Add(selectedPrintOrder);
                        NavigationService.GoBack();
                        pivotIndex = 6;
                        CalculateCosts();
                    }
                    break;

                case "Surcharge":
                    if (validateSurchargeOrder(selectedSurchargeOrder))
                    {
                        surchargeOrders.Remove(originalSurchargeOrder);
                        surchargeOrders.Add(selectedSurchargeOrder);
                        NavigationService.GoBack();
                        pivotIndex = 7;
                        CalculateCosts();
                    }
                    break;
                case "SharedPack":
                    if (validateSharedPackOrder(selectedSharedPack))
                    {
                        DataAccessLayer.SaveSharedPack(selectedSharedPack);
                        sharedPacks = DataAccessLayer.GetAllSharedPacks();
                        
                        NavigationService.GoBack();
                    }
                    break;
            }
        }

        private RelayCommand _saveData;
        public RelayCommand saveData
        {
            get { if (_saveData == null) { _saveData = new RelayCommand(() => { SaveOrderPart("Data"); }); } return _saveData; }
        }

        private RelayCommand _saveEmail;
        public RelayCommand saveEmail
        {
            get { if (_saveEmail == null) { _saveEmail = new RelayCommand(() => { SaveOrderPart("Email"); }); } return _saveEmail; }
        }

        private RelayCommand _saveSchoolSend;
        public RelayCommand saveSchoolSend
        {
            get { if (_saveSchoolSend == null) { _saveSchoolSend = new RelayCommand(() => { SaveOrderPart("SchoolSend"); }); } return _saveSchoolSend; }
        }

        private RelayCommand _saveDirectMailing;
        public RelayCommand saveDirectMailing
        {
            get { if (_saveDirectMailing == null) { _saveDirectMailing = new RelayCommand(() => { SaveOrderPart("DirectMailing"); }); } return _saveDirectMailing; }
        }

        private RelayCommand _saveSharedMailing;
        public RelayCommand saveSharedMailing
        {
            get { if (_saveSharedMailing == null) { _saveSharedMailing = new RelayCommand(() => { SaveOrderPart("SharedMailing"); }); } return _saveSharedMailing; }
        }

        private RelayCommand _savePrint;
        public RelayCommand savePrint
        {
            get { if (_savePrint == null) { _savePrint = new RelayCommand(() => { SaveOrderPart("Print"); }); } return _savePrint; }
        }

        private RelayCommand _saveSurcharge;
        public RelayCommand saveSurcharge
        {
            get { if (_saveSurcharge == null) { _saveSurcharge = new RelayCommand(() => { SaveOrderPart("Surcharge"); }); } return _saveSurcharge; }
        }

        private RelayCommand _saveSharedPack;
        public RelayCommand saveSharedPack
        {
            get { if (_saveSharedPack == null) { _saveSharedPack = new RelayCommand(() => { SaveOrderPart("SharedPack"); }); } return _saveSharedPack; }
        }
        #endregion

        #region Duplicate
        private RelayCommand _duplicateData;
        public RelayCommand duplicateData
        {
            get { if (_duplicateData == null) { _duplicateData = new RelayCommand(() => { duplicateDataOrder(rightClickedData); }); } return _duplicateData; }
        }
        public void duplicateDataOrder(Data data)
        {
            Data dupData = new Data()
            {
                dataStart = data.dataStart,
                dataEnd = data.dataEnd,
                dataDetails = data.dataDetails,
                dataCost = data.dataCost,
                dataCreated = DateTime.Now,
                dataModified = DateTime.Now
            };
            dataOrders.Add(dupData);
        }

        private RelayCommand _duplicateEmail;
        public RelayCommand duplicateEmail
        {
            get { if (_duplicateEmail == null) { _duplicateEmail = new RelayCommand(() => { duplicateEmailOrder(rightClickedEmail); }); } return _duplicateEmail; }
        }
        public async void duplicateEmailOrder(Email email)
        {
            var newDuplicateDialog = new DuplicateDialog(); //Create new input dialog
            var result = await newDuplicateDialog.ShowAsync(); //Show dialog & await result
            if (result == ContentDialogResult.Primary) //If primary option chosen
            {
                var item = newDuplicateDialog.Content; //Get user input
                String dateString = item.ToString(); //Convert to string
                duplicateEmailDate = Convert.ToDateTime(dateString); //Convert to DateTime
            }
            Email dupEmail = new Email()
            {
                emailDate = duplicateEmailDate,
                emailAdminCost = email.emailAdminCost,
                emailCost = email.emailCost,
                emailDetails = email.emailDetails,
                emailDirectCost = email.emailDirectCost,
                emailSetUp = email.emailSetUp,
                emailSubject = email.emailSubject,
                emailCreated = DateTime.Now,
                emailModified = DateTime.Now
            };
            emailOrders.Add(dupEmail);
        }

        private RelayCommand _duplicateSchoolSend;
        public RelayCommand duplicateSchoolSend
        {
            get { if (_duplicateSchoolSend == null) { _duplicateSchoolSend = new RelayCommand(() => { duplicateSchoolSendOrder(rightClickedSchoolSend); }); } return _duplicateSchoolSend; }
        }
        public void duplicateSchoolSendOrder(SchoolSend schoolSend)
        {
            SchoolSend dupSchoolSend = new SchoolSend()
            {
                schoolsendCost = schoolSend.schoolsendCost,
                schoolsendCredits = schoolSend.schoolsendCredits,
                schoolsendEnd = schoolSend.schoolsendEnd,
                schoolsendPackage = schoolSend.schoolsendPackage,
                schoolsendStart = schoolSend.schoolsendStart,
                schoolsendCreated = DateTime.Now,
                schoolsendModified = DateTime.Now
            };
            schoolSendOrders.Add(dupSchoolSend);
        }

        private RelayCommand _duplicateDirectMailing;
        public RelayCommand duplicateDirectMailing
        {
            get { if (_duplicateDirectMailing == null) { _duplicateDirectMailing = new RelayCommand(() => { duplicateDirectMailingOrder(rightClickedDirectMailing); }); } return _duplicateDirectMailing; }
        }
        public void duplicateDirectMailingOrder(DirectMailing directMailing)
        {
            DirectMailing dupDirectMailing = new DirectMailing()
            {
                directArtworkDate = directMailing.directArtworkDate,
                directCost = directMailing.directCost,
                directCreated = DateTime.Now,
                directDataDate = directMailing.directDataDate,
                directDate = directMailing.directDate,
                directDeliveryCode = directMailing.directDeliveryCode,
                directDetails = directMailing.directDetails,
                directFulfilmentCost = directMailing.directFulfilmentCost,
                directInsertDate = directMailing.directInsertDate,
                directLeafletCode = directMailing.directLeafletCode,
                directMailingTo = directMailing.directMailingTo,
                directModified = DateTime.Now,
                directPostageCost = directMailing.directPostageCost,
                directPrintCost = directMailing.directPrintCost
            };
            directMailingOrders.Add(dupDirectMailing);
        }

        private RelayCommand _duplicateSharedMailing;
        public RelayCommand duplicateSharedMailing
        {
            get { if (_duplicateSharedMailing == null) { _duplicateSharedMailing = new RelayCommand(() => { duplicateSharedMailingOrder(rightClickedSharedMailing); }); } return _duplicateSharedMailing; }
        }
        public void duplicateSharedMailingOrder(SharedMailing sharedMailing)
        {
            SharedMailing dupSharedMailing = new SharedMailing()
            {
                sharedArtworkDate = sharedMailing.sharedArtworkDate,
                sharedCost = sharedMailing.sharedCost,
                sharedCreated = DateTime.Now,
                sharedDate = sharedMailing.sharedDate,
                sharedDeliveryCode = sharedMailing.sharedDeliveryCode,
                sharedDeliveryDate = sharedMailing.sharedDeliveryDate,
                sharedFAO = sharedMailing.sharedFAO,
                sharedLeafletName = sharedMailing.sharedLeafletName,
                sharedLeafletSize = sharedMailing.sharedLeafletSize,
                sharedLeafletWeight = sharedMailing.sharedLeafletWeight,
                sharedMailingTo = sharedMailing.sharedMailingTo,
                sharedModified = DateTime.Now,
                sharedNumberOfLeaflets = sharedMailing.sharedNumberOfLeaflets,
                sharedPackage = sharedMailing.sharedPackage
            };
            sharedMailingOrders.Add(dupSharedMailing);
        }

        private RelayCommand _duplicatePrint;
        public RelayCommand duplicatePrint
        {
            get { if (_duplicatePrint == null) { _duplicatePrint = new RelayCommand(() => { duplicatePrintOrder(rightClickedPrint); }); } return _duplicatePrint; }
        }
        public void duplicatePrintOrder(Print print)
        {
            Print dupPrint = new Print()
            {
                printCharge = print.printCharge,
                printCost = print.printCost,
                printCreated = DateTime.Now,
                printDate = print.printDate,
                printDetails = print.printDetails,
                printModified = DateTime.Now,
                printPrinter = print.printPrinter
            };
            printOrders.Add(dupPrint);
        }

        private RelayCommand _duplicateSurcharge;
        public RelayCommand duplicateSurcharge
        {
            get { if (_duplicateSurcharge == null) { _duplicateSurcharge = new RelayCommand(() => { duplicateSurchargeOrder(rightClickedSurcharge); }); } return _duplicateSurcharge; }
        }
        public void duplicateSurchargeOrder(Surcharge surcharge)
        {
            Surcharge dupSurcharge = new Surcharge()
            {
                surchargeCost = surcharge.surchargeCost,
                surchargeCreated = DateTime.Now,
                surchargeDate = surcharge.surchargeDate,
                surchargeDetails = surcharge.surchargeDetails,
                surchargeModified = DateTime.Now
            };
            surchargeOrders.Add(surcharge);
        }
        #endregion

        #region Delete
        public void DeleteOrderPart(String orderPart)
        {
            switch (orderPart)
            {
                case "Data":
                    deletedDataOrders.Add(rightClickedData);
                    dataOrders.Remove(rightClickedData);
                    break;

                case "Email":
                    deletedEmailOrders.Add(rightClickedEmail);
                    emailOrders.Remove(rightClickedEmail);
                    break;

                case "SchoolSend":
                    deletedSchoolSendOrders.Add(rightClickedSchoolSend);
                    schoolSendOrders.Remove(rightClickedSchoolSend);
                    break;

                case "DirectMailing":
                    deletedDirectMailingOrders.Add(rightClickedDirectMailing);
                    directMailingOrders.Remove(rightClickedDirectMailing);
                    break;

                case "SharedMailing":
                    deletedSharedMailingOrders.Add(rightClickedSharedMailing);
                    sharedMailingOrders.Remove(rightClickedSharedMailing);
                    break;

                case "Print":
                    deletedPrintOrders.Add(rightClickedPrint);
                    printOrders.Remove(rightClickedPrint);
                    break;

                case "Surcharge":
                    deletedSurchargeOrders.Add(rightClickedSurcharge);
                    surchargeOrders.Remove(rightClickedSurcharge);
                    break;
            }

            CalculateCosts();
        }

        private RelayCommand _deleteData;
        public RelayCommand deleteData
        {
            get { if (_deleteData == null) { _deleteData = new RelayCommand(() => { if (rightClickedData != null) { DeleteOrderPart("Data"); } }); } return _deleteData; }
        }

        private RelayCommand _deleteEmail;
        public RelayCommand deleteEmail
        {
            get { if (_deleteEmail == null) { _deleteEmail = new RelayCommand(() => { if (rightClickedEmail != null) { DeleteOrderPart("Email"); } }); } return _deleteEmail; }
        }

        private RelayCommand _deleteSchoolSend;
        public RelayCommand deleteSchoolSend
        {
            get { if (_deleteSchoolSend == null) { _deleteSchoolSend = new RelayCommand(() => { if (rightClickedSchoolSend != null) { DeleteOrderPart("SchoolSend"); } }); } return _deleteSchoolSend; }
        }

        private RelayCommand _deleteDirectMailing;
        public RelayCommand deleteDirectMailing
        {
            get { if (_deleteDirectMailing == null) { _deleteDirectMailing = new RelayCommand(() => { if (rightClickedDirectMailing != null) { DeleteOrderPart("DirectMailing"); } }); } return _deleteDirectMailing; }
        }

        private RelayCommand _deleteSharedMailing;
        public RelayCommand deleteSharedMailing
        {
            get { if (_deleteSharedMailing == null) { _deleteSharedMailing = new RelayCommand(() => { if (rightClickedSharedMailing != null) { DeleteOrderPart("SharedMailing"); } }); } return _deleteSharedMailing; }
        }

        private RelayCommand _deletePrint;
        public RelayCommand deletePrint
        {
            get { if (_deletePrint == null) { _deletePrint = new RelayCommand(() => { if (rightClickedPrint != null) { DeleteOrderPart("Print"); } }); } return _deletePrint; }
        }

        private RelayCommand _deleteSurcharge;
        public RelayCommand deleteSurcharge
        {
            get { if (_deleteSurcharge == null) { _deleteSurcharge = new RelayCommand(() => { if (rightClickedSurcharge != null) { DeleteOrderPart("Surcharge"); } }); } return _deleteSurcharge; }
        }
        #endregion

        #region Validation
        private RelayCommand _validateData;
        public RelayCommand validateData
        {
            get { if (_validateData == null) { _validateData = new RelayCommand(() => { ValidateDataOrder(selectedDataOrder); }); } return _validateData; }
        }
        public bool ValidateDataOrder(Data data)
        {
            if (data.dataStart == null) { isValidDataStart = true; }
            else { isValidDataStart = false; }

            if (data.dataEnd == null || data.dataEnd < data.dataStart) { isValidDataEnd = true; }
            else { isValidDataEnd = false; }

            if (data.dataCost < 0) { isValidDataCost = true; }
            else { isValidDataCost = false; }

            if (data.dataDetails == null) { isValidDataDetails = true; }
            else { isValidDataDetails = false; }

            if (isValidDataStart == true ||
                isValidDataEnd == true ||
                isValidDataCost == true ||
                isValidDataDetails == true)
            {
                return false;
            }
            else { return true; }
        }

        private RelayCommand _validateEmail;
        public RelayCommand validateEmail
        {
            get { if (_validateEmail == null) { _validateEmail = new RelayCommand(() => { if (_validateEmail != null) { validateEmailOrder(selectedEmailOrder); } }); } return _validateEmail; }
        }
        public bool validateEmailOrder(Email email)
        {
            if (email.emailDate == null) { isValidEmailDate = true; }
            else { isValidEmailDate = false; }

            if (email.emailDetails == null) { isValidEmailDetails = true; }
            else { isValidEmailDetails = false; }

            if (email.emailAdminCost < 0) { isValidEmailAdminCost = true; }
            else { isValidEmailAdminCost = false; }

            if (email.emailDirectCost < 0) { isValidEmailDirectCost = true; }
            else { isValidEmailDirectCost = false; }

            if (email.emailCost < 0) { isValidEmailCost = true; }
            else { isValidEmailCost = false; }

            if (isValidEmailAdminCost == true ||
                isValidEmailCost == true ||
                isValidEmailDate == true ||
                isValidEmailDetails == true ||
                isValidEmailDirectCost == true)
            {
                return false;
            }
            else
            {
                if (selectedEmailOrder.emailSetUp)
                {
                    selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost + 125;
                }
                else
                {
                    selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost;
                }
                RaisePropertyChanged("selectedEmailOrder");
                return true;
            }
        }

        private RelayCommand _validateSchoolSend;
        public RelayCommand validateSchoolSend
        {
            get { if (_validateSchoolSend == null) { _validateSchoolSend = new RelayCommand(() => { if (_validateSchoolSend != null) { validateSchoolSendOrder(selectedSchoolSendOrder); } }); } return _validateSchoolSend; }
        }
        public bool validateSchoolSendOrder(SchoolSend schoolsend)
        {
            if (schoolsend.schoolsendStart == null) { isValidSchoolSendStart = true; }
            else { isValidSchoolSendStart = false; }

            if (schoolsend.schoolsendEnd == null || schoolsend.schoolsendEnd < schoolsend.schoolsendStart) { isValidSchoolSendEnd = true; }
            else { isValidSchoolSendEnd = false; }

            if (schoolsend.schoolsendPackage <= 0) { isValidSchoolSendPackage = true; }
            else { isValidSchoolSendPackage = false; }

            if (schoolsend.schoolsendCost < 0) { isValidSchoolSendCost = true; }
            else { isValidSchoolSendCost = false; }

            if (isValidSchoolSendStart == true ||
                isValidSchoolSendEnd == true ||
                isValidSchoolSendPackage == true ||
                isValidSchoolSendCost == true)
            {
                return false;
            }
            else { return true; }
        }

        private RelayCommand _validateDirectMailing;
        public RelayCommand validateDirectMailing
        {
            get { if (_validateDirectMailing == null) { _validateDirectMailing = new RelayCommand(() => { if (_validateDirectMailing != null) { validateDirectMailingOrder(selectedDirectMailingOrder); } }); } return _validateDirectMailing; }
        }
        public bool validateDirectMailingOrder(DirectMailing directMailing)
        {
            if (directMailing.directDate == null || directMailing.directDate < directMailing.directArtworkDate || directMailing.directDate < directMailing.directDataDate || directMailing.directDate < directMailing.directInsertDate) { isValidDirectMailingDate = true; }
            else { isValidDirectMailingDate = false; }

            if (directMailing.directArtworkDate == null || directMailing.directArtworkDate > directMailing.directDate) { isValidDirectMailingArtworkDate = true; }
            else { isValidDirectMailingArtworkDate = false; }

            if (directMailing.directDataDate == null || directMailing.directDataDate > directMailing.directDate) { isValidDirectMailingDataDate = true; }
            else { isValidDirectMailingDataDate = false; }

            if (directMailing.directInsertDate == null || directMailing.directInsertDate > directMailing.directDate) { isValidDirectMailingInsertDate = true; }
            else { isValidDirectMailingInsertDate = false; }

            if (directMailing.directDeliveryCode == null) { isValidDirectMailingDeliveryCode = true; }
            else { isValidDirectMailingDeliveryCode = false; }

            if (directMailing.directDetails == null) { isValidDirectMailingDetails = true; }
            else { isValidDirectMailingDetails = false; }

            if (directMailing.directFulfilmentCost < 0) { isValidDirectMailingFulfilmentCost = true; }
            else { isValidDirectMailingFulfilmentCost = false; }

            if (directMailing.directPostageCost < 0) { isValidDirectMailingPostageCost = true; }
            else { isValidDirectMailingPostageCost = false; }

            if (directMailing.directPrintCost < 0) { isValidDirectMailingPrintCost = true; }
            else { isValidDirectMailingPrintCost = false; }

            if (isValidDirectMailingDate == true ||
                isValidDirectMailingArtworkDate == true ||
                isValidDirectMailingInsertDate == true ||
                isValidDirectMailingDataDate == true ||
                isValidDirectMailingDeliveryCode == true ||
                isValidDirectMailingDetails == true ||
                isValidDirectMailingFulfilmentCost == true ||
                isValidDirectMailingPostageCost == true ||
                isValidDirectMailingPrintCost == true)
            {
                return false;
            }
            else
            {
                selectedDirectMailingOrder.directCost = selectedDirectMailingOrder.directFulfilmentCost + selectedDirectMailingOrder.directPostageCost + selectedDirectMailingOrder.directPrintCost;
                RaisePropertyChanged("selectedDirectMailingOrder");
                return true;
            }
        }

        private RelayCommand _validateSharedMailing;
        public RelayCommand validateSharedMailing
        {
            get { if (_validateSharedMailing == null) { _validateSharedMailing = new RelayCommand(() => { if (_validateSharedMailing != null) { validateSharedMailingOrder(selectedSharedMailingOrder); } }); } return _validateSharedMailing; }
        }
        public bool validateSharedMailingOrder(SharedMailing sharedMailing)
        {
            if (sharedMailing.sharedPackage <= 0) { isValidSharedPackage = true; }
            else { isValidSharedPackage = false; }

            if (sharedMailing.sharedDate == null || sharedMailing.sharedDate < sharedMailing.sharedDeliveryDate || sharedMailing.sharedDate < sharedMailing.sharedArtworkDate) { isValidSharedDate = true; }
            else { isValidSharedDate = false; }

            if (sharedMailing.sharedDeliveryDate == null || sharedMailing.sharedDeliveryDate > sharedMailing.sharedDate) { isValidSharedDeliveryDate = true; }
            else { isValidSharedDeliveryDate = false; }

            if (sharedMailing.sharedArtworkDate == null || sharedMailing.sharedArtworkDate > sharedMailing.sharedDate) { isValidSharedArtworkDate = true; }
            else { isValidSharedArtworkDate = false; }

            if (sharedMailing.sharedCost < 0) { isValidSharedCost = true; }
            else { isValidSharedCost = false; }

            if (isValidSharedPackage == true ||
                isValidSharedDate == true ||
                isValidSharedDeliveryDate == true ||
                isValidSharedArtworkDate == true ||
                isValidSharedCost == true)
            {
                return false;
            }
            else { return true; }
        }

        private RelayCommand _validatePrint;
        public RelayCommand validatePrint
        {
            get { if (_validatePrint == null) { _validatePrint = new RelayCommand(() => { if (_validatePrint != null) { validatePrintOrder(selectedPrintOrder); } }); } return _validatePrint; }
        }
        public bool validatePrintOrder(Print print)
        {
            if (print.printDate == null) { isValidPrintDate = true; }
            else { isValidPrintDate = false; }

            if (print.printPrinter == null) { isValidPrintPrinter = true; }
            else { isValidPrintPrinter = false; }

            if (print.printDetails == null) { isValidPrintDetails = true; }
            else { isValidPrintDetails = false; }

            if (print.printCharge < 0) { isValidPrintCharge = true; }
            else { isValidPrintCharge = false; }

            if (print.printCost < 0) { isValidPrintCost = true; }
            else { isValidPrintCost = false; }

            if (isValidPrintCharge == true ||
                isValidPrintDetails == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private RelayCommand _validateSurcharge;
        public RelayCommand validateSurcharge
        {
            get { if (_validateSurcharge == null) { _validateSurcharge = new RelayCommand(() => { if (_validateSurcharge != null) { validateSurchargeOrder(selectedSurchargeOrder); } }); } return _validateSurcharge; }
        }
        public bool validateSurchargeOrder(Surcharge surcharge)
        {
            if (surcharge.surchargeDate == null) { isValidSurchargeDate = true; }
            else { isValidSurchargeDate = false; }

            if (surcharge.surchargeDetails == null) { isValidSurchargeDetails = true; }
            else { isValidSurchargeDetails = false; }

            if (surcharge.surchargeCost < 0) { isValidSurchargeCost = true; }
            else { isValidSurchargeCost = false; }

            if (isValidSurchargeDate == true ||
                isValidSurchargeDetails == true ||
                isValidSurchargeCost == true)
            {
                return false;
            }
            else { return true; }
        }

        private RelayCommand _validateSharedPack;
        public RelayCommand validateSharedPack
        {
            get { if (_validateSharedPack == null) { _validateSharedPack = new RelayCommand(() => { if (_validateSharedPack != null) { validateSharedPackOrder(selectedSharedPack); } }); } return _validateSharedPack; }
        }
        public bool validateSharedPackOrder(SharedPack pack)
        {
            if(pack.packArtworkDate == null || pack.packArtworkDate > pack.packDate) { isValidPackageArtworkDate = true; }
            else { isValidPackageArtworkDate = false; }

            if(pack.packCost < 0) { isValidPackageCost = true; }
            else { isValidPackageCost = false; }

            if(pack.packDate == null || pack.packDate < pack.packArtworkDate || pack.packDate < pack.packDeliveryDate) { isValidPackageDate = true; }
            else { isValidPackageDate = false; }

            if(pack.packDeliveryDate == null || pack.packDeliveryDate > pack.packDate) { isValidPackageDeliveryDate = true; }
            else { isValidPackageDeliveryDate = false; }

            if(pack.packMaxInserts < 0) { isValidPackageMaxInserts = true; }
            else { isValidPackageMaxInserts = false; }

            if(pack.packName == null) { isValidPackageName = true; }
            else { isValidPackageName = false; }

            if (pack.packTo == null) { isValidPackageTo = true; }
            else { isValidPackageTo = false; }

            if(isValidPackageArtworkDate == true ||
                isValidPackageCost == true ||
                isValidPackageDate == true ||
                isValidPackageDeliveryDate == true ||
                isValidPackageMaxInserts == true ||
                isValidPackageName == true ||
                isValidPackageTo == true)
            {
                return false;
            }
            else { return true; }
        }
        #endregion

        #region Sum Totals
        private RelayCommand _sumEmail;
        public RelayCommand sumEmail
        {
            get { if (_sumEmail == null) { _sumEmail = new RelayCommand(() => { if (sumEmail != null) { SumEmailOrder(); } }); } return _sumEmail; }
        }
        public void SumEmailOrder()
        {
            if (selectedEmailOrder.emailSetUp)
            {
                selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost + 125;
            }
            else
            {
                selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost;
            }
            RaisePropertyChanged("selectedEmailOrder");
        }

        private RelayCommand _sumDirectMailing;
        public RelayCommand sumDirectMailing
        {
            get { if (_sumDirectMailing == null) { _sumDirectMailing = new RelayCommand(() => { if (sumDirectMailing != null) { SumDirectMailingOrder(); } }); } return _sumDirectMailing; }
        }
        public void SumDirectMailingOrder()
        {
            selectedDirectMailingOrder.directCost = selectedDirectMailingOrder.directFulfilmentCost + selectedDirectMailingOrder.directPostageCost + selectedDirectMailingOrder.directPrintCost;
            RaisePropertyChanged("selectedDirectMailingOrder");
        }
        #endregion

        #region Left Click
        public void dataClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected data from item
            Data item = arg.ClickedItem as Data;
            //Set selected order
            originalDataOrder = dataOrders.Where(x => x == item).First();
            selectedDataOrder = dataOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(DataView));
        }

        public void emailClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected email from item
            Email item = arg.ClickedItem as Email;
            //Set selected order
            originalEmailOrder = emailOrders.Where(x => x == item).First();
            selectedEmailOrder = emailOrders.Where(x => x == item).First();
            ////Navigate to order
            NavigationService.Navigate(typeof(EmailView));
        }

        public void schoolSendClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected SchoolSend from item
            SchoolSend item = arg.ClickedItem as SchoolSend;
            //Set selected order
            originalSchoolSendOrder = schoolSendOrders.Where(x => x == item).First();
            selectedSchoolSendOrder = schoolSendOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SchoolSendView));
        }

        public void directMailingClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected directMailing from item
            DirectMailing item = arg.ClickedItem as DirectMailing;
            //Set selected order
            originalDirectMailingOrder = directMailingOrders.Where(x => x == item).First();
            selectedDirectMailingOrder = directMailingOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(DirectMailingView));
        }

        public void sharedMailingClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected SharedMailing from item
            SharedMailing item = arg.ClickedItem as SharedMailing;
            //Set selected order
            originalSharedMailingOrder = sharedMailingOrders.Where(x => x == item).First();
            selectedSharedMailingOrder = sharedMailingOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SharedMailingView));
        }

        public void printClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected print from item
            Print item = arg.ClickedItem as Print;
            //Set selected order
            originalPrintOrder = printOrders.Where(x => x == item).First();
            selectedPrintOrder = printOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(PrintView));
        }

        public void surchargeClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected Surcharge from item
            Surcharge item = arg.ClickedItem as Surcharge;
            //Set selected order
            originalSurchargeOrder = surchargeOrders.Where(x => x == item).First();
            selectedSurchargeOrder = surchargeOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SurchargeView));
        }
        #endregion

        #region Right Click
        public void dataRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var dataSource = args.OriginalSource; //Gets right clicked item
                var dataDataContext = (dataSource as TextBlock).DataContext; //Gets the Data context
                rightClickedData = (Data)dataDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void emailRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var emailSource = args.OriginalSource; //Gets right clicked item
                var emailDataContext = (emailSource as TextBlock).DataContext; //Gets the data context
                rightClickedEmail = (Email)emailDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void schoolSendRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var schoolSendSource = args.OriginalSource; //Gets right clicked item
                var SchoolSendContext = (schoolSendSource as TextBlock).DataContext; //Gets the SchoolSend context
                rightClickedSchoolSend = (SchoolSend)SchoolSendContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void directMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var directMailingSource = args.OriginalSource; //Gets right clicked item
                var directMailingDataContext = (directMailingSource as TextBlock).DataContext; //Gets the data context
                rightClickedDirectMailing = (DirectMailing)directMailingDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void sharedMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var sharedMailingSource = args.OriginalSource; //Gets right clicked item
                var sharedMailingSharedMailingContext = (sharedMailingSource as TextBlock).DataContext; //Gets the SharedMailing context
                rightClickedSharedMailing = (SharedMailing)sharedMailingSharedMailingContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void printRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var printSource = args.OriginalSource; //Gets right clicked item
                var printDataContext = (printSource as TextBlock).DataContext; //Gets the Print context
                rightClickedPrint = (Print)printDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void surchargeRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var surchargeSource = args.OriginalSource; //Gets right clicked item
                var surchargeSurchargeContext = (surchargeSource as TextBlock).DataContext; //Gets the Surcharge context
                rightClickedSurcharge = (Surcharge)surchargeSurchargeContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }
        #endregion

        #region Go Back
        private RelayCommand _cancelDataPart;
        public RelayCommand cancelDataPart
        {
            get { if (_cancelDataPart == null) { _cancelDataPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 1; }); } return _cancelDataPart; }
        }

        private RelayCommand _cancelEmailPart;
        public RelayCommand cancelEmailPart
        {
            get { if (_cancelEmailPart == null) { _cancelEmailPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 2; }); } return _cancelEmailPart; }
        }

        private RelayCommand _cancelSchoolSendPart;
        public RelayCommand cancelSchoolSendPart
        {
            get { if (_cancelSchoolSendPart == null) { _cancelSchoolSendPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 3; }); } return _cancelSchoolSendPart; }
        }

        private RelayCommand _cancelDirectMailingPart;
        public RelayCommand cancelDirectMailingPart
        {
            get { if (_cancelDirectMailingPart == null) { _cancelDirectMailingPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 4; }); } return _cancelDirectMailingPart; }
        }

        private RelayCommand _cancelSharedMailingPart;
        public RelayCommand cancelSharedMailingPart
        {
            get { if (_cancelSharedMailingPart == null) { _cancelSharedMailingPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 5; }); } return _cancelSharedMailingPart; }
        }

        private RelayCommand _cancelPrintPart;
        public RelayCommand cancelPrintPart
        {
            get { if (_cancelPrintPart == null) { _cancelPrintPart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 6; }); } return _cancelPrintPart; }
        }

        private RelayCommand _cancelSurchargePart;
        public RelayCommand cancelSurchargePart
        {
            get { if (_cancelSurchargePart == null) { _cancelSurchargePart = new RelayCommand(() => { NavigationService.GoBack(); pivotIndex = 7; }); } return _cancelSurchargePart; }
        }

        private RelayCommand _cancelSharedPack;
        public RelayCommand cancelSharedPack
        {
            get { if (_cancelSharedPack == null) { _cancelSharedPack = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cancelSharedPack; }
        }
        #endregion
    }
}
