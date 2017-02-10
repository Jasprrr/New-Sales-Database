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
            
            //Sample Data
            //SharedPack sp1 = new SharedPack { packCost = 475, packArtworkDate = Convert.ToDateTime("26/01/17"), packDate = Convert.ToDateTime("15/02/17"), packTo = "Secondary", packDeliveryDate = Convert.ToDateTime("06/02/17"), packMaxInserts = 7, packName = "[15/02/17] Secondary" };
            //SharedPack sp2 = new SharedPack { packCost = 1749, packArtworkDate = Convert.ToDateTime("26/01/17"), packDate = Convert.ToDateTime("15/02/17"), packTo = "Primary", packDeliveryDate = Convert.ToDateTime("06/02/17"), packMaxInserts = 12, packName = "[15/02/17] Primary" };
            //SharedPack sp3 = new SharedPack { packCost = 2119, packArtworkDate = Convert.ToDateTime("30/03/17"), packDate = Convert.ToDateTime("26/04/17"), packTo = "Primary & Secondary", packDeliveryDate = Convert.ToDateTime("10/04/16"), packMaxInserts = 6, packName = "[15/02/17] Primary & Secondary" };
            //DataAccessLayer.SaveSharedPack(sp1);
            //DataAccessLayer.SaveSharedPack(sp2);
            //DataAccessLayer.SaveSharedPack(sp3);

            //SchoolSendPack ssp1 = new SchoolSendPack { packCost = 50, packCredits = 10000, packName = "Ruby" };
            //SchoolSendPack ssp2 = new SchoolSendPack { packCost = 100, packCredits = 25000, packName = "Sapphire" };
            //SchoolSendPack ssp3 = new SchoolSendPack { packCost = 150, packCredits = 50000, packName = "Emerald" };
            //SchoolSendPack ssp4 = new SchoolSendPack { packCost = 250, packCredits = 100000, packName = "Silver" };
            //SchoolSendPack ssp5 = new SchoolSendPack { packCost = 350, packCredits = 150000, packName = "Gold" };
            //SchoolSendPack ssp6 = new SchoolSendPack { packCost = 450, packCredits = 200000, packName = "Platinum" };

            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp1);
            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp2);
            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp3);
            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp4);
            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp5);
            //DAL.DataAccessLayer.SaveSchoolSendPack(ssp6);

            this.ItemInvokedCommand = new RelayCommand<ListViewItem>(this.ItemInvoked);

            MessengerInstance.Register<NotificationMessage<User>>(this, RegisterMessages);
        }

        //Register logged in user
        public void RegisterMessages(NotificationMessage<User> obj)
        {
            if (obj.Notification == "userSignIn")
            {
                if(obj.Content != null)
                {
                    loggedInAs = obj.Content;
                }
            }
        }
        

        #region login details
        public static User loggedInAs;
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
                    Label = "Calendar",
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
                    Label = "Orders",
                    Symbol = Symbol.Shop,
                    AssociatedPage = typeof(OrdersView)
                }
                //new SplitViewPaneMenuItem
                //{
                //    Label = "Reports",
                //    Symbol = Symbol.Library
                    
                //}
                //new SplitViewPaneMenuItem
                //{
                //    Label = "Reports",
                //    Symbol = Symbol.Shop,
                //    AssociatedPage = typeof(OrderView)

                //}
            };

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
                if(menuItem.Label == "asd")
                {
                    this.NavigationService.Navigate(typeof(OrderView));
                    //Pass ID parameter
                    MessengerInstance.Send<NotificationMessage<int>>(new NotificationMessage<int>(0, "OrderViewModel"));
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