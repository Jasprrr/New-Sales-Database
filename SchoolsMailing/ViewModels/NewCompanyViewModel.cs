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

        private string _newCompanyName;
        public string newCompanyName
        {
            get { return _newCompanyName; }
            set
            {
                if(_newCompanyName != value)
                {
                    _newCompanyName = value;
                }
            }
        }

        private string _newCompanyAddress1;
        public string newCompanyAddress1
        {
            get { return _newCompanyAddress1; }
            set
            {
                if (_newCompanyAddress1 != value)
                {
                    _newCompanyAddress1 = value;
                }
            }
        }

        private string _newCompanyAddress2;
        public string newCompanyAddress2
        {
            get { return _newCompanyAddress2; }
            set
            {
                if (_newCompanyAddress2 != value)
                {
                    _newCompanyAddress2 = value;
                }
            }
        }

        private string _newCompanyCity;
        public string newCompanyCity
        {
            get { return _newCompanyCity; }
            set
            {
                if (_newCompanyCity != value)
                {
                    _newCompanyCity = value;
                }
            }
        }

        private string _newCompanyCounty;
        public string newCompanyCounty
        {
            get { return _newCompanyCounty; }
            set
            {
                if (_newCompanyCounty != value)
                {
                    _newCompanyCounty = value;
                }
            }
        }

        private string _newCompanyPostCode;
        public string newCompanyPostCode
        {
            get { return _newCompanyPostCode; }
            set
            {
                if (_newCompanyPostCode != value)
                {
                    _newCompanyPostCode = value;
                }
            }
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
                        Company c = new Company();
                        c.companyName = newCompanyName;
                        c.companyAddress1 = newCompanyAddress1;
                        c.companyAddress2 = newCompanyAddress2;
                        c.companyCity = newCompanyCity;
                        c.companyCounty = newCompanyCounty;
                        c.companyPostCode = newCompanyPostCode;
                        c.companyInitial = newCompanyName.Substring(0, 1);

                        DataAccessLayer.SaveCompany(c);

                        string _companyID = c.ID.ToString();
                        this.NavigationService.GoBack();
                        this.NavigationService.Navigate(typeof(CompanyView));
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>("CompanyID", _companyID));

                        newCompanyName = null;
                        newCompanyAddress1 = null;
                        newCompanyAddress2 = null;
                        newCompanyCity = null;
                        newCompanyCounty = null;
                        newCompanyPostCode = null;
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
                    });
                }

                return _cancelCompany;

            }
        }
    }
}
