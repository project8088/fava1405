using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nikan.Common;
using Nikan.Common.ApiCall;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Utilities;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.ViewModel; 
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.Group;
using Nikan.ViewModel.Report;
using Nikan.ViewModel.UserCompanes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Nikan.Services.Citizens
{

    public interface ICitizenService
    {
        Task<ApiResult<string>> AcceptCitizenPicture(int citizenId);

        Task<ApiResult> AddCitizenAddress(AddressDto model, int citizenId);

        Task<ApiResult> AddOrUpdateCitizenProfile(CitizenProfileDto model, int citizenId);

        Task<ApiResult<PagedCitizenViewModel>> advancedSearch(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
          DateTime? birthDateToDate = null,
          string name = null,
          string lastname = null,
          string nationCode = null,
          string mobile = null,
          bool? gender = null,
          int? nationality = null,
          int? cityId = null,
          int[] groupIds = null,
          int? region = null,
          SabtStatusEnum? sabtStatus = null,
          PersonalPictureEnum? pictureConfirmed = null,
          MaritalStatusEnum? mariageStatus = null,
          int? registerByService = null,
          int? groupId = null,
          bool? hasFamily = null,
          bool? faceAuthentication = null);

        Task<ApiResult<string>> ChangeCitizenPicture(int citizenId);

        Task<ApiResult<PreRegisterResult>> CheckCitizenRegister(
          PreRegisterDto model);

        Task<ApiResult<IsCitizenIsRegisterResult>> CheckRegisterCitizenByNtionCode(
          IsCitizenIsRegister model);

        Task<ApiResult> CheckValidMobileNumberForCitizenRegister(string mobileNumber);

        Task<ApiResult<User>> CitizenRegister(CitizensRegisterDto model, int? userId);

        Task<ApiResult<AdminDashbordStatisticalReport>> CountForReport();

        Task<ApiResult<AddressInfo>> GeAddress(int id);

        Task<ApiResult<CitizenProfileInfo>> GetCitizenProfile(
          int citizenId);

        Task<ApiResult<List<AddressInfo>>> GetCitizenAddress(int citizenId);

        Task<ApiResult<BankAccountCardNumberDto>> GetCitizenBankCardNumber(
          int citizenId);

        Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfo(int citizenId);

        Task<ApiResult<UpdateEmailAddressDto>> GetCitizenEmail(
          int citizenId);

        Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfo(int citizenId);

        Task<ApiResult<AddressInfo>> GetCitizenHomeAddress(int citizenId);

        Task<ApiResult<UpdateMobileNumberDto>> GetCitizenMobileNumber(
          int citizenId);

        Task<ApiResult<AddressInfo>> GetCitizenOfficeAddress(int citizenId);

        Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfoByUserCode(
          string userCode);

        Task<ApiResult<string>> GetMobileNumber(int citizenId);

        Task<ApiResult<CitizenBaseInfo>> GetMyFamilyBaseInfo(
          int mycitizenId,
          int myFamilyid);

        Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfo(int citizenId);

        Task<ApiResult<ShortCitizenInfo>> RejectCitizenPicture(
          Nikan.ViewModel.Citizens.RejectCitizenPicture model);

        Task<ApiResult> RemoveCitizenAddress(int id, int citizenId);

        Task<ApiResult> RemoveCitizenAddressByAdmin(int id);

        Task<ApiResult<PagedCitizenViewModel>> SearchCitizens(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string name = null,
          string nationCode = null,
          int? groupId = null);

        Task<ApiResult<PagedImageCitizenViewModel>> SearchImageCitizens(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          PersonalPictureEnum? pictureConfirmed = null,
          string nationCode = null,
          bool? HasCard = null,
          int? groupId = null);

        Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddress(
          AddressDto model,
          int citizenId);

        Task<ApiResult<string>> UpdateCitizenBankCardNumber(
          UpdateBankAccountCardNumberDto model);

        Task<ApiResult<string>> UpdateCitizenByCitizen(
          EditCitizenViewModel model,
          int citizenId);

        Task<ApiResult<string>> UpdateCitizenEmailAddress(UpdateEmailAddressDto model);

        Task<ApiResult<string>> UpdateCitizenMobileNumber(UpdateMobileNumberDto model);

        Task<ApiResult<PagedCitizenGroupsViewModel>> GetGroupsCitizensInfo(
          string userCode,
          int pageNumber,
          int pageSize);

        Task<ApiResult<string>> RemoveCitizenFromGroup(GroupsCitizensDto model, int userId);

        Task<ApiResult<string>> AddCitizenToGroup(GroupsCitizensDto model, int userId);

        Task<ApiResult<string>> UpdateCitizenByAdmin(
          EditCitizenViewModel model,
          int byUserId);

        Task<ApiResult> AddOrUpdateCitizenProfileByAdmin(CitizenProfileDto model);

        Task<ApiResult<IsCitizenIsRegisterResult>> CheckCitizenPicture(
          int citizenId);

        Task<ApiResult<User>> QuickCitizenRegister(
          QuickCitizensRegisterDto model,
          int? userId);

        Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByTicket(
          string ticket);

        Task<ApiResult<List<ShortGroupsCitizensInfo>>> GetShortGroupsCitizensInfo(
          int citizenId);

        Task<ApiResult<string>> UpdateCitizenByWebApiUser(
          EditCitizenViewModel model,
          int userId);

        Task<ApiResult> ConfirmCitizenFile(int importId, int userId);

        Task<ApiResult<UploadFileResult>> AddCitizenImportFile(
          List<CitizenExcelFileColumns> list,
          string fileName,
          int userId);

        Task<ApiResult<CitizenImportPagedList>> CitizenImportFileDetails(
          int pageNumber,
          int pageSize,
          int importId,
          bool? isValidRow = null);

        Task<ApiResult<List<CitizenImportFileInfo>>> CitizenImportFileList();

        Task<ApiResult> ConfirmCompanyPersonelFile(int importId, int userId);

        Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByNationCode(
          string NationCode);

        Task<ApiResult<ShortCitizenInfo>> SearchCitizenByCardUser(
          string nationCode,
          DateTime? birthDate = null);

        Task<ApiResult<List<QueueCheckingCitizensDeadDto>>> GetCheckingCitizensDead(
          int top);

        Task<ApiResult> DeleteCheckingCitizensDead(List<CallCheckingCitizensDead> list);

        Task<ApiResult> UpdateCitizenAfterAuthentication(ItsaazData model, int userId);

        Task<ApiResult<List<ShortCitizenInfo>>> GetShortCitizenInfoByIds(
          List<int> ids);

        Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformation(
          int citizenId);

        Task<ApiResult<string>> UpdateIdentityInformation(
          CitizenIdentityInformation model,
          int citizenId,
          int byUserId);

        Task<ApiResult<string>> UpdateOtherBaseInfoByCitizen(
          EditOtherBaseInfoViewModel model,
          int citizenId);

        Task<ApiResult<string>> UpdateCitizenMobileNumberByAdmin(
          UpdateMobileNumberDto model,
          int userId);

        Task<ApiResult> UpdateCheckDeathStateCitizen(
          string nationCode,
          bool IsDeath,
          int? exportId = null);

        Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfo(
          string nationCode);

        Task<ApiResult> UpdateCitizenGroups(string nationcode);

        Task<ApiResult<HiChartData>> GetCitizenRegisterChartReport(
          BetweenDate model);

        Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfoByUserCode(
          string userCode);

        Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfo(string userCode);

        Task<ApiResult<CitizenProfileInfo>> GetCitizenProfile(
          string userCode);

        Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfoByCitizenId(
          int citizenId);

        Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfo(
          string userCode);

        Task<ApiResult> ConfigAllUserCode();

        Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformation(
          string userCode);

        Task<ApiResult<AddressInfo>> GetCitizenHomeAddress(string userCode);

        Task<ApiResult<AddressInfo>> GetCitizenOfficeAddress(string userCode);

        Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddress(
          AddressDto model,
          string userCode);

        Task<ApiResult<string>> UpdateIdentityInformation(
          CitizenIdentityInformation model,
          string userCode,
          int byUserId);

        Task<ApiResult<AddressInfo>> GeAddress(int citizenId, int id);

        Task<ApiResult<string>> UpdateSabtStatus(Nikan.ViewModel.Citizens.UpdateSabtStatus model, int byUserId);

        Task<ApiResult> UpdateUserCode(string code);

        Task<ApiResult<CitizenGroupsAndQueues>> GetCitizenGroupsAndQueues(
          string nationCode);

        Task<bool> CitizenMembershipInGroup(int citizenId, int groupId);

        Task<ApiResult<string>> AddCitizenToGroupByNationCode(
          GroupsCitizensDto model,
          int userId);
        Task<ApiResult<CitizenSabtStatus>> GetCitizenSabtStatus(string nationCode);
        Task<ApiResult<PagedCitizenViewModel>> SearchCitizensAuthentication(int pageNumber, int pageSize, DateTime? FromDate = null,
            DateTime? ToDate = null, string nationCode = null,int ? registerByService = null,
        SabtStatusEnum? sabtStatus = null);
    }
    public class CitizenService : ICitizenService
    {
        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<Nikan.DomainClasses.CitizensCard.CitizensCard> _citizensCard;
        private readonly DbSet<CardInfo_Export_Citizen> _cardExportDetails;

        private readonly DbSet<CitizensQueue> _citizensQueue;
        private readonly DbSet<GroupsCitizens> _citizenGroups;
        private readonly DbSet<Group> _groups;
        private readonly DbSet<ExportedCitizens> _exportCitizen;
        private readonly DbSet<Address> _address;
        private readonly DbSet<CitizenProfile> _profile;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _role;
        private readonly DbSet<UserRole> _userRole;
        private readonly DbSet<SmsInfo> _sms;
        private readonly DbSet<Nationality> _nationality;
        private readonly DbSet<ImportExcelFile> _fileExcel;
        private readonly DbSet<ImportExcelFileDetails> _fileDetails;

        private readonly DbSet<CitizenFamily> _family;
        private readonly DbSet<UserLoginTickets> _userLoginTickets;
        private readonly DbSet<UserLoginTickets_Archive> _userLoginTicketsArchive;
        private readonly DbSet<AppServices> _app;

        private readonly DbSet<Event> _event;
        private readonly DbSet<QueueCheckingCitizensDead> _queueCheckingCitizensDead;
       
        private readonly DbSet<CitizensAuthentication> _citizensAuthentication;
       
        private readonly ISecurityService _securityService;
        public CitizenService(IUnitOfWork uow,
            ISecurityService securityService,

            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _address = _uow.Set<Address>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _citizen = _uow.Set<Citizen>();
            _citizensQueue = _uow.Set<CitizensQueue>();
            _exportCitizen = _uow.Set<ExportedCitizens>();
            _citizensAuthentication = _uow.Set<CitizensAuthentication>();
            _citizensCard = _uow.Set<Nikan.DomainClasses.CitizensCard.CitizensCard>();
            _profile = _uow.Set<CitizenProfile>();
            _queueCheckingCitizensDead = _uow.Set<QueueCheckingCitizensDead>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _groups = _uow.Set<Group>();
            _event = _uow.Set<Event>();
            _userRole = _uow.Set<UserRole>();
            _role = _uow.Set<Role>();
            _sms = _uow.Set<SmsInfo>();
            _userLoginTickets = _uow.Set<UserLoginTickets>();
            _userLoginTicketsArchive = _uow.Set<UserLoginTickets_Archive>();
            _family = _uow.Set<CitizenFamily>();
            _nationality = _uow.Set<Nationality>();
            _securityService = securityService;
            _app = _uow.Set<AppServices>();
            _fileExcel = _uow.Set<ImportExcelFile>();
            _fileDetails = _uow.Set<ImportExcelFileDetails>();
            _cardExportDetails = _uow.Set<CardInfo_Export_Citizen>();
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        #endregion
        #region ثبت نام اطلاعات شهروند
        public async Task<ApiResult> ConfigAllUserCode()
        {

            /*
             var listsms = await _sms.ToListAsync();
            _sms.RemoveRange(listsms);

            var listevent = await _event.ToListAsync();
            _event.RemoveRange(listevent);


            var listmanzalat = await _manzalat.ToListAsync();
            _manzalat.RemoveRange(listmanzalat);

            var listfamily = await _family.ToListAsync();
            _family.RemoveRange(listfamily);
            await _uow.SaveChangesAsync();
             
             
             */
            var all = await _citizen.Include(i=>i.User).Where(w => w.UserCode == null).ToListAsync();
            if (all.Any())
            {
                foreach (var citizen in all)
                {
                    var guid = Guid.NewGuid();
                    var user = citizen.User;
                    if (user != null)
                    {
                        user.UserCode = guid;
                        _users.Update(user);
                    }
                    citizen.UserCode = guid;
                    
                    _citizen.Update(citizen);
                }
                await _uow.SaveChangesAsync();
            }




             all = await _citizen.Where(w => w.NationId == null).ToListAsync();
            if (all.Any())
            {
                foreach (var citizen in all)
                {
                    citizen.NationId = 0; 
                    _citizen.Update(citizen);
                }
                await _uow.SaveChangesAsync();

            }
             
            return new ApiResult(true, ApiResultStatusCode.Success, "برروزرسانی با موفقیت انجام گردید");

        }


        public async Task<ApiResult> CheckValidMobileNumberForCitizenRegister(string mobileNumber)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {


                if (string.IsNullOrWhiteSpace(mobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                mobileNumber = mobileNumber.Fa2En();
                var countValidMobileNumber = 0;
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(w => w.Key == "CountValidMobileNumber");
                if (settings != null)
                    countValidMobileNumber = int.Parse(settings.Value);

                if (countValidMobileNumber > 0)
                {
                    var countMobileNumber = await _citizen.CountAsync(a => a.Mobile == mobileNumber);
                    if (countMobileNumber >= countValidMobileNumber)
                    {
                        res.IsSuccess = false;
                        res.Messages = "این شماره موبایل بیش از حد مجاز استفاده شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }


            return res;

        }


        public async Task<ApiResult<PreRegisterResult>> CheckCitizenRegister(PreRegisterDto model)
        {
            //چک کردن کد ملی
            //چک کردن شماره موبایل
            //چک کردن شماره گذرنامه


            var res = new ApiResult<PreRegisterResult>(true, ApiResultStatusCode.Success, new PreRegisterResult(), "بررسی ثبت نام با موفقیت انجام گردید.");
            try
            {
                var nationcode = model.NationCode;
                var nationality = model.Nationality;
                var mobileNumber = model.MobileNumber;

                if (string.IsNullOrWhiteSpace(nationcode))
                {
                    res.IsSuccess = false;
                    if (nationality == 0)
                        res.Messages = "کد ملی را وارد نمایید";
                    else
                        res.Messages = "شماره گذرنامه را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                nationcode = model.NationCode.Fa2En();
                if (nationality == 0)
                {
                    var check = nationcode.IsValidNationalCode();
                    if (check != "")
                    {
                        res.IsSuccess = false;
                        res.Messages = check;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                else
                {
                    if (nationcode.Length < 5)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }
                    if (!long.TryParse(nationcode, out long vv))
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }


                if (string.IsNullOrWhiteSpace(mobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                mobileNumber = model.MobileNumber.Fa2En();
                if (!model.MobileNumber.StartsWith("09"))
                {
                    res.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                var countValidMobileNumber = 0;
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(w => w.Key == "CountValidMobileNumber");
                if (settings != null)
                    countValidMobileNumber = int.Parse(settings.Value);

                if (countValidMobileNumber > 0)
                {
                    var countMobileNumber = await _citizen.CountAsync(a => a.Mobile == mobileNumber);
                    if (countMobileNumber >= countValidMobileNumber)
                    {
                        res.Data.IsCanRegister = false;
                        res.Data.Description = "این شماره موبایل بیش از حد مجاز استفاده شده است";
                        return res;
                    }

                }




                var citizen = await _citizen.Where(a => a.NationCode == nationcode).FirstOrDefaultAsync();

                if (citizen != null)
                {
                    var msg = "شما قبلا ثبت نام کرده اید ";
                    if (!string.IsNullOrEmpty(citizen.Mobile))
                    {
                        var mobilenumber = citizen.Mobile.Remove(3, 4);
                        mobilenumber = "[" + mobilenumber.Insert(3, "----") + "]";
                        msg = "  شما قبلا با شماره موبایل  " + mobilenumber + "در سامانه ثبت نام کرده اید";
                    }

                    res.Data.IsCanRegister = false;
                    res.Data.Description = msg;
                    return res;


                }

                res.Data.IsCanRegister = true;
                res.Data.Description = "امکان ثبت نام برای شهروند وجود دارد";
                return res;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }



        public async Task<ApiResult> UpdateCitizenGroups(string nationcode)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"برروزرسانی گروهها با موفقیت انجام گردید");
            try
            {

                var citizen  = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationcode);
                if(citizen==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                if (!await _citizenGroups.AnyAsync(w =>w.IsDeleted!=true && w.CitizenId == citizen.CitizenId && w.GroupId == 22))
                {
                    //اضافه کردن نقش  کاربران عادی به شهروند 
                    await _citizenGroups.AddAsync(new GroupsCitizens()
                    {
                        CitizenId = citizen.CitizenId,
                        GroupId = 22,
                        AddByUserId =  1 ,  // 1 = کاربر سیستمی
                        CreationDate = DateTime.Now,
                    });

                }


                var listcitizensQueue = await _citizensQueue.Where(w => w.NationCode == nationcode).ToListAsync();
                if (listcitizensQueue.Any())
                {
                    foreach (var queue in listcitizensQueue)
                    {
                        if (await _citizenGroups.AnyAsync(w => w.GroupId == queue.GroupId && w.Citizen.NationCode == nationcode))
                        {
                            //اگر وجود داشت صف حذف کن
                            _citizensQueue.Remove(queue);
                        }
                        else
                        {
                            //اضافه کردن شهروند به گروه
                            await _citizenGroups.AddAsync(new GroupsCitizens()
                            {
                                CitizenId = citizen.CitizenId,
                                GroupId = queue.GroupId,
                                AddByUserId = queue.AddByUserId==null ? 1 : queue.AddByUserId, // 1 = کاربر سیستمی
                                CreationDate = DateTime.Now,
                            });
                            //شهروند اضافه کردی حالا صف حذف کن
                            _citizensQueue.Remove(queue);

                        }
                    }
                }




                await _uow.SaveChangesAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;

            }

            return res;
        }


        public async Task<ApiResult<User>> CitizenRegister(CitizensRegisterDto model,int? userId)
        {



            var res = new ApiResult<User>(true, ApiResultStatusCode.Success, new User());
            var errorline = "1";
            try
            {
                var date = DateTime.Now.AddDays(-1);
                var olddate = DateTime.Now.AddYears(-110);
                if (model.BirthDate >= date)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را به صورت صحیح وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (model.BirthDate <= olddate)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را به صورت صحیح وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                errorline = "2";
                var nationcode = model.NationCode;
                var nationality = model.Nationality;
                var mobileNumber = model.MobileNumber;

                if (string.IsNullOrWhiteSpace(nationcode))
                {
                    res.IsSuccess = false;
                    if (nationality == 0)
                        res.Messages = "کد ملی را وارد نمایید";
                    else
                        res.Messages = "شماره گذرنامه را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                nationcode = model.NationCode.Fa2En();
                if (nationality == 0)
                {
                    var check = nationcode.IsValidNationalCode();
                    if (check != "")
                    {
                        res.IsSuccess = false;
                        res.Messages = check;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                else
                {
                    if (nationcode.Length < 5)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }
                    if (!long.TryParse(nationcode, out long vv))
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                if (string.IsNullOrWhiteSpace(mobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                mobileNumber = model.MobileNumber.Fa2En();
                if (!model.MobileNumber.StartsWith("09"))
                {
                    res.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                errorline = "3";
                var countValidMobileNumber = 0;
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(w => w.Key == "CountValidMobileNumber");
                if (settings != null)
                    countValidMobileNumber = int.Parse(settings.Value);

                if (countValidMobileNumber > 0)
                {
                    var countMobileNumber = await _citizen.CountAsync(a => a.Mobile == mobileNumber);
                    if (countMobileNumber >= countValidMobileNumber)
                    {
                        res.IsSuccess = false;
                        res.Messages = "این شماره موبایل بیش از حد مجاز استفاده شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                errorline = "4";
                var firstName = model.FirstName.FixFullString();
                var lastName = model.LastName.FixFullString();
                var fatherName = model.FatherName.FixFullString();
                errorline = "5";

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام خانوادگی را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                if (string.IsNullOrWhiteSpace(fatherName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام پدر را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                errorline = "6";

                if (await _users.AnyAsync(w => w.Username == nationcode))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی وارد شده تکراری می باشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                errorline = "7";
                var citizenRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Citizen);
                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = firstName + " " + lastName,
                    Username = nationcode,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = mobileNumber,
                    EmailAddress = model.EMail,
                    EmailVerification = false,
                    MobileNumberVerification = true,
                    PasswordQuestion = model.PasswordQuestion,
                    PasswordAnswer = model.PasswordAnswer,
                    RegisterByUserId=userId,
                    UserCode=Guid.NewGuid()


                };

                var add = await _users.AddAsync(user);
                if (citizenRole != null)
                {
                    var role = new UserRole() { RoleId = citizenRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }






                var citizen = new Citizen()
                {
                    BirthDate = model.BirthDate,
                    JobGroupId = model.JobGroup,
                    FatherName = fatherName,
                    FirstName = firstName,
                    CreationDate = DateTime.Now,
                    Gender = model.Gender,
                    JobTitle = model.JobTitle,
                    LastName = lastName,
                    Mobile = mobileNumber,
                    MariageStatus = model.MariageStatus,
                    NationId = model.Nationality,
                    NationCode = nationcode,
                    EducationField = model.EducationTitle,
                    EducationGroupId = model.EducationGroup,
                    EducationLevel = model.EducationLevel,
                    //0 پروفایل شهروندی -پیش فرض
                    RegisterByServiceId = model.ServiceId == null ? 0 : model.ServiceId.Value,
                    User = user,
                    UserCode=user.UserCode,
                    SabtStatus = SabtStatusEnum.استعلام_نشده,
                    LastUpdateOnDate = DateTime.Now,


                };
                errorline = "8";
                await _citizen.AddAsync(citizen);

                var fullAddress = "";
                if (!string.IsNullOrWhiteSpace(model.Street))
                    fullAddress += " "+" خیابان "+ model.Street;
                if (!string.IsNullOrWhiteSpace(model.Alley))
                    fullAddress += " " + " کوچه " + model.Alley;
              
                if (!string.IsNullOrWhiteSpace(model.Plaque))
                    fullAddress += " " + " پلاک " + model.Plaque;

                if (!string.IsNullOrWhiteSpace(model.PostalCode))
                    fullAddress += " " + " کدپستی " + model.PostalCode;


                if (model.CityId == null)
                    model.CityId = 7;//اگر شهر محل زندگی شهروند مشخص نشده بود به صورت پیش فرض اصفهان در نظر بگیر


                var address = new Address()
                {
                    AddressType = AddressTypeEnum.منزل,
                    Citizen = citizen,
                    CityId = model.CityId.Value,
                    CreationDate = DateTime.Now,
                    FullAddress = fullAddress,
                    IsActive = true,
                    Alley = model.Alley,
                    Street = model.Street,
                    Plaque = model.Plaque,
                    IsVerified = false,
                    PostalCode = model.PostalCode,
                    Region = model.Region,
                    LasteUpdateOnDate = DateTime.Now,
                    Phone = model.PhoneNumber,
                };
                await _address.AddAsync(address);

                errorline = "9";
                res.Data = user;
               




                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                
                var st = new StackTrace(er, true); 
                var frame = st.GetFrame(0); 
                var line = frame.GetFileLineNumber();

                string output = JsonConvert.SerializeObject(model);
                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
                //save log 
                await _event.AddAsync(new Event()
                {
                    ActionName = "CitizenRegister",
                    CreateDate = DateTime.Now,
                    Description = "Error Line " + line + ">>" + er.Message,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = model.NationCode,
                    EventType = EventTypeEnum.Error,
                    JsonValue = output

                });
                await _uow.SaveChangesAsync();


            }

            return res;
        }

        public async Task<ApiResult<User>> QuickCitizenRegister(QuickCitizensRegisterDto model, int? userId)
        {

            var res = new ApiResult<User>(true, ApiResultStatusCode.Success, new User());
            var errorline = "6";


            try
            {
                errorline = "7";

                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "مدل ورودی نامعتبر می باشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                if (model.ServiceId == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه خدمت را مشخص کنید ";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (model.ServiceId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه خدمت را مشخص کنید ";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                var date = DateTime.Now.AddDays(-1);
                var olddate = DateTime.Now.AddYears(-100);
                if (model.BirthDate >= date)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را به صورت صحیح وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (model.BirthDate <= olddate)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را به صورت صحیح وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                errorline = "8";
                var nationcode = model.NationCode;
                var nationality = model.Nationality;
                var mobileNumber = model.MobileNumber;

                if (string.IsNullOrWhiteSpace(nationcode))
                {
                    res.IsSuccess = false;
                    if (nationality == 0)
                        res.Messages = "کد ملی را وارد نمایید";
                    else
                        res.Messages = "شماره گذرنامه را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                nationcode = model.NationCode.Fa2En();
                if (nationality == 0)
                {
                    var check = nationcode.IsValidNationalCode();
                    if (check != "")
                    {
                        res.IsSuccess = false;
                        res.Messages = check;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                else
                {
                    if (nationcode.Length < 7)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }
                    if (!long.TryParse(nationcode, out long vv))
                    {
                        res.IsSuccess = false;
                        res.Messages = "شماره گذرنامه نامعتبر می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                if (string.IsNullOrWhiteSpace(mobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                mobileNumber = model.MobileNumber.Fa2En();
                if (!model.MobileNumber.StartsWith("09"))
                {
                    res.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                errorline = "9";
                var countValidMobileNumber = 0;
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(w => w.Key == "CountValidMobileNumber");
                if (settings != null)
                    countValidMobileNumber = int.Parse(settings.Value);

                if (countValidMobileNumber > 0)
                {
                    var countMobileNumber = await _citizen.CountAsync(a => a.Mobile == mobileNumber);
                    if (countMobileNumber >= countValidMobileNumber)
                    {
                        res.IsSuccess = false;
                        res.Messages = "این شماره موبایل بیش از حد مجاز استفاده شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }
                errorline = "10";
                var firstName = model.FirstName.FixFullString();
                var lastName = model.LastName.FixFullString();
                var fatherName = model.FatherName.FixFullString();
                errorline = "11";

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام خانوادگی را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                if (string.IsNullOrWhiteSpace(fatherName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام پدر را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                if (await _users.AnyAsync(w => w.Username == nationcode))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی وارد شده تکراری می باشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }


                var citizenRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Citizen);
                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = firstName + " " + lastName,
                    Username = nationcode,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = mobileNumber,
                    EmailAddress = model.EMail,
                    EmailVerification = false,
                    MobileNumberVerification = true,
                    RegisterByUserId=userId,
                    UserCode=Guid.NewGuid()
                    
                };

                var add = await _users.AddAsync(user);
                if (citizenRole != null)
                {
                    var role = new UserRole() { RoleId = citizenRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }


                var citizen = new Citizen()
                {
                    BirthDate = model.BirthDate,
                    FatherName = fatherName,
                    FirstName = firstName,
                    CreationDate = DateTime.Now,
                    Gender = model.Gender,
                    LastName = lastName,
                    Mobile = mobileNumber,
                    NationId = model.Nationality,
                    NationCode = nationcode,
                    //0 پروفایل شهروندی -پیش فرض
                    RegisterByServiceId = model.ServiceId == null ? 0 : model.ServiceId.Value,
                    User = user,
                    UserCode = user.UserCode,
                    SabtStatus = SabtStatusEnum.استعلام_نشده,
                    LastUpdateOnDate = DateTime.Now,

                };

                await _citizen.AddAsync(citizen);

                 


                await _uow.SaveChangesAsync();
                errorline = "12";
                res.Data = user;
            }
            catch (Exception er)
            {
                string output = JsonConvert.SerializeObject(model);

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
                await _event.AddAsync(new Event()
                {
                    ActionName = "QuickCitizenRegister",
                    CreateDate = DateTime.Now,
                    Description = "Error Line " + errorline + ">>" + er.Message,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = model.NationCode,
                    EventType = EventTypeEnum.Error,
                    JsonValue = output




                });
                await _uow.SaveChangesAsync();

            }

            return res;
        }


        public async Task<ApiResult> ConfirmCitizenFile(int importId, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");


            try
            {
                //کل سرویس ها
                var appids = await _app.Where(w => w.IsDeleted != true).Select(s => s.Id).ToListAsync();
                var grpIds = await _groups.Where(w => w.IsDeleted != true).Select(s => s.Id).ToListAsync();
                var citizenRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Citizen);
                var add = 0;

                var list = await _fileDetails.Where(w => w.IsValidRow && w.ImportExcelFileId == importId).ToListAsync();
                if (list.Any())
                {

                    foreach (var item in list)
                    {
                        if (await _citizen.AnyAsync(w => w.NationCode == item.NationCode))
                        {
                            continue;
                        }
                        var serviceId = 0;
                        var groupId = 0;
                        if (item.ServiceId != 0 && item.ServiceId != null)
                        {
                            if (appids.Contains(item.ServiceId.Value))
                            {
                                serviceId = item.ServiceId.Value;
                            }
                        }
                        if (item.GroupId != 0 && item.GroupId != null)
                        {
                            if (grpIds.Contains(item.GroupId.Value))
                            {
                                groupId = item.GroupId.Value;
                            }
                        }
                        var passwordHash = _securityService.GetSha256Hash(item.NationCode);
                        var user = new User
                        {
                            DisplayName = item.FirstName + " " + item.LastName,
                            Username = item.NationCode,
                            UserAccountState = userAccountStateEnum.فعال,
                            Password = passwordHash,
                            SerialNumber = Guid.NewGuid().ToString("N"),
                            CreatedOnDate = DateTime.Now,
                            MobileNumber = item.Mobile,
                            EmailAddress = "",
                            EmailVerification = false,
                            MobileNumberVerification = true,
                            PasswordQuestion = "",
                            PasswordAnswer = "",
                            UserCode=Guid.NewGuid()
                        };
                        if (citizenRole != null)
                        {
                            var role = new UserRole() { RoleId = citizenRole.Id, User = user };
                            await _userRole.AddAsync(role);
                        }
                        await _users.AddAsync(user);
                        var citizen = new Citizen()
                        {
                            
                            BirthDate = item.BirthDate,
                            FatherName = item.FatherName,
                            FirstName = item.FirstName,
                            CreationDate = DateTime.Now,
                            Gender = item.Gender,
                            LastName = item.LastName,
                            Mobile = item.Mobile,
                            NationId = 0,
                            NationCode = item.NationCode,
                            //0 پروفایل شهروندی -پیش فرض
                            RegisterByServiceId = serviceId,
                            User = user,
                            UserCode = user.UserCode,
                            SabtStatus = SabtStatusEnum.استعلام_نشده,
                            LastUpdateOnDate = DateTime.Now,

                        };
                        await _citizen.AddAsync(citizen);
                        if (groupId != 0)
                        {
                            await _citizenGroups.AddAsync(new GroupsCitizens()
                            {
                                GroupId = groupId,
                                Citizen = citizen,
                                CreationDate = DateTime.Now,
                                AddByUserId = userId,


                            });
                        }
                       


                        add++;




                    }

                    if (add > 0)
                    {
                        var importFile = await _fileExcel.FirstOrDefaultAsync(w => w.Id == importId);
                        importFile.IsConfirmed = true;
                        await _uow.SaveChangesAsync();
                        var message = "  تعداد " + add + " با موفقیت ثبت شد ";
                        res.Messages = message;
                    }

                }

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
        #region دریافت اطلاعات شهروند
        public async Task<ApiResult<string>> GetMobileNumber(int citizenId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var mobile = await _citizen.Where(w => w.CitizenId == citizenId).Select(s => s.Mobile).FirstOrDefaultAsync();
                res.Data = mobile;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }


        public async Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfo(int citizenId)
        {
            var res = new ApiResult<CitizenBaseInfo>(true, ApiResultStatusCode.Success, new CitizenBaseInfo());
            try
            {
                var info = await _citizen.Where(w =>
                w.CitizenId == citizenId).Select(s => new CitizenBaseInfo()
                {
                    BirthDate = s.BirthDate,
                    CitizenId = s.CitizenId,
                    UserCode=s.UserCode,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    EducationField = s.EducationField,
                    EducationGroup = s.EducationGroup.Title,
                    EducationGroupId = s.EducationGroupId,
                    EducationLevel = s.EducationLevel,
                    EducationStatues = s.EducationStatues,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    JobGroup = s.JobGroup.Title,
                    JobGroupId = s.JobGroupId,
                    JobTitle = s.JobTitle,
                    LastName = s.LastName,
                    MariageStatus = s.MariageStatus,
                    MobileNumber = s.Mobile,
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPicture_DisapprovalReason = s.PersonalPicture_DisapprovalReason,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    User = s.User.Username,
                    LastUpdateOnDate = s.LastUpdateOnDate,

                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var citizenAddress = await _address
                    .Include(w => w.City).ThenInclude(t => t.Parent).Where(w => w.CitizenId ==
                        citizenId && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                if (citizenAddress != null)
                {
                    info.FullAddress = citizenAddress.FullAddress;
                    info.PostalCode = citizenAddress.PostalCode;
                    info.Region = citizenAddress.Region;
                    info.City = new ViewModel.BaseDataModel { Key = citizenAddress.CityId.ToString(), ParentText = citizenAddress.City.Parent.Title, Text = citizenAddress.City.Title, ParentValue = citizenAddress.City.ParentId.ToString() };
                    info.CityId = citizenAddress.CityId;
                    info.Phone = citizenAddress.Phone;
                    info.Street = citizenAddress.Street;
                    info.Alley = citizenAddress.Alley;
                    info.Plaque = citizenAddress.Plaque;


                }




                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
 
            return res;
        }
        public async Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfo(string  userCode)
        {
            var res = new ApiResult<CitizenBaseInfo>(true, ApiResultStatusCode.Success, new CitizenBaseInfo());
            try
            {
                if(string.IsNullOrWhiteSpace(userCode))
                {
                    res.IsSuccess = false;
                    res.Messages = " شناسه کاربری را مشخص نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var guid = Guid.Empty;
                 Guid.TryParse(userCode, out guid);
                var info = await _citizen.Where(w =>   w.User.UserCode == guid).Select(s => new CitizenBaseInfo()
                {
                    BirthDate = s.BirthDate,
                    CitizenId = s.CitizenId,
                    UserCode=s.UserCode,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    EducationField = s.EducationField,
                    EducationGroup = s.EducationGroup.Title,
                    EducationGroupId = s.EducationGroupId,
                    EducationLevel = s.EducationLevel,
                    EducationStatues = s.EducationStatues,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    JobGroup = s.JobGroup.Title,
                    JobGroupId = s.JobGroupId,
                    JobTitle = s.JobTitle,
                    LastName = s.LastName,
                    MariageStatus = s.MariageStatus,
                    MobileNumber = s.Mobile,
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPicture_DisapprovalReason = s.PersonalPicture_DisapprovalReason,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    User = s.User.Username,
                    LastUpdateOnDate = s.LastUpdateOnDate,

                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var citizenAddress = await _address
                    .Include(w => w.City).ThenInclude(t => t.Parent).Where(w => w.CitizenId ==    info.CitizenId && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                if (citizenAddress != null)
                {
                    info.FullAddress = citizenAddress.FullAddress;
                    info.PostalCode = citizenAddress.PostalCode;
                    info.Region = citizenAddress.Region;
                    info.City = new ViewModel.BaseDataModel { Key = citizenAddress.CityId.ToString(), ParentText = citizenAddress.City.Parent.Title, Text = citizenAddress.City.Title, ParentValue = citizenAddress.City.ParentId.ToString() };
                    info.CityId = citizenAddress.CityId;
                    info.Phone = citizenAddress.Phone;
                    info.Street = citizenAddress.Street;
                    info.Alley = citizenAddress.Alley;
                    info.Plaque = citizenAddress.Plaque;


                }




                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;
        }
       
        public async Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformation(int citizenId)
        {
            var res = new ApiResult<CitizenIdentityInformation>(true, ApiResultStatusCode.Success, new CitizenIdentityInformation());
            try
            {
                var info = await _citizen.AsNoTracking().Where(w =>
                w.CitizenId == citizenId).Select(s => new CitizenIdentityInformation()
                {
                    BirthDate = s.BirthDate,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    IdentityId = s.IdentityId,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    NationCode = s.NationCode,
                    SabtStatus = s.SabtStatus,
                    LastUpdateOnDate = s.LastUpdateOnDate,


                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }


            return res;
        }

        public async Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformation(string  userCode)
        {
            var res = new ApiResult<CitizenIdentityInformation>(true, ApiResultStatusCode.Success, new CitizenIdentityInformation());
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);



                var info = await _citizen.AsNoTracking().Where(w =>
                w.UserCode == guid).Select(s => new CitizenIdentityInformation()
                {
                    BirthDate = s.BirthDate,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    IdentityId = s.IdentityId,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    NationCode = s.NationCode,
                    SabtStatus = s.SabtStatus,
                    LastUpdateOnDate = s.LastUpdateOnDate,


                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }


            return res;
        }





        public async Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfo(int citizenId)
        {
            var res = new ApiResult<CitizenFullInfo>(true, ApiResultStatusCode.Success, new CitizenFullInfo());
            try
            {


                var ticks = DateTime.Now.Ticks;
                var info = await _citizen.Where(w =>
                w.CitizenId == citizenId).Select(s => new CitizenFullInfo()
                {
                    Gender = s.Gender,
                    CitizenId = s.CitizenId,
                    NationId = s.NationId,
                    Nation = s.Nation == null ? "" : s.Nation.Name,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    BirthDate = s.BirthDate,
                    FatherName = s.FatherName,
                    AddByUser=s.User.RegisterByUser==null ? "":s.User.RegisterByUser.Username,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    EducationField = s.EducationField,
                    EducationGroup = s.EducationGroup.Title,
                    EducationGroupId = s.EducationGroupId,
                    EducationLevel = s.EducationLevel,
                    EducationStatues = s.EducationStatues,




                    JobGroup = s.JobGroup.Title,
                    JobGroupId = s.JobGroupId,
                    JobTitle = s.JobTitle,

                    MariageStatus = s.MariageStatus,
                    MobileNumber = s.Mobile,
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPicture_DisapprovalReason = s.PersonalPicture_DisapprovalReason,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    User = s.User.Username,
                    UserCode=s.UserCode,
                    LastUpdateOnDate = s.LastUpdateOnDate,
                    RegisterByServiceId = s.RegisterByServiceId,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    LastPictureUploadOnDate = s.LastPictureUploadOnDate


                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var groups = await _citizenGroups.Where(w =>w.IsDeleted!=true && w.Group.IsDeleted!=true &&  w.CitizenId == citizenId).Select(s => new GroupCitizensInfo()
                {
                    AddByUser = s.AddByUser.Username,
                    CitizenId = s.CitizenId,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    UserCode = s.Citizen.UserCode,
                    NationCode = s.Citizen.NationCode,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    Id = s.Id,
                }).ToListAsync();
                info.Groups = groups;

                var address = await _address.Where(w => w.IsDeleted != true &&
                w.CitizenId == citizenId
                && w.AddressType == AddressTypeEnum.منزل
                && w.IsActive
                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();

                if(address!=null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    info.Address = address;
                    info.Address.FullAddress = fullAddresss;
                }
                else
                {
                    info.Address = new AddressInfo();
                    info.Address.FullAddress = "آدرسی برای شهروند ثبت نشده است";

                }
               
                var his = new CitizenHistory()
                {
                    RegsiterOnDate = info.CreationDate,
                    LastPictureUploadOnDate = info.LastPictureUploadOnDate

                };


                var card = await _citizensCard.Where(w => w.CitizenId == citizenId
                && w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه
                 && w.RequestStatuse != CardRequestStatusEnum.باطل_شده

                ).ToListAsync();
                if (card.Any())
                {
                    var lastCard = card.LastOrDefault();
                    his.CardRequstState = lastCard.RequestStatuse.ToString();
                    his.BuyCard = lastCard.RequestDate;
                    if (lastCard.RequestStatuse == CardRequestStatusEnum.درخواست_جدید)
                    {
                        his.CardDescription = "درخواست جدید";
                    }


                    var cardExport = await _cardExportDetails.Include(i => i.ExportCard).OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.CitizenCardInfoId == lastCard.Id);
                    if (cardExport != null)
                    {
                        var exportFile = cardExport.ExportCard;

                        //برو از دوره های توزیع ببین این کارت کی ارسال شده به چاپ
                        his.CardSendToPrint = exportFile.DateSend;
                        if (cardExport.ExportCard.ExportNumber != null)
                            his.CardsendToPrintDescription = "  ارسال برای چاپ در دوره " + cardExport.ExportCard.ExportNumber.Value;

                        his.CardPrinted = exportFile.DateReceive;



                    }





                }




                info.CitizenHistory = his;

                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }

        public async Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfoByUserCode(string userCode)
        {
            var res = new ApiResult<CitizenFullInfo>(true, ApiResultStatusCode.Success, new CitizenFullInfo());
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);

                var ticks = DateTime.Now.Ticks;
                var info = await _citizen.Where(w =>w.User.UserCode == guid).Select(s => new CitizenFullInfo()
                {
                    Gender = s.Gender,
                    CitizenId = s.CitizenId,
                    NationId = s.NationId,
                    Nation = s.Nation == null ? "" : s.Nation.Name,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    BirthDate = s.BirthDate,
                    FatherName = s.FatherName,
                    AddByUser = s.User.RegisterByUser == null ? "" : s.User.RegisterByUser.Username,
                    CreationDate = s.CreationDate,
                    Date_SabtConfirm = s.Date_SabtConfirm,
                    EducationField = s.EducationField,
                    EducationGroup = s.EducationGroup.Title,
                    EducationGroupId = s.EducationGroupId,
                    EducationLevel = s.EducationLevel,
                    EducationStatues = s.EducationStatues, 
                    JobGroup = s.JobGroup.Title,
                    JobGroupId = s.JobGroupId,
                    JobTitle = s.JobTitle, 
                    MariageStatus = s.MariageStatus,
                    MobileNumber = s.Mobile,
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPicture_DisapprovalReason = s.PersonalPicture_DisapprovalReason,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    User = s.User.Username,
                    UserCode=s.UserCode,
                    LastUpdateOnDate = s.LastUpdateOnDate,
                    RegisterByServiceId = s.RegisterByServiceId,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    LastPictureUploadOnDate = s.LastPictureUploadOnDate


                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var groups = await _citizenGroups.Where(w => w.IsDeleted != true && w.Group.IsDeleted != true && w.CitizenId == info.CitizenId).Select(s => new GroupCitizensInfo()
                {
                    AddByUser = s.AddByUser.Username,
                    CitizenId = s.CitizenId,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    UserCode = s.Citizen.UserCode,
                    NationCode = s.Citizen.NationCode,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    Id = s.Id,
                }).ToListAsync();
                info.Groups = groups;

                var address = await _address.Where(w => w.IsDeleted != true &&
                w.CitizenId == info.CitizenId
                && w.AddressType == AddressTypeEnum.منزل
                && w.IsActive
                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();

                if (address != null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    info.Address = address;
                    info.Address.FullAddress = fullAddresss;
                }
                else
                {
                    info.Address = new AddressInfo();
                    info.Address.FullAddress = "آدرسی برای شهروند ثبت نشده است";

                }

                var his = new CitizenHistory()
                {
                    RegsiterOnDate = info.CreationDate,
                    LastPictureUploadOnDate = info.LastPictureUploadOnDate

                };


                var card = await _citizensCard.Where(w => w.CitizenId == info.CitizenId
                && w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه
                 && w.RequestStatuse != CardRequestStatusEnum.باطل_شده

                ).ToListAsync();
                if (card.Any())
                {
                    var lastCard = card.LastOrDefault();
                    his.CardRequstState = lastCard.RequestStatuse.ToString();
                    his.BuyCard = lastCard.RequestDate;
                    if (lastCard.RequestStatuse == CardRequestStatusEnum.درخواست_جدید)
                    {
                        his.CardDescription = "درخواست جدید";
                    }


                    var cardExport = await _cardExportDetails.Include(i => i.ExportCard).OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.CitizenCardInfoId == lastCard.Id);
                    if (cardExport != null)
                    {
                        var exportFile = cardExport.ExportCard;

                        //برو از دوره های توزیع ببین این کارت کی ارسال شده به چاپ
                        his.CardSendToPrint = exportFile.DateSend;
                        if (cardExport.ExportCard.ExportNumber != null)
                            his.CardsendToPrintDescription = "  ارسال برای چاپ در دوره " + cardExport.ExportCard.ExportNumber.Value;

                        his.CardPrinted = exportFile.DateReceive;



                    }





                }




                info.CitizenHistory = his;

                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }

        public async Task<ApiResult<CitizenBaseInfo>> GetMyFamilyBaseInfo(int mycitizenId, int myFamilyid)
        {
            var res = new ApiResult<CitizenBaseInfo>(true, ApiResultStatusCode.Success, new CitizenBaseInfo());
            try
            {

                if (await _family.AnyAsync(w => w.CitizenId == mycitizenId && w.FamilyCitizenId == myFamilyid))
                {
                    var info = await _citizen.Where(w =>
                                   w.CitizenId == myFamilyid).Select(s => new CitizenBaseInfo()
                                   {
                                       BirthDate = s.BirthDate,
                                       CitizenId = s.CitizenId,
                                       CreationDate = s.CreationDate,
                                       Date_SabtConfirm = s.Date_SabtConfirm,
                                       EducationField = s.EducationField,
                                       EducationGroup = s.EducationGroup.Title,
                                       EducationGroupId = s.EducationGroupId,
                                       EducationLevel = s.EducationLevel,
                                       EducationStatues = s.EducationStatues,
                                       FatherName = s.FatherName,
                                       FirstName = s.FirstName,
                                       Gender = s.Gender,
                                       JobGroup = s.JobGroup.Title,
                                       JobGroupId = s.JobGroupId,
                                       JobTitle = s.JobTitle,
                                       LastName = s.LastName,
                                       MariageStatus = s.MariageStatus,
                                       MobileNumber = s.Mobile,
                                       //Nationality=s.NationalityId,
                                       NationCode = s.NationCode,
                                       PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                                       PersonalPicture_DisapprovalReason = s.PersonalPicture_DisapprovalReason,
                                       RegisterByService = s.RegisterByService.ServiceName,
                                       SabtStatus = s.SabtStatus,
                                       User = s.User.Username,


                                   }).FirstOrDefaultAsync();
                    if (info == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "اطلاعاتی یافت نشد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        return res;
                    }

                    var citizenAddress = await _address
                        .Include(w => w.City).ThenInclude(t => t.Parent).Where(w => w.CitizenId ==
                            myFamilyid && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                    if (citizenAddress != null)
                    {
                        info.FullAddress = citizenAddress.FullAddress;
                        info.PostalCode = citizenAddress.PostalCode;
                        info.Region = citizenAddress.Region;
                        info.City = new ViewModel.BaseDataModel { Key = citizenAddress.CityId.ToString(), ParentText = citizenAddress.City.Parent.Title, Text = citizenAddress.City.Title, ParentValue = citizenAddress.City.ParentId.ToString() };
                        info.CityId = citizenAddress.CityId;
                        info.Phone = citizenAddress.Phone;
                        info.Street = citizenAddress.Street;
                        info.Alley = citizenAddress.Alley;
                        info.Plaque = citizenAddress.Plaque;


                    }

                    res.Data = info;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }



        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByTicket(string ticket)
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            {
                if (string.IsNullOrWhiteSpace(ticket))
                {
                    res.IsSuccess = false;
                    res.Messages = "تیکت دسترسی را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }



                var guild = Guid.NewGuid();

                if (!Guid.TryParse(ticket, out guild))
                {
                    res.IsSuccess = false;
                    res.Messages = "تیکت دسترسی  وارد شده صحیح نمی باشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }




                var ticketInfo = await _userLoginTickets.FirstOrDefaultAsync(w => w.UserTicket == guild);
                if (ticketInfo == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تیکت ارسال شده یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                var ticks = DateTime.Now.Ticks;
                var citizenId = ticketInfo.UserId;
                var info = await _citizen.Where(w =>
                w.CitizenId == citizenId).Select(s => new ShortCitizenInfo()
                {
                    UserCode=s.User.UserCode,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    MobileNumber = s.Mobile, 
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    LastUpdateOnDate = s.LastUpdateOnDate,
                    ReturnUrl = ticketInfo.ReturnUrl
                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی شهروند یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                await _event.AddAsync(new Event()
                {
                    ActionName = "GetShortCitizenInfoByTicket",
                    CreateDate = DateTime.Now,
                    Description = "استعلام تیکت دسترسی" + "(" + ticketInfo.AppServicesId + ")",
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.استعلام_شهروند_تیکت,
                    StrCode = ticket,
                    UserId = citizenId,
                    EventType = EventTypeEnum.Info,
                });
                await _userLoginTicketsArchive.AddAsync(new UserLoginTickets_Archive()
                {
                    AppServicesId = ticketInfo.AppServicesId,
                    CreationDate = ticketInfo.CreationDate,
                    UserId = ticketInfo.UserId,
                    ReturnUrl = ticketInfo.ReturnUrl,
                    ReturnDate = DateTime.Now,
                    TicketId = ticketInfo.Id,
                    UserTicket = ticketInfo.UserTicket,
                    //CreatedByUserId = ticketInfo.CreatedByUserId,
                    //ParamName1 = ticketInfo.ParamName1,
                    //ParamName2 = ticketInfo.ParamName2,
                    //ParamValue1 = ticketInfo.ParamValue1,
                    //ParamValue2 = ticketInfo.ParamValue2,
                    SourceId = ticketInfo.SourceId,


                });
                //انتقال تیکت به جدول ارشیو 
                _userLoginTickets.Remove(ticketInfo);

                await _uow.SaveChangesAsync();
                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;
        }
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByNationCode(string NationCode)
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            {
                var ticks = DateTime.Now.Ticks;
                var info = await _citizen.Where(w =>
                w.NationCode == NationCode).Select(s => new ShortCitizenInfo()
                {
                    CitizenId = s.CitizenId,
                    UserCode=s.User.UserCode,
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    MobileNumber = s.Mobile,
                   
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    LastUpdateOnDate = s.LastUpdateOnDate,

                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی شهروند یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }




                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;
        }



        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfo(int citizenId)
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            {
                var ticks = DateTime.Now.Ticks;
                var info = await _citizen.Where(w =>
                w.CitizenId == citizenId).Select(s => new ShortCitizenInfo()
                {
                    CitizenId = s.CitizenId,
                    UserCode=s.User.UserCode,
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    MobileNumber = s.Mobile,
                    
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    LastUpdateOnDate = s.LastUpdateOnDate,



                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }

        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfo(string  userCode)
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode,out guid);


                var ticks = DateTime.Now.Ticks;
                var info = await _citizen.Where(w =>w.User.UserCode == guid).Select(s => new ShortCitizenInfo()
                {
                    CitizenId = s.CitizenId,
                    UserCode = s.User.UserCode,
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName,
                    MobileNumber = s.Mobile, 
                    NationCode = s.NationCode,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    LastUpdateOnDate = s.LastUpdateOnDate,



                }).FirstOrDefaultAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }






            return res;
        }

        public async Task<ApiResult<List<ShortCitizenInfo>>> GetShortCitizenInfoByIds(List<int> ids)
        {
            var res = new ApiResult<List<ShortCitizenInfo>>(true, ApiResultStatusCode.Success, new List<ShortCitizenInfo>());
            try
            {

                var info = await _citizen.Where(w =>
              ids.Contains(w.CitizenId)).Select(s => new ShortCitizenInfo()
              {
                  CitizenId = s.CitizenId,
                  UserCode=s.User.UserCode,
                  CreationDate = s.CreationDate,
                  FatherName = s.FatherName,
                  FirstName = s.FirstName,
                  Gender = s.Gender,
                  LastName = s.LastName,
                  MobileNumber = s.Mobile, 
                  NationCode = s.NationCode,
                  PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                  BirthDate = s.BirthDate,
                  PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                  RegisterByService = s.RegisterByService.ServiceName,
                  SabtStatus = s.SabtStatus,
                  Nationality = 0,
                  LastUpdateOnDate = s.LastUpdateOnDate,



              }).ToListAsync();
                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                res.Data = info;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }


            return res;
        }


        #endregion
        #region ویرایش اطلاعات شهروند

        public async Task<ApiResult<string>> UpdateIdentityInformation (CitizenIdentityInformation model, int citizenId,int byUserId)
        {

            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات خود را به صورت کامل وارد نمایید";
                    return res;
                }


                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                if (citizen.SabtStatus == SabtStatusEnum.تایید)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات شناسنامه ایی شما تایید شده است و امکان ویرایش وجود ندارد";
                    return res;
                }
                if (citizen.SabtStatus == SabtStatusEnum.فوتی)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان ویرایش اطلاعات وجود ندارد";
                    return res;
                }
                if (model.BirthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را مشخص نمایید";
                    return res;
                }



                citizen.Gender = model.Gender;
                citizen.FirstName = model.FirstName.FixFullString();
                citizen.LastName = model.LastName.FixFullString();
                citizen.FatherName = model.FatherName.FixFullString();
                citizen.BirthDate = model.BirthDate;
                citizen.IdentityId = model.IdentityId;
                citizen.SabtStatus = SabtStatusEnum.استعلام_نشده;
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateIdentityInformationByCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شناسنامه ایی شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شناسنامه_ایی,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    OperationId=byUserId, 
                    JsonValue = output

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateIdentityInformationByCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شناسنامه ایی شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = citizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizenId,
                    OperationId = byUserId,
                    Code = citizenId,
                    JsonValue = output

                });
                await _uow.SaveChangesAsync();


            }

            return res;
        }

        public async Task<ApiResult<string>> UpdateIdentityInformation(CitizenIdentityInformation model, string  userCode, int byUserId)
        {
            var citizenId = 0;
            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات خود را به صورت کامل وارد نمایید";
                    return res;
                }

                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);


                var citizen = await _citizen.FirstOrDefaultAsync(w => w.UserCode == guid);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                citizenId = citizen.CitizenId;
                if (citizen.SabtStatus == SabtStatusEnum.تایید)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات شناسنامه ایی شما تایید شده است و امکان ویرایش وجود ندارد";
                    return res;
                }
                if (citizen.SabtStatus == SabtStatusEnum.فوتی)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان ویرایش اطلاعات وجود ندارد";
                    return res;
                }
                if (model.BirthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را مشخص نمایید";
                    return res;
                }



                citizen.Gender = model.Gender;
                citizen.FirstName = model.FirstName.FixFullString();
                citizen.LastName = model.LastName.FixFullString();
                citizen.FatherName = model.FatherName.FixFullString();
                citizen.BirthDate = model.BirthDate;
                citizen.IdentityId = model.IdentityId;
                citizen.SabtStatus = SabtStatusEnum.استعلام_نشده;
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateIdentityInformationByCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شناسنامه ایی شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شناسنامه_ایی,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Info,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    JsonValue = output

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                if(citizenId!=0)
                {
                    await _event.AddAsync(new Event()
                    {
                        ActionName = "UpdateIdentityInformationByCitizen",
                        CreateDate = DateTime.Now,
                        Description = "ویرایش اطلاعات شناسنامه ایی شهروند",
                        EventPriority = EventPriorityEnum.Necessary,
                        EventSection = EventSectionEnum.خطای_سیستمی,
                        StrCode = citizenId.ToString(),
                        EventType = EventTypeEnum.Error,
                        UserId = citizenId,
                        Code = citizenId,
                        JsonValue = output,
                        OperationId = byUserId,

                    });
                    await _uow.SaveChangesAsync();
                }
              


            }

            return res;
        }


        public async Task<ApiResult<string>> UpdateSabtStatus(UpdateSabtStatus model,  int byUserId)
        {
            var citizenId = 0;
            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }

                var guid = Guid.Empty;
                Guid.TryParse(model.UserCode, out guid);


                var citizen = await _citizen.FirstOrDefaultAsync(w => w.UserCode == guid);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                citizenId = citizen.CitizenId; 

                citizen.SabtStatus = model.SabtStatus;
                
                
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateSabtStatus",
                    CreateDate = DateTime.Now,
                    Description = " ویرایش وضعیت ثبت احوال",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_وضعیت_ثبت_احوال,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Info,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    JsonValue = output

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                if (citizenId != 0)
                {
                    await _event.AddAsync(new Event()
                    {
                        ActionName = "UpdateIdentityInformationByCitizen",
                        CreateDate = DateTime.Now,
                        Description = "ویرایش وضعیت ثبت احوال",
                        EventPriority = EventPriorityEnum.Necessary,
                        EventSection = EventSectionEnum.خطای_سیستمی,
                        StrCode = citizenId.ToString(),
                        EventType = EventTypeEnum.Error,
                        UserId = citizenId,
                        Code = citizenId,
                        JsonValue = output,
                        OperationId = byUserId,

                    });
                    await _uow.SaveChangesAsync();
                }



            }

            return res;
        }




        public async Task<ApiResult<string>> UpdateCitizenByCitizen(EditCitizenViewModel model, int citizenId)
        {

            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات خود را به صورت کامل وارد نمایید";
                    return res;
                }


                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                if (model.CityId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهر محل سکونت را مشخص نمایید";
                    return res;
                }

                if (model.BirthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را مشخص نمایید";
                    return res;
                }
                if (string.IsNullOrWhiteSpace(model.Street))
                {
                    res.IsSuccess = false;
                    res.Messages = "خیابان را وارد نمایید";
                    return res;
                }


                //•	در صورتی که فرد رابطه خانوادگی فرزند یا همسر را اضافه کرده باشد، 
                //امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.

                if (model.JobGroup == 0) model.JobGroup = null;
                if (model.EducationGroup == 0) model.EducationGroup = null;
                if (citizen.SabtStatus == SabtStatusEnum.عدم_تایید
                    ||
                    citizen.SabtStatus == SabtStatusEnum.استعلام_نشده

                    )
                {
                    //فقط در این صورت امکان ویرایش این اطلاعات وجود دارد
                    citizen.BirthDate = model.BirthDate;
                    citizen.Gender = model.Gender;

                    citizen.FirstName = model.FirstName.FixFullString();
                    citizen.LastName = model.LastName.FixFullString();
                    citizen.FatherName = model.FatherName.FixFullString();
                }


                //  	در صورتی که فرد رابطه خانوادگی
                // فرزند یا همسر را اضافه کرده باشد، امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.
                if (await _family.AnyAsync(w => w.CitizenId == citizen.CitizenId &&
                 (w.FamilyRelation == FamilyRelationshipsEnum.همسر
                 ||
                 w.FamilyRelation == FamilyRelationshipsEnum.فرزند
                 )
                ))
                {
                    if (model.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شما دارای عضو خانواده هستید و امکان تغییر وضعیت تاهل به مجرد وجود ندارد";
                        return res;
                    }

                }


                citizen.MariageStatus = model.MariageStatus;
                citizen.EducationStatues = model.EducationStatues;
                citizen.EducationGroupId = model.EducationGroup;
                citizen.JobTitle = model.JobTitle;
                citizen.JobGroupId = model.JobGroup;
                citizen.EducationField = model.EducationTitle;
                citizen.EducationLevel = model.EducationLevel;
                citizen.LastUpdateOnDate = DateTime.Now;


                var citizenAddress = await _address
                   .Where(w => w.CitizenId == citizenId && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                if (citizenAddress != null)
                {
                    citizenAddress.FullAddress = " خیابان " + model.Street + "کوچه" + model.Alley + " پلاک " + model.Plaque + " کدپستی  " + model.PostalCode;
                    citizenAddress.PostalCode = model.PostalCode;
                    citizenAddress.Region = model.Region;
                    citizenAddress.CityId = model.CityId;
                    citizenAddress.Phone = model.PhoneNumber;
                    citizenAddress.Plaque = model.Plaque;
                    citizenAddress.PostalCode = model.PostalCode;
                    citizenAddress.Alley = model.Alley;
                    citizenAddress.Street = model.Street;
                    citizenAddress.LasteUpdateOnDate = DateTime.Now;


                    _address.Update(citizenAddress);
                }
                else
                {
                    var fullAddress = "";
                    if (!string.IsNullOrWhiteSpace(model.Street))
                        fullAddress += " " + " خیابان " + model.Street;
                    if (!string.IsNullOrWhiteSpace(model.Alley))
                        fullAddress += " " + " کوچه " + model.Alley;

                    if (!string.IsNullOrWhiteSpace(model.Plaque))
                        fullAddress += " " + " پلاک " + model.Plaque;

                    if (!string.IsNullOrWhiteSpace(model.PostalCode))
                        fullAddress += " " + " کدپستی " + model.PostalCode;

                    //add address
                    var address = new Address()
                    {
                        AddressType = AddressTypeEnum.منزل,
                        Alley = model.Alley,
                        Plaque = model.Plaque,
                        Street = model.Street,
                        LasteUpdateOnDate = DateTime.Now,
                        CitizenId = citizenId,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = fullAddress,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        Phone = model.PhoneNumber,
                        IsActive = true
                    };

                    await _address.AddAsync(address);


                }

                var user = citizen.User;
                if (user != null)
                {
                    user.MobileNumber = citizen.Mobile;
                    user.DisplayName = citizen.FirstName + " " + citizen.LastName;
                    _users.Update(user);
                }
                _citizen.Update(citizen);





                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شهروند,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    JsonValue = output

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = citizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizenId,
                    Code = citizenId,
                    JsonValue = output

                });

                await _uow.SaveChangesAsync();


            }

            return res;
        }



        public async Task<ApiResult<string>> UpdateOtherBaseInfoByCitizen(EditOtherBaseInfoViewModel model, int citizenId)
        {

            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعات خود را به صورت کامل وارد نمایید";
                    return res;
                }


                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                if (model.CityId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهر محل سکونت را مشخص نمایید";
                    return res;
                }



                if (string.IsNullOrWhiteSpace(model.Street))
                {
                    res.IsSuccess = false;
                    res.Messages = "خیابان را وارد نمایید";
                    return res;
                }


                //•	در صورتی که فرد رابطه خانوادگی فرزند یا همسر را اضافه کرده باشد، 
                //امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.

                if (model.JobGroup == 0) model.JobGroup = null;
                if (model.EducationGroup == 0) model.EducationGroup = null;



                //  	در صورتی که فرد رابطه خانوادگی
                // فرزند یا همسر را اضافه کرده باشد، امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.
                if (await _family.AnyAsync(w => w.CitizenId == citizen.CitizenId &&
                 (w.FamilyRelation == FamilyRelationshipsEnum.همسر
                 ||
                 w.FamilyRelation == FamilyRelationshipsEnum.فرزند
                 )
                ))
                {
                    if (model.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شما دارای عضو خانواده هستید و امکان تغییر وضعیت تاهل به مجرد وجود ندارد";
                        return res;
                    }

                }


                citizen.MariageStatus = model.MariageStatus;
                citizen.EducationStatues = model.EducationStatues;
                citizen.EducationGroupId = model.EducationGroup;
                citizen.JobTitle = model.JobTitle;
                citizen.JobGroupId = model.JobGroup;
                citizen.EducationField = model.EducationTitle;
                citizen.EducationLevel = model.EducationLevel;
                citizen.LastUpdateOnDate = DateTime.Now;


                var citizenAddress = await _address
                   .Where(w => w.CitizenId == citizenId && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                if (citizenAddress != null)
                {
                    citizenAddress.FullAddress = " خیابان " + model.Street + "کوچه" + model.Alley + " پلاک " + model.Plaque + " کدپستی  " + model.PostalCode;
                    citizenAddress.PostalCode = model.PostalCode;
                    citizenAddress.Region = model.Region;
                    citizenAddress.CityId = model.CityId;
                    citizenAddress.Phone = model.PhoneNumber;
                    citizenAddress.Plaque = model.Plaque;
                    citizenAddress.PostalCode = model.PostalCode;
                    citizenAddress.Alley = model.Alley;
                    citizenAddress.Street = model.Street;
                    citizenAddress.LasteUpdateOnDate = DateTime.Now;


                    _address.Update(citizenAddress);
                }
                else
                {
                    var fullAddress = "";
                    if (!string.IsNullOrWhiteSpace(model.Street))
                        fullAddress += " " + " خیابان " + model.Street;
                    if (!string.IsNullOrWhiteSpace(model.Alley))
                        fullAddress += " " + " کوچه " + model.Alley;

                    if (!string.IsNullOrWhiteSpace(model.Plaque))
                        fullAddress += " " + " پلاک " + model.Plaque;

                    if (!string.IsNullOrWhiteSpace(model.PostalCode))
                        fullAddress += " " + " کدپستی " + model.PostalCode;

                    //add address
                    var address = new Address()
                    {
                        AddressType = AddressTypeEnum.منزل,
                        Alley = model.Alley,
                        Plaque = model.Plaque,
                        Street = model.Street,
                        LasteUpdateOnDate = DateTime.Now,
                        CitizenId = citizenId,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = fullAddress,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        Phone = model.PhoneNumber,
                        IsActive = true
                    };

                    await _address.AddAsync(address);


                }

                var user = citizen.User;
                if (user != null)
                {
                    user.MobileNumber = citizen.Mobile;
                    user.DisplayName = citizen.FirstName + " " + citizen.LastName;
                    _users.Update(user);
                }
                _citizen.Update(citizen);





                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شهروند,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    JsonValue = output

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizen",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = citizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizenId,
                    Code = citizenId,
                    JsonValue = output

                });

                await _uow.SaveChangesAsync();


            }

            return res;
        }











        public async Task<ApiResult<string>> UpdateCitizenByAdmin(EditCitizenViewModel model,int byUserId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {


                if (model == null)
                {

                    res.IsSuccess = false;
                    res.Messages = "اطلاعات را به صورت کامل وارد نمایید";
                    return res;

                }


                if (model.CitizenId == 0 &&  model.UserCode==null   )
                    return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, "شناسه شهروند را مشخص کنید ");

                var query =   _citizen.Include(i => i.User).AsQueryable();
                if( model.UserCode!=null)
                { 
                    query = query.Where(w => w.User.UserCode == model.UserCode);
                }
                else
                {
                    query = query.Where(w => w.CitizenId == model.CitizenId);
                }


                var citizen = await query.FirstOrDefaultAsync();
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                if (model.BirthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را مشخص نمایید";
                    return res;
                }

                //•	در صورتی که فرد رابطه خانوادگی فرزند یا همسر را اضافه کرده باشد، 
                //امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.

                if (citizen.SabtStatus == SabtStatusEnum.فوتی)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان ویرایش شهروند با وضعیت فوتی وجود ندارد";
                    return res;
                }



                if (model.JobGroup == 0) model.JobGroup = null;
                if (model.EducationGroup == 0) model.EducationGroup = null;

                var mobile = model.MobileNumber.Fa2En();
              
                string output = JsonConvert.SerializeObject(model);

                if (citizen.SabtStatus!=SabtStatusEnum.تایید)
                {
                    citizen.BirthDate = model.BirthDate;
                    citizen.Gender = model.Gender;
                    citizen.FirstName = model.FirstName.FixFullString();
                    citizen.LastName = model.LastName.FixFullString();
                    citizen.FatherName = model.FatherName.FixFullString();
                }
             


                //TODO وضعیت تاهل برای خانواده در چه صورتی قابل ویرایش است
                citizen.MariageStatus = model.MariageStatus;
                citizen.EducationStatues = model.EducationStatues;
                citizen.EducationGroupId = model.EducationGroup;
                citizen.JobTitle = model.JobTitle;
                citizen.JobGroupId = model.JobGroup;
                citizen.EducationField = model.EducationTitle;
                citizen.EducationLevel = model.EducationLevel;
                citizen.LastUpdateOnDate = DateTime.Now;
                citizen.Mobile = mobile;
                var user = citizen.User;
                if (user != null)
                {
                    user.MobileNumber = mobile;
                    user.DisplayName = citizen.FirstName + " " + citizen.LastName;
                    _users.Update(user);
                }



                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenByAdmin",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات اصلی  شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شهروند,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    OperationId = byUserId,
                    JsonValue = output

                });

                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }


        public async Task<ApiResult<string>> UpdateCitizenByWebApiUser(EditCitizenViewModel model,int byUserId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت", "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {
                if( model.UserCode==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه کاربری شهروند را وارد نمایید";
                    return res;
                }

                

                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.UserCode == model.UserCode);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }


                if (model.BirthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ تولد را مشخص نمایید";
                    return res;
                }
                string output = JsonConvert.SerializeObject(model);

                //•	در صورتی که فرد رابطه خانوادگی فرزند یا همسر را اضافه کرده باشد، 
                //امکان تغییر وضعیت خود از متاهل به مجرد را نداشته باشد.

                if (model.JobGroup == 0) model.JobGroup = null;
                if (model.EducationGroup == 0) model.EducationGroup = null;
                if (citizen.SabtStatus == SabtStatusEnum.عدم_تایید
                    ||
                    citizen.SabtStatus == SabtStatusEnum.استعلام_نشده

                    )
                {
                    //فقط در این صورت امکان ویرایش این اطلاعات وجود دارد
                    citizen.BirthDate = model.BirthDate;
                    citizen.Gender = model.Gender;
                    citizen.FirstName = model.FirstName.FixFullString();
                    citizen.LastName = model.LastName.FixFullString();
                    citizen.FatherName = model.FatherName.FixFullString();
                }

                //TODO وضعیت تاهل برای خانواده در چه صورتی قابل ویرایش است
                citizen.MariageStatus = model.MariageStatus;
                citizen.EducationStatues = model.EducationStatues;
                citizen.EducationGroupId = model.EducationGroup;
                citizen.JobTitle = model.JobTitle;
                citizen.JobGroupId = model.JobGroup;
                citizen.EducationField = model.EducationTitle;
                citizen.EducationLevel = model.EducationLevel;
                citizen.LastUpdateOnDate = DateTime.Now;
                var user = citizen.User;
                if (user != null)
                {
                    user.MobileNumber = citizen.Mobile;
                    user.DisplayName = citizen.FirstName + " " + citizen.LastName;
                    _users.Update(user);
                }
                _citizen.Update(citizen);


                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenByWebApiUser",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش اطلاعات اصلی  شهروند",
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_شهروند,
                    StrCode = citizen.CitizenId.ToString(),
                    EventType = EventTypeEnum.Error,
                    UserId = citizen.CitizenId,
                    Code = citizen.CitizenId,
                    OperationId = byUserId,
                    JsonValue = output

                });



                await _uow.SaveChangesAsync();


                 
            }
            catch (Exception er)
            {
                res.Data = "خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }





        public async Task<ApiResult<string>> ChangeCitizenPicture(int citizenId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "رد تصویر با موفقیت صورت گرفت", "رد تصویر با موفقیت صورت گرفت");
            try
            {
                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                citizen.PersonalPicture_Confirmed = PersonalPictureEnum.درحال_بررسی;
                citizen.PersonalPicture_DisapprovalReason = "";
                citizen.LastPictureUploadOnDate = DateTime.Now;
                _citizen.Update(citizen);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<ShortCitizenInfo>> RejectCitizenPicture(RejectCitizenPicture model)
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo(), "رد تصویر با موفقیت صورت گرفت");
            try
            {


                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                citizen.PersonalPicture_Confirmed = PersonalPictureEnum.عدم_تایید;
                citizen.PersonalPicture_DisapprovalReason = model.Reason;
                _citizen.Update(citizen);
                await _uow.SaveChangesAsync();
                res.Data.FirstName = citizen.FirstName;
                res.Data.LastName = citizen.LastName;
                res.Data.MobileNumber = citizen.Mobile;
                res.Data.Gender = citizen.Gender;
                res.Data.NationCode = citizen.NationCode;
                res.Data.CitizenId = citizen.CitizenId;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<string>> AcceptCitizenPicture(int citizenId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "تایید تصویر با موفقیت صورت گرفت", "تایید تصویر با موفقیت صورت گرفت");
            try
            {
                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                citizen.PersonalPicture_Confirmed = PersonalPictureEnum.تایید_شده;
                _citizen.Update(citizen);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }


        #endregion  
        #region اطلاعات بانکی شهروند
        public async Task<ApiResult<BankAccountCardNumberDto>> GetCitizenBankCardNumber(int citizenId)
        {

            var res = new ApiResult<BankAccountCardNumberDto>(true, ApiResultStatusCode.Success, new BankAccountCardNumberDto());
            try
            {
                var model = await _profile.Where(w => w.CitizenId == citizenId).Select(s => new
             BankAccountCardNumberDto()
                {
                    CardNumber = s.BankCardNumber,
                    ShabaNumber = s.ShabaNumber,
                    BankCardNumber_Confirmed = s.BankCardNumber_Confirmed,
                    VerifyCode = "",
                    CitizenId = s.CitizenId,
                    MobileNumber = s.Citizen.Mobile,
                    OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    OwnerNationCode = s.Citizen.NationCode,


                }).FirstOrDefaultAsync();

                if (model == null)
                {
                    var add = new CitizenProfile()
                    {
                        CitizenId = citizenId,



                    };
                    await _profile.AddAsync(add);
                    await _uow.SaveChangesAsync();
                    var newmodel = await _profile.Where(w => w.CitizenId == citizenId).Select(s => new
                     BankAccountCardNumberDto()
                    {
                        CardNumber = s.BankCardNumber,
                        BankCardNumber_Confirmed = s.BankCardNumber_Confirmed,
                        VerifyCode = "",
                        CitizenId = s.CitizenId,
                        MobileNumber = s.Citizen.Mobile,
                        OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                        OwnerNationCode = s.Citizen.NationCode

                    }).FirstOrDefaultAsync();
                    res.Data = model;
                    return res;
                }


                res.Data = model;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }
            return res;
        }
        public async Task<ApiResult<string>> UpdateCitizenBankCardNumber(UpdateBankAccountCardNumberDto model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "", "موفقیت ثبت شد");
            try
            {
                var citizen = await _profile.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                citizen.BankCardNumber = model.CardNumber;
                citizen.ShabaNumber = model.ShabaNumber;
                citizen.BankCardNumber_Confirmed = true;
                _profile.Update(citizen);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        #endregion 
        #region ثبت اطلاعات آدرس شهروند
        public async Task<ApiResult> AddCitizenAddress(AddressDto model, int citizenId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "آدرس با موفقیت ثبت گردید");
            try
            {
                var fullAddress = "";
                if (!string.IsNullOrWhiteSpace(model.Street))
                    fullAddress += " " + " خیابان " + model.Street;
                if (!string.IsNullOrWhiteSpace(model.Alley))
                    fullAddress += " " + " کوچه " + model.Alley;

                if (!string.IsNullOrWhiteSpace(model.Plaque))
                    fullAddress += " " + " پلاک " + model.Plaque;

                if (!string.IsNullOrWhiteSpace(model.PostalCode))
                    fullAddress += " " + " کدپستی " + model.PostalCode;

                var address = new Address()
                {
                    AddressType = model.AddressType,
                    Alley = model.Alley,
                    CitizenId = citizenId,
                    CityId = model.CityId,
                    CreationDate = DateTime.Now,
                    FullAddress = fullAddress,
                    Plaque = model.Plaque,
                    PostalCode = model.PostalCode,
                    Region = model.Region,
                    Street = model.Street,
                    Phone = model.Phone

                };

                await _address.AddAsync(address);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
      
        
        public async Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddress(AddressDto model, int citizenId)
        {
            var res = new ApiResult<AddressDto>(true, ApiResultStatusCode.Success, new AddressDto(), "آدرس با موفقیت ثبت گردید");
            try
            {

                if (model == null)
                {
                    return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "مدل ورودی نامعتبر می باشد");

                }
                if (string.IsNullOrWhiteSpace(model.Street))
                {
                    return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "نام خیابان را وارد نمایید");

                }
                if (string.IsNullOrWhiteSpace(model.Phone))
                {
                    return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "شماره تلفن ثابت را وارد نمایید");

                }

                if (model.CityId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = " شناسه شهر را وارد نمایید";
                    return res;
                }


                if (model.Id != null && model.Id!=0  )
                {
                    var address = await _address.FirstOrDefaultAsync(w => w.Id == model.Id && w.CitizenId == citizenId);
                    if (address == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = " آدرسی یافت نشد";
                        return res;

                    }


                    var fullAddress = "";
                    if (!string.IsNullOrWhiteSpace(model.Street))
                        fullAddress += " " + " خیابان " + model.Street;
                    if (!string.IsNullOrWhiteSpace(model.Alley))
                        fullAddress += " " + " کوچه " + model.Alley;

                    if (!string.IsNullOrWhiteSpace(model.Plaque))
                        fullAddress += " " + " پلاک " + model.Plaque;

                    if (!string.IsNullOrWhiteSpace(model.PostalCode))
                        fullAddress += " " + " کدپستی " + model.PostalCode;


                    address.AddressType = model.AddressType;
                    address.Alley = model.Alley;
                    address.CityId = model.CityId;
                    address.FullAddress = fullAddress;
                    address.Plaque = model.Plaque;
                    address.Phone = model.Phone;
                    address.PostalCode = model.PostalCode;
                    address.Region = model.Region;
                    address.Street = model.Street;
                    address.LasteUpdateOnDate = DateTime.Now;
                    _address.Update(address);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = address.Id;
                }
                else
                {

                    var address = new Address()
                    {
                        AddressType = model.AddressType,
                        CitizenId = citizenId,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = model.FullAddress,
                        IsActive = true,
                        Alley = model.Alley,
                        Street = model.Street,
                        Plaque = model.Plaque,
                        IsVerified = false,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        LasteUpdateOnDate = DateTime.Now,
                        Phone = model.Phone,
                    };
                    await _address.AddAsync(address);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = address.Id;
                }



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
        public async Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddress(AddressDto model, string userCode  )
        {
            var res = new ApiResult<AddressDto>(true, ApiResultStatusCode.Success, new AddressDto(), "آدرس با موفقیت ثبت گردید");
            try
            {
                if (model.CityId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = " شناسه شهر را وارد نمایید";
                    return res;
                }
                Guid guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);
                var citizen = await _citizen.FirstOrDefaultAsync(w => w.UserCode == guid);
                 
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                var citizenId = citizen.CitizenId;

                if (model.Id != null && model.Id != 0)
                {
                    var address = await _address.FirstOrDefaultAsync(w => w.Id == model.Id && w.CitizenId == citizenId);
                    if (address == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = " آدرسی یافت نشد";
                        return res;

                    }



                    var fullAddress = "";
                    if (!string.IsNullOrWhiteSpace(model.Street))
                        fullAddress += " " + " خیابان " + model.Street;
                    if (!string.IsNullOrWhiteSpace(model.Alley))
                        fullAddress += " " + " کوچه " + model.Alley;

                    if (!string.IsNullOrWhiteSpace(model.Plaque))
                        fullAddress += " " + " پلاک " + model.Plaque;

                    if (!string.IsNullOrWhiteSpace(model.PostalCode))
                        fullAddress += " " + " کدپستی " + model.PostalCode;



                    address.AddressType = model.AddressType;
                    address.Alley = model.Alley;
                    address.CityId = model.CityId;
                    address.FullAddress = fullAddress;
                    address.Plaque = model.Plaque;
                    address.Phone = model.Phone;
                    address.PostalCode = model.PostalCode;
                    address.Region = model.Region;
                    address.Street = model.Street;
                    address.LasteUpdateOnDate = DateTime.Now;
                    _address.Update(address);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = address.Id;
                }
                else
                {
                    var fullAddress = "";
                    if (!string.IsNullOrWhiteSpace(model.Street))
                        fullAddress += " " + " خیابان " + model.Street;
                    if (!string.IsNullOrWhiteSpace(model.Alley))
                        fullAddress += " " + " کوچه " + model.Alley;

                    if (!string.IsNullOrWhiteSpace(model.Plaque))
                        fullAddress += " " + " پلاک " + model.Plaque;

                    if (!string.IsNullOrWhiteSpace(model.PostalCode))
                        fullAddress += " " + " کدپستی " + model.PostalCode;



                    var address = new Address()
                    {
                        AddressType = model.AddressType,
                        CitizenId = citizenId,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = fullAddress,
                        IsActive = true,
                        Alley = model.Alley,
                        Street = model.Street,
                        Plaque = model.Plaque,
                        IsVerified = false,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        LasteUpdateOnDate = DateTime.Now,
                        Phone = model.Phone,
                    };
                    await _address.AddAsync(address);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = address.Id;
                }



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }



        public async Task<ApiResult> RemoveCitizenAddress(int id, int citizenId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "آدرس با موفقیت حذف گردید");
            try
            {
                var address = await _address.FirstOrDefaultAsync(w => w.Id == id && w.CitizenId == citizenId);
                if (address == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " آدرسی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }
                address.IsDeleted = true;
                address.LasteUpdateOnDate = DateTime.Now;
                _address.Update(address);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
        public async Task<ApiResult> RemoveCitizenAddressByAdmin(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "آدرس با موفقیت حذف گردید");
            try
            {
                var address = await _address.FirstOrDefaultAsync(w => w.Id == id);
                if (address == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " آدرسی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }
                address.IsDeleted = true;
                address.LasteUpdateOnDate = DateTime.Now;
                _address.Update(address);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }


        public async Task<ApiResult<List<AddressInfo>>> GetCitizenAddress(int citizenId)
        {
            var res = new ApiResult<List<AddressInfo>>(true, ApiResultStatusCode.Success, new List<AddressInfo>(), "");
            try
            {
                var address = await _address.Where(w => w.IsDeleted != true && w.CitizenId == citizenId).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).ToListAsync();
              

                res.Data = address;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }



        public async Task<ApiResult<AddressInfo>> GetCitizenHomeAddress(int citizenId)
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                var address = await _address.Where(w => w.IsDeleted != true &&
                w.CitizenId == citizenId
                && w.AddressType == AddressTypeEnum.منزل
                && w.IsActive

                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();

                if (address != null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    res.Data = address;
                    res.Data.FullAddress = fullAddresss;
                }



                

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<AddressInfo>> GetCitizenHomeAddress(string userCode  )
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                Guid guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);

                var address = await _address.Where(w => 
                w.IsDeleted != true &&
                w.Citizen.UserCode == guid
                && w.AddressType == AddressTypeEnum.منزل
                && w.IsActive

                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();

                if (address != null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    res.Data = address;
                    res.Data.FullAddress = fullAddresss;
                }





            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<AddressInfo>> GetCitizenOfficeAddress(int citizenId)
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                var address = await _address.Where(w => w.IsDeleted != true &&
                w.CitizenId == citizenId
                && w.AddressType == AddressTypeEnum.محل_کار
                && w.IsActive

                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();
                res.Data = address;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }



        public async Task<ApiResult<AddressInfo>> GetCitizenOfficeAddress(string userCode)
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                Guid guid = Guid.NewGuid();
                Guid.TryParse(userCode, out guid);
                var address = await _address.Where(w => w.IsDeleted != true &&
                w.Citizen.UserCode == guid
                && w.AddressType == AddressTypeEnum.محل_کار
                && w.IsActive

                ).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();
                res.Data = address;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }




        public async Task<ApiResult<AddressInfo>> GeAddress(int id)
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                var address = await _address.Where(w => w.Id == id).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();
                if (address == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " آدرسی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }
                if (address != null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    res.Data = address;
                    res.Data.FullAddress = fullAddresss;
                }
               

                

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
        public async Task<ApiResult<AddressInfo>> GeAddress(int citizenId,int id)
        {
            var res = new ApiResult<AddressInfo>(true, ApiResultStatusCode.Success, new AddressInfo(), "");
            try
            {
                var address = await _address.Where(w =>w.CitizenId== citizenId && w.Id == id).Select(s => new AddressInfo()
                {
                    CityId = s.CityId,
                    CreationDate = s.CreationDate,
                    FullAddress = s.FullAddress,
                    IsDeleted = s.IsDeleted,
                    AddressType = s.AddressType,
                    Alley = s.Alley,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                    Id = s.Id,
                    IsVerified = s.IsVerified,
                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Region = s.Region,
                    Street = s.Street,
                    Phone = s.Phone,

                }).FirstOrDefaultAsync();
                if (address == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " آدرسی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }
                if (address != null)
                {
                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                    if (!string.IsNullOrWhiteSpace(address.Street))
                        fullAddresss += " خیابان  " + address.Street;

                    if (!string.IsNullOrWhiteSpace(address.Alley))
                        fullAddresss += " کوچه  " + address.Alley;

                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                        fullAddresss += "  پلاک " + address.Plaque;


                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                        fullAddresss += "  کدپستی  " + address.PostalCode;

                    res.Data = address;
                    res.Data.FullAddress = fullAddresss;
                }




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        #endregion
        #region  ثبت اطلاعات پروفایل شهروند
        public async Task<ApiResult> AddOrUpdateCitizenProfile(CitizenProfileDto model, int citizenId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "اطلاعات تکمیلی شما با موفقیت ثبت گردید");
            try
            {
                if (model.CityOfBirthId == 0) model.CityOfBirthId = null;
                if (model.ShCityId == 0) model.ShCityId = null;




                var profile = await _profile.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if (profile == null)
                {

                    //
                    var add = new CitizenProfile()
                    {
                        AcademicGrade = model.AcademicGrade,
                        BaseEducation = model.BaseEducation,
                        BirthCitySection = model.BirthCitySection,
                        CitizenId = citizenId,
                        CityOfBirthId = model.CityOfBirthId,
                        DateOfEmployeement = model.DateOfEmployeement,
                        DateOfMarriage = model.DateOfMarriage,
                        EducationStatues = model.EducationStatues,
                        EndOfEducation = model.EndOfEducation,
                        EndOfMilitary = model.EndOfMilitary,
                        PersonnelCode = model.PersonnelCode,
                        ShCityId = model.ShCityId,
                        InsuranceNumber = model.InsuranceNumber,
                        Religion = model.Religion,
                        ShDate = model.ShDate,
                        ShCode = model.ShCode,
                        ShSerial = model.ShSerial,
                        UniversityName = model.UniversityName,
                        ShCitySection = model.ShCitySection,
                        ShNote = model.ShNote,
                        VillageOfBirth = model.VillageOfBirth,
                        SoldierState = model.SoldierState,
                        AcademicNote = model.AcademicNote,
                    };

                    await _profile.AddAsync(add);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    profile.AcademicGrade = model.AcademicGrade;
                    profile.BaseEducation = model.BaseEducation;
                    profile.BirthCitySection = model.BirthCitySection;
                    profile.CityOfBirthId = model.CityOfBirthId;
                    profile.DateOfEmployeement = model.DateOfEmployeement;
                    profile.DateOfMarriage = model.DateOfMarriage;
                    profile.EducationStatues = model.EducationStatues;
                    profile.EndOfEducation = model.EndOfEducation;
                    profile.EndOfMilitary = model.EndOfMilitary;
                    profile.PersonnelCode = model.PersonnelCode;
                    profile.ShCityId = model.ShCityId;
                    profile.InsuranceNumber = model.InsuranceNumber;
                    profile.Religion = model.Religion;
                    profile.ShDate = model.ShDate;
                    profile.ShCode = model.ShCode;
                    profile.ShSerial = model.ShSerial;
                    profile.UniversityName = model.UniversityName;
                    profile.ShCitySection = model.ShCitySection;
                    profile.ShNote = model.ShNote;
                    profile.VillageOfBirth = model.VillageOfBirth;
                    profile.SoldierState = model.SoldierState;
                    profile.AcademicNote = model.AcademicNote;
                    _profile.Update(profile);
                    await _uow.SaveChangesAsync();


                }
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult> AddOrUpdateCitizenProfileByAdmin(CitizenProfileDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "اطلاعات تکمیلی شما با موفقیت ثبت گردید");
            try
            {
                if (model.CityOfBirthId == 0) model.CityOfBirthId = null;
                if (model.ShCityId == 0) model.ShCityId = null;

                var citizen = await _citizen.FirstOrDefaultAsync(w=>w.UserCode==model.UserCode);
                if(citizen==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }


                var profile = await _profile.FirstOrDefaultAsync(w => w.CitizenId == citizen.CitizenId);
                if (profile == null)
                {

                    //
                    var add = new CitizenProfile()
                    {
                        AcademicGrade = model.AcademicGrade,
                        BaseEducation = model.BaseEducation,
                        BirthCitySection = model.BirthCitySection,
                        CitizenId = citizen.CitizenId,
                        CityOfBirthId = model.CityOfBirthId,
                        DateOfEmployeement = model.DateOfEmployeement,
                        DateOfMarriage = model.DateOfMarriage,
                        EducationStatues = model.EducationStatues,
                        EndOfEducation = model.EndOfEducation,
                        EndOfMilitary = model.EndOfMilitary,
                        PersonnelCode = model.PersonnelCode,
                        ShCityId = model.ShCityId,
                        InsuranceNumber = model.InsuranceNumber,
                        Religion = model.Religion,
                        ShDate = model.ShDate,
                        ShCode = model.ShCode,
                        ShSerial = model.ShSerial,
                        UniversityName = model.UniversityName,
                        ShCitySection = model.ShCitySection,
                        ShNote = model.ShNote,
                        VillageOfBirth = model.VillageOfBirth,
                        SoldierState = model.SoldierState,
                        AcademicNote = model.AcademicNote,
                    };

                    await _profile.AddAsync(add);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    profile.AcademicGrade = model.AcademicGrade;
                    profile.BaseEducation = model.BaseEducation;
                    profile.BirthCitySection = model.BirthCitySection;
                    profile.CityOfBirthId = model.CityOfBirthId;
                    profile.DateOfEmployeement = model.DateOfEmployeement;
                    profile.DateOfMarriage = model.DateOfMarriage;
                    profile.EducationStatues = model.EducationStatues;
                    profile.EndOfEducation = model.EndOfEducation;
                    profile.EndOfMilitary = model.EndOfMilitary;
                    profile.PersonnelCode = model.PersonnelCode;
                    profile.ShCityId = model.ShCityId;
                    profile.InsuranceNumber = model.InsuranceNumber;
                    profile.Religion = model.Religion;
                    profile.ShDate = model.ShDate;
                    profile.ShCode = model.ShCode;
                    profile.ShSerial = model.ShSerial;
                    profile.UniversityName = model.UniversityName;
                    profile.ShCitySection = model.ShCitySection;
                    profile.ShNote = model.ShNote;
                    profile.VillageOfBirth = model.VillageOfBirth;
                    profile.SoldierState = model.SoldierState;
                    profile.AcademicNote = model.AcademicNote;
                    _profile.Update(profile);
                    await _uow.SaveChangesAsync();


                }
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }




        public async Task<ApiResult<CitizenProfileInfo>> GetCitizenProfile(int citizenId)
        {
            var res = new ApiResult<CitizenProfileInfo>(true, ApiResultStatusCode.Success, new CitizenProfileInfo(), "");
            try
            {

                var profile = await _profile.Where(w => w.CitizenId == citizenId).Select(s => new CitizenProfileInfo()
                {
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    UserCode=s.Citizen.UserCode,
                    CitizenId = s.CitizenId,
                    EndOfMilitary = s.EndOfMilitary,
                    MilitaryStatus = s.SoldierState,
                    EndOfEducation = s.EndOfEducation,
                    EducationStatues = s.EducationStatues,

                    AcademicGrade = s.AcademicGrade,
                    AcademicNote = s.AcademicNote,
                    BankCardNumber = s.BankCardNumber,
                    BankCardNumber_Confirmed = s.BankCardNumber_Confirmed,
                    BaseEducation = s.BaseEducation,
                    BirthCitySection = s.BirthCitySection,

                    DateOfEmployeement = s.DateOfEmployeement,
                    DateOfMarriage = s.DateOfMarriage,
                    InsuranceNumber = s.InsuranceNumber,
                    PersonnelCode = s.PersonnelCode,
                    Religion = s.Religion,
                    ShCitySection = s.ShCitySection,
                    ShCode = s.ShCode,
                    ShDate = s.ShDate,
                    ShNote = s.ShNote,
                    ShSerial = s.ShSerial,
                   
                    UniversityName = s.UniversityName,
                    VillageOfBirth = s.VillageOfBirth,

                    CityOfBirthId = s.CityOfBirthId,
                    ProvinceCityOfBirth = s.CityOfBirth == null ? null : new ViewModel.BaseDataModel
                    { Key = s.CityOfBirth.ParentId.ToString(), Text = s.CityOfBirth.Parent.Title },
                    CityOfBirth = s.CityOfBirth == null ? "" : s.CityOfBirth.Title,

                    ShCityId = s.ShCityId,
                    ProvinceShCity = s.ShCity == null ? null : new ViewModel.BaseDataModel {
                        Key = s.ShCity.ParentId.ToString(), Text = s.ShCity.Parent.Title },
                    ShCity = s.ShCity == null ? "" : s.ShCity.Title,

                }).FirstOrDefaultAsync();



                if (profile == null)
                {
                    //چک کن شهروندی با این شناسه وجود دارد
                    if (await _citizen.AnyAsync(w => w.CitizenId == citizenId))
                    {
                        await _profile.AddAsync(new CitizenProfile()
                        {
                            CitizenId = citizenId,
                        });
                        await _uow.SaveChangesAsync();

                    }
                    else
                    {
                        res.IsSuccess = false;
                        res.Messages = " شهروندی با این شناسه یافت نشد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        return res;
                    }
                }
                else
                {
                    res.Data = profile;
                }




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
        public async Task<ApiResult<CitizenProfileInfo>> GetCitizenProfile(string  userCode)
        {
            var res = new ApiResult<CitizenProfileInfo>(true, ApiResultStatusCode.Success, new CitizenProfileInfo(), "");
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);

                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.User.UserCode == guid);
                if(citizen==null)
                {
                    res.IsSuccess = false;
                    res.Messages = " شهروندی با این شناسه یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
              
                var profile = await _profile.AsNoTracking().Where(w => w.CitizenId== citizen.CitizenId).Select(s => new CitizenProfileInfo()
                {
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    UserCode=s.Citizen.UserCode,
                    CitizenId = s.CitizenId,
                    EndOfMilitary = s.EndOfMilitary,
                    MilitaryStatus = s.SoldierState,
                    EndOfEducation = s.EndOfEducation,
                    EducationStatues = s.EducationStatues,

                    AcademicGrade = s.AcademicGrade,
                    AcademicNote = s.AcademicNote,
                    BankCardNumber = s.BankCardNumber,
                    BankCardNumber_Confirmed = s.BankCardNumber_Confirmed,
                    BaseEducation = s.BaseEducation,
                    BirthCitySection = s.BirthCitySection,

                    DateOfEmployeement = s.DateOfEmployeement,
                    DateOfMarriage = s.DateOfMarriage,
                    InsuranceNumber = s.InsuranceNumber,
                    PersonnelCode = s.PersonnelCode,
                    Religion = s.Religion,
                    ShCitySection = s.ShCitySection,
                    ShCode = s.ShCode,
                    ShDate = s.ShDate,
                    ShNote = s.ShNote,
                    ShSerial = s.ShSerial,
                   
                    UniversityName = s.UniversityName,
                    VillageOfBirth = s.VillageOfBirth,

                    CityOfBirthId = s.CityOfBirthId,
                    ProvinceCityOfBirth = s.CityOfBirth == null ? null : new ViewModel.BaseDataModel
                    { Key = s.CityOfBirth.ParentId.ToString(), Text = s.CityOfBirth.Parent.Title },
                    CityOfBirth = s.CityOfBirth == null ? "" : s.CityOfBirth.Title,

                    ShCityId = s.ShCityId,
                    ProvinceShCity = s.ShCity == null ? null : new ViewModel.BaseDataModel
                    {
                        Key = s.ShCity.ParentId.ToString(),
                        Text = s.ShCity.Parent.Title
                    },
                    ShCity = s.ShCity == null ? "" : s.ShCity.Title,

                }).FirstOrDefaultAsync();



                if (profile == null)
                {
                    await _profile.AddAsync(new CitizenProfile()
                    {
                        CitizenId = citizen.CitizenId,
                    });
                    await _uow.SaveChangesAsync();


                }
                else
                {
                    res.Data = profile;
                }




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }


        #endregion 
        #region چک کردن ثبتنام شهروند
        public async Task<ApiResult<IsCitizenIsRegisterResult>> CheckRegisterCitizenByNtionCode(IsCitizenIsRegister model)
        {
            var res = new ApiResult<IsCitizenIsRegisterResult>(true, ApiResultStatusCode.Success, new IsCitizenIsRegisterResult());
            try
            {
                if (!string.IsNullOrWhiteSpace(model.NationCode))
                {
                    var data = await _citizen.Where(w => w.NationCode == model.NationCode).Select(s => new IsCitizenIsRegisterResult()
                    {
                        RegisterOnDate = s.CreationDate,
                        MobileNumber = s.Mobile
                    }).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        data.IsRegister = true;
                        var mobileNumber = data.MobileNumber;
                        if (!string.IsNullOrWhiteSpace(mobileNumber))
                        {
                            if (mobileNumber.Length > 8)
                            {
                                mobileNumber = mobileNumber.Substring(0, 7);
                                mobileNumber += "****";
                                data.MobileNumber = mobileNumber;
                            }
                        }
                        res.Data = data;
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی  شهروند را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }

        public async Task<ApiResult<IsCitizenIsRegisterResult>> CheckCitizenPicture(int citizenId)
        {
            var res = new ApiResult<IsCitizenIsRegisterResult>(true, ApiResultStatusCode.Success, new IsCitizenIsRegisterResult());
            try
            {

                var data = await _citizen.Where(w => w.CitizenId == citizenId).Select(s => new IsCitizenIsRegisterResult()
                {
                    RegisterOnDate = s.CreationDate,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed
                }).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsRegister = true;
                    res.Data = data;
                }


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
        #region عضویت شهروند در گروههای شهروندی

        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfoByUserCode( string  userCode)
        {
            var res = new ApiResult<List<GroupsCitizensInfo>>(true, ApiResultStatusCode.Success, new List<GroupsCitizensInfo>(), "");
            try
            {


                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);

                var groups = await _citizenGroups.Where(w => w.Group.IsDeleted != true &&
                w.Group.IsActive
                && w.IsDeleted != true && w.Citizen.User.UserCode == guid).Select(s => new GroupsCitizensInfo()
                {
                    AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    MunicipalPersonnelGroup = s.Group.MunicipalPersonnelGroup,
                    Id = s.Id

                }).ToListAsync();
                res.Data = groups;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfoByCitizenId(int citizenId)
        {
            var res = new ApiResult<List<GroupsCitizensInfo>>(true, ApiResultStatusCode.Success, new List<GroupsCitizensInfo>(), "");
            try
            {


              

                var groups = await _citizenGroups.Where(w => w.Group.IsDeleted != true &&
                w.Group.IsActive
                && w.IsDeleted != true && w.CitizenId  == citizenId).Select(s => new GroupsCitizensInfo()
                {
                    AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    MunicipalPersonnelGroup = s.Group.MunicipalPersonnelGroup,
                    Id = s.Id

                }).ToListAsync();
                res.Data = groups;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }




        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfo(string nationCode)
        {
            var res = new ApiResult<List<GroupsCitizensInfo>>(true, ApiResultStatusCode.Success, new List<GroupsCitizensInfo>(), "");
            try
            {
                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.NationCode == nationCode);
                if(citizen==null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "شهروندی با کد ملی وارد شده یافت نشد";
                    return res;
                }
                var address = await _citizenGroups.Where(w => w.Group.IsDeleted != true &&
                w.Group.IsActive
                && w.IsDeleted != true && w.CitizenId == citizen.CitizenId).Select(s => new GroupsCitizensInfo()
                {
                    AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    MunicipalPersonnelGroup = s.Group.MunicipalPersonnelGroup,
                    Id = s.Id

                }).ToListAsync();
                res.Data = address;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult<CitizenGroupsAndQueues>> GetCitizenGroupsAndQueues(string nationCode)
        {
            var res = new ApiResult<CitizenGroupsAndQueues>(true, ApiResultStatusCode.Success, new CitizenGroupsAndQueues(), "");
            try
            {
                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.NationCode == nationCode);
                if (citizen != null)
                {

                    var groups = await _citizenGroups.Where(w => w.Group.IsDeleted != true &&
               w.Group.IsActive
               && w.IsDeleted != true && w.CitizenId == citizen.CitizenId).Select(s => new GroupsCitizensInfo()
               {
                   AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                   AddByUserId = s.AddByUserId,
                   Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                   CitizenId = s.CitizenId,
                   CreationDate = s.CreationDate,
                   Group = s.Group.GroupName,
                   GroupId = s.GroupId,
                   MunicipalPersonnelGroup = s.Group.MunicipalPersonnelGroup,
                   Id = s.Id

               }).ToListAsync();
                    res.Data.GroupList = groups;
                    res.Data.IsRegistered = true;
                }
                else
                {
                    var queues = await _citizensQueue.Where(w => w.Group.IsDeleted != true &&
              w.Group.IsActive &&
              w.NationCode == nationCode).Select(s => new CitizensQueueInfo()
              {
                  AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                  AddByUserId = s.AddByUserId,
                  CreationDate = s.CreationDate,
                  Group = s.Group.GroupName,
                  GroupId = s.GroupId,
                  NationCode = s.NationCode,
                  Id = s.Id

              }).ToListAsync();
                    res.Data.QueueList = queues;
                    res.Data.IsRegistered = false;
                    res.Data.GroupList = null;
                }




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }


        public async Task<ApiResult<List<ShortGroupsCitizensInfo>>> GetShortGroupsCitizensInfo(int citizenId)
        {
            var res = new ApiResult<List<ShortGroupsCitizensInfo>>(true, ApiResultStatusCode.Success, new List<ShortGroupsCitizensInfo>(), "");
            try
            {
                var groups = await _citizenGroups.Where(w => w.Group.IsDeleted != true &&
                w.Group.IsActive
                &&
                w.IsDeleted != true && w.CitizenId == citizenId).Select(s => new ShortGroupsCitizensInfo()
                {

                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    Id = s.Id,
                    MunicipalPersonnelGroup = s.Group.MunicipalPersonnelGroup


                }).ToListAsync();
                res.Data = groups;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }



        public async Task<ApiResult<PagedCitizenGroupsViewModel>> GetGroupsCitizensInfo(
       string  userCode, int pageNumber, int pageSize
        )
        {

            var res = new ApiResult<PagedCitizenGroupsViewModel>(true, ApiResultStatusCode.Success, new PagedCitizenGroupsViewModel());
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);


                var offset = (pageNumber) * pageSize;
                var query = _citizenGroups.Where(w =>w.Group.IsDeleted!=true && w.IsDeleted != true && w.Citizen.User.UserCode== guid);

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new GroupsCitizensInfo()
                {

                    AddByUser = s.AddByUser == null ? "" : s.AddByUser.DisplayName,
                    AddByUserId = s.AddByUserId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    Group = s.Group.GroupName,
                    GroupId = s.GroupId,
                    Id = s.Id
                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }
        public async Task<ApiResult<string>> RemoveCitizenFromGroup(GroupsCitizensDto model, int userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {

                if(model.GroupId==22)
                {
                    res.IsSuccess = false;
                    res.Messages = "گروه سیستمی می باشد.امکان حذف وجود ندارد";
                    return res;
                }

                var guid = Guid.Empty;
                Guid.TryParse(model.UserCode, out guid);


                var item = await _citizenGroups.FirstOrDefaultAsync(w => w.Citizen.User.UserCode == guid
                && w.GroupId == model.GroupId && w.IsDeleted!=true
                );
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }


                item.DeletedByUserId = userId;
                item.IsDeleted = true;
                item.DeletedDate = DateTime.Now;
                _citizenGroups.Update(item);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;
            }


            return res;
        }
        public async Task<ApiResult<string>> AddCitizenToGroup(GroupsCitizensDto model, int userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {

                var guid = Guid.Empty;
                Guid.TryParse(model.UserCode, out guid);


                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.User.UserCode == guid);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                if (await _citizenGroups.AnyAsync(w => w.CitizenId == citizen.CitizenId
                && w.GroupId == model.GroupId
                && w.IsDeleted != true
                ))
                {

                    res.IsSuccess = false;
                    res.Messages = "این شهروند قبلا به این گروه اضافه شده است";
                    return res;
                }

                if (model.ExpireDate != null)
                {
                    var date = DateTime.Now;
                    if (model.ExpireDate < date)
                    {
                        res.IsSuccess = false;
                        res.Messages = " تاریخ انقضا باید بزرگتر از تاریخ جاری باشد";
                        return res;

                    }
                }

                var group = await _groups.FirstOrDefaultAsync(w => w.Id == model.GroupId);
                if (group == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " گروهی یافت نشد";
                    return res;

                }

                if (group.AutoAddMembers)
                {
                    res.IsSuccess = false;
                    res.Messages = " امکان اضافه کردن شهروندی به گروههای اتوماتیک وجود ندارد";
                    return res;
                }

                var add = new GroupsCitizens()
                {
                    CitizenId = citizen.CitizenId,
                    CreationDate = DateTime.Now,
                    AddByUserId = userId,
                    GroupId = model.GroupId,
                    ExpireDate = model.ExpireDate
                };

                await _citizenGroups.AddAsync(add);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;
            }


            return res;
        }

        #endregion
        #region جستجوی شهروند

        public async Task<ApiResult<PagedCitizenViewModel>> SearchCitizens(
          int pageNumber, int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string name = null,
          string nationCode = null,
           int? groupId = null
          )
        {




            var res = new ApiResult<PagedCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedCitizenViewModel());
            try
            {
                var offset = (pageNumber) * pageSize;
                var query = _citizen.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(nationCode))
                {
                    nationCode = nationCode.Trim().Fa2En();
                    query = query.Where(w => EF.Functions.Like(w.NationCode, "%" + nationCode + "%"));
                }


                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.FirstName, "%" + name + "%")
                    ||
                    EF.Functions.Like(w.LastName, "%" + name + "%")

                    );
                }


                if (groupId != null)
                {
                    var citizenIds = await _citizenGroups.Where(w => w.GroupId == groupId).Select(s => s.CitizenId).ToListAsync();
                    query = query.Where(w => citizenIds.Contains(w.CitizenId));
                }






                if (FromDate != null)
                {

                    var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.CreationDate >= datetime);

                }
                if (ToDate != null)
                {
                    var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.CreationDate <= datetime);


                }


                var ticks = DateTime.Now.Ticks;
                res.Data.TotalItems = await query.CountAsync();
                var queryString = query.ToString();




                res.Data.Citizens = await query.Select(s => new ShortCitizenInfo()
                {

                    CitizenId = s.CitizenId,
                    UserCode=s.User.UserCode, 
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName, 
                    NationCode = s.NationCode,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed


                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }


        public async Task<ApiResult<ShortCitizenInfo>> SearchCitizenByCardUser(
        string nationCode,
        DateTime? birthDate = null 

        )
        {

            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            {


                if (string.IsNullOrEmpty(nationCode))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی شهروند را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                //if (birthDate == null)
                //{
                //    res.IsSuccess = false;
                //    res.Messages = "تاریخ تولد شهروند را وارد نمایید";
                //    res.StatusCode = ApiResultStatusCode.BadRequest;
                //    return res;
                //}
                nationCode = nationCode.Trim().Fa2En();

                var query = _citizen.Where(w => w.NationCode == nationCode); 
                if(birthDate!=null)
                {
                    query = query.Where(w => w.BirthDate == birthDate);
                }


                var ticks = DateTime.Now.Ticks;

                var data = await query.Select(s => new ShortCitizenInfo()
                {

                    CitizenId = s.CitizenId,
                    UserCode=s.User.UserCode,
                    CreationDate = s.CreationDate,
                    FatherName = s.FatherName,
                    FirstName = s.FirstName,
                    Gender = s.Gender,
                    LastName = s.LastName, 
                    NationCode = s.NationCode,
                    PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                    BirthDate = s.BirthDate,
                    PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                    RegisterByService = s.RegisterByService.ServiceName,
                    SabtStatus = s.SabtStatus,
                    Nationality = 0,
                    PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                    MobileNumber=s.Mobile,
                    


                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                res.Data = data;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

        public async Task<ApiResult<PagedImageCitizenViewModel>> SearchImageCitizens(
          int pageNumber, int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          PersonalPictureEnum? pictureConfirmed = null,
         string nationCode = null,
         bool? HasCard = null,
         int? groupId = null
          )
        {



            var res = new ApiResult<PagedImageCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedImageCitizenViewModel());
            try
            {
                var offset = (pageNumber) * pageSize;

                if (HasCard == true)
                {
                    var query = _citizensCard.Where(w => w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه);
                    if (!string.IsNullOrEmpty(nationCode))
                    {
                        nationCode = nationCode.Trim().Fa2En();
                        query = query.Where(w => EF.Functions.Like(w.Citizen.NationCode, "%" + nationCode + "%"));
                    }
                    if (pictureConfirmed != null)
                    {
                        query = query.Where(w => w.Citizen.PersonalPicture_Confirmed == pictureConfirmed);
                    }

                    if (groupId != null)
                    {
                        query = query.Where(w => w.Citizen.GroupsCitizens.Any(a => a.IsDeleted != true && a.GroupId == groupId));
                    }


                    if (FromDate != null)
                    {

                        var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.Citizen.CreationDate >= datetime);

                    }
                    if (ToDate != null)
                    {
                        var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.Citizen.CreationDate <= datetime);


                    }



                    var year = DateTime.Today.Year;
                    var ticks = DateTime.Now.Ticks;
                    res.Data.TotalItems = await query.CountAsync();
                    res.Data.Citizens = await query.Select(s => new ImageCitizenInfo()
                    {

                        BirthDate = s.Citizen.BirthDate,
                        CitizenId = s.CitizenId,
                        UserCode=s.Citizen.UserCode,
                        CreationDate = s.Citizen.CreationDate,
                        FirstName = s.Citizen.FirstName,
                        Gender = s.Citizen.Gender,
                        LastName = s.Citizen.LastName,
                        PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg?v=" + ticks.ToString(),
                        NationCode = s.Citizen.NationCode,
                        PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                        SabtStatus = s.Citizen.SabtStatus,
                        Nationality = 0,
                        Age = year - s.Citizen.BirthDate.Value.Year,
                        PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed == null ? true : false,
                    }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();



                }
                else
                {
                    var query = _citizen.AsQueryable();
                    if (!string.IsNullOrEmpty(nationCode))
                    {
                        nationCode = nationCode.Trim().Fa2En();
                        query = query.Where(w => EF.Functions.Like(w.NationCode, "%" + nationCode + "%"));
                    }
                    if (pictureConfirmed != null)
                    {
                        query = query.Where(w => w.PersonalPicture_Confirmed == pictureConfirmed);
                    }


                    if (FromDate != null)
                    {

                        var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.CreationDate >= datetime);

                    }
                    if (ToDate != null)
                    {
                        var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.CreationDate <= datetime);


                    }





                    if (groupId != null)
                    {
                        query = query.Where(w => w.GroupsCitizens.Any(a => a.IsDeleted != true && a.GroupId == groupId));
                    }
                    var year = DateTime.Today.Year;
                    var ticks = DateTime.Now.Ticks;
                    res.Data.TotalItems = await query.CountAsync();
                    res.Data.Citizens = await query.Select(s => new ImageCitizenInfo()
                    {

                        BirthDate = s.BirthDate,
                        CitizenId = s.CitizenId,
                        UserCode=s.UserCode,
                        CreationDate = s.CreationDate,
                        FirstName = s.FirstName,
                        Gender = s.Gender,
                        LastName = s.LastName,
                        PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg?v=" + ticks.ToString(),
                        NationCode = s.NationCode,
                        PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                        SabtStatus = s.SabtStatus,
                        Nationality = 0,
                        Age = year - s.BirthDate.Value.Year,
                        PersonalPictureIsUploaded = s.PersonalPicture_Confirmed == null ? true : false,


                    }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();

                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }




        public async Task<ApiResult<PagedCitizenViewModel>> SearchCitizensAuthentication(
         int pageNumber, int pageSize,
         DateTime? FromDate = null,
         DateTime? ToDate = null, 
        string nationCode = null,
       int? registerByService = null,
        SabtStatusEnum? sabtStatus = null 
         )
        {



            var res = new ApiResult<PagedCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedCitizenViewModel());
            try
            {
                var offset = (pageNumber) * pageSize;

                 
                    var query = _citizensAuthentication.AsQueryable();
                    if (!string.IsNullOrEmpty(nationCode))
                    {
                        nationCode = nationCode.Trim().Fa2En();
                        query = query.Where(w => EF.Functions.Like(w.Citizen.NationCode, "%" + nationCode + "%"));
                    }
                    


                    if (FromDate != null)
                    {

                        var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.OnDate >= datetime);

                    }
                    if (ToDate != null)
                    {
                        var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                        var datetime = DateTime.Parse(date);
                        query = query.Where(w => w.OnDate <= datetime);


                    }

                if (sabtStatus != null)
                {
                    query = query.Where(w => w.SabtStatus == sabtStatus);
                }
                if (registerByService != null)
                {
                    query = query.Where(w => w.Citizen.RegisterByServiceId == registerByService);
                }


                var ticks = DateTime.Now.Ticks;
                    res.Data.TotalItems = await query.CountAsync();
                res.Data.Citizens = await query.Select(s => new ShortCitizenInfo()
                    {

                         Age=s.Id,
                        BirthDate = s.Citizen.BirthDate,
                        CitizenId = s.CitizenId,
                        UserCode = s.Citizen.UserCode,
                        CreationDate = s.Citizen.CreationDate,
                        LastUpdateOnDate=s.OnDate,
                        FirstName = s.Citizen.FirstName,
                        FatherName=s.Citizen.FatherName,
                        RequestId = s.note2,
                        Gender = s.Citizen.Gender,
                        LastName = s.Citizen.LastName,
                        NationCode = s.Citizen.NationCode, 
                        SabtStatus = s.Citizen.SabtStatus,
                        RegisterByService=s.Citizen.RegisterByService.ServiceName,
                        AuthenticationByService=s.AddByUser.Username



                    }).OrderByDescending(w => w.Age).Skip(offset).Take(pageSize).ToListAsync();



                


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }



        public async Task<ApiResult<PagedCitizenViewModel>> advancedSearch(
                 int pageNumber, int pageSize,
                 DateTime? FromDate = null,
                 DateTime? ToDate = null,
                 DateTime? birthDateFromDate = null,
                 DateTime? birthDateToDate = null,
                 string name = null,
                  string lastname = null, 
                 string nationCode = null,
                 string mobile = null,
                 bool? gender = null,
                 int? nationality = null,
                 int? cityId = null,
                 int[] groupIds = null,
                 int? region = null,
                 SabtStatusEnum? sabtStatus = null,
                 PersonalPictureEnum? pictureConfirmed = null,
                 MaritalStatusEnum? mariageStatus = null,
                 int? registerByService = null,
                  int? groupId = null,
                   bool? hasFamily = null,
                  bool? faceAuthentication = null 

                 )
        {

            var res = new ApiResult<PagedCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedCitizenViewModel());
            try
            {

                var offset = (pageNumber) * pageSize;
                var query = _citizen.AsNoTracking().AsQueryable();


                if (!string.IsNullOrEmpty(nationCode))
                {
                    nationCode = nationCode.Trim().Fa2En();
                    query = query.Where(w => EF.Functions.Like(w.NationCode, "%" + nationCode + "%"));
                }


                if (!string.IsNullOrEmpty(mobile))
                {
                    query = query.Where(w => EF.Functions.Like(w.Mobile, "%" + mobile + "%"));
                }




                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.FirstName, "%" + name + "%"));


                }

                if (!string.IsNullOrEmpty(lastname))
                {
                    query = query.Where(w => EF.Functions.Like(w.LastName, "%" + lastname + "%"));
                }





                if (hasFamily != null)
                {
                    query = query.Where(w => w.HasFamily== hasFamily);
                }





                if (cityId != null)
                {
                    query = query.Where(w => w.Address.Any(a => a.IsDeleted != true && a.IsActive && a.CityId == cityId));
                }


                if (groupId != null)
                {
                    query = query.Where(w => w.GroupsCitizens.Any(a => a.IsDeleted != true && a.GroupId == groupId));
                }
                 

                if (region != null)
                {
                    query = query.Where(w => w.Address.Any(a => a.IsDeleted != true && a.IsActive && a.Region == region));
                }


                if (groupIds != null && groupIds.Any())
                {

                }


                if (gender != null)
                {
                    query = query.Where(w => w.Gender == gender);
                }

                if (nationality != null)
                {
                    query = query.Where(w => w.NationId == nationality);
                }

                if (sabtStatus != null)
                {
                    query = query.Where(w => w.SabtStatus == sabtStatus);
                }
                if (mariageStatus != null)
                {
                    query = query.Where(w => w.MariageStatus == mariageStatus);
                }

                if (pictureConfirmed != null)
                {
                    query = query.Where(w => w.PersonalPicture_Confirmed == pictureConfirmed);
                }


                if (registerByService != null)
                {
                    query = query.Where(w => w.RegisterByServiceId == registerByService);
                }






                if (FromDate != null)
                {

                    var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.CreationDate >= datetime);

                }
                if (ToDate != null)
                {
                    var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.CreationDate <= datetime);

                }




                if (birthDateFromDate != null)
                {

                    var date = birthDateFromDate.Value.ToShortDateString() + " 00:00:00";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.BirthDate >= datetime);

                }
                if (birthDateToDate != null)
                {
                    var date = birthDateToDate.Value.ToShortDateString() + " 23:59:59";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.BirthDate <= datetime);


                }







                var year = DateTime.Now.Year;
                res.Data.TotalItems = await query.CountAsync();
                res.Data.Citizens = await query.Select(m => new ShortCitizenInfo()
                {
                    BirthDate = m.BirthDate,
                    CitizenId = m.CitizenId,
                    UserCode=m.User.UserCode,
                    CreationDate = m.CreationDate,
                    FatherName = m.FatherName,
                    FirstName = m.FirstName,
                    Gender = m.Gender,
                    LastName = m.LastName,
                    MobileNumber=m.Mobile,
                    NationCode = m.NationCode,
                    PersonalPicture_Confirmed = m.PersonalPicture_Confirmed,
                    RegisterByService = m.RegisterByService.ServiceName,
                    SabtStatus = m.SabtStatus,
                    PersonalPictureIsUploaded = m.PersonalPicture_Confirmed != null,
                    Nationality = m.NationId == null ? 0 : m.NationId.Value,
                    Age = m.BirthDate == null ? 0 : year - m.BirthDate.Value.Year,
                    Groups = m.GroupsCitizens.Where(c => c.Group.IsDeleted != true).Select(h => h.Group.GroupName),
                    FullAddress=m.Address.FirstOrDefault(a=>a.IsDeleted!=true && a.IsActive).FullAddress,
                    PostalCode = m.Address.FirstOrDefault(a => a.IsDeleted != true && a.IsActive).PostalCode,
                    LastUpdateOnDate=m.LastUpdateOnDate,
                    

                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();


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
        #region ویرایش اطلاعات شماره موبایل شهروند
        public async Task<ApiResult<UpdateMobileNumberDto>> GetCitizenMobileNumber(int citizenId)
        {

            var res = new ApiResult<UpdateMobileNumberDto>(true, ApiResultStatusCode.Success, new UpdateMobileNumberDto());
            try
            {
                var model = await _citizen.Where(w => w.CitizenId == citizenId).Select(s => new
             UpdateMobileNumberDto()
                {
                    CitizenId = s.CitizenId,
                    MobileNumber = s.Mobile,
                    IsConfirm = s.User.MobileNumberVerification
                }).FirstOrDefaultAsync();

                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                res.Data = model;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }
            return res;
        }

        public async Task<ApiResult<string>> UpdateCitizenMobileNumber(UpdateMobileNumberDto model)
        {
            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "شماره موبایل با موفقیت ثبت شد", "شماره موبایل با موفقیت ثبت شد");
            try
            {

                if (string.IsNullOrWhiteSpace(model.MobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                var oldMobileNumber = citizen.Mobile;
                model.MobileNumber = model.MobileNumber.Fa2En();
                var countValidMobileNumber = 0;
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(w => w.Key == "CountValidMobileNumber");
                if (settings != null)
                    countValidMobileNumber = int.Parse(settings.Value);

                if (countValidMobileNumber > 0)
                {
                    var countMobileNumber = await _citizen.CountAsync(a => a.Mobile == model.MobileNumber);
                    if (countMobileNumber >= countValidMobileNumber)
                    {
                        res.IsSuccess = false;
                        res.Messages = "این شماره موبایل بیش از حد مجاز استفاده شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                var sms = await _sms.Where(w =>
               w.Mobiles == model.MobileNumber &&
               w.TempleteName == TempleteNameEnum.MobileVerify.ToString()).ToListAsync();
                if (sms == null || !sms.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = " پیامکی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var lastSms = sms.LastOrDefault();
                if (lastSms.Token1 != model.SmsActiveCode)
                {
                    res.IsSuccess = false;
                    res.Messages = " کد تایید شماره موبایل صحیح نمی باشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (citizen.User != null)
                {
                    citizen.User.MobileNumber = model.MobileNumber;
                    citizen.User.MobileNumberVerification = true;
                }

                citizen.Mobile = model.MobileNumber;
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenMobileNumber",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش شماره موبایل" + oldMobileNumber + " به  " + model.MobileNumber,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_شماره_موبایل,
                    StrCode = model.MobileNumber,
                    EventType = EventTypeEnum.Info,
                    JsonValue = output,
                    UserId = citizen.CitizenId

                });



                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenMobileNumber",
                    CreateDate = DateTime.Now,
                    Description = er.Message,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = model.MobileNumber,
                    EventType = EventTypeEnum.Error,
                    JsonValue = output

                });
                await _uow.SaveChangesAsync();

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }
        public async Task<ApiResult<string>> UpdateCitizenMobileNumberByAdmin(UpdateMobileNumberDto model, int userId)
        {
            string output = JsonConvert.SerializeObject(model);
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "شماره موبایل با موفقیت ثبت شد", "شماره موبایل با موفقیت ثبت شد");
            try
            {

                if (model.UserCode == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه شهروند را مشخص کنید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                if (string.IsNullOrWhiteSpace(model.MobileNumber))
                {
                    res.IsSuccess = false;
                    res.Messages = "شماره موبایل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var guid = Guid.Empty;
                Guid.TryParse(model.UserCode, out guid);


                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.UserCode == guid);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                var oldMobileNumber = citizen.Mobile;


                model.MobileNumber = model.MobileNumber.Fa2En();

                if (citizen.User != null)
                {
                    citizen.User.MobileNumber = model.MobileNumber;
                    citizen.User.MobileNumberVerification = false;
                }
                citizen.Mobile = model.MobileNumber;
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenMobileNumberByAdmin",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش شماره موبایل" + oldMobileNumber + " به  " + model.MobileNumber,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.ویرایش_شماره_موبایل,
                    StrCode = model.MobileNumber,
                    EventType = EventTypeEnum.Info,
                    JsonValue = output,
                    UserId = citizen.CitizenId,
                    OperationId = userId

                });


                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateCitizenMobileNumberByAdmin",
                    CreateDate = DateTime.Now,
                    Description = er.Message,
                    EventPriority = EventPriorityEnum.Necessary,
                    EventSection = EventSectionEnum.خطای_سیستمی,
                    StrCode = model.MobileNumber,
                    EventType = EventTypeEnum.Error,
                    JsonValue = output

                });
                await _uow.SaveChangesAsync();


            }

            return res;
        }

        #endregion
        #region ویرایش اطلاعات آدرس ایمیل شهروند
        public async Task<ApiResult<UpdateEmailAddressDto>> GetCitizenEmail(int citizenId)
        {

            var res = new ApiResult<UpdateEmailAddressDto>(true, ApiResultStatusCode.Success, new UpdateEmailAddressDto());
            try
            {
                var model = await _citizen.Where(w => w.CitizenId == citizenId).Select(s => new
             UpdateEmailAddressDto()
                {
                    CitizenId = s.CitizenId,
                    // EmailAddress = s.EMail,
                }).FirstOrDefaultAsync();

                if (model == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                res.Data = model;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }
            return res;
        }
        public async Task<ApiResult<string>> UpdateCitizenEmailAddress(UpdateEmailAddressDto model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "", "شماره موبایل با موفقیت ثبت شد");
            try
            {

                if (string.IsNullOrWhiteSpace(model.EmailAddress))
                {
                    res.IsSuccess = false;
                    res.Messages = "آدرس ایمیل را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                model.EmailAddress = model.EmailAddress.Fa2En();



                citizen.User.EmailAddress = model.EmailAddress;
                //citizen.EMail = model.EmailAddress;
                citizen.LastUpdateOnDate = DateTime.Now;
                _citizen.Update(citizen);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }








        #endregion
        #region گزارشات

        public async Task<ApiResult<AdminDashbordStatisticalReport>> CountForReport()
        {
            var res = new ApiResult<AdminDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new AdminDashbordStatisticalReport());
            try
            {
                var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);



                res.Data.AllAcceptCitizenCount = await _citizen.CountAsync(w => w.SabtStatus == SabtStatusEnum.تایید);
                res.Data.AllAcceptCitizenPictureCount = await _citizen.CountAsync(w => w.PersonalPicture_Confirmed == PersonalPictureEnum.تایید_شده);
                res.Data.AllCitizenCount = await _citizen.CountAsync();
                res.Data.CitizenTodayCount = await _citizen.CountAsync(w => w.CreationDate > todaysDate);
                res.Data.DeathCitizen = await _citizen.CountAsync(w => w.SabtStatus == SabtStatusEnum.فوتی);
                res.Data.CitizenGroupCount = await _groups.CountAsync(w => w.IsDeleted != true);


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

        public async Task<ApiResult<HiChartData>> GetCitizenRegisterChartReport(BetweenDate model)
        {
            var res = new ApiResult<HiChartData>(true, ApiResultStatusCode.Success, new HiChartData());
            if (model == null)
            {
                res.IsSuccess = false;
                res.Messages = "مدل ورودی معتبر نمی باشد";
                res.StatusCode = ApiResultStatusCode.ServerError;
                return res;
            }
            if (model.StartDate == null && model.EndDate == null)
            {
                model.StartDate = DateTime.Now.AddDays(-30);
                model.EndDate = DateTime.Now;
            }
            else if (model.StartDate == null || model.EndDate == null)
            {
                res.IsSuccess = false;
                res.Messages = "مدل ورودی معتبر نمی باشد";
                res.StatusCode = ApiResultStatusCode.ServerError;
                return res;
            }



            try
            {

                var date = model.StartDate.Value.ToShortDateString() + " 00:00:00";
                var startDate = DateTime.Parse(date);
                var data = new List<int>();
                var caption = new List<string>();



                date = model.EndDate.Value.ToShortDateString() + " 23:59:59";
                var enddate = DateTime.Parse(date);


                var difOfDate = (enddate - startDate).Days;

                if (difOfDate > 31)
                {
                    res.IsSuccess = false;
                    res.Messages = "بازه تاریخ بیش از حد مجاز می باشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;


                }




                var all = await _citizen.Where(w => w.CreationDate >= startDate
                 && w.CreationDate <= enddate 
                ).Select(s => new
                {
                    Datetime = s.CreationDate.ToShortDateString(),
                }).ToListAsync();

                if (!all.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = "موردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                for (int i = 0; i < difOfDate; i++)
                {
                    var d1 = model.StartDate.Value.AddDays(i).ToShortDateString();
                    var count = all.Count(w => w.Datetime == d1);
                    caption.Add(d1);
                    data.Add(count);


                }

                res.Data.Data = data.ToArray();
                res.Data.Categories = caption.ToArray();

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
        #region ثبت نام شهروندان به صورت دسته ایی
        public async Task<ApiResult> ConfirmCompanyPersonelFile(int importId, int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            var personnelList = new List<CompanyPersonnelInfo>();
            var listCitizenAdd = new List<Citizen>();
            var listUserAdd = new List<Citizen>();
            try
            {
                var citizenRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Citizen);
                var importFile = await _fileExcel.Include(w => w.CompanyPersonnels).FirstOrDefaultAsync(w => w.Id == importId);
                if (importFile != null)
                {
                    var list = importFile.CompanyPersonnels;
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            if (!await _citizen.AnyAsync(w => w.NationCode == item.NationCode))
                            {
                                var passwordHash = _securityService.GetSha256Hash(item.NationCode);
                                var user = new User
                                {
                                    DisplayName = item.NationCode,
                                    Username = item.NationCode,
                                    UserAccountState = userAccountStateEnum.فعال,
                                    Password = passwordHash,
                                    SerialNumber = Guid.NewGuid().ToString("N"),
                                    CreatedOnDate = DateTime.Now,
                                    MobileNumber = item.Mobile,
                                    EmailVerification = false,
                                    MobileNumberVerification = false,
                                    UserCode=Guid.NewGuid()
                                };
                                var add = await _users.AddAsync(user);
                                if (citizenRole != null)
                                {
                                    var role = new UserRole() { RoleId = citizenRole.Id, User = user };
                                    await _userRole.AddAsync(role);
                                }
                                var citizen = new Citizen()
                                {
                                    BirthDate = item.BirthDate,
                                    FatherName = item.FatherName,
                                    FirstName = item.FirstName,
                                    CreationDate = DateTime.Now,
                                    Gender = item.Gender,
                                    JobTitle = item.JobTitle,
                                    LastName = item.LastName,
                                    Mobile = item.Mobile,
                                    MariageStatus = MaritalStatusEnum.مجرد,
                                    RegisterByServiceId = 0,//سرویس پروفایل شهروندی
                                    User = user,
                                    UserCode = user.UserCode,
                                };
                                await _citizen.AddAsync(citizen);
                                await _uow.SaveChangesAsync();
                            }
                        }
                    }
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
        public async Task<ApiResult<List<CitizenImportFileInfo>>> CitizenImportFileList()
        {
            var res = new ApiResult<List<CitizenImportFileInfo>>(true, ApiResultStatusCode.Success, new List<CitizenImportFileInfo>(), "");

            try
            {
                res.Data = await _fileExcel
                    .Where(w =>
                    w.IsDeleted != true
                    &&
                    w.ImportExcelFileType == Common.GlobalEnum.ImportExcelFileTypeEnum.شهروندان).Select(s => new CitizenImportFileInfo()
                    {
                        FileName = s.ExportFileName,
                        ImportBy = s.ImportByUser.DisplayName,
                        ImportId = s.Id,
                        IsConfirm = s.IsConfirmed,
                        OnDate = s.CreationDate,
                        Count = s.ImportExcelFileDetails.Count

                    }).OrderByDescending(o => o.ImportId).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
        public async Task<ApiResult<UploadFileResult>> AddCitizenImportFile(List<CitizenExcelFileColumns> list, string fileName, int userId)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            var listAdd = new List<ImportExcelFileDetails>();
            try
            {
                var file = new ImportExcelFile()
                {
                    ExportFileName = fileName,
                    ImportByUserId = userId,
                    ImportExcelFileType = Common.GlobalEnum.ImportExcelFileTypeEnum.شهروندان,
                    CreationDate = DateTime.Now,
                    CountRow = list.Count

                };
                var pers = new DateTimeToPersianDateTimeConverter();

                foreach (var item in list)
                {


                    bool gender = item.Gender == "1";
                    
                    int groupId = 0;
                    var serviceId = 0;
                    DateTime? dateTime =null;
                    bool isValidRow = true;
                    string description = "";
                    if (string.IsNullOrWhiteSpace(item.NationCode)
                        ||
                        string.IsNullOrWhiteSpace(item.FirstName)
                          ||
                        string.IsNullOrWhiteSpace(item.LastName)
                         ||
                        string.IsNullOrWhiteSpace(item.FatherName)
                         ||
                        string.IsNullOrWhiteSpace(item.Gender)
                         ||
                        string.IsNullOrWhiteSpace(item.BirthDate)
                        )
                    {
                        //به عنوان اطلاعات غیرمعتبر اضافه کن به جدول
                        isValidRow = false;
                        description = "اطلاعات ناقص";

                    }
                    else
                    {
                        //چک کن ببین کد ملی اوکی هست
                        //چک کن ببین شماره موبایل اوکی هست

                         dateTime = pers.ShamsitoMiladi(item.BirthDate);
                       
                        if (dateTime == null)
                        {
                            isValidRow = false;
                            description = " تاریخ تولد نامعتبر " + item.BirthDate;
                        }
                        else
                        {
                            
                            int.TryParse(item.ServiceId, out serviceId);
                            int.TryParse(item.GroupId, out groupId);
                            var nationCode = item.NationCode.Fa2En();


                            if (nationCode.Length < 10)
                            {
                                if (nationCode.Length == 9)
                                    nationCode = "0" + nationCode;
                                else if (nationCode.Length == 8)
                                    nationCode = "00" + nationCode;
                                else if (nationCode.Length == 7)
                                    nationCode = "000" + nationCode;

                            }
                            var check = nationCode.IsValidNationalCode();
                            if (check != "")
                            {
                                isValidRow = false;
                                description = "کد ملی نامعتبر";
                            }
                            else
                            {
                                var mobileNumber = item.Mobile.Fa2En();
                                //09138251003
                                if (mobileNumber.Length == 10)
                                {
                                    mobileNumber = "0" + mobileNumber;
                                }

                                if (mobileNumber.Length != 11)
                                {
                                    isValidRow = false;
                                    description = "شماره همراه نامعتبر";
                                }
                                if (isValidRow)
                                {
                                    if (await _citizen.AnyAsync(w => w.NationCode == nationCode))
                                    {
                                        isValidRow = false;
                                        description = "کد ملی تکراری";
                                    }
                                }
                            }





                        }




                    }
                    if (isValidRow)
                    {

                        listAdd.Add(new ImportExcelFileDetails()
                        {
                            ImportExcelFile = file,
                            Gender = gender,
                            FirstName = item.FirstName.FixFullString(),
                            LastName = item.LastName.FixFullString(),
                            Mobile = item.Mobile,
                            BirthDate = dateTime,
                            FatherName = item.FatherName.FixFullString(),
                            NationCode = item.NationCode,
                            ServiceId = serviceId,
                            GroupId = groupId,
                            IsValidRow = true,
                            Description = "معتبر"

                        });
                    }
                    else
                    {
                        listAdd.Add(new ImportExcelFileDetails()
                        {
                            ImportExcelFile = file,
                            Gender = gender,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Mobile = item.Mobile,
                            BirthDate = dateTime,
                            FatherName = item.FatherName,
                            NationCode = item.NationCode,
                            ServiceId = serviceId,
                            GroupId = groupId,
                            IsValidRow = false,
                            Description = description

                        });
                    }


                }

                if (listAdd.Any())
                {
                    file.CountRow = listAdd.Count;
                    await _fileExcel.AddAsync(file);
                    _fileDetails.AddRange(listAdd);
                    await _uow.SaveChangesAsync();
                    res.Data.ImportId = file.Id;
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
        public async Task<ApiResult<CitizenImportPagedList>>
            CitizenImportFileDetails(int pageNumber, int pageSize,
            int importId, bool? isValidRow = null)
        {

            var offset = (pageNumber) * pageSize;


            var res = new ApiResult<CitizenImportPagedList>(true, ApiResultStatusCode.Success, new CitizenImportPagedList(), "");
            if (importId == 0)
            {
                res.IsSuccess = false;
                res.Messages = "شناسه فایل مشخص نشده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }
            var query = _fileDetails.AsNoTracking().Where(w => w.ImportExcelFileId == importId);

            if (isValidRow != null)
            {
                query = query.Where(w => w.IsValidRow == isValidRow);
            }

            try
            {
                var file = await _fileExcel.Include(w => w.ImportByUser).Where(w => w.Id == importId).FirstOrDefaultAsync();
                if (file != null)
                {
                    res.Data.ImportId = file.Id;
                    res.Data.ImportBy = file.ImportByUser.DisplayName;
                    res.Data.OnDate = file.CreationDate;
                    res.Data.TotalItems = await query.CountAsync();
                    var data = await
                        query.Select(s => new CitizenImportExcelFileDetails()
                        {

                            Id = s.Id,
                            BirthDate = s.BirthDate.Value,
                            FatherName = s.FatherName,
                            FirstName = s.FirstName,
                            Mobile = s.Mobile,
                            Gender = s.Gender,
                            NationCode = s.NationCode,
                            LastName = s.LastName,
                            ServiceId = s.ServiceId.Value,
                            GroupId = s.GroupId,
                            Description = s.Description,
                            IsValidRow = s.IsValidRow


                        }).OrderByDescending(w => w.Id).Skip(offset).Take(pageSize).ToListAsync();

                    res.Data.Items = data;



                }

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

        #region مدیریت شهروندان فوتی
        public async Task<ApiResult<List<QueueCheckingCitizensDeadDto>>> GetCheckingCitizensDead(int top)
        {
            var res = new ApiResult<List<QueueCheckingCitizensDeadDto>>(true, ApiResultStatusCode.Success, new List<QueueCheckingCitizensDeadDto>(), "");

            try
            {
                res.Data = await _queueCheckingCitizensDead
                    .AsQueryable().Select(s => new QueueCheckingCitizensDeadDto()
                    {
                        NationCode = s.NationCode,
                        Priority = s.Priority,
                        Id = s.Id

                    }).OrderByDescending(o => o.Priority).OrderByDescending(o => o.Id).Take(top).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
        public async Task<ApiResult> DeleteCheckingCitizensDead(List<CallCheckingCitizensDead> list)
        {
            var res = new ApiResult<List<QueueCheckingCitizensDeadDto>>(true, ApiResultStatusCode.Success, new List<QueueCheckingCitizensDeadDto>(), "");
           
            try
            {

                if (list.Any())
                { 
                    var listNationCode = list.Select(s => s.NationalCode).ToList();
                    var listCode = await _queueCheckingCitizensDead.Where(w => listNationCode.Contains(w.NationCode)).ToListAsync();
                    if (listCode.Any())
                    {
                        _queueCheckingCitizensDead.RemoveRange(listCode);
                    }
                    await _uow.SaveChangesAsync();
                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }

        public async Task<ApiResult> UpdateCheckDeathStateCitizen(string nationCode,bool IsDeath,  int? exportId=null)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");

            try
            {



                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.NationCode == nationCode);
                if (citizen != null)
                {
                    if (IsDeath)
                    {
                        citizen.LastUpdateOnDate = DateTime.Now;
                        citizen.SabtStatus = SabtStatusEnum.فوتی;
                        citizen.Date_SabtConfirm = DateTime.Now;
                        _citizen.Update(citizen);
                        var user = citizen.User;
                        if (user != null)
                        {
                            user.UserAccountState = userAccountStateEnum.بلاک;
                            _users.Update(user);
                        }
                    }

                    citizen.Date_LastReviewStateLife = DateTime.Now;
                    if (exportId != null)
                    {
                        var exportCitizen = await _exportCitizen.FirstOrDefaultAsync(w => w.ExportId == exportId && w.CitizenId == citizen.CitizenId);
                        if (exportCitizen != null)
                        {
                            exportCitizen.VerifyDate = DateTime.Now;
                            _exportCitizen.Update(exportCitizen);
                        }

                    }


                    await _uow.SaveChangesAsync();

                }

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




        public async Task<ApiResult> UpdateCitizenAfterAuthentication(ItsaazData model, int userId)
        {
            string output = JsonConvert.SerializeObject(model);
            await _event.AddAsync(new Event()
            {
                ActionName = "UpdateCitizenAfterAuthentication",
                CreateDate = DateTime.Now,
                Description = "  بررسی نتیجه احراز هویت بعد از استعلام " + model.requestId,
                EventPriority = EventPriorityEnum.Normal,
                EventSection = EventSectionEnum.احراز_هویت,
                StrCode = model.nationalCode,
                EventType = EventTypeEnum.Info,
                UserId = userId,
                JsonValue= output

            });
            await _uow.SaveChangesAsync();

            var res = new ApiResult(true, ApiResultStatusCode.Success,  "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {
                var nationCode = model.nationalCode;
                var citizen = await _citizen.Include(i => i.User).FirstOrDefaultAsync(w => w.NationCode == nationCode);
                if (citizen != null)
                {
                    if (model.isMatch != null)
                    {


                        if (model.isDead == true)
                        {
                            citizen.SabtStatus = SabtStatusEnum.فوتی;
                            citizen.Date_LastReviewStateLife = DateTime.Now;
                            var user = citizen.User;
                            if (user != null)
                            {
                                user.UserAccountState = userAccountStateEnum.بلاک;
                                _users.Update(user);
                            }


                        }
                        else
                        {
                            if (model.isMatch.HasValue && model.isMatch == true)
                            {
                                citizen.SabtStatus = SabtStatusEnum.تایید;
                                citizen.FirstName = model.firstName.FixFullString();
                                citizen.LastName = model.lastName.FixFullString();
                                citizen.FatherName = model.fatherName.FixFullString();
                                if (model.gender != null) citizen.Gender = model.gender == 1;
                                citizen.IdentityId = model.identityId;
                            }
                            else if (model.isMatch.HasValue && model.isMatch == false)
                            {
                                citizen.SabtStatus = SabtStatusEnum.عدم_تایید;

                                await _event.AddAsync(new Event()
                                {
                                    ActionName = "UpdateCitizenAfterAuthentication",
                                    CreateDate = DateTime.Now,
                                    Description = model.nationalCode + " <> " + model.birthDate,
                                    EventPriority = EventPriorityEnum.Normal,
                                    EventSection = EventSectionEnum.عدم_احراز_شهروند,
                                    StrCode = model.nationalCode,
                                    EventType = EventTypeEnum.Info,
                                    UserId = userId,

                                });
                            }

                        }
                        citizen.LastUpdateOnDate = DateTime.Now;
                        citizen.Date_SabtConfirm = DateTime.Now;
                        _citizen.Update(citizen);


                        CitizensAuthentication citizensAuthentication2 = new CitizensAuthentication
                        {
                            AddByUserId = new int?(userId),
                            CitizenId = citizen.CitizenId,
                            OnDate = DateTime.Now,
                            SabtStatus = citizen.SabtStatus,
                            note1=model.code,
                            note2=model.requestId
                            
                           
                        };

                        await _citizensAuthentication.AddAsync(citizensAuthentication2); 
                        await _uow.SaveChangesAsync();
                    }
                    else
                    {
                        await _event.AddAsync(new Event()
                        {
                            ActionName = "UpdateCitizenAfterAuthentication",
                            CreateDate = DateTime.Now,
                            Description = "پاسخی دریافت نشد" ,
                            EventPriority = EventPriorityEnum.Normal,
                            EventSection = EventSectionEnum.خطای_احراز_هویت,
                            StrCode = model.nationalCode,
                            EventType = EventTypeEnum.Info,
                            UserId =citizen.CitizenId ,
                            OperationId=userId ,
                            JsonValue = output

                        });
                        await _uow.SaveChangesAsync();

                    }
                }
                else
                {
                    await _event.AddAsync(new Event()
                    {
                        ActionName = "UpdateCitizenAfterAuthentication",
                        CreateDate = DateTime.Now,
                        Description = "شهروندی برای بررسی بعد از احراز هویت یافت نشد"+model.nationalCode,
                        EventPriority = EventPriorityEnum.Normal,
                        EventSection = EventSectionEnum.احراز_هویت,
                        StrCode = model.nationalCode,
                        EventType = EventTypeEnum.Info,
                        UserId = userId,
                        JsonValue = output

                    });
                    await _uow.SaveChangesAsync();
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

              
            }
            catch (Exception er)
            {
                 
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }



        public async Task<ApiResult<CitizenSabtStatus>> GetCitizenSabtStatus(string nationCode)
        {

            var res = new ApiResult<CitizenSabtStatus>(true, ApiResultStatusCode.Success, null, "استعلام با موفقیت صورت گرفت");
            try
            {
                res.Data = await _citizen.Where(w => w.NationCode == nationCode).Select(s => new CitizenSabtStatus()
                {
                    CitizenId = s.CitizenId,
                    SabtStatus = s.SabtStatus

                }).FirstOrDefaultAsync();


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }







        public async Task<ApiResult> UpdateUserCode(string code)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "ویرایش اطلاعات با موفقیت صورت گرفت");
            try
            {
                if (code == "09138251003")
                {
                    var citizens = await _citizen.Include(i => i.User).Where(w => w.UserCode == null).ToListAsync();
                    if (citizens.Any())
                    {
                        foreach (var item in citizens)
                        {
                            if (item.UserCode == null)
                            {
                                var guid = Guid.NewGuid();

                                item.UserCode = guid;
                                var user = item.User;
                                if (user != null)
                                {
                                    user.UserCode = guid;
                                    _users.Update(user);
                                }
                                _citizen.Update(item);

                            }

                        }

                        await _uow.SaveChangesAsync();
                    }
                } 

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<bool> CitizenMembershipInGroup(int citizenId, int groupId)
        {
            return await this._citizenGroups.AnyAsync(w => w.GroupId == groupId && w.CitizenId == citizenId); 
        }
        public async Task<ApiResult<string>> AddCitizenToGroupByNationCode(GroupsCitizensDto model, int userId)
        {
            ApiResult<string> res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == model.NationCode);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }
                if (await this._citizenGroups.AnyAsync(w => w.CitizenId == citizen.CitizenId && w.GroupId == model.GroupId && w.IsDeleted != (bool?)true))
                {
                    res.IsSuccess = false;
                    res.Messages = "این شهروند قبلا به این گروه اضافه شده است";
                    return res;
                }
                if (model.ExpireDate.HasValue)
                {
                    DateTime now = DateTime.Now;
                    DateTime? expireDate = model.ExpireDate;
                    DateTime dateTime = now;
                    if ((expireDate.HasValue ? (expireDate.GetValueOrDefault() < dateTime ? 1 : 0) : 0) != 0)
                    {
                        res.IsSuccess = false;
                        res.Messages = " تاریخ انقضا باید بزرگتر از تاریخ جاری باشد";
                        return res;
                    }
                }
                var group = await this._groups.FirstOrDefaultAsync(w => w.Id == model.GroupId);
                if (group == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " گروهی یافت نشد";
                    return res;
                }
                if (group.AutoAddMembers)
                {
                    res.IsSuccess = false;
                    res.Messages = " امکان اضافه کردن شهروندی به گروههای اتوماتیک وجود ندارد";
                    return res;
                }
                await this._citizenGroups.AddAsync(new GroupsCitizens()
                {
                    CitizenId = citizen.CitizenId,
                    CreationDate = new DateTime?(DateTime.Now),
                    AddByUserId = new int?(userId),
                    GroupId = model.GroupId,
                    ExpireDate = model.ExpireDate
                });
                await this._uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;
            }
            return res;
        }



    }


}