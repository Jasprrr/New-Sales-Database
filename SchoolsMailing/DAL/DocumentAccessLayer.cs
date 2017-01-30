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
using Windows.Storage.Pickers;

namespace SchoolsMailing.DAL
{
    public class DocumentAccessLayer
    {
        
        #region Paths
        public static string assetsPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path.ToString(), "Assets"); //Get installed assets location
        public static string componentPath = Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString(), "OrderFormComponents"); //Get %APPDATA% "LocalState/Component" component folder
        public static string localPath = Path.Combine(ApplicationData.Current.LocalFolder.Path.ToString()); //Get %APPDATA% "LocalState" folder
        #endregion

        public static string[] orderComponentFiles = new string[]
        {
            "doc_CompanyTable.txt",
            "doc_DataTable.txt",
            "doc_EmailTable.txt",
            "doc_SchoolSendTable.txt",
            "doc_DirectMailingTable.txt",
            "doc_SharedMailingTable.txt",
            "doc_SurchargeTable.txt",
            "doc_Footer.txt",
            "doc_Header.txt",
            "doc_Spacer.txt",
            "foot_Footer.txt",
            "head_Header.txt",
            "word_Rels_Document.txt",
            "docProps_Core.txt"
        };
        
        public static string companyTable;
        public static string spacer;
        public static string dataTable;
        public static string emailTable;
        public static string schoolsendTable;
        public static string directMailingTable;
        public static string sharedMailingTable;
        public static string surchargeTable;
        public static string doc_Header;
        public static string doc_Footer;
        public static string head_Header;
        public static string foot_Footer;
        public static string word_Rels_Document;
        public static string docProps_Core;
        public static string fileSuggestName;

        #region Order lists
        public static List<Data> dataOrders = new List<Data>();
        public static List<Email> emailOrders = new List<Email>();
        public static List<SchoolSend> schoolsendOrders = new List<SchoolSend>();
        public static List<DirectMailing> directMailingOrders = new List<DirectMailing>();
        public static List<SharedMailing> sharedMailingOrders = new List<SharedMailing>();
        public static List<Print> printOrders = new List<Print>();
        public static List<Surcharge> surchargeOrders = new List<Surcharge>();
        #endregion
        
        public static bool isOrderFormAvailable()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(localPath, "OrderForm"))) //Check if order form directory exists
                {
                    File.Copy(Path.Combine(assetsPath, "OrderForm.zip"), Path.Combine(localPath,"OrderForm.zip"),true); //Copy zip file to local path to get app permissions to extract
                    ZipFile.ExtractToDirectory(Path.Combine(localPath, "OrderForm.zip"), localPath); //Extract zip file
                }

                if (!Directory.Exists(Path.Combine(componentPath))) //Check if components folder exists
                {
                    Directory.CreateDirectory(Path.Combine(componentPath)); //Create components folder if it does not
                    
                    foreach (string p in orderComponentFiles)
                    {
                        File.Copy(Path.Combine(assetsPath, p), Path.Combine(componentPath, p), true); //Copy components to components folder
                    }
                }
                else
                {
                    foreach (string p in orderComponentFiles)
                    {
                        if (!File.Exists(Path.Combine(componentPath, p))) //Check all component documents exists
                        {
                            File.Copy(Path.Combine(assetsPath, p), Path.Combine(componentPath, p), true); //Move component file if does not exist
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Critical Error:");
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        
        public static async Task GetFileComponents()
        {
            //Get company table
            StorageFile companyTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_CompanyTable.txt"));
            companyTable = await FileIO.ReadTextAsync(companyTableFile);

            //Get spacer
            StorageFile spacerFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_Spacer.txt"));
            spacer = await FileIO.ReadTextAsync(spacerFile);

            //Get data table
            StorageFile dataTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_DataTable.txt"));
            dataTable = await FileIO.ReadTextAsync(dataTableFile);

            //Get email table
            StorageFile emailTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_EmailTable.txt"));
            emailTable = await FileIO.ReadTextAsync(emailTableFile);

            //Get SchoolSend table
            StorageFile schoolSendTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_SchoolSendTable.txt"));
            schoolsendTable = await FileIO.ReadTextAsync(schoolSendTableFile);

            //Get direct mailing table
            StorageFile directMailingTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_DirectMailingTable.txt"));
            directMailingTable = await FileIO.ReadTextAsync(directMailingTableFile);

            //Get shared mailing table
            StorageFile sharedMailingTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_SharedMailingTable.txt"));
            sharedMailingTable = await FileIO.ReadTextAsync(sharedMailingTableFile);

            //Get surcharge table
            StorageFile surchargeTableFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_SurchargeTable.txt"));
            surchargeTable = await FileIO.ReadTextAsync(surchargeTableFile);

            //Get document header
            StorageFile doc_HeaderFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_Header.txt"));
            doc_Header = await FileIO.ReadTextAsync(doc_HeaderFile);

            //Get document footer
            StorageFile doc_FooterFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "doc_Footer.txt"));
            doc_Footer = await FileIO.ReadTextAsync(doc_FooterFile);

            //Get page header
            StorageFile head_HeaderFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "head_Header.txt"));
            head_Header = await FileIO.ReadTextAsync(head_HeaderFile);

            //Get page footer
            StorageFile foot_FooterFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "foot_Footer.txt"));
            foot_Footer = await FileIO.ReadTextAsync(foot_FooterFile);

            //Get relations document
            StorageFile word_Rels_DocumentFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "word_Rels_Document.txt"));
            word_Rels_Document = await FileIO.ReadTextAsync(word_Rels_DocumentFile);

            StorageFile docProps_CoreFile = await StorageFile.GetFileFromPathAsync(Path.Combine(componentPath, "docProps_Core.txt"));
            docProps_Core = await FileIO.ReadTextAsync(docProps_CoreFile);
        }

        public static void GetOrderParts(Orders selectedOrder)
        {
            dataOrders = DataAccessLayer.getDataByOrderID(selectedOrder.ID);
            emailOrders = DataAccessLayer.getEmailsByOrderID(selectedOrder.ID);
            schoolsendOrders = DataAccessLayer.getSchoolSendByOrderID(selectedOrder.ID);
            directMailingOrders = DataAccessLayer.getDirectMailingByOrderID(selectedOrder.ID);
            sharedMailingOrders = DataAccessLayer.getSharedMailingByOrderID(selectedOrder.ID);
            printOrders = DataAccessLayer.getPrintByOrderID(selectedOrder.ID);
            surchargeOrders = DataAccessLayer.getSurchargeByOrderID(selectedOrder.ID);
        }

        public static async void CreateOrderForm(Orders selectedOrder)
        {
            if (isOrderFormAvailable())
            {
                await GetFileComponents();

                GetOrderParts(selectedOrder);
                CreateDocumentName(selectedOrder);
                CreateCompanyTable(selectedOrder);
                CreateDataTable(selectedOrder);
                CreateEmailTable(selectedOrder);
                CreateSchoolSendTable(selectedOrder);
                CreateDirectMailingTable(selectedOrder);
                CreateSharedMailingTable(selectedOrder);
                CreateSurchargeTable(selectedOrder);

                string Document_XML = string.Empty;
                Document_XML = doc_Header + companyTable + dataTable + emailTable + schoolsendTable + 
                                directMailingTable + sharedMailingTable + surchargeTable + doc_Footer;

                //Write document core file - contains file name
                File.WriteAllText(Path.Combine(localPath, "OrderForm", "docProps", "core.xml"), docProps_Core);

                //Write document body file
                File.WriteAllText(Path.Combine(localPath, "OrderForm", "word", "document.xml"), Document_XML);
                
                if(File.Exists(Path.Combine(localPath, "Form.zip")))
                {
                    //Delete file if already exists - no overwrite method
                    File.Delete(Path.Combine(localPath, "Form.zip"));
                }

                //Create zip file
                ZipFile.CreateFromDirectory(Path.Combine(localPath, "OrderForm"), Path.Combine(localPath, "Form.zip"));

                if (File.Exists(Path.Combine(localPath, "form.docx")))
                {
                    //Delete file if already exists
                    File.Delete(Path.Combine(localPath, "form.docx"));
                }

                //Convert the .zip to .docx
                File.Move(Path.Combine(localPath, "Form.zip"), Path.Combine(localPath, "form.docx"));

                //Create file save dialog
                var folderPicker = new FileSavePicker();

                //Add default location to save dialog
                folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;

                //Add suggested file name to save dialog
                folderPicker.SuggestedFileName = fileSuggestName;

                //Add file type choices to save dialog
                folderPicker.FileTypeChoices.Add("docx", new List<string>() { ".docx" });
                
                //Creates a temporary file and user selected location
                StorageFile tempFile = await folderPicker.PickSaveFileAsync();
                
                //If user created a file
                if(tempFile != null)
                {
                    //Convert new order form to storage file to allow write permissions to restricted locations
                    StorageFile file = await StorageFile.GetFileFromPathAsync(Path.Combine(localPath,"form.docx"));

                    //Replace temporary file with actual order form
                    await file.MoveAndReplaceAsync(tempFile);

                    //TODO: Add user setting to auto open ms word

                    //Open file with ms word protocol
                    var asd = Windows.System.Launcher.LaunchUriAsync(new Uri("ms-word:ofe|u|" + tempFile.Path.ToString()));
                }
            }
        }
        
        public static void CreateDocumentName(Orders selectedOrder)
        {
            string fileName = string.Empty;
            fileName = fileName + selectedOrder.orderDate.Date.ToString("dd-MMM-yy") + " ";
            fileName = fileName + replaceInvalidCharacters(selectedOrder.companyName) + " ";

            fileName = fileName + ((dataOrders.Count          > 0) ? "Data "          : string.Empty);
            fileName = fileName + ((emailOrders.Count         > 0) ? "Email "         : string.Empty);
            fileName = fileName + ((schoolsendOrders.Count    > 0) ? "SchoolSend "    : string.Empty);
            fileName = fileName + ((directMailingOrders.Count > 0) ? "DirectMailing " : string.Empty);
            fileName = fileName + ((sharedMailingOrders.Count > 0) ? "SharedMailing " : string.Empty);
            fileName = fileName + ((printOrders.Count         > 0) ? "Print "         : string.Empty);
            fileName = fileName + ((surchargeOrders.Count     > 0) ? "Surcharge "     : string.Empty);

            //fileName = fileName.Substring(0, fileName.Length - 2);

            fileName = fileName + "Order Form " + selectedOrder.orderCode;
            fileSuggestName = fileName.Replace(" ", "_");
            docProps_Core = string.Format(docProps_Core,
                                            fileName);
        }

        public static void CreateCompanyTable(Orders selectedOrder)
        {
            //Format Address
            string address = string.Empty;
            address = address + ((selectedOrder.companyAddress1 != null) ? selectedOrder.companyAddress1 + ", " : string.Empty);
            address = address + ((selectedOrder.companyAddress2 != null) ? selectedOrder.companyAddress2 + ", " : string.Empty);
            address = address + ((selectedOrder.companyCity     != null) ? selectedOrder.companyCity     + ", " : string.Empty);
            address = address + ((selectedOrder.companyCounty   != null) ? selectedOrder.companyCounty   + ", " : string.Empty);
            address = address + ((selectedOrder.companyPostCode != null) ? selectedOrder.companyPostCode + ", " : string.Empty);
            //Remove last ", "
            address = (address != string.Empty) ? address.Substring(0, address.Length - 2) : string.Empty;

            //Format name
            string name = string.Empty;
            name = name + ((selectedOrder.contactTitle    != null) ? selectedOrder.contactTitle    + " " : string.Empty);
            name = name + ((selectedOrder.contactForename != null) ? selectedOrder.contactForename + " " : string.Empty);
            name = name + ((selectedOrder.contactSurname  != null) ? selectedOrder.contactSurname  + " " : string.Empty);
            //Remove double spaces
            name = (name != string.Empty) ? name.Trim() : string.Empty;

            companyTable = string.Format(companyTable, 
                                            selectedOrder.orderDate.Date.ToString(), 
                                            selectedOrder.orderCode, 
                                            selectedOrder.companyName, 
                                            address, 
                                            selectedOrder.contactTelephone, 
                                            selectedOrder.contactEmail, 
                                            name); //Add parameters to read document
        }

        public static void CreateDataTable(Orders selectedOrder)
        {
            if (dataOrders.Count != 0)
            {
                string dataStart = string.Empty;
                string dataEnd = string.Empty;
                string dataDetails = string.Empty;
                string dataCosts = string.Empty;
                double dataTotal = 0;

                //Iterate through each data order and comma seperate each part
                foreach (Data p in dataOrders)
                {
                    dataStart = dataStart + p.dataStart.Date + ", ";
                    dataEnd = dataEnd + p.dataEnd.ToString() + ", ";
                    dataDetails = dataDetails + p.dataDetails.ToString() + ", ";
                    dataCosts = dataCosts + p.dataCost.ToString() + ", ";
                    dataTotal = dataTotal + p.dataCost;
                }
                dataStart = dataStart.Substring(0, dataStart.Length - 2); //Remove last ", "
                dataEnd = dataEnd.Substring(0, dataEnd.Length - 2); //Remove last ", "
                dataDetails = dataDetails.Substring(0, dataDetails.Length - 2); //Remove last ", "
                dataCosts = dataCosts.Substring(0, dataCosts.Length - 2); //Remove last ", "

                //Adds variables to string
                dataTable = spacer + string.Format(dataTable,
                                                    dataStart,
                                                    dataEnd,
                                                    dataDetails,
                                                    dataCosts, 
                                                    dataTotal);
            }
            else
            {
                dataTable = string.Empty;
            }
        }

        public static void CreateEmailTable(Orders selectedOrder)
        {
            if(emailOrders.Count != 0)
            {
                string emailDate = string.Empty;
                string emailDetails = string.Empty;
                string emailSubject = string.Empty;
                string emailArtwork = @"send to josh@schoolsmailing.co.uk";
                string emailCost = string.Empty;

                foreach(Email p in emailOrders)
                {
                    emailDate = emailDate + p.emailDate.Date + ", ";
                    emailDetails = emailDetails + p.emailDetails + Environment.NewLine + Environment.NewLine;
                    emailSubject = emailSubject + p.emailSubject + Environment.NewLine;
                    emailCost = emailCost + p.emailCost.ToString() + ", ";
                }

                emailDate = emailDate.Substring(0, emailDate.Length - 2);
                emailDetails = emailDetails.Substring(0, emailDetails.Length - 2);
                emailCost = emailCost.Substring(0, emailCost.Length - 2);

                emailTable = spacer + string.Format(emailTable,
                                                        emailDate,
                                                        emailDetails,
                                                        emailSubject,
                                                        emailArtwork,
                                                        emailCost);
            }
            else
            {
                emailTable = string.Empty;
            }
        }

        public static void CreateSchoolSendTable(Orders selectedOrder)
        {
            if(schoolsendOrders.Count != 0)
            {
                string schoolsendStart = string.Empty;
                string schoolsendEnd = string.Empty;
                string schoolsendCredits = string.Empty;
                string schoolsendCost = string.Empty;

                foreach(SchoolSend p in schoolsendOrders)
                {
                    schoolsendStart = schoolsendStart + p.schoolsendStart.Date + ", ";
                    schoolsendEnd = schoolsendEnd + p.schoolsendEnd.Date + ", ";
                    schoolsendCredits = schoolsendCredits + p.schoolsendCredits.ToString() + ", ";
                    schoolsendCost = schoolsendCost + p.schoolsendCost.ToString() + ", ";
                }

                schoolsendStart = schoolsendStart.Substring(0, schoolsendStart.Length - 2);
                schoolsendEnd = schoolsendEnd.Substring(0, schoolsendEnd.Length - 2);
                schoolsendCredits = schoolsendCredits.Substring(0, schoolsendCredits.Length - 2);
                schoolsendCost = schoolsendCost.Substring(0, schoolsendCost.Length - 2);

                schoolsendTable = spacer + string.Format(schoolsendTable,
                                                            schoolsendStart,
                                                            schoolsendEnd,
                                                            schoolsendCredits,
                                                            schoolsendCost);
            }
            else
            {
                schoolsendTable = string.Empty;
            }
        }

        public static void CreateDirectMailingTable(Orders selectedOrder)
        {
            if(directMailingOrders.Count != 0)
            {
                string directDate = string.Empty;
                string directDataDate = string.Empty;
                string directInsertDate = string.Empty;
                string directArtworkDate = string.Empty;
                string directLeafletCode = string.Empty;
                string directDeliveryCode = string.Empty;
                string directMailingTo = string.Empty;
                string directDetails = string.Empty;
                string directFulfilment = string.Empty;
                string directPrint = string.Empty;
                string directPostage = string.Empty;

                foreach(DirectMailing p in directMailingOrders)
                {
                    directDate = directDate + p.directDate.Date + ", ";
                    directDataDate = directDataDate + p.directDataDate.ToString() + ", ";
                    directInsertDate = directInsertDate + p.directInsertDate.ToString() + ", ";
                    directArtworkDate = directArtworkDate + p.directArtworkDate.ToString() + ", ";
                    directLeafletCode = directLeafletCode + p.directLeafletCode + ", ";
                    directDeliveryCode = directDeliveryCode + p.directDeliveryCode + ", ";
                    directMailingTo = directMailingTo + p.directMailingTo + ", ";
                    directDetails = directDetails + p.directDetails + ", ";
                    directFulfilment = directFulfilment + p.directFulfilmentCost.ToString() + ", ";
                    directPrint = directPrint + p.directPrintCost.ToString() + ", ";
                    directPostage = directPostage + p.directPostageCost.ToString() + ", ";
                }

                directDate = directDate.Substring(0, directDate.Length - 2);
                directDataDate = directDataDate.Substring(0, directDataDate.Length - 2);
                directInsertDate = directInsertDate.Substring(0, directInsertDate.Length - 2);
                directArtworkDate = directArtworkDate.Substring(0, directArtworkDate.Length - 2);
                directLeafletCode = directLeafletCode.Substring(0, directLeafletCode.Length - 2);
                directDeliveryCode = directDeliveryCode.Substring(0, directDeliveryCode.Length - 2);
                directMailingTo = directMailingTo.Substring(0, directMailingTo.Length - 2);
                directDetails = directDetails.Substring(0, directDetails.Length - 2);
                directFulfilment = directFulfilment.Substring(0, directFulfilment.Length - 2);
                directPrint = directPrint.Substring(0, directPrint.Length - 2);
                directPostage = directPostage.Substring(0, directPostage.Length - 2);

                directMailingTable = spacer + string.Format(directMailingTable,
                                                                directDate,
                                                                directDataDate,
                                                                directInsertDate,
                                                                directArtworkDate,
                                                                directLeafletCode,
                                                                directDeliveryCode,
                                                                directMailingTo,
                                                                directDetails,
                                                                directFulfilment,
                                                                directPrint,
                                                                directPostage);
            }
            else
            {
                directMailingTable = string.Empty;
            }
        }

        public static void CreateSharedMailingTable(Orders selectedOrder)
        {
            if(sharedMailingOrders.Count != 0)
            {
                string sharedDate = string.Empty;
                string sharedMailingTo = string.Empty;
                string sharedArtworkDate = string.Empty;
                string sharedDeliveryDate = string.Empty;
                string sharedFAO = string.Empty;
                string sharedLeafletName = string.Empty;
                string sharedSize = string.Empty;
                string sharedWeight = string.Empty;
                string sharedCost = string.Empty;

                foreach(SharedMailing p in sharedMailingOrders)
                {
                    sharedDate = sharedDate + p.sharedDate.Date + ", ";
                    sharedMailingTo = sharedMailingTo+ p.sharedMailingTo + ", ";
                    sharedArtworkDate = sharedArtworkDate + p.sharedArtworkDate.Date + ", ";
                    sharedDeliveryDate = sharedDeliveryDate + p.sharedDeliveryDate.Date +", ";
                    sharedFAO = sharedFAO + p.sharedFAO + ", ";
                    sharedLeafletName = sharedLeafletName + p.sharedLeafletName + ", ";
                    sharedSize = sharedSize + p.sharedLeafletSize + ", ";
                    sharedWeight = sharedWeight + p.sharedLeafletWeight + ", ";
                    sharedCost = sharedCost + p.sharedCost.ToString() + ", ";
                }

                sharedDate = sharedDate.Substring(0, sharedDate.Length - 2);
                sharedMailingTo = sharedMailingTo.Substring(0, sharedMailingTo.Length - 2);
                sharedArtworkDate = sharedArtworkDate.Substring(0, sharedArtworkDate.Length - 2);
                sharedDeliveryDate = sharedDeliveryDate.Substring(0, sharedDeliveryDate.Length - 2);
                sharedFAO = sharedFAO.Substring(0, sharedFAO.Length - 2);
                sharedLeafletName = sharedLeafletName.Substring(0, sharedLeafletName.Length - 2);
                sharedSize = sharedSize.Substring(0, sharedSize.Length - 2);
                sharedWeight = sharedWeight.Substring(0, sharedWeight.Length - 2);
                sharedCost = sharedCost.Substring(0, sharedCost.Length - 2);
            }
            else
            {
                sharedMailingTable = string.Empty;
            }
        }

        public static void CreatePrintTable(Orders selectedOrder)
        {
            //Add print table?
        }

        public static void CreateSurchargeTable(Orders selectedOrder)
        {
            if(surchargeOrders.Count != 0)
            {
                string surchargeDate = string.Empty;
                string surchargeDetails = string.Empty;
                string surchargeCost = string.Empty;

                foreach(Surcharge p in surchargeOrders)
                {
                    surchargeDate = surchargeDate + p.surchargeDate.Date + ", ";
                    surchargeDetails = surchargeDetails + p.surchargeDetails + ", ";
                    surchargeCost = surchargeCost + p.surchargeCost.ToString() + ", ";
                }

                surchargeDate = surchargeDate.Substring(0, surchargeDate.Length - 2);
                surchargeDetails = surchargeDetails.Substring(0, surchargeDetails.Length - 2);
                surchargeCost = surchargeCost.Substring(0, surchargeCost.Length - 2);
            }
            else
            {
                surchargeTable = string.Empty;
            }
        }

        public static string replaceInvalidCharacters(string invalidString)
        {
            return Path.GetInvalidFileNameChars().Aggregate(invalidString, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
