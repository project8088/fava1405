using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.Citizens
{
    public interface ICitizenFamiliesService
    {
         
        Task<ApiResult> AcceptFamilyByCitizen(ReviewFamily model, int myId);
        Task<ApiResult> AddFamily(CheckFamilyModel familyProfile ,int myId );
        Task<ApiResult> AddFamilyMemberIfNotAny(AddFamilyCitizenDto model, int myId); 
        Task<ApiResult> ConfirmFamilyByAdmin(ConfirmFamily model, int ConfirmBy);
        Task<ApiResult<List<CitizenFamiliesInfo>>> GetAllCitizenFamily(int citizenId);
        Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByAdmin(string userCode);
        Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByFamily(int citizenId);
        Task<ApiResult<CitizenFamiliesInfo>> GetCitizenFamily(int familyId, int myId);
        Task<ApiResult> Remove(DeleteFamilly model);
        Task<ApiResult> RemoveByCitizen(int id, int citizenId);
        Task<ApiResult<PagedFamilyCitizenViewModel>> SearchFamilyCitizens(int pageNumber, int pageSize, 
            DateTime? FromDate = null, DateTime? ToDate = null, 
            string name = null, string nationCode = null, FamilyRelationshipsEnum? familyRelation = null,
             int? groupId = null);
        Task<ApiResult> UpdateFamilyMemberByCitizen(UpdateFamilyCitizenDto model, int myId);
    }
    public class CitizenFamiliesService : ICitizenFamiliesService
    {



        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CitizenFamily> _family;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<CitizenProfile> _profile;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _role;
        private readonly DbSet<Address> _address;
        private readonly DbSet<UserRole> _userRole;
        private readonly ISecurityService _securityService;
        private readonly DbSet<GroupsCitizens> _citizenGroups;



        public CitizenFamiliesService(IUnitOfWork uow, ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _family = _uow.Set<CitizenFamily>();
            _citizen = _uow.Set<Citizen>();
            _address = _uow.Set<Address>();
            _profile = _uow.Set<CitizenProfile>();
            _userRole = _uow.Set<UserRole>();
            _users = _uow.Set<User>();
            _role = _uow.Set<Role>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        #endregion

        public async Task<ApiResult<List<CitizenFamiliesInfo>>> GetAllCitizenFamily(int  citizenId )
        {
            var res = new ApiResult<List<CitizenFamiliesInfo>>(true, ApiResultStatusCode.Success, new List<CitizenFamiliesInfo>());
            try
            {
                var query = _family.Where(w => w.CitizenId == citizenId  && w.IsDeleted!=true  ); 
                res.Data = await query.Select(s => new CitizenFamiliesInfo()
                {
                    AcceptDate=s.AcceptDate,
                    CitizenId=s.CitizenId,
                    Citizen=s.Citizen.FirstName + " "+s.Citizen.LastName,
                    ConfirmDate=s.ConfirmDate,
                    ConfirmerUser = s.ConfirmerUser.Username,
                    CreationDate=s.CreationDate,
                    Id=s.Id,
                    FamilyCitizen=s.FamilyCitizen.FirstName+" "+s.FamilyCitizen.LastName,
                    FamilyCitizenId=s.FamilyCitizenId.Value,
                    FamilyNationCode= s.FamilyCitizen.NationCode,
                    FamilyRelation =s.FamilyRelation,
                    Heirs=s.Heirs,
                    NationCode=s.Citizen.NationCode,
                    UnderProtection=s.UnderProtection, 
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


        public async Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByAdmin(string userCode)
        {
            var res = new ApiResult<CitizenAndFamilyList>(true, ApiResultStatusCode.Success, new CitizenAndFamilyList());
            try
            {
                var year = DateTime.Today.Year;
                var code = Guid.Empty;
                Guid.TryParse(userCode, out code);



                var citizen = await _citizen.Where(w =>
               w.UserCode == code).Select(s => new ShortFamilyCitizenInfo()
               {
                   CitizenId = s.CitizenId,
                   UserCode=s.UserCode,

                   CreationDate = s.CreationDate,
                   FatherName = s.FatherName,
                   FirstName = s.FirstName,
                   Gender = s.Gender,
                   LastName = s.LastName,
                   Mobile = s.Mobile,
                   NationCode = s.NationCode,
                   PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                   PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg",
                   BirthDate = s.BirthDate,
                   PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                   RegisterByService = s.RegisterByService.ServiceName,
                   SabtStatus = s.SabtStatus,
                   Nationality = 0,
                   Age = year - s.BirthDate.Value.Year,
                   MariageStatus=s.MariageStatus


               }).FirstOrDefaultAsync();
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                res.Data.Citizen = citizen;
                res.Data.CitizenId = citizen.CitizenId;
                res.Data.UserCode = citizen.UserCode;
                var query = _family.Where(w => w.CitizenId == citizen.CitizenId && w.IsDeleted != true);
                res.Data.FamilyList = await query.Select(s => new CitizenFamiliesInfo()
                {
                    AcceptDate = s.AcceptDate,
                    CitizenId = s.CitizenId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    UserCode=s.Citizen.UserCode,
                    FamilyUserCode=s.FamilyCitizen.UserCode,
                    ConfirmDate = s.ConfirmDate,
                    Confirm=s.Confirm,
                    ConfirmerUser = s.ConfirmerUser.Username,
                    CreationDate = s.CreationDate,
                    Id = s.Id,
                    FamilyCitizen = s.FamilyCitizen.FirstName + " " + s.FamilyCitizen.LastName,
                    FamilyCitizenId = s.FamilyCitizenId.Value,
                    FamilyNationCode = s.FamilyCitizen.NationCode,
                    FamilyRelation = s.FamilyRelation,
                    Heirs = s.Heirs,
                    NationCode = s.Citizen.NationCode,
                    UnderProtection = s.UnderProtection,
                    FamilyPictureUrl = s.FamilyCitizen.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.FamilyCitizen.NationCode + ".jpg",
                    FamilyAge = year - s.FamilyCitizen.BirthDate.Value.Year,
                    FamilyBirthDate = s.FamilyCitizen.BirthDate,
                    FamilyMariageStatus=s.FamilyCitizen.MariageStatus,


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

        public async Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByFamily(int citizenId)
        {
            var res = new ApiResult<CitizenAndFamilyList>(true, ApiResultStatusCode.Success, new CitizenAndFamilyList());
            try
            {
                var year = DateTime.Today.Year;

                var citizen = await _citizen.Where(w =>
               w.CitizenId == citizenId).Select(s => new ShortFamilyCitizenInfo()
               {
                   CitizenId = s.CitizenId,
                   CreationDate = s.CreationDate,
                   FatherName = s.FatherName,
                   FirstName = s.FirstName,
                   Gender = s.Gender,
                   LastName = s.LastName,
                   Mobile = s.Mobile,
                   NationCode = s.NationCode,
                   PersonalPicture_Confirmed = s.PersonalPicture_Confirmed,
                   PersonalPictureUrl = s.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.UserCode + "/" + s.NationCode + ".jpg",
                   BirthDate = s.BirthDate,
                   PersonalPictureIsUploaded = s.PersonalPicture_Confirmed != null,
                   RegisterByService = s.RegisterByService.ServiceName,
                   SabtStatus = s.SabtStatus,
                   Nationality = 0,
                   Age = year - s.BirthDate.Value.Year,
                   MariageStatus = s.MariageStatus


               }).FirstOrDefaultAsync();
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                res.Data.Citizen = citizen;
                res.Data.CitizenId = citizenId;

                var query = _family.Where(w => w.FamilyCitizenId == citizenId && w.IsDeleted != true);
                res.Data.FamilyList = await query.Select(s => new CitizenFamiliesInfo()
                {
                    AcceptDate = s.AcceptDate,
                    CitizenId = s.CitizenId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    ConfirmDate = s.ConfirmDate,
                    ConfirmerUser = s.ConfirmerUser.Username,
                    CreationDate = s.CreationDate,
                    Id = s.Id,
                    FamilyCitizen = s.FamilyCitizen.FirstName + " " + s.FamilyCitizen.LastName,
                    FamilyCitizenId = s.FamilyCitizenId.Value,
                    FamilyNationCode = s.FamilyCitizen.NationCode,
                    FamilyRelation = s.FamilyRelation,
                    Heirs = s.Heirs,
                    NationCode = s.Citizen.NationCode,
                    UnderProtection = s.UnderProtection,
                    FamilyPictureUrl = s.FamilyCitizen.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.FamilyCitizen.UserCode + "/" + s.FamilyCitizen.NationCode + ".jpg",
                    FamilyAge = year - s.FamilyCitizen.BirthDate.Value.Year,
                    FamilyBirthDate = s.FamilyCitizen.BirthDate,
                    FamilyMariageStatus = s.FamilyCitizen.MariageStatus,


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




        public async Task<ApiResult<CitizenFamiliesInfo>> GetCitizenFamily(int familyId,int myId)
        {
            var res = new ApiResult<CitizenFamiliesInfo>(true, ApiResultStatusCode.Success, new CitizenFamiliesInfo());
            try
            {
                var query = _family.Where(w =>w.CitizenId== myId &&  w.FamilyCitizenId == familyId && w.IsDeleted != true);
                res.Data = await query.Select(s => new CitizenFamiliesInfo()
                {
                    AcceptDate = s.AcceptDate,
                    CitizenId = s.CitizenId,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    ConfirmDate = s.ConfirmDate,
                    ConfirmerUser = s.ConfirmerUser.Username,
                    CreationDate = s.CreationDate,
                    Id = s.Id,
                    FamilyCitizen = s.FamilyCitizen.FirstName + " " + s.FamilyCitizen.LastName,
                    FamilyCitizenId = s.FamilyCitizenId.Value,
                    FamilyNationCode = s.FamilyCitizen.NationCode,
                    FamilyRelation = s.FamilyRelation,
                    Heirs = s.Heirs,
                    NationCode = s.Citizen.NationCode,
                    UnderProtection = s.UnderProtection,
                }).FirstOrDefaultAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }



        public async Task<ApiResult>  AddFamily( CheckFamilyModel familyProfile,int myId )
        {

           
            var res = new ApiResult(false, ApiResultStatusCode.BadRequest, "" );
            //چک کن ببین شهروندی با این مشخات وجود دارد
            try
            {

                var date =DateTime.Parse( familyProfile.BirthDate.ToShortDateString());
                var myFamily = await _citizen.FirstOrDefaultAsync(w =>
           w.NationCode == familyProfile.NationCode &&
           w.BirthDate == date);

                if (myFamily == null)
                {
                    res.Messages = "شهروندی با مشخصات وارد شده یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                #region رابطه تکراری

                if (myFamily.CitizenId == myId)
                {
                    res.Messages = "کد ملی عضو جدید خانواده را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }

                //چک کن قبلا به رابطه من اضافه نشده باشد 
                if (await _family.AnyAsync(w => 
                w.CitizenId == myId && w.FamilyCitizenId == myFamily.CitizenId
                && w.IsDeleted!=true
                
                ))
                {
                    res.Messages = "قبلا این عضو خانواده اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }




                //یک شخص نباید دو مادر داشته باشد
                //یک شخص نباید دو تا پدر داشته باشد
                //اگر رابطه همسر بود نباید قبلا همسر شخص دیگری اضافه شده باشد
                //یک شخص نمی تواند دو همسر داشته باشد البته میتونه ولی من گفتم یکی همسر
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true &&
                    w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.مادر))
                    {

                        res.Messages = "نسبت مادر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true && w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.پدر))
                    {
                        res.Messages = "نسبت پدر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true && w.FamilyCitizenId == myFamily.CitizenId && w.FamilyRelation == FamilyRelationshipsEnum.همسر))
                    {
                        res.Messages = "امکان اضافه کردن این رابطه وجود ندارد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                    if (await _family.AnyAsync(w =>w.IsDeleted!=true &&
                                        w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.همسر))
                    {

                        res.Messages = "نسبت همسر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }



                }

                #endregion

                var myProfile = await _citizen.Where(w => w.CitizenId == myId).FirstOrDefaultAsync();



                #region قوانین جنسیتی

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (myProfile.Gender == myFamily.Gender)
                    {
                        //رابطه جنسیت یکسان
                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (myFamily.Gender)
                {

                    //طرف مقابل اقا است
                    if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.خواهر
                        || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                        )
                    {
                        //نسبت اشتباه انتخاب شده

                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;


                    }


                }
                else
                {
                    //طرف مقابل خانم است
                    if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.برادر
                       || familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر

                       )
                    {
                        //نسبت اشتباه انتخاب شده

                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }

                }




                #endregion  
                var today = DateTime.Today;
                int myage = today.Year - myProfile.BirthDate.Value.Year;
                int citizenage = today.Year - familyProfile.BirthDate.Year;

                #region قوانین سنی
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر
                   || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                   )
                {
                    //اگر نسبت پدر یا مادر بود سن من باید حداقل ده سال از سن پدر یا مادرم کمتر باشد 


                    if (myage + 10 > citizenage)
                    {
                        res.Messages = "عدم تطابق سنی:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    //سن طرف مقابل برای تاهل بودن

                    if (citizenage < 15)
                    {
                        res.Messages = "عدم تطابق سنی برای تاهل:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                else if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.فرزند)
                {

                    //سن من باید بزرگتر از سن رابطه باشد 
                    if ((citizenage + 10) > myage)
                    {
                        res.Messages = "عدم تطابق سنی :سن فرزند باید کوچکتر از سن شما باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                    //باید سن شخص بزرگتر از 15 سال باشد
                    if (myage < 15)
                    {
                        res.Messages = "عدم تطابق سنی :اطلاعات وارد شده منطبق نيست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }



                }
                #endregion
                #region قوانین تاهل
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (myProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {

                        res.Messages = "وضعیت تاهل شما مجرد می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }
                    if (myFamily.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.Messages = "وضعیت تاهل همسر شما مجرد انتخاب شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.فرزند)
                {
                    if (myProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {

                        res.Messages = "وضعیت تاهل شما مجرد می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر
                   || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                    )
                {
                    if (myFamily.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.Messages = "وضعیت تاهل نباید مجرد  باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                #endregion



                var add = new CitizenFamily()
                {
                    CitizenId = myId,
                    FamilyCitizenId = myFamily.CitizenId,
                    CreationDate = DateTime.Now,
                    FamilyRelation = familyProfile.FamilyRelation,
                };

                await _family.AddAsync(add);
                myProfile.HasFamily = true;
                myFamily.HasFamily = true;
                _citizen.Update(myProfile);
                _citizen.Update(myFamily);
                await _uow.SaveChangesAsync();
                res.Messages = "عضو جدید خانواده با موفقیت ثبت شد";
                res.IsSuccess = true;
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
     
        public async Task<ApiResult> AddFamilyMemberIfNotAny(AddFamilyCitizenDto familyProfile, int myId)
        {


            var res = new ApiResult(false, ApiResultStatusCode.BadRequest, "");
            //چک کن ببین شهروندی با این مشخات وجود دارد
            try
            {

                if(string.IsNullOrWhiteSpace(familyProfile.NationCode))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی عضو جدید خانواده را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                #region رابطه تکراری

                //کد ملی اصلاح شود
                familyProfile.NationCode = familyProfile.NationCode.Fa2En();

                //چک کن قبلا به رابطه من اضافه نشده باشد 
                if (await _family.AnyAsync(w =>
                w.CitizenId == myId && w.FamilyCitizen.NationCode == familyProfile.NationCode
                && w.IsDeleted != true

                ))
                {
                    res.Messages = "قبلا این عضو خانواده اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }




                //یک شخص نباید دو مادر داشته باشد
                //یک شخص نباید دو تا پدر داشته باشد
                //اگر رابطه همسر بود نباید قبلا همسر شخص دیگری اضافه شده باشد
                //یک شخص نمی تواند دو همسر داشته باشد البته میتونه ولی من گفتم یکی همسر
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true &&
                    w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.مادر))
                    {

                        res.Messages = "نسبت مادر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true && w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.پدر))
                    {
                        res.Messages = "نسبت پدر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (await _family.AnyAsync(w => w.IsDeleted != true && w.FamilyCitizen.NationCode == familyProfile.NationCode && w.FamilyRelation == FamilyRelationshipsEnum.همسر))
                    {
                        res.Messages = "امکان اضافه کردن این رابطه وجود ندارد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                    if (await _family.AnyAsync(w => w.IsDeleted != true &&
                                        w.CitizenId == myId && w.FamilyRelation == FamilyRelationshipsEnum.همسر))
                    {

                        res.Messages = "نسبت همسر قبلا به لیست خانواده اضافه شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }



                }

                #endregion



                //اطلاعات من 
                var myProfile = await _citizen.Where(w => w.CitizenId == myId).FirstOrDefaultAsync();
                



                #region قوانین جنسیتی

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (myProfile.Gender == familyProfile.Gender)
                    {
                        //رابطه جنسیت یکسان
                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }

                if (familyProfile.Gender)
                {

                    //طرف مقابل اقا است
                    if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.خواهر
                        || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                        )
                    {
                        //نسبت اشتباه انتخاب شده

                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;


                    }


                }
                else
                {
                    //طرف مقابل خانم است
                    if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.برادر
                       || familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر

                       )
                    {
                        //نسبت اشتباه انتخاب شده

                        res.Messages = "عدم تطابق جنسیت:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }

                }




                #endregion  
                var today = DateTime.Today;
                int myage = today.Year - myProfile.BirthDate.Value.Year;
                int citizenage = today.Year - familyProfile.BirthDate.Year;

                #region قوانین سنی
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر
                   || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                   )
                {
                    //اگر نسبت پدر یا مادر بود سن من باید حداقل ده سال از سن پدر یا مادرم کمتر باشد 


                    if (myage + 10 > citizenage)
                    {
                        res.Messages = "عدم تطابق سنی:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    //سن طرف مقابل برای تاهل بودن

                    if (citizenage < 15)
                    {
                        res.Messages = "عدم تطابق سنی برای تاهل:اطلاعات وارد شده منطبق نیست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                }
                else if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.فرزند)
                {

                    //سن من باید بزرگتر از سن رابطه باشد 
                    if ((citizenage + 10) > myage)
                    {
                        res.Messages = "عدم تطابق سنی :سن فرزند باید کوچکتر از سن شما باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }
                    //باید سن شخص بزرگتر از 15 سال باشد
                    if (myage < 15)
                    {
                        res.Messages = "عدم تطابق سنی :اطلاعات وارد شده منطبق نيست";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }



                }
                #endregion
                #region قوانین تاهل
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.همسر)
                {
                    if (myProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {

                        res.Messages = "وضعیت تاهل شما مجرد می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;

                    }
                    if (familyProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.Messages = "وضعیت تاهل همسر شما مجرد انتخاب شده است";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.فرزند)
                {
                    if (myProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {

                        res.Messages = "وضعیت تاهل شما مجرد می باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }
                if (familyProfile.FamilyRelation == FamilyRelationshipsEnum.پدر
                   || familyProfile.FamilyRelation == FamilyRelationshipsEnum.مادر

                    )
                {
                    if (familyProfile.MariageStatus == MaritalStatusEnum.مجرد)
                    {
                        res.Messages = "وضعیت تاهل نباید مجرد  باشد";
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                }

                #endregion


                if (await _users.AnyAsync(w => w.Username == familyProfile.NationCode))
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی با کد ملی وارد شده قبلا ثبت نام کرده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res; 
                } 
                var citizenRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Citizen);
                var passwordHash = _securityService.GetSha256Hash(familyProfile.NationCode);
                var user = new User
                {
                    DisplayName = familyProfile.NationCode,
                    Username = familyProfile.NationCode,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = familyProfile.Mobile,
                    EmailAddress = familyProfile.EMail,
                    EmailVerification = false,
                    MobileNumberVerification = true,
                    PasswordQuestion ="",
                    PasswordAnswer = "", 
                    RegisterByUserId= myId
                };

                var add = await _users.AddAsync(user);
                if (citizenRole != null)
                {
                    var role = new UserRole() { RoleId = citizenRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }


                var citizen = new Citizen()
                {
                    BirthDate = familyProfile.BirthDate, 
                    JobGroupId = familyProfile.JobGroup,
                    FatherName = familyProfile.FatherName,
                    FirstName = familyProfile.FirstName,
                    CreationDate = DateTime.Now,
                    Gender = familyProfile.Gender,
                    JobTitle = familyProfile.JobTitle,
                    LastName = familyProfile.LastName,
                    Mobile = familyProfile.Mobile,
                    MariageStatus = familyProfile.MariageStatus,
                   // NationalityId = 0 ,//model.Nationality,
                    NationCode = familyProfile.NationCode, 
                    EducationField = familyProfile.EducationTitle,
                    EducationGroupId = familyProfile.EducationGroup, 
                    User = user,
                    RegisterByServiceId=0 ,//ثبت نام خانواده از طربق پروفایل شهروندی
                    HasFamily=true,
                    
                };
                myProfile.HasFamily = true;
                _citizen.Update(myProfile);
                await _citizen.AddAsync(citizen);
                var address = new Address()
                {
                    AddressType = AddressTypeEnum.منزل,
                    Citizen = citizen,
                    CityId = familyProfile.CityId,
                    CreationDate = DateTime.Now,
                    FullAddress = familyProfile.FullAddress,
                    IsActive = true,
                    IsVerified = false,
                    PostalCode = familyProfile.PostalCode,
                    Region = familyProfile.Region,
                    LasteUpdateOnDate = DateTime.Now,
                    Phone= familyProfile.PhoneNumber
                };
                await _address.AddAsync(address);
                //add family Relation
                var addfamily = new CitizenFamily()
                {
                    CitizenId = myId,
                    FamilyCitizen = citizen,
                    CreationDate = DateTime.Now,
                    FamilyRelation = familyProfile.FamilyRelation.Value,
                };

                await _family.AddAsync(addfamily);


                await _uow.SaveChangesAsync();
                res.IsSuccess = true;
                res.Messages = "عضو جدید خانواده با موفقیت اضافه گردید.";
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


        public async Task<ApiResult> UpdateFamilyMemberByCitizen(UpdateFamilyCitizenDto model, int myId)
        {


            var res = new ApiResult(false, ApiResultStatusCode.BadRequest, "");
            //چک کن ببین شهروندی با این مشخات وجود دارد
            try
            {
                 
               if(model.FamilyCitizenId==null )
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه عضو خانواده برای ویرایش مشخص نشده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (model.FamilyCitizenId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه عضو خانواده برای ویرایش مشخص نشده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                if (! await _family.AnyAsync(w=>w.CitizenId== myId  && w.FamilyCitizenId==model.FamilyCitizenId))
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان ویرایش این عضو برای شما امکان پذیر نیست";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                var citizen = await _citizen.FirstOrDefaultAsync(w=>w.CitizenId==model.FamilyCitizenId);

                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                //ویرایش اطلاعات خانواده

                citizen.BirthDate = model.BirthDate;
                citizen.Gender = model.Gender;
                citizen.FirstName = model.FirstName;
                citizen.LastName = model.LastName;
                citizen.FatherName = model.FatherName;
                citizen.BirthDate = model.BirthDate;
                citizen.MariageStatus = model.MariageStatus;
                citizen.EducationStatues = model.EducationStatues;
                citizen.EducationGroupId = model.EducationGroup;
                citizen.JobTitle = model.JobTitle;
                citizen.JobGroupId = model.JobGroup;
                citizen.EducationField = model.EducationTitle;

               

                _citizen.Update(citizen);

                citizen.Mobile = model.Mobile;  
                _citizen.Update(citizen);

             


                //ویرایش اطلاعات ادرس شهروند
                var citizenAddress = await _address
                   .Where(w => w.CitizenId == model.FamilyCitizenId && w.AddressType == AddressTypeEnum.منزل && w.IsActive).FirstOrDefaultAsync();
                if (citizenAddress != null)
                {
                    citizenAddress.FullAddress = model.FullAddress;
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
                    //add address
                    var address = new Address()
                    {
                        AddressType = AddressTypeEnum.منزل,
                        Alley = model.Alley,
                        Plaque = model.Plaque,
                        Street = model.Street,
                        LasteUpdateOnDate = DateTime.Now,
                        CitizenId = model.FamilyCitizenId.Value,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = model.FullAddress,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        Phone = model.PhoneNumber,
                        IsActive = true
                    };

                    await _address.AddAsync(address);


                }

                await _uow.SaveChangesAsync();
                res.IsSuccess = true;
                res.Messages = " عملیات ویرایش با موفقیت صورت گرفت";
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




        public async Task<ApiResult> Remove(DeleteFamilly model )
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "حذف نسبت خانوادگی با موفقیت انجام گردید.");
            try
            {

                if(string.IsNullOrWhiteSpace(model.Description))
                {
                    res.IsSuccess = false;
                    res.Messages = "دلیل حذف را وارد نمایید";
                    return res;
                }
                     
                if (model.FamillyId==0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه عضو خانواده را مشخص کنید";
                    return res;
                }

                var code = Guid.Empty;
                Guid.TryParse(model.UserCode, out code); 



                var item = await _family.FirstOrDefaultAsync(w => w.Citizen.UserCode == code && w.FamilyCitizenId == model.FamillyId);
                if (item == null)
                { 
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res; 
                }
                item.ReasonFordeleting = model.Description;
                item.IsDeleted = true; 
                _family.Update(item);
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



        public async Task<ApiResult> RemoveByCitizen(int id,int citizenId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "حذف نسبت خانوادگی با موفقیت انجام گردید.");
            try
            {
                 
                var item = await _family.FirstOrDefaultAsync(w => w.CitizenId == citizenId && w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }
                item.ReasonFordeleting = "حذف توسط خود شهروند";
                item.IsDeleted = true; 
                _family.Update(item);
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













        public async Task<ApiResult> AcceptFamilyByCitizen(ReviewFamily model,int myId)
        {
            var res = new ApiResult(false, ApiResultStatusCode.BadRequest, "");
            try
            {
                var citizen = await _family.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId &&
                w.FamilyCitizenId == myId);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                citizen.AcceptRelative = model.IsAccept;
                if (model.IsAccept)
                {
                    citizen.AcceptDate = DateTime.Now;
                }

                _family.Update(citizen);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
            }


            return res;
        }


        public async Task<ApiResult> ConfirmFamilyByAdmin(ConfirmFamily model,int ConfirmBy)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت انجام گردید");
            try
            {

                var familyCode = Guid.Empty;
                var citizenCode = Guid.Empty;


                Guid.TryParse(model.UserCode ,out citizenCode);
                Guid.TryParse(model.FamilyUserCode, out familyCode);



                var citizen = await    _family.FirstOrDefaultAsync(w => w.Citizen.UserCode == citizenCode && w.FamilyCitizen.UserCode == familyCode);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                citizen.Confirm = model.IsAccept;
                citizen.ConfirmDate = DateTime.Now;
                citizen.ConfirmerUserId = ConfirmBy;
                _family.Update(citizen);
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
            }


            return res;
        }




        public async Task<ApiResult<PagedFamilyCitizenViewModel>> SearchFamilyCitizens(
        int pageNumber, int pageSize,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string name = null,
        string nationCode = null,
        FamilyRelationshipsEnum? familyRelation=null,
        int? groupId = null 
        )
        {

            var res = new ApiResult<PagedFamilyCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedFamilyCitizenViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _family.Where(w=>w.IsDeleted!=true);

                if (!string.IsNullOrEmpty(nationCode))
                {
                    nationCode = nationCode.Fa2En();
                    query = query.Where(w =>  w.Citizen.NationCode == nationCode || w.FamilyCitizen.NationCode== nationCode);
                }


                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.Citizen.FirstName, "%" + name + "%")
                    ||
                    EF.Functions.Like(w.Citizen.LastName, "%" + name + "%")

                    );
                }


                if (groupId != null)
                {
                    var citizenIds = await _citizenGroups.Where(w => w.GroupId == groupId).Select(s => s.CitizenId).ToListAsync();
                    query = query.Where(w => citizenIds.Contains(w.CitizenId));
                }


                if (FromDate != null)
                {
                    query = query.Where(w => w.Citizen.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.Citizen.CreationDate <= ToDate);
                }
               
                if (familyRelation != null)
                {
                    query = query.Where(w => w.FamilyRelation == familyRelation);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Citizens = await query.Select(s => new ShortFamilyCitizenInfo()
                {
                    NationCode = s.Citizen.NationCode,
                    Gender = s.Citizen.Gender,
                    BirthDate = s.Citizen.BirthDate,
                    CitizenId = s.CitizenId,
                    CreationDate = s.CreationDate,
                    FatherName = s.Citizen.FatherName,
                    FirstName = s.Citizen.FirstName,
                    LastName = s.Citizen.LastName,
                    
                    FamilyFirstName = s.FamilyCitizen.FirstName,
                    FamilyLastName = s.FamilyCitizen.LastName,
                    FamilyGender = s.FamilyCitizen.Gender,
                    FamilyBirthDate = s.FamilyCitizen.BirthDate,
                    FamilyNationCode = s.FamilyCitizen.NationCode,
                    FamilyRelation = s.FamilyRelation,
                    FamilyCitizenId=s.FamilyCitizen.CitizenId,


                    Mobile = s.Citizen.Mobile,
                    //  Nationality = s.NationalityId,
                  
                    PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                    RegisterByService = s.Citizen.RegisterByService.ServiceName,
                    SabtStatus = s.Citizen.SabtStatus,
                    UserCode=s.Citizen.UserCode,
                    FamilyUserCode=s.FamilyCitizen.UserCode,
                    


                }).AsNoTracking().OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();


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
