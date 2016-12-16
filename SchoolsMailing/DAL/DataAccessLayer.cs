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

        public static ObservableCollection<Contact> GetAllContactsByCompany(long companyID)
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

        #endregion

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

        #region Orders
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
        #endregion

        #region Data
        public static ObservableCollection<Data> GetAllData(long orderID)
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
        public static void SaveData(Data data)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (data.ID == 0)
                {
                    // New
                    db.Insert(data);
                }
                else
                {
                    // Update
                    db.Update(data);
                }
            }
        }
        #endregion

        #region Email
        public static ObservableCollection<Email> GetAllEmails(long orderID)
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
        public static void SaveEmail(Email email)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (email.ID == 0)
                {
                    // New
                    db.Insert(email);
                }
                else
                {
                    // Update
                    db.Update(email);
                }
            }
        }
        #endregion

        #region Direct Mailing
        public static ObservableCollection<DirectMailing> GetAllDirectMailings(long orderID)
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
        public static void SaveDirectMailing(DirectMailing directmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (directmailing.ID == 0)
                {
                    // New
                    db.Insert(directmailing);
                }
                else
                {
                    // Update
                    db.Update(directmailing);
                }
            }
        }
        #endregion

        #region Shared Mailing
        public static ObservableCollection<SharedMailing> GetAllSharedMailings(long orderID)
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
        public static void SaveSharedMailing(SharedMailing sharedmailing)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (sharedmailing.ID == 0)
                {
                    // New
                    db.Insert(sharedmailing);
                }
                else
                {
                    // Update
                    db.Update(sharedmailing);
                }
            }
        }
        #endregion

        #region Surcharge
        public static ObservableCollection<Surcharge> GetAllSurcharges(long orderID)
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
        public static void SaveSurcharge(Surcharge surcharge)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (surcharge.ID == 0)
                {
                    // New
                    db.Insert(surcharge);
                }
                else
                {
                    // Update
                    db.Update(surcharge);
                }
            }
        }
        #endregion

        #region SchoolSend
        public static ObservableCollection<SchoolSend> GetAllSchoolSends(long orderID)
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
        public static void SaveSchoolSend(SchoolSend schoolsend)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (schoolsend.ID == 0)
                {
                    // New
                    db.Insert(schoolsend);
                }
                else
                {
                    // Update
                    db.Update(schoolsend);
                }
            }
        }
        #endregion

        #region Print
        public static ObservableCollection<Print> GetAllPrint(long orderID)
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
        public static void SavePrint(Print print)
        {
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                if (print.ID == 0)
                {
                    // New
                    db.Insert(print);
                }
                else
                {
                    // Update
                    db.Update(print);
                }
            }
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
                    db.Insert(company);
                }
                else
                {
                    // Update
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
                    db.Insert(contact);
                }
                else
                {
                    // Update
                    db.Update(contact);
                }
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
