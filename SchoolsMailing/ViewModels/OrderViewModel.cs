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
                    emailOrders = DataAccessLayer.GetAllEmails(obj.Content);
                    dataOrders = DataAccessLayer.GetAllData(obj.Content);
                    schoolSendOrders = DataAccessLayer.GetAllSchoolSends(obj.Content);
                    sharedMailingOrders = DataAccessLayer.GetAllSharedMailings(obj.Content);
                    directMailingOrders = DataAccessLayer.GetAllDirectMailings(obj.Content);
                    printOrders = DataAccessLayer.GetAllPrint(obj.Content);
                    surchargeOrders = DataAccessLayer.GetAllSurcharges(obj.Content);

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
            }
            //else if(obj.Notification == "NewOrder")
            //{
            //    selectedCompany = DataAccessLayer.GetCompanyById(obj.Content);
            //}
        }
        
        #region Navigate to new orders
        private RelayCommand _newEmail;
        public RelayCommand newEmail
        {
            get { if (_newEmail == null) {
                    _newEmail = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(EmailView));
                        selectedEmailOrder = new Email() { emailDate = DateTime.Now };
                        originalEmailOrder = new Email();
                    });
                } return _newEmail;
            }
        }

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

        private RelayCommand _newSchoolSend;
        public RelayCommand newSchoolSend
        {
            get { if (_newSchoolSend == null) {
                    _newSchoolSend = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SchoolSendView));
                        selectedSchoolSendOrder = new SchoolSend() { schoolsendStart = DateTime.Now, schoolsendEnd = DateTime.Now };
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
                        originalSharedMailingOrder = new SharedMailing();
                    });
                } return _newSharedMailing;
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

        private RelayCommand _newPrint;
        public RelayCommand newPrint
        {
            get { if (_newPrint == null) {
                    _newPrint = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(PrintView));
                        selectedPrintOrder = new Print() { printDate = DateTime.Now };
                        originalPrintOrder = new Print();
                    });
                } return _newPrint;
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

        private RelayCommand _cancelNew; //Cancels new email/data/direct mailing/schoolsend/shared mailing/surcharge
        public RelayCommand cancelNew
        {
            get { if (_cancelNew == null) { _cancelNew = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cancelNew; }
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
                        foreach(Email e in emailOrders) //Add emails
                        {
                            e.orderID = orderID;
                            DataAccessLayer.SaveEmail(e);
                        }
                        foreach(Email e in deletedEmailOrders)
                        {
                            DataAccessLayer.DeleteEmail(e);
                        }
                        //Data
                        foreach(Data d in dataOrders)
                        {
                            d.orderID = orderID;
                            DataAccessLayer.SaveData(d);
                        }
                        foreach(Data d in deletedDataOrders)
                        {
                            DataAccessLayer.DeleteData(d);
                        }
                        //Direct mailing
                        foreach(DirectMailing dm in directMailingOrders)
                        {
                            dm.orderID = orderID;
                            DataAccessLayer.SaveDirectMailing(dm);
                        }
                        foreach(DirectMailing dm in deletedDirectMailingOrders)
                        {
                            DataAccessLayer.DeleteDirectMailing(dm);
                        }
                        //Shared mailing
                        foreach(SharedMailing sm in sharedMailingOrders)
                        {
                            sm.orderID = orderID;
                            DataAccessLayer.SaveSharedMailing(sm);
                        }
                        foreach(SharedMailing sm in deletedSharedMailingOrders)
                        {
                            DataAccessLayer.DeleteSharedMailing(sm);
                        }
                        //Surcharge
                        foreach(Surcharge s in surchargeOrders)
                        {
                            s.orderID = orderID;
                            DataAccessLayer.SaveSurcharge(s);
                        }
                        foreach(Surcharge s in deletedSurchargeOrders)
                        {
                            DataAccessLayer.DeleteSurcharge(s);
                        }
                        //Print
                        foreach(Print p in printOrders)
                        {
                            p.orderID = orderID;
                            DataAccessLayer.SavePrint(p);
                        }
                        foreach(Print p in deletedPrintOrders)
                        {
                            DataAccessLayer.DeletePrint(p);
                        }
                        //SchoolSend
                        foreach(SchoolSend ss in schoolSendOrders)
                        {
                            ss.orderID = orderID;
                            DataAccessLayer.SaveSchoolSend(ss);
                        }
                        foreach(SchoolSend ss in deletedSchoolSendOrders)
                        {
                            DataAccessLayer.DeleteSchoolSend(ss);
                        }
                    });
                }
                return _saveOrder;
            }
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
            set { if(_dataOrders != value) { _dataOrders = value; RaisePropertyChanged("dataOrders"); } }
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

        private RelayCommand _saveData;
        public RelayCommand saveData
        {
            get
            {
                if (_saveData == null)
                {
                    _saveData = new RelayCommand(() =>
                    {
                        if(ValidData(selectedDataOrder))
                        {
                            dataOrders.Remove(originalDataOrder);
                            dataOrders.Add(selectedDataOrder);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            Debug.WriteLine("Invalid data cost");
                        }
                        
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
                        if(rightClickedData != null)
                        {
                            deletedDataOrders.Add(rightClickedData);
                            dataOrders.Remove(rightClickedData);
                        }
                    });
                }

                return _deleteData;

            }
        }

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

        public Data rightClickedData = new Data(); //Holds right clicked item for flyout

        public void dataRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var dataSource = args.OriginalSource; //Gets right clicked item
                var dataDataContext = (dataSource as TextBlock).DataContext; //Gets the Data context
                rightClickedData = (Data)dataDataContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }

        public bool ValidData(Data data)
        {
            try
            {
                double x = Convert.ToDouble(data.dataCost);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        private RelayCommand _dataTextChanged;
        public RelayCommand dataTextChanged
        {
            get
            {
                if (_dataTextChanged == null)
                {
                    _dataTextChanged = new RelayCommand(() =>
                    {
                        Debug.WriteLine("text changed");
                    });
                }

                return _dataTextChanged;

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
            set { if(_selectedEmailOrder != value) { _selectedEmailOrder = value; RaisePropertyChanged("selectedEmailOrder"); } }
        }

        private Email _originalEmailOrder;
        public Email originalEmailOrder
        {
            get { return _originalEmailOrder; }
            set { if (_originalEmailOrder != value) { _originalEmailOrder = value; RaisePropertyChanged("originalEmailOrder"); } }
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
                        emailOrders.Remove(originalEmailOrder);
                        emailOrders.Add(selectedEmailOrder);
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
                        if(rightClickedEmail != null)
                        {
                            deletedEmailOrders.Add(rightClickedEmail);
                            emailOrders.Remove(rightClickedEmail);
                        }
                    });
                }
                return _deleteEmail;
            }
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
        
        public Email rightClickedEmail = new Email(); //Holds right clicked item for flyout

        public void emailRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var emailSource = args.OriginalSource; //Gets right clicked item
                var emailDataContext = (emailSource as TextBlock).DataContext; //Gets the data context
                rightClickedEmail = (Email)emailDataContext; //Convert to class
            }
            catch(Exception x) { } //Catch null exception
        }
        #endregion

        #region Direct Mailing Order
        private ObservableCollection<DirectMailing> _directMailingOrders;
        public ObservableCollection<DirectMailing> directMailingOrders
        {
            get { return _directMailingOrders; }
            set { if(_directMailingOrders != value) { _directMailingOrders = value;  RaisePropertyChanged("directMailingOrders"); } }
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
            set { if(_selectedDirectMailingOrder != value) { _selectedDirectMailingOrder = value; RaisePropertyChanged("selectedDirectMailingOrder"); } }
        }

        private DirectMailing _originalDirectMailingOrder;
        public DirectMailing originalDirectMailingOrder
        {
            get { return _originalDirectMailingOrder; }
            set { if (_originalDirectMailingOrder != value) { _originalDirectMailingOrder = value; RaisePropertyChanged("originalDirectMailingOrder"); } }
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
                        directMailingOrders.Remove(originalDirectMailingOrder);
                        directMailingOrders.Add(selectedDirectMailingOrder);
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
                        if(rightClickedDirectMailing != null)
                        {
                            deletedDirectMailingOrders.Add(rightClickedDirectMailing);
                            directMailingOrders.Remove(rightClickedDirectMailing);
                        }
                    });
                }

                return _deleteDirectMailing;
            }
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

        public DirectMailing rightClickedDirectMailing = new DirectMailing(); //Holds right clicked item for flyout

        public void directMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var directMailingSource = args.OriginalSource; //Gets right clicked item
                var directMailingDataContext = (directMailingSource as TextBlock).DataContext; //Gets the data context
                rightClickedDirectMailing = (DirectMailing)directMailingDataContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }
        #endregion

        #region Print Order
        private ObservableCollection<Print> _printOrders;
        public ObservableCollection<Print> printOrders
        {
            get { return _printOrders; }
            set { if(_printOrders != value) { _printOrders = value; RaisePropertyChanged("printOrders"); } }
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
            set { if(_selectedPrintOrder != value) { _selectedPrintOrder = value; RaisePropertyChanged("selectedPrintOrder"); } }
        }

        private Print _originalPrintOrder;
        public Print originalPrintOrder
        {
            get { return _originalPrintOrder; }
            set { if (_originalPrintOrder != value) { _originalPrintOrder = value; RaisePropertyChanged("originalPrintOrder"); } }
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
                        printOrders.Remove(originalPrintOrder);
                        printOrders.Add(selectedPrintOrder);
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
                        if(rightClickedPrint != null)
                        {
                            deletedPrintOrders.Add(rightClickedPrint);
                            printOrders.Remove(rightClickedPrint);
                        }
                    });
                }

                return _deletePrint;
            }
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

        public Print rightClickedPrint = new Print(); //Holds right clicked item for flyout

        public void printRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var printSource = args.OriginalSource; //Gets right clicked item
                var printDataContext = (printSource as TextBlock).DataContext; //Gets the Print context
                rightClickedPrint = (Print)printDataContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }
        #endregion

        #region SchoolSend Order
        private ObservableCollection<SchoolSend> _schoolSendOrders;
        public ObservableCollection<SchoolSend> schoolSendOrders
        {
            get { return _schoolSendOrders; }
            set { if(_schoolSendOrders != value) { _schoolSendOrders = value;  RaisePropertyChanged("schoolSendOrders"); } }
        }

        private ObservableCollection<SchoolSend> _deletedSchoolSendOrders;
        public ObservableCollection<SchoolSend> deletedSchoolSendOrders
        {
            get { return _deletedSchoolSendOrders; }
            set { if (_deletedSchoolSendOrders != value) { _deletedSchoolSendOrders = value; RaisePropertyChanged("deletedSchoolSendOrders"); } }
        }

        private SchoolSend _selectedSchoolSendOrder;
        public SchoolSend selectedSchoolSendOrder
        {
            get { return _selectedSchoolSendOrder; }
            set { if(_selectedSchoolSendOrder != value) { _selectedSchoolSendOrder = value; RaisePropertyChanged("selectedSchoolSendOrder"); } }
        }

        private SchoolSend _originalSchoolSendOrder;
        public SchoolSend originalSchoolSendOrder
        {
            get { return _originalSchoolSendOrder; }
            set { if (_originalSchoolSendOrder != value) { _originalSchoolSendOrder = value; RaisePropertyChanged("originalSchoolSendOrder"); } }
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
                        schoolSendOrders.Remove(originalSchoolSendOrder);
                        schoolSendOrders.Add(selectedSchoolSendOrder);
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
                            Debug.WriteLine(string.Format(selectedSchoolSendOrder.schoolsendPackage.ToString()));
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
                        if(rightClickedSchoolSend != null)
                        {
                            deletedSchoolSendOrders.Add(rightClickedSchoolSend);
                            schoolSendOrders.Remove(rightClickedSchoolSend);
                        }
                    });
                }

                return _deleteSchoolSend;
            }
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

        public SchoolSend rightClickedSchoolSend = new SchoolSend(); //Holds right clicked item for flyout

        public void schoolSendRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var schoolSendSource = args.OriginalSource; //Gets right clicked item
                var schoolSendSchoolSendContext = (schoolSendSource as TextBlock).DataContext; //Gets the SchoolSend context
                rightClickedSchoolSend = (SchoolSend)schoolSendSchoolSendContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }
        #endregion

        #region Shared Mailing Order
        private ObservableCollection<SharedMailing> _sharedMailingOrders;
        public ObservableCollection<SharedMailing> sharedMailingOrders
        {
            get { return _sharedMailingOrders; }
            set { if(_sharedMailingOrders != value) { _sharedMailingOrders = value; RaisePropertyChanged("sharedMailingOrders"); } }
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
            set { if(_selectedSharedMailingOrder != value) { _selectedSharedMailingOrder = value; RaisePropertyChanged("selectedSharedMailingOrder"); } }
        }

        private SharedMailing _originalSharedMailingOrder;
        public SharedMailing originalSharedMailingOrder
        {
            get { return _originalSharedMailingOrder; }
            set { if (_originalSharedMailingOrder != value) { _originalSharedMailingOrder = value; RaisePropertyChanged("originalSharedMailingOrder"); } }
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
                        sharedMailingOrders.Remove(originalSharedMailingOrder);
                        sharedMailingOrders.Add(selectedSharedMailingOrder);
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
                        if(rightClickedSharedMailing != null)
                        {
                            deletedSharedMailingOrders.Add(rightClickedSharedMailing);
                            sharedMailingOrders.Remove(rightClickedSharedMailing);
                        }
                    });
                }

                return _deleteSharedMailing;
            }
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

        public SharedMailing rightClickedSharedMailing = new SharedMailing(); //Holds right clicked item for flyout

        public void sharedMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var sharedMailingSource = args.OriginalSource; //Gets right clicked item
                var sharedMailingSharedMailingContext = (sharedMailingSource as TextBlock).DataContext; //Gets the SharedMailing context
                rightClickedSharedMailing = (SharedMailing)sharedMailingSharedMailingContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }
        #endregion

        #region Surcharge Order
        private ObservableCollection<Surcharge> _surchargeOrders;
        public ObservableCollection<Surcharge> surchargeOrders
        {
            get { return _surchargeOrders; }
            set { if(_surchargeOrders != value) { _surchargeOrders = value; RaisePropertyChanged("surchargeOrders"); } }
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
            set { if(_selectedSurchargeOrder != value) { _selectedSurchargeOrder = value; RaisePropertyChanged("selectedSurchargeOrder"); } }
        }

        private Surcharge _originalSurchargeOrder;
        public Surcharge originalSurchargeOrder
        {
            get { return _originalSurchargeOrder; }
            set { if (_originalSurchargeOrder != value) { _originalSurchargeOrder = value; RaisePropertyChanged("originalSurchargeOrder"); } }
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
                        surchargeOrders.Remove(originalSurchargeOrder);
                        surchargeOrders.Add(selectedSurchargeOrder);
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
                        if(rightClickedSurcharge != null)
                        {
                            deletedSurchargeOrders.Add(rightClickedSurcharge);
                            surchargeOrders.Remove(rightClickedSurcharge);
                        }
                    });
                }

                return _deleteSurcharge;
            }
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

        public Surcharge rightClickedSurcharge = new Surcharge(); //Holds right clicked item for flyout

        public void surchargeRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var surchargeSource = args.OriginalSource; //Gets right clicked item
                var surchargeSurchargeContext = (surchargeSource as TextBlock).DataContext; //Gets the Surcharge context
                rightClickedSurcharge = (Surcharge)surchargeSurchargeContext; //Convert to class
            }
            catch (Exception x) { } //Catch null exception
        }
        #endregion
    }
}
