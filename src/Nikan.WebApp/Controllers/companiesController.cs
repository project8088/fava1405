using cle.Services; 
using cle.Services.UserCompanyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.ImportFile;
using Nikan.ViewModel;
using Nikan.ViewModel.ImportExcelFile;
using Nikan.ViewModel.UserCompanes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")] 
    public class companiesController : Controller
    {
         


        #region Ctor
        
        private readonly IUserCompanyService _userCompanyService;
        private readonly IUsersService _usersService; 
        public static IHostingEnvironment _environment;
        private readonly ITransactionService _transactionService;
        private readonly ISmsInfoService _smsService;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IImportExcelFileService _importFile;
        
        public companiesController( 
            IUserCompanyService userProfileService,
            ISmsInfoService  smsService,
             IImportExcelFileService  iImportExcelFile,
              IUsersService userManager, 
        IHostingEnvironment environment,
              ISiteSettingService  siteSettingService,
                ITransactionService transactionService 
            )
        {
            _transactionService = transactionService; 
            _userCompanyService = userProfileService; 
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _smsService =  smsService;
            _importFile =  iImportExcelFile;
            _siteSettingService = siteSettingService;
        }
        #endregion



        /// <summary>
        /// ثبت نام شرکت سمت پنل مدیریت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AdminCompanyRegisterResult>> CompanyRegister(AdminCompanyRegister model)
        {
            var  userId= _usersService.GetCurrentUserId(); 
            return await _userCompanyService.CompanyRegisterAsync(model, userId);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfigRegisterAll")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> ConfigRegisterAll()
        {
            return await _userCompanyService.ConfigRegisterAll();

        }


        /// <summary>
        /// دریافت اطلاعات کامل یک شرکت
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFullCompanyInfo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyInfoDto>> GetFullCompanyInfo(int? companyId)
        { 
            //check user is admin  
            return await _userCompanyService.GetFullCompanyInfo(companyId.Value, true); 
        }

        
       



        /// <summary>
        /// جستجوی شرکت ها
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchCompanies")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ApiResult<PagedCompaniesViewModel>> SearchCompanies(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string companyName = null,
            int? contractCode = null,
             string managerName = null

            )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _userCompanyService.SearchCompanies(offset.Value, count.Value, FromDate, ToDate,
                companyName, contractCode, managerName);

        }


        /// <summary>
        /// لیست اطلاعات اولیه شرکت ها
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListCompany")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<BaseDataModel>>> GetListCompany(int? selected = null)
        {
            return await _userCompanyService.GetAllCompany(selected);

        }
        /// <summary>
        /// دریافت اطلاعات ادرس شرکت
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAddressInfo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyAddressInfo>> GetAddressInfo(int? companyId)
        {
            if (companyId == null || companyId == 0)
            {
                companyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری

            }
            return await _userCompanyService.GetAddressInfo(companyId.Value);

        }

        [HttpGet]
        [Route("RemoveCompany")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> RemoveCompany(int companyId)
        {
            return await _userCompanyService.Remove(companyId);

        }




        /// <summary>
        /// دریافت اطلاعات پایه شرکت
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseInfo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyBaseInfo>> GetBaseInfo(int? companyId)
        {
            if (companyId == null || companyId == 0)
            {
                companyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری

            }
            return await _userCompanyService.GetBaseInfo(companyId.Value);

        }



        /// <summary>
        /// دریافت اطلاعات اصلی شرکت
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMainInfo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyMainInfo>> GetMainInfo(int? companyId)
        {
            if (companyId == null || companyId == 0)
            {
                companyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری

            }
            return await _userCompanyService.GetMainInfo(companyId.Value);

        }


        [HttpGet]
        [Route("GetAdditionalInfo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyAdditionalInfo>> GetAdditionalInfo(int companyId)
        {
            if (companyId == 0)
            {
                return new ApiResult<CompanyAdditionalInfo>(false, ApiResultStatusCode.BadRequest, null, "شناسه شرکت را مشخص نمایید ");

            }
            return await _userCompanyService.GetAdditionalInfo(companyId);

        }




        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> UpdateAddressInfo([FromBody]CompanyAddressInfo model)
        {
            if (model.CompanyId == null || model.CompanyId == 0)
            {
                model.CompanyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری

            }

            return await _userCompanyService.UpdateAddressInfo(model);

        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> UpdateBaseInfo([FromBody]CompanyBaseInfo model)
        {
            bool isAdmint = _usersService.IsAdmin(); 
            if (model.CompanyId == null || model.CompanyId == 0)
               model.CompanyId = _usersService.GetCurrentUserCompanyId(); 
           
            return await _userCompanyService.UpdateBaseInfo(model, isAdmint);

        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> UpdateMainInfo([FromBody]CompanyMainInfo model)
        {
            if (model.CompanyId == null || model.CompanyId == 0)
            {
                model.CompanyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری

            }
            return await _userCompanyService.UpdateMainInfo(model);

        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> UpdateAdditionalInfo([FromBody]CompanyAdditionalInfo model)
        {
            if (model.CompanyId == 0)
            {
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "شناسه شرکت را مشخص نمایید ");

            }
            return await _userCompanyService.UpdateAdditionalInfo(model);

        }


        #region ویرایش حساب کاربری شرکت
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> ChangeCompanyAccount([FromBody]ChangeCompanyAccountStatus model)
        {
            if (model.CompanyId == 0)
            {
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "شناسه شرکت را مشخص نمایید ");

            }
            return await _userCompanyService.ChangeCompanyAccount(model);

        }

        #endregion  
        #region فعالیت های  شرکت
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserCompanyActivitiyInfo>> AddCompanyActivity([FromBody]AddUserCompanyActivities model)
        {
            if (model.UserCompanyId == null)
                model.UserCompanyId = _usersService.GetCurrentUserCompanyId();
            return await _userCompanyService.AddCompanyActivity(model);
        }

        [HttpGet]
        [Route("RemoveActivitiy")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public async Task<ApiResult<string>> RemoveActivitiy(int id)
        {
            return await _userCompanyService.RemoveActivitiy(id);

        }

        [HttpGet]
        [Route("GetCompanyActivity")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<UserCompanyActivitiyInfo>>> GetCompanyActivity(int? companyId)
        {
            if (companyId == null)
                companyId = _usersService.GetCurrentUserCompanyId();
            return await _userCompanyService.GetCompanyActivity(companyId.Value);

        }

        #endregion 
      
        #region UploadImage
        [HttpPost]
        [Route("UploadImage")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize()]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyLogo>> UploadImage(CompanyLogo model)
        {
            var response = new ApiResult<CompanyLogo>(false, ApiResultStatusCode.Success, new CompanyLogo());
            try
            {
                if (model == null)
                {
                    response.IsSuccess = false;
                    response.Messages = "مدل ورودی خالی می باشد. ";
                    return response;
                }
                var file = model.file;
                if (file == null)
                {
                    response.IsSuccess = false;
                    response.Messages = "فایل را  انتخاب نمایید ";
                    return response;
                }


                if (model.CompanyId == null)
                    model.CompanyId = _usersService.GetCurrentUserCompanyId();



                var filesPath = _environment.WebRootPath + "/uploads/Resources/Images/companies/" + model.CompanyId + "/Logo";
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var fileSize = file.Length;
                if (fileSize > 0)
                {
                    var megLength = (fileSize / 1048576.0); //(i.e. 1024 * 1024): 
                    if (megLength > 1)
                    {
                        response.IsSuccess = false;
                        response.Messages = "حجم تصویر بیش از حد مجاز می باشد.";
                        return response;

                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    var shortPath = "/uploads/Resources/Images/companies/" + model.CompanyId + "/Logo/" + fileName;
                    string minPath = _environment.WebRootPath + shortPath;
                    using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                    image.Mutate(x => x.Resize(300, 400));
                    image.Save(minPath);
                    response.Data.ImageUrl = shortPath;
                    response.IsSuccess = true;
                    return await _userCompanyService.UpdateCompanyLogo(new CompanyLogo() { ImageUrl = shortPath, CompanyId = model.CompanyId });

                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود لوگو  !";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود   لوگو: {ex}";
                return response;
            }
        }


        [HttpGet]
        [Route("GetCompanyLogo")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyLogo>> GetCompanyLogo(int? companyId)
        {
            if (companyId == null)
                companyId = _usersService.GetCurrentUserCompanyId();

            return await _userCompanyService.GetCompanyLogo(companyId.Value);

        }




        /// <summary>
        /// دریافت اطلاعات کوتاه شرکت
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyShortInfo")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyShortInfo>> GetCompanyShortInfo(int? companyId)
        {
            if (companyId == null)
                companyId = _usersService.GetCurrentUserCompanyId();

            return await _userCompanyService.GetCompanyShortInfo(companyId.Value);

        }



        [HttpGet]
        [Route("GetCompanySignature")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanySignatureInfo>> GetCompanySignature(int? companyId)
        {
            if (companyId == null)
                companyId = _usersService.GetCurrentUserCompanyId();

            return await _userCompanyService.GetCompanySignature(companyId.Value);

        }

        [HttpPost]
        [Route("UploadSignature")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanySignatureInfo>> UploadSignature([FromForm]CompanySignatureInfo model)
        {
            var response = new ApiResult<CompanySignatureInfo>(false, ApiResultStatusCode.Success, new CompanySignatureInfo());
            try
            {
                if (model == null)
                {
                    response.IsSuccess = false;
                    response.Messages = "مدل ورودی خالی می باشد. ";
                    return response;
                }


                if (model.CompanyId == null)
                    model.CompanyId = _usersService.GetCurrentUserCompanyId();


                var file = model.file;
                if (file == null)
                {
                    response.IsSuccess = false;
                    response.Messages = "فایل را  انتخاب نمایید ";
                    return response;
                }

                var filesPath = _environment.WebRootPath + "/uploads/Resources/Images/companies/" + model.CompanyId.Value + "/Signature";
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var fileSize = file.Length;
                if (fileSize > 0)
                {
                    var megLength = (fileSize / 1048576.0); //(i.e. 1024 * 1024): 
                    if (megLength > 1)
                    {
                        response.IsSuccess = false;
                        response.Messages = "حجم تصویر بیش از حد مجاز می باشد.";
                        return response;

                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var shortPath = "/uploads/Resources/Images/companies/" + model.CompanyId + "/Signature/" + fileName;
                    string minPath = _environment.WebRootPath + shortPath;
                    using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                    image.Mutate(x => x.Resize(300, 400));
                    image.Save(minPath);
                    response.Data.SignatureUrl = shortPath;
                    response.IsSuccess = true;
                    return await _userCompanyService.UpdateCompanySignature(new CompanySignatureInfo() { SignatureUrl = shortPath, CompanyId = model.CompanyId.Value });

                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود تصویر امضاء  !";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود   تصویر امضاء: {ex}";
                return response;
            }
        }



        /// <summary>
        /// دریافت تصویر قرارداد
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyContract")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyContractInfo>> GetCompanyContract(int? companyId)
        {
            if (companyId == null)
                companyId = _usersService.GetCurrentUserCompanyId();

            return await _userCompanyService.GetCompanyContract(companyId.Value);

        }


        /// <summary>
        /// ویژه پنل مدیریت 
        /// بارگذاری توسط پنل مدیریت صورت میگیرد
        /// بارگذاری تصویر قرارداد
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadContract")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CompanyContractInfo>> UploadContract(CompanyContractInfo model)
        {
            var response = new ApiResult<CompanyContractInfo>(false, ApiResultStatusCode.Success, new CompanyContractInfo());
            try
            {
                if (model.CompanyId == null)
                    model.CompanyId = _usersService.GetCurrentUserCompanyId();

                var filesPath = _environment.WebRootPath + "/uploads/Resources/Images/companies/" + model.CompanyId + "/Contract";
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var file = model.file;
                var fileSize = model.file.Length;
                if (fileSize > 0)
                {
                    var megLength = (fileSize / 1048576.0); //(i.e. 1024 * 1024): 
                    if (megLength > 1)
                    {
                        response.IsSuccess = false;
                        response.Messages = "حجم تصویر بیش از حد مجاز می باشد.";
                        return response;

                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var shortPath = "/uploads/Resources/Images/companies/" + model.CompanyId + "/Contract/" + fileName;
                    string minPath = _environment.WebRootPath + shortPath;
                    using var image = SixLabors.ImageSharp.Image.Load(model.file.OpenReadStream());
                    image.Mutate(x => x.Resize(300, 400));
                    image.Save(minPath);
                    response.Data.ContractUrl = shortPath;
                    response.IsSuccess = true;
                    return await _userCompanyService.UpdateCompanyContract(new CompanyContractInfo() { ContractUrl = shortPath, CompanyId = model.CompanyId });

                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود تصویر قرارداد  !";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود   تصویر قرارداد: {ex}";
                return response;
            }
        }




        #endregion


        #region Excel File
        /// <summary>
        /// لیست کل فایل های ورودی
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("PersonnelImportFileList")]
        [Authorize(Roles = "Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<ImportExcelFileInfo>>> PersonnelImportFileList()
        {
           var companyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری
            return await _importFile.GetAllCompanyFile(companyId); 
        }

        [HttpPost]
        [Route("ImportPersonnelFromExcel")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UploadFileResult>> ImportPersonnelFromExcel(IFormFile file)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            try
            {
                var fileName = "";
                string folderName = "CitizenList";
                var ticks = DateTime.Now.Ticks;
                var webRootPath = _environment.WebRootPath + "/uploads/Resources/Company/" + ticks;

                string newPath = Path.Combine(webRootPath, folderName);
                var list = new List<AddCompanyPersonnelInfo>();
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
                        if (cellCount < 7)
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
                               
                                var gender = row.GetCell(0);
                                var nationCode = row.GetCell(1);
                                var firstName = row.GetCell(2);
                                var lastName = row.GetCell(3);
                                var fatherName = row.GetCell(4);
                                var birthDate = row.GetCell(5);
                                var mobile = row.GetCell(6);
                                var jobTitle = row.GetCell(7);
                                var address = row.GetCell(8);

                                if (gender != null && nationCode != null && firstName != null
                                    && lastName != null
                                    && fatherName != null
                                    && birthDate != null
                                    && mobile != null
                                    )
                                {
                                    list.Add(new AddCompanyPersonnelInfo
                                    {
                                        Address = address.ToString(),
                                        JobTitle = jobTitle.ToString(),
                                        BirthDate = birthDate.ToString(),
                                        FatherName = fatherName.ToString(),
                                        FirstName = firstName.ToString(),
                                        Gender = gender.ToString(),
                                        LastName = lastName.ToString(),
                                        Mobile = mobile.ToString(),
                                        NationCode = nationCode.ToString(), 
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
                    var companyId = _usersService.GetCurrentUserCompanyId();//دریافت شناسه شرکت جاری
                    var userId = _usersService.GetCurrentUserId();//دریافت شناسه کارجوی جاری
                    var addfile = await _importFile.AddCompanyPersonnelImportFile(list, fileName, companyId, userId);
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

        //[HttpGet]
        //[Route("ConfirmPersonnelFile")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ApiResult> ConfirmPersonnelFile(int importId)
        //{
        //    var userId = _usersService.GetCurrentUserId();//دریافت شناسه کارجوی جاری
        //    return await _ci.ConfirmWaterFile(importId, userId);

        //}



        [HttpGet]
        [Route("PersonnelImportFileDetails")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ImportFileInfo>> PersonnelImportFileDetails(int importId)
        {
            return await _importFile.PersonnelImportFileDetails(importId);

        }

        [HttpGet]
        [Route("RemoveImportFile")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> RemoveImportFile(int importId)
        {
            return await _importFile.Remove(importId);

        }


        #endregion 



    }
}