using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SchoolsMailing.Models;
using SchoolsMailing.DAL;
using GalaSoft.MvvmLight.Command;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using static SchoolsMailing.ViewModels.CompanyViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using SchoolsMailing.Controls;

namespace SchoolsMailing.ViewModels
{
    public class CompanyViewModel : PageViewModel
    {
        //http://www.c-sharpcorner.com/UploadFile/0cb003/contentdialog-in-universal-windows-program-part-1/
        //private IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public CompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            // register company parameter
            MessengerInstance.Register<NotificationMessage<string>>(this, setCompanyID);
            addTestData();
            MessengerInstance.Send<string>("false");
        }

        private ObservableCollection<CompanyDataOrder> _companyDataOrder;
        public ObservableCollection<CompanyDataOrder> companyDataOrder
        {
            get { return _companyDataOrder; }
            set
            {
                if(_companyDataOrder != value)
                {
                    _companyDataOrder = value;
                    RaisePropertyChanged("companyDataOrder");
                }
            }
        }
        private ObservableCollection<CompanyHistory> _selectedCompanyHistory;
        public ObservableCollection<CompanyHistory> selectedCompanyHistory
        {
            get { return _selectedCompanyHistory; }
            set
            {
                if(_selectedCompanyHistory != value)
                {
                    _selectedCompanyHistory = value;
                    RaisePropertyChanged("companyHistory");
                }
            }
        }
        private ObservableCollection<Contact> _companyContacts;
        public ObservableCollection<Contact> companyContacts
        {
            get { return _companyContacts; }
            set
            {
                if(_companyContacts != value)
                {
                    _companyContacts = value;
                    RaisePropertyChanged("companyContacts");
                }
            }
        }

        public async void addTestData()
        {
            ObservableCollection<CompanyDataOrder> c = new ObservableCollection<CompanyDataOrder>();
            CompanyDataOrder d1 = new CompanyDataOrder() { companyID = 1, dataCost = 12500.00, dataDetails = "a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 ", orderDate = Convert.ToDateTime("01/01/16"), dataID = 1, orderCode = "L200"};
            CompanyDataOrder d2 = new CompanyDataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 2", orderDate = Convert.ToDateTime("02/01/16"), dataID = 1, orderCode = "L201"};
            CompanyDataOrder d3 = new CompanyDataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 3", orderDate = Convert.ToDateTime("03/01/16"), dataID = 1, orderCode = "L202"};
            CompanyDataOrder d4 = new CompanyDataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 4", orderDate = Convert.ToDateTime("04/01/16"), dataID = 1, orderCode = "L204"};
            CompanyDataOrder d5 = new CompanyDataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 5", orderDate = Convert.ToDateTime("05/01/16"), dataID = 1, orderCode = "L205"};
            CompanyDataOrder d6 = new CompanyDataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 6", orderDate = Convert.ToDateTime("06/01/16"), dataID = 1, orderCode = "L206"};
            c.Add(d1);
            c.Add(d2);
            c.Add(d3);
            c.Add(d4);
            c.Add(d5);
            c.Add(d6);
            companyDataOrder = c;
            ObservableCollection<CompanyHistory> h = new ObservableCollection<CompanyHistory>();
            CompanyHistory h1 = new CompanyHistory() { companyHistoryDate = Convert.ToDateTime("02/01/16"), isVisible = true, companyHistoryDetails = "Some details about this company Some details about this company Some details about this company Some details about this company Some details about this company", companyID=1, ID=1 };
            CompanyHistory h2 = new CompanyHistory() { companyHistoryDate = Convert.ToDateTime("03/01/16"), isVisible = true, companyHistoryDetails = "Some details about this company", companyID = 1, ID = 1 };
            h.Add(h1);
            h.Add(h2);
            selectedCompanyHistory = h;

            //var txtBox = new TextBox { Width=200, MinHeight=32 };
            //Binding binding = new Binding() { Path = new PropertyPath("123") };
            //txtBox.SetBinding(TextBox.TextProperty, binding);
            //historyDialog.Content = txtBox;
            //await historyDialog.ShowAsync();

            //var dial = new HistoryDialog()
            //{
            //    DataContext = new
            //    {
            //        companyHistory = h1.companyHistoryDetails
            //    }
            //};

            //var result = await dial.ShowAsync();

            //if (result == ContentDialogResult.Primary)
            //{
            //    var item = (TextBox)dial.Content;
            //}
            //else
            //{
            //    Debug.Write("Canceled!");
            //}
        }

