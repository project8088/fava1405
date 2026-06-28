using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.CitizensCard;
using Nikan.ViewModel;
using Nikan.ViewModel.CitizenCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.CitizenCards
{
    public interface IDiscountCardService
    {
        Task<ApiResult> Add(CardInfo_DiscountDto model, int userId);
        Task<ApiResult> AddCenter(AddDiscountCenterDto model);
        Task<ApiResult> ChangeDiscountCenterState(int centerId);
        Task<ApiResult> ChangeDiscountGroupState(int discountItemId);
        Task<ApiResult> ChangeDiscountState(int id);
        Task<ApiResult<PagedDiscountCardList>> PagedDiscountCardList(int cardTypeId, int pageNumber, int pageSize);
        Task<ApiResult<CardInfo_DiscountViewModel>> GetDiscount(int id);
        Task<ApiResult> Remove(int id);
        Task<ApiResult> RemoveDisCountItem(int discountItemId);
        Task<ApiResult> Update(CardInfo_DiscountDto model, int userId);
    }
    public class DiscountCardService : IDiscountCardService
    {

        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CardInfo_Discount> _discount;
        private readonly DbSet<CardInfo_Discount_Group> _discountGroups;
        private readonly DbSet<CardInfo_Discount_Center> _discountCenter;
        public DiscountCardService(IUnitOfWork uow )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _discount = _uow.Set<CardInfo_Discount>();
            _discountGroups = _uow.Set<CardInfo_Discount_Group>();
            _discountCenter = _uow.Set<CardInfo_Discount_Center>();
        }
        #endregion 
    
        
        

        public async Task<ApiResult> Add(CardInfo_DiscountDto model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            if(model==null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "مدل ورودی معتبر نمی باشد";
                return res;
            }

            if (model.GroupIds == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "حداقل یک گروه تخفیف انتخاب نمایید";
                return res;  
            }

            if (model.CenterDeliveryPossibility && model.CenterIds == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "حداقل یک  مرکز تحویل انتخاب نمایید";
                return res;
              
            }

            if (model.CenterDeliveryPossibility == false && model.PostDeliveryPossibility == false)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = " نوع تحویل را مشخص نمایید ";
                return res;


            }
            if (model.StartDate != null)
            {
                if (model.EndDate != null)
                    if (model.StartDate > model.EndDate)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        res.Messages = "تاریخ پایان باید بزرگتر از تاریخ شروع باشد";
                        return res;
                    }
                


            }

            var strDate = DateTime.Now.ToShortDateString();
            var date = DateTime.Parse(strDate);

            var preActiveGroups =await _discount.Where(w => w.CardTypeId == model.CardTypeId && date >= w.StartDate && date <= w.EndDate)
              .Select(s => s.CardInfo_Discount_Groups.Select(g => g.GroupId)).ToListAsync();

            if (preActiveGroups.Contains(model.GroupIds))
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "گروه تخفیف شامل گروه تکراری می باشد";
                return res;
            }


            try
            {
                 
                    var mappingModel = new CardInfo_Discount()
                    {

                        CardTypeId = model.CardTypeId,
                        DiscountIsActive = model.DiscountIsActive,
                        DiscountTitle = model.DiscountTitle,
                        EndDate = model.EndDate,
                        OperationId = userId,
                        StartDate = model.StartDate,
                        CreationDate = DateTime.Now,
                        CenterDeliveryPossibility = model.CenterDeliveryPossibility,
                        PostDeliveryPossibility = model.PostDeliveryPossibility,
                        Description = model.Description,
                        AttachmentGroup = model.AttachmentGroup,
                        DiscountPercent = model.DiscountPercent ,
                        PostalPercentInCity = model.PostalPercentInCity ,
                        PostalPercentOutCity = model.PostalPercentOutCity ,

                    };
                    await _discount.AddAsync(mappingModel);
                    if (model.GroupIds != null)
                    {
                        foreach (var grp in model.GroupIds)
                        {
                            _discountGroups.Add(new  CardInfo_Discount_Group()
                            {
                                GroupId = grp,
                                Discount = mappingModel, 
                                DiscountGroupIsActive = true,
                                
                            });
                        }
                    }
                    if (model.CenterIds != null)
                    {
                        foreach (var center in model.CenterIds)
                        {
                            _discountCenter.Add(new  CardInfo_Discount_Center()
                            {
                                CenterID = center,
                                Discount = mappingModel,
                                CenterIsActive = true
                            });
                        }
                    } 
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


        public async Task<ApiResult> Update(CardInfo_DiscountDto model, int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"ویرایش تخفیف با موفقیت انجام گردید");
            try
            {


                var item = await _discount.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "تخفیفی یافت نشد";
                    return res;
                }



                if (model.CenterDeliveryPossibility == false && model.PostDeliveryPossibility == false)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = " نوع تحویل را مشخص نمایید ";
                    return res; 
                }

                if (model.StartDate >= model.EndDate)
                {
                    res.IsSuccess = false;
                    res.Messages = "تاریخ شروع دوره باید کوچکتر  از تاریخ پایان دوره   باشد.";
                    return res;

                }
                 


                item.LastUpdateByUserId = userId;
                item.DiscountTitle = model.DiscountTitle;
                item.DiscountIsActive = model.DiscountIsActive;






                _discount.Update(item);
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


        public async Task<ApiResult<PagedDiscountCardList>> PagedDiscountCardList(  int cardTypeId,  int pageNumber, int pageSize )
        {

            var res = new ApiResult<PagedDiscountCardList>(true, ApiResultStatusCode.Success, new PagedDiscountCardList());
            try
            {
                var offset = (pageNumber) * pageSize;
                var query = _discount.Where(w=>  w.CardTypeId== cardTypeId);

                
 
                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new CardInfo_DiscountViewModel()
                {
                    
                    Id = s.Id,
                    ByUser=s.Operation.Username,
                    CardTypeId=s.CardTypeId,
                    CardType=s.CardType.Title,
                    CreationDate=s.CreationDate,
                    ByUserId=s.OperationId,
                    EndDate=s.EndDate,
                    Description=s.Description,
                    DiscountIsActive=s.DiscountIsActive,
                    DiscountPercent=s.DiscountPercent,
                    CenterDeliveryPossibility=s.CenterDeliveryPossibility,
                    DiscountTitle=s.DiscountTitle,
                    StartDate=s.StartDate,
                    PostalPercentOutCity=s.PostalPercentOutCity,
                    PostalPercentInCity=s.PostalPercentInCity,
                    PostDeliveryPossibility=s.PostDeliveryPossibility,
                    
                     


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


        public async Task<ApiResult<CardInfo_DiscountViewModel>> GetDiscount(int id    )
        {

            var res = new ApiResult<CardInfo_DiscountViewModel>(true, ApiResultStatusCode.Success, new CardInfo_DiscountViewModel());
            try
            {
              
                var query = _discount.Where(w => w.Id == id);




                var data = await query.Select(s => new CardInfo_DiscountViewModel()
                {

                    Id = s.Id,
                    ByUser = s.Operation.Username,
                    CardTypeId = s.CardTypeId,
                    CardType = s.CardType.Title,
                    CreationDate = s.CreationDate,
                    ByUserId = s.OperationId,
                    EndDate = s.EndDate,
                    Description = s.Description,
                    DiscountIsActive = s.DiscountIsActive,
                    DiscountPercent = s.DiscountPercent,
                    CenterDeliveryPossibility = s.CenterDeliveryPossibility,
                    DiscountTitle = s.DiscountTitle,
                    StartDate = s.StartDate,
                    PostalPercentOutCity = s.PostalPercentOutCity,
                    PostalPercentInCity = s.PostalPercentInCity,
                    PostDeliveryPossibility = s.PostDeliveryPossibility,




                }).FirstOrDefaultAsync();
                if(data==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تخفیفی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                 
                var groupList = await _discountGroups.Include(i=>i.Group).Where(w => w.DiscountId == id).ToListAsync();
                if (groupList.Any())
                {
                    data.GroupIds = groupList.Select(s => s.GroupId).ToList();
                    data.Groups = groupList.Select(s => new BaseDataModel { Text = s.Group.GroupName, Key = s.GroupId.ToString() }).AsEnumerable();

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


        public async Task<ApiResult> AddCenter(AddDiscountCenterDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");

            try
            {

                //چک کن قبلا اضافه نشده باشه

                if (await _discountCenter.AnyAsync(a=>a.CenterID== model.CenterId && a.DiscountId== model.DiscountId))
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "قبلا اضافه شده است";
                    return res;



                }

                await _discountCenter.AddAsync(new CardInfo_Discount_Center()
                {
                    CenterID = model.CenterId,
                    DiscountId = model.DiscountId,
                    CenterIsActive = true
                });


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
       
        
        
        public async Task<ApiResult> Remove(int id )
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item =await _discount.FirstOrDefaultAsync(w=>w.Id== id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "تخفیفی یافت نشد";
                    return res;

                }
                item.IsDeleted = true;
                _discount.Update(item);
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
        public async Task<ApiResult> RemoveDisCountItem(int discountItemId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item = await _discountGroups.FirstOrDefaultAsync(w => w.Id == discountItemId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "'گروه تخفیفی یافت نشد";
                    return res;

                }
                
                _discountGroups.Remove(item);
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
        public async Task<ApiResult> ChangeDiscountGroupState(int discountItemId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item = await _discountGroups.FirstOrDefaultAsync(w => w.Id == discountItemId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "'گروه تخفیفی یافت نشد";
                    return res;

                }
                var state = item.DiscountGroupIsActive;
                item.DiscountGroupIsActive = !state; 
                _discountGroups.Update(item);
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
        public async Task<ApiResult> ChangeDiscountState(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item = await _discount.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "'گروه تخفیفی یافت نشد";
                    return res;

                }
                var state = item.DiscountIsActive;
                item.DiscountIsActive = !state;
                _discount.Update(item);
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
        public async Task<ApiResult> ChangeDiscountCenterState(int centerId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item = await _discountCenter.FirstOrDefaultAsync(w => w.Id == centerId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "'گروه تخفیفی یافت نشد";
                    return res;

                }
                var state = item.CenterIsActive;
                item.CenterIsActive = !state;
                _discountCenter.Update(item);
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

         



    }
}
