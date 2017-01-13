using SchoolsMailing.DAL;
using SchoolsMailing.Models;
using SchoolsMailing.ViewModels;
using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchoolsMailing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            email = DataAccessLayer.GetAllEmail();
            schoolSend = DataAccessLayer.GetAllSchoolSend();
            directMailing = DataAccessLayer.GetAllDirectMailing();
            sharedMailing = DataAccessLayer.GetAllSharedMailing();
        }

        public List<OrdersEmail> emailsByDate;
        public List<OrdersSchoolSend> schoolSendByDate;
        public List<OrdersDirectMailing> directMailingByDate;
        public List<OrdersSharedMailing> sharedMailingByDate;

        public List<OrdersEmail> email;
        public List<OrdersDirectMailing> directMailing;
        public List<OrdersSharedMailing> sharedMailing;
        public List<OrdersSchoolSend> schoolSend;

        private void CalendarView_CalendarViewDayItemChanging(Windows.UI.Xaml.Controls.CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            }

            List<Color> densityColors = new List<Color>();

            foreach (OrdersSharedMailing _sharedMailing in sharedMailing)
            {
                if (_sharedMailing.sharedDate.Date == args.Item.Date.Date)
                {
                    densityColors.Add(Color.FromArgb(255, 52, 61, 146));
                    break;
                }
            }

            foreach (OrdersDirectMailing _directMailing in directMailing)
            {
                if (_directMailing.directDate.Date == args.Item.Date.Date)
                {
                    densityColors.Add(Color.FromArgb(255, 26, 154, 213));
                    break;
                }
            }

            foreach (OrdersSchoolSend _schoolSend in schoolSend)
            {
                if (_schoolSend.schoolsendEnd.Date == args.Item.Date.Date)
                {
                    densityColors.Add(Color.FromArgb(255, 39, 179, 75));
                    break;
                }
            }

            foreach (OrdersEmail _email in email)
            {
                if (_email.emailDate.Date == args.Item.Date.Date)
                {
                    densityColors.Add(Color.FromArgb(255, 242, 182, 17));
                    break;
                }
            }

            args.Item.SetDensityColors(densityColors);
        }

        private void CalendarView_SelectedDatesChanged(Windows.UI.Xaml.Controls.CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTimeOffset selectedDateTimeOffset = sender.SelectedDates.FirstOrDefault(); //Get selected DateTimeOffset
            DateTime selectedDateTime = selectedDateTimeOffset.UtcDateTime; //Convert to DateTime
            if(selectedDateTime != null)
            {
                
            }
        }
    }
}
