using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.ExportCitizen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.ExportCitizen
{
    public interface IExportCitizenService
    {
        Task<ApiResult> AddCitizenTodayExportList(int citizenId);

        Task<ApiResult<ExportCitizensInfo>> ExportCitizenForSabtAhval(
          ExportedCitizenForSabtAhvalDto model,
          int userId);

        Task<ApiResult<PagedExportedCitizenViewModel>> GetAllCitizenExported(
          int exportId,
          int pageNumber,
          int pageSize);

        Task<ApiResult<PagedExportCitizenViewModel>> GetAllExportSabtAhval(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          int? exportNumber = null);

        Task<ApiResult<ShortCitizenInfo>> GetCitizenForAuthentication();

        Task<ApiResult<List<ExportedCitizensInfo>>> GetOnlineAuthentication(
          int exportId);

        Task<ApiResult<List<ExportedCitizensInfo>>> GetOnlineAuthenticationChekStateLife(
          int exportId);

        Task<ApiResult<string>> Remove(int Id);

        Task<ApiResult<string>> Send(int Id, int userId);
    }

    public class ExportCitizenService : IExportCitizenService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ExportCitizens> _export;
        private readonly DbSet<ExportedCitizens> _exportCitizen;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<GroupsCitizens> _groupcitizen;



        public ExportCitizenService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _export = _uow.Set<ExportCitizens>();
            _exportCitizen = _uow.Set<ExportedCitizens>();
            _citizen = _uow.Set<Citizen>();
            _groupcitizen = _uow.Set<GroupsCitizens>();
        }




        public async Task<ApiResult<PagedExportCitizenViewModel>> GetAllExportSabtAhval(
            int pageNumber, int pageSize,
            DateTime? FromDate = null,
             DateTime? ToDate = null,
             int? exportNumber = null)
        {
            var res = new ApiResult<PagedExportCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedExportCitizenViewModel());
            try
            {
                var offset = (pageNumber) * pageSize;
                var query = _export.Where(w => w.IsDeleted != true);


                if (FromDate != null)
                {
                    query = query.Where(w => w.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreationDate <= ToDate);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new ExportCitizensInfo()
                {
                    CountRow = s.ExportedCitizens.Count,
                    CreationDate = s.CreationDate,
                    ExportFileName = s.ExportFileName,
                    AcceptCount = s.AcceptCount,
                    ExportBy = s.ExportBy.DisplayName,
                    ExportById = s.ExportById,
                    ExportByUserCode = s.ExportBy == null ? Guid.Empty : s.ExportBy.UserCode,
                    ReceiveBy = s.ReceiveBy.DisplayName,
                    ReceiveById = s.ReceiveById,
                    ReceiveByUserCode = s.ReceiveBy == null ? Guid.Empty : s.ReceiveBy.UserCode,
                    
                    ExportedForeignId = s.ExportedForeignId,
                    Id = s.Id,
                    ReceiveOnDate = s.ReceiveOnDate,
                    SendOnDate = s.SendOnDate,
                    IsConfirmed = s.SendOnDate != null,
                    ExportNumber = s.ExportNumber,
                    GroupName = s.Group == null ? "" : s.Group.GroupName,
                    ExportType = s.ExportType,


                }).OrderByDescending(w => w.Id).Skip(offset).Take(pageSize).ToListAsync();



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
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _export.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                if (item.SendOnDate != null)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان حذف این خروجی وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                item.IsDeleted = true;
                _export.Update(item);
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


        public async Task<ApiResult<ExportCitizensInfo>> ExportCitizenForSabtAhval(ExportedCitizenForSabtAhvalDto model,int userId)
        {
            var res = new ApiResult<ExportCitizensInfo>(true, ApiResultStatusCode.Success,new ExportCitizensInfo(), "با موفقیت خروجی صورت گرفت");
            try
            {
 
                
                    var strFromDate = model.FromDate.ToShortDateString() + " 00:00:00";
                    var fromDate = DateTime.Parse(strFromDate);
                
                    var strFromDate2 = model.ToDate.ToShortDateString() + " 23:59:59";
                    var toDate = DateTime.Parse(strFromDate2);

                var listCitizenId = new List<int>();

                if (model.ExportType == Common.GlobalEnum.ExportCitizenTypeEnum.Sabt)
                {
                    if (model.GroupId != null)
                    {
                        var citizenGroupIds = await _groupcitizen.Where(w => w.GroupId == model.GroupId).Select(s => s.CitizenId).ToListAsync();


                        listCitizenId = await _citizen.Where(w => citizenGroupIds.Contains(w.CitizenId) && w.NationId == 0 && w.CreationDate >= fromDate && w.CreationDate < toDate
                    && w.SabtStatus == Common.GlobalEnum.SabtStatusEnum.استعلام_نشده
                   ).Select(s => s.CitizenId).ToListAsync();
                    }
                    else
                    {
                        listCitizenId = await _citizen.Where(w => w.NationId == 0 && w.CreationDate >= fromDate && w.CreationDate < toDate
                    && w.SabtStatus == Common.GlobalEnum.SabtStatusEnum.استعلام_نشده
                   ).Select(s => s.CitizenId).ToListAsync();
                    }

                }
                else if (model.ExportType == Common.GlobalEnum.ExportCitizenTypeEnum.BagRezvan)
                {
                    //
                    if (model.GroupId != null)
                    {
                        var citizenGroupIds = await _groupcitizen.Where(w => w.GroupId == model.GroupId).Select(s => s.CitizenId).ToListAsync();


                        listCitizenId = await _citizen.Where(w => citizenGroupIds.Contains(w.CitizenId) && w.NationId == 0 && w.CreationDate >= fromDate && w.CreationDate < toDate
                    && w.SabtStatus != Common.GlobalEnum.SabtStatusEnum.فوتی
                   ).Select(s => s.CitizenId).ToListAsync();
                    }
                    else
                    {
                        listCitizenId = await _citizen.Where(w => w.NationId == 0 && w.CreationDate >= fromDate && w.CreationDate < toDate
                    && w.SabtStatus != Common.GlobalEnum.SabtStatusEnum.فوتی
                   ).Select(s => s.CitizenId).ToListAsync();
                    }


                }
                

                if(listCitizenId.Any())
                {

                    var exportNumber = 0;
                    if(await _export.AnyAsync())
                    {
                        exportNumber =await _export.MaxAsync(w => w.ExportNumber);
                        exportNumber++;
                    }


                    var export = new ExportCitizens()
                    {
                        CountRow= listCitizenId.Count,
                        CreationDate=DateTime.Now,
                        ExportById= userId,
                        ExportType=model.ExportType, 
                        ExportNumber= exportNumber,
                        GroupId=model.GroupId,
                        
                    };

                  await  _export.AddAsync(export);
                  var listExport = new List<ExportedCitizens>();
                    foreach (var citizenId in listCitizenId)
                    {
                        listExport.Add(new ExportedCitizens()
                        {
                            CitizenId= citizenId,
                            CreationDate=DateTime.Now,
                            Export= export, 
                            
                        });
                    }

                      _exportCitizen.AddRange(listExport); 
                    await _uow.SaveChangesAsync();
                    res.Data.Id = export.Id; 
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "هیچ شهروندی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res; 
                }
                 
               
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است"+ er.Message;
               
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

         
        public async Task<ApiResult<PagedExportedCitizenViewModel>> GetAllCitizenExported(
        int exportId,
        int pageNumber, int pageSize  )
        {
            var res = new ApiResult<PagedExportedCitizenViewModel>(true, ApiResultStatusCode.Success, new PagedExportedCitizenViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _exportCitizen.Where(w => w.ExportId == exportId); 


                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new ExportedCitizensInfo()
                { 
                    CreationDate = s.CreationDate,
                    BirthDate=s.Citizen.BirthDate,
                    ExportId=s.ExportId,
                    CitizenId=s.CitizenId,
                    UserCode=s.Citizen.UserCode,
                    FatherName=s.Citizen.FatherName,
                    FirstName=s.Citizen.FirstName,
                    Gender=s.Citizen.Gender,
                    Id=s.Id,
                    LastName=s.Citizen.LastName,
                    NationCode=s.Citizen.NationCode,
                    Verified=s.Verified,
                    VerifyDate=s.VerifyDate, 
                    SabtStatus=s.Citizen.SabtStatus,
                    IsConfirmed=s.Export.SendOnDate!=null,
                    Count = s.Count,

                }).OrderBy(o=>o.SabtStatus).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }






        public async Task<ApiResult> AddCitizenTodayExportList(int citizenId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"با موفقیت به صف استعلام اضافه گردید" );
            try
            {


                var code = DateTime.Now.ToString("yyyyMMdd");  //کد روز جاری
                var todayexport=  await _export.FirstOrDefaultAsync(w => w.Code== code);
                if (todayexport == null)
                {
                    var exportNumber = 0;
                    if (await _export.AnyAsync())
                    {
                        exportNumber = await _export.MaxAsync(w => w.ExportNumber);
                        exportNumber++;
                    }

                    var export = new ExportCitizens()
                    {
                        CountRow = 0,
                        CreationDate = DateTime.Now,
                        ExportById = 1,// کاربر سیستمی
                        ExportType = Common.GlobalEnum.ExportCitizenTypeEnum.Sabt,
                        ExportNumber = exportNumber,
                        Code= code
                    };
                    await _export.AddAsync(export); 
                    await _exportCitizen.AddAsync(new ExportedCitizens()
                    {
                        CitizenId= citizenId,
                        Export= export,
                        CreationDate=DateTime.Now, 
                    });
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    await _exportCitizen.AddAsync(new ExportedCitizens()
                    {
                        CitizenId = citizenId,
                        Export = todayexport,
                        CreationDate = DateTime.Now,
                    });
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

        public async Task<ApiResult<ShortCitizenInfo>> GetCitizenForAuthentication()
        {
            var res = new ApiResult<ShortCitizenInfo>(true, ApiResultStatusCode.Success, new ShortCitizenInfo());
            try
            { 

                var info = await _exportCitizen.Include(i=>i.Citizen).FirstOrDefaultAsync(w =>
                w.Export.Code != null && w.VerifyDate == null && w.Citizen.BirthDate != null && w.Export.ExportType==Common.GlobalEnum.ExportCitizenTypeEnum.Sabt
                && w.Citizen.SabtStatus==Common.GlobalEnum.SabtStatusEnum.استعلام_نشده    );
                if (info != null && info.Citizen!=null  )
                {
                    var citizen = new ShortCitizenInfo()
                    {
                        CitizenId = info.CitizenId,
                        NationCode = info.Citizen.NationCode,
                        BirthDate = info.Citizen.BirthDate,

                    };
                    res.Data = citizen;
                    info.VerifyDate = DateTime.Now;
                    var count = info.Count;
                   
                    info.Count = ++ count;
                    _exportCitizen.Update(info);
                    await _uow.SaveChangesAsync();
                    return res;
                }

                var exportedCitizens2 = await _exportCitizen.Include(i => i.Citizen).FirstOrDefaultAsync(w =>
                 w.Export.Code != null    && w.Citizen.BirthDate!=null && w.Count < 5 && w.Citizen.SabtStatus == Common.GlobalEnum.SabtStatusEnum.استعلام_نشده);
 

                if (exportedCitizens2 != null && exportedCitizens2.Citizen != null)
                {
                    res.Data = new ShortCitizenInfo()
                    {
                        CitizenId = exportedCitizens2.CitizenId,
                        NationCode = exportedCitizens2.Citizen.NationCode,
                        BirthDate = exportedCitizens2.Citizen.BirthDate
                    };
                   

                    exportedCitizens2.VerifyDate = DateTime.Now;
                    var count = exportedCitizens2.Count;
                    
                    exportedCitizens2.Count =++ count;
                    _exportCitizen.Update(exportedCitizens2);
                    await _uow.SaveChangesAsync();
                    return res;
                }
                 

                res.Data = null;
                res.IsSuccess = false;
                return res;



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
             
            return res;
        }

     



        public async Task<ApiResult<List<ExportedCitizensInfo>>> GetOnlineAuthentication(  int exportId )
        {
            var res = new ApiResult<List<ExportedCitizensInfo>>(true, ApiResultStatusCode.Success, new List<ExportedCitizensInfo>());
            try
            {
                
                var query = _exportCitizen.Where(w =>w.Citizen.SabtStatus==Common.GlobalEnum.SabtStatusEnum.استعلام_نشده  && w.ExportId == exportId);
                res.Data = await query.Select(s => new ExportedCitizensInfo()
                {
                    CreationDate = s.CreationDate,
                    BirthDate = s.Citizen.BirthDate,
                    ExportId = s.ExportId,
                    CitizenId = s.CitizenId,
                    UserCode = s.Citizen.UserCode,
                    FatherName = s.Citizen.FatherName,
                    FirstName = s.Citizen.FirstName,
                    Gender = s.Citizen.Gender,
                    Id = s.Id,
                    LastName = s.Citizen.LastName,
                    NationCode = s.Citizen.NationCode,
                    Verified = s.Verified,
                    VerifyDate = s.VerifyDate,
                    SabtStatus = s.Citizen.SabtStatus,
                    Count = s.Count,
                    IsConfirmed = s.Export.SendOnDate != null
                }).OrderBy(o => o.SabtStatus).AsNoTracking().ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }


        public async Task<ApiResult<List<ExportedCitizensInfo>>> GetOnlineAuthenticationChekStateLife(int exportId)
        {
            var res = new ApiResult<List<ExportedCitizensInfo>>(true, ApiResultStatusCode.Success, new List<ExportedCitizensInfo>());
            try
            {


                var query = _exportCitizen.Where(w => w.Citizen.SabtStatus != Common.GlobalEnum.SabtStatusEnum.فوتی && w.ExportId == exportId);
                res.Data = await query.Select(s => new ExportedCitizensInfo()
                {
                    CreationDate = s.CreationDate,
                    BirthDate = s.Citizen.BirthDate,
                    ExportId = s.ExportId,
                    CitizenId = s.CitizenId,
                    UserCode = s.Citizen.UserCode,
                    FatherName = s.Citizen.FatherName,
                    FirstName = s.Citizen.FirstName,
                    Gender = s.Citizen.Gender,
                    Id = s.Id,
                    LastName = s.Citizen.LastName,
                    NationCode = s.Citizen.NationCode,
                    Verified = s.Verified,
                    VerifyDate = s.VerifyDate,
                    SabtStatus = s.Citizen.SabtStatus,
                    Count = s.Count,
                    IsConfirmed = s.Export.SendOnDate != null
                }).OrderBy(o => o.SabtStatus).AsNoTracking().ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }



        public async Task<ApiResult<string>> Send(int Id,int userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت ارسال شد");
            try
            {

                var item = await _export.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                item.SendOnDate = DateTime.Now ;
                item.ReceiveById = userId;
                _export.Update(item);
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
