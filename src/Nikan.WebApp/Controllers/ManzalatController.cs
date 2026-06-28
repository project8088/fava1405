using cle.Services;
using cle.Services.Citizens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Utilities;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.Events;
using Nikan.Services.Permissions;
using Nikan.ViewModel.BsseEntity;
using Nikan.ViewModel.Citizens;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{
    /// <summary>
    ///عضویت در طرح منزلت
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
   // [ApiExplorerSettings(IgnoreApi = true)]
    public class ManzalatController : Controller
    {
        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene;
        private readonly IManzalatService _manzalat;
        private readonly ITokenStoreService _tokenStoreService;
     
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICitizenSummaryEducationServices _educationService;
        private readonly IPermissionService _permission;
        private readonly IEventService _event;



        public ManzalatController(
              IUsersService userManager,
              ICitizenService citzene,
               IPermissionService permission,
              ICitizenFeedbackService citizenFeedback,
             IManzalatService  manzalat,
        ICitizenSummaryEducationServices educationService,
              IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService, 
            ISiteSettingService siteSettingService,
              IEventService siteevent,
            IAntiForgeryCookieService antiforgery


            )
        {
            _permission = permission;
            _citzene = citzene;
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _manzalat = manzalat;
            _siteSettingService = siteSettingService; 
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _event = siteevent;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _educationService = educationService;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));

        }
        #endregion


        #region فرم طرح منزلت
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult>  UpdateManzalatBaseForm(ManzalatBaseFormDto model)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.تنظیمات_منزلت);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");
            }

            return await _manzalat.UpdateManzalatBaseForm(model);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("ConfigManzalatForm")]
        public async Task<ApiResult> ConfigManzalatForm()
        { 
            return await _manzalat.ConfigManzalatForm();

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetManzalatBaseForm")]
        public async Task<ApiResult<ManzalatBaseFormDto>> GetManzalatBaseForm(int id)
        {
            return await _manzalat.GetManzalatBaseForm(id);

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetManzalatBaseForms")]
        public async Task<ApiResult<List<ManzalatBaseFormDto>>> GetManzalatBaseForms()
        {
            return await _manzalat.GetManzalatBaseForms();

        }


        #endregion





        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetCitizenManzalat")]
        public async Task<ApiResult<ManzalatDto>> GetCitizenManzalat(int formBaseId)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _manzalat.GetCitizenManzalat(userId,formBaseId);

        }




        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> AddOrUpdateManzalat(ManzalatDto model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _manzalat.AddOrUpdateManzalat (model, userId);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Remove")]
        public async Task<ApiResult<string>> RemoveManzalt(int id)
        {
            return await _manzalat.Remove(id);

        }

        


        /// <summary>
        /// دریافت طرح های منزلت جهت ثبت نام شهروند
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetAllAvailableManzaltForm")]
        public async Task<ApiResult<AvailableManzaltFormsAndAddress>> GetAllAvailableManzaltForm()
        {
           
            var  userId = _usersService.GetCurrentUserId();
            return await _manzalat.GetAllAvailableManzaltForm(userId );

        }




 
        [HttpGet]
        [Route("SearchManzaltCitizens")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedManzalatCitizenViewModel>> SearchManzaltCitizens(
            int? offset = 1, int? count = 20,
             DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
     DateTime? birthDateToDate = null,
     bool? gender = null,
     SabtStatusEnum? sabtStatus = null,
    MaritalStatusEnum? mariageStatus = null,
          string name = null,
          string nationCode = null,
           ManzalatFormStatuseEnum? formStatuse = null,
           ManzalatFormTypeEnum? manzalatFormType = null, bool? inManzalatGroups = null,
           bool? hasCard = null 
            )
        {



            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.درخواست_کنندگان_منزلت);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<PagedManzalatCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _manzalat.SearchManzaltCitizens(offset.Value, count.Value, FromDate, ToDate,
            birthDateFromDate, birthDateToDate, gender, sabtStatus, mariageStatus, name, nationCode, 
            formStatuse, manzalatFormType, inManzalatGroups,null, hasCard);

        }





        [HttpGet]
        [Route("SearchManzaltCitizens_Export")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SearchManzaltCitizens_Export(
            int? offset = 1, int? count = 20,
             DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
     DateTime? birthDateToDate = null,
     bool? gender = null,
     SabtStatusEnum? sabtStatus = null,
    MaritalStatusEnum? mariageStatus = null,
          string name = null,
          string nationCode = null,
           ManzalatFormStatuseEnum? formStatuse = null,
           ManzalatFormTypeEnum? manzalatFormType = null, bool? inManzalatGroups = null,
           bool? hasCard = null
            )
        {



            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.درخواست_کنندگان_منزلت);
            if (!canAccess.IsSuccess)
                return BadRequest("شما به این قسمت دسترسی ندارید");
 
            canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.دریافت_خروجی_اکسل_شهروندان);
            if (!canAccess.IsSuccess)
                return BadRequest("شما به خروجی اکسل دسترسی ندارید");





            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var getlist = await _manzalat.SearchManzaltCitizens(offset.Value, count.Value, FromDate, ToDate,
            birthDateFromDate, birthDateToDate, gender, sabtStatus, mariageStatus, name, nationCode,
            formStatuse, manzalatFormType, inManzalatGroups, null, hasCard);

            var errorLine = 0;

            if (getlist.IsSuccess != true)
                return BadRequest(getlist.Messages);

            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            await _event.AddEvent(new ViewModel.Events.EventDto()
            {
                ActionName = "SearchManzaltCitizens_Export",
                Code = 0,
                Description = "دریافت خروجی اکسل از اطلاعات منزلت",
                EventPriority = EventPriorityEnum.Important,
                EventSection = EventSectionEnum.خروجی_اکسل_شهروندان,
                EventType = EventTypeEnum.Warning,
                StrCode = "",
                UserId = userId,
                OperationId = userId,
                UserIp = userIp.ToString()

            });





            //ثبت لاگ دریافت فایل اکسل
            try
            {
                var data = getlist.Data.Citizens;

                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();

                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 20 * 100);//row
                sheet.SetColumnWidth(1, 20 * 100);//Gender
                sheet.SetColumnWidth(2, 20 * 100);//NationCode
                sheet.SetColumnWidth(3, 20 * 100);//FirstName
                sheet.SetColumnWidth(4, 20 * 100);//LastName
                sheet.SetColumnWidth(5, 20 * 100);//FatherName
                sheet.SetColumnWidth(6, 20 * 100);//BirthDate 
                sheet.SetColumnWidth(7, 20 * 100);//Age 
                sheet.SetColumnWidth(8, 20 * 100);//MobileNumber  
                sheet.SetColumnWidth(9, 20 * 100);//CreationDate
                sheet.SetColumnWidth(10, 20 * 100);//RegisterByService
                sheet.SetColumnWidth(11, 20 * 100);//SabtStatus 
                sheet.SetColumnWidth(12, 20 * 100);//PersonalPicture_Confirmed
                sheet.SetColumnWidth(13, 20 * 100);// Nationality 
                sheet.SetColumnWidth(14, 20 * 100);//Groups
                sheet.SetColumnWidth(15, 20 * 100);//FullAddress
                sheet.SetColumnWidth(16, 20 * 400);// PostalCode
                sheet.SetColumnWidth(17, 20 * 100);//LastUpdateOnDate


                sheet.SetColumnWidth(18, 20 * 100);// 
                sheet.SetColumnWidth(19, 20 * 400);//  



                sheet.SetColumnWidth(20, 20 * 100);// 
                sheet.SetColumnWidth(21, 20 * 100);//  
                sheet.SetColumnWidth(22, 20 * 100);// 
                sheet.SetColumnWidth(23, 20 * 100);//   
                sheet.SetColumnWidth(24, 20 * 100);// 
                sheet.SetColumnWidth(25, 20 * 100);// 
                sheet.SetColumnWidth(26, 20 * 400);//  
                sheet.SetColumnWidth(27, 20 * 100);//  
                sheet.SetColumnWidth(28, 20 * 100);// 
                sheet.SetColumnWidth(29, 20 * 400);//  



                sheet.SetColumnWidth(30, 20 * 100);// 
                sheet.SetColumnWidth(31, 20 * 100);//  
                sheet.SetColumnWidth(32, 20 * 100);// 
                sheet.SetColumnWidth(33, 20 * 100);//   
                sheet.SetColumnWidth(34, 20 * 100);// 
                 



                //Create a header row
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ردیف");
                headerRow.CreateCell(1).SetCellValue("جنسیت");
                headerRow.CreateCell(2).SetCellValue("کد ملی");
                headerRow.CreateCell(3).SetCellValue("نام");
                headerRow.CreateCell(4).SetCellValue("نام خانوادگی");
                headerRow.CreateCell(5).SetCellValue("نام پدر");
                headerRow.CreateCell(6).SetCellValue("تاریخ تولد");
                headerRow.CreateCell(7).SetCellValue("سن");
                headerRow.CreateCell(8).SetCellValue("-----");
                headerRow.CreateCell(9).SetCellValue("تاریخ ثبت نام");
                headerRow.CreateCell(10).SetCellValue("ثبت نام در سرویس");
                headerRow.CreateCell(11).SetCellValue("وضعیت ثبت احوال ");
                headerRow.CreateCell(12).SetCellValue("وضعیت تایید تصویر  ");
                headerRow.CreateCell(13).SetCellValue("ملیت");
                headerRow.CreateCell(14).SetCellValue("گروههای شهروندی");
                headerRow.CreateCell(15).SetCellValue("آدرس کامل");
                headerRow.CreateCell(16).SetCellValue("کد پستی");
                headerRow.CreateCell(17).SetCellValue(" تاریخ ثبت نام شهروند  "); 
                headerRow.CreateCell(18).SetCellValue("  وضعیت فرم  ");
                headerRow.CreateCell(19).SetCellValue("  تایید شدگان طرح منزلت  "); 
                headerRow.CreateCell(20).SetCellValue(" عنوان طرح منزلت ثبت نامی"); 
                headerRow.CreateCell(21).SetCellValue(" نوع سرپرست خانواده ");
                headerRow.CreateCell(22).SetCellValue(" نوع بیماری  ");
                headerRow.CreateCell(23).SetCellValue("نوع جانبازی بینایی ");
                headerRow.CreateCell(24).SetCellValue("جانبازی جسمی حرکتی ويلچري ");
                headerRow.CreateCell(25).SetCellValue(" جانبازی اعصاب و روان");
                headerRow.CreateCell(26).SetCellValue("جانبازی ذهنی  ");
                headerRow.CreateCell(27).SetCellValue("  معلولین جسمی حرکتی غير ويلچري  ");
                headerRow.CreateCell(28).SetCellValue(" معلولین جسمی حرکتی   ويلچري   ");
                headerRow.CreateCell(29).SetCellValue("  معلولین ذهنی  ");
                headerRow.CreateCell(30).SetCellValue("معلولین اعصاب و روان "); 
               
                headerRow.CreateCell(31).SetCellValue(" نوع معلولیت بینایی ");
                headerRow.CreateCell(32).SetCellValue(" نوع معلولیت شنواسس  ");
                headerRow.CreateCell(33).SetCellValue("جسمی حرکتی غير ويلچري");
                headerRow.CreateCell(34).SetCellValue(" نوع جانبازی شنوایی ");

               

                 


                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowNumber = 1;
                DateTimeToPersianDateTimeConverter pdate = new DateTimeToPersianDateTimeConverter("-", false);

                foreach (var obj in data)
                {
                    errorLine = 1;
                    var row = sheet.CreateRow(rowNumber );
                    row.CreateCell(0).SetCellValue(rowNumber);
                    row.CreateCell(1).SetCellValue(obj.Gender);
                    row.CreateCell(2).SetCellValue(obj.NationCode);
                    row.CreateCell(3).SetCellValue(obj.FirstName);
                    row.CreateCell(4).SetCellValue(obj.LastName);
                    row.CreateCell(5).SetCellValue(obj.FatherName);
                    errorLine = 2;
                    if (obj.BirthDate != null)
                    {
                        row.CreateCell(6).SetCellValue(pdate.toShamsiDateTime(obj.BirthDate.Value));
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue("");
                    }
                    errorLine = 4;
                    row.CreateCell(7).SetCellValue(obj.Age.ToString());
                    row.CreateCell(8).SetCellValue("");
                    row.CreateCell(9).SetCellValue(pdate.toShamsiDateTime(obj.CreationDate));
                    errorLine = 9;
                    row.CreateCell(10).SetCellValue(obj.RegisterByService);
                    row.CreateCell(11).SetCellValue(obj.SabtStatus.ToString());
                    errorLine = 10;
                    if (obj.PersonalPicture_Confirmed != null)
                    {
                        row.CreateCell(12).SetCellValue(obj.PersonalPicture_Confirmed.ToString());
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue("");
                    }

                    errorLine = 20;
                    row.CreateCell(13).SetCellValue(obj.Nationality);
                    row.CreateCell(14).SetCellValue("");
                    errorLine = 22;
                    row.CreateCell(15).SetCellValue(obj.FullAddress);
                    row.CreateCell(16).SetCellValue(obj.PostalCode); 
                    row.CreateCell(17).SetCellValue(pdate.toShamsiDateTime(obj.CitizenCreationDate));



                    row.CreateCell(18).SetCellValue(obj.FormStatuse.ToString()); 
                    if (obj.InManzalatGroups == true)
                    {
                        row.CreateCell(19).SetCellValue("تایید شدگان طرح منزلت");
                    }
                    else
                    {
                        row.CreateCell(19).SetCellValue("");
                    }

                    row.CreateCell(20).SetCellValue(obj.FormTitle.ToString());
                    row.CreateCell(21).SetCellValue(obj.Typ_ZananSarparast.ToString());
                    row.CreateCell(22).SetCellValue(obj.Typ_SpecialDiseases.ToString());
                    row.CreateCell(23).SetCellValue(obj.Typ_Janbazan_Binaei.ToString());
                    row.CreateCell(24).SetCellValue(obj.Typ_Janbazan_JesmiHarekati_WheelChair.ToString());
                    row.CreateCell(25).SetCellValue(obj.Typ_Janbazan_AsabRavan.ToString());
                    row.CreateCell(26).SetCellValue(obj.Typ_Janbazan_Zehni.ToString());
                    row.CreateCell(27).SetCellValue(obj.Typ_Maloulin_JesmiHarekati_NoWheelChair.ToString());
                    row.CreateCell(28).SetCellValue(obj.Typ_Maloulin_JesmiHarekati_WheelChair.ToString());
                    row.CreateCell(29).SetCellValue(obj.Typ_Maloulin_Zehni.ToString());
                    row.CreateCell(30).SetCellValue(obj.Typ_Maloulin_AsabRavan.ToString());


                    row.CreateCell(31).SetCellValue(obj.Typ_Maloulin_Binaei.ToString());
                    row.CreateCell(32).SetCellValue(obj.Typ_Maloulin_Shenavaei.ToString());
                    row.CreateCell(33).SetCellValue(obj.Typ_Janbazan_JesmiHarekati_NoWheelChair.ToString());
                    row.CreateCell(34).SetCellValue(obj.Typ_Janbazan_Shenavaei.ToString());
                    

                    //RegisterByService




                    errorLine = 50;
                    rowNumber++;  

                }
                errorLine = 100;
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel", //MIME type of Excel files
                 "allCitizens.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user


            }
            catch (Exception er)
            {

                return BadRequest(er.Message + " " + errorLine);
            }

             


        }






        [HttpGet]
        [Route("SearchManzalatCitizens")]
        [Authorize(Roles = "WebApiUser")]
        public async Task<ApiResult<PagedManzalatCitizenViewModel>> SearchManzalatCitizens(
            int? offset = 1, int? count = 20,
             DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
     DateTime? birthDateToDate = null,
     bool? gender = null,
     SabtStatusEnum? sabtStatus = null,
    MaritalStatusEnum? mariageStatus = null,
          string name = null,
          string nationCode = null,
           ManzalatFormStatuseEnum? formStatuse = null,
           ManzalatFormTypeEnum? manzalatFormType = null, bool? inManzalatGroups = null,
           bool? hasCard = null
            )
        {



            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_ثبت_نام_کنندگان_طرح_منزلت);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedManzalatCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, new PagedManzalatCitizenViewModel(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");





            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _manzalat.SearchManzaltCitizens(offset.Value, count.Value, FromDate, ToDate,
            birthDateFromDate, birthDateToDate, gender, sabtStatus, mariageStatus, name, nationCode,
            formStatuse, manzalatFormType, inManzalatGroups, null, hasCard);

        }







        /// <summary>
        /// دریافت اطلاعات شهروند به همراه طرح های منزلت
        /// </summary>
        /// <param name="userCode">شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetCitizenInfoAndManzaltForm")]
        public async Task<ApiResult<CitizenReviewManzalatForm>> GetCitizenInfoAndManzaltForm(string  userCode)
        {
            return await _manzalat.GetCitizenInfoAndManzaltForm(userCode);
        }


       




      



        /// <summary>
        /// تایید یا عدم تایید فرم منزلت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> ConfirmManzaltByAdmin(ConfirmFormManzalat model)
        {
            ApiResult res1 = new ApiResult(true, ApiResultStatusCode.Success, "");
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بررسی_درخواست_منزلت);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");
            }


            var isupdate = await _manzalat.ConfirmManzaltByAdmin(model, userId);
            if (isupdate.IsSuccess && model.SendSms) //ارسال پیامک اوکی بود
            {
                var update = isupdate.Data;
                SendSms sms = new SendSms();
               
              
                int count = await this._smsInfoService.CountManzalatSms(update.MobileNumber);
                if (count > 2)
                {
                    res1.Messages = " تعداد پیامک های ارسالی به شماره موبایل شما بیش از حد مجاز می باشد بعد از گذشت 24 ساعت مجددا اقدام نمائید. ";
                    res1.IsSuccess = false;
                    res1.StatusCode = ApiResultStatusCode.BadRequest;
                    return res1;
                }

                var smsOption = await _siteSettingService.GetSmsSettingForSend();
                if (update.FormResult == true)
                {


                    if (update.HasCard)//اگر کارت شهروندی دارد
                    {


                        var smsLog = await sms.VerifyLookup(update.MobileNumber, " شد ", null, null, "شهروند گرامی", TempleteNameEnum.ManzelatHasCardAndConvertCard, smsOption.SmsToken);
                        if (smsLog.IsSuccess)
                        {
                            var citizenId = _usersService.GetCurrentUserId();
                            var saveSms = await _smsInfoService.Add(smsLog.Data, citizenId);
                            if (!saveSms.IsSuccess)
                            {
                                return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                            }
                        }

                    }
                    else ///کارتی ندارد
                    {

                        var smsLog = await sms.VerifyLookup(update.MobileNumber, " شد ", null, null, "شهروند گرامی", TempleteNameEnum.ManzelatNoCard, smsOption.SmsToken);
                        if (smsLog.IsSuccess)
                        {
                            var citizenId = _usersService.GetCurrentUserId();
                            var saveSms = await _smsInfoService.Add(smsLog.Data, citizenId);
                            if (!saveSms.IsSuccess)
                            {
                                return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                            }
                        }

                    }




                }
                else if (update.FormResult == false)
                {

                    var smsLog = await sms.VerifyLookup(update.MobileNumber, " نشد ", null, null, "شهروند گرامی", TempleteNameEnum.ManzelatReview, smsOption.SmsToken);
                    if (smsLog.IsSuccess)
                    {
                        var citizenId = _usersService.GetCurrentUserId();
                        var saveSms = await _smsInfoService.Add(smsLog.Data, citizenId);
                        if (!saveSms.IsSuccess)
                        {
                            return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                        }
                    }


                }


            }


            return isupdate;


        }


    


        #region تنظیمات منزلت


        [HttpGet]
        [Route("GetManzalatSettings")]
        [AllowAnonymous]
        public async Task<ApiResult<ManzalatSettings>> GetManzalatSettings()
        {
           

            return await _siteSettingService.GetManzalatSettings();

        }
         
        /// <summary>
        /// تنطیم تنطیمات سایت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<ManzalatSettings>> UpdateManzalatSettings(ManzalatSettings model)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.تنظیمات_منزلت);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<ManzalatSettings>(false, ApiResultStatusCode.NotAllowed,null, "شما به این قسمت دسترسی ندارید");
            }


            return await _siteSettingService.UpdateManzalatSettings(model);

        }


        #endregion


      

        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetCitizenRegisterManzalatForm")]
        public async Task<ApiResult<CitizenReviewManzalatFormItem>> GetCitizenRegisterManzalatForm(int formBaseId)
        {
            int userId = this._usersService.GetCurrentUserId();
            return  await this._manzalat.GetCitizenRegisterManzalatForm(userId, formBaseId);
            
        }
 
       

    }
}