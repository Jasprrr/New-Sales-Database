using SchoolsMailing.ViewModels.Common;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.IO;
using Windows.Storage;
using SchoolsMailing.Models;

namespace SchoolsMailing.ViewModels
{
    public class LoginViewModel : PageViewModel
    {
        string path;
        SQLite.Net.SQLiteConnection conn;

        public LoginViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //Creates local tables
            path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Sales.sqlite");
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
        }
        

        private string _userName;
        public string userName
        {
            get { return _userName; }
            set { if(_userName != value) { _userName = value; RaisePropertyChanged("userName"); } }
        }
        private string _userPassword;
        public string userPassword
        {
            get { return _userPassword; }
            set { if(_userPassword != value) { _userPassword = value; RaisePropertyChanged("userPassword"); } }
        }

        public static int invalidLoginAttempts = 0;

        private bool _isInvalidUserPassword = false;
        public bool isInvalidUserPassword
        {
            get { return _isInvalidUserPassword; }
            set { if(_isInvalidUserPassword != value) { _isInvalidUserPassword = !_isInvalidUserPassword; RaisePropertyChanged("isInvalidUserPassword"); } }
        }
        private bool _isInvalidUserName = false;
        public bool isInvalidUserName
        {
            get { return _isInvalidUserName; }
            set { if (_isInvalidUserName != value) { _isInvalidUserName = !_isInvalidUserName; RaisePropertyChanged("isInvalidUserName"); } }
        }

        private User loggedInAs = new User()
        {
            ID = 1,
            userName = "Jasper",
            userInitials = "J",
            userPassword = "envelope1",
            userCode = 1
        };

        //TODO: add enter event to login

        private RelayCommand _attemptLogin;
        public RelayCommand attemptLogin
        {
            get
            {
                if (_attemptLogin == null)
                {
                    _attemptLogin = new RelayCommand(() =>
                    {
                        if (isCorrectLogin())
                        {
                            var rootFrame = Window.Current.Content as Frame;
                            rootFrame.Navigate(typeof(MainPage));
                            MessengerInstance.Send<NotificationMessage<User>>(new NotificationMessage<User>(loggedInAs, "userSignIn"));
                        }      
                    });
                }
                return _attemptLogin;
            }
        }

        public bool isCorrectLogin()
        {            
            if (!DAL.DataAccessLayer.isValidPassword(userName, userPassword))
            {
                isInvalidUserPassword = true;
                invalidLoginAttempts++;
                return false;
            }
            else
            {
                isInvalidUserPassword = false;
            }
            return true;
        }
    }
}
