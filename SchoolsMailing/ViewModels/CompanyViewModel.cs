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
using SchoolsMailing.Views;

namespace SchoolsMailing.ViewModels
{
    public class CompanyViewModel : PageViewModel
    {
        //http://www.c-sharpcorner.com/UploadFile/0cb003/contentdialog-in-universal-windows-program-part-1/
        //private IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public CompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            
            MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp); // register company parameter
            MessengerInstance.Send<string>("false"); //Register can go back
        }

        public void SetUp(NotificationMessage<Int64> obj)
        {
            if (obj.Notification == "CompanyViewModel")
            {
                long id = obj.Content;
                GetCompany(id);
                GetContacts(id);
                GetHistory(id);
            }
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

        //Initialise company
        public void GetCompany(long id)
        {
            selectedCompany = DataAccessLayer.getCompanyByID(id);
        }
        //Get company contacts
        public void GetContacts(long id)
        {
            companyContacts = DataAccessLayer.getContactsByCompanyID(id);
        }
        //Get company history
        public void GetHistory(long id)
        {
            selectedCompanyHistory = DataAccessLayer.getHistoryByCompanyID(id);
        }

        #region Data Lists
        private List<CompanyHistory> _selectedCompanyHistory;
        public List<CompanyHistory> selectedCompanyHistory
        {
            get { return _selectedCompanyHistory; }
            set { if(_selectedCompanyHistory != value) { _selectedCompanyHistory = value; RaisePropertyChanged("selectedCompanyHistory"); } }
        }

        private List<Contact> _companyContacts;
        public List<Contact> companyContacts
        {
            get { return _companyContacts; }
            set { if(_companyContacts != value) { _companyContacts = value; RaisePropertyChanged("companyContacts"); } }
        }

        #endregion

        #region Company Data

        //Selected company's ID
        public int ID { get; private set; }

        private Company _selectedCompany = new Company();
        public Company selectedCompany {
            get { return _selectedCompany; }
            set { if (_selectedCompany != value) { _selectedCompany = value; RaisePropertyChanged("selectedCompany"); } }
        }

        public async void deleteCompanyDialog()
        {
            ContentDialog deleteCompanyDialog = new ContentDialog()
            {
                Title = "Delete this company?",
                Content = "If you delete this company, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            var result = await deleteCompanyDialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                DataAccessLayer.deleteCompany(selectedCompany);
                NavigationService.GoBack();
            }
            else if(result == ContentDialogResult.Secondary)
            {

            }
        }
        #endregion

        #region Contact Data
        
        private Contact _selectedContact; //Contact selected from drop down
        public Contact selectedContact
        {
            get { return _selectedContact; }
            set { if (_selectedContact != value) { _selectedContact = value; RaisePropertyChanged("selectedContact"); } }
        }

        public async void deleteContactDialog()
        {
            ContentDialog deleteContactDialog = new ContentDialog()
            {
                Title = "Delete this contact?",
                Content = "If you delete this contact, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            var result = await deleteContactDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                DataAccessLayer.deleteContact(selectedContact);
            }
            else if (result == ContentDialogResult.Secondary)
            {

            }
        }
        #endregion

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
                        createCompanyHistoryDialog(); //Create dialog
                    });
                }

                return _newCompanyHistory;

            }
        }
                
        public async void createCompanyHistoryDialog(long HistoryID = 0)
        {
            var newHistoryDialog = new HistoryDialog(); //Create new input dialog

            if (HistoryID != 0)
            {
                invokedCompanyHistory = DataAccessLayer.getCompanyHistoryByID(HistoryID); //Get company history
                newHistoryDialog.SecondaryButtonText = "Delete"; //Set secondary option
            }
            else
            {
                invokedCompanyHistory = new CompanyHistory(); //Create new history
                invokedCompanyHistory.companyHistoryDate = DateTime.Now; //Set date to today
                newHistoryDialog.SecondaryButtonText = "Cancel"; //Set secondary option
            }

            var result = await newHistoryDialog.ShowAsync(); //Show dialog & await result

            if(result == ContentDialogResult.Primary) //If primary option chosen
            {
                var item = newHistoryDialog.Content; //Get user input
                invokedCompanyHistory.companyHistoryDetails = item.ToString(); //Convert to string
                invokedCompanyHistory.companyID = selectedCompany.ID; //Attribute company
                DataAccessLayer.saveHistory(invokedCompanyHistory); //Save history
                GetHistory(selectedCompany.ID); //Refresh history
            }
            else if(result == ContentDialogResult.Secondary) //If secondary option chosen
            {
                if(HistoryID != 0)
                {
                    DataAccessLayer.deleteHistory(invokedCompanyHistory);
                    GetHistory(selectedCompany.ID); //Refresh history
                }
            }
        }

        #endregion

        #region Commands

        private RelayCommand _newContact;
        public RelayCommand newContact
        {
            get
            {
                if (_newContact == null)
                {
                    _newContact = new RelayCommand(() =>
                    {
                        this.NavigationService.Navigate(typeof(NewContact)); //Navigate to view
                        MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<long>(selectedCompany.ID, "NewContactViewModel")); //Send company parameter
                    });
                }
                return _newContact;
            }
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
                        selectedCompany.companyModified = DateTime.Now; //Set date modified
                        selectedCompany.companyInitial = selectedCompany.companyName.Substring(0, 1); //Set company initial
                        DataAccessLayer.saveCompany(selectedCompany);
                        foreach (Contact c in companyContacts) //Loop through contacts
                        {
                            DataAccessLayer.saveContact(c); //Save contacts
                        }
                    });
                }
                return _saveCompany;
            }
        }
        
        private RelayCommand _refreshCompany;
        public RelayCommand refreshCompany
        {
            get
            {
                if (_refreshCompany == null)
                {
                    _refreshCompany = new RelayCommand(() =>
                    {
                        GetCompany(selectedCompany.ID);
                        GetContacts(selectedCompany.ID);
                        GetHistory(selectedCompany.ID);
                    });
                }
                return _refreshCompany;
            }
        }

        private RelayCommand _deleteCompany;
        public RelayCommand deleteCompany
        {
            get
            {
                if (_deleteCompany == null)
                {
                    _deleteCompany = new RelayCommand(() =>
                    {
                        deleteCompanyDialog();
                    });
                }
                return _deleteCompany;
            }
        }

        private RelayCommand _deleteContact;
        public RelayCommand deleteContact
        {
            get
            {
                if (_deleteContact == null)
                {
                    _deleteContact = new RelayCommand(() =>
                    {
                        deleteContactDialog();
                    });
                }
                return _deleteContact;
            }
        }

        public bool isDirty = false;

        private RelayCommand _goBack;
        public RelayCommand goBack
        {
            get
            {
                if (_goBack == null)
                {
                    _goBack = new RelayCommand(() =>
                    {
                        cmdGoBack();
                    });
                }
                return _goBack;
            }
        }

        public async void cmdGoBack()
        {
            if (isDirty == true)
            {
                ContentDialog isDirtyDialog = new ContentDialog()
                {
                    Title = "Unsaved Changes",
                    Content = string.Format("Do you want to save changes to {0}", selectedCompany.companyName),
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "No"
                };

                var result = await isDirtyDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    selectedCompany.companyModified = DateTime.Now; //Set date modified
                    selectedCompany.companyInitial = selectedCompany.companyName.Substring(0, 1); //Set company initial
                    DataAccessLayer.saveCompany(selectedCompany);
                    foreach (Contact c in companyContacts) //Loop through contacts
                    {
                        DataAccessLayer.saveContact(c); //Save contacts
                    }
                    isDirty = false;
                    NavigationService.GoBack();
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    isDirty = false;
                    NavigationService.GoBack();
                }
            }
            else
            {
                isDirty = false;
                NavigationService.GoBack();
            }
        }
        #endregion
    }
}
