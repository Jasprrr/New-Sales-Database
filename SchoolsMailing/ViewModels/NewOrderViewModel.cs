using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;

namespace SchoolsMailing.ViewModels
{
    public class NewOrderViewModel : PageViewModel
    {
        public NewOrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {

        }



    }
}
