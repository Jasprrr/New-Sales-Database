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

namespace SchoolsMailing.ViewModels
{
    public class CompanyViewModel : PageViewModel
    {
        //private IMobileServiceTable<Company> companyTable = App.MobileService.GetTable<Company>();

        public CompanyViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            // register parameter
            MessengerInstance.Register<NotificationMessage<string>>(this, setCompanyID);
        }

        private Boolean _postCodeRequested = false;
        public Boolean postCodeRequested
        {
            get { return _postCodeRequested; }
            set
            {
                if (_postCodeRequested != value)
                {
                    _postCodeRequested = !_postCodeRequested;
                    RaisePropertyChanged("postCodeRequested");
                }
            }
        }

        #region Company Data

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
                    companyName = selectedCompany.companyName;
                    companyAddress1 = selectedCompany.companyAddress1;
                    companyAddress2 = selectedCompany.companyAddress2;
                    companyCity = selectedCompany.companyCity;
                    companyCounty = selectedCompany.companyCounty;
                    companyPostCode = selectedCompany.companyPostCode;
                    companyWebsite = selectedCompany.companyWebsite;
                    companyProspects = selectedCompany.companyProspects;
                    companyProduct = selectedCompany.companyProduct;
                    companyRemove = selectedCompany.companyRemove;
                    companyTelephone = selectedCompany.companyTelephone;
                    companyHistory = selectedCompany.companyHistory;
                    userID = selectedCompany.userID;
                }
            }
        }

        public int ID { get; private set; }

        private string _companyName;
        public string companyName
        {
            get { return selectedCompany.companyName; }
            set { if (_companyName != value)
                {
                    _companyName = value;
                    RaisePropertyChanged("companyName");
                }
            }
        }

        private string _companyAddress1;
        public string companyAddress1
        {
            get { return _companyAddress1; }
            set
            {
                if (_companyAddress1 != value)
                {
                    _companyAddress1 = value;
                    RaisePropertyChanged("companyAddress1");
                }
            }
        }
        private string _companyAddress2;
        public string companyAddress2
        {
            get { return _companyAddress2; }
            set
            {
                if (_companyAddress2 != value)
                {
                    _companyAddress2 = value;
                    RaisePropertyChanged("companyAddress2");
                }
            }
        }
        private string _companyCity;
        public string companyCity
        {
            get { return _companyCity; }
            set
            {
                if (_companyCity != value)
                {
                    _companyCity = value;
                    RaisePropertyChanged("companyCity");
                }
            }
        }
        private string _companyCounty;
        public string companyCounty
        {
            get { return _companyCounty; }
            set
            {
                if (_companyCounty != value)
                {
                    _companyCounty = value;
                    RaisePropertyChanged("companyCounty");
                }
            }
        }
        private string _companyPostCode;
        public string companyPostCode
        {
            get { return _companyPostCode; }
            set
            {
                if (_companyPostCode != value)
                {
                    _companyPostCode = value;
                    RaisePropertyChanged("companyPostCode");
                }
            }
        }

        private string _companyWebsite;
        public string companyWebsite
        {
            get { return _companyWebsite; }
            set
            {
                if (_companyWebsite != value)
                {
                    _companyWebsite = value;
                    RaisePropertyChanged("companyWebsite");
                }
            }
        }

        private string _companyProspects;
        public string companyProspects
        {
            get { return _companyProspects; }
            set
            {
                if (_companyProspects != value)
                {
                    _companyProspects = value;
                    RaisePropertyChanged("companyProspects");
                }
            }
        }

        private string _companyTelephone;
        public string companyTelephone
        {
            get { return _companyTelephone; }
            set
            {
                if (_companyTelephone != value)
                {
                    _companyTelephone = value;
                    RaisePropertyChanged("companyTelephone");
                }
            }
        }

        private string _companyProduct;
        public string companyProduct
        {
            get { return _companyProduct; }
            set
            {
                if (_companyProduct != value)
                {
                    _companyProduct = value;
                    RaisePropertyChanged("companyProduct");
                }
            }
        }

        private string _companyHistory;
        public string companyHistory
        {
            get { return _companyHistory; }
            set
            {
                if (_companyHistory != value)
                {
                    _companyHistory = value;
                    RaisePropertyChanged("companyHistory");
                }
            }
        }

        private bool _companyRemove;
        public bool companyRemove
        {
            get { return _companyRemove; }
            set
            {
                if (_companyRemove != value)
                {
                    _companyRemove = value;
                    RaisePropertyChanged("companyRemove");
                }
            }
        }

        private int _userID;
        public int userID
        {
            get { return _userID; }
            set
            {
                if (_userID != value)
                {
                    _userID = value;
                    RaisePropertyChanged("userID");
                }
            }
        }

        private DateTime _companyCreated;
        public DateTime companyCreated
        {
            get { return _companyCreated; }
            set
            {
                if (_companyCreated != value)
                {
                    _companyCreated = value;
                    RaisePropertyChanged("companyCreated");
                }
            }
        }

        private DateTime _companyModified;
        public DateTime companyModified
        {
            get { return _companyModified; }
            set
            {
                if (_companyModified != value)
                {
                    _companyModified = value;
                    RaisePropertyChanged("companyModified");
                }
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

        //public async Task getCompany()
        //{
        //    Get company from parameter


        //}

        private RelayCommand _updateCompany;
        public RelayCommand updateCompany
        {
            get
            {
                if (_updateCompany == null)
                {
                    _updateCompany = new RelayCommand(() =>
                    {
                        //DataAccessLayer.SaveCompany(selectedCompany);
                        postCodeRequested = !postCodeRequested;
                    });
                }

                return _updateCompany;

            }
        }

        //private RelayCommand _deleteCompany;
        //public RelayCommand deleteCompany
        //{
        //    get
        //    {
        //        if (_deleteCompany == null)
        //        {
        //            _deleteCompany = new RelayCommand(() =>
        //            {
        //                DataAccessLayer.DeletePerson(selectedCompany);
        //            });
        //        }

        //        return _deleteCompany;

        //    }
        //}
    }
}
