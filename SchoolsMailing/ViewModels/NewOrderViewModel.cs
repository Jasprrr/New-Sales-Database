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
using SchoolsMailing.Views;

namespace SchoolsMailing.ViewModels
{
    public class NewOrderViewModel : PageViewModel
    {
        public NewOrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {

        }

        private RelayCommand _saveEmail;
        public RelayCommand saveEmail
        {
            get
            {
                if (_saveEmail == null)
                {
                    _saveEmail = new RelayCommand(() =>
                    {
                        Debug.Write(orderCode);
                    });
                }

                return _saveEmail;

            }
        }

        private string _someBinding;
        public string someBinding
        {
            get { return _someBinding; }
            set { if(_someBinding != value) { _someBinding = value; RaisePropertyChanged("someBinding"); } }
        }

        #region Navigate to new orders
        private RelayCommand _newEmail;
        public RelayCommand newEmail
        {
            get { if (_newEmail == null) { _newEmail = new RelayCommand(() => { NavigationService.Navigate(typeof(NewEmailView)); }); } return _newEmail; }
        }
        private RelayCommand _newData;
        public RelayCommand newData
        {
            get { if (_newData == null) { _newData = new RelayCommand(() => { NavigationService.Navigate(typeof(NewDataView)); }); } return _newData; }
        }
        private RelayCommand _newSchoolSend;
        public RelayCommand newSchoolSend
        {
            get { if (_newSchoolSend == null) { _newSchoolSend = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSchoolSendView)); }); } return _newSchoolSend; }
        }
        private RelayCommand _newDirectMailing;
        public RelayCommand newDirectMailing
        {
            get { if (_newDirectMailing == null) { _newDirectMailing = new RelayCommand(() => { NavigationService.Navigate(typeof(NewDirectMailingView)); }); } return _newDirectMailing; }
        }
        private RelayCommand _newSharedMailing;
        public RelayCommand newSharedMailing
        {
            get { if (_newSharedMailing == null) { _newSharedMailing = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSharedMailingView)); }); } return _newSharedMailing; }
        }
        private RelayCommand _newSurcharge;
        public RelayCommand newSurcharge
        {
            get { if (_newSurcharge == null) { _newSurcharge = new RelayCommand(() => { NavigationService.Navigate(typeof(NewSurchargeView)); }); } return _newSurcharge; }
        }
        private RelayCommand _newPrint;
        public RelayCommand newPrint
        {
            get { if (_newPrint == null) { _newPrint = new RelayCommand(() => { NavigationService.Navigate(typeof(NewPrintView)); }); } return _newPrint; }
        }
        #endregion

        private RelayCommand _cancelNew;
        public RelayCommand cancelNew
        {
            get { if (_cancelNew == null) { _cancelNew = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cancelNew; }
        }

        private string _orderCode;
        public string orderCode
        {
            get { return _orderCode; }
            set { if(_orderCode != value) { _orderCode = value; RaisePropertyChanged("orderCode"); } }
        }

        private string _emailSubject;
        public string emailSubject
        {
            get { return _emailSubject; }
            set { if(_emailSubject != value) { _emailSubject = value; RaisePropertyChanged("emailSubject"); } }
        }
    }
}
