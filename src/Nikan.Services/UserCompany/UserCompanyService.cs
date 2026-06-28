using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.UserCompanes;
using Nikan.ViewModel;
using Nikan.ViewModel.UserCompanes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nikan.DomainClasses;
using Nikan.Services;
using Nikan.Common.GlobalEnum;
using Nikan.ViewModel.Report;
using Nikan.DomainClasses.Factor;

namespace cle.Services.UserCompanyServices
{
    public interface IUserCompanyService
    {

        /// <summary>
        /// دریافت اطلاعات کامل یک شرکت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<CompanyInfoDto>> GetFullCompanyInfo(int id, bool showMobileNumber = false);




        /// <summary>
        /// دریافت اصلاعات آدرس
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<ApiResult<CompanyAddressInfo>> GetAddressInfo(int companyId);
        Task<ApiResult<List<BaseDataModel>>> GetAllCompany(string query, int offset = 0, int count = 20);
        Task<ApiResult<List<BaseDataModel>>> GetAllCompany(int? selected = null);

        /// <summary>
        /// دریافت اطلاعات پایه
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<ApiResult<CompanyBaseInfo>> GetBaseInfo(int companyId);
        /// <summary>
        /// دریافت اطلاعات اصلی
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<ApiResult<CompanyMainInfo>> GetMainInfo(int companyId);
        Task<ApiResult> UpdateAddressInfo(CompanyAddressInfo model);
        Task<ApiResult> UpdateBaseInfo(CompanyBaseInfo model, bool isAdmin = true);
        Task<ApiResult> UpdateMainInfo(CompanyMainInfo model);
        Task<ApiResult<PagedCompaniesViewModel>> SearchCompanies(int pageNumber, int pageSize,
            DateTime? FromDate = null, DateTime? ToDate = null, string title = null,
             int? contractCode = null,string managerName=null);
        Task<ApiResult<UserCompanyActivitiyInfo>> AddCompanyActivity(AddUserCompanyActivities model);
        Task<ApiResult<string>> RemoveActivitiy(int id);
        Task<ApiResult<List<UserCompanyActivitiyInfo>>> GetCompanyActivity(int companyId);
        Task<ApiResult<List<BaseDataModel>>> GetActivitiyList();
        Task<ApiResult<AdminCompanyRegisterResult>> CompanyRegisterAsync(AdminCompanyRegister model, int userId);
        Task<ApiResult<CompanyLogo>> UpdateCompanyLogo(CompanyLogo model);
        Task<ApiResult<CompanyLogo>> GetCompanyLogo(int companyId);
        Task<ApiResult<CompanySignatureInfo>> UpdateCompanySignature(CompanySignatureInfo model);
        Task<ApiResult<CompanySignatureInfo>> GetCompanySignature(int companyId);
        Task<ApiResult<CompanyContractInfo>> UpdateCompanyContract(CompanyContractInfo model);
        Task<ApiResult<CompanyContractInfo>> GetCompanyContract(int companyId);
        Task<ApiResult> UpdateAdditionalInfo(CompanyAdditionalInfo model);
        Task<ApiResult<CompanyAdditionalInfo>> GetAdditionalInfo(int companyId);
        Task<ApiResult> ChangeCompanyAccount(ChangeCompanyAccountStatus model);
        Task<ApiResult> Remove(int Id);
        Task<ApiResult<CompanyShortInfo>> GetCompanyShortInfo(int companyId);
        Task<ApiResult<AdminDashbordStatisticalReport>> CountForReport();
      
        Task<ApiResult> ConfigRegisterAll();
       
        
    }
    public class UserCompanyService : IUserCompanyService
    {

        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserCompany> _company;
        private readonly DbSet<UserCompanyPersonel> _personel;
        private readonly DbSet<UserCompanyActivities> _activities;
        private readonly DbSet<UserCompanyFieldOfActivity> _fieldOfActivity;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _role;
        private readonly ISecurityService _securityService;
        private readonly DbSet<UserRole> _userRole;
     


