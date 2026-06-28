using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Utilities;
using Nikan.DataLayer.Context;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.CitizenCards;
using Nikan.Services.Citizens;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.CitizenCards;
using Nikan.ViewModel.Citizens;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CardInfoController : Controller
    {


        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene;
        private readonly IAppService _app;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IUnitOfWork _uow;
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ICardService _card;
        private readonly ICitizenCardService _citizencard;
        private readonly ICardInfoExportService _cardInfoExport;
        private readonly IPermissionService _permission;
        private readonly IDistributeCardService _distributeCardService;
        private readonly IDiscountCardService _discountCardService;
        private readonly IRequestFreeCardService _requestFreeCardService;




        public CardInfoController(
              IUsersService userManager,
              ICitizenService citzene,
               IDiscountCardService discountCardService,
               IRequestFreeCardService   requestFreeCardService,
               IDistributeCardService distributeCardService,
              ICardInfoExportService cardInfoExport,
              ICitizenCardService citizencard,
              ICitizenFeedbackService citizenFeedback,
              IAppService app,
               IPermissionService permission,
              IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IUnitOfWork uow,
            ICardService card,
            IAntiForgeryCookieService antiforgery


            )
        {
            _requestFreeCardService =  requestFreeCardService;
            _citzene = citzene;
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _app = app;
            _card = card;
            _cardInfoExport = cardInfoExport;
            _citizencard = citizencard;
            _uow = uow;
            _permission = permission;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _distributeCardService = distributeCardService;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
            _discountCardService = discountCardService;

        }
        #endregion




        #region کارت شهروندی



        [HttpGet]
        [Route("GetDisCountGroupBaseList")]
        public async Task<ApiResult<List<BaseDataModel>>> GetDisCountGroupBaseList()
        {
            var  countGroupBaseList = await this._card.GetDisCountGroupBaseList();
            return countGroupBaseList;
        }

         


        [HttpGet]
        [Route("CitizencardAdvSearch")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedCitizensCardViewModel>> CitizencardAdvSearch(
          int? offset = 1,
   int? count = 20,
   DateTime? FromDate = null,
   DateTime? ToDate = null,
   string name = null,
   string nationCode = null,
   string cardNumber = null,
   SabtStatusEnum? sabtStatus = null,
   PersonalPictureEnum? pictureConfirmed = null,
   MaritalStatusEnum? mariageStatus = null,
   int? cardTypeId = null,
   DeliverTypeEnum? deliverType = null,
   CardRequestStatusEnum? requestStatuse = null,
   int? discountGroupId = null, bool? hasCard = null, bool? hasdiscount = null,
int? groupId = null, bool? gender = null
        )
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.درخواست_کنندگان_کارت);
            if (!canAccess.IsSuccess)
            {
                canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_کارت_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<PagedCitizensCardViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به جستجوی کارت دسترسی ندارید");

            }


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20; 

            return await _citizencard.AdvancedSearch(offset.Value, count.Value, FromDate,
                           ToDate, name, nationCode, null, cardNumber,
                           sabtStatus, pictureConfirmed, mariageStatus, cardTypeId, deliverType, requestStatuse, discountGroupId, hasCard, hasdiscount, groupId, gender);


        }

        [HttpGet]
        [Route("CitizencardAdvSearch_Export")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ActionResult> CitizencardAdvSearch_Export(int? offset = 1,
   int? count = 20,
   DateTime? FromDate = null,
   DateTime? ToDate = null,
   string name = null,
   string nationCode = null,
   string cardNumber = null,
   SabtStatusEnum? sabtStatus = null,
   PersonalPictureEnum? pictureConfirmed = null,
   MaritalStatusEnum? mariageStatus = null,
   int? cardTypeId = null,
   DeliverTypeEnum? deliverType = null,
   CardRequestStatusEnum? requestStatuse = null,
   int? discountGroupId = null, bool? hasCard = null, bool? hasdiscount = null,int ? groupId = null, bool? gender = null
      )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.فایل_اکسل_خروجی_چاپ_کارت);
            if (!canAccess.IsSuccess)
            {
                return BadRequest(new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به خروجی اکسل دسترسی ندارید"));

            }
           
              canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.درخواست_کنندگان_کارت);
            if (!canAccess.IsSuccess)
            {
                canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_کارت_کارت);
                if (!canAccess.IsSuccess)
                    return BadRequest(new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به خروجی اکسل دسترسی ندارید"));

            }


            var getlist =await _citizencard.AdvancedSearch(offset.Value, count.Value, FromDate,
                           ToDate, name, nationCode, null, cardNumber,
                           sabtStatus, pictureConfirmed, mariageStatus, cardTypeId, deliverType, requestStatuse, discountGroupId, hasCard, hasdiscount, groupId, gender);




            if (getlist.IsSuccess != true)
                return BadRequest(getlist.Messages);

            //ثبت لاگ دریافت فایل اکسل


            var data = getlist.Data.Items;

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            for (int i = 0; i < 30; i++)
            {
                sheet.SetColumnWidth(i, 20 * 100);//row
            }
           
             
            //Create a header row
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ردیف");
            headerRow.CreateCell(1).SetCellValue("شماره کارت");
            headerRow.CreateCell(2).SetCellValue("شماره سریال");
            headerRow.CreateCell(3).SetCellValue("کد ملی");
            headerRow.CreateCell(4).SetCellValue("جنسیت");

            headerRow.CreateCell(5).SetCellValue("شهروند");
            headerRow.CreateCell(6).SetCellValue("نام");
            headerRow.CreateCell(7).SetCellValue("نام خانوادگی");
            headerRow.CreateCell(8).SetCellValue("شماره تماس");
            headerRow.CreateCell(9).SetCellValue("نوع تحویل");
            headerRow.CreateCell(10).SetCellValue("مرکز تحویل");
            headerRow.CreateCell(11).SetCellValue("منطقه ");
            headerRow.CreateCell(12).SetCellValue("خیابان  ");
            headerRow.CreateCell(13).SetCellValue("کوچه");
            headerRow.CreateCell(14).SetCellValue("پلاک");
            headerRow.CreateCell(15).SetCellValue("شماره تلفن");
            headerRow.CreateCell(16).SetCellValue("کد پستی");
            headerRow.CreateCell(17).SetCellValue(" آدرس کامل  ");
            headerRow.CreateCell(18).SetCellValue("عنوان کارت");
            headerRow.CreateCell(19).SetCellValue("تاریخ درخواست");


            headerRow.CreateCell(20).SetCellValue("دارای تخفیف؟");
            headerRow.CreateCell(21).SetCellValue("عنوان تخفیف");
            headerRow.CreateCell(22).SetCellValue("وضعیت کارت");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;
            DateTimeToPersianDateTimeConverter pdate = new DateTimeToPersianDateTimeConverter("-", false);

            foreach (var obj in data)
            {

                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(rowNumber);
                row.CreateCell(1).SetCellValue(obj.CardNumber);
                row.CreateCell(2).SetCellValue(obj.CardSerial); 
                row.CreateCell(3).SetCellValue(obj.NationCode);
                row.CreateCell(4).SetCellValue(obj.Gender);
                row.CreateCell(5).SetCellValue(obj.Citizen);
                row.CreateCell(6).SetCellValue(obj.FirstName);
                row.CreateCell(7).SetCellValue(obj.LastName);
                row.CreateCell(8).SetCellValue(obj.Mobile);
                row.CreateCell(9).SetCellValue(obj.DeliverType.ToString());
                row.CreateCell(10).SetCellValue(obj.DeliveringCenter);

                if (obj.Region != null)
                {
                    row.CreateCell(11).SetCellValue(obj.Region.Value);
                }
                else
                {
                    row.CreateCell(11).SetCellValue("");
                }

                row.CreateCell(12).SetCellValue(obj.Street);
                row.CreateCell(13).SetCellValue(obj.Alley);
                row.CreateCell(14).SetCellValue(obj.Plaque);
                row.CreateCell(15).SetCellValue(obj.Phone);
                row.CreateCell(16).SetCellValue(obj.PostalCode);
                row.CreateCell(17).SetCellValue(obj.FullAddress);
                row.CreateCell(18).SetCellValue(obj.CardTitle);
                row.CreateCell(19).SetCellValue(pdate.toShamsiDateTime(obj.RequestDate));

                row.CreateCell(20).SetCellValue(obj.HasDiscount.ToString());
                row.CreateCell(21).SetCellValue(obj.DiscountTitle);
                row.CreateCell(22).SetCellValue(obj.RequestStatuse.ToString());

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel", //MIME type of Excel files
             "ArticleList.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user



        }



        /// <summary>
        /// دریافت اطلاعات کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetCitizenCardInfoByCardId")]
        public async Task<ApiResult<CitizensCardInfo>> GetCitizenCardInfoByCardId(int id)
        {
            return await _citizencard.GetCitizenCardInfoByCardId(id);
        }



        /// <summary>
        /// برگشت کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> BackCard(BackCardDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citizencard.BackCard(model, userId);
        }

        /// <summary>
        /// تحویل کارت به شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> DeliveredCard(DeliveredCardDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citizencard.DeliveredCard(model, userId);
        }

        /// <summary>
        /// ابطال کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> CardCancellation(CardCancellationDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citizencard.CardCancellation(model, userId);
            //TODO ارسال پیامک
        }



        #endregion




        #region خروجی کارت جهت چاپ کارت

        /// <summary>
        /// دریافت لیست خروجی کارت ها جهت چاپ
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetPagedCardInfoExport")]
        public async Task<ApiResult<PagedCardInfoExport>> GetPagedCardInfoExport(int? offset = 1,
            int? count = 20,
            DateTime? FromDate = null, DateTime? ToDate = null)
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _cardInfoExport.GetPagedCardInfoExport(offset.Value, count.Value, FromDate, ToDate);

        }

        /// <summary>
        /// جزئیات کارتهای صادر شده در یک دوره
        /// </summary>
        /// <param name="exportId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="nationCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedCardInfoExportDetails")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedCitizensCardViewModel>> GetPagedCardInfoExportDetails(
        int exportId, int? offset = 1, int? count = 20,
        string name = null,
        string nationCode = null,
        string cardNumber = null,
        int? cardTypeId = null
        )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;


            return await _cardInfoExport.GetPagedCardInfoExportDetails(offset.Value, count.Value, exportId
               , name, nationCode, cardNumber, cardTypeId);


        }




        [HttpGet]
        [Route("GetPagedCardInfoExportDetails_Export")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ActionResult> GetPagedCardInfoExportDetails_Export(
       int exportId, int? offset = 1, int? count = 20,
       string name = null,
       string nationCode = null,
       string cardNumber = null,
       int? cardTypeId = null
       )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var userId = _usersService.GetCurrentUserId();
            var  canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.فایل_اکسل_خروجی_چاپ_کارت);
            if (!canAccess.IsSuccess)
            {
                return   BadRequest(new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به خروجی اکسل دسترسی ندارید"));

            }



            var getlist = await _cardInfoExport.GetPagedCardInfoExportDetails(offset.Value, count.Value, exportId
               , name, nationCode, cardNumber, cardTypeId);
            if (getlist.IsSuccess != true)
                return BadRequest(getlist.Messages);

            //ثبت لاگ دریافت فایل اکسل
           

            var data = getlist.Data.Items;
            
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            for (int i = 0; i < 25; i++)
            {
                sheet.SetColumnWidth(i, 20 * 100);//row
            }
           
             
          

            //Create a header row
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ردیف");
            headerRow.CreateCell(1).SetCellValue("شماره کارت");
            headerRow.CreateCell(2).SetCellValue("شماره سریال");
            headerRow.CreateCell(3).SetCellValue("کد ملی");
            headerRow.CreateCell(4).SetCellValue("شهروند");
            headerRow.CreateCell(5).SetCellValue("نام");
            headerRow.CreateCell(6).SetCellValue("نام خانوادگی");
            headerRow.CreateCell(7).SetCellValue("جنسیت");
            headerRow.CreateCell(8).SetCellValue("شماره تماس");
            headerRow.CreateCell(9).SetCellValue("نوع تحویل");
            headerRow.CreateCell(10).SetCellValue("مرکز تحویل");
            headerRow.CreateCell(11).SetCellValue("منطقه ");
            headerRow.CreateCell(12).SetCellValue("خیابان  ");
            headerRow.CreateCell(13).SetCellValue("کوچه");
            headerRow.CreateCell(14).SetCellValue("پلاک");
            headerRow.CreateCell(15).SetCellValue("شماره تلفن");
            headerRow.CreateCell(16).SetCellValue("کد پستی");
            headerRow.CreateCell(17).SetCellValue(" آدرس کامل  ");
            headerRow.CreateCell(18).SetCellValue("عنوان کارت");
            headerRow.CreateCell(19).SetCellValue("تاریخ درخواست");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;
            DateTimeToPersianDateTimeConverter pdate = new DateTimeToPersianDateTimeConverter("-", false);

            foreach (var obj in data)
            {

                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(rowNumber);
                row.CreateCell(1).SetCellValue(obj.CardNumber);
                row.CreateCell(2).SetCellValue(obj.CardSerial);
                row.CreateCell(3).SetCellValue(obj.NationCode);
                row.CreateCell(4).SetCellValue(obj.Citizen);
                row.CreateCell(5).SetCellValue(obj.FirstName); 
                row.CreateCell(6).SetCellValue(obj.LastName);
                row.CreateCell(7).SetCellValue(obj.Gender);
                row.CreateCell(8).SetCellValue(obj.Mobile);
                row.CreateCell(9).SetCellValue(obj.DeliverType.ToString());
                row.CreateCell(10).SetCellValue(obj.DeliveringCenter);
               
                if (obj.Region != null)
                {
                    row.CreateCell(11).SetCellValue(obj.Region.Value);
                }
                else
                {
                    row.CreateCell(11).SetCellValue("");
                }

                row.CreateCell(12).SetCellValue(obj.Street);
                row.CreateCell(13).SetCellValue(obj.Alley);
                row.CreateCell(14).SetCellValue(obj.Plaque);
                row.CreateCell(15).SetCellValue(obj.Phone);
                row.CreateCell(16).SetCellValue(obj.PostalCode);
                row.CreateCell(17).SetCellValue(obj.FullAddress);
                row.CreateCell(18).SetCellValue(obj.CardTitle); 
                row.CreateCell(19).SetCellValue(pdate.toShamsiDateTime(obj.RequestDate));



            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel", //MIME type of Excel files
             "ArticleList.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user



        }







        /// <summary>
        /// فرمت خروجی جهت  چاپ کارت
        /// </summary>
        /// <param name="exportId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="nationCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedShahrvandiCardInfoExportDetailsForSend")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedShahrvandiCardInfoExportDetailsForSend(
        int exportId, int? offset = 1, int? count = 2000,
        string name = null,
        string nationCode = null,
        string cardNumber = null,
        int? cardTypeId = null
        )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 2000;


            return await _cardInfoExport.GetPagedShahrvandiCardInfoExportDetailsForSend(offset.Value, count.Value, exportId
               , name, nationCode, cardNumber, cardTypeId);


        }







        /// <summary>
        /// فرمت خروجی جهت  چاپ کارت
        /// </summary>
        /// <param name="exportId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="nationCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedManzalatCardInfoExportDetailsForSend")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedManzalatCardInfoExportDetailsForSend(
        int exportId, int? offset = 1, int? count = 2000,
        string name = null,
        string nationCode = null,
        string cardNumber = null,
        int? cardTypeId = null
        )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 2000;


            return await _cardInfoExport.GetPagedManzalatCardInfoExportDetailsForSend(offset.Value, count.Value, exportId
               , name, nationCode, cardNumber, cardTypeId);


        }














        /// <summary>
        /// خروجی صدور کارت جدید
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> NewExportCard(NewExportCard model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citizencard.NewExportCard(model, userId);
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetExportCardPicture")]
        public async Task<ApiResult> GetExportCardPicture(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "فایل زیپ تصاویر با موفقیت ایجاد گردید");
            var getlist = await _cardInfoExport.GetExportDetailsList(id);
            if (getlist.IsSuccess)
            {
                string exportPath = "~/Uploads/Resources/Exports/CitizenPictures/";
                exportPath += "CitizenPictures_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");


                var notfound = new List<string>();
                var list = getlist.Data;
                int countFile = 0;
                foreach (var card in list)
                {
                    var citizenId = card.CitizenId;
                    var natioCode = card.NationCode;
                    var vFileName = natioCode + ".jpg";

                    var path = "/uploads/Resources/Citizens/" + citizenId + $@"/{ vFileName}";
                    var topath = exportPath + $@"/{ vFileName}";
                    string FilePath = _environment.WebRootPath + path;
                    string toFilePath = _environment.WebRootPath + topath;
                    if (System.IO.File.Exists(FilePath))
                    {
                        countFile++;
                        System.IO.File.Copy(FilePath, toFilePath, true);

                    }
                    else
                    {
                        notfound.Add(natioCode);
                    }

                }
                if (countFile > 0)
                {

                    string zipPath = exportPath + "/result.zip";
                    ZipFile.CreateFromDirectory(exportPath, zipPath);
                }


            }


            return res;

        }


        /// <summary>
        /// ارسال برای چاپ کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("SendToPrint")]
        public async Task<ApiResult> SendToPrint(int id)
        {
            return await _cardInfoExport.SendCardToPrint(id);
        }


        [HttpGet]
        [Route("RemoveExportCard")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<string>> RemoveExportCard(int id)
        {
            return await _cardInfoExport.RemoveExport(id);

        }


        /// <summary>
        /// حذف کارت در خروجی کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveCardInExportList")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<string>> RemoveCardInExportList(int id)
        {
            return await _cardInfoExport.RemoveCardInExportList(id);

        }





        [HttpPost]
        [Route("ImportExcelFileCardNumber")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<UploadFileResult>> ImportExcelFileCardNumber([FromForm] ImportExcelFileCardNumber model)
        {

            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            if (model.ExportId == 0)
            {
                res.IsSuccess = false;
                res.Messages = "شناسه فایل را مشخص نمایید";
                res.StatusCode = ApiResultStatusCode.ServerError;
                return res;
            } 

            try
            {
                var file = model.file;
                var fileName = "";
                string folderName = "ImportCardNumber";
                var ticks = DateTime.Now.Ticks;
                var webRootPath = _environment.WebRootPath + "/uploads/Resources/ImportCardNumber/" + ticks;

                string newPath = Path.Combine(webRootPath, folderName);
                var list = new List<ImportExcelCardNumber>();
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


                                var cell1 = row.GetCell(0);//ActivationDate
                                var cell2 = row.GetCell(1);//ExpiredDate
                                var cell3 = row.GetCell(2);//DCNum
                                var cell4 = row.GetCell(3);//cardSerial
                                var cell5 = row.GetCell(4);//NationalCode
                                var cell6 = row.GetCell(5);// Cancellation

                                if (cell3 != null && cell5 != null)
                                {
                                    var item = new ImportExcelCardNumber();

                                    var activationDate = DateTime.Now;//cell1
                                    var expiredDate = DateTime.Now;//cell2
                                    var dCNum = "";//cell3
                                    var cardSerial = "";  //cell4
                                    var nationalCode = "";//cell5
                                    var cancellation = "";//cell6



                                    if (cell1 != null)
                                    {
                                        DateTime.TryParse(cell1.ToString(), out activationDate);
                                    }
                                    if (cell2 != null)
                                    {
                                        var isdate = DateTime.TryParse(cell2.ToString(), out expiredDate);
                                        if (isdate) item.CardExpirationDate = expiredDate;
                                    }

                                    if (cell3 != null)
                                        dCNum = cell3.ToString();

                                    if (cell4 != null)
                                        cardSerial = cell4.ToString();

                                    if (cell5 != null)
                                        nationalCode = cell5.ToString();

                                    if (cell6 != null)
                                        cancellation = cell6.ToString();



                                    item.CardActivationDate = activationDate;

                                    item.CardNumber = dCNum.Trim();
                                    item.NationCode = nationalCode.Trim();
                                    item.CardSerial = cardSerial.Trim();
                                    item.CardCancellation = !string.IsNullOrWhiteSpace(cancellation);

                                    list.Add(item);




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
                    var userId = _usersService.GetCurrentUserId();// 
                    var addfile = await _cardInfoExport.ImportExcelCardNumber(list, model.ExportId, userId);
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
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "هیچ رکوردی ثبت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
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










        #endregion 




        /// <summary>
        /// دریافت لیست کارت های شهروند
        /// [Card1]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetCitizenCardInfo")]
        public async Task<ApiResult<List<CitizensCardInfo>>> GetCitizenCardInfo()
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _citizencard.GetCitizenCardInfo(citizenId);

        }

        #region مشخصات نوع کارت

        /// <summary>
        /// مشخصات نوع کارت 
        /// لیست پایه
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCardTypeBaseList")]
        public async Task<ApiResult<List<BaseDataModel>>> GetCardTypeBaseList()
        {
            return await _card.GetCardTypeBaseList();

        }
        [HttpGet]
        [Route("GetActiveCardTypeBaseList")]
        public async Task<ApiResult<List<BaseDataModel>>> GetActiveCardTypeBaseList()
        {
            return await _card.GetCardTypeBaseList(true);

        }
        #endregion 
        #region تعریف کارت شهروندی




        /// <summary>
        /// نمایش اطلاعات یک کارت
        /// </summary>
        /// <param name="cardInfoId">شناسه کارت</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCardInfo")]
        [AllowAnonymous]
        public async Task<ApiResult<CardInfoViewModel>> GetCardInfo(string cardInfoId)
        {
            return await _card.GetCardInfo(cardInfoId);
        }



        /// <summary>
        /// دریافت لیست کارت ها به صورت صفحه بندی شده
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cardTypeId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetPagedCardInfo")]
        public async Task<ApiResult<PagedCardInfoViewModel>> GetPagedCardInfo(int? offset = 1,
            int? count = 20, int? cardTypeId = null,
            DateTime? FromDate = null, DateTime? ToDate = null)
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _card.GetPagedCardInfo(offset.Value, count.Value, cardTypeId, FromDate, ToDate);

        }




        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<CardInfoDto>> AddOrUpdateCard(CardInfoDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _card.AddOrUpdate(model, userId);
        }

        #endregion


        #region لیست دوره های توزیع کارت

        /// <summary>
        /// افزودن دوره توزیع کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<CardInfo_DistributeCard_CoursesDto>> AddCardCourses(CardInfo_DistributeCard_CoursesDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _distributeCardService.AddCardCourses(model, userId);
        }





        /// <summary>
        /// بستن دوره توزیع کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("CloseCardCourses")]
        public async Task<ApiResult<string>> CloseCardCourses(int id)
        {
            return await _distributeCardService.CloseCardCourses(id);
        }



        /// <summary>
        /// حذف دوره توزیع کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveCardCourses")]
        public async Task<ApiResult<string>> RemoveCardCourses(int id)
        {
            return await _distributeCardService.RemoveCardCourses(id);
        }


        /// <summary>
        /// لیست دوره های توزیع کارت
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="courseNumber">شماره دوره</param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetPagedeCardCourses")]
        public async Task<ApiResult<PagedCardInfoDistributeCardCoursesViewModel>> GetPagedeCardCourses(
            int? offset = 0,
            int? count = 20,
             int? courseNumber = null,
            DateTime? FromDate = null, DateTime? ToDate = null)
        {

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _distributeCardService.GetPagedeCardCourses(offset.Value, count.Value, courseNumber, FromDate, ToDate);

        }



        /// <summary>
        /// مشخصات صف های داخل دوره توزیع کارت
        /// </summary>
        /// <param name="courseId">دوره توزیع کارت</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetQueueListInCourse")]
        public async Task<ApiResult<List<DistributeCardQueueShortInfoViewModel>>> GetQueueListInCourse(int courseId)
        {
            return await _distributeCardService.GetQueueListInCourse(courseId);
        }



        #endregion


        #region مدیریت صف توزیع کارت


        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetPagedeDistributionQueue")]
        public async Task<ApiResult<PagedCardInfoDistributeCardQueue>> GetPagedeDistributionQueue(int courseId, int? offset = 0,
            int? count = 20,
            string name = null)
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _distributeCardService.GetPagedeDistributionQueue(offset.Value, count.Value,
                courseId, name

                );
        }




        /// <summary>
        /// تغییر وضعیت قفل کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("ChangeLockDistributionQueue")]
        public async Task<ApiResult<string>> ChangeLockDistributionQueue(int id)
        {
            return await _distributeCardService.ChangeLockDistributionQueue(id);
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetDistributionQueueInfo")]
        public async Task<ApiResult<CardInfo_DistributeCard_QueueInfoViewModel>> GetDistributionQueueInfo(int id)
        {
            return await _distributeCardService.GetDistributionQueueInfo(id);
        }




        /// <summary>
        /// اضافه کردن صف توزیع کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<CardDistributionQueueDto>> AddCardDistributionQueue(CardDistributionQueueDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _distributeCardService.AddCardDistributionQueue(model, userId);
        }

        /// <summary>
        /// ویرایش صف توزیع کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<CardDistributionQueueDto>> UpdateCardDistributionQueue(CardDistributionQueueDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _distributeCardService.UpdateCardDistributionQueue(model, userId);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveCardQueue")]
        public async Task<ApiResult<string>> RemoveCardQueue(int id)
        {
            return await _distributeCardService.RemoveQueue(id);
        }

        /// <summary>
        /// جستجوی کارت در صف های توزیع کارت
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="nationCode"></param>
        /// <param name="courseId"></param>
        /// <param name="courseNumber"></param>
        /// <param name="queueId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="cardTypeId"></param>
        /// <param name="queueInputType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchCardInQueue")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedCitizenCardsInQueueInfo>> SearchCardInQueue(
        int? offset = 1, int? count = 20,
       string name = null,
       string nationCode = null,
       int? courseId = null,
       int? courseNumber = null,
       int? queueId = null,
       string cardNumber = null,
       int? cardTypeId = null,
       QueueInputTypeEnum? queueInputType = null

       )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;


            return await _distributeCardService.SearchCardInQueue(offset.Value, count.Value,
                name, nationCode, courseId, courseNumber, queueId, cardNumber,
                           cardTypeId, queueInputType);


        }

        [HttpGet]
        [Route("SearchCardInQueue_Export")]
        [Authorize(Roles = "Admin,Card")]
        public   async Task<ActionResult> SearchCardInQueue_Export(
        int? offset = 1, int? count = 20,
       string name = null,
       string nationCode = null,
       int? courseId = null,
       int? courseNumber = null,
       int? queueId = null,
       string cardNumber = null,
       int? cardTypeId = null,
       QueueInputTypeEnum? queueInputType = null

       )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.فایل_اکسل_خروجی_چاپ_کارت);
            if (!canAccess.IsSuccess)
            {
                return BadRequest(new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به خروجی اکسل دسترسی ندارید"));

            }


            var getlist= await _distributeCardService.SearchCardInQueue(offset.Value, count.Value,
                name, nationCode, courseId, courseNumber, queueId, cardNumber,
                           cardTypeId, queueInputType);

             


            if (getlist.IsSuccess != true)
                return BadRequest(getlist.Messages);

            //ثبت لاگ دریافت فایل اکسل


            var data = getlist.Data.Items;

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 100);//row
            sheet.SetColumnWidth(1, 20 * 100);//CardNumber
            sheet.SetColumnWidth(2, 20 * 100);//CardSerial
            sheet.SetColumnWidth(3, 20 * 100);//NationCode
            sheet.SetColumnWidth(4, 20 * 100);//Citizen
            sheet.SetColumnWidth(5, 20 * 100);//FirstName
            sheet.SetColumnWidth(6, 20 * 100);//LastName 
            sheet.SetColumnWidth(7, 20 * 100);//Mobile 
            sheet.SetColumnWidth(8, 20 * 100);//DeliverType  
            sheet.SetColumnWidth(9, 20 * 100);//DeliveringCenter
            sheet.SetColumnWidth(10, 20 * 100);//Region
            sheet.SetColumnWidth(11, 20 * 100);//Street 
            sheet.SetColumnWidth(12, 20 * 100);//Alley
            sheet.SetColumnWidth(13, 20 * 100);//Plaque 
            sheet.SetColumnWidth(14, 20 * 100);//Phone
            sheet.SetColumnWidth(15, 20 * 100);//PostalCode
            sheet.SetColumnWidth(16, 20 * 400);// FullAddress
            sheet.SetColumnWidth(17, 20 * 100);//CardTitle
            sheet.SetColumnWidth(18, 20 * 100);//RequestDate
            sheet.SetColumnWidth(19, 20 * 100);//QueueName

            //Create a header row
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ردیف");
            headerRow.CreateCell(1).SetCellValue("شماره کارت");
            headerRow.CreateCell(2).SetCellValue("شماره سریال");
            headerRow.CreateCell(3).SetCellValue("کد ملی");
            headerRow.CreateCell(4).SetCellValue("شهروند");
            headerRow.CreateCell(5).SetCellValue("نام");
            headerRow.CreateCell(6).SetCellValue("نام خانوادگی");
            headerRow.CreateCell(7).SetCellValue("شماره تماس");
            headerRow.CreateCell(8).SetCellValue("نوع تحویل");
            headerRow.CreateCell(9).SetCellValue("مرکز تحویل");
            headerRow.CreateCell(10).SetCellValue("منطقه ");
            headerRow.CreateCell(11).SetCellValue("خیابان  ");
            headerRow.CreateCell(12).SetCellValue("کوچه");
            headerRow.CreateCell(13).SetCellValue("پلاک");
            headerRow.CreateCell(14).SetCellValue("شماره تلفن");
            headerRow.CreateCell(15).SetCellValue("کد پستی");
            headerRow.CreateCell(16).SetCellValue(" آدرس کامل  ");
            headerRow.CreateCell(17).SetCellValue("عنوان کارت");
            headerRow.CreateCell(18).SetCellValue("تاریخ درخواست");
            headerRow.CreateCell(19).SetCellValue("صف");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;
            DateTimeToPersianDateTimeConverter pdate = new DateTimeToPersianDateTimeConverter("-", false);

            foreach (var obj in data)
            {

                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(rowNumber);
                row.CreateCell(1).SetCellValue(obj.CardNumber);
                row.CreateCell(2).SetCellValue(obj.CardSerial);
                row.CreateCell(3).SetCellValue(obj.NationCode);
                row.CreateCell(4).SetCellValue(obj.Citizen);
                row.CreateCell(5).SetCellValue(obj.FirstName);
                row.CreateCell(6).SetCellValue(obj.LastName);
                row.CreateCell(7).SetCellValue(obj.Mobile);
                row.CreateCell(8).SetCellValue(obj.DeliverType.ToString());
                row.CreateCell(9).SetCellValue(obj.DeliveringCenter);

                if (obj.Region != null)
                {
                    row.CreateCell(10).SetCellValue(obj.Region.Value);
                }
                else
                {
                    row.CreateCell(10).SetCellValue("");
                }

                row.CreateCell(11).SetCellValue(obj.Street);
                row.CreateCell(12).SetCellValue(obj.Alley);
                row.CreateCell(13).SetCellValue(obj.Plaque);
                row.CreateCell(14).SetCellValue(obj.Phone);
                row.CreateCell(15).SetCellValue(obj.PostalCode);
                row.CreateCell(16).SetCellValue(obj.FullAddress);
                row.CreateCell(17).SetCellValue(obj.CardTitle);
                row.CreateCell(18).SetCellValue(pdate.toShamsiDateTime(obj.RequestDate));
                row.CreateCell(19).SetCellValue(obj.QueueName);


            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel", //MIME type of Excel files
             "ArticleList.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user













        }



        /// <summary>
        /// تحویل صف به اپراتور
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> DeliveryQueueToOperator(DeliveryQueueToOperatorViewModel model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _distributeCardService.DeliveryQueueToOperator(model);
        }



        /// <summary>
        /// جستجوی کارت برای توزیع در صف
        /// </summary>
        /// <param name="searchCitizensType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("SearchCardForQueue")]
        public async Task<ApiResult<CitizensCardInfo>> SearchCardForQueue(SearchCitizensTypeEnum? searchCitizensType, string value)
        {
            return await _distributeCardService.SearchCardForQueue(new MiniSearchCitizensViewModel() { SearchCitizensType = searchCitizensType, Value = value });
        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PrintqueueInfoViewModel>> SendCardToQueue(SendCardToQueueDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            if (model.DeliverType == 1)
            {
                //تحویل پستی
                return await _distributeCardService.AddCardToPostQueue(userId, model.CitizenId, model.CardTypeId, model.Id, model.CourseId);
            }
            else
            {

                return await _distributeCardService.AddCardToDeliveredQueue(model.Id, model.DeliveringCenterId, model.CourseId, userId);
            }

        }



        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveCardFromQueue")]
        public async Task<ApiResult<string>> RemoveCardFromQueue(int id)
        {
            return await _distributeCardService.RemoveCardFromQueue(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveCardFromQueueByCourseId")]
        public async Task<ApiResult<string>> RemoveCardFromQueueByCourseId(int courseId, int cardId)
        {
            return await _distributeCardService.RemoveCardFromQueueByCourseId(courseId, cardId);
        }
        #endregion









     


        #region مدیریت تخفیف کارت

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> AddCardDiscount(CardInfo_DiscountDto model)
        {

            var userId = _usersService.GetCurrentUserId();
            return await _discountCardService.Add(model, userId);
        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> UpdateCardDiscount(CardInfo_DiscountDto model)
        {

            var userId = _usersService.GetCurrentUserId();
            return await _discountCardService.Update(model, userId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("ChangeDiscountCenterState")]
        public async Task<ApiResult> ChangeDiscountCenterState(int id)
        {
            return await _discountCardService.ChangeDiscountCenterState(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("ChangeDiscountGroupState")]
        public async Task<ApiResult> ChangeDiscountGroupState(int id)
        {
            return await _discountCardService.ChangeDiscountGroupState(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("ChangeDiscountState")]
        public async Task<ApiResult> ChangeDiscountState(int id)
        {
            return await _discountCardService.ChangeDiscountState(id);
        }

        /// <summary>
        /// اضافه کردن مرکز تحویل کارت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> AddDiscountCenter(AddDiscountCenterDto model)
        {
            return await _discountCardService.AddCenter(model);
        }

        /// <summary>
        /// حذف تخفیف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveDiscount")]
        public async Task<ApiResult> RemoveDiscount(int id)
        {
            return await _discountCardService.Remove(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveDisCountItem")]
        public async Task<ApiResult> RemoveDisCountItem(int id)
        {
            return await _discountCardService.RemoveDisCountItem(id);
        }






        /// <summary>
        /// تخفیفات تعریف شده برای کارت
        /// </summary>
        /// <param name="cardTypeId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("PagedDiscountCardList")]
        public async Task<ApiResult<PagedDiscountCardList>> PagedDiscountCardList(int cardTypeId, int? offset = 0, int? count = 20)
        {

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _discountCardService.PagedDiscountCardList(cardTypeId, offset.Value, count.Value);
        }


        /// <summary>
        ///  دریافت جزئیات یک تخفیف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetCardDiscountInfo")]
        public async Task<ApiResult<CardInfo_DiscountViewModel>> GetCardDiscountInfo(int id)
        {
            return await _discountCardService.GetDiscount(id);
        }



        #endregion
        #region ویرایش ادرس تحویل کارت
        /// <summary>
        /// ویرایش ادرس تحویل کارت توسط شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<AddressDto>> UpdateCitizenCardAddressByCitizen(AddressDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citizencard.UpdateCitizenCardAddress(model, userId);
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<AddressDto>> UpdateCitizenCardAddress(AddressDto model, int citizenId)
        {
            return await _citizencard.UpdateCitizenCardAddress(model, citizenId);
        }

        #endregion


        #region صدور کارت رایگان

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> AddRequestFreeCard(RequestFreeCardDto model)
        {
            

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ثبت_درخواست_کارت_رایگان_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به ثبت درخواست کارت رایگان دسترسی ندارید");






            return await _requestFreeCardService.Add(model, userId);
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> UpdateRequestFreeCard(RequestFreeCardDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ثبت_درخواست_کارت_رایگان_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به ویرایش درخواست کارت رایگان دسترسی ندارید");


            return await _requestFreeCardService.Update(model, userId);
        }



        [HttpGet]
        [Route("PagedRequestFreeCardLsit")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedRequestFreeCard>> PagedRequestFreeCardLsit( int? offset = 1, int? count = 20,int? cardTypeId = null, string discountTitle = null, int? groupId = null, string letterNumber = null)
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20; 
            return await _requestFreeCardService.PagedRequestFreeCardLsit(offset.Value, count.Value,cardTypeId, discountTitle, groupId, letterNumber );


        }

        /// <summary>
        /// جزئیات یک درخواست
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <param name="nationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRequestFreeCardCitizens")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedRequestFreeCardCitizens>> GetRequestFreeCardCitizens(string id , int? offset = 1, int? count = 20,
          string nationCode = null)
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _requestFreeCardService.GetRequestFreeCardCitizens(offset.Value, count.Value, id, nationCode);


        }















        /// <summary>
        /// حذف درخواست کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("RemoveRequestFreeCard")]
        public async Task<ApiResult> RemoveRequestFreeCard(string id)
        {
            return await _requestFreeCardService.RemoveRequestFreeCard(id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetRequestFreeCard")]
        public async Task<ApiResult<RequestFreeCardInfoViewModel>> GetRequestFreeCard(string id)
        {
            return await _requestFreeCardService.GetRequestFreeCard(id);
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("AcceptedRequestFreeCard")]
        public async Task<ApiResult> AcceptedRequestFreeCard(string id)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.تایید_درخواست_کارت_رایگان_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به تایید درخواست کارت رایگان دسترسی ندارید");


            return await _requestFreeCardService.AcceptedRequestFreeCard(id, userId);
        }













        #endregion 
















    }
}