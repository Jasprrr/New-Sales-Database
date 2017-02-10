using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.ViewModels.Common;
using SchoolsMailing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolsMailing.DAL;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using SchoolsMailing.Views;
using Windows.UI.Xaml.Controls;

namespace SchoolsMailing.ViewModels
{
    public class NewCompanyViewModel : PageViewModel
    {
        #region Variables
        private Company _newCompany = new Company() { companyCallBack = DateTime.Now, companyLastCall = DateTime.Now };
        public Company newCompany
        {
            get { return _newCompany; }
            set { if (_newCompany != value) { _newCompany = value; RaisePropertyChanged("newCompany"); } }
        }
        #endregion

        #region Commands
        private RelayCommand _cmdCancelCompany;
        public RelayCommand cmdCancelCompany
        {
            get { if (_cmdCancelCompany == null) { _cmdCancelCompany = new RelayCommand(() => { NavigationService.GoBack(); newCompany = null; }); } return _cmdCancelCompany; }
        }

        private RelayCommand _cmdSaveCompany;
        public RelayCommand cmdSaveCompany
        {
            get { if (_cmdSaveCompany == null) { _cmdSaveCompany = new RelayCommand(() => { SaveCompany(); }); } return _cmdSaveCompany; }
        }
        #endregion

        public NewCompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            
        }
        
        //TODO: Company verification
        
        public async void SaveCompany()
        {
            if(newCompany.companyInvoiceAddress1 == null &&
                newCompany.companyInvoiceAddress2 == null &&
                newCompany.companyInvoiceCity == null &&
                newCompany.companyInvoiceCounty == null &&
                newCompany.companyInvoiceName == null && 
                newCompany.companyInvoicePostCode == null)
            {
                ContentDialog deleteContactDialog = new ContentDialog()
                {
                    Title = "Auto Fill Invoice Fields",
                    Content = "Do you want to automatically fill invoice data from company data?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };
                var result = await deleteContactDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    newCompany.companyInvoiceName     = (newCompany.companyName != null)     ? newCompany.companyName     : null;
                    newCompany.companyInvoiceAddress1 = (newCompany.companyAddress1 != null) ? newCompany.companyAddress1 : null;
                    newCompany.companyInvoiceAddress2 = (newCompany.companyAddress2 != null) ? newCompany.companyAddress2 : null;
                    newCompany.companyInvoiceCity     = (newCompany.companyCity != null)     ? newCompany.companyCity     : null;
                    newCompany.companyInvoiceCounty   = (newCompany.companyCounty != null)   ? newCompany.companyCounty   : null;
                    newCompany.companyInvoicePostCode = (newCompany.companyPostCode != null) ? newCompany.companyPostCode : null;
                }
            }
            
            newCompany.companyCreated = DateTime.Now;
            newCompany.companyModified = DateTime.Now;
            newCompany.companyInitial = newCompany.companyName.Substring(0, 1);
            //DateTimeOffset
            DataAccessLayer.saveCompany(newCompany);
            newCompany = new Company();
            NavigationService.GoBack();
        }

    }
}
