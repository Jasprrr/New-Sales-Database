using SchoolsMailing.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SchoolsMailing.DAL
{
    public class DocumentAccessLayer
    {
        public long orderID = 1;
        public Orders selectedOrder { get { return DAL.DataAccessLayer.GetOrder(orderID); } }

        #region URIs
        public static Uri Assets = new Uri(@"ms-appx:///Assets/");
        public static Uri OrderForm = new Uri(@"ms-appx:///Assets/OrderForm.zip");
        public static Uri[] orderComponentLocations = new Uri[] 
        {
            new Uri(@"ms-appx:///Assets/doc_CompanyTable.txt"),
            new Uri(@"ms-appx:///Assets/doc_DataTable.txt"),
            new Uri(@"ms-appx:///Assets/doc_EmailTable.txt"),
            new Uri(@"ms-appx:///Assets/doc_SchoolSend.txt"),
            new Uri(@"ms-appx:///Assets/doc_DirectMailingTable.txt"),
            new Uri(@"ms-appx:///Assets/doc_SharedMailingTable.txt"),
            new Uri(@"ms-appx:///Assets/doc_Surcharge.txt"),
            new Uri(@"ms-appx:///Assets/doc_Footer.txt"),
            new Uri(@"ms-appx:///Assets/doc_Header.txt"),
            new Uri(@"ms-appx:///Assets/doc_Spacer.txt"),
            new Uri(@"ms-appx:///Assets/foot_Footer.txt"),
            new Uri(@"ms-appx:///Assets/head_Header.txt"),
            new Uri(@"ms-appx:///Assets/word_Rels_Document")
        };

        public static string[] orderComponentFiles = new string[]
        {
            "doc_CompanyTable.txt",
            "doc_DataTable.txt",
            "doc_EmailTable.txt",
            "doc_SchoolSend.txt",
            "doc_DirectMailingTable.txt",
            "doc_SharedMailingTable.txt",
            "doc_Surcharge.txt",
            "doc_Footer.txt",
            "doc_Header.txt",
            "doc_Spacer.txt",
            "foot_Footer.txt",
            "head_Header.txt",
            "word_Rels_Document"
        };
        #endregion URIs

        #region Order lists
        public static List<Data> dataOrders = new List<Data>();
        public static List<Email> emailOrders = new List<Email>();
        public static List<SchoolSend> schoolSendOrders = new List<SchoolSend>();
        public static List<DirectMailing> directMailingOrders = new List<DirectMailing>();
        public static List<SharedMailing> sharedMailingOrders = new List<SharedMailing>();
        public static List<Print> printOrders = new List<Print>();
        public static List<Surcharge> surchargeOrders = new List<Surcharge>();
        #endregion

        public static bool OrderFormAvailable()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderForm"))) //Check if order form directory exists
                {
                    ZipFile.ExtractToDirectory(OrderForm.LocalPath.ToString(), ApplicationData.Current.LocalFolder.Path.ToString()); //Unzip default order form to location
                }

                if (!Directory.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderFormComponents"))) //Check if components folder exists
                {
                    Directory.CreateDirectory(Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderFormComponents")); //Create components folder if it does not

                    foreach (Uri p in orderComponentLocations)
                    {
                        File.Move(p.LocalPath.ToString(), Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderFormComponents")); //Move components to components folder
                    }
                }
                else
                {
                    foreach (string p in orderComponentFiles)
                    {
                        if (!File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderFormComponents", p))) //Check all component documents exists
                        {
                            File.Move(Path.Combine(Assets.LocalPath.ToString(), p), Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), p)); //Move component file if does not exist
                        }
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Critical Error:");
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static void CreateOrderForm(Orders selectedOrder)
        {
            if (OrderFormAvailable())
            {
                dataOrders = DataAccessLayer.ListDataByOrderID(selectedOrder.ID);
                emailOrders = DataAccessLayer.ListEmailByOrderID(selectedOrder.ID);
                schoolSendOrders = DataAccessLayer.ListSchoolSendByOrderID(selectedOrder.ID);
                directMailingOrders = DataAccessLayer.ListDirectMailingByOrderID(selectedOrder.ID);
                sharedMailingOrders = DataAccessLayer.ListSharedMailingByOrderID(selectedOrder.ID);
                printOrders = DataAccessLayer.ListPrintByOrderID(selectedOrder.ID);
                surchargeOrders = DataAccessLayer.ListSurchargeByOrderID(selectedOrder.ID);

                string Document_XML = "" + CompanyTable(selectedOrder);


            }
        }

        public static async Task<string> CompanyTable(Orders selectedOrder)
        {
            //{0} - Company
            //{1} - Address
            //{2} - Telephone
            //{3} - Email
            //{4} - Name

            StorageFile companyTableFile = await StorageFile.GetFileFromPathAsync(ApplicationData.Current.LocalFolder.Path.ToString());
            string companyTable = await FileIO.ReadTextAsync(companyTableFile);

            //Format Address
            string address = string.Empty;
            if (selectedOrder.companyAddress1 != null)
            {
                address = address + selectedOrder.companyAddress1 + ", ";
            }
            if (selectedOrder.companyAddress2 != null)
            {
                address = address + selectedOrder.companyAddress2 + ", ";
            }
            if (selectedOrder.companyCity != null)
            {
                address = address + selectedOrder.companyCity + ", ";
            }
            if (selectedOrder.companyCounty != null)
            {
                address = address + selectedOrder.companyCounty + ", ";
            }
            if (selectedOrder.companyPostCode != null)
            {
                address = address + selectedOrder.companyPostCode + ", ";
            }
            if (address != string.Empty)
            {
                address = address.Substring(0, address.Length - 2); //Remove last ", "
            }

            //Format Name
            string name = string.Empty;
            if (selectedOrder.contactTitle != null)
            {
                name = name + selectedOrder.contactTitle + " ";
            }
            if (selectedOrder.contactForename != null)
            {
                name = name + selectedOrder.contactForename + " ";
            }
            if (selectedOrder.contactSurname != null)
            {
                name = name + selectedOrder.contactSurname + " ";
            }
            if(name != string.Empty)
            {
                name = name.Trim(); //Remove double spaces
            }

            companyTable = string.Format(companyTable, selectedOrder.companyName, address, selectedOrder.contactTelephone, selectedOrder.contactEmail, name); //Add parameters to read document

            return companyTable;
        }

        public static string DataTable()
        {
            return "";
        }

        public static string EmailTable()
        {
            return "";
        }

        public static string SchoolSendTable()
        {
            return "";
        }

        public static string DirectMailingTable()
        {
            return "";
        }

        public static string SharedMailingTable()
        {
            return "";
        }

        public static string PrintTable()
        {
            return "";
        }

        public static string SurchargeTable()
        {
            return "";
        }
    }
}
