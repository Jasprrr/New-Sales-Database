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
                                DoMove();
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
            //StorageFolder test = await StorageFolder.GetFolderFromPathAsync("");
            Debug.WriteLine(string.Format(Windows.Storage.ApplicationData.Current.LocalFolder.Path.ToString()));
            //string locPath = @"C:\Users\Jasper\AppData\Local\Packages\430d4efd-8648-4a35-8670-6dcecc88d151_7mzr475ysvhxg\LocalState\CreateDoc";
            //string desPath = @"C:\Users\Jasper\AppData\Local\Packages\430d4efd-8648-4a35-8670-6dcecc88d151_7mzr475ysvhxg\LocalState\FinishDoc\test.docx";
            //ZipFile.CreateFromDirectory(locPath, desPath);
            
            //XDocument doc = new XDocument(new XElement("test", new XAttribute("test_name", "test_Value"), new XElement("test_Child", "test_node")));
            //XmlWriter writer = XmlWriter.Create()
            //await doc.Save();

            StorageFolder fol = ApplicationData.Current.LocalFolder;
            StorageFile fil = await fol.CreateFileAsync("test.xml", CreationCollisionOption.ReplaceExisting);

            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            XNamespace cx = "http://schemas.microsoft.com/office/drawing/2014/chartex";
            XNamespace cx1 = "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex";
            XNamespace cx2 = "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex";
            XNamespace cx3 = "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex";
            XNamespace cx4 = "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex";
            XNamespace cx5 = "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex";
            XNamespace m = "http://schemas.openxmlformats.org/officeDocument/2006/math";
            XNamespace mc = "http://schemas.openxmlformats.org/markup-compatibility/2006";
            XNamespace o = "urn:schemas-microsoft-com:office:office";
            XNamespace r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
            XNamespace v = "urn:schemas-microsoft-com:vml";

            XDocument xdoc = new XDocument(new XElement(w + "document", new XAttribute(XNamespace.Xmlns+"w", w.NamespaceName)));

            try
            {
                using (var stream = await fil.OpenStreamForWriteAsync())
                {
                    xdoc.Save(stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
