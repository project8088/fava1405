
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel;
using Nikan.ViewModel.Group;
using Nikan.ViewModel.ImportExcelFile;
using Nikan.ViewModel.Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cle.Services.CitizensGroups
{


    public interface IGroupService
    {
        Task<ApiResult<UploadFileResult>> AddNationCodeList(
      List<ImportFileGroupNationCodeInfo> list,
      string fileName,
      int groupId,
      int userId);

        Task<ApiResult<GroupDto>> AddOrUpdate(GroupDto model, int userId);

        Task<ApiResult<List<Nikan.ViewModel.Group.GroupInfo>>> GetAllGroups();
        Task<ApiResult<List<BaseDataModel>>> GetAllRefunAccessUsers(int groupId);
        Task<ApiResult<List<BaseDataModel>>> GetFreeCardGroups(int? selected);
        Task<ApiResult<List<BaseDataModel>>> GetGroups(int? selected);

        Task<ApiResult<Nikan.ViewModel.Group.GroupInfo>> GroupInfo(int id);

        Task<ApiResult> GroupTransfer(GroupTransferModel model, int userId);

        Task<ApiResult<string>> Remove(int Id);

        Task<ApiResult<string>> RemoveQueue(int Id);

        Task<ApiResult> ReviewGroups(int id);

        Task<ApiResult<PagedCitizensQueueViewModel>> SearchCitizensQueue(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string groupname = null,
          string nationCode = null,
          int? groupId = null);

        Task<ApiResult<PagedGroupsViewModel>> SearchGroups(
          int pageNumber,
          int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string groupname = null);

        Task<ApiResult<List<BaseDataModel>>> SearchGroups(
          string query,
          int offset = 0,
          int count = 20);

        Task<ApiResult<PagedRefundAccessViewModel>> SearchRefundUser(
          int pageNumber,
          int pageSize,
          int groupId,
          string name = null,
          string nationCode = null);

    }
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Group> _group;
        private readonly DbSet<CitizensQueue> _citizensQueue; 
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<CitizenProfile> _profile; 
        private readonly DbSet<GroupsCitizens> _groupCitizens;
 

        public GroupService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _group = _uow.Set<Group>();
            _citizensQueue = _uow.Set<CitizensQueue>();
            _citizen = _uow.Set<Citizen>();
            _profile = _uow.Set<CitizenProfile>();
            _groupCitizens = _uow.Set<GroupsCitizens>();
           

        }

         
   
         
        public async Task<ApiResult<List<BaseDataModel>>> GetGroups(int? selected)
        { 
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            { 
                res.Data = await _group.Where(w =>w.IsActive==true && w.IsDeleted != true).Select(s => new BaseDataModel()
                { 
                    Disabled = !s.IsActive,
                    Selected = s.Id == selected,
                    Text = s.GroupName,
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
        public async Task<ApiResult<List<BaseDataModel>>> GetFreeCardGroups(int? selected)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _group.Where(w =>w.CanBuyFreeCard==true  && w.IsActive == true && w.IsDeleted != true).Select(s => new BaseDataModel()
                {
                    Disabled = !s.IsActive,
                    Selected = s.Id == selected,
                    Text = s.GroupName + "("+s.GroupsCitizens.Count(r=>r.IsDeleted!=true)+ " شهروند )",
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


        public async Task<ApiResult<List<BaseDataModel>>> SearchGroups(string query, int offset = 0, int count = 20)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _group.Where(w => w.IsDeleted != true && w.GroupName.Contains(query))
                .Select(s => new BaseDataModel()
                {
                    Text = s.GroupName, 
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






        public async Task<ApiResult<PagedGroupsViewModel>> SearchGroups(
         int pageNumber, int pageSize,
         DateTime? FromDate = null,
         DateTime? ToDate = null,
         string groupname = null 
         )
        {

            var res = new ApiResult<PagedGroupsViewModel>(true, ApiResultStatusCode.Success, new PagedGroupsViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _group.Where(w=>w.IsDeleted!=true);

                if (!string.IsNullOrEmpty(groupname))
                {
                    query = query.Where(w => EF.Functions.Like(w.GroupName, "%" + groupname + "%"));
                }
                 

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreationDate <= ToDate);
                }


                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new GroupInfo()
                {
                    CreatedByUserId = s.CreatedByUserId,
                    GroupName = s.GroupName,
                    AutoAddMembers = s.AutoAddMembers,
                    Code = s.Code,
                    CreatedByUser = s.CreatedByUser.Username,
                    CreationDate = s.CreationDate,
                    ExpireDate = s.ExpireDate,
                    GroupCategory = s.GroupCategory,
                    Id = s.Id,
                    IsActive = s.IsActive,
                    Law_AgeFrom = s.Law_AgeFrom,
                    Law_AgeTo = s.Law_AgeTo,
                    Law_City = s.Law_City,
                    Law_EducationLeve = s.Law_EducationLeve,
                    Law_Gender = s.Law_Gender,
                    Law_JobGroup = s.Law_JobGroup,
                    Law_MariageStatus = s.Law_MariageStatus,
                    MainGroupId = s.MainGroupId,
                    MaxMembers = s.MaxMembers,
                    ParentId = s.ParentId,
                    Parent = s.Parent.GroupName,
                    ShowToAddCitizen = s.ShowToAddCitizen,
                    ShowToMembers = s.ShowToMembers,
                    SpecialRules = s.SpecialRules,
                    UseForServices = s.UseForServices,
                    ViewCssClass = s.ViewCssClass,
                    ViewIcon = s.ViewIcon ,
                     MunicipalPersonnelGroup=s.MunicipalPersonnelGroup,

                    CountCitizen=s.GroupsCitizens.Count,
                    CountQueue=s.CitizensQueue.Count

                }).OrderByDescending(o=>o.Id) .Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }






     public async Task<ApiResult<PagedRefundAccessViewModel>> SearchRefundUser(
         int pageNumber,
          int pageSize,
          int groupId,
          string name = null,
          string nationCode = null
         )
        {


            var res = new ApiResult<PagedRefundAccessViewModel>(true, ApiResultStatusCode.Success, new PagedRefundAccessViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _groupCitizens.Where(w=>w.GroupId== groupId &&  w.IsDeleted!=true);

                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w =>  w.Citizen.NationCode== nationCode);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.Citizen.FirstName, "%" + name + "%")
                    ||
                     EF.Functions.Like(w.Citizen.LastName, "%" + name + "%")  
                    );
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new GroupCitizensInfo()
                { 
                    Id = s.Id,
                    GroupId=s.GroupId,
                    Citizen=s.Citizen.FirstName +" "+s.Citizen.LastName,
                    NationCode = s.Citizen.NationCode,
                    UserCode = s.Citizen.UserCode,
                    CitizenId =s.CitizenId,
                    AddByUserId=s.AddByUserId,
                    AddByUser=s.AddByUser.Username,
                    CreationDate=s.CreationDate,
                    Group=s.Group.GroupName,
                    
                    

                }).OrderByDescending(o=>o.Id) .Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }





 

        public async Task<ApiResult<List<BaseDataModel>>> GetAllRefunAccessUsers(int groupId )
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                var query = _groupCitizens.AsNoTracking().Where(w => w.GroupId == groupId && w.IsDeleted != true);


                res.Data = await query.Select(s => new BaseDataModel()
                {
                    Text = s.Citizen.LastName +"  " + s.Citizen.FirstName + " (   " + s.Citizen.NationCode + ")   ",
                    Key = s.CitizenId.ToString(), 
                }).OrderBy(o=>o.Text).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }






        public async Task<ApiResult<List<GroupInfo>>> GetAllGroups()
        {
            var res = new ApiResult<List<GroupInfo>>(true, ApiResultStatusCode.Success, new List<GroupInfo>());
            try
            {

                res.Data = await _group.Where(w => w.IsDeleted != true).Select(s => new GroupInfo()
                {
                    CreatedByUserId=s.CreatedByUserId,
                    GroupName=s.GroupName,
                    AutoAddMembers=s.AutoAddMembers,
                    Code=s.Code,
                    CreatedByUser=s.CreatedByUser.Username,
                    CreationDate=s.CreationDate,
                    ExpireDate=s.ExpireDate,
                    GroupCategory=s.GroupCategory,
                    Id=s.Id,
                    IsActive=s.IsActive,
                    Law_AgeFrom=s.Law_AgeFrom,
                    Law_AgeTo=s.Law_AgeTo,
                    Law_City=s.Law_City,
                    Law_EducationLeve=s.Law_EducationLeve,
                    Law_Gender=s.Law_Gender,
                    Law_JobGroup=s.Law_JobGroup,
                    Law_MariageStatus=s.Law_MariageStatus,
                    MainGroupId=s.MainGroupId,
                    MaxMembers=s.MaxMembers,
                    ParentId=s.ParentId,
                    Parent=s.Parent.GroupName,
                    ShowToAddCitizen=s.ShowToAddCitizen,
                    ShowToMembers=s.ShowToMembers,
                    SpecialRules=s.SpecialRules,
                    UseForServices=s.UseForServices,
                    ViewCssClass=s.ViewCssClass,
                    ViewIcon=s.ViewIcon,
                    MunicipalPersonnelGroup=s.MunicipalPersonnelGroup,
                   
                    
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
       
        public async Task<ApiResult<GroupInfo>> GroupInfo(int id)
        {
            var res = new ApiResult<GroupInfo>(true, ApiResultStatusCode.Success, new GroupInfo());
            try
            {

                res.Data = await _group.Where(w =>w.Id== id && w.IsDeleted != true).Select(s => new GroupInfo()
                {
                    CreatedByUserId = s.CreatedByUserId,
                    GroupName = s.GroupName,
                    AutoAddMembers = s.AutoAddMembers,
                    Code = s.Code,
                    CreatedByUser = s.CreatedByUser.Username,
                    CreationDate = s.CreationDate,
                    ExpireDate = s.ExpireDate,
                    GroupCategory = s.GroupCategory,
                    Id = s.Id,
                    IsActive = s.IsActive,
                    Law_AgeFrom = s.Law_AgeFrom,
                    Law_AgeTo = s.Law_AgeTo,
                    Law_City = s.Law_City,
                    Law_EducationLeve = s.Law_EducationLeve,
                    Law_Gender = s.Law_Gender,
                    Law_JobGroup = s.Law_JobGroup,
                    Law_MariageStatus = s.Law_MariageStatus,
                    MainGroupId = s.MainGroupId,
                    MaxMembers = s.MaxMembers,
                    ParentId = s.ParentId,
                    Parent = s.Parent.GroupName,
                    ShowToAddCitizen = s.ShowToAddCitizen,
                    ShowToMembers = s.ShowToMembers,
                    SpecialRules = s.SpecialRules,
                    UseForServices = s.UseForServices,
                    ViewCssClass = s.ViewCssClass,
                    ViewIcon = s.ViewIcon ,
                    MunicipalPersonnelGroup=s.MunicipalPersonnelGroup,
                    CanBuyFreeCard=s.CanBuyFreeCard
                    

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



        public async Task<ApiResult> ReviewGroups(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "بازبینی با موفقیت صورت گرفت");
            try
            {


                var list =await _citizensQueue.Where(w => w.GroupId == id).ToListAsync();
                if(list.Any())
                {
                    foreach (var item in list)
                    {
                     
                        var nationCode = item.NationCode.Fa2En();
                        var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                        if (citizen != null)
                        {
                            if (await _groupCitizens.AnyAsync(w => w.GroupId == id && w.CitizenId == citizen.CitizenId))
                            {
                                //اگر وجود داشت صف حذف کن
                                _citizensQueue.Remove(item);
                            }
                            else
                            {
                                //اضافه کردن شهروند به گروه
                                await _groupCitizens.AddAsync(new GroupsCitizens()
                                {
                                    CitizenId = citizen.CitizenId,
                                    GroupId = id,
                                    AddByUserId = item.AddByUserId,
                                    CreationDate = DateTime.Now, 
                                });
                                //شهروند اضافه کردی حالا صف حذف کن
                                _citizensQueue.Remove(item);

                            }
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


        public async Task<ApiResult> GroupTransfer(GroupTransferModel model,int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "انتقال با موفقیت صورت گرفت");
            try
            {
               
                if(model.SourceGroupId==model.DestinationGroupId)
                {
                    res.IsSuccess = false;
                    res.Messages = "گروه مبدا و مقصد همسان می باشد.";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                var add=0;
                var queueadd = 0;


                var sourceList =await _groupCitizens.Where(w => w.GroupId == model.SourceGroupId).ToListAsync();
                if(sourceList.Any())
                {
                    foreach (var source in sourceList)
                    {

                        if(!await _groupCitizens.AnyAsync(w=>w.GroupId==model.DestinationGroupId && w.CitizenId== source.CitizenId))
                        {
                            await _groupCitizens.AddAsync(new GroupsCitizens()
                            {
                                AddByUserId= userId,
                                CitizenId= source.CitizenId,
                                CreationDate=DateTime.Now,
                                GroupId = model.DestinationGroupId, 
                            });
                            add++;
                            if(model.IsTransfer)
                            {
                                _groupCitizens.Remove(source);
                            }

                        }

                    }
                }


                //آیا صف در نظر گرفته شود ؟
                if(model.IsHasQueue)
                {

                   
                    var listQueue = await _citizensQueue.Where(w => w.GroupId == model.SourceGroupId).ToListAsync();
                    if (listQueue.Any())
                    {
                        foreach (var source in listQueue)
                        {

                            if (!await _citizensQueue.AnyAsync(w => w.GroupId == model.DestinationGroupId && w.NationCode == source.NationCode))
                            {
                                await _citizensQueue.AddAsync(new CitizensQueue()
                                {
                                    AddByUserId = userId,
                                    NationCode = source.NationCode,
                                    CreationDate = DateTime.Now,
                                    GroupId = model.DestinationGroupId,
                                });
                                queueadd++;
                                if (model.IsTransferQueue)
                                {
                                    _citizensQueue.Remove(source);
                                }

                            }

                        }
                    }

                }
              






                if (add>0  || queueadd > 0)
                {
                    await _uow.SaveChangesAsync();
                    var message = "تعداد " + add +" شهروند با موفقیت انتقال یافت ";
                    res.Messages = message;

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











        public async Task<ApiResult<UploadFileResult>> AddNationCodeList(List<ImportFileGroupNationCodeInfo> list, string fileName, int groupId, int userId)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            var invalidCode = "";
            var duplicateCode = "";
            try
            {
                if (groupId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه گروه مشخص نشده است ";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var listAdd = new List<string>();
              
                foreach (var item in list)
                {

                    if (string.IsNullOrWhiteSpace(item.NationCode))
                    {
                        continue;
                    }


                    var nationCode = item.NationCode.Fa2En();
                    var check = nationCode.IsValidNationalCode();
                    if (check != "")
                    {

                        invalidCode += ";"+ nationCode;
                         continue;
                    }

                    //برای اینکه کد ملی دو بار نباشد
                    if (listAdd.Contains(nationCode))
                    {
                        duplicateCode += nationCode + ";";
                        continue;
                    }
                        
                    else
                        listAdd.Add(nationCode);



                    int? citizenId = null;
                    //بیا چک کن ببین شهروندیی با این کد ملی وجود دارد یا نه ؟
                    var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == item.NationCode);
                    if (citizen != null)
                    {
                        //شهروندی با این کد ملی وجود دارد اوکی
                        citizenId = citizen.CitizenId;

                    }

                    if (citizenId == null)
                    {
                        //صف
                        //چک کن اگر این کد ملی قبلا به صف اضافه نشده باشه اضافه کن
                        

                        if (!await _citizensQueue.AnyAsync(w => w.NationCode == nationCode
                       && w.GroupId == groupId))
                        {
                            //اضافه کردن شهروند به گروه
                            await _citizensQueue.AddAsync(new  CitizensQueue()
                            {
                                NationCode = nationCode,
                                GroupId = groupId,
                                AddByUserId = userId,
                                CreationDate = DateTime.Now,
                            });


                        }


                    }
                    else
                    {
                        //گروه
                        //چک کن اگر این کد ملی قبلا به گروه اضافه نشده باشه اضافه کن
                        if(! await _groupCitizens.AnyAsync(w=>w.CitizenId== citizenId.Value 
                        && w.GroupId== groupId))
                        {
                            //اضافه کردن شهروند به گروه
                            await _groupCitizens.AddAsync(new GroupsCitizens()
                            {
                                CitizenId = citizenId.Value,
                                GroupId = groupId,
                                AddByUserId = userId,
                                CreationDate = DateTime.Now,
                            });

                        }
                        

                         

                    }


                }

                
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            res.Data.ErrorMessage = "  invalid Code = ["+ invalidCode +"]"+  "duplicate Code: ["+ duplicateCode+"]";
            return res;

        }









        public async Task<ApiResult<GroupDto>> AddOrUpdate(GroupDto model,int userId )
        {

            var res = new ApiResult<GroupDto>(true, ApiResultStatusCode.Success, new GroupDto());
            try
            {
                if (model.Id == null || model.Id.Value == 0)
                {
                    var maxid =await _group.MaxAsync(w => w.Id);
                    maxid += 1;
                    var item = new Group()
                    {
                        Id= maxid,
                        CreatedByUserId = userId,
                        GroupName = model.GroupName,
                        AutoAddMembers = model.AutoAddMembers,
                        Code = model.Code, 
                        CreationDate = DateTime.Now,
                        ExpireDate = model.ExpireDate, 
                        IsActive = model.IsActive,
                        Law_AgeFrom = model.Law_AgeFrom,
                        Law_AgeTo = model.Law_AgeTo, 
                        Law_Gender = model.Law_Gender, 
                        MaxMembers = model.MaxMembers,
                        ParentId = model.ParentId, 
                        ShowToAddCitizen = model.ShowToAddCitizen,
                        ShowToMembers = model.ShowToMembers,
                        SpecialRules = model.SpecialRules,
                        MunicipalPersonnelGroup=model.MunicipalPersonnelGroup,
                        CanBuyFreeCard=model.CanBuyFreeCard

                    };
                    await _group.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    
                }
                else
                {
                    var item = _group.Find(model.Id);
                    if (item.IsSystem)
                    {
                        res.IsSuccess = false;
                        res.Messages = " امکان ویرایش گروههای سیستمی وجود ندارد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        return res;
                    }



                    item.GroupName = model.GroupName;
                    item.AutoAddMembers = model.AutoAddMembers;
                    item.Code = model.Code;

                    item.ExpireDate = model.ExpireDate;
                    
                    item.IsActive = model.IsActive;
                    item.Law_AgeFrom = model.Law_AgeFrom;
                    item.Law_AgeTo = model.Law_AgeTo;
                    item.CanBuyFreeCard = model.CanBuyFreeCard; 
                    item.Law_Gender = model.Law_Gender; 
                    item.MaxMembers = model.MaxMembers;
                    item.ParentId = model.ParentId;
                    item.ShowToAddCitizen = model.ShowToAddCitizen;
                    item.ShowToMembers = model.ShowToMembers;
                    item.SpecialRules = model.SpecialRules;
                     item.MunicipalPersonnelGroup = model.MunicipalPersonnelGroup;
                    _group.Update(item); 
                    await _uow.SaveChangesAsync();
                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
           

        }

     
        public async Task<ApiResult<string>> Remove(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _group.Include(i=>i.GroupsCitizens).FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                if (item.IsSystem  )
                {
                    res.IsSuccess = false;
                    res.Messages = " امکان حذف گروههای سیستمی وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }



                if (item.GroupsCitizens.Any())
                {
                    item.IsDeleted = true;
                    _group.Update(item);
                }
                else
                { 
                    _group.Remove(item);
                }
               
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


        #region صف شهروندی
        public async Task<ApiResult<PagedCitizensQueueViewModel>> SearchCitizensQueue(
       int pageNumber, int pageSize,
       DateTime? FromDate = null,
       DateTime? ToDate = null,
       string groupname = null,
        string nationCode = null,
        int? groupId=null

       )
        {

            var res = new ApiResult<PagedCitizensQueueViewModel>(true, ApiResultStatusCode.Success, new PagedCitizensQueueViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _citizensQueue.AsQueryable();

                if (!string.IsNullOrEmpty(groupname))
                {
                    query = query.Where(w => EF.Functions.Like(w.Group.GroupName, "%" + groupname + "%"));
                }
                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.NationCode, "%" + nationCode + "%"));
                }

                if (groupId != null)
                {
                    query = query.Where(w => w.GroupId == groupId);
                }


                if (FromDate != null)
                {
                    query = query.Where(w => w.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreationDate <= ToDate);
                }


                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new CitizensQueueInfo()
                {
                    AddByUser=s.AddByUser.DisplayName,
                    AddByUserId=s.AddByUserId,
                    CreationDate=s.CreationDate,
                    Group=s.Group.GroupName,
                    Id=s.Id,
                    GroupId=s.GroupId,
                    NationCode=s.NationCode,
                    

                }).OrderByDescending(o=>o.Id).Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

        public async Task<ApiResult<string>> RemoveQueue(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _citizensQueue.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                _citizensQueue.Remove(item);  
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



    }

}
