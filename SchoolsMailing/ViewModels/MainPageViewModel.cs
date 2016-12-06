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
        public MainPageViewModel(IMessenger messenger, NavigationService navigationService)
            : base(messenger, navigationService)
        {
            this.InitializeMenu();

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"Sales.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Company>();
            conn.CreateTable<Contact>();
            conn.CreateTable<CompanyHistory>();

            this.ItemInvokedCommand = new RelayCommand<ListViewItem>(this.ItemInvoked);

            MessengerInstance.Register<string>(this, backButtonVisible);
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
                    Label = "Orders",
                    Symbol = Symbol.Library,
                    AssociatedPage = typeof(OrdersView)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "Reports",
                    Symbol = Symbol.Folder,
                    AssociatedPage = typeof(NewOrderView)
                    
                },
                new SplitViewPaneMenuItem
                {
                    Label = "GridView",
                    Symbol = Symbol.Folder,
                    AssociatedPage = typeof(CompaniesView2)

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
                this.NavigationService.Navigate(menuItem.AssociatedPage, menuItem.Parameters);
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
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