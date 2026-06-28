 
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.NewsItem;
using Nikan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services
{

    public interface IMenuItemService
    {
        Task<ApiResult<MenuItemDto>> AddOrUpdate(MenuItemDto model, int userId);
        Task<ApiResult<List<MenuItemInfo>>> GetAll();
        Task<ApiResult<List<MenuItemsListDto>>> GetMainMenuItems();
        Task<ApiResult<MenuItemInfo>> GetMenu(int id);
        Task<ApiResult<List<TreeMenu>>> GetTreeMenus();
         ApiResult<List<MenuItem>> GetTreeMenus2();
        #region Remove

        Task<ApiResult<string>> Remove(int id);
        Task<ApiResult<string>> UpdateSort(List<SortMenuDto> tabs, int? parentId);


        #endregion
    }


    public class MenuItemService: IMenuItemService
    {
        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MenuItem> _menu;
         

        public MenuItemService(IUnitOfWork uow )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _menu = _uow.Set<MenuItem>();
            
        }

        #endregion

        public async Task<ApiResult<List<MenuItemInfo>>> GetAll()
        {
            var res = new ApiResult<List<MenuItemInfo>>(true, ApiResultStatusCode.Success, new List<MenuItemInfo>());
            try
            {
                var query = _menu.Where(w => w.IsDeleted != true); 
                res.Data = await query.Select(item => new MenuItemInfo()
                {
                    DisableLink = item.DisableLink,
                    IconFile = item.IconFile,
                    Id = item.Id,
                    IsVisible = item.IsVisible,
                    MenuName = item.MenuName,
                    MenuPath = item.MenuPath,
                    OpenInNewPage = item.OpenInNewPage,
                    ParentId = item.ParentId,
                    TabOrder = item.TabOrder,
                    CreatedByUser=item.CreatedByUser.DisplayName,
                    IsSystem=item.IsSystem,
                    Parent = item.Parent==null ? "" :item.Parent.MenuName,
                    ModifiedOnDate=item.ModifiedOnDate,
                    LastModifiedByUserId=item.LastModifiedByUserId,
                    CreatedByUserId=item.CreatedByUserId,
                    CreatedOnDate=item.CreatedOnDate,
                    LastModifiedByUser=item.LastModifiedByUser==null ? "":item.LastModifiedByUser.DisplayName,
                    
                    


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

        public async Task<ApiResult<List<TreeMenu>>> GetTreeMenus()
        {
            var res = new ApiResult<List<TreeMenu>>(true, ApiResultStatusCode.Success, new List<TreeMenu>());
            try
            {



                var list =await _menu.Include(s=>s.Children).Include(p=>p.Parent) 
                    .OrderBy(o => o.TabOrder).Select(item => new TreeMenu()
                {
                    Id=item.Id,
                    DisableLink = item.DisableLink,
                    Icon = item.IconFile,  
                    Name = item.MenuName,
                    Url = item.MenuPath,
                    OpenInNewPage = item.OpenInNewPage, 
                    IsDeleted=item.IsDeleted,
                    IsVisible=item.IsVisible,
                    ParentId=item.ParentId,
                    IsSystem = item.IsSystem,
                        Children =item.Children.Select(s=>new TreeMenu() {Name=s.MenuName ,Url=s.MenuPath }).ToList()

                }).Where(w => w.IsDeleted != true && w.IsVisible && w.ParentId == null).ToListAsync();

                 

                res.Data = list;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

        public  ApiResult<List<MenuItem>>  GetTreeMenus2()
        {
            var res = new ApiResult<List<MenuItem>>(true, ApiResultStatusCode.Success, new List<MenuItem>());
            try
            {

                res.Data = _menu.Include(s => s.Children).Include(p => p.Parent).ToList().Where(w => w.ParentId == null).ToList();
                 

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }





        public async Task<ApiResult<MenuItemInfo>> GetMenu(int id)
        {
            var res = new ApiResult<MenuItemInfo>(true, ApiResultStatusCode.Success, new MenuItemInfo());
            try
            {
                var item = await _menu.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";

                }


                res.Data = new MenuItemInfo()
                {
                     DisableLink= item.DisableLink,
                     IconFile=item.IconFile,
                     Id=item.Id,
                     IsVisible=item.IsVisible,
                     MenuName=item.MenuName,
                     MenuPath=item.MenuPath,
                     OpenInNewPage=item.OpenInNewPage,
                     ParentId=item.ParentId,
                     TabOrder=item.TabOrder,
                     IsSystem=item.IsSystem,
                     Parent=item.Parent==null ? "":item.Parent.MenuName,
                     CreatedByUserId=item.CreatedByUserId,
                     CreatedOnDate=item.CreatedOnDate,
                     
                     

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
        public async Task<ApiResult<MenuItemDto>> AddOrUpdate(MenuItemDto model, int userId)
        {
            if (model.ParentId == 0)
                model.ParentId = null;
            var res = new ApiResult<MenuItemDto>(true, ApiResultStatusCode.Success, new MenuItemDto());
            try
            {
                if (model.Id == null)
                {
                    var item = new MenuItem
                    {
                        CreatedByUserId = userId,
                        TabOrder = model.TabOrder,
                        ParentId = model.ParentId,
                        OpenInNewPage = model.OpenInNewPage,
                        MenuPath = model.MenuPath,
                        ModifiedOnDate = DateTime.Now,
                        CreatedOnDate = DateTime.Now,
                        DisableLink = model.DisableLink,
                        IconFile = model.IconFile,
                        IsVisible = model.IsVisible,
                        LastModifiedByUserId = userId,
                        MenuName = model.MenuName,
                        IsSystem = model.IsSystem,

                    };


                    await _menu.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = item.Id;
                    res.Data.MenuName = item.MenuName;
                    res.Data.MenuPath = item.MenuPath;

                }
                else
                {

                    var item = await _menu.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.TabOrder = model.TabOrder;
                    item.ParentId = model.ParentId;
                    item.OpenInNewPage = model.OpenInNewPage;
                    item.IsSystem = model.IsSystem;
                    item.MenuPath = model.MenuPath;
                     
                   
                    item.DisableLink = model.DisableLink;
                    item.IconFile = model.IconFile;
                    item.IsVisible = model.IsVisible;
                    item.MenuName = model.MenuName;
                    item.ModifiedOnDate = DateTime.Now;
                    _menu.Update(item);
                    await _uow.SaveChangesAsync();

                }




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;

        }

        public async Task<ApiResult<string>> UpdateSort(List<SortMenuDto> tabs, int? parentId)
        {
            int order = 0;
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success,"");
            try
            {
                foreach (var item in tabs)
                {
                    var menu =await _menu.FirstOrDefaultAsync(w=>w.Id==item.id);
                    if (menu != null)
                    {
                        menu.ParentId = parentId;
                        menu.TabOrder = order;
                        _menu.Update(menu);

                        if (item.children != null)
                          await  UpdateSort(item.children, item.id); 
                        order++;
                    }
                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            } 
            return res;
        }


        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
               
                var item = await _menu.Include(s => s.Children).Include(p => p.Parent).FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }

                //if(item.IsSystem)
                //{

                //    res.IsSuccess = false;
                //    res.Messages = "امکان حذف منوی سیستمی وجود ندارد";
                //    return res;

                //}
                item.ModifiedOnDate = DateTime.Now;
                item.IsDeleted = true;

                foreach (var children in item.Children)
                {
                    children.ParentId = null;

                }




                _menu.Update(item);
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

        public async Task<ApiResult<List<MenuItemsListDto>>> GetMainMenuItems()
        {
            var res = new ApiResult<List<MenuItemsListDto>>(true, ApiResultStatusCode.Success, new List<MenuItemsListDto>());
            try
            {

                //  var result = _menu.Child.Where(child => EF.Property<int?>(child, "ParentId") == 3);

                var result =await _menu.Include(i => i.Children).ToListAsync();
                var query = _menu.Include(i => i.Children).Where(w => w.IsDeleted != true);
                res.Data = await query.Select(item => new MenuItemsListDto()
                {
                    DisableLink = item.DisableLink,
                    IconFile = item.IconFile,
                    Id = item.Id,
                    IsVisible = item.IsVisible,
                    MenuName = item.MenuName,
                    MenuPath = item.MenuPath,
                    OpenInNewPage = item.OpenInNewPage,
                    ParentId = item.ParentId,
                    IsSystem = item.IsSystem,
                    ParentName = item.Parent == null ? "" : item.Parent.MenuName,
                    TabOrder = item.TabOrder, 
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




    }



}
