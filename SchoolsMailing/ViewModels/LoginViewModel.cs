using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SchoolsMailing.Views;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;

namespace SchoolsMailing.ViewModels
{
    public class LoginViewModel : PageViewModel
    {
        public LoginViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
        }

        private string _username;
        public string username
        {
            get { return _username; }
            set
            {
                if(_username != value)
                {
                    _username = value;
                    RaisePropertyChanged("username");
                }
            }
        }
        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if(_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("password");
                }
            }
        }

        private RelayCommand _authenticateLogin;
        public RelayCommand authenticateLogin
        {
            get
            {
                if (_authenticateLogin == null)
                {
                    _authenticateLogin = new RelayCommand(() =>
                    {
                        if(username == "admin")
                        {
                            if(password == "@")
                            {
                                var rootFrame = Window.Current.Content as Frame;

                                rootFrame.Navigate(typeof(MainPage));
                            }
                        }
                        else
                        {
                            var rootFrame = Window.Current.Content as Frame;

                            rootFrame.Navigate(typeof(MainPage));
                        }
                    });
                }

                return _authenticateLogin;

            }
        }
    }
}
