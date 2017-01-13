﻿using SchoolsMailing.ViewModels.Common;
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
                                DoMove();
                                //ViewModels.MoveFile.DoMove();
                                //var rootFrame = Window.Current.Content as Frame;

                                //rootFrame.Navigate(typeof(MainPage));
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
            //StorageApplicationPermissions.FutureAccessList.A
            StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            StorageFile file = await storageFolder.TryGetItemAsync("testzip.zip") as StorageFile;
            if(file != null)
            {
                await file.RenameAsync("testzip.docx");
            }
            //ZipFile.CreateFromDirectory()
        }
    }
}
