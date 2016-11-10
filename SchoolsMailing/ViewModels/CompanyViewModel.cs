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
            
        }

        private ObservableCollection<DataOrder> _companyDataOrder;
        public ObservableCollection<DataOrder> companyDataOrder
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


        public async void addTestData()
        {
            ObservableCollection<DataOrder> c = new ObservableCollection<DataOrder>();
            DataOrder d1 = new DataOrder() { companyID = 1, dataCost = 12500.00, dataDetails = "a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 a lot of data 1 ", orderDate = Convert.ToDateTime("01/01/16"), ID = 1, orderCode = "L200"};
            DataOrder d2 = new DataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 2", orderDate = Convert.ToDateTime("02/01/16"), ID = 1, orderCode = "L201"};
            DataOrder d3 = new DataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 3", orderDate = Convert.ToDateTime("03/01/16"), ID = 1, orderCode = "L202"};
            DataOrder d4 = new DataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 4", orderDate = Convert.ToDateTime("04/01/16"), ID = 1, orderCode = "L204"};
            DataOrder d5 = new DataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 5", orderDate = Convert.ToDateTime("05/01/16"), ID = 1, orderCode = "L205"};
            DataOrder d6 = new DataOrder() { companyID = 1, dataCost = 125.00, dataDetails = "a lot of data 6", orderDate = Convert.ToDateTime("06/01/16"), ID = 1, orderCode = "L206"};
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
            var txtBox = new TextBox { Width=200, MinHeight=32 };
            conDlg.Content = txtBox;
            var conDlg2 = await conDlg.ShowAsync();
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

        public ContentDialog conDlg = new ContentDialog
        {
            Title = "Title!",
            
            PrimaryButtonText = "Yes!"
        };

        //private RelayCommand _updateCompany;
        //public RelayCommand updateCompany
        //{
        //    get
        //    {
        //        if (_updateCompany == null)
        //        {
        //            _updateCompany = new RelayCommand(async () =>
        //            {
                        
        //            });
        //        }
        //        return updateCompany;
        //    }
        //}

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
