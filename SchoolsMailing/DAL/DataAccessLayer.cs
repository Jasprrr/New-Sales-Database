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

        #region Get by ID
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

        public static ObservableCollection<Contact> GetContactsByCompany(int companyID)
        {
            List<Contact> c;
            ObservableCollection<Contact> c2;
            using(var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                c = (from p in db.Table<Contact>()
                     select p).ToList();
            }
            c2 = new ObservableCollection<Contact>(c);
            return c2;
        }
    }
}
