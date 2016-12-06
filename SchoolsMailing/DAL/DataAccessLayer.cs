﻿using SchoolsMailing.Models;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
