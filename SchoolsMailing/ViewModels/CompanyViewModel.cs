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
                    RaisePropertyChanged("selectedCompanyHistory");
                }
            }
        }
        private List<Contact> _companyContacts;
        public List<Contact> companyContacts
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
            GetHistory();

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


        //public List<CompanyHistory> companyHistoryList;

        #region Company History

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

        public void historyInvoked(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs; //Get invoked item
            var history = arg.ClickedItem as CompanyHistory; //Get history
            long HistoryID = history.ID;
            createCompanyHistoryDialog(HistoryID); //Create dialog
        }

        private RelayCommand _newCompanyHistory;
        public RelayCommand newCompanyHistory
        {
            get
            {
                if (_newCompanyHistory == null)
                {
                    _newCompanyHistory = new RelayCommand(() =>
                    {
                        createCompanyHistoryDialog();
                    });
                }

                return _newCompanyHistory;

            }
        }
                
        public async void createCompanyHistoryDialog(long HistoryID = 0)
        {
            if (HistoryID != 0)
            {
                invokedCompanyHistory = DataAccessLayer.GetHistoryByID(HistoryID); //Get company history
            }
            else
            {
                invokedCompanyHistory = new CompanyHistory(); //Create new history
                invokedCompanyHistory.companyHistoryDate = DateTime.Now; //Set date to today
            }

            var newHistoryDialog = new HistoryDialog(); //Create new input dialog
            newHistoryDialog.SecondaryButtonText = "Delete";
            var result = await newHistoryDialog.ShowAsync(); //Await result

            if(result == ContentDialogResult.Primary)
            {
                var item = newHistoryDialog.Content; //Get user input
                invokedCompanyHistory.companyHistoryDetails = item.ToString(); //Convert to string
                invokedCompanyHistory.companyID = selectedCompany.ID; //Attribute company
                DataAccessLayer.SaveHistory(invokedCompanyHistory); //Save history
            }
            else
            {

            }
            GetHistory();
        }

        public void GetHistory()
        {
            selectedCompanyHistory = DataAccessLayer.GetAllHistoryByCompany(ID);
        }
        

        #endregion
        

    }
}
