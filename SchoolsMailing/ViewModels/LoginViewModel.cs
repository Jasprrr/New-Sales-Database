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
using System.IO;
using Windows.Storage;
using System.IO.Compression;
using Windows.Storage.AccessCache;
using Windows.Data.Xml.Dom;
using System.Xml;
using System.Xml.Linq;

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

        private bool _invalidPassword = false;
        public bool invalidPassword
        {
            get { return _invalidPassword; }
            set { if(_invalidPassword != value) { _invalidPassword = !invalidPassword; RaisePropertyChanged("invalidPassword"); } }
        }

        private bool _invalidUsername = false;
        public bool invalidUsername
        {
            get { return _invalidUsername; }
            set { if (_invalidUsername != value) { _invalidUsername = !invalidUsername; RaisePropertyChanged("invalidUsername"); } }
        }

        //TODO: add enter event to login

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
                            invalidUsername = false;

                            if(password == "@")
                            {
                                Uri test = new Uri(@"ms-appx:///Assets/doc_CompanyTable.txt");
                                Debug.WriteLine(test.ToString());
                                //DAL.DocumentAccessLayer.CompanyTable();
                                //string test = "testing string {0}";
                                //string test2 = string.Format(test, "success!");
                                //Debug.WriteLine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path.ToString());
                                //DoMove();
                                //ViewModels.MoveFile.DoMove();
                                //var rootFrame = Window.Current.Content as Frame;

                                //rootFrame.Navigate(typeof(MainPage));
                                //ZipFile.
                            }
                            else
                            {
                                invalidPassword = true;
                            }
                        }
                        else
                        {
                            invalidUsername = true;
                        }
                    });
                }

                return _authenticateLogin;

            }
        }

        public async void DoMove()
        {
            //Debug.WriteLine(string.Format(Windows.Storage.ApplicationData.Current.LocalFolder.Path.ToString()));


            //try
            //{
            //    using (var stream = await fil.OpenStreamForWriteAsync())
            //    {
            //        xdoc.Save(stream);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }
    }
}
