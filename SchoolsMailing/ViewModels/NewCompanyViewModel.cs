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

namespace SchoolsMailing.ViewModels
{
    public class NewCompanyViewModel : PageViewModel
    {
        public NewCompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            
        }

        #region Company Data
        private Company _newCompany = new Company();
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
                        //if(asd.Value != null) { newCompany.companyCallBackDate = asd.Value.Date; }
                        newCompany.companyCreated = DateTime.Now;
                        newCompany.companyModified = DateTime.Now;
                        newCompany.companyInitial = newCompany.companyName.Substring(0, 1);
                        //DateTimeOffset
                        DataAccessLayer.SaveCompany(newCompany); 

                        //string _companyID = c.ID.ToString();
                        //this.NavigationService.GoBack();
                        //this.NavigationService.Navigate(typeof(CompanyView));
                        //MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>("CompanyID", _companyID));

                        newCompany = null;
                    });
                }

                return _saveCompany;

            }
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
