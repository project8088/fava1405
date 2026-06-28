using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.BaseEntity
{
    public interface ISmsInfoService
    {
        Task<ApiResult> Add(SmsInfo model);

        Task<ApiResult> Add(
          string text,
          string mobile,
          string Sender = "",
          int? userId = null,
          int? companyId = null);

        Task<ApiResult> Add(
          string text,
          string mobile,
          string token1,
          string Sender = "",
          int? userId = null,
          int? companyId = null);

        Task<ApiResult> Add(
          string text,
          string mobile,
          string token1,
          string templeteName = "",
          string Sender = "",
          int? userId = null,
          int? companyId = null);

        Task<ApiResult> Add(SendSMSResult model, int? userId);

        Task<ApiResult> AddRange(List<SmsInfo> list);

        Task<int> CountManzalatSms(string mobileNumber);

        Task<int> CountSms(string mobileNumber, TempleteNameEnum templeteName);

        Task<ApiResult<PagedSmsInfoViewModel>> GetCitizenPagedSmsListAsync(
          string userCode,
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null);

        Task<ApiResult<List<SmsInfoDto>>> GetCitizenRejectImage(int citizenId);

        Task<ApiResult<SmsInfoDto>> GetLastForgotCodeByMobileNumber(
          string mobileNumber);

        Task<ApiResult<SmsInfoDto>> GetLastMobileValidationByMobileNumber(
          string mobileNumber);

        Task<ApiResult<PagedSmsInfoViewModel>> GetPagedSmsListAsync(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string mobileNumber = null,
          string smsText = null,
          string companyName = null,
          int? contractCode = null,
          bool? isArchive = false);

        Task<ApiResult<SmsInfoDto>> GetSmsInfo(int id);

        Task<ApiResult> TransToArchive();




    }

    public class SmsInfoService : ISmsInfoService
    {

        private readonly IUnitOfWork _uow;
        private readonly DbSet<SmsInfo> _sms;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<ArchiveSmsInfo> _archive;




        public SmsInfoService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _sms = _uow.Set<SmsInfo>();
            this._archive = this._uow.Set<ArchiveSmsInfo>();
            _citizen = _uow.Set<Citizen>();
        }
        public async Task<ApiResult> Add(SmsInfo model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                model.SendOnDate = DateTime.Now;
                await _sms.AddAsync(model);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }




        public async Task<ApiResult> Add(string text, string mobile, string Sender = "", int? userId = null, int? companyId = null)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                var add = new SmsInfo()
                {
                    SendOnDate = DateTime.Now,
                    MessageText = text,
                    Mobiles = mobile,
                    UserId = userId,
                    Sender = Sender


                };

                await _sms.AddAsync(add);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }

        public async Task<ApiResult> Add(SendSMSResult model, int? userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                var add = new SmsInfo()
                {
                    SendOnDate = DateTime.Now,
                    MessageText = model.Message,
                    Mobiles = model.Receptor,
                    UserId = userId,
                    Sender = model.Sender,
                    Cost = model.Cost,
                    Date = model.Date,
                    Token1 = model.Token1,
                    Token2 = model.Token2,
                    Token10 = model.Token10,
                    Token20 = model.Token20,
                    MessageId = model.Messageid,
                    Token3 = model.Token3,
                    StatusText = model.StatusText,
                    TempleteName = model.TempleteName,

                };

                await _sms.AddAsync(add);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی در عملیات ذخیره پیامک رخ داده است";

            }



            return res;
        }




        public async Task<ApiResult> Add(string text,
            string mobile, string token1, string Sender = "", int? userId = null, int? companyId = null)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                var add = new SmsInfo()
                {
                    SendOnDate = DateTime.Now,
                    MessageText = text,
                    Mobiles = mobile,
                    UserId = userId,
                    Sender = Sender,
                    Token1 = token1,



                };

                await _sms.AddAsync(add);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }


        public async Task<ApiResult> Add(string text, string mobile,
            string token1, string templeteName = "", string Sender = "", int? userId = null
            , int? companyId = null)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                var add = new SmsInfo()
                {
                    SendOnDate = DateTime.Now,
                    MessageText = text,
                    Mobiles = mobile,
                    UserId = userId,
                    Sender = Sender,
                    Token1 = token1,
                    TempleteName = templeteName,
                };

                await _sms.AddAsync(add);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }


        public async Task<ApiResult> AddRange(List<SmsInfo> list)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                _sms.AddRange(list);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }

            return res;
        }

        public async Task<ApiResult> TransToArchive()
        {
            ApiResult res = new ApiResult(true, ApiResultStatusCode.Success, "    با موفقیت ثبت شد");
            try
            {
                DateTime date = DateTime.Now.AddDays(-1.0);
                List<SmsInfo> list = await this._sms.Where(w => w.SendOnDate < date).Take(3000).ToListAsync();
                foreach (SmsInfo smsInfo in list)
                {

                    ArchiveSmsInfo archiveSmsInfo = new ArchiveSmsInfo();
                    archiveSmsInfo.Id = smsInfo.Id;
                    archiveSmsInfo.Cost = smsInfo.Cost;
                    archiveSmsInfo.Sender = smsInfo.Sender;
                    archiveSmsInfo.UserId = smsInfo.UserId;
                    archiveSmsInfo.StatusText = smsInfo.StatusText;
                    archiveSmsInfo.Token1 = smsInfo.Token1;
                    archiveSmsInfo.SmsStatus = smsInfo.SmsStatus;
                    archiveSmsInfo.Mobiles = smsInfo.Mobiles;
                    archiveSmsInfo.MessageText = smsInfo.MessageText;
                    archiveSmsInfo.Date = smsInfo.Date;
                    archiveSmsInfo.MessageId = smsInfo.MessageId;
                    archiveSmsInfo.SendOnDate = smsInfo.SendOnDate;
                    archiveSmsInfo.Token10 = smsInfo.Token10;
                    archiveSmsInfo.TempleteName = smsInfo.TempleteName;
                    archiveSmsInfo.Token2 = smsInfo.Token2;
                    archiveSmsInfo.Token20 = smsInfo.Token20;
                    archiveSmsInfo.Token3 = smsInfo.Token3;
                    archiveSmsInfo.GroupListId = smsInfo.GroupListId;
                    await _archive.AddAsync(archiveSmsInfo);
                }
                if (list.Any())
                {
                    this._sms.RemoveRange(list);
                    int num = await this._uow.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
            }
            return res;
        }

        public async Task<int> CountSms(string mobileNumber, TempleteNameEnum templeteName)
        {
            int res = -1;
            try
            {
                string name = templeteName.ToString();
                return await this._sms.CountAsync(c => c.Mobiles == mobileNumber && c.TempleteName == name);
            }
            catch (Exception ex)
            {
                return res;
            }
        }


        public async Task<ApiResult<SmsInfoDto>> GetSmsInfo(int id)
        {

            var res = new ApiResult<SmsInfoDto>(true, ApiResultStatusCode.Success, new SmsInfoDto());

            try
            {


                var item = await _sms.Where(w => w.Id == id).Select(sms => new SmsInfoDto()
                {
                    Id = sms.Id,
                    Cost = sms.Cost,
                    Sender = sms.Sender,
                    UserId = sms.UserId,
                    StatusText = sms.StatusText,
                    Token1 = sms.Token1,
                    SmsStatus = sms.SmsStatus,
                    Mobiles = sms.Mobiles,
                    MessageText = sms.MessageText,
                    Date = sms.Date,
                    MessageId = sms.MessageId,
                    SendOnDate = sms.SendOnDate,
                    Token10 = sms.Token10,
                    TempleteName = sms.TempleteName,
                    UserName = sms.User == null ? "" : sms.User.Username,
                    Token2 = sms.Token2,
                    Token20 = sms.Token20,
                    Token3 = sms.Token3,
                    GroupListId = sms.GroupListId,



                }).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "  یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                res.Data = item;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }

        public async Task<ApiResult<SmsInfoDto>> GetLastMobileValidationByMobileNumber(string mobileNumber)
        {

            var res = new ApiResult<SmsInfoDto>(true, ApiResultStatusCode.Success, new SmsInfoDto());

            try
            {
                var sms = await _sms.Where(w =>
                w.Mobiles == mobileNumber &&
                w.TempleteName == TempleteNameEnum.MobileVerify.ToString()).ToListAsync();
                if (sms == null || !sms.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = " پیامکی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var lastSms = sms.LastOrDefault();

                var data = new SmsInfoDto()
                {
                    Id = lastSms.Id,
                    Cost = lastSms.Cost,
                    Sender = lastSms.Sender,
                    UserId = lastSms.UserId,
                    StatusText = lastSms.StatusText,
                    Token1 = lastSms.Token1,
                    SmsStatus = lastSms.SmsStatus,
                    Mobiles = lastSms.Mobiles,
                    MessageText = lastSms.MessageText,
                    Date = lastSms.Date,
                    MessageId = lastSms.MessageId,
                    SendOnDate = lastSms.SendOnDate,
                    Token10 = lastSms.Token10,
                    TempleteName = lastSms.TempleteName,
                    UserName = lastSms.User == null ? "" : lastSms.User.Username,
                    Token2 = lastSms.Token2,
                    Token20 = lastSms.Token20,
                    Token3 = lastSms.Token3,
                    GroupListId = lastSms.GroupListId,


                };

                res.Data = data;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }
        public async Task<ApiResult<SmsInfoDto>> GetLastForgotCodeByMobileNumber(string mobileNumber)
        {

            var res = new ApiResult<SmsInfoDto>(true, ApiResultStatusCode.Success, new SmsInfoDto());

            try
            {
                var sms = await _sms.Where(w =>
                w.Mobiles == mobileNumber &&
                w.TempleteName == TempleteNameEnum.ForgotPassword.ToString()).ToListAsync();
                if (sms == null || !sms.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = " پیامکی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var lastSms = sms.LastOrDefault();

                var data = new SmsInfoDto()
                {
                    Id = lastSms.Id,
                    Cost = lastSms.Cost,
                    Sender = lastSms.Sender,
                    UserId = lastSms.UserId,
                    StatusText = lastSms.StatusText,
                    Token1 = lastSms.Token1,
                    SmsStatus = lastSms.SmsStatus,
                    Mobiles = lastSms.Mobiles,
                    MessageText = lastSms.MessageText,
                    Date = lastSms.Date,
                    MessageId = lastSms.MessageId,
                    SendOnDate = lastSms.SendOnDate,
                    Token10 = lastSms.Token10,
                    TempleteName = lastSms.TempleteName,
                    UserName = lastSms.User == null ? "" : lastSms.User.Username,
                    Token2 = lastSms.Token2,
                    Token20 = lastSms.Token20,
                    Token3 = lastSms.Token3,
                    GroupListId = lastSms.GroupListId,


                };

                res.Data = data;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }




        public async Task<ApiResult<List<SmsInfoDto>>> GetCitizenRejectImage(int citizenId)
        {

            var res = new ApiResult<List<SmsInfoDto>>(true, ApiResultStatusCode.Success, new List<SmsInfoDto>());
            if (citizenId == 0)
            {
                res.IsSuccess = false;
                res.Messages = " شناسه شهروندی را مشخص کنید ";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }
            try
            {
                var smsList = await _sms.Where(w =>
                w.UserId == citizenId &&
                w.TempleteName == TempleteNameEnum.ReviewPicture.ToString()).Select(s => new SmsInfoDto()
                {
                    Id = s.Id,
                    Cost = s.Cost,
                    Sender = s.Sender,
                    UserId = s.UserId,
                    StatusText = s.StatusText,
                    Token1 = s.Token1,
                    SmsStatus = s.SmsStatus,
                    Mobiles = s.Mobiles,
                    MessageText = s.MessageText,
                    Date = s.Date,
                    MessageId = s.MessageId,
                    SendOnDate = s.SendOnDate,
                    Token10 = s.Token10,
                    TempleteName = s.TempleteName,
                    UserName = s.User == null ? "" : s.User.Username,
                    Token2 = s.Token2,
                    Token20 = s.Token20,
                    Token3 = s.Token3,
                    GroupListId = s.GroupListId,


                }).ToListAsync();

                res.Data = smsList;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }




        public async Task<ApiResult<PagedSmsInfoViewModel>> GetPagedSmsListAsync(
            int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string mobileNumber = null,
             string smsText = null
            , string companyName = null,
            int? contractCode = null,
             bool? isArchive = false

            )
        {
            var offset = (pageNumber) * pageSize;
            var res = new ApiResult<PagedSmsInfoViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {



                if (isArchive == true)
                {

                    var query = _archive.AsQueryable();
                    if (!string.IsNullOrEmpty(mobileNumber) && mobileNumber != "null")
                    {
                        query = query.Where(w => EF.Functions.Like(w.Mobiles, "%" + mobileNumber + "%"));

                    }

                    if (!string.IsNullOrEmpty(smsText) && smsText != "null")
                    {
                        query = query.Where(w => EF.Functions.Like(w.MessageText, "%" + smsText + "%"));

                    }

                    if (FromDate != null)
                    {
                        query = query.Where(w => w.SendOnDate >= FromDate);
                    }
                    if (ToDate != null)
                    {
                        query = query.Where(w => w.SendOnDate <= ToDate);
                    }



                    var list = await query.Select(sms => new SmsInfoDto()
                    {
                        Id = sms.Id,
                        Cost = sms.Cost,
                        Sender = sms.Sender,
                        UserId = sms.UserId,
                        StatusText = sms.StatusText,
                        Token1 = sms.Token1,
                        SmsStatus = sms.SmsStatus,
                        Mobiles = sms.Mobiles,
                        MessageText = sms.MessageText,
                        Date = sms.Date,
                        MessageId = sms.MessageId,
                        SendOnDate = sms.SendOnDate,
                        Token10 = sms.Token10,
                        TempleteName = sms.TempleteName,
                        UserName = sms.User == null ? "" : sms.User.Username,
                        Token2 = sms.Token2,
                        Token20 = sms.Token20,
                        Token3 = sms.Token3,
                        GroupListId = sms.GroupListId,
                    }).OrderByDescending(o => o.Id).Skip(offset).Take(pageSize).ToListAsync();

                    res.Data = new PagedSmsInfoViewModel
                    {
                        TotalItems = await query.CountAsync(),
                        SmsList = list
                    };

                }
                else
                {
                    var query = _sms.AsQueryable();



                    if (!string.IsNullOrEmpty(mobileNumber) && mobileNumber != "null")
                    {
                        query = query.Where(w => EF.Functions.Like(w.Mobiles, "%" + mobileNumber + "%"));

                    }

                    if (!string.IsNullOrEmpty(smsText) && smsText != "null")
                    {
                        query = query.Where(w => EF.Functions.Like(w.MessageText, "%" + smsText + "%"));

                    }

                    if (FromDate != null)
                    {
                        query = query.Where(w => w.SendOnDate >= FromDate);
                    }
                    if (ToDate != null)
                    {
                        query = query.Where(w => w.SendOnDate <= ToDate);
                    }



                    var list = await query.Select(sms => new SmsInfoDto()
                    {
                        Id = sms.Id,
                        Cost = sms.Cost,
                        Sender = sms.Sender,
                        UserId = sms.UserId,
                        StatusText = sms.StatusText,
                        Token1 = sms.Token1,
                        SmsStatus = sms.SmsStatus,
                        Mobiles = sms.Mobiles,
                        MessageText = sms.MessageText,
                        Date = sms.Date,
                        MessageId = sms.MessageId,
                        SendOnDate = sms.SendOnDate,
                        Token10 = sms.Token10,
                        TempleteName = sms.TempleteName,
                        UserName = sms.User == null ? "" : sms.User.Username,
                        Token2 = sms.Token2,
                        Token20 = sms.Token20,
                        Token3 = sms.Token3,
                        GroupListId = sms.GroupListId,
                    }).OrderByDescending(o => o.Id).Skip(offset).Take(pageSize).ToListAsync();

                    res.Data = new PagedSmsInfoViewModel
                    {
                        TotalItems = await query.CountAsync(),
                        SmsList = list
                    };
                }




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }



        public async Task<ApiResult<PagedSmsInfoViewModel>> GetCitizenPagedSmsListAsync(
           string userCode, int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null
            )
        {
            var offset = (pageNumber) * pageSize;
            var res = new ApiResult<PagedSmsInfoViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {

                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);


                var ciitzen = await _citizen.FirstOrDefaultAsync(w => w.User.UserCode == guid);
                if (ciitzen == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var mobile = ciitzen.Mobile;
                var nationCode = ciitzen.NationCode;

                var query = _sms.AsNoTracking().Where(w =>
               (w.UserId == ciitzen.CitizenId)
                ||
                (w.Mobiles == mobile)

                );


                if (FromDate != null)
                {
                    query = query.Where(w => w.SendOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.SendOnDate <= ToDate);
                }


                var list = await query.Select(sms => new SmsInfoDto()
                {
                    Id = sms.Id,
                    Cost = sms.Cost,
                    Sender = sms.Sender,
                    UserId = sms.UserId,
                    StatusText = sms.StatusText,
                    Token1 = sms.Token1,
                    SmsStatus = sms.SmsStatus,
                    Mobiles = sms.Mobiles,
                    MessageText = sms.MessageText,
                    Date = sms.Date,
                    MessageId = sms.MessageId,
                    SendOnDate = sms.SendOnDate,
                    Token10 = sms.Token10,
                    TempleteName = sms.TempleteName,
                    UserName = sms.User == null ? "" : sms.User.Username,
                    Token2 = sms.Token2,
                    Token20 = sms.Token20,
                    Token3 = sms.Token3,
                    GroupListId = sms.GroupListId,
                }).OrderByDescending(o => o.Id).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedSmsInfoViewModel
                {
                    TotalItems = await query.CountAsync(),
                    SmsList = list
                };


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }






        public async Task<int> CountManzalatSms(string mobileNumber)
        {
            int res = -1;
            try
            {
                string name = TempleteNameEnum.ManzelatHasCardAndConvertCard.ToString();
                string name2 = TempleteNameEnum.ManzelatNoCard.ToString();
                string name3 = TempleteNameEnum.ManzelatReview.ToString();
                return await this._sms.CountAsync(c => c.Mobiles == mobileNumber && (c.TempleteName == name || c.TempleteName == name2 || c.TempleteName == name3));
            }
            catch (Exception ex)
            {
                return res;
            }
        }


 


    }

}