        #region Company Data
        //Selected company's ID
        public int ID { get; private set; }

        private Company _selectedCompany;
        public Company selectedCompany {
            get { return _selectedCompany; }
            set
            {
                if (_selectedCompany != value)
                {
                    _selectedCompany = value;
                    //Refresh bindings
                    RaisePropertyChanged("selectedCompany");
                }
            }
        }
        //Allows binding from DateTime to DateTimeOffset
        public DateTimeOffset companyCallBack
        {
            get
            {
                DateTime dateTimeToOffset = DateTime.SpecifyKind(selectedCompany.companyCallBackDate, DateTimeKind.Utc);
                DateTimeOffset OffsetCallBack = dateTimeToOffset;
                return dateTimeToOffset;
            }
        }

        #endregion

        

        //public MobileServiceCollection<Company, Company> companyCollection;

        public async void setCompanyID(NotificationMessage<string> obj)
        {
            //Name of parameter
            string content = obj.Content;
            //Parameter
            string notification = obj.Notification;
            try
            {
                //Convert ID string to integer
                ID = Int32.Parse(notification);
            }
            catch (FormatException e)
            {
                Debug.Write(e.Message);
            }

            selectedCompany = DataAccessLayer.GetCompanyById(ID);
            //companyContacts = DataAccessLayer.

            //MobileServiceInvalidOperationException exception = null;
            //try
            //{
            //    //Turn azure table to bindable collection
            //    companyCollection = await companyTable
            //                            .Where(selectedCompany => selectedCompany.ID == ID)
            //                            .ToCollectionAsync();
            //    //Get record from collection
            //    selectedCompany = companyCollection.FirstOrDefault();
            //}
            //catch (MobileServiceInvalidOperationException e)
            //{
            //    exception = e;
            //}
        }

        private RelayCommand _saveCompany;
        public RelayCommand saveCompany
        {
            get
            {
                if (_saveCompany == null)
                {
                    _saveCompany = new RelayCommand(() =>
                    {
                        selectedCompany.companyModified = DateTime.Now;
                        selectedCompany.companyInitial = selectedCompany.companyName.Substring(0, 1);
                        DataAccessLayer.SaveCompany(selectedCompany);
                    });
                }

                return _saveCompany;

            }
        }

        public void historyInvoked(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get clicked company
            var history = arg.ClickedItem as CompanyHistory;
            //Get clicked company ID

            try
            {
                h = Int32.Parse(history.ToString());
            }
            catch (FormatException e) { }
        }

        #region Company History

        private RelayCommand _newCompanyHistory;
        public RelayCommand newCompanyHistory
        {
            get
            {
                if (_newCompanyHistory == null)
                {
                    _newCompanyHistory = new RelayCommand(() =>
                    {

                    });
                }

                return _newCompanyHistory;

            }
        }

        private CompanyHistory _invokedCompanyHistory;
        public CompanyHistory invokedCompanyHistory
        {
            get { return _invokedCompanyHistory; }
            set
            {
                if (_invokedCompanyHistory != value)
                {
                    _invokedCompanyHistory = value;
                    RaisePropertyChanged("selectedCompanyHistory");
                }
            }
        }
        
        public async void createCompanyHistoryDialog(long HistoryID = 0)
        {
            if (HistoryID != 0)
            {
                invokedCompanyHistory = DataAccessLayer.GetHistoryByID(HistoryID);
            }
            else
            {
                invokedCompanyHistory = new CompanyHistory();
            }

            var newHistoryDialog = new HistoryDialog();
            var result = await newHistoryDialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                var item = (TextBox)newHistoryDialog.Content;
            }
            else
            {

            }
        }

        #endregion
        

    }
}