        public UserCompanyService(IUnitOfWork uow , ISecurityService securityService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _company = _uow.Set<UserCompany>();
            _personel = _uow.Set<UserCompanyPersonel>();
            _activities = _uow.Set<UserCompanyActivities>();
            _fieldOfActivity = _uow.Set<UserCompanyFieldOfActivity>();
            _users = _uow.Set<User>();
            _role = _uow.Set<Role>();
            _securityService = securityService;
            _userRole = _uow.Set<UserRole>();
             
            _securityService.CheckArgumentIsNull(nameof(_securityService));
           
        }




        public async Task<ApiResult<AdminDashbordStatisticalReport>>  CountForReport()
        {
            var res = new ApiResult<AdminDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new AdminDashbordStatisticalReport());
            try
            {
                res.Data.ActiveCompany =await _company.CountAsync(w => w.IsDeleted != true && w.UserCompanyStatus == UserCompanyStatusEnum.فعال);
                res.Data.PersonelCount =   _company.Where(w => w.IsDeleted != true && w.UserCompanyStatus == UserCompanyStatusEnum.فعال).Sum(s => s.NumberOfEmployees);
                res.Data.StagnantCompany = await _company.CountAsync(w => w.IsDeleted != true && w.UserCompanyStatus == UserCompanyStatusEnum.راکد);
                res.Data.BuildingCompany = await _company.CountAsync(w => w.IsDeleted != true && w.UserCompanyStatus == UserCompanyStatusEnum.در_حال_احداث);
                 

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }



        public async Task<ApiResult<List<BaseDataModel>>> GetAllCompany(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _company.Where(w=>w.IsDeleted!=true).Select(s => new BaseDataModel()
                {
                    Text = s.CompanyName + " (شرکت: " + s.ManagerName + ") قرارداد: " + s.ContractCode,
                    Key = s.Id.ToString(),
                    // Selected = s.Id == selected
                }).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }
    



        public async Task<ApiResult<List<BaseDataModel>>> GetAllCompany(string query, int offset = 0, int count = 20)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _company.Where(w =>w.IsDeleted!=true && w.CompanyName.Contains(query))
                .Select(s => new BaseDataModel()
                {
                    Text = s.CompanyName,
                    Description = " مدیر عامل: " + s.ManagerName + "  - قرارداد: " + s.ContractCode,
                    Key = s.Id.ToString(),

                }).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }





