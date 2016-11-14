using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using System.Windows.Input;
using SchoolsMailing.Views;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using System.Diagnostics;
using SchoolsMailing.Models;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;

namespace SchoolsMailing.ViewModels
{
    public class LoginViewModel : PageViewModel
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        public LoginViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {

        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
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
                }
            }
        }

        private RelayCommand _attemptLogin;
        public RelayCommand attemptLogin
        {
            get
            {
                if (_attemptLogin == null)
                {
                    _attemptLogin = new RelayCommand(() =>
                    {
                        if(username == "jasper@schoolsmailing.co.uk")
                        {
                            if(password == "envelope1")
                            {
                                Application.Current.MainWindow.Close();
                            }
                        }
                    });
                }

                return _attemptLogin;

            }
        }
    }
}
