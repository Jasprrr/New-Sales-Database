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
        public NewCompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            
        }

        #region Company Data
        private Company _newCompany = new Company() { companyCallBack = DateTime.Now, companyLastCall = DateTime.Now };
        public Company newCompany
        {
            get { return _newCompany; }
            set { if(_newCompany != value) { _newCompany = value; RaisePropertyChanged("newCompany"); } }
        }
        #endregion

        private RelayCommand _saveCompany;
        public RelayCommand saveCompany
        {
            get
            {
                if (_saveCompany == null)
                {
                    _saveCompany = new RelayCommand(() =>
                    {
                        DALSave();
                    });
                }
                return _saveCompany;
            }
        }

        public async void DALSave()
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
                    if (newCompany.companyName != null) { newCompany.companyInvoiceName = newCompany.companyName; }
                    if (newCompany.companyAddress1 != null) { newCompany.companyInvoiceAddress1 = newCompany.companyAddress1; }
                    if (newCompany.companyAddress2 != null) { newCompany.companyInvoiceAddress2 = newCompany.companyAddress2; }
                    if (newCompany.companyCity != null) { newCompany.companyInvoiceCity = newCompany.companyCity; }
                    if (newCompany.companyCounty != null) { newCompany.companyInvoiceCounty = newCompany.companyCounty; }
                    if (newCompany.companyPostCode != null) { newCompany.companyInvoicePostCode = newCompany.companyPostCode; }
                }
            }
            
            newCompany.companyCreated = DateTime.Now;
            newCompany.companyModified = DateTime.Now;
            newCompany.companyInitial = newCompany.companyName.Substring(0, 1);
            //DateTimeOffset
            DataAccessLayer.SaveCompany(newCompany);
            newCompany = new Company();
            NavigationService.GoBack();
        }

        private RelayCommand _cancelCompany;
        public RelayCommand cancelCompany
        {
            get
            {
                if (_cancelCompany == null)
                {
                    _cancelCompany = new RelayCommand(() =>
                    {
                        NavigationService.GoBack();
                        newCompany = null;
                    });
                }

                return _cancelCompany;

            }
        }
    }
}
