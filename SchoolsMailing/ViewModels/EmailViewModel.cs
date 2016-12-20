using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using SchoolsMailing.ViewModels.Common;
using SchoolsMailing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolsMailing.DAL;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using SchoolsMailing.Views;

namespace SchoolsMailing.ViewModels
{
    public class EmailViewModel : PageViewModel
    {
        public EmailViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            Messenger.Register<NotificationMessage<Email>>(this, GetEmail);
        }

        public void GetEmail(NotificationMessage<Email> email)
        {
            if (email.Notification == "Edit_Email")
            {
                if(email.Content != null)
                {
                    selectedEmailOrder = email.Content;
                }
                else
                {
                    Email selectedEmailOrder = new Email();
                }
            }
        }

        private Email _selectedEmailOrder;
        public Email selectedEmailOrder
        {
            get { return _selectedEmailOrder; }
            set { if(_selectedEmailOrder != value) { _selectedEmailOrder = value;  RaisePropertyChanged("selectedEmailOrder"); } }
        }

        private RelayCommand _cancelEmail;
        public RelayCommand cancelEmail
        {
            get { if (_cancelEmail == null) { _cancelEmail = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cancelEmail; }
        }

        private RelayCommand _saveEmail;
        public RelayCommand saveEmail
        {
            get { if (_saveEmail == null) { _saveEmail = new RelayCommand(() => {
                NavigationService.GoBack();
                MessengerInstance.Send<NotificationMessage<Email>>(new NotificationMessage<Email>(selectedEmailOrder, "Add_Email"));
            }); } return _saveEmail; }
        }

        private RelayCommand _deleteEmail;
        public RelayCommand deleteEmail
        {
            get
            {
                if (_deleteEmail == null)
                {
                    _deleteEmail = new RelayCommand(() => {
                        MessengerInstance.Send<NotificationMessage<Email>>(new NotificationMessage<Email>(selectedEmailOrder, "Delete_Email"));
                        NavigationService.GoBack();
                    });
                }
                return _deleteEmail;
            }
        }
    }
}