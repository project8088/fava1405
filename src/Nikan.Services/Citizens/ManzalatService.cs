using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.Citizens
{
    public interface IManzalatService
    { 
        Task<ApiResult> AddFile(ManzalatDocumentInfoViewModel model, int manzalatId); 
        Task<ApiResult> AddOrUpdateManzalat(ManzalatDto model, int citizenId); 
        Task<ApiResult> ConfigManzalatForm(); 
        Task<ApiResult<ConfirmFormManzalatResult>> ConfirmManzaltByAdmin( ConfirmFormManzalat model, int ConfirmBy); 
        Task<ApiResult<AvailableManzaltFormsAndAddress>> GetAllAvailableManzaltForm( int citizenId); 
        Task<ApiResult<CitizenReviewManzalatForm>> GetCitizenInfoAndManzaltForm(  string userCode); 
        Task<ApiResult<ManzalatDto>> GetCitizenManzalat(
          int citizenId,
          int formBaseId);

        Task<ApiResult<CitizenReviewManzalatFormItem>> GetCitizenRegisterManzalatForm(
          int citizenId,
          int formId);

        Task<ApiResult<ManzalatBaseFormDto>> GetManzalatBaseForm(int id);

        Task<ApiResult<List<ManzalatBaseFormDto>>> GetManzalatBaseForms();

        Task<ApiResult<string>> Remove(int Id);

        Task<ApiResult> RemoveManzalatUploadImage(int citizenId, int formId);

        Task<ApiResult<PagedManzalatCitizenViewModel>> SearchManzaltCitizens(
          int pageNumber,
          int pageSize,
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
          ManzalatFormTypeEnum? manzalatFormType = null ,bool? inManzalatGroups = null,int ? groupId = null
            , bool? hasCard = null 
            );

        Task<ApiResult> UpdateManzalatBaseForm(ManzalatBaseFormDto model);
        Task<ApiResult> UploadFileForCitizenManzalatForm(int citizenId, int formId);
    }

    public class ManzalatService : IManzalatService
    {
        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<Manzalat> _manzalat;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<User> _users;
        private readonly DbSet<GroupsCitizens> _citizenGroups;
        private readonly DbSet<Address> _address;
        private readonly DbSet<CitizensCard> _citizensCard;
        private readonly DbSet<ManzalatDocumentInfo> _manzalatDocumentInfo;
        private readonly DbSet<ManzalatBaseForm> _manzalatForm;

        public ManzalatService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _manzalat = _uow.Set<Manzalat>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _address = _uow.Set<Address>();
            _citizensCard = _uow.Set<CitizensCard>();
            _manzalatDocumentInfo = _uow.Set<ManzalatDocumentInfo>();
            _manzalatForm = _uow.Set<ManzalatBaseForm>();


        }

        #endregion


        #region بارگذاری مدارک

        public async Task<ApiResult> AddFile(ManzalatDocumentInfoViewModel model,int manzalatId)
        {
            var res = new ApiResult (true, ApiResultStatusCode.Success, "ثبت مدرک با موفقیت صورت گرفت");

            try
            {

                var info = new ManzalatDocumentInfo()
                {

                    Extension = model.Extension,
                    FileName = model.FileName,
                    Size = model.Size,
                   // OwnerId = model.OwnerId,
                    ThumnailPath = "",
                    AttachedOnDate = DateTime.Now,
                    ManzalatId = manzalatId,
                    FilePath = model.FilePath,
                    Title = model.Title,
                    DocumentStatus = ManzalatFormStatuseEnum.در_حال_بررسی,
                    Description = "",
                };
                await _manzalatDocumentInfo.AddAsync(info);
                await _uow.SaveChangesAsync();
               
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
            }

            return res;

        }


        #endregion 








        public async Task<ApiResult> ConfigManzalatForm()
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {
                var add = false;

                if(! await _manzalatForm.AnyAsync(a=>a.FormId == (int)ManzalatFormTypeEnum.جانبازان))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = null,
                        FormId =  (int)ManzalatFormTypeEnum.جانبازان,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه جانبازان"

                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.معلولین))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = null,
                        FormId = (int)ManzalatFormTypeEnum.معلولین,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه معلولین"

                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = false,
                        FormId = (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه زنان سرپرست خانواده"

                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.بازنشسته))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = null,
                        FormId = (int)ManzalatFormTypeEnum.بازنشسته,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه بازنشستگان",
                        MinAge=60
                        
                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.سالمند))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = null,
                        FormId = (int)ManzalatFormTypeEnum.سالمند,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه سالمندان",
                        MinAge=65

                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.بیماران_خاص))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = null,
                        FormId = (int)ManzalatFormTypeEnum.بیماران_خاص,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه بیماران خاص",
                        

                    });

                }
                if (!await _manzalatForm.AnyAsync(a => a.FormId == (int)ManzalatFormTypeEnum.مادران_دارای_سه_فرزند))
                {
                    add = true;
                    await _manzalatForm.AddAsync(new ManzalatBaseForm()
                    {
                        Description = "",
                        Gender = false,
                        FormId = (int)ManzalatFormTypeEnum.مادران_دارای_سه_فرزند,
                        IsActive = true,
                        Title = "عضویت در طرح منزلت ویژه مادران دارای سه فرزند",



                    });

                }




                if (add)
                {
                    await _uow.SaveChangesAsync();
                }



            }
            catch (Exception er)
            {
                res.Messages = "  خطایی در اجرای عملیات رخ داده است";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;

            }



            return res;


        }

        public async Task<ApiResult> UpdateManzalatBaseForm (ManzalatBaseFormDto model)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "ویرایش فرم با موفقیت انجام گردید");
            try
            {

                if(model.MaxAge!=null && model.MinAge!=null)
                {
                    if(model.MaxAge < model.MinAge)
                    {

                        res.Messages = "  حداقل سن باید کوچکتر از حداکثر سن باشد";
                        res.IsSuccess = false;
                        return res;
                    }
                }



                var form = await _manzalatForm.FirstOrDefaultAsync(w => w.FormId == model.Id);
                if (form != null)
                {
                    form.Description = model.Description;
                    form.IsActive = model.IsActive;
                    form.UploadDescription = model.UploadDescription;
                    form.OrderIndex = model.OrderIndex;
                    form.MaxAge = model.MaxAge;
                    form.MinAge = model.MinAge;
                    form.Title = model.Title;
                    form.Gender = model.Gender;
                    _manzalatForm.Update(form);
                    await _uow.SaveChangesAsync();
                }
                else
                {

                    res.Messages = "   فرم یافت نشد ";
                    res.IsSuccess = false;
                    return res;

                }


            }
            catch (Exception er)
            {
                res.Messages = "   خطایی در اجرای عملیات رخ داده است ";
                res.IsSuccess = false;
                return res;

            }


            return res;

        }

        public async Task<ApiResult<ManzalatBaseFormDto>> GetManzalatBaseForm (int id)
        {

            var res = new ApiResult<ManzalatBaseFormDto>(true, ApiResultStatusCode.Success,new ManzalatBaseFormDto(), " ");
            try
            {
                var form = await _manzalatForm.FirstOrDefaultAsync(w => w.FormId == id);
                if (form != null)
                {
                    res.Data.Description = form.Description;
                    res.Data.IsActive = form.IsActive;
                    res.Data.MaxAge = form.MaxAge;
                    res.Data.MinAge = form.MinAge;
                    res.Data.Title = form.Title;
                    res.Data.Id = form.FormId;
                    res.Data.Gender = form.Gender;
                    res.Data.OrderIndex = form.OrderIndex;
                    res.Data.UploadDescription = form.UploadDescription;

                }
                else
                {

                    res.Messages = "   فرم یافت نشد ";
                    res.IsSuccess = false;
                    return res;

                }


            }
            catch (Exception er)
            {
                res.Messages = "   خطایی در اجرای عملیات رخ داده است ";
                res.IsSuccess = false;
                return res;

            }


            return res;

        }

        public async Task<ApiResult<List<ManzalatBaseFormDto>>> GetManzalatBaseForms()
        {

            var res = new ApiResult<List<ManzalatBaseFormDto>>(true, ApiResultStatusCode.Success, new List<ManzalatBaseFormDto>(), " ");
            try
            {
                res.Data = await _manzalatForm.Select(s => new ManzalatBaseFormDto()
                {
                    Description=s.Description,
                    Gender=s.Gender,
                    Id=s.FormId,
                    IsActive=s.IsActive,
                    MaxAge=s.MaxAge,
                    MinAge=s.MinAge,
                    Title=s.Title,
                    OrderIndex=s.OrderIndex,
                    UploadDescription=s.UploadDescription
                    

                }).OrderBy(o=>o.OrderIndex) .ToListAsync();
                
            }
            catch (Exception er)
            {
                res.Messages = "   خطایی در اجرای عملیات رخ داده است ";
                res.IsSuccess = false;
                return res;

            }


            return res;

        }


       
         

        public async Task<ApiResult> AddOrUpdateManzalat (ManzalatDto model,int citizenId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {
                var registerForm = await _manzalat.Include(i=> i.ManzalatBaseForm).FirstOrDefaultAsync(w => w.CitizenId == citizenId && w.FormStatuse == ManzalatFormStatuseEnum.در_حال_بررسی);
                if (registerForm != null)
                {
                 
                    res.Messages = $"  امکان ثبت درخواست جدید وجود ندارد . درخواست {registerForm.ManzalatBaseForm.Title} شما در دست بررسی می باشد ";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                
                var manzalat = await _manzalat.Where(w => w.CitizenId == citizenId && w.ManzalatBaseFormId==model.ManzalatBaseFormId).FirstOrDefaultAsync();
                if (manzalat == null)
                {

                    var add = new Manzalat()
                    {
                        FormStatuse = ManzalatFormStatuseEnum.در_حال_بررسی,
                        CreationDate = DateTime.Now,
                        CitizenId = citizenId,
                        ManzalatBaseFormId = model.ManzalatBaseFormId, //در چه فرمی ثبت نام کرده است
                    };

                    if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.بازنشسته)
                    {
                        

                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.بیماران_خاص)
                    {
                      
                        add.Typ_SpecialDiseases = model.Typ_SpecialDiseases;
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.جانبازان)
                    {
                        

                        add.Chk_Janbazan_AsabRavan = model.Chk_Janbazan_AsabRavan;
                        add.Chk_Janbazan_Binaei = model.Chk_Janbazan_Binaei;
                        add.Chk_Janbazan_JesmiHarekati_NoWheelChair = model.Chk_Janbazan_JesmiHarekati_NoWheelChair;
                        add.Typ_Janbazan_JesmiHarekati_NoWheelChair = model.Typ_Janbazan_JesmiHarekati_NoWheelChair;
                        add.Chk_Janbazan_JesmiHarekati_WheelChair = model.Chk_Janbazan_JesmiHarekati_WheelChair;
                        add.Typ_Janbazan_JesmiHarekati_WheelChair = model.Typ_Janbazan_JesmiHarekati_WheelChair;
                        add.Chk_Janbazan_Zehni = model.Chk_Janbazan_Zehni;
                        add.Typ_Janbazan_Zehni = model.Typ_Janbazan_Zehni;
                        add.Typ_Janbazan_AsabRavan = model.Typ_Janbazan_AsabRavan;
                        add.Typ_Janbazan_Binaei = model.Typ_Janbazan_Binaei;
                        add.Chk_Janbazan_Shenavaei = model.Chk_Janbazan_Shenavaei;
                        add.Typ_Janbazan_Shenavaei = model.Typ_Janbazan_Shenavaei;
                        add.Chk_Janbazan_Sayer = model.Chk_Janbazan_Sayer;
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده)
                    {
                       
                        add.Typ_ZananSarparast = model.Typ_ZananSarparast;
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده)
                    {
                        if(model.Typ_ZananSarparast==null)
                        {
                            res.Messages = "  نوع سرپرستی را انتخاب کنید  ";
                            res.IsSuccess = false;
                            res.StatusCode = ApiResultStatusCode.BadRequest;
                            return res;
                        }

                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.سالمند)
                    {
                        
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.مادران_دارای_سه_فرزند)
                    {
                       
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.معلولین)
                    {
                       
                        add.Chk_Maloulin_JesmiHarekati_NoWheelChair = model.Chk_Maloulin_JesmiHarekati_NoWheelChair;
                        add.Typ_Maloulin_JesmiHarekati_NoWheelChair = model.Typ_Maloulin_JesmiHarekati_NoWheelChair;
                        add.Chk_Maloulin_JesmiHarekati_WheelChair = model.Chk_Maloulin_JesmiHarekati_WheelChair;
                        add.Typ_Maloulin_JesmiHarekati_WheelChair = model.Typ_Maloulin_JesmiHarekati_WheelChair;
                        add.Chk_Maloulin_Zehni = model.Chk_Maloulin_Zehni;
                        add.Typ_Maloulin_Zehni = model.Typ_Maloulin_Zehni;
                        add.Chk_Maloulin_AsabRavan = model.Chk_Maloulin_AsabRavan;
                        add.Typ_Maloulin_AsabRavan = model.Typ_Maloulin_AsabRavan;
                        add.Chk_Maloulin_Binaei = model.Chk_Maloulin_Binaei;
                        add.Typ_Maloulin_Binaei = model.Typ_Maloulin_Binaei;
                        add.Chk_Maloulin_Shenavaei = model.Chk_Maloulin_Shenavaei;
                        add.Typ_Maloulin_Shenavaei = model.Typ_Maloulin_Shenavaei;
                        add.Chk_Maloulin_Sayer = model.Chk_Maloulin_Sayer;
                    }


                     
                    await _manzalat.AddAsync(add);
                    await _uow.SaveChangesAsync();
                }
                else if(manzalat.FormStatuse==ManzalatFormStatuseEnum.تایید )
                {
                    res.Messages = "  درخواست عضویت شما در طرح منزلت   تایید شده است . امکان ویرایش وجود ندارد";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                else {


                   if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.جانبازان)
                    { 
                        manzalat.Chk_Janbazan_AsabRavan = model.Chk_Janbazan_AsabRavan;
                        manzalat.Chk_Janbazan_Binaei = model.Chk_Janbazan_Binaei;
                        manzalat.Chk_Janbazan_JesmiHarekati_NoWheelChair = model.Chk_Janbazan_JesmiHarekati_NoWheelChair;
                        manzalat.Typ_Janbazan_JesmiHarekati_NoWheelChair = model.Typ_Janbazan_JesmiHarekati_NoWheelChair;
                        manzalat.Chk_Janbazan_JesmiHarekati_WheelChair = model.Chk_Janbazan_JesmiHarekati_WheelChair;
                        manzalat.Typ_Janbazan_JesmiHarekati_WheelChair = model.Typ_Janbazan_JesmiHarekati_WheelChair;
                        manzalat.Chk_Janbazan_Zehni = model.Chk_Janbazan_Zehni;
                        manzalat.Typ_Janbazan_Zehni = model.Typ_Janbazan_Zehni;
                        manzalat.Typ_Janbazan_AsabRavan = model.Typ_Janbazan_AsabRavan;
                        manzalat.Typ_Janbazan_Binaei = model.Typ_Janbazan_Binaei;
                        manzalat.Chk_Janbazan_Shenavaei = model.Chk_Janbazan_Shenavaei;
                        manzalat.Typ_Janbazan_Shenavaei = model.Typ_Janbazan_Shenavaei;
                        manzalat.Chk_Janbazan_Sayer = model.Chk_Janbazan_Sayer;
                    }
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده)
                    {
                        if (model.Typ_ZananSarparast == null)
                        {
                            res.Messages = "  نوع سرپرستی را انتخاب کنید  ";
                            res.IsSuccess = false;
                            res.StatusCode = ApiResultStatusCode.BadRequest;
                            return res;
                        }

                        manzalat.Typ_ZananSarparast = model.Typ_ZananSarparast;
                    }
                     
                    else if (model.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.معلولین)
                    {
                      
                        manzalat.Chk_Maloulin_JesmiHarekati_NoWheelChair = model.Chk_Maloulin_JesmiHarekati_NoWheelChair;
                        manzalat.Typ_Maloulin_JesmiHarekati_NoWheelChair = model.Typ_Maloulin_JesmiHarekati_NoWheelChair;
                        manzalat.Chk_Maloulin_JesmiHarekati_WheelChair = model.Chk_Maloulin_JesmiHarekati_WheelChair;
                        manzalat.Typ_Maloulin_JesmiHarekati_WheelChair = model.Typ_Maloulin_JesmiHarekati_WheelChair;
                        manzalat.Chk_Maloulin_Zehni = model.Chk_Maloulin_Zehni;
                        manzalat.Typ_Maloulin_Zehni = model.Typ_Maloulin_Zehni;
                        manzalat.Chk_Maloulin_AsabRavan = model.Chk_Maloulin_AsabRavan;
                        manzalat.Typ_Maloulin_AsabRavan = model.Typ_Maloulin_AsabRavan;
                        manzalat.Chk_Maloulin_Binaei = model.Chk_Maloulin_Binaei;
                        manzalat.Typ_Maloulin_Binaei = model.Typ_Maloulin_Binaei;
                        manzalat.Chk_Maloulin_Shenavaei = model.Chk_Maloulin_Shenavaei;
                        manzalat.Typ_Maloulin_Shenavaei = model.Typ_Maloulin_Shenavaei;
                        manzalat.Chk_Maloulin_Sayer = model.Chk_Maloulin_Sayer;
                    }


                    

                    manzalat.LastUpdate = DateTime.Now;
                    if (manzalat.FormStatuse == ManzalatFormStatuseEnum.عدم_تایید)
                        manzalat.FormStatuse = ManzalatFormStatuseEnum.در_حال_بررسی;
                    _manzalat.Update(manzalat);
                    await _uow.SaveChangesAsync(); 
                }

            }
            catch (Exception er)
            {

                res.Messages = "  خطایی رخ داده است ";
                res.IsSuccess = false;
                return res;
            }

            return res;
        }
         
        

      
        public async Task<ApiResult<ManzalatDto>> GetCitizenManzalat (int citizenId,int formBaseId)
        {
            var res = new ApiResult<ManzalatDto>(true, ApiResultStatusCode.Success, new ManzalatDto());
            try
            {
                var data = await _manzalat.Where(w => w.CitizenId == citizenId && w.ManzalatBaseFormId== formBaseId).Select(s => new ManzalatDto()
                {

                    Id = s.Id,

                    Chk_Maloulin_AsabRavan =s.Chk_Maloulin_AsabRavan,
                    Chk_Maloulin_Binaei=s.Chk_Maloulin_Binaei,
                    Chk_Maloulin_JesmiHarekati_NoWheelChair=s.Chk_Maloulin_JesmiHarekati_NoWheelChair,
                    Chk_Maloulin_JesmiHarekati_WheelChair=s.Chk_Maloulin_JesmiHarekati_WheelChair,
                    Chk_Maloulin_Sayer=s.Chk_Maloulin_Sayer,
                    Chk_Maloulin_Shenavaei=s.Chk_Maloulin_Shenavaei,
                    Chk_Maloulin_Zehni=s.Chk_Maloulin_Zehni,
                   
                    Typ_Maloulin_AsabRavan=s.Typ_Maloulin_AsabRavan,
                   
                    Typ_Maloulin_Binaei=s.Typ_Maloulin_Binaei,
                    Typ_Maloulin_JesmiHarekati_NoWheelChair=s.Typ_Maloulin_JesmiHarekati_NoWheelChair,
                    Typ_Maloulin_JesmiHarekati_WheelChair=s.Typ_Maloulin_JesmiHarekati_WheelChair,
                    Typ_Maloulin_Shenavaei=s.Typ_Maloulin_Shenavaei,
                    Typ_Maloulin_Zehni=s.Typ_Maloulin_Zehni ,
                    Chk_Janbazan_AsabRavan = s.Chk_Janbazan_AsabRavan,
                    Chk_Janbazan_Binaei = s.Chk_Janbazan_Binaei,
                    Chk_Janbazan_JesmiHarekati_NoWheelChair = s.Chk_Janbazan_JesmiHarekati_NoWheelChair,
                    Chk_Janbazan_JesmiHarekati_WheelChair = s.Chk_Janbazan_JesmiHarekati_WheelChair,
                    Chk_Janbazan_Sayer = s.Chk_Janbazan_Sayer,
                    Chk_Janbazan_Shenavaei = s.Chk_Janbazan_Shenavaei,
                    Chk_Janbazan_Zehni = s.Chk_Janbazan_Zehni,
                    Typ_Janbazan_AsabRavan = s.Typ_Janbazan_AsabRavan,
                    Typ_Janbazan_Binaei = s.Typ_Janbazan_Binaei,
                    Typ_Janbazan_JesmiHarekati_NoWheelChair = s.Typ_Janbazan_JesmiHarekati_NoWheelChair,
                    Typ_Janbazan_JesmiHarekati_WheelChair = s.Typ_Janbazan_JesmiHarekati_WheelChair,
                    Typ_Janbazan_Shenavaei = s.Typ_Janbazan_Shenavaei,
                    Typ_Janbazan_Zehni = s.Typ_Janbazan_Zehni,
                    Typ_ZananSarparast = s.Typ_ZananSarparast,
                    Typ_SpecialDiseases = s.Typ_SpecialDiseases



                }).FirstOrDefaultAsync(); 
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

  
      
      

    


        public async Task<ApiResult<string>> Remove(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شد.");
            try
            {

                var item = await _manzalat.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                if (item.FormStatuse == Common.GlobalEnum.ManzalatFormStatuseEnum.تایید)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان حذف وجود ندارد- درخواست تایید شده است";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                _manzalat.Remove(item);
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



     public async Task<ApiResult<AvailableManzaltFormsAndAddress>> GetAllAvailableManzaltForm(int citizenId )
        {
            var res = new ApiResult<AvailableManzaltFormsAndAddress>(true, ApiResultStatusCode.Success, new AvailableManzaltFormsAndAddress());
            try
            {


                var list = new List<AvailableManzaltForms>();
                var listRegister = await _manzalat.Include(i=>i.ManzalatDocumentInfo).Where(w => w.CitizenId == citizenId).ToListAsync();

                if (listRegister.Any())
                {
                    if(listRegister.Any(w=>w.FormStatuse==ManzalatFormStatuseEnum.تایید))
                    {
                        res.Data.HasRegister = true;
                        res.Data.FormStatuse = ManzalatFormStatuseEnum.تایید;
                    }
                    else if (listRegister.Any(w =>w.ManzalatDocumentInfo.Any() && w.FormStatuse == ManzalatFormStatuseEnum.در_حال_بررسی))
                    {
                        res.Data.HasRegister = true;
                        res.Data.FormStatuse = ManzalatFormStatuseEnum.در_حال_بررسی;
                    }
                      
                }
                else
                {
                    res.Data.HasRegister = false;
                }



                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if(citizen==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var address = await _address.FirstOrDefaultAsync(w =>w.IsDeleted!=true && w.CitizenId == citizenId);
                if(address==null)
                {
                    res.IsSuccess = true;
                    res.Messages = "اطلاعات آدرس را تکمیل نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Data.HasAddress = false;
                    return res;
                }
                else
                {
                    res.Data.HasAddress = true;

                }











                //در صورتیکه بالای 60 سال بود امکان بازنشستگی وجود دارد
                var birthDate = citizen.BirthDate;
                if (birthDate == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "از قسمت اطلاعات پایه تاریخ تولد خود را مشخص نمایید";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                var age = (DateTime.Now.Year) - birthDate.Value.Date.Year;
                var gender = citizen.Gender;
             
                var listForms=  await _manzalatForm.Where(w => w.IsActive).OrderBy(o => o.OrderIndex).ToListAsync();
                foreach (var f in listForms)
                {
                    if (f.Gender != null && f.Gender != gender)
                        continue; // این فرم نمی تونه ثبت نام کنه 
                    if(f.MinAge !=null && age < f.MinAge)
                        continue;

                    if (f.MaxAge != null && age > f.MaxAge)
                        continue;

                    var item = new AvailableManzaltForms()
                    { 
                        Description = f.Description, 
                        Title = f.Title,
                        Id = f.FormId,
                        ManzalatFormType = (ManzalatFormTypeEnum)f.FormId

                    };
                    if(listRegister.Any())
                    {
                        var registerItem = listRegister.Where(w => w.ManzalatBaseFormId == f.FormId).FirstOrDefault();
                        if(registerItem!=null)
                        {
                            item.CitizenIsRegisterThisForm = true;
                            item.FormResult = registerItem.FormStatuse;
                        }

                    }

                    list.Add(item);


                }

  

                res.Data.Forms = list;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

      public async Task<ApiResult<CitizenReviewManzalatFormItem>> GetCitizenRegisterManzalatForm(int citizenId,int formId)
        {
            var res = new ApiResult<CitizenReviewManzalatFormItem>(true, ApiResultStatusCode.Success, new CitizenReviewManzalatFormItem());
            try
            {
               

                var year = DateTime.Today.Year;
                var citizen = await _manzalat.Where(w =>  w.CitizenId== citizenId && w.ManzalatBaseFormId== formId).Select(s => new ManzalatShortCitizenInfo()
                { 

                    CitizenId = s.CitizenId, 
                    UserCode = s.Citizen.UserCode,
                    CreationDate = s.CreationDate,
                    FatherName = s.Citizen.FatherName,
                    FirstName = s.Citizen.FirstName,
                    Gender = s.Citizen.Gender,
                    LastName = s.Citizen.LastName,
                    Mobile = s.Citizen.Mobile,
                    NationCode = s.Citizen.NationCode,
                    PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                    PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                    BirthDate = s.Citizen.BirthDate,
                    PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed != null,
                    RegisterByService = s.Citizen.RegisterByService.ServiceName,
                    SabtStatus = s.Citizen.SabtStatus,
                    Nationality = 0,
                    Age = year - s.Citizen.BirthDate.Value.Year,
                    MariageStatus = s.Citizen.MariageStatus == null ? "" : s.Citizen.MariageStatus.ToString(),
                    FormStatuse = s.FormStatuse,
                    CkeckOperation = s.CkeckOperation == null ? "" : s.CkeckOperation.DisplayName,
                    CkeckOperationId = s.CkeckOperationId,
                    FormTitle = s.ManzalatBaseForm == null ? "" : s.ManzalatBaseForm.Title,


                }).FirstOrDefaultAsync();

                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                var manzalatForm = new CitizenManzaltForms();
                var itemform = await _manzalat.Include(i => i.ManzalatBaseForm).Where(w => w.CitizenId == citizenId &&
                w.ManzalatBaseFormId == formId).FirstOrDefaultAsync();
                if (itemform != null)
                {
                    manzalatForm.ManzalatRegisterId = itemform.Id;
                    manzalatForm.CitizenIsRegisterThisForm = true;
                    manzalatForm.Title = itemform.ManzalatBaseForm.Title;  
                    manzalatForm.ManzalatFormType = (ManzalatFormTypeEnum)itemform.ManzalatBaseFormId.Value;
                     
                    var uploadFile = await _manzalatDocumentInfo.Where(w =>w.IsDeleted!=true && w.ManzalatId == itemform.Id).Select(s => new ManzalatDocumentInfoViewModel()
                    {

                        DocumentStatus = s.DocumentStatus,
                        DocumentGroupDescription = s.DocumentGroupDescription,
                        Description = s.Description,
                        FileName = s.FileName,
                        Id = s.Id,
                        AttachedOnDate = s.AttachedOnDate,
                        Extension = s.Extension,
                        FilePath = s.FilePath,
                        ManzalatId = s.ManzalatId,
                        //OwnerId = s.OwnerId,
                        Size = s.Size,
                        Title = s.Title

                    }).FirstOrDefaultAsync(); 

                    if(uploadFile!=null)
                    {
                        res.Data.HasFiles = true;
                    }



                    res.Data.UpoladFiles = uploadFile;


                }


               


                res.Data.Citizen = citizen;
                res.Data.CitizenId = citizen.CitizenId;
                res.Data.UserCode = citizen.UserCode;
                res.Data.ManzaltForm  = manzalatForm;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult> UploadFileForCitizenManzalatForm(int citizenId, int formId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"");
            try
            {


                
                var manzalat = await _manzalat.FirstOrDefaultAsync(w => w.CitizenId == citizenId && w.ManzalatBaseFormId == formId);
                if (manzalat == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                if (manzalat.FormStatuse == ManzalatFormStatuseEnum.عدم_تایید)
                {
                    manzalat.FormStatuse = ManzalatFormStatuseEnum.در_حال_بررسی;
                    _manzalat.Update(manzalat);
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










        public async Task<ApiResult<CitizenReviewManzalatForm>> GetCitizenInfoAndManzaltForm(string  userCode)
        {
            var res = new ApiResult<CitizenReviewManzalatForm>(true, ApiResultStatusCode.Success, new CitizenReviewManzalatForm());
            try
            {
                var code = Guid.Empty;
                Guid.TryParse(userCode, out code);


                var year = DateTime.Today.Year; 
                var citizen = await _manzalat.Where(w =>
                w.Citizen.UserCode == code).Select(s => new ManzalatShortCitizenInfo()
               {


                   CitizenId = s.CitizenId,
                   UserCode=s.Citizen.UserCode,
                   CreationDate = s.CreationDate,
                   FatherName = s.Citizen.FatherName,
                   FirstName = s.Citizen.FirstName,
                   Gender = s.Citizen.Gender,
                   LastName = s.Citizen.LastName,
                   Mobile = s.Citizen.Mobile,
                   NationCode = s.Citizen.NationCode,
                   PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                   PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "/assets/images/avatar.png" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                   BirthDate = s.Citizen.BirthDate,
                   PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed != null,
                   RegisterByService = s.Citizen.RegisterByService.ServiceName,
                   SabtStatus = s.Citizen.SabtStatus,
                   Nationality = 0,
                   Age = year - s.Citizen.BirthDate.Value.Year,
                   MariageStatus = s.Citizen.MariageStatus==null ? "":s.Citizen.MariageStatus.ToString(),
                   FormStatuse=s.FormStatuse,
                   CkeckOperation=s.CkeckOperation==null ? "" :s.CkeckOperation.DisplayName,
                   CkeckOperationId=s.CkeckOperationId


                }).FirstOrDefaultAsync();

                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                citizen.CitizenGroups = await _citizenGroups.Where(w => w.CitizenId == citizen.CitizenId).Select(s => s.Group.GroupName).ToListAsync();

                var addres = await _address.Where(w => w.CitizenId == citizen.CitizenId
                  && w.IsActive && w.AddressType == AddressTypeEnum.منزل

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
                    Phone=s.Phone,
                    

                }).FirstOrDefaultAsync();
                if(addres!=null)
                {
                    var city = "";
                    if(addres.City!=null)
                    {
                        city= addres.City.ParentText+"-" + addres.City.Text;
                    }
                    citizen.FullAddress = city + "-" + addres.Street + "-" + addres.Alley + "-" + addres.Plaque + "-" + addres.PostalCode;
                }

                var list = new List<CitizenManzaltForms>();
                var registerforms = await _manzalat.Include(i=>i.ManzalatBaseForm).Where(w => w.CitizenId == citizen.CitizenId).ToListAsync();
                if (registerforms.Any())
                {
                    var  ids = registerforms.Select(s => s.Id).ToList();
                    var listAsync = await this._manzalatDocumentInfo.Where(w => w.IsDeleted != true 
                    && ids.Contains(w.ManzalatId)).ToListAsync();


                    foreach (var  manzalat in registerforms)
                    {
                       
                        CitizenManzaltForms citizenManzaltForms = new CitizenManzaltForms()
                        {
                            CitizenIsRegisterThisForm = true,
                            ManzalatRegisterId = manzalat.Id,
                            ManzalatFormType = (ManzalatFormTypeEnum)manzalat.ManzalatBaseFormId.Value,
                            Title = manzalat.ManzalatBaseForm.Title
                        };
                        if (manzalat.FormStatuse == ManzalatFormStatuseEnum.تایید)
                            citizenManzaltForms.FormResult = new bool?(true);
                        else if (manzalat.FormStatuse == ManzalatFormStatuseEnum.عدم_تایید)
                            citizenManzaltForms.FormResult = new bool?(false);
                        if (listAsync.Any())
                        {
                            var manzalatDocumentInfo = listAsync.FirstOrDefault(w => w.ManzalatId == manzalat.Id);
                            if (manzalatDocumentInfo != null)
                                citizenManzaltForms.FileUrl = manzalatDocumentInfo.FilePath;
                        }
                        list.Add(citizenManzaltForms);
                    }
                }
                

                res.Data.Citizen = citizen;
                res.Data.CitizenId = citizen.CitizenId;
                res.Data.UserCode = citizen.UserCode;
                res.Data.ManzaltForms = list;



                
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است"+er.Message;
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }



        public async Task<ApiResult<ConfirmFormManzalatResult>> ConfirmManzaltByAdmin(ConfirmFormManzalat model, int ConfirmBy)
        {
            var res = new ApiResult<ConfirmFormManzalatResult>(true, ApiResultStatusCode.Success, new ConfirmFormManzalatResult(), "عملیات با موفقیت صورت گرفت");
           
            try
            {
                var code = Guid.Empty;
                Guid.TryParse(model.UserCode, out code);
                

                int baseFormId = (int)model.ManzalatFormType;

                var manzalat = await _manzalat.Include(i=>i.Citizen).FirstOrDefaultAsync(w => 
                w.ManzalatBaseFormId == (int?)baseFormId && w.Citizen.UserCode == code);

                if (manzalat == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " فرم منزلت یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                 

                res.Data.Name = manzalat.Citizen.FirstName + " " + manzalat.Citizen.LastName;
                res.Data.MobileNumber = manzalat.Citizen.Mobile;
                res.Data.FormResult =true;

                if (await _citizensCard.AnyAsync(w => w.CitizenId == manzalat.CitizenId && (int)w.RequestStatuse > 0))
                {
                    res.Data.HasCard = true;
                }


                manzalat.DenyDescription = model.FormResultDescription;
                 

                if (model.FormResult == true)
                {
                    manzalat.FormStatuse = ManzalatFormStatuseEnum.تایید;
                    res.Data.FormResult = true;

                } 
                else if (model.FormResult == false)
                {
                    manzalat.FormStatuse = ManzalatFormStatuseEnum.عدم_تایید;
                    res.Data.FormResult = false;
                }
                 
                manzalat.CkeckOperationId = ConfirmBy; 

                _manzalat.Update(manzalat);
                var addGroup = false;
                

                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(option => option.Key == "ManzalatGroupId");
                if (settings != null && manzalat.FormStatuse!=ManzalatFormStatuseEnum.در_حال_بررسی)
                {
                    var groupId = int.Parse(settings.Value);
                    if (manzalat.FormStatuse == ManzalatFormStatuseEnum.تایید)
                      addGroup = true; 
                    else if (await _manzalat.AnyAsync(w => w.CitizenId == manzalat.CitizenId && w.FormStatuse == ManzalatFormStatuseEnum.تایید))
                      addGroup = true; 
                    else if (await _citizenGroups.AnyAsync(w => w.GroupId == groupId && w.CitizenId == manzalat.CitizenId))
                    {
                        //اگر شهروندی فرمی تایید نشده براش برو منزلت حذف کن
                        // در صورت عدم تایید شهروند را ازگروه منزلت حذف کن
                        var item = await _citizenGroups.FirstOrDefaultAsync(w => w.GroupId == groupId && w.CitizenId == manzalat.CitizenId);
                        if (item != null)
                        {
                            item.IsDeleted = true;
                            item.DeletedByUserId = ConfirmBy;
                            item.DeletedDate = DateTime.Now;
                            _citizenGroups.Update(item);

                        }
                    }

                    if(addGroup)
                    {
                        //در صورت تایید شهروند را به گروه منزلت اضافه کن
                        if (!await _citizenGroups.AnyAsync(w => w.GroupId == groupId && w.CitizenId == manzalat.CitizenId))
                        {
                            await _citizenGroups.AddAsync(new GroupsCitizens()
                            {
                                AddByUserId = ConfirmBy,
                                CitizenId = manzalat.CitizenId,
                                CreationDate = DateTime.Now,
                                GroupId = groupId,
                            });
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
            }


            return res;
        }
    
        public async Task<ApiResult<PagedManzalatCitizenViewModel>> SearchManzaltCitizens(
     int pageNumber, int pageSize,
     DateTime? FromDate = null,
     DateTime? ToDate = null,
     DateTime? birthDateFromDate = null,
     DateTime? birthDateToDate = null,
     bool? gender = null,
     SabtStatusEnum? sabtStatus = null, 
    MaritalStatusEnum? mariageStatus = null, 
    string name = null,
     string nationCode = null,
     ManzalatFormStatuseEnum? formStatuse=null,
    ManzalatFormTypeEnum? manzalatFormType = null,
    bool? inManzalatGroups=null,
     int? groupId = null,bool ? hasCard = null
     )
        {


            var res = new ApiResult<PagedManzalatCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedManzalatCitizenViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _manzalat.AsQueryable();
                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.Citizen.NationCode, "%" + nationCode + "%"));
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


                if (formStatuse != null)
                {
                    query = query.Where(w => w.FormStatuse == formStatuse);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreationDate <= ToDate);
                }
                if (formStatuse != null)
                {
                    query = query.Where(w => w.FormStatuse == formStatuse);
                }
                if (gender != null)
                {
                    query = query.Where(w => w.Citizen.Gender == gender);
                }
                if (sabtStatus != null)
                {
                    query = query.Where(w => w.Citizen.SabtStatus == sabtStatus);
                }
                if (mariageStatus != null)
                {
                    query = query.Where(w => w.Citizen.MariageStatus == mariageStatus);
                }

                if (birthDateFromDate != null)
                {
                    query = query.Where(w => w.Citizen.BirthDate >= birthDateFromDate);
                }
                if (hasCard != null)
                {
                    query = query.Where(w => w.Citizen.HasCard == hasCard.Value);
                }



                if (birthDateToDate != null)
                {
                    query = query.Where(w => w.Citizen.BirthDate <= birthDateToDate);
                }

                

                if (manzalatFormType != null)
                {
                    query = query.Where(w => w.ManzalatBaseFormId ==(int) manzalatFormType);
                }

                var manzalatgroupId = 0;
                var year = DateTime.Now.Year;
              
                var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(option => option.Key == "ManzalatGroupId");
                manzalatgroupId = int.Parse(settings.Value);


                if (inManzalatGroups != null)
                {
                   
                    if(inManzalatGroups==true)
                       query = query.Where(w => w.Citizen.GroupsCitizens.Any(w=>w.GroupId== manzalatgroupId));
                    else
                        query = query.Where(w =>! w.Citizen.GroupsCitizens.Any(w => w.GroupId == manzalatgroupId));
                }
                  
                 



                    res.Data.TotalItems = await query.CountAsync();
                res.Data.Citizens = await query.Select(s => new ManzalatShortCitizenInfo()
                {


                    DocumentUploaded = s.ManzalatDocumentInfo.Any(),
                    BirthDate = s.Citizen.BirthDate,
                    CitizenId = s.CitizenId,
                    UserCode = s.Citizen.UserCode,
                    CreationDate = s.CreationDate,
                    CitizenCreationDate = s.Citizen.CreationDate,
                    NationCode = s.Citizen.NationCode,
                    Gender = s.Citizen.Gender,
                    FirstName = s.Citizen.FirstName,
                    LastName = s.Citizen.LastName,
                    FatherName = s.Citizen.FatherName,
                    MariageStatus = s.Citizen.MariageStatus==null ?"": s.Citizen.MariageStatus.ToString(),
                    //Mobile = s.Citizen.Mobile, 

                    PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                    RegisterByService = s.Citizen.RegisterByService.ServiceName,
                    SabtStatus = s.Citizen.SabtStatus,
                    FormStatuse=s.FormStatuse,
                    InManzalatGroups=s.Citizen.GroupsCitizens.Any(g=>g.IsDeleted!=true && g.GroupId== manzalatgroupId),
                    Age = s.Citizen.BirthDate == null ? 0 : year - s.Citizen.BirthDate.Value.Year, 
                    FormTitle=s.ManzalatBaseForm==null ? "":s.ManzalatBaseForm.Title ,
                    FullAddress =s.Citizen.Address.FirstOrDefault(a => a.IsDeleted != true && a.IsActive).FullAddress,
                    PostalCode = s.Citizen.Address.FirstOrDefault(a => a.IsDeleted != true && a.IsActive).PostalCode,
                    Typ_ZananSarparast=s.Typ_ZananSarparast==null ? "" : s.Typ_ZananSarparast.ToString(),
                    Typ_SpecialDiseases = s.Typ_SpecialDiseases == null ? "" : s.Typ_SpecialDiseases.ToString(),
                    Typ_Janbazan_Binaei = s.Typ_Janbazan_Binaei == null ? "" : s.Typ_Janbazan_Binaei.ToString(),


                    Typ_Janbazan_JesmiHarekati_WheelChair = s.Typ_Janbazan_JesmiHarekati_WheelChair == null ? "" : s.Typ_Janbazan_JesmiHarekati_WheelChair.ToString(),
                    Typ_Janbazan_AsabRavan = s.Typ_Janbazan_AsabRavan == null ? "" : s.Typ_Janbazan_AsabRavan.ToString(),
                    Typ_Janbazan_Zehni = s.Typ_Janbazan_Zehni == null ? "" : s.Typ_Janbazan_Zehni.ToString(),


                    Typ_Maloulin_JesmiHarekati_NoWheelChair = s.Typ_Maloulin_JesmiHarekati_NoWheelChair == null ? "" : s.Typ_Maloulin_JesmiHarekati_NoWheelChair.ToString(),
                    Typ_Maloulin_JesmiHarekati_WheelChair = s.Typ_Maloulin_JesmiHarekati_WheelChair == null ? "" : s.Typ_Maloulin_JesmiHarekati_WheelChair.ToString(),
                    Typ_Maloulin_Zehni = s.Typ_Maloulin_Zehni == null ? "" : s.Typ_Maloulin_Zehni.ToString(),
                    Typ_Maloulin_AsabRavan = s.Typ_Maloulin_AsabRavan == null ? "" : s.Typ_Maloulin_AsabRavan.ToString(),
                    Typ_Maloulin_Binaei = s.Typ_Maloulin_Binaei == null ? "" : s.Typ_Maloulin_Binaei.ToString(),
                    Typ_Maloulin_Shenavaei = s.Typ_Maloulin_Shenavaei == null ? "" : s.Typ_Maloulin_Shenavaei.ToString(),
                    Typ_Janbazan_JesmiHarekati_NoWheelChair = s.Typ_Janbazan_JesmiHarekati_NoWheelChair == null ? "" : s.Typ_Janbazan_JesmiHarekati_NoWheelChair.ToString(),
                    Typ_Janbazan_Shenavaei = s.Typ_Janbazan_Shenavaei == null ? "" : s.Typ_Janbazan_Shenavaei.ToString(),
                   

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






        public async Task<ApiResult> RemoveManzalatUploadImage(int citizenId, int formId)
        {
            ApiResult res = new ApiResult(true, ApiResultStatusCode.Success, "حذف مدرک با موفقیت انجام گردید");
            try
            {
                CitizenManzaltForms citizenManzaltForms = new CitizenManzaltForms();
                var  itemform = await  this._manzalat.FirstOrDefaultAsync(w => w.CitizenId == citizenId && 
                w.ManzalatBaseFormId ==  formId );
                if (itemform != null)
                {
                    if (itemform.FormStatuse == ManzalatFormStatuseEnum.تایید)
                    {
                        res.IsSuccess = false;
                        res.Messages = "عضویت شما در طرح منزلت تایید شده است و امکان حذف وجود ندارد";
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        return res;
                    }
                    var  manzalatDocumentInfo = await  this._manzalatDocumentInfo
                        .FirstOrDefaultAsync(w => w.IsDeleted != true && w.ManzalatId == itemform.Id );
                    if (manzalatDocumentInfo != null)
                    {
                        manzalatDocumentInfo.IsDeleted = true;
                        this._manzalatDocumentInfo.Update(manzalatDocumentInfo);
                        int num = await this._uow.SaveChangesAsync();
                        return res;
                    }
                }
                res.IsSuccess = false;
                res.Messages = "مدرکی یافت نشد";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }













    }
}