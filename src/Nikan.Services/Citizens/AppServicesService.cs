using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.Citizens
{
    public interface IAppService
    {
        Task<ApiResult> AddOrUpdate(AppServicesDto model, int userId);
        Task<ApiResult<PagedAppServiceViewModel>> GetAllApp(int pageNumber, int pageSize, string serviceName = null);
        Task<ApiResult<List<AppServiceInfo>>> GetAppDashbordList(int userId);
        Task<ApiResult<AppServiceInfo>> GetAppInfo(int serviceId);
        Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterList();
        Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterListForMainPage();
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAppService();


        Task<ApiResult<string>> Remove(int id);
    }

    public class AppService: IAppService
    {


        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<AppServices> _app;
        public AppService(IUnitOfWork uow )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _app = _uow.Set<AppServices>();
            
        }

        #endregion

        public async Task<ApiResult> AddOrUpdate(AppServicesDto model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {
                if (model.ServiceId == null)
                {

                    var maxId =await _app.MaxAsync(m => m.Id);
                    var item = new AppServices
                    {
                        Id = maxId+1 ,
                        Icon = model.Icon,
                        Description = model.Description,
                        CreationDate = DateTime.Now,
                        CreatedById = userId,
                        Priority = model.Priority,
                        CssClass = model.CssClass,
                        IsActive = model.IsActive,
                        HaveTerms = model.HaveTerms,
                        IsMain = model.IsMain,
                        IsShowInDashbordCitizen = model.IsShowInDashbordCitizen,
                        ServiceName = model.ServiceName,
                        IsLinkService = model.IsLinkService,
                        IsNeedAuthenticate = model.IsNeedAuthenticate,
                        Link = model.Link,
                        ParamName1 = model.ParamName1,
                        OpenInNewWindow = model.OpenInNewWindow,
                        Terms = model.Terms,
                        ParamName2 = model.ParamName2,
                        ParamValue1 = model.ParamValue1,
                        ParamValue2 = model.ParamValue2,
                        ParentId = model.ParentId,
                        ServicePicture = model.ImageUrl,
                        

                    };


                    await _app.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _app.FirstOrDefaultAsync(w => w.Id == model.ServiceId);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";
                        return res;
                    }

                    item.Icon = model.Icon;
                    item.Description = model.Description; 
                    item.Priority = model.Priority;
                    item.CssClass = model.CssClass;
                    item.IsActive = model.IsActive;
                    item.HaveTerms = model.HaveTerms;
                    item.IsMain = model.IsMain;
                    item.IsShowInDashbordCitizen = model.IsShowInDashbordCitizen;
                    item.ServiceName = model.ServiceName;
                    item.IsLinkService = model.IsLinkService;
                    item.IsNeedAuthenticate = model.IsNeedAuthenticate;
                    item.Link = model.Link;
                    item.ParamName1 = model.ParamName1;
                    item.OpenInNewWindow = model.OpenInNewWindow;
                    item.Terms = model.Terms;
                    item.ParamName2 = model.ParamName2;
                    item.ParamValue1 = model.ParamValue1;
                    item.ParamValue2 = model.ParamValue2;
                    item.ParentId = model.ParentId;
                    item.ServicePicture = model.ImageUrl;
                    _app.Update(item);
                    await _uow.SaveChangesAsync();

                }



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

        public async Task<ApiResult<PagedAppServiceViewModel>> GetAllApp(int pageNumber, int pageSize, string serviceName =null)
        {
            var res = new ApiResult<PagedAppServiceViewModel>(true, ApiResultStatusCode.Success, new PagedAppServiceViewModel(), "");
            try
            {
               
                 var offset = (pageNumber ) * pageSize ; 
                var query = _app.Where(w => w.IsDeleted  !=true );

                if (!string.IsNullOrEmpty(serviceName))
                {
                    query = query.Where(w => EF.Functions.Like(w.ServiceName, "%" + serviceName + "%"));
                }



                var list =await query.Where(w =>  w.IsDeleted != true  ).Select(s => new AppServiceInfo()
                {

                    CssClass = s.CssClass,
                    HaveTerms = s.HaveTerms,
                    Icon = s.Icon,
                    ImageUrl = s.ServicePicture,
                    Link = s.Link,
                    OpenInNewWindow = s.OpenInNewWindow,
                    ServiceId = s.Id,
                    ServiceName = s.ServiceName,
                    Terms=s.Terms,
                    IsActive=s.IsActive,
                    CreatedBy=s.CreatedBy.Username,
                    CreationDate=s.CreationDate,
                    Description=s.Description,
                    IsLinkService=s.IsLinkService,
                    IsMain=s.IsMain,
                    IsNeedAuthenticate=s.IsNeedAuthenticate,
                    ParamName1=s.ParamName1,
                    CreatedById=s.CreatedById,
                    ParamName2=s.ParamName2,
                    ParamValue1=s.ParamValue1,
                    ModifiedOnDate=s.ModifiedOnDate,
                    ParamValue2=s.ParamValue2,
                    ParentId=s.ParentId,
                    Parent=s.Parent.ServiceName,
                    Priority=s.Priority


                }).Skip(offset).Take(pageSize).ToListAsync();


                res.Data = new PagedAppServiceViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items   = list
                };


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;


        }

        public async Task<ApiResult<List<AppServiceInfo>>> GetAppDashbordList(int userId)
        {
            var res = new ApiResult<List<AppServiceInfo>>(true, ApiResultStatusCode.Success, new List<AppServiceInfo>(), "");
            try
            {
                res.Data = await _app.Where(w => w.IsActive && w.IsDeleted != true
                && w.IsShowInDashbordCitizen == true).Select(s => new AppServiceInfo()
                {
                    CssClass = s.CssClass,
                    HaveTerms = s.HaveTerms,
                    Icon = s.Icon,
                    ImageUrl = s.ServicePicture,
                    Link = s.Link,
                    OpenInNewWindow = s.OpenInNewWindow,
                    ServiceId = s.Id,
                    ServiceName = s.ServiceName,
                    Terms = s.Terms,

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


        public async Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterList()
        {
            var res = new ApiResult<List<AppServiceInfo>>(true, ApiResultStatusCode.Success, new List<AppServiceInfo>(), "");
            try
            {
                res.Data =await _app.Where(w => w.IsActive && w.IsDeleted != true && w.IsNeedAuthenticate!=true  ).Select(s => new AppServiceInfo()
                {
                    CssClass=s.CssClass,
                    HaveTerms=s.HaveTerms,
                    Icon=s.Icon,
                    ImageUrl=s.ServicePicture,
                    Link=s.Link,
                    OpenInNewWindow=s.OpenInNewWindow,
                    ServiceId=s.Id,
                    IsShowInDashbordCitizen=s.IsShowInDashbordCitizen,
                    ServiceName=s.ServiceName, 
                    Terms = s.Terms,

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

        public async Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterListForMainPage()
        {
            var res = new ApiResult<List<AppServiceInfo>>(true, ApiResultStatusCode.Success, new List<AppServiceInfo>(), "");
            try
            {

                res.Data = await _app.Where(w => w.IsActive && w.IsMain && w.IsDeleted != true && w.IsNeedAuthenticate != true).Select(s => new AppServiceInfo()
                {
                    CssClass = s.CssClass,
                    HaveTerms = s.HaveTerms,
                    Icon = s.Icon,
                    ImageUrl = s.ServicePicture,
                    Link = s.Link,
                    OpenInNewWindow = s.OpenInNewWindow,
                    ServiceId = s.Id,
                    ServiceName = s.ServiceName,
                    Terms = s.Terms,

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




        public async Task<ApiResult<AppServiceInfo>> GetAppInfo(int serviceId)
        {
            var res = new ApiResult<AppServiceInfo>(true, ApiResultStatusCode.Success, new AppServiceInfo(), "");
            try
            {
                var data = await _app.Where(w => w.Id== serviceId ).Select(s => new AppServiceInfo()
                {
                    CssClass = s.CssClass,
                    HaveTerms = s.HaveTerms,
                    Icon = s.Icon,
                    ImageUrl = s.ServicePicture,
                    Link = s.Link,
                    OpenInNewWindow = s.OpenInNewWindow,
                    ServiceId = s.Id,
                    ServiceName = s.ServiceName,
                    Terms = s.Terms,
                    IsActive = s.IsActive,
                    CreatedBy = s.CreatedBy.Username,
                    CreationDate = s.CreationDate,
                    Description = s.Description,
                    IsLinkService = s.IsLinkService,
                    IsMain = s.IsMain,
                    IsNeedAuthenticate = s.IsNeedAuthenticate,
                    ParamName1 = s.ParamName1,
                    CreatedById = s.CreatedById,
                    ParamName2 = s.ParamName2,
                    ParamValue1 = s.ParamValue1,
                    ModifiedOnDate = s.ModifiedOnDate,
                    ParamValue2 = s.ParamValue2,
                    ParentId = s.ParentId,
                    Parent = s.Parent.ServiceName,
                    Priority = s.Priority,
                    IsShowInDashbordCitizen = s.IsShowInDashbordCitizen,

                }).FirstOrDefaultAsync(); 
                if(data==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه سرویس وارد شده یافت نشد";
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

        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAppService()
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _app.Where(w => w.IsActive )
                                        .OrderBy(o => o.ServiceName).Select
                                        (s => new BaseDataModel()
                                        {
                                            Text = s.ServiceName,
                                            Key = s.Id.ToString(),
                                            ParentValue = s.ParentId.ToString(),
                                            Selected = false, 

                                        })
                                        .ToListAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;



        }

        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _app.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }


                item.ModifiedOnDate = DateTime.Now;
                item.IsDeleted = true;
                _app.Update(item);
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





    }



}
