using cle.Services;
using cle.Services.CitizensGroups;
using cle.Services.Faq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.ImportFile;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.Group;
using Nikan.ViewModel.ImportExcelFile;
using Nikan.ViewModel.Ticket;
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

    /// <summary>
    /// مدیریت گروههای شهروندی
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class GroupsCitizensController : Controller 
    {
        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene;
        private readonly ICitizenFamiliesService _family;
        private readonly IAppService _app;
        private readonly ITokenStoreService _tokenStoreService; 
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IGroupService _group;
        private readonly IImportExcelFileService _importFile;
        private readonly IPermissionService _permission;


        public GroupsCitizensController(
              IUsersService userManager,
              ICitizenService citzene,
               IImportExcelFileService  importFile,
                IGroupService group,
                IPermissionService permission,
               ICitizenFamiliesService family,
              ICitizenFeedbackService citizenFeedback,
              IAppService app, 
              IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService, 
            ISiteSettingService siteSettingService,
            IAntiForgeryCookieService antiforgery


            )
        {
            _importFile = importFile;
            _citzene = citzene;
            _family = family;
            _permission = permission;
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _app = app;
            _siteSettingService = siteSettingService; 
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _group = group;

            _antiforgery.CheckArgumentIsNull(nameof(antiforgery)); 
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));

        }
        #endregion
        #region Groups
        /// <summary>
        ///  اضافه کردن گروه شهروندی
        /// [Group1]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<GroupDto>> AddOrUpdateGroup(GroupDto model)
        {
            if (model == null)
                return new ApiResult<GroupDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
          
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<GroupDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به گروههای شهروندی دسترسی ندارید");




            return await _group.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// حذف گروه شهروندی
        ///  [Group2]
        /// </summary>
        /// <param name="id">شناسه گروه شهروندی</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveGroup")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> RemoveGroup(int id)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به گروههای شهروندی دسترسی ندارید");


            return await _group.Remove(id);

        }

        /// <summary>
        /// دریافت جزئیات یک گروه
        ///  [Group3]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GroupInfo")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<GroupInfo>> GroupInfo(int id)
        {
            return await _group.GroupInfo(id);
        }





        /// <summary>
        /// بازبینی عضویت شهروندان در گروههای شهروندی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ReviewGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> ReviewGroups(int id)
        {
            return await _group.ReviewGroups(id);
        }


         


        /// <summary>
        ///  [Group4]
        /// دریافت لیست گروهها پنل مدیریت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<GroupInfo>>> GetAllGroups()
        {
            return await _group.GetAllGroups();
        }

        /// <summary>
        /// دریافت لیست گروهها به صورت صفحه بندی شده
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchGroups")]
        [Authorize(Roles = "Admin")] 
        public async Task<ApiResult<PagedGroupsViewModel>> SearchGroups(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
         DateTime? ToDate = null,
         string groupname = null 
           )
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedGroupsViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به گروههای شهروندی دسترسی ندارید");


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _group.SearchGroups(offset.Value, count.Value, FromDate, ToDate,
                groupname);

        }



        /// <summary>
        /// انتقال گروه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> GroupTransfer(GroupTransferModel model)
        {
            if (model == null)
                return new ApiResult(false, ApiResultStatusCode.ServerError,  "مدل ورودی خالی است");
            var userId = _usersService.GetCurrentUserId();
            return await _group.GroupTransfer(model, userId);
        }











        #endregion
        #region اضافه کردن شهروند از طریق فایل اکسل


        /// <summary>
        /// جزئیات ورودی فایل اکسل  گروههای شهروندی
        /// </summary>
        /// <param name="importId">شناسه فایل</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GroupImportFileDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<ImportFileInfo>> GroupImportFileDetails(int importId)
        {
            return await _importFile.GroupImportFileDetails(importId);

        }

        /// <summary>
        /// لیست فایل های بارگذاری شده بابت گروههای شهروندی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllGroupImportList")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<ImportFileInfo>>> GetAllGroupImportList()
        {
            return await _importFile.GetAllGroupImportList();

        }


        /// <summary>
        /// اضافه کردن گرووها به گروه یا صف 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportGroupNationCodeFromExcel")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<UploadFileResult>> ImportGroupNationCodeFromExcel([FromForm]AttachmentViewModel model)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            try
            {
               

                var fileName = "";
                string folderName = "GroupNationCode";
                var ticks = DateTime.Now.Ticks;
                var webRootPath = _environment.WebRootPath + "/uploads/Resources/GroupNationCode/" + ticks;
                var file = model.File;
                string newPath = Path.Combine(webRootPath, folderName);
                var list = new List<ImportFileGroupNationCodeInfo>();
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
                        if (cellCount < 1)
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
                                var cell1 = row.GetCell(0);// کد ملی شهروند


                                if (cell1 != null)
                                {

                                    var nationCode = cell1.ToString().Trim();
                                    if (nationCode.Length < 10)
                                        nationCode = "0" + nationCode;
                                    if (nationCode.Length < 10)
                                        nationCode = "0" + nationCode;
                                    if (nationCode.Length < 10)
                                        nationCode = "0" + nationCode;


                                    list.Add(new ImportFileGroupNationCodeInfo
                                    {
                                        NationCode = nationCode
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
                    var userId = _usersService.GetCurrentUserId();// 
                    var addfile = await _group.AddNationCodeList(list, fileName, model.GroupId, userId);
                    if (addfile.IsSuccess == false)
                    {
                        res.Messages = addfile.Messages;
                        res.IsSuccess = false;
                        res.Data.ErrorMessage = addfile.Data.ErrorMessage;
                    }
                    else
                    {
                        res.Data.ImportId = addfile.Data.ImportId;
                        res.Data.ErrorMessage = addfile.Data.ErrorMessage;
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


        [HttpGet]
        [Route("ConfirmNationCodeImportListToGroup")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> ConfirmNationCodeImportListToGroup(int importId)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _importFile.ConfirmNationCodeImportListToGroup(importId, userId);

        }


        #endregion
        #region صف شهروندی
        [HttpGet]
        [Route("SearchCitizensQueue")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedCitizensQueueViewModel>> SearchCitizensQueue(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
         DateTime? ToDate = null,
         string groupname = null,
        string nationCode = null,
        int? groupId = null
           )
        {


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _group.SearchCitizensQueue(offset.Value, count.Value, FromDate, ToDate,
                groupname, nationCode, groupId);

        }
      
        
        [HttpGet]
        [Route("RemoveQueue")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> RemoveQueue(int id)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به گروههای شهروندی دسترسی ندارید");


            return await _group.RemoveQueue(id);
        }









        #endregion 



    }
}
