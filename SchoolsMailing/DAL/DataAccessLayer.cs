using SchoolsMailing.Models;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SchoolsMailing.DAL
{
    internal static class DataAccessLayer
    {
        private static string dbPath = string.Empty;

        private static string DbPath
        {

            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Sales.sqlite");
                }

                return dbPath;
            }
        }

        private static SQLiteConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            }
        }



        //public static async Task CreateDatabase()
        //{
        //    Create a new connection
        //    using (var db = DbConnection)
        //    {
        //        // Create the table if it does not exist
        //        var c = db.CreateTable<Company>();
        //        var info = db.GetMapping(typeof(Company));
        //    }
        //}

        public static List<Email> GetEmailByDate(DateTime date)
        {
            List<Email> email;

            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                email = (from p in db.Table<Email>()
                         select p).ToList();
            }
            return email.FindAll(p => p.emailDate.Date == date.Date);
        }

        #region Get data list
        public static List<Company> GetAllCompanies()
        {
            List<Company> c;

            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                c = (from p in db.Table<Company>()
                          select p).ToList();
            }
            List<Company> model = new List<Company>(c);
            return model;
        }

        public static ObservableCollection<Company> GetAllCompanies2()
        {
            List<Company> c;
            ObservableCollection<Company> c2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                c = (from p in db.Table<Company>()
                     orderby p.companyName descending
                     select p).ToList();
            }
            c2 = new ObservableCollection<Company>(c);
            return c2;
        }

        public static ObservableCollection<CompanyHistory> GetAllHistoryByCompany(long companyID)
        {
            List<CompanyHistory> h;
            ObservableCollection<CompanyHistory> h2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                h = (from p in db.Table<CompanyHistory>()
                     where p.companyID == companyID
                     orderby p.companyHistoryDate descending
                     select p).ToList();
            }
            h2 = new ObservableCollection<CompanyHistory>(h);
            return h2;
        }

        public static ObservableCollection<Contact> GetAllContactsByCompanyID(long companyID)
        {
            List<Contact> c; //Create list
            ObservableCollection<Contact> c2; //Create ObservableCollection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath)) //Connect to SQLite table
            {
                c = (from p in db.Table<Contact>() //Query
                     where p.companyID == companyID
                     orderby p.contactForename ascending
                     select p).ToList(); 
            }
            c2 = new ObservableCollection<Contact>(c); //Convert list to ObservableCollection
            return c2;
        }

        public static ObservableCollection<Contact> GetAllContacts()
        {
            List<Contact> c; //Create list
            ObservableCollection<Contact> c2; //Create ObservableCollection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath)) //Connect to SQLite table
            {
                c = (from p in db.Table<Contact>() //Query
                     orderby p.contactForename ascending
                     select p).ToList();
            }
            c2 = new ObservableCollection<Contact>(c); //Convert list to ObservableCollection
            return c2;
        }

        public static List<Orders> GetAllOrders()
        {
            List<Orders> o;
            using(var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<Orders>()
                     orderby p.orderDate ascending
                     select p).ToList();
            }
            return o;
        }
        public static List<OrdersData> GetAllData()
        {
            using(var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersData> od;
                od = db.Query<OrdersData>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Data.dataDetails, Data.dataCost, Data.dataStart, 
                                                   Data.dataEnd, Data.dataCreated, Data.dataModified 
                                            FROM Data, Orders 
                                            WHERE Data.orderID = Orders.ID 
                                            ORDER BY Data.dataEnd DESC;").ToList();
                return od;
            }
        }
        public static List<OrdersEmail> GetAllEmail()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersEmail> oe;
                oe = db.Query<OrdersEmail>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Email.emailDate, Email.emailDetails, Email.emailAdminCost,
                                                   Email.emailDirectCost, Email.emailCost, Email.emailSubject, Email.emailSetUp, Email.emailCreated, Email.emailModified
                                             FROM Email, Orders 
                                             WHERE Email.orderID = Orders.ID 
                                             ORDER BY Email.emailDate DESC;").ToList();
                return oe;
            }
        }
        public static List<OrdersSchoolSend> GetAllSchoolSend()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSchoolSend> os;
                os = db.Query<OrdersSchoolSend>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SchoolSend.schoolsendPackage, SchoolSend.schoolsendStart, 
                                                   SchoolSend.schoolsendEnd, SchoolSend.schoolsendCost, SchoolSend.schoolsendCredits, SchoolSend.schoolsendCreated, SchoolSend.schoolsendModified
                                            FROM SchoolSend, Orders 
                                            WHERE SchoolSend.orderID = Orders.ID 
                                            ORDER BY SchoolSend.schoolsendEnd DESC;").ToList();
                return os;
            }
        }
        public static List<OrdersDirectMailing> GetAllDirectMailing()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersDirectMailing> odm;
                odm = db.Query<OrdersDirectMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                             Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                             Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                             Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, DirectMailing.directDeliveryCode,
                                                             DirectMailing.directDataDate, DirectMailing.directInsertDate, DirectMailing.directArtworkDate, DirectMailing.directDate, 
                                                             DirectMailing.directMailingTo, DirectMailing.directLeafletCode, DirectMailing.directDetails, DirectMailing.directFulfilmentCost, 
                                                             DirectMailing.directPrintCost, DirectMailing.directPostageCost, DirectMailing.directCreated, DirectMailing.directModified, DirectMailing.directCost
                                                     FROM DirectMailing, Orders 
                                                     WHERE DirectMailing.orderID = Orders.ID 
                                                     ORDER BY DirectMailing.directDate DESC;").ToList();
                return odm;
            }
        }
        public static List<OrdersSharedMailing> GetAllSharedMailing()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSharedMailing> osm;
                osm = db.Query<OrdersSharedMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SharedMailing.sharedDeliveryCode, SharedMailing.sharedDate,
                                                   SharedMailing.sharedMailingTo, SharedMailing.sharedArtworkDate, SharedMailing.sharedLeafletName, SharedMailing.sharedNumberOfLeaflets, SharedMailing.sharedFAO, 
                                                   SharedMailing.sharedLeafletSize, SharedMailing.sharedLeafletWeight, SharedMailing.sharedDeliveryDate, SharedMailing.sharedPackage, SharedMailing.sharedCost,
                                                   SharedMailing.sharedCreated, SharedMailing.sharedModified
                                            FROM SharedMailing, Orders 
                                            WHERE SharedMailing.orderID = Orders.ID 
                                            ORDER BY SharedMailing.sharedDate DESC;").ToList();
                return osm;
            }
        }
        public static List<OrdersPrint> GetAllPrint()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersPrint> op;
                op = db.Query<OrdersPrint>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Print.printPrinter, Print.printDetails,
                                                   Print.printCharge, Print.printCost, Print.printDate, Print.printCreated, Print.printModified
                                            FROM Print, Orders 
                                            WHERE Print.orderID = Orders.ID 
                                            ORDER BY Print.printDate DESC;").ToList();
                return op;
            }
        }
        public static List<OrdersSurcharge> GetAllSurcharge()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSurcharge> os;
                os = db.Query<OrdersSurcharge>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Surcharge.surchargeDate, Surcharge.surchargeDetails, 
                                                   Surcharge.surchargeCost, Surcharge.surchargeCreated, Surcharge.surchargeModified
                                            FROM Surcharge, Orders 
                                            WHERE Surcharge.orderID = Orders.ID 
                                            ORDER BY Surcharge.surchargeDate DESC;").ToList();
                return os;
            }
        }
        #endregion

        #region Order part by date
        //public static List<OrdersData> GetDataByOrderDate(DateTime date)
        //{
        //    // Create a new connection
        //    using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
        //    {
        //        List<OrdersData> od;
        //        od = db.Query<OrdersData>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
        //                                           Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
        //                                           Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
        //                                           Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Data.dataDetails, Data.dataCost, Data.dataStart, 
        //                                           Data.dataEnd, Data.dataCreated, Data.dataModified 
        //                                    FROM Data, Orders 
        //                                    WHERE Data.orderID = Orders.ID AND Data.dataEnd = ? 
        //                                    ORDER BY Data.dataEnd DESC;", date).ToList();
        //        return od;
        //    }
        //}
        public static List<OrdersEmail> GetOrdersEmailByDate(DateTime date)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersEmail> o;
                o = db.Query<OrdersEmail>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Email.emailDate, Email.emailDetails, Email.emailAdminCost,
                                                   Email.emailDirectCost, Email.emailCost, Email.emailSubject, Email.emailSetUp, Email.emailCreated, Email.emailModified
                                             FROM Email, Orders 
                                             WHERE Email.orderID = Orders.ID
                                             ORDER BY Email.emailDate DESC;").ToList();
                return o.FindAll(p => p.emailDate.Date == date.Date);
            }
        }
        public static List<OrdersSchoolSend> GetOrdersSchoolSendByDate(DateTime date)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSchoolSend> o;
                o = db.Query<OrdersSchoolSend>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                       Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                       Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                       Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SchoolSend.schoolsendPackage, SchoolSend.schoolsendStart, 
                                                       SchoolSend.schoolsendEnd, SchoolSend.schoolsendCost, SchoolSend.schoolsendCredits, SchoolSend.schoolsendCreated, SchoolSend.schoolsendModified
                                                 FROM SchoolSend, Orders 
                                                 WHERE SchoolSend.orderID = Orders.ID
                                                 ORDER BY SchoolSend.schoolsendEnd DESC;").ToList();
                return o.FindAll(p => p.schoolsendEnd.Date == date.Date);
            }
        }
        public static List<OrdersDirectMailing> GetOrdersDirectMailingByDate(DateTime date)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersDirectMailing> o;
                o = db.Query<OrdersDirectMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                             Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                             Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                             Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, DirectMailing.directDeliveryCode,
                                                             DirectMailing.directDataDate, DirectMailing.directInsertDate, DirectMailing.directArtworkDate, DirectMailing.directDate, 
                                                             DirectMailing.directMailingTo, DirectMailing.directLeafletCode, DirectMailing.directDetails, DirectMailing.directFulfilmentCost, 
                                                             DirectMailing.directPrintCost, DirectMailing.directPostageCost, DirectMailing.directCreated, DirectMailing.directModified, DirectMailing.directCost
                                                     FROM DirectMailing, Orders 
                                                     WHERE DirectMailing.orderID = Orders.ID
                                                     ORDER BY DirectMailing.directDate DESC;").ToList();
                return o.FindAll(p => p.directDate.Date == date.Date);
            }
        }
        public static List<OrdersSharedMailing> GetOrdersSharedMailingByDate(DateTime date)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSharedMailing> o;
                o = db.Query<OrdersSharedMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                           Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                           Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                           Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SharedMailing.sharedDeliveryCode, SharedMailing.sharedDate,
                                                           SharedMailing.sharedMailingTo, SharedMailing.sharedArtworkDate, SharedMailing.sharedLeafletName, SharedMailing.sharedNumberOfLeaflets, SharedMailing.sharedFAO, 
                                                           SharedMailing.sharedLeafletSize, SharedMailing.sharedLeafletWeight, SharedMailing.sharedDeliveryDate, SharedMailing.sharedPackage, SharedMailing.sharedCost,
                                                           SharedMailing.sharedCreated, SharedMailing.sharedModified
                                                     FROM SharedMailing, Orders 
                                                     WHERE SharedMailing.orderID = Orders.ID
                                                     ORDER BY SharedMailing.sharedDate DESC;").ToList();
                return o.FindAll(p => p.sharedDate.Date == date.Date);
            }
        }
        #endregion

        #region Order part by ID
        public static List<OrdersData> GetDataByOrderID(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersData> od;
                od = db.Query<OrdersData>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Data.dataDetails, Data.dataCost, Data.dataStart, 
                                                   Data.dataEnd, Data.dataCreated, Data.dataModified 
                                            FROM Data, Orders 
                                            WHERE Data.orderID = Orders.ID AND Orders.ID = ? 
                                            ORDER BY Data.dataEnd DESC;", orderID).ToList();
                return od;
            }
        }
        public static List<OrdersEmail> GetEmailByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersEmail> oe;
                oe = db.Query<OrdersEmail>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Email.emailDate, Email.emailDetails, Email.emailAdminCost,
                                                   Email.emailDirectCost, Email.emailCost, Email.emailSubject, Email.emailSetUp, Email.emailCreated, Email.emailModified
                                             FROM Email, Orders 
                                             WHERE Email.orderID = Orders.ID AND Orders.ID = ?
                                             ORDER BY Email.emailDate DESC;", orderID).ToList();
                return oe;
            }
        }
        public static List<OrdersSchoolSend> GetSchoolSendByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSchoolSend> os;
                os = db.Query<OrdersSchoolSend>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SchoolSend.schoolsendPackage, SchoolSend.schoolsendStart, 
                                                   SchoolSend.schoolsendEnd, SchoolSend.schoolsendCost, SchoolSend.schoolsendCredits, SchoolSend.schoolsendCreated, SchoolSend.schoolsendModified
                                            FROM SchoolSend, Orders 
                                            WHERE SchoolSend.orderID = Orders.ID AND Orders.ID = ? 
                                            ORDER BY SchoolSend.schoolsendEnd DESC;", orderID).ToList();
                return os;
            }
        }
        public static List<OrdersDirectMailing> GetDirectMailingByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersDirectMailing> odm;
                odm = db.Query<OrdersDirectMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                             Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                             Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                             Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, DirectMailing.directDeliveryCode,
                                                             DirectMailing.directDataDate, DirectMailing.directInsertDate, DirectMailing.directArtworkDate, DirectMailing.directDate, 
                                                             DirectMailing.directMailingTo, DirectMailing.directLeafletCode, DirectMailing.directDetails, DirectMailing.directFulfilmentCost, 
                                                             DirectMailing.directPrintCost, DirectMailing.directPostageCost, DirectMailing.directCreated, DirectMailing.directModified, DirectMailing.directCost
                                                     FROM DirectMailing, Orders 
                                                     WHERE DirectMailing.orderID = Orders.ID AND Orders.ID = ?
                                                     ORDER BY DirectMailing.directDate DESC;", orderID).ToList();
                return odm;
            }
        }
        public static List<OrdersSharedMailing> GetSharedMailingByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSharedMailing> osm;
                osm = db.Query<OrdersSharedMailing>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, SharedMailing.sharedDeliveryCode, SharedMailing.sharedDate,
                                                   SharedMailing.sharedMailingTo, SharedMailing.sharedArtworkDate, SharedMailing.sharedLeafletName, SharedMailing.sharedNumberOfLeaflets, SharedMailing.sharedFAO, 
                                                   SharedMailing.sharedLeafletSize, SharedMailing.sharedLeafletWeight, SharedMailing.sharedDeliveryDate, SharedMailing.sharedPackage, SharedMailing.sharedCost,
                                                   SharedMailing.sharedCreated, SharedMailing.sharedModified
                                            FROM SharedMailing, Orders 
                                            WHERE SharedMailing.orderID = Orders.ID AND Orders.ID = ?
                                            ORDER BY SharedMailing.sharedDate DESC;", orderID).ToList();
                return osm;
            }
        }
        public static List<OrdersPrint> GetPrintByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersPrint> op;
                op = db.Query<OrdersPrint>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Print.printPrinter, Print.printDetails,
                                                   Print.printCharge, Print.printCost, Print.printDate, Print.printCreated, Print.printModified
                                            FROM Print, Orders 
                                            WHERE Print.orderID = Orders.ID AND Orders.ID = ?
                                            ORDER BY Print.printDate DESC;", orderID).ToList();
                return op;
            }
        }
        public static List<OrdersSurcharge> GetSurchargeByOrderID(long orderID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersSurcharge> os;
                os = db.Query<OrdersSurcharge>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Surcharge.surchargeDate, Surcharge.surchargeDetails, 
                                                   Surcharge.surchargeCost, Surcharge.surchargeCreated, Surcharge.surchargeModified
                                            FROM Surcharge, Orders 
                                            WHERE Surcharge.orderID = Orders.ID AND Orders.ID = ?
                                            ORDER BY Surcharge.surchargeDate DESC;", orderID).ToList();
                return os;
            }
        }
        #endregion

        #region Single Get - By ID
        public static Orders GetOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Orders o = (from p in db.Table<Orders>()
                            where p.ID == orderID
                            select p).FirstOrDefault();
                return o;
            }
        }
        public static Company GetCompanyById(long companyID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Company m = (from p in db.Table<Company>()
                             where p.ID == companyID
                             select p).FirstOrDefault();
                return m;
            }
        }
        public static Contact GetContactById(long ContactID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Contact m = (from p in db.Table<Contact>()
                             where p.ID == ContactID
                             select p).FirstOrDefault();
                return m;
            }
        }
        public static CompanyHistory GetHistoryByID(long historyID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                CompanyHistory m = (from p in db.Table<CompanyHistory>()
                                    where p.ID == historyID
                                    select p).FirstOrDefault();
                return m;
            }
        }
        public static Data GetDataOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Data m = (from p in db.Table<Data>()
                          where p.orderID == orderID
                          select p).FirstOrDefault();
                return m;
            }
        }
        public static Email GetEmailOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Email m = (from p in db.Table<Email>()
                           where p.ID == orderID
                           select p).FirstOrDefault();
                return m;
            }
        }
        public static SchoolSend GetSchoolSendOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                SchoolSend m = (from p in db.Table<SchoolSend>()
                                where p.ID == orderID
                                select p).FirstOrDefault();
                return m;
            }
        }
        public static DirectMailing GetDirectMailingOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                DirectMailing m = (from p in db.Table<DirectMailing>()
                                   where p.ID == orderID
                                   select p).FirstOrDefault();
                return m;
            }
        }
        public static SharedMailing GetSharedMailingOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                SharedMailing m = (from p in db.Table<SharedMailing>()
                                   where p.ID == orderID
                                   select p).FirstOrDefault();
                return m;
            }
        }
        public static Print GetPrintOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Print m = (from p in db.Table<Print>()
                           where p.ID == orderID
                           select p).FirstOrDefault();
                return m;
            }
        }
        public static Surcharge GetSurchargeOrder(long orderID)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                Surcharge m = (from p in db.Table<Surcharge>()
                               where p.ID == orderID
                               select p).FirstOrDefault();
                return m;
            }
        }
        #endregion

        public static List<Data> ListDataAll()
        {
            List<Data> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Data>()
                     orderby p.dataEnd descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Email> ListEmailAll()
        {
            List<Email> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Email>()
                     orderby p.emailDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<SchoolSend> ListSchoolSendAll()
        {
            List<SchoolSend> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<SchoolSend>()
                     orderby p.schoolsendEnd descending
                     select p).ToList();
            }
            return l;
        }
        public static List<DirectMailing> ListDirectMailingAll()
        {
            List<DirectMailing> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<DirectMailing>()
                     orderby p.directDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<SharedMailing> ListSharedMailingAll()
        {
            List<SharedMailing> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<SharedMailing>()
                     orderby p.sharedDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Print> ListPrintAll()
        {
            List<Print> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Print>()
                     orderby p.printDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Surcharge> ListSurchargeAll()
        {
            List<Surcharge> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Surcharge>()
                     orderby p.surchargeDate descending
                     select p).ToList();
            }
            return l;
        }

        #region List Get
        public static List<Data> ListDataByOrderID(long orderID)
        {
            List<Data> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Data>()
                     where p.orderID == orderID
                     orderby p.dataEnd descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Email> ListEmailByOrderID(long orderID)
        {
            List<Email> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Email>()
                     where p.orderID == orderID
                     orderby p.emailDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<SchoolSend> ListSchoolSendByOrderID(long orderID)
        {
            List<SchoolSend> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<SchoolSend>()
                     where p.orderID == orderID
                     orderby p.schoolsendEnd descending
                     select p).ToList();
            }
            return l;
        }
        public static List<DirectMailing> ListDirectMailingByOrderID(long orderID)
        {
            List<DirectMailing> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<DirectMailing>()
                     where p.orderID == orderID
                     orderby p.directDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<SharedMailing> ListSharedMailingByOrderID(long orderID)
        {
            List<SharedMailing> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<SharedMailing>()
                     where p.orderID == orderID
                     orderby p.sharedDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Print> ListPrintByOrderID(long orderID)
        {
            List<Print> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Print>()
                     where p.orderID == orderID
                     orderby p.printDate descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Surcharge> ListSurchargeByOrderID(long orderID)
        {
            List<Surcharge> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Surcharge>()
                     where p.orderID == orderID
                     orderby p.surchargeDate descending
                     select p).ToList();
            }
            return l;
        }
        #endregion

        #region ObservableCollection Get
        public static ObservableCollection<Data> GetAllDataByOrderID(long orderID)
        {
            List<Data> o;
            ObservableCollection<Data> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<Data>()
                     where p.orderID == orderID
                     orderby p.dataEnd descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<Data>(o);
            return o2;
        }
        public static ObservableCollection<Email> GetAllEmailsByOrderID(long orderID)
        {
            List<Email> o;
            ObservableCollection<Email> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<Email>()
                     where p.orderID == orderID
                     orderby p.emailDate descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<Email>(o);
            return o2;
        }
        public static ObservableCollection<SchoolSend> GetAllSchoolSendsByOrderID(long orderID)
        {
            List<SchoolSend> o;
            ObservableCollection<SchoolSend> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<SchoolSend>()
                     where p.orderID == orderID
                     orderby p.schoolsendEnd descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<SchoolSend>(o);
            return o2;
        }
        public static ObservableCollection<DirectMailing> GetAllDirectMailingsByOrderID(long orderID)
        {
            List<DirectMailing> o;
            ObservableCollection<DirectMailing> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<DirectMailing>()
                     where p.orderID == orderID
                     orderby p.directDate descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<DirectMailing>(o);
            return o2;
        }
        public static ObservableCollection<SharedMailing> GetAllSharedMailingsByOrderID(long orderID)
        {
            List<SharedMailing> o;
            ObservableCollection<SharedMailing> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<SharedMailing>()
                     where p.orderID == orderID
                     orderby p.sharedDate descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<SharedMailing>(o);
            return o2;
        }
        public static ObservableCollection<Print> GetAllPrintByOrderID(long orderID)
        {
            List<Print> o;
            ObservableCollection<Print> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<Print>()
                     where p.orderID == orderID
                     orderby p.printDate descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<Print>(o);
            return o2;
        }
        public static ObservableCollection<Surcharge> GetAllSurchargesByOrderID(long orderID)
        {
            List<Surcharge> o;
            ObservableCollection<Surcharge> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<Surcharge>()
                     where p.orderID == orderID
                     orderby p.surchargeDate descending
                     select p).ToList();
            }
            o2 = new ObservableCollection<Surcharge>(o);
            return o2;
        }
        public static ObservableCollection<SchoolSendPack> GetAllSchoolSendPacks()
        {
            List<SchoolSendPack> o;
            ObservableCollection<SchoolSendPack> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<SchoolSendPack>()
                     select p).ToList();
            }
            o2 = new ObservableCollection<SchoolSendPack>(o);
            return o2;
        }
        public static ObservableCollection<SharedPack> GetAllSharedPacks()
        {
            List<SharedPack> o;
            ObservableCollection<SharedPack> o2;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                o = (from p in db.Table<SharedPack>()
                     select p).ToList();
            }
            o2 = new ObservableCollection<SharedPack>(o);
            return o2;
        }
        #endregion

        #region Saving
        public static void SaveCompany(Company company)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (company.ID == 0)
                {
                    // New
                    company.companyCreated = DateTime.Now;
                    company.companyModified = DateTime.Now;
                    db.Insert(company);
                }
                else
                {
                    // Update
                    company.companyModified = DateTime.Now;
                    db.Update(company);
                }
            }
        }
        public static void SaveHistory(CompanyHistory history)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (history.ID == 0)
                {
                    // New
                    db.Insert(history);
                    Debug.Write("History inserted");
                }
                else
                {
                    // Update
                    db.Update(history);
                    Debug.Write("History updated");
                }
            }
        }
        public static void SaveContact(Contact contact)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (contact.ID == 0)
                {
                    // New
                    contact.contactCreated = DateTime.Now;
                    contact.contactModified = DateTime.Now;
                    db.Insert(contact);
                }
                else
                {
                    // Update
                    contact.contactModified = DateTime.Now;
                    db.Update(contact);
                }
            }
        }
        public static void SaveOrder(Orders order)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (order.ID == 0)
                {
                    // New
                    order.orderDate = DateTime.Now;
                    order.orderModified = DateTime.Now;
                    db.Insert(order);
                }
                else
                {
                    // Update
                    order.orderModified = DateTime.Now;
                    db.Update(order);
                }
            }
        }
        public static void SaveData(Data data)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (data.ID == 0)
                {
                    // New
                    data.dataCreated = DateTime.Now;
                    data.dataModified = DateTime.Now;
                    db.Insert(data);
                }
                else
                {
                    // Update
                    data.dataModified = DateTime.Now;
                    db.Update(data);
                }
            }
        }
        public static void SaveEmail(Email email)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (email.ID == 0)
                {
                    // New
                    email.emailCreated = DateTime.Now;
                    email.emailModified = DateTime.Now;
                    db.Insert(email);
                }
                else
                {
                    // Update
                    email.emailModified = DateTime.Now;
                    db.Update(email);
                }
            }
        }
        public static void SaveSchoolSend(SchoolSend schoolsend)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (schoolsend.ID == 0)
                {
                    // New
                    schoolsend.schoolsendCreated = DateTime.Now;
                    schoolsend.schoolsendModified = DateTime.Now;
                    db.Insert(schoolsend);
                }
                else
                {
                    // Update
                    schoolsend.schoolsendModified = DateTime.Now;
                    db.Update(schoolsend);
                }
            }
        }
        public static void SaveDirectMailing(DirectMailing directmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (directmailing.ID == 0)
                {
                    // New
                    directmailing.directCreated = DateTime.Now;
                    directmailing.directModified = DateTime.Now;
                    db.Insert(directmailing);
                }
                else
                {
                    // Update
                    directmailing.directModified = DateTime.Now;
                    db.Update(directmailing);
                }
            }
        }
        public static void SaveSharedMailing(SharedMailing sharedmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (sharedmailing.ID == 0)
                {
                    // New
                    sharedmailing.sharedCreated = DateTime.Now;
                    sharedmailing.sharedModified = DateTime.Now;
                    db.Insert(sharedmailing);
                }
                else
                {
                    // Update
                    sharedmailing.sharedModified = DateTime.Now;
                    db.Update(sharedmailing);
                }
            }
        }
        public static void SavePrint(Print print)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (print.ID == 0)
                {
                    // New
                    print.printCreated = DateTime.Now;
                    print.printModified = DateTime.Now;
                    db.Insert(print);
                }
                else
                {
                    // Update
                    print.printModified = DateTime.Now;
                    db.Update(print);
                }
            }
        }
        public static void SaveSurcharge(Surcharge surcharge)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (surcharge.ID == 0)
                {
                    // New
                    surcharge.surchargeCreated = DateTime.Now;
                    surcharge.surchargeModified = DateTime.Now;
                    db.Insert(surcharge);
                }
                else
                {
                    // Update
                    surcharge.surchargeModified = DateTime.Now;
                    db.Update(surcharge);
                }
            }
        }
        public static void SaveSchoolSendPack(SchoolSendPack schoolsendpack)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (schoolsendpack.ID == 0)
                {
                    // New
                    db.Insert(schoolsendpack);
                }
                else
                {
                    // Update
                    db.Update(schoolsendpack);
                }
            }
        }
        public static void SaveSharedPack(SharedPack sharedPack)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (sharedPack.ID == 0)
                {
                    // New
                    db.Insert(sharedPack);
                }
                else
                {
                    // Update
                    db.Update(sharedPack);
                }
            }
        }
        #endregion

        #region Deleting
        public static void DeletePerson(Company company)
        {
            // Create a new connection
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {

                // Object model:
                db.Delete(company);

                // SQL Syntax:
                //db.Execute("DELETE FROM Person WHERE Id = ?", company.companyID);
            }
        }
        public static void DeleteCompany(Company company)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(company);
            }
        }
        public static void DeleteContact(Contact contact)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(contact);
            }
        }
        public static void DeleteHistory(CompanyHistory history)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(history);
            }
        }
        public static void DeleteData(Orders order)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(order);
            }
        }
        public static void DeleteData(Data data)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(data);
            }
        }
        public static void DeleteEmail(Email email)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(email);
            }
        }
        public static void DeleteSchoolSend(SchoolSend schoolsend)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(schoolsend);
            }
        }
        public static void DeleteDirectMailing(DirectMailing directmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(directmailing);
            }
        }
        public static void DeleteSharedMailing(SharedMailing sharedmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(sharedmailing);
            }
        }
        public static void DeletePrint(Print print)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(print);
            }
        }
        public static void DeleteSurcharge(Surcharge surcharge)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(surcharge);
            }
        }
        #endregion

        public static ObservableCollection<Contact> GetContactsByCompany(long companyID)
        {
            List<Contact> c;
            ObservableCollection<Contact> c2;
            using(var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                c = (from p in db.Table<Contact>()
                     where p.companyID == companyID
                     select p).ToList();
            }
            c2 = new ObservableCollection<Contact>(c);
            return c2;
        }
    }
}
