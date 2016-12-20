namespace SchoolsMailing.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Windows.UI.Xaml.Controls;
    using System;
    using SchoolsMailing.Common;
    using SchoolsMailing.Controls.Models;
    using SchoolsMailing.ViewModels.Common;
    using SchoolsMailing.Views;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using DAL;
    using Models;
    using System.IO;
    using Controls;
    using System.Diagnostics;
    using Windows.UI.Core;
    public class MainPageViewModel : PageViewModel
    {
        string path;
        SQLite.Net.SQLiteConnection conn;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The messenger.
        /// </param>
        /// <param name="navigationService">
        /// The navigation service.
        /// </param>
        public MainPageViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            this.InitializeMenu();

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"Sales.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Company>();
            conn.CreateTable<Contact>();
            conn.CreateTable<CompanyHistory>();
            conn.CreateTable<Contact>();
            conn.CreateTable<SharedPack>();
            conn.CreateTable<Email>();
            conn.CreateTable<Data>();
            conn.CreateTable<SchoolSend>();
            conn.CreateTable<SharedMailing>();
            conn.CreateTable<DirectMailing>();
            conn.CreateTable<Print>();
            conn.CreateTable<Surcharge>();
            conn.CreateTable<Orders>();
            conn.CreateTable<User>();
            conn.CreateTable<SchoolSendPack>();

            //Sample Data
            SharedPack sp1 = new SharedPack { packCost = 475, packArtworkDate = Convert.ToDateTime("26/01/17"), packDate = Convert.ToDateTime("15/02/17"), packTo = "Secondary", packDeliveryDate = Convert.ToDateTime("06/02/17"), packMaxInserts = 7, packName = "[15/02/17] Secondary" };
            SharedPack sp2 = new SharedPack { packCost = 1749, packArtworkDate = Convert.ToDateTime("26/01/17"), packDate = Convert.ToDateTime("15/02/17"), packTo = "Primary", packDeliveryDate = Convert.ToDateTime("06/02/17"), packMaxInserts = 12, packName = "[15/02/17] Primary" };
            SharedPack sp3 = new SharedPack { packCost = 2119, packArtworkDate = Convert.ToDateTime("30/03/17"), packDate = Convert.ToDateTime("26/04/17"), packTo = "Primary & Secondary", packDeliveryDate = Convert.ToDateTime("10/04/16"), packMaxInserts = 6, packName = "[15/02/17] Primary & Secondary" };
            DataAccessLayer.SaveSharedPack(sp1);
            DataAccessLayer.SaveSharedPack(sp2);
            DataAccessLayer.SaveSharedPack(sp3);

            SchoolSendPack ssp1 = new SchoolSendPack { packCost = 50, packCredits = 10000, packName = "Ruby" };
            SchoolSendPack ssp2 = new SchoolSendPack { packCost = 100, packCredits = 25000, packName = "Sapphire" };
            SchoolSendPack ssp3 = new SchoolSendPack { packCost = 150, packCredits = 50000, packName = "Emerald" };
            SchoolSendPack ssp4 = new SchoolSendPack { packCost = 250, packCredits = 100000, packName = "Silver" };
            SchoolSendPack ssp5 = new SchoolSendPack { packCost = 350, packCredits = 150000, packName = "Gold" };
            SchoolSendPack ssp6 = new SchoolSendPack { packCost = 450, packCredits = 200000, packName = "Platinum" };

            DAL.DataAccessLayer.SaveSchoolSendPack(ssp1);
            DAL.DataAccessLayer.SaveSchoolSendPack(ssp2);
            DAL.DataAccessLayer.SaveSchoolSendPack(ssp3);
            DAL.DataAccessLayer.SaveSchoolSendPack(ssp4);
            DAL.DataAccessLayer.SaveSchoolSendPack(ssp5);
            DAL.DataAccessLayer.SaveSchoolSendPack(ssp6);

            this.ItemInvokedCommand = new RelayCommand<ListViewItem>(this.ItemInvoked);

            MessengerInstance.Register<NotificationMessage<string>>(this, RegisterMessages);
        }

        public void RegisterMessages(NotificationMessage<string> obj)
        {
            if(obj.Notification == "frameTitle")
            {
                string newTitle = obj.Content.ToString();
                frameTitle = newTitle;
            }
            else if(obj.Notification == "backButtonVisible")
            {

            }
        }


        public bool backVisisble;
        public void backButtonVisible(string obj)
        {
            backVisisble = Convert.ToBoolean(obj);
            Debug.Write("Msg Recieved!");
        }

        #region login details

        private User _loggedInAs = new User()
        {
            userInitials="J", ID=1, userName="Jasper", userPassword="@"
        };
        public User loggedInAs
        {
            get { return _loggedInAs; }
            set
            {
                if (loggedInAs != value)
                {
                    _loggedInAs = value;
                    RaisePropertyChanged("loggedInAs");
                }
            }
        }

        #endregion
        
        /// <summary>
        /// Gets the menu items for the app.
        /// </summary>
        /// 
        public ObservableCollection<SplitViewPaneMenuItem> MenuItems { get; private set; }

        private void InitializeMenu()
        {
            // Dummy Data
            this.MenuItems = new ObservableCollection<SplitViewPaneMenuItem>
            {
                new SplitViewPaneMenuItem
                {
                    Label = "Home",
                    Symbol = Symbol.Calendar,
                    AssociatedPage = typeof(HomePage)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "Companies",
                    Symbol = Symbol.Contact,
                    AssociatedPage = typeof(CompaniesView)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "Contacts",
                    Symbol = Symbol.People,
                    AssociatedPage = typeof(CompaniesView)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "New Company",
                    Symbol = Symbol.Add,
                    AssociatedPage = typeof(NewCompanyView)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "Reports",
                    Symbol = Symbol.Folder,
                    AssociatedPage = typeof(OrderView)
                    
                }
            };

        }

        private int _selectedItem = 0;
        public int selectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged("selectedItem"); }
        }

        private string _frameTitle;
        public string frameTitle
        {
            get { return _frameTitle; }
            set
            {
                if(_frameTitle != value)
                {
                    _frameTitle = value;
                }
                RaisePropertyChanged("frameTitle");
            }
        }

        /// <summary>
        /// Gets the item invoked command.
        /// </summary>
        public ICommand ItemInvokedCommand { get; private set; }
        private void ItemInvoked(ListViewItem obj)
        {
            var menuItem = obj?.Content as SplitViewPaneMenuItem;
            if (menuItem != null)
            {
                if(menuItem.Label == "Reports")
                {
                    this.NavigationService.Navigate(typeof(OrderView));
                    //Pass ID parameter
                    MessengerInstance.Send<NotificationMessage<Int64>>(new NotificationMessage<Int64>(0, "OrderViewModel"));
                }
                else
                {
                    this.NavigationService.Navigate(menuItem.AssociatedPage, menuItem.Parameters);
                    //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }

            }
        }

        private RelayCommand _openSettings;
        public RelayCommand openSettings
        {
            get
            {
                if (_openSettings == null)
                {
                    _openSettings = new RelayCommand(() =>
                    {
                        //selectedItem = 3;
                        Debug.Write("Settings invoked");
                    });
                }

                return _openSettings;

            }
        }
    }
}