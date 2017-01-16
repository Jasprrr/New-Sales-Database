using SchoolsMailing.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SchoolsMailing.Common;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SchoolsMailing.Views;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;
using System.IO;
using Windows.Storage;
using System.IO.Compression;
using Windows.Storage.AccessCache;
using Windows.Data.Xml.Dom;
using System.Xml;
using System.Xml.Linq;

namespace SchoolsMailing.ViewModels
{
    public class LoginViewModel : PageViewModel
    {
        public LoginViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
        }

        private string _username;
        public string username
        {
            get { return _username; }
            set
            {
                if(_username != value)
                {
                    _username = value;
                    RaisePropertyChanged("username");
                }
            }
        }
        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if(_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("password");
                }
            }
        }

        private bool _invalidPassword = false;
        public bool invalidPassword
        {
            get { return _invalidPassword; }
            set { if(_invalidPassword != value) { _invalidPassword = !invalidPassword; RaisePropertyChanged("invalidPassword"); } }
        }

        private bool _invalidUsername = false;
        public bool invalidUsername
        {
            get { return _invalidUsername; }
            set { if (_invalidUsername != value) { _invalidUsername = !invalidUsername; RaisePropertyChanged("invalidUsername"); } }
        }

        //TODO: add enter event to login

        private RelayCommand _authenticateLogin;
        public RelayCommand authenticateLogin
        {
            get
            {
                if (_authenticateLogin == null)
                {
                    _authenticateLogin = new RelayCommand(() =>
                    {
                        if(username == "admin")
                        {
                            invalidUsername = false;

                            if(password == "@")
                            {
                                DoMove();
                                //ViewModels.MoveFile.DoMove();
                                //var rootFrame = Window.Current.Content as Frame;

                                //rootFrame.Navigate(typeof(MainPage));
                                //ZipFile.
                            }
                            else
                            {
                                invalidPassword = true;
                            }
                        }
                        else
                        {
                            invalidUsername = true;
                        }
                    });
                }

                return _authenticateLogin;

            }
        }

        public async void DoMove()
        {
            //StorageFolder test = await StorageFolder.GetFolderFromPathAsync("");
            Debug.WriteLine(string.Format(Windows.Storage.ApplicationData.Current.LocalFolder.Path.ToString()));
            //string locPath = @"C:\Users\Jasper\AppData\Local\Packages\430d4efd-8648-4a35-8670-6dcecc88d151_7mzr475ysvhxg\LocalState\CreateDoc";
            //string desPath = @"C:\Users\Jasper\AppData\Local\Packages\430d4efd-8648-4a35-8670-6dcecc88d151_7mzr475ysvhxg\LocalState\FinishDoc\test.docx";
            //ZipFile.CreateFromDirectory(locPath, desPath);
            
            //XDocument doc = new XDocument(new XElement("test", new XAttribute("test_name", "test_Value"), new XElement("test_Child", "test_node")));
            //XmlWriter writer = XmlWriter.Create()
            //await doc.Save();

            StorageFolder fol = ApplicationData.Current.LocalFolder;
            StorageFile fil = await fol.CreateFileAsync("test.xml", CreationCollisionOption.ReplaceExisting);
            //XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            //XNamespace cx = "http://schemas.microsoft.com/office/drawing/2014/chartex";
            //XNamespace cx1 = "http://schemas.microsoft.com/office/drawing/2015/9/8/chartex";
            //XNamespace cx2 = "http://schemas.microsoft.com/office/drawing/2015/10/21/chartex";
            //XNamespace cx3 = "http://schemas.microsoft.com/office/drawing/2016/5/9/chartex";
            //XNamespace cx4 = "http://schemas.microsoft.com/office/drawing/2016/5/10/chartex";
            //XNamespace cx5 = "http://schemas.microsoft.com/office/drawing/2016/5/11/chartex";
            //XNamespace m = "http://schemas.openxmlformats.org/officeDocument/2006/math";
            //XNamespace mc = "http://schemas.openxmlformats.org/markup-compatibility/2006";
            //XNamespace o = "urn:schemas-microsoft-com:office:office";
            //XNamespace r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
            //XNamespace v = "urn:schemas-microsoft-com:vml";

            //XDocument xdoc = new XDocument(new XElement(w + "document", new XAttribute(XNamespace.Xmlns+"w", w.NamespaceName)));

            await FileIO.WriteTextAsync(fil, @"<?xml version=""1.0"" encoding=""UTF-8""?>
<w:document xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" 
			xmlns:cx=""http://schemas.microsoft.com/office/drawing/2014/chartex"" 
			xmlns:cx1=""http://schemas.microsoft.com/office/drawing/2015/9/8/chartex"" 
			xmlns:cx2=""http://schemas.microsoft.com/office/drawing/2015/10/21/chartex"" 
			xmlns:cx3=""http://schemas.microsoft.com/office/drawing/2016/5/9/chartex"" 
			xmlns:cx4=""http://schemas.microsoft.com/office/drawing/2016/5/10/chartex"" 
			xmlns:cx5=""http://schemas.microsoft.com/office/drawing/2016/5/11/chartex"" 
			xmlns:m=""http://schemas.openxmlformats.org/officeDocument/2006/math"" 
			xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" 
			xmlns:o=""urn:schemas-microsoft-com:office:office"" 
			xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" 
			xmlns:v=""urn:schemas-microsoft-com:vml"" 
			xmlns:w10=""urn:schemas-microsoft-com:office:word"" 
			xmlns:w14=""http://schemas.microsoft.com/office/word/2010/wordml"" 
			xmlns:w15=""http://schemas.microsoft.com/office/word/2012/wordml"" 
			xmlns:w16se=""http://schemas.microsoft.com/office/word/2015/wordml/symex"" 
			xmlns:wne=""http://schemas.microsoft.com/office/word/2006/wordml"" 
			xmlns:wp=""http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"" 
			xmlns:wp14=""http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing"" 
			xmlns:wpc=""http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas"" 
			xmlns:wpg=""http://schemas.microsoft.com/office/word/2010/wordprocessingGroup"" 
			xmlns:wpi=""http://schemas.microsoft.com/office/word/2010/wordprocessingInk"" 
			xmlns:wps=""http://schemas.microsoft.com/office/word/2010/wordprocessingShape"" 
			mc:Ignorable=""w14 w15 w16se wp14"">
   <w:body>
      <w:p w:rsidR=""00810DBD"" w:rsidRPr=""003636BA"" w:rsidRDefault=""00601E00"" w:rsidP=""00056BC8"">
         <w:pPr>
            <w:spacing w:line=""360"" w:lineRule=""auto"" />
            <w:jc w:val=""center"" />
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:b />
               <w:color w:val=""595959"" />
               <w:sz w:val=""28"" />
               <w:szCs w:val=""28"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
         </w:pPr>
         <w:r w:rsidRPr=""003636BA"">
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:b />
               <w:color w:val=""595959"" />
               <w:sz w:val=""28"" />
               <w:szCs w:val=""28"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
            <w:t>Order Form</w:t>
         </w:r>
      </w:p>
      <w:p w:rsidR=""00BA5814"" w:rsidRPr=""003636BA"" w:rsidRDefault=""008C0362"" w:rsidP=""001952B3"">
         <w:pPr>
            <w:spacing w:line=""360"" w:lineRule=""auto"" />
            <w:jc w:val=""center"" />
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:color w:val=""595959"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
         </w:pPr>
         <w:r>
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:color w:val=""595959"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
            <w:t xml:space=""preserve"">Please check details before signing and email signed copy to</w:t>
         </w:r>
         <w:hyperlink r:id=""rId8"" w:history=""1"">
            <w:r w:rsidR=""00ED7F75"">
               <w:rPr>
                  <w:rStyle w:val=""Hyperlink"" />
                  <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                  <w:sz w:val=""20"" />
                  <w:szCs w:val=""20"" />
                  <w:lang w:val=""en-GB"" />
               </w:rPr>
               <w:t>!USER</w:t>
            </w:r>
         </w:hyperlink>
         <w:r w:rsidR=""00ED7F75"">
            <w:rPr>
               <w:rStyle w:val=""Hyperlink"" />
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
            <w:t xml:space=""preserve"">EMAIL</w:t>
         </w:r>
      </w:p>
      <w:tbl>
         <w:tblPr>
            <w:tblW w:w=""11516"" w:type=""dxa"" />
            <w:tblInd w:w=""-34"" w:type=""dxa"" />
            <w:tblBorders>
               <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideH w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideV w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
            </w:tblBorders>
            <w:tblCellMar>
               <w:top w:w=""57"" w:type=""dxa"" />
               <w:bottom w:w=""57"" w:type=""dxa"" />
            </w:tblCellMar>
            <w:tblLook w:val=""01E0"" w:firstRow=""1"" w:lastRow=""1"" w:firstColumn=""1"" w:lastColumn=""1"" w:noHBand=""0"" w:noVBand=""0"" />
         </w:tblPr>
         <w:tblGrid>
            <w:gridCol w:w=""2127"" />
            <w:gridCol w:w=""3294"" />
            <w:gridCol w:w=""709"" />
            <w:gridCol w:w=""5386"" />
         </w:tblGrid>
         <w:tr w:rsidR=""0033694A"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""275"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""0033694A"" w:rsidRPr=""004152DB"" w:rsidRDefault=""0033694A"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""004152DB"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Company Name</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9389"" w:type=""dxa"" />
                  <w:gridSpan w:val=""3"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""0033694A"" w:rsidRPr=""003636BA"" w:rsidRDefault=""0033694A"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!Company</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00AC2283"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""354"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00AC2283"" w:rsidRPr=""004152DB"" w:rsidRDefault=""00AC2283"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""004152DB"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Address</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9389"" w:type=""dxa"" />
                  <w:gridSpan w:val=""3"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00270D00"" w:rsidRPr=""003636BA"" w:rsidRDefault=""00EB392B"" w:rsidP=""00525295"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!Address1, !Address2, !Address3, !Address4, !</w:t>
                  </w:r>
                  <w:proofErr w:type=""spellStart"" />
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>PostCode</w:t>
                  </w:r>
                  <w:proofErr w:type=""spellEnd"" />
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00EB392B"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""306"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00EB392B"" w:rsidRPr=""004152DB"" w:rsidRDefault=""00EB392B"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""004152DB"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Telephone</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""3294"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00EB392B"" w:rsidRPr=""003636BA"" w:rsidRDefault=""0033694A"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!Telephone</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""709"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" w:themeFill=""background1"" w:themeFillShade=""D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00EB392B"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00EB392B"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Email</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""5386"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00EB392B"" w:rsidRPr=""003636BA"" w:rsidRDefault=""00EB392B"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!Email</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00AC2283"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""318"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00AC2283"" w:rsidRPr=""004152DB"" w:rsidRDefault=""00AC2283"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""004152DB"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Authorising Name</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9389"" w:type=""dxa"" />
                  <w:gridSpan w:val=""3"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00AC2283"" w:rsidRPr=""003636BA"" w:rsidRDefault=""0033694A"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!Name</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00AC2283"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""429"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00AC2283"" w:rsidRPr=""004152DB"" w:rsidRDefault=""00AC2283"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""004152DB"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Authorising Signature</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9389"" w:type=""dxa"" />
                  <w:gridSpan w:val=""3"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                     <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""808080"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00AC2283"" w:rsidRPr=""003636BA"" w:rsidRDefault=""001503F0"" w:rsidP=""000F5CE1"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""003636BA"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>*</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
      </w:tbl>
      <w:p w:rsidR=""00601E00"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00601E00"" w:rsidP=""00601E00"">
         <w:pPr>
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:color w:val=""595959"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
         </w:pPr>
      </w:p>
      <w:tbl>
         <w:tblPr>
            <w:tblpPr w:leftFromText=""180"" w:rightFromText=""180"" w:vertAnchor=""text"" w:horzAnchor=""margin"" w:tblpXSpec=""center"" w:tblpY=""48"" />
            <w:tblW w:w=""11449"" w:type=""dxa"" />
            <w:tblBorders>
               <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideH w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideV w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
            </w:tblBorders>
            <w:tblLook w:val=""04A0"" w:firstRow=""1"" w:lastRow=""0"" w:firstColumn=""1"" w:lastColumn=""0"" w:noHBand=""0"" w:noVBand=""1"" />
         </w:tblPr>
         <w:tblGrid>
            <w:gridCol w:w=""2127"" />
            <w:gridCol w:w=""9322"" />
         </w:tblGrid>
         <w:tr w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""452"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Send Dates</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9322"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailDate1</w:t>
                  </w:r>
               </w:p>
               <w:p w:rsidR=""007C49D6"" w:rsidRPr=""00A02982"" w:rsidRDefault=""007C49D6"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailDate2</w:t>
                  </w:r>
                  <w:bookmarkStart w:id=""0"" w:name=""_GoBack"" />
                  <w:bookmarkEnd w:id=""0"" />
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""510"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Email Details</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9322"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailDetails1</w:t>
                  </w:r>
               </w:p>
               <w:p w:rsidR=""007C49D6"" w:rsidRPr=""00A02982"" w:rsidRDefault=""007C49D6"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailDetails2</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""418"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Email Subject</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9322"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailSubject1</w:t>
                  </w:r>
               </w:p>
               <w:p w:rsidR=""007C49D6"" w:rsidRPr=""005D636F"" w:rsidRDefault=""007C49D6"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailSubject2</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
         <w:tr w:rsidR=""00A02982"" w:rsidRPr=""005D636F"" w:rsidTr=""00A02982"">
            <w:trPr>
               <w:trHeight w:val=""510"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r w:rsidRPr=""00A02982"">
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Email Costs</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9322"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00A02982"" w:rsidRDefault=""00A02982"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailCost1 +/- !EmailSetup1</w:t>
                  </w:r>
               </w:p>
               <w:p w:rsidR=""007C49D6"" w:rsidRPr=""005D636F"" w:rsidRDefault=""007C49D6"" w:rsidP=""00A02982"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!EmailCost2</w:t>
                  </w:r>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t xml:space=""preserve""> +/- !EmailSetup</w:t>
                  </w:r>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>2</w:t>
                  </w:r>
               </w:p>
            </w:tc>
         </w:tr>
      </w:tbl>
      <w:p w:rsidR=""00FE049B"" w:rsidRDefault=""00FE049B"" w:rsidP=""00A02982"">
         <w:pPr>
            <w:jc w:val=""center"" />
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:color w:val=""5F5F5F"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
         </w:pPr>
      </w:p>
      <w:tbl>
         <w:tblPr>
            <w:tblpPr w:leftFromText=""180"" w:rightFromText=""180"" w:vertAnchor=""text"" w:horzAnchor=""margin"" w:tblpXSpec=""center"" w:tblpY=""48"" />
            <w:tblW w:w=""11449"" w:type=""dxa"" />
            <w:tblBorders>
               <w:top w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:left w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:bottom w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:right w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideH w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
               <w:insideV w:val=""single"" w:sz=""4"" w:space=""0"" w:color=""auto"" />
            </w:tblBorders>
            <w:tblLook w:val=""04A0"" w:firstRow=""1"" w:lastRow=""0"" w:firstColumn=""1"" w:lastColumn=""0"" w:noHBand=""0"" w:noVBand=""1"" />
         </w:tblPr>
         <w:tblGrid>
            <w:gridCol w:w=""2127"" />
            <w:gridCol w:w=""9322"" />
         </w:tblGrid>
         <w:tr w:rsidR=""00F254A1"" w:rsidRPr=""005D636F"" w:rsidTr=""0038052E"">
            <w:trPr>
               <w:trHeight w:val=""510"" />
            </w:trPr>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""2127"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""nil"" />
                     <w:left w:val=""nil"" />
                     <w:bottom w:val=""nil"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""D9D9D9"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00F254A1"" w:rsidRPr=""00A02982"" w:rsidRDefault=""00F254A1"" w:rsidP=""0038052E"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>Total</w:t>
                  </w:r>
               </w:p>
            </w:tc>
            <w:tc>
               <w:tcPr>
                  <w:tcW w:w=""9322"" w:type=""dxa"" />
                  <w:tcBorders>
                     <w:top w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:left w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:bottom w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                     <w:right w:val=""single"" w:sz=""2"" w:space=""0"" w:color=""7F7F7F"" />
                  </w:tcBorders>
                  <w:shd w:val=""clear"" w:color=""auto"" w:fill=""auto"" />
                  <w:vAlign w:val=""center"" />
               </w:tcPr>
               <w:p w:rsidR=""00F254A1"" w:rsidRPr=""005D636F"" w:rsidRDefault=""00F254A1"" w:rsidP=""0038052E"">
                  <w:pPr>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                  </w:pPr>
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>!</w:t>
                  </w:r>
                  <w:proofErr w:type=""spellStart"" />
                  <w:r>
                     <w:rPr>
                        <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
                        <w:color w:val=""595959"" />
                        <w:sz w:val=""20"" />
                        <w:szCs w:val=""20"" />
                        <w:lang w:val=""en-GB"" />
                     </w:rPr>
                     <w:t>OrderCost</w:t>
                  </w:r>
                  <w:proofErr w:type=""spellEnd"" />
               </w:p>
            </w:tc>
         </w:tr>
      </w:tbl>
      <w:p w:rsidR=""00F254A1"" w:rsidRPr=""003636BA"" w:rsidRDefault=""00F254A1"" w:rsidP=""00A02982"">
         <w:pPr>
            <w:jc w:val=""center"" />
            <w:rPr>
               <w:rFonts w:ascii=""Calibri"" w:hAnsi=""Calibri"" w:cs=""Tahoma"" />
               <w:color w:val=""5F5F5F"" />
               <w:sz w:val=""20"" />
               <w:szCs w:val=""20"" />
               <w:lang w:val=""en-GB"" />
            </w:rPr>
         </w:pPr>
      </w:p>
      <w:sectPr w:rsidR=""00F254A1"" w:rsidRPr=""003636BA"" w:rsidSect=""0064312C"">
         <w:headerReference w:type=""default"" r:id=""rId9"" />
         <w:footerReference w:type=""default"" r:id=""rId10"" />
         <w:pgSz w:w=""12240"" w:h=""15840"" />
         <w:pgMar w:top=""1246"" w:right=""333"" w:bottom=""1079"" w:left=""426"" w:header=""0"" w:footer=""177"" w:gutter=""0"" />
         <w:cols w:space=""708"" />
         <w:docGrid w:linePitch=""360"" />
      </w:sectPr>
   </w:body>
</w:document>");

            //try
            //{
            //    using (var stream = await fil.OpenStreamForWriteAsync())
            //    {
            //        xdoc.Save(stream);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
        }
    }
}
