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
            createLogin();
            this.InitializeMenu();

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"Sales.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new
               SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<Company>();

            this.ItemInvokedCommand = new RelayCommand<ListViewItem>(this.ItemInvoked);
        }

        /// <summary>
        /// Gets the menu items for the app.
        /// </summary>
        /// 
        public async void createLogin()
        {
            var dial = new LoginDialog();

            var result = await dial.ShowAsync();
        }
        


        public ObservableCollection<SplitViewPaneMenuItem> MenuItems { get; private set; }

        private void InitializeMenu()
        {
            // Dummy Data
            this.MenuItems = new ObservableCollection<SplitViewPaneMenuItem>
            {
                new SplitViewPaneMenuItem
                {
                    Label = "Home",
                    Symbol = Symbol.Home,
                    AssociatedPage = typeof(HomePage)
                },
                new SplitViewPaneMenuItem
                {
                    Label = "Settings",
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
                }
            };

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
                        createLogin();
                    });
                }

                return _openSettings;

            }
        }
    }
}