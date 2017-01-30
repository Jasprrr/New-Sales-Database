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


        public static bool isValidUsername(string userName)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<User> u;
                u = (from p in db.Table<User>()
                     where p.userName == userName
                     select p).ToList();
                return (u.Count == 1) ? true : false;
            }
        }

        public static bool isValidPassword(string userName, string userPassword)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<User> u;
                u = (from p in db.Table<User>()
                     where p.userName == userName && p.userPassword == userPassword
                     select p).ToList();
                return (u.Count == 1) ? true : false;
            }
        }

        public static Company getCompanyByID(long companyID)
        {
            Company q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Company>()
                     where p.ID == companyID
                     select p).FirstOrDefault();
            }
            return q;
        }
        public static List<Company> getCompanies()
        {
            List<Company> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Company>()
                     orderby p.companyName descending
                     select p).ToList();
            }
            return l;
        }

        public static Contact getContactByID(long contactID)
        {
            Contact q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Contact>()
                     where p.ID == contactID
                     select p).FirstOrDefault();
            }
            return q;
        }
        public static List<Contact> getContactsByCompanyID(long companyID)
        {
            List<Contact> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Contact>()
                     where p.companyID == companyID
                     orderby p.contactSurname descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Contact> getContacts()
        {
            List<Contact> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Contact>()
                     orderby p.contactSurname descending
                     select p).ToList();
            }
            return l;
        }

        public static List<CompanyHistory> getHistoryByCompanyID(long companyID)
        {
            List<CompanyHistory> h;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                h = (from p in db.Table<CompanyHistory>()
                     where p.companyID == companyID
                     orderby p.companyHistoryDate descending
                     select p).ToList();
            }
            return h;
        }
        public static CompanyHistory getCompanyHistoryByID(long historyID)
        {
            CompanyHistory q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<CompanyHistory>()
                     where p.ID == historyID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static Orders getOrderByID(long orderID)
        {
            Orders q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Orders>()
                     orderby p.orderDate descending
                     select p).FirstOrDefault();
            }
            return q;
        }
        public static List<Orders> getOrders()
        {
            List<Orders> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Orders>()
                     orderby p.orderDate descending
                     select p).ToList();
            }
            return l;
        }

        public static List<OrdersEmail> getOrdersEmails()
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
        public static List<OrdersEmail> getOrdersEmailsByCompanyID(long companyID)
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
                                             WHERE Email.orderID = Orders.ID AND Orders.companyID = ?
                                             ORDER BY Email.emailDate DESC;", companyID).ToList();
                return oe;
            }
        }
        public static List<OrdersEmail> getOrdersEmailsByOrderID(long orderID)
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
        public static List<Email> getEmailsByOrderID(long orderID)
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
        public static List<Email> getEmails()
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
        public static Email getEmailByID(long emailID)
        {
            Email q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Email>()
                     where p.ID == emailID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersData> getOrdersData()
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
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
        public static List<OrdersData> getOrdersDataByCompanyID(long companyID)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                List<OrdersData> od;
                od = db.Query<OrdersData>(@"SELECT Orders.ID, Orders.userID, Orders.companyID, Orders.contactID, Orders.promotionID, Orders.orderCode, Orders.orderDate, Orders.orderModified, 
                                                   Orders.orderPayment, Orders.orderContent, Orders.orderRemove, Orders.companyName, Orders.companyAddress1, Orders.companyAddress2, Orders.companyCity, 
                                                   Orders.companyCounty, Orders.companyPostCode, Orders.companyProspects, Orders.contactTitle, Orders.contactForename, Orders.contactSurname, 
                                                   Orders.contactEmail, Orders.contactTelephone, Orders.orderTotal, Orders.orderTotalVAT, Data.dataDetails, Data.dataCost, Data.dataStart, 
                                                   Data.dataEnd, Data.dataCreated, Data.dataModified 
                                            FROM Data, Orders 
                                            WHERE Data.orderID = Orders.ID AND Orders.companyID = ? 
                                            ORDER BY Data.dataEnd DESC;", companyID).ToList();
                return od;
            }
        }
        public static List<OrdersData> getOrdersDataByOrderID(long orderID)
        {
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
        public static List<Data> getDataByOrderID(long orderID)
        {
            List<Data> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Data>()
                     where p.orderID == orderID
                     orderby p.dataStart descending
                     select p).ToList();
            }
            return l;
        }
        public static List<Data> getData()
        {
            List<Data> l;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                l = (from p in db.Table<Data>()
                     orderby p.dataStart descending
                     select p).ToList();
            }
            return l;
        }
        public static Data getDataByID(long dataID)
        {
            Data q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Data>()
                     where p.ID == dataID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersSchoolSend> getOrdersSchoolSend()
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
        public static List<OrdersSchoolSend> getOrdersSchoolSendByCompanyID(long companyID)
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
                                            WHERE SchoolSend.orderID = Orders.ID AND Orders.companyID = ? 
                                            ORDER BY SchoolSend.schoolsendEnd DESC;", companyID).ToList();
                return os;
            }
        }
        public static List<OrdersSchoolSend> getOrdersSchoolSendByOrderID(long orderID)
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
        public static List<SchoolSend> getSchoolSendByOrderID(long orderID)
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
        public static List<SchoolSend> getSchoolSend()
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
        public static SchoolSend getSchoolSendByID(long schoolsendID)
        {
            SchoolSend q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<SchoolSend>()
                     where p.ID == schoolsendID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersDirectMailing> getOrdersDirectMailing()
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
        public static List<OrdersDirectMailing> getOrdersDirectMailingByCompanyID(long companyID)
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
                                                     WHERE DirectMailing.orderID = Orders.ID AND Orders.companyID = ?
                                                     ORDER BY DirectMailing.directDate DESC;", companyID).ToList();
                return odm;
            }
        }
        public static List<OrdersDirectMailing> getOrdersDirectMailingByOrderID(long orderID)
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
        public static List<DirectMailing> getDirectMailingByOrderID(long orderID)
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
        public static List<DirectMailing> getDirectMailing()
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
        public static DirectMailing getDirectMailingByID(long directMailingID)
        {
            DirectMailing q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<DirectMailing>()
                     where p.ID == directMailingID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersSharedMailing> getOrdersSharedMailing()
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
        public static List<OrdersSharedMailing> getOrdersSharedMailingByCompanyID(long companyID)
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
                                            WHERE SharedMailing.orderID = Orders.ID AND Orders.companyID = ?
                                            ORDER BY SharedMailing.sharedDate DESC;", companyID).ToList();
                return osm;
            }
        }
        public static List<OrdersSharedMailing> getOrdersSharedMailingByOrderID(long orderID)
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
        public static List<SharedMailing> getSharedMailingByOrderID(long orderID)
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
        public static List<SharedMailing> getSharedMailing()
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
        public static SharedMailing getSharedMailingByID(long sharedMailingID)
        {
            SharedMailing q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<SharedMailing>()
                     where p.ID == sharedMailingID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersPrint> getOrdersPrint()
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
        public static List<OrdersPrint> getOrdersPrintByCompanyID(long companyID)
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
                                            WHERE Print.orderID = Orders.ID AND Orders.companyID = ?
                                            ORDER BY Print.printDate DESC;", companyID).ToList();
                return op;
            }
        }
        public static List<OrdersPrint> getOrdersPrintByOrderID(long orderID)
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
        public static List<Print> getPrintByOrderID(long orderID)
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
        public static List<Print> getPrint()
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
        public static Print getPrintByID(long printID)
        {
            Print q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Print>()
                     where p.ID == printID
                     select p).FirstOrDefault();
            }
            return q;
        }

        public static List<OrdersSurcharge> getOrdersSurcharge()
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
        public static List<OrdersSurcharge> getOrdersSurchargeByCompanyID(long companyID)
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
                                            WHERE Surcharge.orderID = Orders.ID AND Orders.companyID = ?
                                            ORDER BY Surcharge.surchargeDate DESC;", companyID).ToList();
                return os;
            }
        }
        public static List<OrdersSurcharge> getOrdersSurchargeByOrderID(long orderID)
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
        public static List<Surcharge> getSurchargeByOrderID(long orderID)
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
        public static List<Surcharge> getSurcharge()
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
        public static Surcharge getSurchargeByID(long surchargeID)
        {
            Surcharge q;
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                q = (from p in db.Table<Surcharge>()
                     where p.ID == surchargeID
                     select p).FirstOrDefault();
            }
            return q;
        }

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
        public static void saveCompany(Company company)
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
        public static void saveHistory(CompanyHistory history)
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
        public static void saveContact(Contact contact)
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
        public static void saveOrder(Orders order)
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
        public static void saveData(Data data)
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
        public static void saveEmail(Email email)
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
        public static void saveSchoolSend(SchoolSend schoolsend)
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
        public static void saveDirectMailing(DirectMailing directmailing)
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
        public static void saveSharedMailing(SharedMailing sharedmailing)
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
        public static void savePrint(Print print)
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
        public static void saveSurcharge(Surcharge surcharge)
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
        public static void saveSchoolSendPack(SchoolSendPack schoolsendpack)
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
        public static void saveSharedPack(SharedPack sharedPack)
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
        public static void deletePerson(Company company)
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
        public static void deleteCompany(Company company)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(company);
            }
        }
        public static void deleteContact(Contact contact)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(contact);
            }
        }
        public static void deleteHistory(CompanyHistory history)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(history);
            }
        }
        public static void deleteData(Orders order)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(order);
            }
        }
        public static void deleteData(Data data)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(data);
            }
        }
        public static void deleteEmail(Email email)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(email);
            }
        }
        public static void deleteSchoolSend(SchoolSend schoolsend)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(schoolsend);
            }
        }
        public static void deleteDirectMailing(DirectMailing directmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(directmailing);
            }
        }
        public static void deleteSharedMailing(SharedMailing sharedmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(sharedmailing);
            }
        }
        public static void deletePrint(Print print)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                db.Delete(print);
            }
        }
        public static void deleteSurcharge(Surcharge surcharge)
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
