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

        #region Variables
        //Selected company
        private Company _selectedCompany = new Company();
        public Company selectedCompany
        {
            get { return _selectedCompany; }
            set { if (_selectedCompany != value) { _selectedCompany = value; RaisePropertyChanged("selectedCompany"); } }
        }

        //Contact selected from drop down
        private Contact _selectedContact; 
        public Contact selectedContact
        {
            get { return _selectedContact; }
            set { if (_selectedContact != value) { _selectedContact = value; RaisePropertyChanged("selectedContact"); } }
        }

        //History selected from list
        private CompanyHistory _selectedCompanyHistory = new CompanyHistory();
        public CompanyHistory selectedCompanyHistory
        {
            get { return _selectedCompanyHistory; }
            set { if (_selectedCompanyHistory != value) { _selectedCompanyHistory = value; RaisePropertyChanged("selectedCompanyHistory"); } }
        }

        //List of company history records
        private List<CompanyHistory> _companyHistory;
        public List<CompanyHistory> companyHistory
        {
            get { return _companyHistory; }
            set { if (_companyHistory != value) { _companyHistory = value; RaisePropertyChanged("companyHistory"); } }
        }

        //List of the companies conctacts
        private List<Contact> _companyContacts;
        public List<Contact> companyContacts
        {
            get { return _companyContacts; }
            set { if (_companyContacts != value) { _companyContacts = value; RaisePropertyChanged("companyContacts"); } }
        }
        #endregion

        #region Commands
        private RelayCommand _cmdNewContact;
        public RelayCommand cmdNewContact
        {
            get { if (_cmdNewContact == null) { _cmdNewContact = new RelayCommand(() => { NewContact(); }); } return _cmdNewContact; }
        }

        private RelayCommand _cmdNewCompanyHistory;
        public RelayCommand cmdNewCompanyHistory
        {
            get { if (_cmdNewCompanyHistory == null) { _cmdNewCompanyHistory = new RelayCommand(() => { EditCompanyHistory(); }); } return _cmdNewCompanyHistory; }
        }

        private RelayCommand _cmdRefreshCompany;
        public RelayCommand cmdRefreshCompany
        {
            get { if (_cmdRefreshCompany == null) { _cmdRefreshCompany = new RelayCommand(() => { GetCompany(selectedCompany.ID); GetContacts(selectedCompany.ID); GetHistory(selectedCompany.ID); }); } return _cmdRefreshCompany; }
        }

        private RelayCommand _cmdSaveCompany;
        public RelayCommand cmdSaveCompany
        {
            get { if (_cmdSaveCompany == null) { _cmdSaveCompany = new RelayCommand(() => { SaveCompany(); }); } return _cmdSaveCompany; }
        }

        private RelayCommand _cmdDeleteCompany;
        public RelayCommand cmdDeleteCompany
        {
            get { if (_cmdDeleteCompany == null) { _cmdDeleteCompany = new RelayCommand(() => { DeleteCompany(); }); } return _cmdDeleteCompany; }
        }

        private RelayCommand _cmdDeleteContact;
        public RelayCommand cmdDeleteContact
        {
            get { if (_cmdDeleteContact == null) { _cmdDeleteContact = new RelayCommand(() => { DeleteContact(); }); } return _cmdDeleteContact; }
        }

        private RelayCommand _cmdGoBack;
        public RelayCommand cmdGoBack
        {
            get { if (_cmdGoBack == null) { _cmdGoBack = new RelayCommand(() => { GoBack(); }); } return _cmdGoBack; }
        }

        public void cmdHistoryInvoked(object sender, object parameter)
        {
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs; //Get invoked item
            var history = arg.ClickedItem as CompanyHistory; //Get history
            long HistoryID = history.ID;
            EditCompanyHistory(HistoryID); //Create dialog
        }
        #endregion

        public CompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //Register company parameter
            MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp);
            //Register can go back
            MessengerInstance.Send<string>("false");
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

        public void NewContact()
        {
            //Navigate to new contact page
            this.NavigationService.Navigate(typeof(NewContact));
            //Pass company ID parameter
            MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<long>(selectedCompany.ID, "NewContactViewModel"));
        }
        
        public void GetCompany(long ID)
        {
            selectedCompany = DataAccessLayer.getCompanyByID(ID);
        }
        
        public void GetContacts(long ID)
        {
            companyContacts = DataAccessLayer.getContactsByCompanyID(ID);
        }
        
        public void GetHistory(long ID)
        {
            companyHistory = DataAccessLayer.getHistoryByCompanyID(ID);
        }

        public async void EditCompanyHistory(long HistoryID = 0)
        {
            var newHistoryDialog = new HistoryDialog(); //Create new input dialog

            if (HistoryID != 0)
            {
                selectedCompanyHistory = DataAccessLayer.getCompanyHistoryByID(HistoryID); //Get company history
                newHistoryDialog.SecondaryButtonText = "Delete"; //Set secondary option
            }
            else
            {
                selectedCompanyHistory = new CompanyHistory(); //Create new history
                selectedCompanyHistory.companyHistoryDate = DateTime.Now; //Set date to today
                newHistoryDialog.SecondaryButtonText = "Cancel"; //Set secondary option
            }

            var result = await newHistoryDialog.ShowAsync(); //Show dialog & await result

            if (result == ContentDialogResult.Primary) //If primary option chosen
            {
                var item = newHistoryDialog.Content; //Get user input
                selectedCompanyHistory.companyHistoryDetails = item.ToString(); //Convert to string
                selectedCompanyHistory.companyID = selectedCompany.ID; //Attribute company
                DataAccessLayer.saveHistory(selectedCompanyHistory); //Save history
                GetHistory(selectedCompany.ID); //Refresh history
            }
            else if (result == ContentDialogResult.Secondary) //If secondary option chosen
            {
                if (HistoryID != 0)
                {
                    DataAccessLayer.deleteHistory(selectedCompanyHistory);
                    GetHistory(selectedCompany.ID); //Refresh history
                }
            }
        }

        public void SaveCompany()
        {
            selectedCompany.companyModified = DateTime.Now; //Set date modified
            selectedCompany.companyInitial = selectedCompany.companyName.Substring(0, 1); //Set company initial
            DataAccessLayer.saveCompany(selectedCompany);
            foreach (Contact c in companyContacts) //Loop through contacts
            {
                DataAccessLayer.saveContact(c); //Save contacts
            }
        }

        public async void DeleteCompany()
        {
            ContentDialog deleteCompanyDialog = new ContentDialog()
            {
                Title = "Delete this company?",
                Content = "If you delete this company, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };

            var result = await deleteCompanyDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                DataAccessLayer.deleteCompany(selectedCompany);
                NavigationService.GoBack();
            }
            else if (result == ContentDialogResult.Secondary)
            {

            }
        }

        public async void DeleteContact()
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
        
        public bool isDirty = false;
        
        public async void GoBack()
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
                    SaveCompany();
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
    }
}
