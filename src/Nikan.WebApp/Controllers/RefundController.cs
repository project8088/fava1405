using cle.Services;
using cle.Services.Citizens;
using cle.Services.CitizensGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses;
using Nikan.MellatApp;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.ImportFile;
using Nikan.Services.Refund;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.Refund;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RefundController : Controller
    {

        #region Ctor
        private readonly IImportExcelFileService _importFile; 
        private readonly IRefundService _refundService;
        private readonly IUsersService _usersService;
        
        private readonly ICitizenService _citzene;
       
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICitizenSummaryEducationServices _educationService;
        private readonly IOptionsSnapshot<ApiSettings> _configuration;
        private readonly IGroupService _group;
        public static IHostingEnvironment _environment;

        public RefundController(
              IUsersService userManager,
              ICitizenService citzene,
               
               IGroupService  group,
                IOptionsSnapshot<ApiSettings> configuration,
              ICitizenFeedbackService citizenFeedback,
              
               IRefundService  refundService,
              ICitizenSummaryEducationServices educationService,
              IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            ISiteSettingService siteSettingService,
             IImportExcelFileService importFile,
            IAntiForgeryCookieService antiforgery


            )
        {
            _importFile = importFile;
            _refundService =  refundService;
            _citzene = citzene;
            
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
           
            _group =  group;
            _siteSettingService = siteSettingService;
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _educationService = educationService;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));


             this._configuration = configuration;
           this._configuration.CheckArgumentIsNull(nameof(configuration));

        }
        #endregion


        
 


        /// <summary>
        /// لیست فایل های وارد شده 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RefundImportFileList")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<RefundImportFileInfo>>> RefundImportFileList()
        {
            int citizenId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
                return new ApiResult<List<RefundImportFileInfo>>(false, ApiResultStatusCode.BadRequest,  null, "شما به  استرداد هزینه  دسترسی ندارید");

            return await _importFile.RefundImportFileList();

        }


        /// <summary>
        /// بارگذاری فایل لیست  برگشت
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportRefundListFromExcel")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<UploadFileResult>> ImportRefundListFromExcel(IFormFile file)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                res.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                res.IsSuccess = false;
                return res;
            }


            try
            {
                var fileName = "";
                string folderName = "Refund";
                var ticks = DateTime.Now.Ticks;
                var webRootPath = _environment.WebRootPath + "/uploads/Resources/Refund/" + ticks;

                string newPath = Path.Combine(webRootPath, folderName);
                var list = new List<RefundExcelFileColumns>();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    fileName = file.FileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        var cells = headerRow.Cells;
                        if (cellCount < 2)
                        {
                            res.IsSuccess = false;
                            res.Messages = "فایل اکسل نامعتبر می باشد";
                            res.StatusCode = ApiResultStatusCode.BadRequest;
                        }
                        else
                        {
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                            {
                                IRow row = sheet.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                                

                                var cell1 = row.GetCell(0);//NationalCode 
                                var cell2 = row.GetCell(1);//orderId



                                var cell3 = row.GetCell(2);//saleReferenceId
                                var cell4 = row.GetCell(3);//refundAmount 
                                var cell5 = row.GetCell(4);// TotalRefundAmount
                                var cell6 = row.GetCell(5);//Description

                                if (cell1 != null && cell2 != null)
                                {

                                    var orderId = "";
                                    var saleReferenceId = ""; 
                                    var description = "";
                                    var nationalCode = "";
                                    var otherDescription = "";
                                    
                                    long refundAmount = 0;
                                    long totalRefundAmount = 0;



                                    //NationalCode cell1
                                    if (cell1 != null)
                                        nationalCode = cell1.ToString();

                                    //orderId cell2
                                    if (cell2 != null)
                                        orderId = cell2.ToString();

                                    //saleReferenceId cell3
                                    if (cell3 != null)
                                        saleReferenceId = cell3.ToString();

                                    //refundAmount cell4
                                    if (cell4 != null)
                                        long.TryParse(cell4.ToString(), out refundAmount);


                                    //TotalRefundAmount cell5 
                                    if (cell5 != null)
                                        long.TryParse(cell5.ToString(), out totalRefundAmount);



                                    //Description cell6
                                    if (cell6 != null)
                                        description = cell6.ToString();
 
                                     
                                   
                                        list.Add(new RefundExcelFileColumns
                                        {
                                            Description=description,
                                            NationalCode=nationalCode,
                                            OrderId=orderId,
                                            OtherDescription=otherDescription,
                                            RefundAmount=refundAmount,
                                            SaleReferenceId=saleReferenceId,
                                            TotalRefundAmount=totalRefundAmount, 
                                             
                                        });
                                    

                                }

                            }

                        }


                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "فایل اکسل را انتخاب کنید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                }
                if (list.Any())
                {
                   
                    var addfile = await _importFile.AddRefundImportFile(list, fileName, userId);
                    if (addfile.IsSuccess == false)
                    {
                        res.Messages = addfile.Messages;
                        res.IsSuccess = false;
                    }
                    else
                    {
                        res.Data.ImportId = addfile.Data.ImportId;
                    }

                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است. ممکن است فایل ورودی نامعتبر باشد";
                res.StatusCode = ApiResultStatusCode.BadRequest;

            }
            return res;
        }


        /// <summary>
        /// جزئیات دریافت یک فایل اکسل
        /// </summary>
        /// <param name="importId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RefundImportFileDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<RefundImportFileInfo>> RefundImportFileDetails(int importId)
        {

            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
                return new ApiResult<RefundImportFileInfo>(false, ApiResultStatusCode.BadRequest, null, "شما به  استرداد هزینه  دسترسی ندارید");



            return await _importFile.RefundImportFileDetails(importId);

        }

        /// <summary>
        /// دریافت اطلاعات کارت براساس شناسه فایل استرداد
        /// </summary>
        /// <param name="importId">شناسه فایل اکسل</param>
        /// <returns></returns> 
        [HttpGet]
        [Route("GetCardsNumber")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> GetCardsNumber(int importId)
        {

            var result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }










            if (importId == 0)
            {
                result.Messages = "شناسه استعلام را مشخص نمایید";
                result.IsSuccess = false;
                return result;
            }

            var bankOption = await _siteSettingService.GetFinancialSettings();
            if(bankOption==null)
            {
                result.Messages = "تنطیمات درگاه بانکی انجام نشده است";
                result.IsSuccess = false;
                return result;
            }

            if(bankOption.Data.RefundTerminalId == null)
            {
                result.Messages = "تنطیمات درگاه بانکی انجام نشده است";
                result.IsSuccess = false;
                return result;
            }


            var bankMellatImplement = new BankMellatImplement(bankOption.Data.RefundTerminalId.Value,
                bankOption.Data.RefundUserName, bankOption.Data.RefundPassword, 0, 0);

            try
            {
                var res = await _refundService.RefundList(importId);
                if (res.IsSuccess)
                {

                    var data = res.Data;
                    var setCard = 0;
                    foreach (var order in data)
                    {
                        var card = order.RefundCardNumber;
                         
                            long refId = 0;
                            if (!string.IsNullOrWhiteSpace(order.TransactionCode))
                            {
                                if (long.TryParse(order.TransactionCode, out refId))
                                {


                                    card = bankMellatImplement.CallRefundWebService(refId);
                                    if (!string.IsNullOrWhiteSpace(card))
                                    {
                                        if (!card.Contains("Transaction Not Found"))
                                        {

                                            card = card.Replace("&#xD;", " ").Replace("&lt;", "<").Replace("&gt;", ">");
                                            var index = card.IndexOf("name='pan'") + 18;
                                            var cradNumber = card.Substring(index, 16);
                                            var resAdd = await _refundService.UpdateRefundCardNumber(order.RefundId, cradNumber, card);
                                            if (resAdd.IsSuccess) setCard++;
                                        }
                                     }

                                }

                            } 
                    }

                    result.Messages = "  تعداد " + setCard +" شماره کارت با موفقیت استعلام گردید ";
                }
                else
                {
                    result.Messages = "خطا در دریافت لیست اطلاعات";
                    result.IsSuccess = false; 
                }


            }
            catch (Exception er)
            {
                result.Messages = "خطایی رخ داده است" + er.Message;
                result.IsSuccess = false;
            }

            return result;
        }


        [HttpGet]
        [Route("GetCardNumber")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> GetCardNumber(int refundId)
        {

            var result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }



            if (refundId == 0)
            {
                result.Messages = "شناسه استرداد را مشخص نمایید";
                result.IsSuccess = false;
                return result;
            }

            var bankOption = await _siteSettingService.GetFinancialSettings();
            if (bankOption == null)
            {
                result.Messages = "تنطیمات درگاه بانکی انجام نشده است";
                result.IsSuccess = false;
                return result;
            }

            if (bankOption.Data.RefundTerminalId == null)
            {
                result.Messages = "تنطیمات درگاه بانکی انجام نشده است";
                result.IsSuccess = false;
                return result;
            }


            var bankMellatImplement = new BankMellatImplement(bankOption.Data.RefundTerminalId.Value,
                bankOption.Data.RefundUserName, bankOption.Data.RefundPassword, 0, 0);

            try
            {
                var res = await _refundService.RefundInfoByAdmin(refundId);
                if (res.IsSuccess)
                {

                    var order = res.Data;
                    var setCard = 0;

                    var card = order.RefundCardNumber;

                    long refId = 0;
                    if (!string.IsNullOrWhiteSpace(order.TransactionCode))
                    {
                        if (long.TryParse(order.TransactionCode, out refId))
                        {

                            card = bankMellatImplement.CallRefundWebService(refId);
                            if (!string.IsNullOrWhiteSpace(card))
                            {
                                if (!card.Contains("Transaction Not Found"))
                                {

                                    card = card.Replace("&#xD;", " ").Replace("&lt;", "<").Replace("&gt;", ">");
                                    var index = card.IndexOf("name='pan'") + 18;
                                    var cradNumber = card.Substring(index, 16);
                                    var resAdd = await _refundService.UpdateRefundCardNumber(order.RefundId, cradNumber, card);
                                    if (resAdd.IsSuccess) setCard++;
                                }
                            }

                        }

                    }


                    result.Messages = "  تعداد " + setCard + " شماره کارت با موفقیت استعلام گردید ";


                }
                else
                {
                    result.Messages = "خطا در دریافت لیست اطلاعات";
                    result.IsSuccess = false;
                }


            }
            catch (Exception er)
            {
                result.Messages = "خطایی رخ داده است" + er.Message;
                result.IsSuccess = false;
            }

            return result;
        }


        /// <summary>
        /// حذف فایل اکسل
        /// </summary>
        /// <param name="importId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveRefundImportFile")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> RemoveRefundImportFile(int importId)
        {
            var  result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _importFile.Remove(importId);

        }




        #region مدیریت دسترسیها



        /// <summary>
        ///  ایجاد دسترسی برای مسئول مرکز
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<RefundDto>> AddRefundAccess(RefundDto model)
        {
            var  result = new ApiResult<RefundDto>(true, ApiResultStatusCode.Success, (RefundDto)null, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.AddRefundFromFileList(model, userId);
        }

        /// <summary>
        /// ویرایش دسترسی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<ChangeRefundDto>> UpdateRefundAccess(ChangeRefundDto model)
        {
            ApiResult<ChangeRefundDto> result = new ApiResult<ChangeRefundDto>(true, ApiResultStatusCode.Success, (ChangeRefundDto)null, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.UpdateRefundAccess(model, userId);
        }


        /// <summary>
        /// ویرایش اطلاعات استرداد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<ChangeTransactionRefund>> UpdateRefund(ChangeTransactionRefund model)
        {
           var result = new ApiResult<ChangeTransactionRefund>(true, ApiResultStatusCode.Success,  null, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.UpdateRefund(model, userId);
        }

        /// <summary>
        /// اضافه کردن یک تراکنش جدید
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> AddRefund(AddTransactionRefund model)
        {
            ApiResult result = new ApiResult(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.AddRefund(model, userId);
        }




        /// <summary>
        /// گزارش استراد
        /// </summary>
        /// <param name="importId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReportRefund")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ApiResult<ReportRefund>> GetReportRefund(int importId)
        {
            ApiResult<ReportRefund> result = new ApiResult<ReportRefund>(true, ApiResultStatusCode.Success, (ReportRefund)null, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.ReportRefund(importId);

        }



        /// <summary>
        /// ثبت شماره کارت توسط کارشناس مرکز یا مدیر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ApiResult> UpdateRefundSaveCardNmber(UpdateRefundCardNumber model)
        {
            ApiResult result = new ApiResult(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(userId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            return await _refundService.UpdateRefundSaveCardNmber(model, userId);

        }

        /// <summary>
        /// لیست دسترسی های ایجاد شده برای استرداد
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="unitName"></param>
        /// <param name="letterNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllRefundAccessPageList")]
        public async Task<ApiResult<PagedRefundViewModel>> GetAllRefundAccessPageList
         (int? offset = 1, int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          bool? isClosed = null,
          string unitName = null, string nationCode = null, string letterNumber = null

         )
        {

            int citizenId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
                return new ApiResult<PagedRefundViewModel>(false, ApiResultStatusCode.BadRequest, (PagedRefundViewModel)null, "شما به  استرداد هزینه  دسترسی ندارید");


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _refundService.RefundAccessPageList(offset.Value, count.Value, FromDate, 
                 ToDate, isClosed, unitName, nationCode, letterNumber);

        }



        /// <summary>
        /// لیست دسترسی هایی که برای مراکز باز می باشد
        /// مخصوص کارشناسان مراکز
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="letterNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetRefundCitizenAccessPageList")]
        public async Task<ApiResult<PagedRefundViewModel>> GetRefundCitizenAccessPageList
        (int? offset = 1, int? count = 20,    string letterNumber = null

        )
        {


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var userId = _usersService.GetCurrentUserId();
            return await _refundService.RefundCitizenAccessPageList(offset.Value, count.Value, userId,
                  letterNumber);

        }
        /// <summary>
        /// لیست دسترسی هایی که برای کارشناسان مراکز باز شده است 
        /// نمایش در پنل شهروندی
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="unitName"></param>
        /// <param name="letterNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("RefundCitizenAccessPageList")]
        public async Task<ApiResult<PagedRefundViewModel>> RefundCitizenAccessPageList
         (int? offset = 1, int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string unitName = null, string letterNumber = null

         )
        {

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var userId = _usersService.GetCurrentUserId();
            return await _refundService.RefundCitizenAccessPageList(userId,offset.Value, count.Value, FromDate,
                 ToDate, unitName, letterNumber);

        }




        /// <summary>
        /// جزئیات یک دسترسی
        /// </summary>
        /// <param name="importId">شناسه فایل</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate">از تاریخ</param>
        /// <param name="ToDate">تاتاریخ</param>
        /// <param name="nationCode">کد ملی شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RefundAccessDetailsList")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> RefundAccessDetailsList(
          int importId, int? offset = 1, int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string nationCode = null,
           string transactionCode = null,
       string orderId = null,
         RefundStateEnum? refundState  = null
            )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;


            return await _refundService.RefundAccessPagesList(offset.Value, count.Value, importId, FromDate,
                 ToDate, nationCode, transactionCode, orderId, refundState);

        }




        /// <summary>
        /// جستجوی استرداد
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="nationCode"></param>
        /// <param name="unitName"></param>
        /// <param name="transactionCode"></param>
        /// <param name="orderId"></param>
        /// <param name="refundState"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AllRefundAccessPagesList")]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> AllRefundAccessPagesList(
         int? offset = 1, int? count = 20,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string nationCode = null,
         string unitName = null,
         string transactionCode = null,
     string orderId = null,
       RefundStateEnum? refundState = null, string name = null
		  )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;


            return await _refundService.AllRefundAccessPagesList(offset.Value, count.Value,  FromDate,
                 ToDate, nationCode, unitName, transactionCode, orderId, refundState,name);

        }













        /// <summary>
        /// جزئیات یک استرداد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRefundInfoDetailsByAdmin")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ApiResult<ImportRefundList>> GetRefundInfoDetailsByAdmin(int id)
        { 
            return await _refundService.RefundInfoByAdmin(id); 
        }


        /// <summary>
        /// حذف دسترسی 
        /// </summary>
        /// <param name="id">شناسه دسترسی</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveRefundAccess")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> RemoveRefundAccess(int id)
        {
            return await _refundService.Remove(id);

        }




        #endregion


        #region تایید یا عدم تایید برگشت هزینه

        /// <summary>
        /// برگشت هزینه توسط مدیر یا کارشناس مرکز
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CommitRefundByAdmin")]
        [Authorize(Roles = "Admin,Citizen")]
        /// <summary>
        /// تایید برگشت
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> CommitRefundByAdmin(int refundId)
        {
            var result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");

           
            int citizenId = this._usersService.GetCurrentUserId();
            bool flag = await this._citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }


            try
            {
                var bankOption = await _siteSettingService.GetFinancialSettings();
                if(!bankOption.IsSuccess)
                {
                    result.Messages = "خطا در بازیابی تنظیمات بانکی";
                    result.IsSuccess = false;
                }
                if (!bankOption.IsSuccess)
                {
                    result.Messages = " تنظیمات بانکی انجام نشده است ";
                    result.IsSuccess = false;
                    return result;
                }
                if(bankOption.Data.RefundTerminalId==null)
                {
                    result.Messages = " تنظیمات بانکی انجام نشده است ";
                    result.IsSuccess = false;
                    return result;
                }
                var res = await _refundService.RefundInfoByAdmin(refundId);
                if (res.IsSuccess)
                {
                    
                    var data = res.Data;

                    if(data.RefundState ==RefundStateEnum.برگشت_هزینه)
                    {
                        result.Messages = " برای این ردیف برگشت هزینه صورت گرفته است";
                        result.IsSuccess = false;
                        return result;
                    }

                    if (data.IsClosed==true)
                    {
                        result.Messages = " دسترسی برای برای برگشت هزینه بسته شده است";
                        result.IsSuccess = false;
                        return result;
                    }
                    long orderId = long.Parse(data.OrderId);
                    long refundAmount = data.TotalRefundAmount;
                    long saleOrderId = long.Parse(data.OrderId);
                    long saleReferenceId = long.Parse(data.TransactionCode);
                  
                    var bankMellatImplement = 
                        new BankMellatImplement(bankOption.Data.RefundTerminalId.Value,
                        bankOption.Data.RefundUserName, bankOption.Data.RefundPassword, 0, 0);


                    var resultRequest = bankMellatImplement.bpRefundRequest(orderId, saleReferenceId, refundAmount,
                 bankOption.Data.RefundTerminalId.Value, bankOption.Data.RefundUserName, bankOption.Data.RefundPassword);

                    if (resultRequest.ResultCode == "0")
                    {

                        await _refundService.UpdateRefund(refundId, refundAmount, citizenId, resultRequest.RefId);
                        result.IsSuccess = true;

                    }
                    else
                    {
                        result.Messages = resultRequest.Result;
                        result.IsSuccess = false;

                    }


                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages = res.Messages;
                }

            }
            catch (System.Exception er)
            {
                result.IsSuccess = false;
                result.Messages = "امکان برگشت هزینه در حال حاضر وجود ندارد لطفا شماره کارت خود را ثبت نمایید";

            }

            return result;

        }


        #endregion






        [HttpGet]
        [Route("SearchRefundUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedRefundAccessViewModel>> SearchRefundUser(
         int? offset = 1,
         int? count = 20,
         string name = null,
         string nationCode = null)
        {
            int citizenId = this._usersService.GetCurrentUserId();
            var  result = new ApiResult<PagedRefundAccessViewModel>(true, ApiResultStatusCode.Success, (PagedRefundAccessViewModel)null, "");
            bool flag = await  _citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return result;
            }

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;



            return   await this._group. SearchRefundUser(offset.Value, count.Value, 
                this._configuration.Value.RefundAccessGroupId, name, nationCode);
            
        }




        [HttpGet]
        [Route("GetAllRefunAccessUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllRefunAccessUsers()
        {
            return await this._group.GetAllRefunAccessUsers(this._configuration.Value.RefundAccessGroupId);

        }








        [HttpGet]
        [Route("DeleteRefundUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> DeleteRefundUser(string userCode)
        {
            int citizenId = this._usersService.GetCurrentUserId();
            ApiResult<string> result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            bool flag = await this._citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return (ApiResult)result;
            }
            GroupsCitizensDto model = new GroupsCitizensDto()
            {
                UserCode = userCode,
                GroupId = this._configuration.Value.RefundAccessGroupId
            };
            ApiResult<string> apiResult = await this._citzene.RemoveCitizenFromGroup(model, citizenId);
            return (ApiResult)apiResult;
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> AddRefundUser([FromBody] RefundUserRegister model)
        {
            int citizenId = this._usersService.GetCurrentUserId();
            ApiResult<string> result = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            bool flag = await this._citzene.CitizenMembershipInGroup(citizenId, this._configuration.Value.RefundAccessGroupId);
            if (!flag)
            {
                result.Messages = "شما به  استرداد هزینه  دسترسی ندارید";
                result.IsSuccess = false;
                return (ApiResult)result;
            }
            GroupsCitizensDto add = new GroupsCitizensDto()
            {
                GroupId = this._configuration.Value.RefundAccessGroupId,
                NationCode = model.NationCode
            };
            ApiResult<string> groupByNationCode = await this._citzene.AddCitizenToGroupByNationCode(add, citizenId);
            return (ApiResult)groupByNationCode;
        }
















    }
}