        public async Task<ApiResult<PagedCompaniesViewModel>> SearchCompanies(
            int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string companyName = null, 
            int? contractCode = null,
            string managerName = null
            )
        { 

            var res = new ApiResult<PagedCompaniesViewModel>(true, ApiResultStatusCode.Success, new PagedCompaniesViewModel());
            try
            {

                 var offset = (pageNumber ) * pageSize ;
                var query = _company.Where(w => w.IsDeleted != true);

                if (!string.IsNullOrEmpty(companyName)   )
                {
                    query = query.Where(w => EF.Functions.Like(w.CompanyName, "%" + companyName + "%"));
                }


                if (!string.IsNullOrEmpty(managerName))
                {
                    query = query.Where(w => EF.Functions.Like(w.ManagerName, "%" + managerName + "%"));
                }



                if ( contractCode!=null)
                {
                    query = query.Where(w =>  w.ContractCode == contractCode);
                }


                if (FromDate != null)
                {
                    query = query.Where(w => w.ContractOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.ContractOnDate <= ToDate);
                }


                res.Data.TotalItems = await query.CountAsync();
                res.Data.companies = await query.Select(s => new CompanyInfoDto()
                {
                    ActivityLicense = s.ActivityLicense,
                    ActivityLicenseDate = s.ActivityLicenseDate,
                    ActivityLicenseType = s.ActivityLicenseType,
                    AreaOfGreenSpace = s.AreaOfGreenSpace,
                    BuildingArea = s.BuildingArea,
                    BuildingLicenseArea = s.BuildingLicenseArea,
                    CellNumber = s.CellNumber,
                    CellNumber2 = s.CellNumber2,
                    CellNumber3 = s.CellNumber3,

                    CityId = s.CityId,
                    City = s.City == null ? "" : s.City.Title,
                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    CompanyOwnerType = s.CompanyOwnerType,
                    CompanyRepresentative = s.CompanyRepresentative,
                    Content = s.Content,
                    EarthCondition = s.EarthCondition,
                    Email = s.Email,
                    EnglishName = s.EnglishName,
                    EstablishedYear = s.EstablishedYear,
                    Fax = s.Fax,
                    FieldOfActivity = s.FieldOfActivity,
                    FullAddress = s.FullAddress,
                    InsuranceNumber = s.InsuranceNumber,
                    ManagerName = s.ManagerName,
                    ManagerNationCode = s.ManagerNationCode,
                    MobileNumber = s.MobileNumber,
                    MobileNumber2 = s.MobileNumber2,
                    MobileNumber3 = s.MobileNumber3,
                    Lat=s.Lat,
                    Lng=s.Lng,
                    Pelak = s.Pelak,
                    SlagUrl = s.SlagUrl,
                    SMSNumber = s.SMSNumber,
                    Street = s.Street,
                    Telegram = s.Telegram,
                    TxtRegNO = s.TxtRegNO,
                    TxtTinNo = s.TxtTinNo,
                    UnitArea = s.UnitArea,
                    Website = s.Website,
                    ZipCode = s.ZipCode,
                    NumberOfEmployees = s.NumberOfEmployees,
                    ChargeCode = s.ChargeCode,
                    ChargeDepositId = s.ChargeDepositId,
                    ChargeMoeinCode = s.ChargeMoeinCode,
                    ContractCode = s.ContractCode,
                    ContractOnDate = s.ContractOnDate,
                    FileCode = s.FileCode,
                    IsBuildingCompany = s.IsBuildingCompany,
                    IsBusinessUnit = s.IsBusinessUnit,
                    IssueChargeBill = s.IssueChargeBill,
                    IssueWaterBill = s.IssueWaterBill,
                    VolumeAirTanks = s.VolumeAirTanks,
                    WaterCode = s.WaterCode,
                    WaterContractOnDate = s.WaterContractOnDate,
                    WaterDepositId = s.WaterDepositId,
                    WaterMoeinCode = s.WaterMoeinCode,
                    LockEdit = s.LockEdit,
                    RegistrationDate = DateTime.Now,
                    UserCompanyAccountStatus = s.UserCompanyAccountStatus,
                    UserCompanyStatus = s.UserCompanyStatus,
                    SignatureUrl = s.SignatureUrl,
                    ContractUrl = s.ContractUrl,
                    ImageUrl=s.ImageUrl,
                    

                }).OrderBy(w=>w.CompanyId).Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

  
        public async Task<ApiResult<CompanyInfoDto>> GetFullCompanyInfo(int id,bool  showMobileNumber=false)
        {
            var res = new ApiResult<CompanyInfoDto>(true, ApiResultStatusCode.Success, new CompanyInfoDto());
            try
            { 

                var info =await _company.Where(w=>w.Id==id && w.IsDeleted!=true ).Select(s=>  new CompanyInfoDto()
                {
                    ActivityLicense = s.ActivityLicense,
                    ActivityLicenseDate = s.ActivityLicenseDate,
                    ActivityLicenseType = s.ActivityLicenseType,
                    AreaOfGreenSpace = s.AreaOfGreenSpace,
                    BuildingArea = s.BuildingArea,
                    BuildingLicenseArea = s.BuildingLicenseArea,
                    CellNumber = s.CellNumber,
                    CellNumber2 = s.CellNumber2,
                    CellNumber3 = s.CellNumber3,


                    CityId = s.CityId,
                    City = s.City == null ? "" : s.City.Title,
                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    CompanyOwnerType = s.CompanyOwnerType,
                    CompanyRepresentative = s.CompanyRepresentative,
                    Content = s.Content,
                    EarthCondition = s.EarthCondition,
                    Email = s.Email,
                    EnglishName = s.EnglishName,
                    EstablishedYear = s.EstablishedYear,
                    Fax = s.Fax,
                    FieldOfActivity = s.FieldOfActivity,
                    FullAddress = s.FullAddress,
                    InsuranceNumber = s.InsuranceNumber,
                    ManagerName = s.ManagerName,
                    ManagerNationCode = s.ManagerNationCode,
                   
                    MobileNumber = showMobileNumber?   s.MobileNumber :"09***",
                    MobileNumber2 = showMobileNumber ? s.MobileNumber2 : "09***",
                    MobileNumber3 = showMobileNumber ? s.MobileNumber3 : "09***",
                    Lat = s.Lat,
                    Lng = s.Lng,
                    Pelak = s.Pelak,
                    SlagUrl = s.SlagUrl,
                    SMSNumber = s.SMSNumber,
                    Street = s.Street,
                    Telegram = s.Telegram,
                    TxtRegNO = s.TxtRegNO,
                    TxtTinNo = s.TxtTinNo,
                    UnitArea = s.UnitArea,
                    Website = s.Website,
                    ZipCode = s.ZipCode,
                    ThumbnailUrl=s.ThumbnailUrl,
                    ImageUrl=s.ImageUrl,
                    NumberOfEmployees = s.NumberOfEmployees,
                    ChargeCode=s.ChargeCode,
                    ChargeDepositId=s.ChargeDepositId,
                    ChargeMoeinCode=s.ChargeMoeinCode,
                    ContractCode=s.ContractCode,
                    ContractOnDate=s.ContractOnDate,
                    FileCode=s.FileCode,
                    IsBuildingCompany=s.IsBuildingCompany,
                    IsBusinessUnit=s.IsBusinessUnit,
                    IssueChargeBill=s.IssueChargeBill,
                    IssueWaterBill=s.IssueWaterBill,
                    VolumeAirTanks=s.VolumeAirTanks,
                    WaterCode=s.WaterCode,
                    WaterContractOnDate=s.WaterContractOnDate,
                    WaterDepositId=s.WaterDepositId,
                    WaterMoeinCode=s.WaterMoeinCode, 
                    LockEdit = s.LockEdit,
                    RegistrationDate = DateTime.Now,
                    UserCompanyAccountStatus = s.UserCompanyAccountStatus,
                    UserCompanyStatus =  s.UserCompanyStatus,
                    SignatureUrl = s.SignatureUrl,
                    ContractUrl = s.ContractUrl, 
                    UserCompanyActivities =new List<UserCompanyActivitiyInfo>() 
                }).FirstOrDefaultAsync();

                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                 

                info.UserCompanyActivities=await _activities.Where(w => w.UserCompanyId == id).Select(s => new UserCompanyActivitiyInfo()
                {
                    Activity = s.Activity.Title,
                    ActivityId = s.ActivityId,
                    UserCompanyId = s.UserCompanyId,
                    CompanyName = info.CompanyName,
                    Id = s.Id 
                }).ToListAsync();

                
                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

        public async Task<ApiResult> ConfigRegisterAll()
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success,"");
            //چک شود نام کاربری قبلا ثیت نام نکرده باشد.
            try
            {
                var companyRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Company); 
                var all = await _company.Where(w => w.IsDeleted != true).ToListAsync();
                foreach (var com in all)
                {
                    var code = com.ContractCode;
                    var pass = "123456";
                    if ( code!=0)
                    {
                        var username = "company" + code;
                        if (!string.IsNullOrWhiteSpace(com.MobileNumber))
                        { 
                            pass = com.MobileNumber;
                        } 
                        if (! await _users.AnyAsync(w => w.Username == username))
                        {
                            var passwordHash = _securityService.GetSha256Hash(pass);
                            var user = new User
                            {
                                DisplayName = com.CompanyName,
                                Username = username,
                                UserAccountState = userAccountStateEnum.فعال,
                                Password = passwordHash,
                                SerialNumber = Guid.NewGuid().ToString("N"),
                                UserCompany = com,
                                CreatedOnDate = DateTime.Now,
                                MobileNumber = com.MobileNumber,
                                EmailAddress = com.Email, 
                            };

                            var add = await _users.AddAsync(user);
                            if (companyRole != null)
                            {
                                var role = new UserRole() { RoleId = companyRole.Id, User = user };
                                await _userRole.AddAsync(role);
                            }



                            await _uow.SaveChangesAsync();

                        }

                    }

                }


                 
                
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }





        public async Task<ApiResult<AdminCompanyRegisterResult>> CompanyRegisterAsync(AdminCompanyRegister model,int userId)
        {

            var res = new ApiResult<AdminCompanyRegisterResult>(true, ApiResultStatusCode.Success, new AdminCompanyRegisterResult());
            //چک شود نام کاربری قبلا ثیت نام نکرده باشد.
            try
            {
                if (await _users.AnyAsync(w => w.Username == model.UserName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام کاربری وارد شده تکراری می باشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }


                var companyRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Company); 
                var company = new UserCompany()
                {
                    CompanyName = model.CompanyName,
                    EnglishName = model.EnglishName,
                    CompanyRepresentative = model.CompanyRepresentative,
                    EstablishedYear = model.EstablishedYear,
                    TxtTinNo = model.TxtTinNo,
                    TxtRegNO = model.TxtRegNO,
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = model.MobileNumber,
                    Email = model.Email, 
                    LastModifiedOnDate = DateTime.Now, 
                    Content = "", 
                    LockEdit = false, 
                    RegistrationDate = DateTime.Now, 
                    UserCompanyAccountStatus = UserCompanyAccountStatusEnum.تایید_شده,
                    UserCompanyStatus = UserCompanyStatusEnum.فعال,
                    ContractCode=model.ContractCode,

                };



                await _company.AddAsync(company);

             


                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = model.CompanyName,
                    Username = model.UserName,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    UserCompany = company,
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = model.MobileNumber,
                    EmailAddress = model.Email,
                    

                };

                var add = await _users.AddAsync(user);
                if (companyRole != null)
                {
                    var role = new UserRole() { RoleId = companyRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }



                await _uow.SaveChangesAsync();

                res.Data.CompanyId = company.Id;
                res.Data.CompanyName = model.CompanyName;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }


        #region تغییر وضعیت حساب کاربری شرکت
        public async Task<ApiResult> ChangeCompanyAccount(ChangeCompanyAccountStatus model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                 
                info.UserCompanyAccountStatus = model.UserCompanyAccountStatus;
                info.RejectDesription = model.RejectDesription; 

                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }


        #endregion 


        #region اطلاعات اصلی
        public async Task<ApiResult> UpdateMainInfo(CompanyMainInfo model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                info.Content = model.Content;
                info.InsuranceNumber = model.InsuranceNumber;
                info.CompanyRepresentative = model.CompanyRepresentative;
                info.SlagUrl = model.SlagUrl;
                info.NumberOfEmployees = model.NumberOfEmployees; 
                info.ThumbnailUrl = model.ThumbnailUrl;
                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        public async Task<ApiResult<CompanyMainInfo>> GetMainInfo(int companyId)
        {
            var res = new ApiResult<CompanyMainInfo>(true, ApiResultStatusCode.Success, new CompanyMainInfo());
            var info = new CompanyMainInfo();


            try
            {

                var model = await _company.FirstOrDefaultAsync(w => w.Id == companyId);
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.CompanyId = model.Id;
                info.Content = model.Content;
                info.InsuranceNumber = model.InsuranceNumber;
                info.CompanyRepresentative = model.CompanyRepresentative;
                info.SlagUrl = model.SlagUrl;
                info.ImageUrl = model.ImageUrl;
                info.ThumbnailUrl = model.ThumbnailUrl;
               
                info.NumberOfEmployees = model.NumberOfEmployees;
                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        #endregion 
        #region اطلاعات ادرس
        public async Task<ApiResult> UpdateAddressInfo(CompanyAddressInfo model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                info.MobileNumber = model.MobileNumber;
                info.MobileNumber2 = model.MobileNumber2;
                info.MobileNumber3 = model.MobileNumber3;


                info.CellNumber = model.CellNumber;
                info.CellNumber2 = model.CellNumber2;
                info.CellNumber3 = model.CellNumber3;




                info.SMSNumber = model.SMSNumber;
                info.Fax = model.Fax;
                info.Website = model.Website;
                info.Email = model.Email;
                info.Telegram = model.Telegram;
                info.CityId = model.CityId;
                info.ZipCode = model.ZipCode;
                info.Street = model.Street;
                info.FullAddress = model.FullAddress;
                info.Pelak = model.Pelak;
                info.Lat = model.Lat;
                info.Lng = model.Lng;
                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        public async Task<ApiResult<CompanyAddressInfo>> GetAddressInfo(int companyId)
        {
            var res = new ApiResult<CompanyAddressInfo>(true, ApiResultStatusCode.Success, new CompanyAddressInfo());
            var info = new CompanyAddressInfo();


            try
            {
                var model = await _company.Include(i => i.City).ThenInclude(i => i.Parent)
                    .FirstOrDefaultAsync(w => w.Id == companyId);
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.CompanyId = model.Id;
                info.MobileNumber = model.MobileNumber;
                info.MobileNumber2 = model.MobileNumber2;
                info.MobileNumber3 = model.MobileNumber3;


                info.CellNumber = model.CellNumber;
                info.CellNumber2 = model.CellNumber2;
                info.CellNumber3 = model.CellNumber3;



                info.SMSNumber = model.SMSNumber;
                info.Fax = model.Fax;
                info.Website = model.Website;
                info.Email = model.Email;
                info.Telegram = model.Telegram;

                if (model.City != null && model.City.Parent != null)
                {
                    info.Province = new BaseDataModel()
                    {
                        Key = model.City.ParentId.Value.ToString()
                        ,
                        Text = model.City.Parent.Title
                    };
                }
                info.CityId = model.CityId;
                info.City = model.City == null ? "" : model.City.Title;
                info.ZipCode = model.ZipCode;
                info.Street = model.Street;
                info.FullAddress = model.FullAddress;
                info.Pelak = model.Pelak;
                info.Lng = model.Lng;
                info.Lat = model.Lat;
                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        #endregion
        #region اطلاعات پایه
        public async Task<ApiResult> UpdateBaseInfo(CompanyBaseInfo model,bool isAdmin=true)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                if (info.UserCompanyAccountStatus ==  UserCompanyAccountStatusEnum.تایید_شده && isAdmin==false)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات شما تایید شده است و امکان ویرایش وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
                numberFormat.NumberDecimalSeparator = ".";

                info.BuildingLicenseArea = model.BuildingLicenseArea;
                info.BuildingArea = model.BuildingArea;
                info.AreaOfGreenSpace = model.AreaOfGreenSpace;
                info.UnitArea = model.UnitArea;
                info.EarthCondition = model.EarthCondition;
                info.ManagerName = model.ManagerName;
                info.ManagerNationCode = model.ManagerNationCode;

                info.FieldOfActivity = model.FieldOfActivity;
                info.ActivityLicenseDate = model.ActivityLicenseDate;
                info.ActivityLicense = model.ActivityLicense;

                info.CompanyOwnerType = model.CompanyOwnerType;
                info.ActivityLicenseType = model.ActivityLicenseType;
                info.TxtRegNO = model.TxtRegNO;
                info.TxtTinNo = model.TxtTinNo;
                info.EstablishedYear = model.EstablishedYear;
                info.EnglishName = model.EnglishName;
                info.CompanyName = model.CompanyName;

                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        public async Task<ApiResult<CompanyBaseInfo>> GetBaseInfo(int companyId)
        {
            var res = new ApiResult<CompanyBaseInfo>(true, ApiResultStatusCode.Success, new CompanyBaseInfo());
            var info = new CompanyBaseInfo();


            try
            {
                var model = await _company.FirstOrDefaultAsync(w => w.Id == companyId);
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.CompanyId = model.Id;
                info.BuildingLicenseArea = model.BuildingLicenseArea;
                info.BuildingArea = model.BuildingArea;
                info.AreaOfGreenSpace = model.AreaOfGreenSpace;
                info.UnitArea = model.UnitArea;
                info.EarthCondition = model.EarthCondition;
                info.ManagerName = model.ManagerName;
                info.ManagerNationCode = model.ManagerNationCode;

                info.FieldOfActivity = model.FieldOfActivity;
                info.ActivityLicenseDate = model.ActivityLicenseDate;
                info.ActivityLicense = model.ActivityLicense;

                info.CompanyOwnerType = model.CompanyOwnerType;
                info.ActivityLicenseType = model.ActivityLicenseType;
                info.TxtRegNO = model.TxtRegNO;
                info.TxtTinNo = model.TxtTinNo;
                info.EstablishedYear = model.EstablishedYear;
                info.EnglishName = model.EnglishName;
                info.CompanyName = model.CompanyName;


                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }

        #endregion
        #region اطلاعات تکمیلی
        public async Task<ApiResult> UpdateAdditionalInfo(CompanyAdditionalInfo model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                //کد قرارداد نباید تکراری باشد
                if(await _company.AnyAsync(w=>w.ContractCode==model.ContractCode  && w.Id!=model.CompanyId ))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد قرارداد وارد شده تکراری است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                //شماره پرونده نباید تکراری باشد

                if (await _company.AnyAsync(w => w.FileCode == model.FileCode && w.Id != model.CompanyId))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره پرونده وارد شده تکراری است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }




                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                info.ContractCode = model.ContractCode;
                info.ContractOnDate = model.ContractOnDate;
                info.WaterContractOnDate = model.WaterContractOnDate;
                info.FileCode = model.FileCode;
                info.WaterDepositId = model.WaterDepositId;
                info.WaterCode = model.WaterCode;
                info.ChargeDepositId = model.ChargeDepositId;

                info.ChargeCode = model.ChargeCode;
                info.ChargeMoeinCode = model.ChargeMoeinCode;
                info.VolumeAirTanks = model.VolumeAirTanks;

                info.IsBusinessUnit = model.IsBusinessUnit;
                info.IsBuildingCompany = model.IsBuildingCompany;
                info.IssueWaterBill = model.IssueWaterBill;
                info.IssueChargeBill = model.IssueChargeBill;
                

                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<CompanyAdditionalInfo>> GetAdditionalInfo(int companyId)
        {
            var res = new ApiResult<CompanyAdditionalInfo>(true, ApiResultStatusCode.Success, new CompanyAdditionalInfo());
            var info = new CompanyAdditionalInfo();


            try
            {
                var model = await _company.FirstOrDefaultAsync(w => w.Id == companyId);
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.CompanyId = model.Id;
                info.ContractCode = model.ContractCode;
                info.ContractOnDate = model.ContractOnDate;
                info.WaterContractOnDate = model.WaterContractOnDate;
                info.FileCode = model.FileCode;
                info.WaterDepositId = model.WaterDepositId;
                info.WaterCode = model.WaterCode;
                info.ChargeDepositId = model.ChargeDepositId;

                info.ChargeCode = model.ChargeCode;
                info.ChargeMoeinCode = model.ChargeMoeinCode;
                info.VolumeAirTanks = model.VolumeAirTanks;

                info.IsBusinessUnit = model.IsBusinessUnit;
                info.IsBuildingCompany = model.IsBuildingCompany;
                info.IssueWaterBill = model.IssueWaterBill;
                info.IssueChargeBill = model.IssueChargeBill; 
                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }



    

        #endregion
        #region فعالیتهای شرکت
        public async Task<ApiResult<UserCompanyActivitiyInfo>> AddCompanyActivity(AddUserCompanyActivities model)
        {
            var res = new ApiResult<UserCompanyActivitiyInfo>(true, ApiResultStatusCode.Success, new UserCompanyActivitiyInfo(), "");
            try
            {

                if (await _activities.AnyAsync(w => w.ActivityId == model.ActivityId
              && w.UserCompanyId == model.UserCompanyId))
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                }
                var activty = new UserCompanyActivities()
                {
                    ActivityId = model.ActivityId,
                    UserCompanyId = model.UserCompanyId.Value,


                };



                await _activities.AddAsync(activty);
                await _uow.SaveChangesAsync();
                res.Data.Id = activty.Id;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<string>> RemoveActivitiy(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");


            try
            {
                var info = await _activities.FirstOrDefaultAsync(w => w.Id == id);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }

                _activities.Remove(info);
                await _uow.SaveChangesAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
        public async Task<ApiResult<List<UserCompanyActivitiyInfo>>> GetCompanyActivity(int companyId)
        {
            var res = new ApiResult<List<UserCompanyActivitiyInfo>>(true, ApiResultStatusCode.Success, new List<UserCompanyActivitiyInfo>());


            try
            {
                res.Data = await _activities.Where(w => w.UserCompanyId == companyId).Select(s => new UserCompanyActivitiyInfo()
                {
                    Activity = s.Activity.Title,
                    ActivityId = s.ActivityId,
                    UserCompanyId = s.UserCompanyId,
                    CompanyName = s.UserCompany.CompanyName,
                    Id = s.Id

                }).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<List<BaseDataModel>>> GetActivitiyList()
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _fieldOfActivity.Select(s => new BaseDataModel()
                {
                    Text = s.Title,
                    Key = s.Id.ToString(),
                }).ToListAsync();


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "در واکشی اطلاعات خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }


        #endregion


        public   async Task<ApiResult<CompanyLogo>> UpdateCompanyLogo(CompanyLogo model)
        {
            var res = new ApiResult<CompanyLogo>(true, ApiResultStatusCode.Success,new CompanyLogo());
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                 
                info.ImageUrl = model.ImageUrl;
                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
                res.Data.CompanyId = model.CompanyId;
                res.Data.ImageUrl = model.ImageUrl;
                res.Data.CompanyName = model.CompanyName;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<CompanyLogo>> GetCompanyLogo(int companyId)
        {
            var res = new ApiResult<CompanyLogo>(true, ApiResultStatusCode.Success, new CompanyLogo());
            try
            {
                var info = await _company.Where(W => W.Id == companyId).Select(s => new CompanyLogo()
                {
                    
                    CompanyId = s.Id,
                    CompanyName = s.CompanyName, 
                    ImageUrl = s.ImageUrl,
                    
                }).FirstOrDefaultAsync();

                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                } 

                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }




        public async Task<ApiResult<CompanyShortInfo>> GetCompanyShortInfo(int companyId)
        {
            var res = new ApiResult<CompanyShortInfo>(true, ApiResultStatusCode.Success, new CompanyShortInfo());
            try
            {
                var info = await _company.Where(W => W.Id == companyId).Select(s => new CompanyShortInfo()
                {

                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    ImageUrl = s.ImageUrl,
                    CreatedOnDate=s.CreatedOnDate,
                    ManagerName=s.ManagerName,
                    SlagUrl=s.SlagUrl,
                    TxtRegNO=s.TxtRegNO,
                    TxtTinNo=s.TxtTinNo,
                    UserCompanyStatus=s.UserCompanyStatus, 
                }).FirstOrDefaultAsync();

                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }









        public async Task<ApiResult<CompanySignatureInfo>> UpdateCompanySignature(CompanySignatureInfo model)
        {
            var res = new ApiResult<CompanySignatureInfo>(true, ApiResultStatusCode.Success, new CompanySignatureInfo());
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.SignatureUrl = model.SignatureUrl;
                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
                res.Data.CompanyId = model.CompanyId;
                res.Data.SignatureUrl = model.SignatureUrl;
                res.Data.CompanyName = model.CompanyName;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<CompanySignatureInfo>> GetCompanySignature(int companyId)
        {
            var res = new ApiResult<CompanySignatureInfo>(true, ApiResultStatusCode.Success, new CompanySignatureInfo());
            try
            {
                var info = await _company.Where(W => W.Id == companyId).Select(s => new CompanySignatureInfo()
                {

                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    SignatureUrl = s.SignatureUrl,

                }).FirstOrDefaultAsync();

                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }




        public async Task<ApiResult<CompanyContractInfo>> UpdateCompanyContract(CompanyContractInfo model)
        {
            var res = new ApiResult<CompanyContractInfo>(true, ApiResultStatusCode.Success, new CompanyContractInfo());
            try
            {
                var info = await _company.FirstOrDefaultAsync(w => w.Id == model.CompanyId);
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                info.ContractUrl = model.ContractUrl;
                info.LastModifiedOnDate = DateTime.Now;
                _company.Update(info);
                await _uow.SaveChangesAsync();
                res.Data.CompanyId = model.CompanyId;
                res.Data.ContractUrl = model.ContractUrl;
                res.Data.CompanyName = model.CompanyName;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult<CompanyContractInfo>> GetCompanyContract(int companyId)
        {
            var res = new ApiResult<CompanyContractInfo>(true, ApiResultStatusCode.Success, new CompanyContractInfo());
            try
            {
                var info = await _company.Where(W => W.Id == companyId).Select(s => new CompanyContractInfo()
                {

                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    ContractUrl = s.ContractUrl,

                }).FirstOrDefaultAsync();

                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شرکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }


        public async Task<ApiResult> Remove(int Id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _company.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                }

                item.IsDeleted = true;
                _company.Update(item);
                await _uow.SaveChangesAsync(); 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }




    }
}
