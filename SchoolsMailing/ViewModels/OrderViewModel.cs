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
using SchoolsMailing.Views;
using SchoolsMailing.Models;
using System.Collections.ObjectModel;
using SchoolsMailing.DAL;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Text.RegularExpressions;
using SchoolsMailing.Controls;
using System.IO.Compression;
using Windows.UI.Xaml;
using System.IO;
using Windows.Storage;

namespace SchoolsMailing.ViewModels
{
    public class OrderViewModel : PageViewModel
    {
        #region Relay commmands
        #region Navigation commands
        private RelayCommand _cmdNewData;
        public RelayCommand cmdNewData
        {
            get
            {
                if (_cmdNewData == null)
                {
                    _cmdNewData = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(DataView));
                        selectedDataOrder = new Data() { dataStart = DateTime.Now, dataEnd = DateTime.Now };
                        originalDataOrder = new Data();
                    });
                }
                return _cmdNewData;
            }
        }

        private RelayCommand _cmdNewEmail;
        public RelayCommand cmdNewEmail
        {
            get
            {
                if (_cmdNewEmail == null)
                {
                    _cmdNewEmail = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(EmailView));
                        selectedEmailOrder = new Email() { emailDate = DateTime.Now };
                        originalEmailOrder = new Email();
                    });
                }
                return _cmdNewEmail;
            }
        }

        private RelayCommand _cmdNewSchoolSend;
        public RelayCommand cmdNewSchoolSend
        {
            get
            {
                if (_cmdNewSchoolSend == null)
                {
                    _cmdNewSchoolSend = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SchoolSendView));
                        selectedSchoolSendOrder = new SchoolSend() { schoolsendStart = DateTime.Now, schoolsendEnd = DateTime.Now.AddYears(1), schoolsendPackage = 0 };
                        originalSchoolSendOrder = new SchoolSend() { schoolsendStart = DateTime.Now, schoolsendEnd = DateTime.Now.AddYears(1), schoolsendPackage = 0 };
                    });
                }
                return _cmdNewSchoolSend;
            }
        }

        private RelayCommand _cmdNewDirectMailing;
        public RelayCommand cmdNewDirectMailing
        {
            get
            {
                if (_cmdNewDirectMailing == null)
                {
                    _cmdNewDirectMailing = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(DirectMailingView));
                        selectedDirectMailingOrder = new DirectMailing() { directArtworkDate = DateTime.Now, directDate = DateTime.Now, directDataDate = DateTime.Now, directInsertDate = DateTime.Now };
                        originalDirectMailingOrder = new DirectMailing();
                    });
                }
                return _cmdNewDirectMailing;
            }
        }

        private RelayCommand _cmdNewSharedMailing;
        public RelayCommand cmdNewSharedMailing
        {
            get
            {
                if (_cmdNewSharedMailing == null)
                {
                    _cmdNewSharedMailing = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SharedMailingView));
                        selectedSharedMailingOrder = new SharedMailing() { sharedPackage = 0 };
                        originalSharedMailingOrder = new SharedMailing() { sharedPackage = 0 };
                    });
                }
                return _cmdNewSharedMailing;
            }
        }

        private RelayCommand _cmdNewPrint;
        public RelayCommand cmdNewPrint
        {
            get
            {
                if (_cmdNewPrint == null)
                {
                    _cmdNewPrint = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(PrintView));
                        selectedPrintOrder = new Print() { printDate = DateTime.Now };
                        originalPrintOrder = new Print();
                    });
                }
                return _cmdNewPrint;
            }
        }

        private RelayCommand _cmdNewSurcharge;
        public RelayCommand cmdNewSurcharge
        {
            get
            {
                if (_cmdNewSurcharge == null)
                {
                    _cmdNewSurcharge = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SurchargeView));
                        selectedSurchargeOrder = new Surcharge() { surchargeDate = DateTime.Now };
                        originalSurchargeOrder = new Surcharge();
                    });
                }
                return _cmdNewSurcharge;
            }
        }

        private RelayCommand _cmdNewSharedPack;
        public RelayCommand cmdNewSharedPack
        {
            get
            {
                if (_cmdNewSharedPack == null)
                {
                    _cmdNewSharedPack = new RelayCommand(() => {
                        NavigationService.Navigate(typeof(SharedPackageView));
                        selectedSharedPack = new SharedPack() { packArtworkDate = DateTime.Now, packDate = DateTime.Now, packDeliveryDate = DateTime.Now };
                    });
                }
                return _cmdNewSharedPack;
            }
        }
        
        private RelayCommand _cmdGoBack;
        public RelayCommand cmdGoBack
        {
            get { if (_cmdGoBack == null) { _cmdGoBack = new RelayCommand(() => { NavigationService.GoBack(); }); } return _cmdGoBack; }
        }
        #endregion

        #region Saving commands
        private RelayCommand _cmdSaveOrder;
        public RelayCommand cmdSaveOrder
        {
            get { if (_cmdSaveOrder == null) { _cmdSaveOrder = new RelayCommand(() => { SaveOrder(); }); } return _cmdSaveOrder; }
        }

        private RelayCommand _cmdSaveData;
        public RelayCommand cmdSaveData
        {
            get { if (_cmdSaveData == null) { _cmdSaveData = new RelayCommand(() => { SaveOrderPart("Data"); }); } return _cmdSaveData; }
        }

        private RelayCommand _cmdSaveEmail;
        public RelayCommand cmdSaveEmail
        {
            get { if (_cmdSaveEmail == null) { _cmdSaveEmail = new RelayCommand(() => { SaveOrderPart("Email"); }); } return _cmdSaveEmail; }
        }

        private RelayCommand _cmdSaveSchoolSend;
        public RelayCommand cmdSaveSchoolSend
        {
            get { if (_cmdSaveSchoolSend == null) { _cmdSaveSchoolSend = new RelayCommand(() => { SaveOrderPart("SchoolSend"); }); } return _cmdSaveSchoolSend; }
        }

        private RelayCommand _cmdSaveDirectMailing;
        public RelayCommand cmdSaveDirectMailing
        {
            get { if (_cmdSaveDirectMailing == null) { _cmdSaveDirectMailing = new RelayCommand(() => { SaveOrderPart("DirectMailing"); }); } return _cmdSaveDirectMailing; }
        }

        private RelayCommand _cmdSaveSharedMailing;
        public RelayCommand cmdSaveSharedMailing
        {
            get { if (_cmdSaveSharedMailing == null) { _cmdSaveSharedMailing = new RelayCommand(() => { SaveOrderPart("SharedMailing"); }); } return _cmdSaveSharedMailing; }
        }

        private RelayCommand _cmdSavePrint;
        public RelayCommand cmdSavePrint
        {
            get { if (_cmdSavePrint == null) { _cmdSavePrint = new RelayCommand(() => { SaveOrderPart("Print"); }); } return _cmdSavePrint; }
        }

        private RelayCommand _cmdSaveSurcharge;
        public RelayCommand cmdSaveSurcharge
        {
            get { if (_cmdSaveSurcharge == null) { _cmdSaveSurcharge = new RelayCommand(() => { SaveOrderPart("Surcharge"); }); } return _cmdSaveSurcharge; }
        }

        private RelayCommand _cmdSaveSharedPack;
        public RelayCommand cmdSaveSharedPack
        {
            get { if (_cmdSaveSharedPack == null) { _cmdSaveSharedPack = new RelayCommand(() => { SaveOrderPart("SharedPack"); }); } return _cmdSaveSharedPack; }
        }
        #endregion
        
        #region Duplicate commands
        private RelayCommand _cmdDuplicateData;
        public RelayCommand cmdDuplicateData
        {
            get { if (_cmdDuplicateData == null) { _cmdDuplicateData = new RelayCommand(() => { duplicateDataOrder(selectedDataOrder); }); } return _cmdDuplicateData; }
        }

        private RelayCommand _cmdDuplicateEmail;
        public RelayCommand cmdDuplicateEmail
        {
            get { if (_cmdDuplicateEmail == null) { _cmdDuplicateEmail = new RelayCommand(() => { duplicateEmailOrder(selectedEmailOrder); }); } return _cmdDuplicateEmail; }
        }

        private RelayCommand _cmdDuplicateSchoolSend;
        public RelayCommand cmdDuplicateSchoolSend
        {
            get { if (_cmdDuplicateSchoolSend == null) { _cmdDuplicateSchoolSend = new RelayCommand(() => { duplicateSchoolSendOrder(selectedSchoolSendOrder); }); } return _cmdDuplicateSchoolSend; }
        }

        private RelayCommand _cmdDuplicateDirectMailing;
        public RelayCommand cmdDuplicateDirectMailing
        {
            get { if (_cmdDuplicateDirectMailing == null) { _cmdDuplicateDirectMailing = new RelayCommand(() => { duplicateDirectMailingOrder(selectedDirectMailingOrder); }); } return _cmdDuplicateDirectMailing; }
        }

        private RelayCommand _cmdDuplicateSharedMailing;
        public RelayCommand cmdDuplicateSharedMailing
        {
            get { if (_cmdDuplicateSharedMailing == null) { _cmdDuplicateSharedMailing = new RelayCommand(() => { duplicateSharedMailingOrder(selectedSharedMailingOrder); }); } return _cmdDuplicateSharedMailing; }
        }

        private RelayCommand _cmdDuplicatePrint;
        public RelayCommand cmdDuplicatePrint
        {
            get { if (_cmdDuplicatePrint == null) { _cmdDuplicatePrint = new RelayCommand(() => { duplicatePrintOrder(selectedPrintOrder); }); } return _cmdDuplicatePrint; }
        }

        private RelayCommand _cmdDuplicateSurcharge;
        public RelayCommand cmdDuplicateSurcharge
        {
            get { if (_cmdDuplicateSurcharge == null) { _cmdDuplicateSurcharge = new RelayCommand(() => { duplicateSurchargeOrder(selectedSurchargeOrder); }); } return _cmdDuplicateSurcharge; }
        }
        #endregion

        #region Deleting commands
        private RelayCommand _cmdDeleteData;
        public RelayCommand cmdDeleteData
        {
            get { if (_cmdDeleteData == null) { _cmdDeleteData = new RelayCommand(() => { DeleteOrderPart("Data"); }); } return _cmdDeleteData; }
        }

        private RelayCommand _cmdDeleteEmail;
        public RelayCommand cmdDeleteEmail
        {
            get { if (_cmdDeleteEmail == null) { _cmdDeleteEmail = new RelayCommand(() => { DeleteOrderPart("Email"); }); } return _cmdDeleteEmail; }
        }

        private RelayCommand _cmdDeleteSchoolSend;
        public RelayCommand cmdDeleteSchoolSend
        {
            get { if (_cmdDeleteSchoolSend == null) { _cmdDeleteSchoolSend = new RelayCommand(() => { DeleteOrderPart("SchoolSend"); }); } return _cmdDeleteSchoolSend; }
        }

        private RelayCommand _cmdDeleteDirectMailing;
        public RelayCommand cmdDeleteDirectMailing
        {
            get { if (_cmdDeleteDirectMailing == null) { _cmdDeleteDirectMailing = new RelayCommand(() => { DeleteOrderPart("DirectMailing"); }); } return _cmdDeleteDirectMailing; }
        }

        private RelayCommand _cmdDeleteSharedMailing;
        public RelayCommand cmdDeleteSharedMailing
        {
            get { if (_cmdDeleteSharedMailing == null) { _cmdDeleteSharedMailing = new RelayCommand(() => { DeleteOrderPart("SharedMailing"); }); } return _cmdDeleteSharedMailing; }
        }

        private RelayCommand _cmdDeletePrint;
        public RelayCommand cmdDeletePrint
        {
            get { if (_cmdDeletePrint == null) { _cmdDeletePrint = new RelayCommand(() => { DeleteOrderPart("Print"); }); } return _cmdDeletePrint; }
        }

        private RelayCommand _cmdDeleteSurcharge;
        public RelayCommand cmdDeleteSurcharge
        {
            get { if (_cmdDeleteSurcharge == null) { _cmdDeleteSurcharge = new RelayCommand(() => { DeleteOrderPart("Surcharge"); }); } return _cmdDeleteSurcharge; }
        }
        #endregion
        
        #region Validate commands
        private RelayCommand _cmdValidateOrder;
        public RelayCommand cmdValidateOrder
        {
            get { if (_cmdValidateOrder == null) { _cmdValidateOrder = new RelayCommand(() => { isValidOrder(selectedOrder); }); } return _cmdValidateOrder; }
        }

        private RelayCommand _cmdValidateData;
        public RelayCommand cmdValidateData
        {
            get { if (_cmdValidateData == null) { _cmdValidateData = new RelayCommand(() => { isValidDataOrder(selectedDataOrder); }); } return _cmdValidateData; }
        }

        private RelayCommand _cmdValidateEmail;
        public RelayCommand cmdValidateEmail
        {
            get { if (_cmdValidateEmail == null) { _cmdValidateEmail = new RelayCommand(() => { isValidEmailOrder(selectedEmailOrder); }); } return _cmdValidateEmail; }
        }

        private RelayCommand _cmdValidateDirectMailing;
        public RelayCommand cmdValidateDirectMailing
        {
            get { if (_cmdValidateDirectMailing == null) { _cmdValidateDirectMailing = new RelayCommand(() => { isValidDirectMailingOrder(selectedDirectMailingOrder); }); } return _cmdValidateDirectMailing; }
        }

        private RelayCommand _cmdValidateSchoolSend;
        public RelayCommand cmdValidateSchoolSend
        {
            get { if (_cmdValidateSchoolSend == null) { _cmdValidateSchoolSend = new RelayCommand(() => { InvalidateSchoolSendOrder(selectedSchoolSendOrder); }); } return _cmdValidateSchoolSend; }
        }

        private RelayCommand _cmdValidateSharedMailing;
        public RelayCommand cmdValidateSharedMailing
        {
            get { if (_cmdValidateSharedMailing == null) { _cmdValidateSharedMailing = new RelayCommand(() => { isValidSharedMailingOrder(selectedSharedMailingOrder); }); } return _cmdValidateSharedMailing; }
        }

        private RelayCommand _cmdValidatePrint;
        public RelayCommand cmdValidatePrint
        {
            get { if (_cmdValidatePrint == null) { _cmdValidatePrint = new RelayCommand(() => { isValidPrintOrder(selectedPrintOrder); }); } return _cmdValidatePrint; }
        }

        private RelayCommand _cmdValidateSurcharge;
        public RelayCommand cmdValidateSurcharge
        {
            get { if (_cmdValidateSurcharge == null) { _cmdValidateSurcharge = new RelayCommand(() => { isValidSurchargeOrder(selectedSurchargeOrder); }); } return _cmdValidateSurcharge; }
        }

        private RelayCommand _cmdValidateSharedPack;
        public RelayCommand cmdValidateSharedPack
        {
            get { if (_cmdValidateSharedPack == null) { _cmdValidateSharedPack = new RelayCommand(() => { isValidSharedPackOrder(selectedSharedPack); }); } return _cmdValidateSharedPack; }
        }
        #endregion

        #region Data changed commands
        private RelayCommand _cmdCompanyChanged;
        public RelayCommand cmdCompanyChanged
        {
            get { if (_cmdCompanyChanged == null) { _cmdCompanyChanged = new RelayCommand(() => {
                        if (selectedCompany != null)
                        {
                            if (selectedCompany.companyInvoiceName != null) { selectedOrder.companyName = selectedCompany.companyInvoiceName.ToString(); }
                            if (selectedCompany.companyInvoiceAddress1 != null) { selectedOrder.companyAddress1 = selectedCompany.companyInvoiceAddress1.ToString(); }
                            if (selectedCompany.companyInvoiceAddress2 != null) { selectedOrder.companyAddress2 = selectedCompany.companyInvoiceAddress2.ToString(); }
                            if (selectedCompany.companyInvoiceCity != null) { selectedOrder.companyCity = selectedCompany.companyInvoiceCity.ToString(); }
                            if (selectedCompany.companyInvoiceCounty != null) { selectedOrder.companyCounty = selectedCompany.companyInvoiceCounty.ToString(); }
                            if (selectedCompany.companyInvoicePostCode != null) { selectedOrder.companyPostCode = selectedCompany.companyInvoicePostCode.ToString(); }
                            selectedOrder.companyProspects = (selectedCompany.companyProspects != null) ? selectedCompany.companyProspects : null;

                            contacts = DataAccessLayer.getContactsByCompanyID(selectedCompany.ID);
                        } }); } return _cmdCompanyChanged; }
        }

        private RelayCommand _cmdContactChanged;
        public RelayCommand cmdContactChanged
        {
            get
            {
                if (_cmdContactChanged == null)
                {
                    _cmdContactChanged = new RelayCommand(() =>
                    {
                        if (selectedContact != null)
                        {
                            if (selectedContact.contactTitle != null) { selectedOrder.contactTitle = selectedContact.contactTitle.ToString(); }
                            if (selectedContact.contactForename != null) { selectedOrder.contactForename = selectedContact.contactForename.ToString(); }
                            if (selectedContact.contactSurname != null) { selectedOrder.contactSurname = selectedContact.contactSurname.ToString(); }
                            if (selectedContact.contactTelephone != null) { selectedOrder.contactTelephone = selectedContact.contactTelephone.ToString(); }
                            if (selectedContact.contactEmail != null) { selectedOrder.contactEmail = selectedContact.contactEmail.ToString(); }
                        }
                    });
                }
                return _cmdContactChanged;
            }
        }
        
        private RelayCommand _cmdDirectMailingDateChanged;
        public RelayCommand cmdDirectMailingDateChanged
        {
            get
            {
                if (_cmdDirectMailingDateChanged == null)
                {
                    _cmdDirectMailingDateChanged = new RelayCommand(() => {
                        if (_cmdDirectMailingDateChanged != null)
                        {
                            selectedDirectMailingOrder.directArtworkDate = selectedDirectMailingOrder.directDate.AddDays(-14);
                            selectedDirectMailingOrder.directInsertDate = selectedDirectMailingOrder.directDate.AddDays(-5);
                            RaisePropertyChanged("selectedDirectMailingOrder");
                        }
                    });
                }
                return _cmdDirectMailingDateChanged;
            }
        }

        private RelayCommand _cmdSchoolSendDateChanged;
        public RelayCommand cmdSchoolSendDateChanged
        {
            get { if (_cmdSchoolSendDateChanged == null) { _cmdSchoolSendDateChanged = new RelayCommand(() => { selectedSchoolSendOrder.schoolsendEnd = selectedSchoolSendOrder.schoolsendStart.AddYears(1); RaisePropertyChanged("selectedSchoolSendOrder"); }); } return _cmdSchoolSendDateChanged; }
        }

        private RelayCommand _cmdSchoolSendPackageChanged;
        public RelayCommand cmdSchoolSendPackageChanged
        {
            get
            {
                if (_cmdSchoolSendPackageChanged == null)
                {
                    _cmdSchoolSendPackageChanged = new RelayCommand(() => {
                        if (selectedSchoolSendPack != null)
                        {
                            selectedSchoolSendOrder.schoolsendPackage = selectedSchoolSendPack.ID;
                            selectedSchoolSendOrder.schoolsendCredits = selectedSchoolSendPack.packCredits;
                            selectedSchoolSendOrder.schoolsendCost = selectedSchoolSendPack.packCost;
                            RaisePropertyChanged("selectedSchoolSendOrder");
                        }
                    });
                }
                return _cmdSchoolSendPackageChanged;
            }
        }
        
        private RelayCommand _cmdSharedMailingPackageChanged;
        public RelayCommand cmdSharedMailingPackageChanged
        {
            get
            {
                if (_cmdSharedMailingPackageChanged == null)
                {
                    _cmdSharedMailingPackageChanged = new RelayCommand(() => {
                        if (selectedSharedPack != null)
                        {
                            selectedSharedMailingOrder.sharedArtworkDate = selectedSharedPack.packArtworkDate;
                            selectedSharedMailingOrder.sharedDate = selectedSharedPack.packDate;
                            selectedSharedMailingOrder.sharedDeliveryDate = selectedSharedPack.packDeliveryDate;
                            selectedSharedMailingOrder.sharedMailingTo = selectedSharedPack.packTo;
                            selectedSharedMailingOrder.sharedCost = selectedSharedPack.packCost;
                            RaisePropertyChanged("selectedSharedMailingOrder");
                        }
                    });
                }
                return _cmdSharedMailingPackageChanged;
            }
        }
        
        private RelayCommand _cmdSumEmailTotal;
        public RelayCommand cmdSumEmailTotal
        {
            get { if (_cmdSumEmailTotal == null) { _cmdSumEmailTotal = new RelayCommand(() => { SumEmailOrder(); }); } return _cmdSumEmailTotal; }
        }

        private RelayCommand _cmdSumDirectMailingTotal;
        public RelayCommand cmdSumDirectMailingTotal
        {
            get { if (_cmdSumDirectMailingTotal == null) { _cmdSumDirectMailingTotal = new RelayCommand(() => { SumDirectMailingOrder(); }); } return _cmdSumDirectMailingTotal; }
        }
        #endregion

        private RelayCommand _createOrderForm;
        public RelayCommand createOrderForm
        {
            get { if (_createOrderForm == null) { _createOrderForm = new RelayCommand(() => { if (selectedOrder != null) { DocumentAccessLayer.CreateOrderForm(selectedOrder); } }); } return _createOrderForm; }
        }
        #endregion

        #region Variables
        #region Validation
        #region Order Validation
        private bool _isInvalidOrderCompany = false;
        public bool isInvalidOrderCompany
        {
            get { return _isInvalidOrderCompany; }
            set { if (_isInvalidOrderCompany != value) { _isInvalidOrderCompany = !_isInvalidOrderCompany; RaisePropertyChanged("isInvalidOrderCompany"); } }
        }

        private bool _isInvalidOrderContact = false;
        public bool isInvalidOrderContact
        {
            get { return _isInvalidOrderContact; }
            set { if (_isInvalidOrderContact != value) { _isInvalidOrderContact = !_isInvalidOrderContact; RaisePropertyChanged("isInvalidOrderContact"); } }
        }

        private bool _isInvalidOrderCode = false;
        public bool isInvalidOrderCode
        {
            get { return _isInvalidOrderCode; }
            set { if (_isInvalidOrderCode != value) { _isInvalidOrderCode = !_isInvalidOrderCode; RaisePropertyChanged("isInvalidOrderCode"); } }
        }

        private bool _isInvalidOrderDate = false;
        public bool isInvalidOrderDate
        {
            get { return _isInvalidOrderDate; }
            set { if (_isInvalidOrderDate != value) { _isInvalidOrderDate = !_isInvalidOrderDate; RaisePropertyChanged("isInvalidOrderDate"); } }
        }
        #endregion

        #region Data Validation
        private bool _isInvalidDataStart = false;
        public bool isInvalidDataStart
        {
            get { return _isInvalidDataStart; }
            set { if (_isInvalidDataStart != value) { _isInvalidDataStart = !_isInvalidDataStart; RaisePropertyChanged("isInvalidDataStart"); } }
        }

        private bool _isInvalidDataEnd = false;
        public bool isInvalidDataEnd
        {
            get { return _isInvalidDataEnd; }
            set { if (_isInvalidDataEnd != value) { _isInvalidDataEnd = !_isInvalidDataEnd; RaisePropertyChanged("isInvalidDataEnd"); } }
        }

        private bool _isInvalidDataDetails = false;
        public bool isInvalidDataDetails
        {
            get { return _isInvalidDataDetails; }
            set { if (_isInvalidDataDetails != value) { _isInvalidDataDetails = !_isInvalidDataDetails; RaisePropertyChanged("isInvalidDataDetails"); } }
        }

        private bool _isInvalidDataCost = false;
        public bool isInvalidDataCost
        {
            get { return _isInvalidDataCost; }
            set { if (_isInvalidDataCost != value) { _isInvalidDataCost = !_isInvalidDataCost; RaisePropertyChanged("isInvalidDataCost"); } }
        }
        #endregion
        
        #region Email validation
        private bool _isInvalidEmailDate = false;
        public bool isInvalidEmailDate
        {
            get { return _isInvalidEmailDate; }
            set { if (_isInvalidEmailDate != value) { _isInvalidEmailDate = !_isInvalidEmailDate; RaisePropertyChanged("isInvalidEmailDate"); } }
        }

        private bool _isInvalidEmailDetails = false;
        public bool isInvalidEmailDetails
        {
            get { return _isInvalidEmailDetails; }
            set { if (_isInvalidEmailDetails != value) { _isInvalidEmailDetails = !_isInvalidEmailDetails; RaisePropertyChanged("isInvalidEmailDetails"); } }
        }

        private bool _isInvalidEmailAdminCost = false;
        public bool isInvalidEmailAdminCost
        {
            get { return _isInvalidEmailAdminCost; }
            set { if (_isInvalidEmailAdminCost != value) { _isInvalidEmailAdminCost = !_isInvalidEmailAdminCost; RaisePropertyChanged("isInvalidEmailAdminCost"); } }
        }

        private bool _isInvalidEmailDirectCost = false;
        public bool isInvalidEmailDirectCost
        {
            get { return _isInvalidEmailDirectCost; }
            set { if (_isInvalidEmailDirectCost != value) { _isInvalidEmailDirectCost = !_isInvalidEmailDirectCost; RaisePropertyChanged("isInvalidEmailDirectCost"); } }
        }

        private bool _isInvalidEmailCost = false;
        public bool isInvalidEmailCost
        {
            get { return _isInvalidEmailCost; }
            set { if (_isInvalidEmailCost != value) { _isInvalidEmailCost = !_isInvalidEmailCost; RaisePropertyChanged("isInvalidEmailCost"); } }
        }
        #endregion

        #region SchoolSend validation
        private bool _isInvalidSchoolSendStart = false;
        public bool isInvalidSchoolSendStart
        {
            get { return _isInvalidSchoolSendStart; }
            set { if (_isInvalidSchoolSendStart != value) { _isInvalidSchoolSendStart = !_isInvalidSchoolSendStart; RaisePropertyChanged("isInvalidSchoolSendStart"); } }
        }

        private bool _isInvalidSchoolSendEnd = false;
        public bool isInvalidSchoolSendEnd
        {
            get { return _isInvalidSchoolSendEnd; }
            set { if (_isInvalidSchoolSendEnd != value) { _isInvalidSchoolSendEnd = !_isInvalidSchoolSendEnd; RaisePropertyChanged("isInvalidSchoolSendEnd"); } }
        }

        private bool _isInvalidSchoolSendPackage = false;
        public bool isInvalidSchoolSendPackage
        {
            get { return _isInvalidSchoolSendPackage; }
            set { if (_isInvalidSchoolSendPackage != value) { _isInvalidSchoolSendPackage = !_isInvalidSchoolSendPackage; RaisePropertyChanged("isInvalidSchoolSendPackage"); } }
        }

        private bool _isInvalidSchoolSendCost = false;
        public bool isInvalidSchoolSendCost
        {
            get { return _isInvalidSchoolSendCost; }
            set { if (_isInvalidSchoolSendCost != value) { _isInvalidSchoolSendCost = !_isInvalidSchoolSendCost; RaisePropertyChanged("isInvalidSchoolSendCost"); } }
        }
        #endregion

        #region Direct Mailing validation
        private bool _isInvalidDirectMailingFulfilmentCost = false;
        public bool isInvalidDirectMailingFulfilmentCost
        {
            get { return _isInvalidDirectMailingFulfilmentCost; }
            set { if (_isInvalidDirectMailingFulfilmentCost != value) { _isInvalidDirectMailingFulfilmentCost = !_isInvalidDirectMailingFulfilmentCost; RaisePropertyChanged("isInvalidDirectMailingFulfilmentCost"); } }
        }

        private bool _isInvalidDirectMailingPrintCost = false;
        public bool isInvalidDirectMailingPrintCost
        {
            get { return _isInvalidDirectMailingPrintCost; }
            set { if (_isInvalidDirectMailingPrintCost != value) { _isInvalidDirectMailingPrintCost = !_isInvalidDirectMailingPrintCost; RaisePropertyChanged("isInvalidDirectMailingPrintCost"); } }
        }

        private bool _isInvalidDirectMailingPostageCost = false;
        public bool isInvalidDirectMailingPostageCost
        {
            get { return _isInvalidDirectMailingPostageCost; }
            set { if (_isInvalidDirectMailingPostageCost != value) { _isInvalidDirectMailingPostageCost = !_isInvalidDirectMailingPostageCost; RaisePropertyChanged("isInvalidDirectMailingPostageCost"); } }
        }

        private bool _isInvalidDirectMailingDate = false;
        public bool isInvalidDirectMailingDate
        {
            get { return _isInvalidDirectMailingDate; }
            set { if (_isInvalidDirectMailingDate != value) { _isInvalidDirectMailingDate = !_isInvalidDirectMailingDate; RaisePropertyChanged("isInvalidDirectMailingDate"); } }
        }

        private bool _isInvalidDirectMailingDetails = false;
        public bool isInvalidDirectMailingDetails
        {
            get { return _isInvalidDirectMailingDetails; }
            set { if (_isInvalidDirectMailingDetails != value) { _isInvalidDirectMailingDetails = !_isInvalidDirectMailingDetails; RaisePropertyChanged("isInvalidDirectMailingDetails"); } }
        }

        private bool _isInvalidDirectMailingDeliveryCode = false;
        public bool isInvalidDirectMailingDeliveryCode
        {
            get { return _isInvalidDirectMailingDeliveryCode; }
            set { if (_isInvalidDirectMailingDeliveryCode != value) { _isInvalidDirectMailingDeliveryCode = !_isInvalidDirectMailingDeliveryCode; RaisePropertyChanged("isInvalidDirectMailingDeliveryCode"); } }
        }

        private bool _isInvalidDirectMailingArtworkDate = false;
        public bool isInvalidDirectMailingArtworkDate
        {
            get { return _isInvalidDirectMailingArtworkDate; }
            set { if (_isInvalidDirectMailingArtworkDate != value) { _isInvalidDirectMailingArtworkDate = !_isInvalidDirectMailingArtworkDate; RaisePropertyChanged("isInvalidDirectMailingArtworkDate"); } }
        }

        private bool _isInvalidDirectMailingDataDate = false;
        public bool isInvalidDirectMailingDataDate
        {
            get { return _isInvalidDirectMailingDataDate; }
            set { if (_isInvalidDirectMailingDataDate != value) { _isInvalidDirectMailingDataDate = !_isInvalidDirectMailingDataDate; RaisePropertyChanged("isInvalidDirectMailingDataDate"); } }
        }

        private bool _isInvalidDirectMailingInsertDate = false;
        public bool isInvalidDirectMailingInsertDate
        {
            get { return _isInvalidDirectMailingInsertDate; }
            set { if (_isInvalidDirectMailingInsertDate != value) { _isInvalidDirectMailingInsertDate = !_isInvalidDirectMailingInsertDate; RaisePropertyChanged("isInvalidDirectMailingInsertDate"); } }
        }
        #endregion
        
        #region Shared Mailing validation
        private bool _isInvalidSharedPackage = false;
        public bool isInvalidSharedPackage
        {
            get { return _isInvalidSharedPackage; }
            set { if (_isInvalidSharedPackage != value) { _isInvalidSharedPackage = !_isInvalidSharedPackage; RaisePropertyChanged("isInvalidSharedPackage"); } }
        }

        private bool _isInvalidSharedDate = false;
        public bool isInvalidSharedDate
        {
            get { return _isInvalidSharedDate; }
            set { if (_isInvalidSharedDate != value) { _isInvalidSharedDate = !_isInvalidSharedDate; RaisePropertyChanged("isInvalidSharedDate"); } }
        }

        private bool _isInvalidSharedDeliveryDate = false;
        public bool isInvalidSharedDeliveryDate
        {
            get { return _isInvalidSharedDeliveryDate; }
            set { if (_isInvalidSharedDeliveryDate != value) { _isInvalidSharedDeliveryDate = !_isInvalidSharedDeliveryDate; RaisePropertyChanged("isInvalidSharedDeliveryDate"); } }
        }

        private bool _isInvalidSharedArtworkDate = false;
        public bool isInvalidSharedArtworkDate
        {
            get { return _isInvalidSharedArtworkDate; }
            set { if (_isInvalidSharedArtworkDate != value) { _isInvalidSharedArtworkDate = !_isInvalidSharedArtworkDate; RaisePropertyChanged("isInvalidSharedArtworkDate"); } }
        }

        private bool _isInvalidSharedCost = false;
        public bool isInvalidSharedCost
        {
            get { return _isInvalidSharedCost; }
            set { if (_isInvalidSharedCost != value) { _isInvalidSharedCost = !_isInvalidSharedCost; RaisePropertyChanged("isInvalidSharedCost"); } }
        }
        #endregion

        #region Print validation
        private bool _isInvalidPrintCharge = false;
        public bool isInvalidPrintCharge
        {
            get { return _isInvalidPrintCharge; }
            set { if (_isInvalidPrintCharge != value) { _isInvalidPrintCharge = !_isInvalidPrintCharge; RaisePropertyChanged("isInvalidPrintCharge"); } }
        }

        private bool _isInvalidPrintCost = false;
        public bool isInvalidPrintCost
        {
            get { return _isInvalidPrintCost; }
            set { if (_isInvalidPrintCost != value) { _isInvalidPrintCost = !_isInvalidPrintCost; RaisePropertyChanged("isInvalidPrintCost"); } }
        }

        private bool _isInvalidPrintDetails = false;
        public bool isInvalidPrintDetails
        {
            get { return _isInvalidPrintDetails; }
            set { if (_isInvalidPrintDetails != value) { _isInvalidPrintDetails = !_isInvalidPrintDetails; RaisePropertyChanged("isInvalidPrintDetails"); } }
        }

        private bool _isInvalidPrintPrinter = false;
        public bool isInvalidPrintPrinter
        {
            get { return _isInvalidPrintPrinter; }
            set { if (_isInvalidPrintPrinter != value) { _isInvalidPrintPrinter = !_isInvalidPrintPrinter; RaisePropertyChanged("isInvalidPrintPrinter"); } }
        }

        private bool _isInvalidPrintDate = false;
        public bool isInvalidPrintDate
        {
            get { return _isInvalidPrintDate; }
            set { if (_isInvalidPrintDate != value) { _isInvalidPrintDate = !_isInvalidPrintDate; RaisePropertyChanged("isInvalidPrintDate"); } }
        }
        #endregion
        
        #region Surcharge validation
        private bool _isInvalidSurchargeDate = false;
        public bool isInvalidSurchargeDate
        {
            get { return _isInvalidSurchargeDate; }
            set { if (_isInvalidSurchargeDate != value) { _isInvalidSurchargeDate = !_isInvalidSurchargeDate; RaisePropertyChanged("isInvalidSurchargeDate"); } }
        }

        private bool _isInvalidSurchargeDetails = false;
        public bool isInvalidSurchargeDetails
        {
            get { return _isInvalidSurchargeDetails; }
            set { if (_isInvalidSurchargeDetails != value) { _isInvalidSurchargeDetails = !_isInvalidSurchargeDetails; RaisePropertyChanged("isInvalidSurchargeDetails"); } }
        }

        private bool _isInvalidSurchargeCost = false;
        public bool isInvalidSurchargeCost
        {
            get { return _isInvalidSurchargeCost; }
            set { if (_isInvalidSurchargeCost != value) { _isInvalidSurchargeCost = !_isInvalidSurchargeCost; RaisePropertyChanged("isInvalidSurchargeCost"); } }
        }
        #endregion
        #endregion

        #region Bindings
        private string _pageTitle;
        public string pageTitle
        {
            get { return _pageTitle; }
            set { if (_pageTitle != value) { _pageTitle = value; RaisePropertyChanged("pageTitle"); } }
        }

        private int _pivotIndex;
        public int pivotIndex
        {
            get { return _pivotIndex; }
            set { if (_pivotIndex != value) { _pivotIndex = value; RaisePropertyChanged("pivotIndex"); } }
        }

        #region Order bindings
        private string _orderCode;
        public string orderCode
        {
            get { return _orderCode; }
            set { if (_orderCode != value) { _orderCode = value; RaisePropertyChanged("orderCode"); } }
        }

        private Orders _selectedOrder;
        public Orders selectedOrder
        {
            get { return _selectedOrder; }
            set { if (_selectedOrder != value) { _selectedOrder = value; RaisePropertyChanged("selectedOrder"); } }
        }

        private double _dataCosts = 0;
        public double dataCosts
        {
            get { return _dataCosts; }
            set { if (_dataCosts != value) { _dataCosts = value; RaisePropertyChanged("dataCosts"); } }
        }

        private double _emailCosts = 0;
        public double emailCosts
        {
            get { return _emailCosts; }
            set { if (_emailCosts != value) { _emailCosts = value; RaisePropertyChanged("emailCosts"); } }
        }

        private double _schoolSendCosts = 0;
        public double schoolSendCosts
        {
            get { return _schoolSendCosts; }
            set { if (_schoolSendCosts != value) { _schoolSendCosts = value; RaisePropertyChanged("schoolSendCosts"); } }
        }

        private double _directMailingCosts = 0;
        public double directMailingCosts
        {
            get { return _directMailingCosts; }
            set { if (_directMailingCosts != value) { _directMailingCosts = value; RaisePropertyChanged("directMailingCosts"); } }
        }

        private double _sharedMailingCosts = 0;
        public double sharedMailingCosts
        {
            get { return _sharedMailingCosts; }
            set { if (_sharedMailingCosts != value) { _sharedMailingCosts = value; RaisePropertyChanged("sharedMailingCosts"); } }
        }

        private double _printCosts = 0;
        public double printCosts
        {
            get { return _printCosts; }
            set { if (_printCosts != value) { _printCosts = value; RaisePropertyChanged("printCosts"); } }
        }

        private double _surchargeCosts = 0;
        public double surchargeCosts
        {
            get { return _surchargeCosts; }
            set { if (_surchargeCosts != value) { _surchargeCosts = value; RaisePropertyChanged("surchargeCosts"); } }
        }
        #endregion

        #region Company
        public List<Company> companies
        {
            get { return DataAccessLayer.getCompanies(); }
        }

        private Company _selectedCompany = new Company();
        public Company selectedCompany
        {
            get { return _selectedCompany; }
            set { if (_selectedCompany != value) { _selectedCompany = value; RaisePropertyChanged("selectedCompany"); } }
        }
        #endregion

        #region Contact
        private Contact _selectedContact = new Contact();
        public Contact selectedContact
        {
            get { return _selectedContact; }
            set { if (_selectedContact != value) { _selectedContact = value; RaisePropertyChanged("selectedContact"); } }
        }

        private List<Contact> _contacts;
        public List<Contact> contacts
        {
            get
            {
                if (selectedOrder.companyID > 0)
                {
                    _contacts = DataAccessLayer.getContactsByCompanyID(selectedCompany.ID);
                    return _contacts;
                }
                else
                {
                    _contacts = DataAccessLayer.getContacts();
                    return _contacts;
                }
            }
            set { if (_contacts != value) { _contacts = value; RaisePropertyChanged("contacts"); } }
        }
        #endregion

        #region Data bindings
        private ObservableCollection<Data> _dataOrders;
        public ObservableCollection<Data> dataOrders
        {
            get { return _dataOrders; }
            set { if (_dataOrders != value) { _dataOrders = value; RaisePropertyChanged("dataOrders"); } }
        }

        private ObservableCollection<Data> _deletedDataOrders;
        public ObservableCollection<Data> deletedDataOrders
        {
            get { return _deletedDataOrders; }
            set { if (_deletedDataOrders != value) { _deletedDataOrders = value; RaisePropertyChanged("deletedDataOrders"); } }
        }

        private Data _selectedDataOrder;
        public Data selectedDataOrder
        {
            get { return _selectedDataOrder; }
            set { if (_selectedDataOrder != value) { _selectedDataOrder = value; RaisePropertyChanged("selectedDataOrder"); } }
        }

        private Data originalDataOrder;
        #endregion
        
        #region Email bindings
        private ObservableCollection<Email> _emailOrders;
        public ObservableCollection<Email> emailOrders
        {
            get { return _emailOrders; }
            set { if (_emailOrders != value) { _emailOrders = value; RaisePropertyChanged("emailOrders"); } }
        }

        private ObservableCollection<Email> _deletedEmailOrders;
        public ObservableCollection<Email> deletedEmailOrders
        {
            get { return _deletedEmailOrders; }
            set { if (_deletedEmailOrders != value) { _deletedEmailOrders = value; RaisePropertyChanged("deletedEmailOrders"); } }
        }

        private Email _selectedEmailOrder;
        public Email selectedEmailOrder
        {
            get { return _selectedEmailOrder; }
            set { if (_selectedEmailOrder != value) { _selectedEmailOrder = value; RaisePropertyChanged("selectedEmailOrder"); } }
        }

        private Email originalEmailOrder;
        
        public DateTime duplicateEmailDate;
        #endregion

        #region SchoolSend bindings
        private ObservableCollection<SchoolSend> _schoolSendOrders;
        public ObservableCollection<SchoolSend> schoolSendOrders
        {
            get { return _schoolSendOrders; }
            set { if (_schoolSendOrders != value) { _schoolSendOrders = value; RaisePropertyChanged("schoolSendOrders"); } }
        }

        private ObservableCollection<SchoolSend> _deletedSchoolSendOrders;
        public ObservableCollection<SchoolSend> deletedSchoolSendOrders
        {
            get { return _deletedSchoolSendOrders; }
            set { if (_deletedSchoolSendOrders != value) { _deletedSchoolSendOrders = value; RaisePropertyChanged("deletedSchoolSendOrders"); } }
        }

        private ObservableCollection<SchoolSendPack> _schoolsendPacks;
        public ObservableCollection<SchoolSendPack> schoolsendPacks
        {
            get { return _schoolsendPacks; }
            set { if (_schoolsendPacks != value) { _schoolsendPacks = value; RaisePropertyChanged("schoolsendPacks"); } }
        }

        private SchoolSendPack _selectedSchoolSendPack = new SchoolSendPack();
        public SchoolSendPack selectedSchoolSendPack
        {
            get { return _selectedSchoolSendPack; }
            set { if (_selectedSchoolSendPack != value) { _selectedSchoolSendPack = value; RaisePropertyChanged("selectedSchoolSendPack"); } }
        }

        private SchoolSend _selectedSchoolSendOrder;
        public SchoolSend selectedSchoolSendOrder
        {
            get { return _selectedSchoolSendOrder; }
            set { if (_selectedSchoolSendOrder != value) { _selectedSchoolSendOrder = value; RaisePropertyChanged("selectedSchoolSendOrder"); } }
        }

        private SchoolSend originalSchoolSendOrder;
        #endregion

        #region Direct Mailing bindings
        private ObservableCollection<DirectMailing> _directMailingOrders;
        public ObservableCollection<DirectMailing> directMailingOrders
        {
            get { return _directMailingOrders; }
            set { if (_directMailingOrders != value) { _directMailingOrders = value; RaisePropertyChanged("directMailingOrders"); } }
        }

        private ObservableCollection<DirectMailing> _deletedDirectMailingOrders;
        public ObservableCollection<DirectMailing> deletedDirectMailingOrders
        {
            get { return _deletedDirectMailingOrders; }
            set { if (_deletedDirectMailingOrders != value) { _deletedDirectMailingOrders = value; RaisePropertyChanged("deletedDirectMailingOrders"); } }
        }

        private DirectMailing _selectedDirectMailingOrder;
        public DirectMailing selectedDirectMailingOrder
        {
            get { return _selectedDirectMailingOrder; }
            set { if (_selectedDirectMailingOrder != value) { _selectedDirectMailingOrder = value; RaisePropertyChanged("selectedDirectMailingOrder"); } }
        }

        private DirectMailing originalDirectMailingOrder;
        #endregion
        
        #region Shared Mailing bindings
        private ObservableCollection<SharedMailing> _sharedMailingOrders;
        public ObservableCollection<SharedMailing> sharedMailingOrders
        {
            get { return _sharedMailingOrders; }
            set { if (_sharedMailingOrders != value) { _sharedMailingOrders = value; RaisePropertyChanged("sharedMailingOrders"); } }
        }

        private ObservableCollection<SharedMailing> _deletedSharedMailingOrders;
        public ObservableCollection<SharedMailing> deletedSharedMailingOrders
        {
            get { return _deletedSharedMailingOrders; }
            set { if (_deletedSharedMailingOrders != value) { _deletedSharedMailingOrders = value; RaisePropertyChanged("deletedSharedMailingOrders"); } }
        }

        private SharedMailing _selectedSharedMailingOrder;
        public SharedMailing selectedSharedMailingOrder
        {
            get { return _selectedSharedMailingOrder; }
            set { if (_selectedSharedMailingOrder != value) { _selectedSharedMailingOrder = value; RaisePropertyChanged("selectedSharedMailingOrder"); } }
        }

        private SharedMailing originalSharedMailingOrder;
        #endregion
        
        #region Print bindings
        private ObservableCollection<Print> _printOrders;
        public ObservableCollection<Print> printOrders
        {
            get { return _printOrders; }
            set { if (_printOrders != value) { _printOrders = value; RaisePropertyChanged("printOrders"); } }
        }

        private ObservableCollection<Print> _deletedPrintOrders;
        public ObservableCollection<Print> deletedPrintOrders
        {
            get { return _deletedPrintOrders; }
            set { if (_deletedPrintOrders != value) { _deletedPrintOrders = value; RaisePropertyChanged("deletedPrintOrders"); } }
        }

        private Print _selectedPrintOrder;
        public Print selectedPrintOrder
        {
            get { return _selectedPrintOrder; }
            set { if (_selectedPrintOrder != value) { _selectedPrintOrder = value; RaisePropertyChanged("selectedPrintOrder"); } }
        }

        private Print originalPrintOrder;
        #endregion

        #region Surcharge bindings
        private ObservableCollection<Surcharge> _surchargeOrders;
        public ObservableCollection<Surcharge> surchargeOrders
        {
            get { return _surchargeOrders; }
            set { if (_surchargeOrders != value) { _surchargeOrders = value; RaisePropertyChanged("surchargeOrders"); } }
        }

        private ObservableCollection<Surcharge> _deletedSurchargeOrders;
        public ObservableCollection<Surcharge> deletedSurchargeOrders
        {
            get { return _deletedSurchargeOrders; }
            set { if (_deletedSurchargeOrders != value) { _deletedSurchargeOrders = value; RaisePropertyChanged("deletedSurchargeOrders"); } }
        }

        private Surcharge _selectedSurchargeOrder;
        public Surcharge selectedSurchargeOrder
        {
            get { return _selectedSurchargeOrder; }
            set { if (_selectedSurchargeOrder != value) { _selectedSurchargeOrder = value; RaisePropertyChanged("selectedSurchargeOrder"); } }
        }

        private Surcharge originalSurchargeOrder;
        #endregion
        #endregion
        #endregion

        public OrderViewModel(IMessenger messenger, NavigationService navigationService) : base(messenger, navigationService)
        {
            MessengerInstance.Register<NotificationMessage<Int64>>(this, SetUp); // register company parameter
        }

        public void SetUp(NotificationMessage<Int64> obj)
        {
            //Get order
            if (obj.Notification == "OrderViewModel-OrderID")
            {
                //Get packs
                schoolsendPacks = DataAccessLayer.GetAllSchoolSendPacks();
                sharedPacks = DataAccessLayer.GetAllSharedPacks();

                //Set selected pivot menu item
                pivotIndex = 0;

                //Get existing order parts
                selectedOrder = DataAccessLayer.getOrderByID(obj.Content);
                emailOrders = new ObservableCollection<Email>(DataAccessLayer.GetAllEmailsByOrderID(obj.Content));
                dataOrders = new ObservableCollection<Data>(DataAccessLayer.GetAllDataByOrderID(obj.Content));
                schoolSendOrders = new ObservableCollection<SchoolSend>(DataAccessLayer.GetAllSchoolSendsByOrderID(obj.Content));
                sharedMailingOrders = new ObservableCollection<SharedMailing>(DataAccessLayer.GetAllSharedMailingsByOrderID(obj.Content));
                directMailingOrders = new ObservableCollection<DirectMailing>(DataAccessLayer.GetAllDirectMailingsByOrderID(obj.Content));
                printOrders = new ObservableCollection<Print>(DataAccessLayer.GetAllPrintByOrderID(obj.Content));
                surchargeOrders = new ObservableCollection<Surcharge>(DataAccessLayer.GetAllSurchargesByOrderID(obj.Content));

                //Set selected company & contact in dropdowns
                selectedCompany = DataAccessLayer.getCompanyByID(selectedOrder.companyID);
                selectedContact = DataAccessLayer.getContactByID(selectedOrder.contactID);

                //Set the page title
                pageTitle = selectedOrder.orderCode;

                //Calculate order costs
                CalculateCosts();

                //Set deleted lists to new lists
                deletedEmailOrders = new ObservableCollection<Email>();
                deletedDataOrders = new ObservableCollection<Data>();
                deletedDirectMailingOrders = new ObservableCollection<DirectMailing>();
                deletedPrintOrders = new ObservableCollection<Print>();
                deletedSchoolSendOrders = new ObservableCollection<SchoolSend>();
                deletedSharedMailingOrders = new ObservableCollection<SharedMailing>();
                deletedSurchargeOrders = new ObservableCollection<Surcharge>();
            }
            //Create order
            else if(obj.Notification == "OrderViewModel-CompanyID")
            {
                //Get packs
                schoolsendPacks = DataAccessLayer.GetAllSchoolSendPacks();
                sharedPacks = DataAccessLayer.GetAllSharedPacks();

                //Set selected pivot menu item
                pivotIndex = 0;
                
                //Create new lists
                selectedOrder = new Orders() { orderDate = DateTime.Now, userID = MainPageViewModel.loggedInAs.ID };
                emailOrders = new ObservableCollection<Email>();
                dataOrders = new ObservableCollection<Data>();
                schoolSendOrders = new ObservableCollection<SchoolSend>();
                sharedMailingOrders = new ObservableCollection<SharedMailing>();
                directMailingOrders = new ObservableCollection<DirectMailing>();
                printOrders = new ObservableCollection<Print>();
                surchargeOrders = new ObservableCollection<Surcharge>();
                selectedSchoolSendPack = new SchoolSendPack();
                selectedSharedPack = new SharedPack();

                //Set selected company if id passed
                if(obj.Content != 0)
                {
                    selectedCompany = DataAccessLayer.getCompanyByID(obj.Content);
                }

                //Set the page title
                pageTitle = "New Order";

                //Calculate order costs (none)
                CalculateCosts();

                //Set deleted lists to new lists
                deletedEmailOrders = new ObservableCollection<Email>();
                deletedDataOrders = new ObservableCollection<Data>();
                deletedDirectMailingOrders = new ObservableCollection<DirectMailing>();
                deletedPrintOrders = new ObservableCollection<Print>();
                deletedSchoolSendOrders = new ObservableCollection<SchoolSend>();
                deletedSharedMailingOrders = new ObservableCollection<SharedMailing>();
                deletedSurchargeOrders = new ObservableCollection<Surcharge>();
            }
        }

        public void SumEmailOrder()
        {
            if (selectedEmailOrder.emailSetUp)
            {
                selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost + 125;
            }
            else
            {
                selectedEmailOrder.emailCost = selectedEmailOrder.emailAdminCost + selectedEmailOrder.emailDirectCost;
            }
            RaisePropertyChanged("selectedEmailOrder");
        }

        public void SumDirectMailingOrder()
        {
            selectedDirectMailingOrder.directCost = selectedDirectMailingOrder.directFulfilmentCost + selectedDirectMailingOrder.directPostageCost + selectedDirectMailingOrder.directPrintCost;
            RaisePropertyChanged("selectedDirectMailingOrder");
        }

        public void CalculateCosts()
        {
            emailCosts = 0;
            dataCosts = 0;
            schoolSendCosts = 0;
            directMailingCosts = 0;
            sharedMailingCosts = 0;
            printCosts = 0;
            surchargeCosts = 0;

            if (dataOrders != null)
            {
                foreach (Data p in dataOrders)
                {
                    dataCosts = dataCosts + p.dataCost;
                    RaisePropertyChanged("dataCosts");
                }
            }
            else { RaisePropertyChanged("dataCosts"); }
            if (directMailingOrders != null)
            {
                foreach (DirectMailing p in directMailingOrders)
                {
                    directMailingCosts = directMailingCosts + p.directCost;
                    RaisePropertyChanged("directMailingCosts");
                }
            }
            else { RaisePropertyChanged("directMailingCosts"); }
            if (sharedMailingOrders != null)
            {
                foreach (SharedMailing p in sharedMailingOrders)
                {
                    sharedMailingCosts = sharedMailingCosts + p.sharedCost;
                    RaisePropertyChanged("sharedMailingCosts");
                }
            }
            else { RaisePropertyChanged("sharedCosts"); }
            if (printOrders != null)
            {
                foreach (Print p in printOrders)
                {
                    printCosts = printCosts + p.printCost;
                    RaisePropertyChanged("printCosts");
                }
            }
            else { RaisePropertyChanged("printCosts"); }
            if (surchargeOrders != null)
            {
                foreach (Surcharge p in surchargeOrders)
                {
                    surchargeCosts = surchargeCosts + p.surchargeCost;
                    RaisePropertyChanged("surchargeCosts");
                }
            }
            else { RaisePropertyChanged("surchargeCosts"); }

            if (emailOrders != null)
            {
                foreach (Email p in emailOrders)
                {
                    emailCosts = emailCosts + p.emailCost;
                    RaisePropertyChanged("emailCosts");
                }
            }
            else { RaisePropertyChanged("emailCosts"); }
            selectedOrder.orderTotal = emailCosts + dataCosts + directMailingCosts + sharedMailingCosts + printCosts + surchargeCosts;
        }
        
        public void DeleteOrderPart(String orderPart)
        {
            switch (orderPart)
            {
                case "Data":
                    deletedDataOrders.Add(selectedDataOrder);
                    dataOrders.Remove(selectedDataOrder);
                    break;

                case "Email":
                    deletedEmailOrders.Add(selectedEmailOrder);
                    emailOrders.Remove(selectedEmailOrder);
                    break;

                case "SchoolSend":
                    deletedSchoolSendOrders.Add(selectedSchoolSendOrder);
                    schoolSendOrders.Remove(selectedSchoolSendOrder);
                    break;

                case "DirectMailing":
                    deletedDirectMailingOrders.Add(selectedDirectMailingOrder);
                    directMailingOrders.Remove(selectedDirectMailingOrder);
                    break;

                case "SharedMailing":
                    deletedSharedMailingOrders.Add(selectedSharedMailingOrder);
                    sharedMailingOrders.Remove(selectedSharedMailingOrder);
                    break;

                case "Print":
                    deletedPrintOrders.Add(selectedPrintOrder);
                    printOrders.Remove(selectedPrintOrder);
                    break;

                case "Surcharge":
                    deletedSurchargeOrders.Add(selectedSurchargeOrder);
                    surchargeOrders.Remove(selectedSurchargeOrder);
                    break;
            }

            CalculateCosts();
        }
        
        #region School Send Package
        private ObservableCollection<SharedPack> _sharedPacks;
        public ObservableCollection<SharedPack> sharedPacks
        {
            get { return _sharedPacks; }
            set { if (_sharedPacks != value) { _sharedPacks = value; RaisePropertyChanged("sharedPacks"); } }
        }
        private SharedPack _selectedSharedPack = new SharedPack();
        public SharedPack selectedSharedPack
        {
            get { return _selectedSharedPack; }
            set { if (_selectedSharedPack != value) { _selectedSharedPack = value; RaisePropertyChanged("selectedSharedPack"); } }
        }

        private bool _isInvalidPackageName = false;
        public bool isInvalidPackageName
        {
            get { return _isInvalidPackageName; }
            set { if (_isInvalidPackageName != value) { _isInvalidPackageName = !_isInvalidPackageName; RaisePropertyChanged("isInvalidPackageName"); } }
        }
        private bool _isInvalidPackageTo = false;
        public bool isInvalidPackageTo
        {
            get { return _isInvalidPackageTo; }
            set { if (_isInvalidPackageTo != value) { _isInvalidPackageTo = !_isInvalidPackageTo; RaisePropertyChanged("isInvalidPackageTo"); } }
        }
        private bool _isInvalidPackageDate = false;
        public bool isInvalidPackageDate
        {
            get { return _isInvalidPackageDate; }
            set { if (_isInvalidPackageDate != value) { _isInvalidPackageDate = !_isInvalidPackageDate; RaisePropertyChanged("isInvalidPackageDate"); } }
        }
        private bool _isInvalidPackageArtworkDate = false;
        public bool isInvalidPackageArtworkDate
        {
            get { return _isInvalidPackageArtworkDate; }
            set { if (_isInvalidPackageArtworkDate != value) { _isInvalidPackageArtworkDate = !_isInvalidPackageArtworkDate; RaisePropertyChanged("isInvalidPackageArtworkDate"); } }
        }
        private bool _isInvalidPackageDeliveryDate = false;
        public bool isInvalidPackageDeliveryDate
        {
            get { return _isInvalidPackageDeliveryDate; }
            set { if (_isInvalidPackageDeliveryDate != value) { _isInvalidPackageDeliveryDate = !_isInvalidPackageDeliveryDate; RaisePropertyChanged("isInvalidPackageDeliveryDate"); } }
        }
        private bool _isInvalidPackageCost = false;
        public bool isInvalidPackageCost
        {
            get { return _isInvalidPackageCost; }
            set { if (_isInvalidPackageCost != value) { _isInvalidPackageCost = !_isInvalidPackageCost; RaisePropertyChanged("isInvalidPackageCost"); } }
        }
        private bool _isInvalidPackageMaxInserts = false;
        public bool isInvalidPackageMaxInserts
        {
            get { return _isInvalidPackageMaxInserts; }
            set { if (_isInvalidPackageMaxInserts != value) { _isInvalidPackageMaxInserts = !_isInvalidPackageMaxInserts; RaisePropertyChanged("isInvalidPackageMaxInserts"); } }
        }
        #endregion

        #region Save
        public void SaveOrder()
        {
            DataAccessLayer.saveOrder(selectedOrder);
            long orderID = selectedOrder.ID;

            //Data
            if (dataOrders != null)
            {
                foreach (Data d in dataOrders)
                {
                    d.orderID = orderID;
                    DataAccessLayer.saveData(d);
                }
            }
            if (deletedDataOrders != null)
            {
                foreach (Data d in deletedDataOrders)
                {
                    DataAccessLayer.deleteData(d);
                }
            }
            //Email
            if (emailOrders != null)
            {
                foreach (Email e in emailOrders) //Loop through emails
                {
                    e.orderID = orderID;
                    DataAccessLayer.saveEmail(e); //Add emails
                }
            }
            if (deletedEmailOrders != null)
            {
                foreach (Email e in deletedEmailOrders) //Loop through emails
                {
                    DataAccessLayer.deleteEmail(e); //Delete emails
                }
            }
            //SchoolSend
            if (sharedMailingOrders != null)
            {
                foreach (SchoolSend ss in schoolSendOrders)
                {
                    ss.orderID = orderID;
                    DataAccessLayer.saveSchoolSend(ss);
                }
            }
            if (deletedSchoolSendOrders != null)
            {
                foreach (SchoolSend ss in deletedSchoolSendOrders)
                {
                    DataAccessLayer.deleteSchoolSend(ss);
                }
            }
            //Direct mailing
            if (directMailingOrders != null)
            {
                foreach (DirectMailing dm in directMailingOrders)
                {
                    dm.orderID = orderID;
                    DataAccessLayer.saveDirectMailing(dm);
                }
            }
            if (deletedDirectMailingOrders != null)
            {
                foreach (DirectMailing dm in deletedDirectMailingOrders)
                {
                    DataAccessLayer.deleteDirectMailing(dm);
                }
            }
            //Shared mailing
            if (sharedMailingOrders != null)
            {
                foreach (SharedMailing sm in sharedMailingOrders)
                {
                    sm.orderID = orderID;
                    DataAccessLayer.saveSharedMailing(sm);
                }
            }
            if (deletedSharedMailingOrders != null)
            {
                foreach (SharedMailing sm in deletedSharedMailingOrders)
                {
                    DataAccessLayer.deleteSharedMailing(sm);
                }
            }
            //Print
            if (printOrders != null)
            {
                foreach (Print p in printOrders)
                {
                    p.orderID = orderID;
                    DataAccessLayer.savePrint(p);
                }
            }
            if (deletedPrintOrders != null)
            {
                foreach (Print p in deletedPrintOrders)
                {
                    DataAccessLayer.deletePrint(p);
                }
            }
            //Surcharge
            if (surchargeOrders != null)
            {
                foreach (Surcharge s in surchargeOrders)
                {
                    s.orderID = orderID;
                    DataAccessLayer.saveSurcharge(s);
                }
            }
            if (deletedSurchargeOrders != null)
            {
                foreach (Surcharge s in deletedSurchargeOrders)
                {
                    DataAccessLayer.deleteSurcharge(s);
                }
            }
        }

        public void SaveOrderPart(String orderPart)
        {
            switch (orderPart)
            {
                case "Data":
                    if (isValidDataOrder(selectedDataOrder))
                    {
                        dataOrders.Remove(originalDataOrder);
                        dataOrders.Add(selectedDataOrder);
                        NavigationService.GoBack();
                        pivotIndex = 1;
                        CalculateCosts();
                    }
                    break;

                case "Email":
                    if (isValidEmailOrder(selectedEmailOrder))
                    {
                        emailOrders.Remove(originalEmailOrder);
                        emailOrders.Add(selectedEmailOrder);
                        pivotIndex = 2;
                        NavigationService.GoBack();
                        CalculateCosts();
                    }
                    break;

                case "SchoolSend":
                    if (InvalidateSchoolSendOrder(selectedSchoolSendOrder))
                    {
                        schoolSendOrders.Remove(originalSchoolSendOrder);
                        schoolSendOrders.Add(selectedSchoolSendOrder);
                        NavigationService.GoBack();
                        pivotIndex = 3;
                        CalculateCosts();
                    }
                    break;

                case "DirectMailing":
                    if (isValidDirectMailingOrder(selectedDirectMailingOrder))
                    {
                        directMailingOrders.Remove(originalDirectMailingOrder);
                        directMailingOrders.Add(selectedDirectMailingOrder);
                        NavigationService.GoBack();
                        pivotIndex = 4;
                        CalculateCosts();
                    }
                    break;

                case "SharedMailing":
                    if (isValidSharedMailingOrder(selectedSharedMailingOrder))
                    {
                        sharedMailingOrders.Remove(originalSharedMailingOrder);
                        sharedMailingOrders.Add(selectedSharedMailingOrder);
                        NavigationService.GoBack();
                        pivotIndex = 5;
                        CalculateCosts();
                    }
                    break;

                case "Print":
                    if (isValidPrintOrder(selectedPrintOrder))
                    {
                        printOrders.Remove(originalPrintOrder);
                        printOrders.Add(selectedPrintOrder);
                        NavigationService.GoBack();
                        pivotIndex = 6;
                        CalculateCosts();
                    }
                    break;

                case "Surcharge":
                    if (isValidSurchargeOrder(selectedSurchargeOrder))
                    {
                        surchargeOrders.Remove(originalSurchargeOrder);
                        surchargeOrders.Add(selectedSurchargeOrder);
                        NavigationService.GoBack();
                        pivotIndex = 7;
                        CalculateCosts();
                    }
                    break;
                case "SharedPack":
                    if (isValidSharedPackOrder(selectedSharedPack))
                    {
                        DataAccessLayer.saveSharedPack(selectedSharedPack);
                        sharedPacks = DataAccessLayer.GetAllSharedPacks();

                        NavigationService.GoBack();
                    }
                    break;
            }
        }
        #endregion

        #region Duplicate
        public void duplicateDataOrder(Data data)
        {
            Data dupData = new Data()
            {
                dataStart = data.dataStart,
                dataEnd = data.dataEnd,
                dataDetails = data.dataDetails,
                dataCost = data.dataCost,
                dataCreated = DateTime.Now,
                dataModified = DateTime.Now
            };
            dataOrders.Add(dupData);
        }
        
        public async void duplicateEmailOrder(Email email)
        {
            var newDuplicateDialog = new DuplicateDialog(); //Create new input dialog
            var result = await newDuplicateDialog.ShowAsync(); //Show dialog & await result
            if (result == ContentDialogResult.Primary) //If primary option chosen
            {
                var item = newDuplicateDialog.Content; //Get user input
                string dateString = item.ToString(); //Convert to string
                duplicateEmailDate = Convert.ToDateTime(dateString); //Convert to DateTime
            }
            Email dupEmail = new Email()
            {
                emailDate = duplicateEmailDate,
                emailAdminCost = email.emailAdminCost,
                emailCost = email.emailCost,
                emailDetails = email.emailDetails,
                emailDirectCost = email.emailDirectCost,
                emailSetUp = email.emailSetUp,
                emailSubject = email.emailSubject,
                emailCreated = DateTime.Now,
                emailModified = DateTime.Now
            };
            emailOrders.Add(dupEmail);
        }

        public void duplicateSchoolSendOrder(SchoolSend schoolSend)
        {
            SchoolSend dupSchoolSend = new SchoolSend()
            {
                schoolsendCost = schoolSend.schoolsendCost,
                schoolsendCredits = schoolSend.schoolsendCredits,
                schoolsendEnd = schoolSend.schoolsendEnd,
                schoolsendPackage = schoolSend.schoolsendPackage,
                schoolsendStart = schoolSend.schoolsendStart,
                schoolsendCreated = DateTime.Now,
                schoolsendModified = DateTime.Now
            };
            schoolSendOrders.Add(dupSchoolSend);
        }

        public void duplicateDirectMailingOrder(DirectMailing directMailing)
        {
            DirectMailing dupDirectMailing = new DirectMailing()
            {
                directArtworkDate = directMailing.directArtworkDate,
                directCost = directMailing.directCost,
                directCreated = DateTime.Now,
                directDataDate = directMailing.directDataDate,
                directDate = directMailing.directDate,
                directDeliveryCode = directMailing.directDeliveryCode,
                directDetails = directMailing.directDetails,
                directFulfilmentCost = directMailing.directFulfilmentCost,
                directInsertDate = directMailing.directInsertDate,
                directLeafletCode = directMailing.directLeafletCode,
                directMailingTo = directMailing.directMailingTo,
                directModified = DateTime.Now,
                directPostageCost = directMailing.directPostageCost,
                directPrintCost = directMailing.directPrintCost
            };
            directMailingOrders.Add(dupDirectMailing);
        }

        public void duplicateSharedMailingOrder(SharedMailing sharedMailing)
        {
            SharedMailing dupSharedMailing = new SharedMailing()
            {
                sharedArtworkDate = sharedMailing.sharedArtworkDate,
                sharedCost = sharedMailing.sharedCost,
                sharedCreated = DateTime.Now,
                sharedDate = sharedMailing.sharedDate,
                sharedDeliveryCode = sharedMailing.sharedDeliveryCode,
                sharedDeliveryDate = sharedMailing.sharedDeliveryDate,
                sharedFAO = sharedMailing.sharedFAO,
                sharedLeafletName = sharedMailing.sharedLeafletName,
                sharedLeafletSize = sharedMailing.sharedLeafletSize,
                sharedLeafletWeight = sharedMailing.sharedLeafletWeight,
                sharedMailingTo = sharedMailing.sharedMailingTo,
                sharedModified = DateTime.Now,
                sharedNumberOfLeaflets = sharedMailing.sharedNumberOfLeaflets,
                sharedPackage = sharedMailing.sharedPackage
            };
            sharedMailingOrders.Add(dupSharedMailing);
        }

        public void duplicatePrintOrder(Print print)
        {
            Print dupPrint = new Print()
            {
                printCharge = print.printCharge,
                printCost = print.printCost,
                printCreated = DateTime.Now,
                printDate = print.printDate,
                printDetails = print.printDetails,
                printModified = DateTime.Now,
                printPrinter = print.printPrinter
            };
            printOrders.Add(dupPrint);
        }

        public void duplicateSurchargeOrder(Surcharge surcharge)
        {
            Surcharge dupSurcharge = new Surcharge()
            {
                surchargeCost = surcharge.surchargeCost,
                surchargeCreated = DateTime.Now,
                surchargeDate = surcharge.surchargeDate,
                surchargeDetails = surcharge.surchargeDetails,
                surchargeModified = DateTime.Now
            };
            surchargeOrders.Add(surcharge);
        }
        #endregion
        
        #region Validation Fuctions
        public bool isValidOrder(Orders order)
        {
            isInvalidOrderCompany = (order.companyID != 0)    ? true : false;
            isInvalidOrderContact = (order.contactID != 0)    ? true : false;
            isInvalidOrderCode    = (order.orderCode != null) ? true : false;
            isInvalidOrderDate    = (order.orderDate != null) ? true : false;
            
            return (isInvalidOrderCompany == true || isInvalidOrderContact == true || isInvalidOrderCode == true || isInvalidOrderDate == true) ? false : true;
        }

        public bool isValidDataOrder(Data data)
        {
            isInvalidDataStart   = (data.dataStart == null)        ? true : false;
            isInvalidDataEnd     = (data.dataEnd < data.dataStart) ? true : false;
            isInvalidDataCost    = (data.dataCost < 0)             ? true : false;
            isInvalidDataDetails = (data.dataDetails == null)      ? true : false;

            return (isInvalidDataStart == true || isInvalidDataEnd == true || isInvalidDataCost == true || isInvalidDataDetails == true) ? false : true;
        }

        public bool isValidEmailOrder(Email email)
        { 
            isInvalidEmailDate       = (email.emailDate == null)    ? true : false;
            isInvalidEmailDetails    = (email.emailDetails == null) ? true : false;
            isInvalidEmailAdminCost  = (email.emailAdminCost < 0)   ? true : false;
            isInvalidEmailDirectCost = (email.emailDirectCost< 0)   ? true : false;
            isInvalidEmailCost       = (email.emailCost< 0)         ? true : false;

            return (isInvalidEmailAdminCost == true || isInvalidEmailCost == true || isInvalidEmailDetails == true || isInvalidEmailDirectCost == true) ? false : true;
        }

        public bool InvalidateSchoolSendOrder(SchoolSend schoolsend)
        {
            isInvalidSchoolSendStart   = (schoolsend.schoolsendStart == null)                                                        ? true : false;
            isInvalidSchoolSendEnd     = (schoolsend.schoolsendEnd == null || schoolsend.schoolsendEnd < schoolsend.schoolsendStart) ? true : false;
            isInvalidSchoolSendPackage = (schoolsend.schoolsendPackage <= 0)                                                         ? true : false;
            isInvalidSchoolSendCost    = (schoolsend.schoolsendCost < 0)                                                             ? true : false;

            return (isInvalidSchoolSendStart == true || isInvalidSchoolSendEnd == true || isInvalidSchoolSendPackage == true || isInvalidSchoolSendCost == true) ? false : true;
        }

        public bool isValidDirectMailingOrder(DirectMailing directMailing)
        {
            isInvalidDirectMailingDate           = (directMailing.directDate == null)                           ? true : false;
            isInvalidDirectMailingArtworkDate    = (directMailing.directArtworkDate > directMailing.directDate) ? true : false;
            isInvalidDirectMailingDataDate       = (directMailing.directDataDate > directMailing.directDate)    ? true : false;
            isInvalidDirectMailingInsertDate     = (directMailing.directInsertDate > directMailing.directDate)  ? true : false;
            isInvalidDirectMailingDeliveryCode   = (directMailing.directDeliveryCode == null)                   ? true : false;
            isInvalidDirectMailingDetails        = (directMailing.directDetails == null)                        ? true : false;
            isInvalidDirectMailingFulfilmentCost = (directMailing.directFulfilmentCost < 0)                     ? true : false;
            isInvalidDirectMailingPostageCost    = (directMailing.directPostageCost < 0)                        ? true : false;
            isInvalidDirectMailingPrintCost      = (directMailing.directPrintCost < 0)                          ? true : false;
            
            if (isInvalidDirectMailingDate == true ||
                isInvalidDirectMailingArtworkDate == true ||
                isInvalidDirectMailingInsertDate == true ||
                isInvalidDirectMailingDataDate == true ||
                isInvalidDirectMailingDeliveryCode == true ||
                isInvalidDirectMailingDetails == true ||
                isInvalidDirectMailingFulfilmentCost == true ||
                isInvalidDirectMailingPostageCost == true ||
                isInvalidDirectMailingPrintCost == true)
            {
                return false;
            }
            else
            {
                selectedDirectMailingOrder.directCost = selectedDirectMailingOrder.directFulfilmentCost + selectedDirectMailingOrder.directPostageCost + selectedDirectMailingOrder.directPrintCost;
                RaisePropertyChanged("selectedDirectMailingOrder");
                return true;
            }
        }

        public bool isValidSharedMailingOrder(SharedMailing sharedMailing)
        {

            isInvalidSharedPackage      = (sharedMailing.sharedPackage <= 0)                                                                                                                              ? true : false;
            isInvalidSharedDate         = (sharedMailing.sharedDate == null || sharedMailing.sharedDate < sharedMailing.sharedDeliveryDate || sharedMailing.sharedDate < sharedMailing.sharedArtworkDate) ? true : false;
            isInvalidSharedDeliveryDate = (sharedMailing.sharedDeliveryDate == null || sharedMailing.sharedDeliveryDate > sharedMailing.sharedDate)                                                       ? true : false;
            isInvalidSharedArtworkDate  = (sharedMailing.sharedArtworkDate == null || sharedMailing.sharedArtworkDate > sharedMailing.sharedDate)                                                         ? true : false;
            isInvalidSharedCost         = (sharedMailing.sharedCost < 0)                                                                                                                                  ? true : false;

            return (isInvalidSharedPackage == true || isInvalidSharedDate == true || isInvalidSharedDeliveryDate == true || isInvalidSharedArtworkDate == true || isInvalidSharedCost == true) ? false : true;
        }

        public bool isValidPrintOrder(Print print)
        {
            isInvalidPrintDate    = (print.printDate == null)    ? true : false;
            isInvalidPrintPrinter = (print.printPrinter == null) ? true : false;
            isInvalidPrintDetails = (print.printDetails == null) ? true : false;
            isInvalidPrintCharge  = (print.printCharge < 0)      ? true : false;
            isInvalidPrintCost    = (print.printCost < 0)        ? true : false;

            return (isInvalidPrintCharge == true || isInvalidPrintDetails == true || isInvalidPrintPrinter == true || isInvalidPrintCost == true) ? false : true;
        }

        public bool isValidSurchargeOrder(Surcharge surcharge)
        {
            isInvalidSurchargeDate    = (surcharge.surchargeDate == null)    ? true : false;
            isInvalidSurchargeDetails = (surcharge.surchargeDetails == null) ? true : false;
            isInvalidSurchargeCost    = (surcharge.surchargeCost < 0)        ? true : false;

            return (isInvalidSurchargeDate == true || isInvalidSurchargeDetails == true || isInvalidSurchargeCost == true) ? false : true;
        }

        public bool isValidSharedPackOrder(SharedPack pack)
        {
            if(pack.packArtworkDate == null || pack.packArtworkDate > pack.packDate) { isInvalidPackageArtworkDate = true; }
            else { isInvalidPackageArtworkDate = false; }

            if(pack.packCost < 0) { isInvalidPackageCost = true; }
            else { isInvalidPackageCost = false; }

            if(pack.packDate == null || pack.packDate < pack.packArtworkDate || pack.packDate < pack.packDeliveryDate) { isInvalidPackageDate = true; }
            else { isInvalidPackageDate = false; }

            if(pack.packDeliveryDate == null || pack.packDeliveryDate > pack.packDate) { isInvalidPackageDeliveryDate = true; }
            else { isInvalidPackageDeliveryDate = false; }

            if(pack.packMaxInserts < 0) { isInvalidPackageMaxInserts = true; }
            else { isInvalidPackageMaxInserts = false; }

            if(pack.packName == null) { isInvalidPackageName = true; }
            else { isInvalidPackageName = false; }

            if (pack.packTo == null) { isInvalidPackageTo = true; }
            else { isInvalidPackageTo = false; }

            if(isInvalidPackageArtworkDate == true ||
                isInvalidPackageCost == true ||
                isInvalidPackageDate == true ||
                isInvalidPackageDeliveryDate == true ||
                isInvalidPackageMaxInserts == true ||
                isInvalidPackageName == true ||
                isInvalidPackageTo == true)
            {
                return false;
            }
            else { return true; }
        }
        #endregion

        #region Left Click
        public void dataClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected data from item
            Data item = arg.ClickedItem as Data;
            //Set selected order
            originalDataOrder = dataOrders.Where(x => x == item).First();
            selectedDataOrder = dataOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(DataView));
        }

        public void emailClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected email from item
            Email item = arg.ClickedItem as Email;
            //Set selected order
            originalEmailOrder = emailOrders.Where(x => x == item).First();
            selectedEmailOrder = emailOrders.Where(x => x == item).First();
            ////Navigate to order
            NavigationService.Navigate(typeof(EmailView));
        }

        public void schoolSendClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected SchoolSend from item
            SchoolSend item = arg.ClickedItem as SchoolSend;
            //Set selected order
            originalSchoolSendOrder = schoolSendOrders.Where(x => x == item).First();
            selectedSchoolSendOrder = schoolSendOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SchoolSendView));
        }

        public void directMailingClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected directMailing from item
            DirectMailing item = arg.ClickedItem as DirectMailing;
            //Set selected order
            originalDirectMailingOrder = directMailingOrders.Where(x => x == item).First();
            selectedDirectMailingOrder = directMailingOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(DirectMailingView));
        }

        public void sharedMailingClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected SharedMailing from item
            SharedMailing item = arg.ClickedItem as SharedMailing;
            //Set selected order
            originalSharedMailingOrder = sharedMailingOrders.Where(x => x == item).First();
            selectedSharedMailingOrder = sharedMailingOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SharedMailingView));
        }

        public void printClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected print from item
            Print item = arg.ClickedItem as Print;
            //Set selected order
            originalPrintOrder = printOrders.Where(x => x == item).First();
            selectedPrintOrder = printOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(PrintView));
        }

        public void surchargeClicked(object sender, object parameter)
        {
            //Get selected item
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            //Get selected Surcharge from item
            Surcharge item = arg.ClickedItem as Surcharge;
            //Set selected order
            originalSurchargeOrder = surchargeOrders.Where(x => x == item).First();
            selectedSurchargeOrder = surchargeOrders.Where(x => x == item).First();
            //Navigate to order
            NavigationService.Navigate(typeof(SurchargeView));
        }
        #endregion

        #region Right Click
        public void dataRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var dataSource = args.OriginalSource; //Gets right clicked item
                var dataDataContext = (dataSource as TextBlock).DataContext; //Gets the Data context
                selectedDataOrder = (Data)dataDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void emailRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var emailSource = args.OriginalSource; //Gets right clicked item
                var emailDataContext = (emailSource as TextBlock).DataContext; //Gets the data context
                selectedEmailOrder = (Email)emailDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void schoolSendRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var schoolSendSource = args.OriginalSource; //Gets right clicked item
                var SchoolSendContext = (schoolSendSource as TextBlock).DataContext; //Gets the SchoolSend context
                selectedSchoolSendOrder = (SchoolSend)SchoolSendContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void directMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var directMailingSource = args.OriginalSource; //Gets right clicked item
                var directMailingDataContext = (directMailingSource as TextBlock).DataContext; //Gets the data context
                selectedDirectMailingOrder = (DirectMailing)directMailingDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void sharedMailingRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var sharedMailingSource = args.OriginalSource; //Gets right clicked item
                var sharedMailingSharedMailingContext = (sharedMailingSource as TextBlock).DataContext; //Gets the SharedMailing context
                selectedSharedMailingOrder = (SharedMailing)sharedMailingSharedMailingContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void printRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var printSource = args.OriginalSource; //Gets right clicked item
                var printDataContext = (printSource as TextBlock).DataContext; //Gets the Print context
                selectedPrintOrder = (Print)printDataContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }

        public void surchargeRightClicked(object sender, RightTappedRoutedEventArgs args)
        {
            try
            {
                var surchargeSource = args.OriginalSource; //Gets right clicked item
                var surchargeSurchargeContext = (surchargeSource as TextBlock).DataContext; //Gets the Surcharge context
                selectedSurchargeOrder = (Surcharge)surchargeSurchargeContext; //Convert to class
            }
            catch (Exception e) { } //Catch null exception
        }
        #endregion
    }

}
