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
    public class NewContactViewModel : PageViewModel
    {
        public NewContactViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            //MessengerInstance.Register<NotificationMessage<string>>(this, setCompanyID);
            MessengerInstance.Register<NotificationMessage<Int64>>(this, newContactCompanyID);
            string test = this.ToString();
            Debug.Write(test);
        }

        public void newContactCompanyID(NotificationMessage<Int64> obj)
        {
            if(obj.Notification == "NewContactViewModel")
            {
                newContact.companyID = obj.Content;
            }
        }

        private Contact _newContact = new Contact();
        public Contact newContact
        {
            get { return _newContact; }
            set
            {
                if(_newContact != value)
                {
                    _newContact = value;
                    RaisePropertyChanged("newContact");
                }
            }
        }

        private RelayCommand _saveContact;
        public RelayCommand saveContact
        {
            get
            {
                if (_saveContact == null)
                {
                    _saveContact = new RelayCommand(() =>
                    {
                        DAL.DataAccessLayer.saveContact(newContact);
                        this.NavigationService.GoBack();
                    });
                }

                return _saveContact;

            }
        }

        private RelayCommand _cancelContact;
        public RelayCommand cancelContact
        {
            get
            {
                if (_cancelContact == null)
                {
                    _cancelContact = new RelayCommand(() =>
                    {
                        this.NavigationService.GoBack();
                    });
                }

                return _cancelContact;

            }
        }
    }
}
