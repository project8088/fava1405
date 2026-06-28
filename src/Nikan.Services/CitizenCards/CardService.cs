using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.ViewModel;
using Nikan.ViewModel.CitizenCards;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



namespace Nikan.Services.CitizenCards
{
    public interface ICardService
    {
        Task<ApiResult<CardInfoDto>> AddOrUpdate(CardInfoDto model, int userId);

        /// <summary>
        /// آیا شهروند می تواند خرید کارت داشته باشد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<string>> CheckCanOrderCard(CheckCanOrderCardDto model); 

        /// <summary>
        /// دریافت اطلاعات کامل یک کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<CardInfoViewModel>> GetCardInfo(string id);
        Task<ApiResult<CardInfoViewModel>> GetCardInfoForBuy(string id, int citizenId);
        Task<ApiResult<List<BaseDataModel>>> GetCardTypeBaseList(bool? isActive = null);



        /// <summary>
        /// دریافت نوع کارت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<CardTypeViewModel>> GetCardTypeInfo(int id);
        Task<ApiResult<List<BaseDataModel>>> GetDisCountGroupBaseList();
        Task<ApiResult<PagedCardInfoViewModel>> GetPagedCardInfo(int pageNumber, int pageSize, int? cardTypeId = null, DateTime? FromDate = null, DateTime? ToDate = null);



        /// <summary>
        /// لیست کارتهای موجود برای خرید 
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<CardInfoViewModel>>> ListAvailableCards();
    }


    public class CardService: ICardService
    {

        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<CitizenProfile> _profile;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<User> _users;
        private readonly DbSet<CitizensCard> _citizensCard;
        private readonly DbSet<CardInfo_PermissionsForGroups> _cardInfo_PermissionsForGroups;
        private readonly DbSet<Group> _group;
        private readonly DbSet<GroupsCitizens> _groupCitizens;
        private readonly DbSet<CardInfo> _cardInfo;
        private readonly DbSet<CardType> _cardType;



        private readonly DbSet<OrganizationalUnit> _organizationalUnit;



        private readonly DbSet<CardInfo_Discount> _discount;
        private readonly DbSet<CardInfo_Discount_Group> _discountGroups;
        private readonly DbSet<CardInfo_Discount_Center> _discountCenter;
        private readonly DbSet<Event> _event;


        public CardService(IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _profile = _uow.Set<CitizenProfile>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _citizensCard = _uow.Set<CitizensCard>();
            _cardInfo_PermissionsForGroups = _uow.Set<CardInfo_PermissionsForGroups>();
            _group = _uow.Set<Group>();
            _groupCitizens = _uow.Set<GroupsCitizens>();
            _cardInfo = _uow.Set<CardInfo>();
            _cardType = _uow.Set<CardType>();

            _organizationalUnit = _uow.Set<OrganizationalUnit>();

            _event = _uow.Set<Event>();

            _discount = _uow.Set<CardInfo_Discount>();
            _discountGroups = _uow.Set<CardInfo_Discount_Group>();
            _discountCenter = _uow.Set<CardInfo_Discount_Center>();


        }

        #endregion


 
        
        public async Task<ApiResult<List<BaseDataModel>>> GetDisCountGroupBaseList()
        {
            var  res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                
                var list = await this._discountGroups.OrderByDescending(o => o.Id).Select(s => new BaseDataModel()
                {  
                    Text=s.Discount.DiscountTitle  ,
                    Key=s.Id.ToString() 
               
                }).ToListAsync();
                res.Data = list;


            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }









        public async Task<ApiResult<CardInfoViewModel>> GetCardInfo(string  id)
        {
            var res = new ApiResult<CardInfoViewModel>(true, ApiResultStatusCode.Success, new CardInfoViewModel());
            try
            { 

                var data = await _cardInfo.Where(w => w.CardInfoId == id).Select(s => new CardInfoViewModel()
                {
                    AttachmentGroup=s.AttachmentGroup,
                   

                    CardInfoId=s.CardInfoId,
                    CardTypeId=s.CardTypeId,
                    BuyCardDescription=s.BuyCardDescription,
                    CardIsActive=s.CardIsActive,
                    CardType=s.CardType.Title,
                    CreationDate=s.CreationDate,
                    DoubleCardCost=s.DoubleCardCost,
                    ExpirationDate=s.ExpirationDate,
                    UserName=s.Operation.Username,
                    OperationId=s.OperationId,
                  
                    StartFromDate=s.StartFromDate,

                    CardCost = s.CardCost,
                    
                    VATForCardCost =s.VATForCardCost.PercentToInt(),
                    VATForPost=s.VATForPost.PercentToInt(),
                   
                    PostalCostInCity = s.PostalCostInCity,
                    PostalCostOutCity = s.PostalCostOutCity,

                }).FirstOrDefaultAsync();  
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }

                //هزینه کارت
                var tax = data.VATForCardCost.IntToPercent();
                var pricetax = Convert.ToInt32(data.CardCost * tax);
                var totalamount = pricetax + data.CardCost;
               
                
                //هزینه پست
                 var posttax = data.VATForPost.IntToPercent();
                 var postpricetax = Convert.ToInt32(data.PostalCostInCity * posttax);
                 var posttotalamount = postpricetax + data.PostalCostInCity;


                data.PostTotalAmount = posttotalamount;
                data.CardTotalAmount = totalamount;
                data.TotalAmount = totalamount+ posttotalamount;
                data.TotalStr = data.TotalAmount.ToCurrency();


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


        public async Task<ApiResult<CardInfoViewModel>> GetCardInfoForBuy(string id,int citizenId)
        {
            var res = new ApiResult<CardInfoViewModel>(true, ApiResultStatusCode.Success, new CardInfoViewModel());
            try
            {

                if(string.IsNullOrWhiteSpace(id))
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه کارت را مشخص نمایید";
                    return res;
                }




                var data = await _cardInfo.Where(w => w.CardInfoId == id).Select(s => new CardInfoViewModel()
                {
                    AttachmentGroup = s.AttachmentGroup, 
                    CardInfoId = s.CardInfoId,
                    CardTypeId = s.CardTypeId,
                    BuyCardDescription = s.BuyCardDescription,
                    CardIsActive = s.CardIsActive,
                    CardType = s.CardType.Title,
                    CreationDate = s.CreationDate,
                    DoubleCardCost = s.DoubleCardCost,
                    ExpirationDate = s.ExpirationDate,
                    UserName = s.Operation.Username,
                    OperationId = s.OperationId, 
                    StartFromDate = s.StartFromDate, 
                    CardCost = s.CardCost, 
                    VATForCardCost = s.VATForCardCost.PercentToInt(),
                    VATForPost = s.VATForPost.PercentToInt(), 
                    PostalCostInCity = s.PostalCostInCity,
                    PostalCostOutCity = s.PostalCostOutCity,

                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }

                int? CardDiscountPercent = 0;
                int? PostDiscountPercent = 0;
                var date = DateTime.Parse(DateTime.Now.ToShortDateString());

                var discountCards = await _discount.Include(i=>i.CardInfo_Discount_Groups).Where(w =>
                w.DiscountIsActive && w.IsDeleted != true && w.CardTypeId == data.CardTypeId
                && (w.StartDate == null && w.EndDate == null)
                    || (w.StartDate >= date && (w.EndDate <= date || w.EndDate == null))

                ).ToListAsync();
                if (discountCards.Any())
                {
                    var groupIdList = await _groupCitizens.Where(w => w.Group.IsDeleted != true && w.IsDeleted != true && w.CitizenId == citizenId).Select(s => s.GroupId).ToListAsync();
                    if (groupIdList.Any())
                    {
                        //شناسه تخفیف هایی که شامل تخفیف برای گروههای شهروندی می باشد
                        var discountIds = await _discountGroups.Where(w => w.Discount.IsDeleted != true
                          &&
                          w.Discount.DiscountIsActive
                          &&
                          w.Discount.CardTypeId == data.CardTypeId
                          && groupIdList.Contains(w.GroupId)
                        ).Select(s=>s.DiscountId).ToListAsync();

                        if(discountIds.Any())
                        {
                            //بدست آوردن رکورد تخفیف مربوط به این شهروند
                            //لیست تخفیف ها
                            var discountList  = discountCards.Where(w => discountIds.Contains(w.Id)).ToList(); 
                            if (discountList.Any())
                            {
                                //مرتب سازی براساس مبلغ تخفیف 
                                //انتخاب رکورد با بیشترین مبلغ تخفیف
                                var discount = discountList.OrderByDescending(o => o.DiscountPercent).FirstOrDefault();

                                CardDiscountPercent = discount.DiscountPercent;
                                PostDiscountPercent = discount.PostalPercentInCity;

                                var discountGroupList = discount.CardInfo_Discount_Groups.ToList();
                                //بدست آوردن شناسه گروه تخفیف
                                foreach (var dis in discountGroupList)
                                {
                                    var groupid = dis.GroupId;
                                    if (groupIdList.Contains(groupid))
                                    {
                                        data.DiscountGroupId = dis.Id;//شناسه گروه تخفیف  
                                        break;
                                    }
                                }

                                data.PostDeliveryPossibility = discount.PostDeliveryPossibility;
                                data.CenterDeliveryPossibility = discount.CenterDeliveryPossibility;
                                data.DiscountId = discount.Id;//شناسه تخفیف

                            }
                        }
                         
                    }
                }
                if (data.DiscountId == null)
                {
                    //اگر شامل هیچ تخفیف نبود فقط  امکان تحویل پستی وجود داشته باشد 
                    data.PostDeliveryPossibility = true;
                }

                if (data.CenterDeliveryPossibility)
                {
                    //اگر امکان تحویل حضوری وجود دارد مراکز تحویل را مشخص کن
                    //بدست آوردن مراکز تحویل پستی برای این شناسه تخفیف
                    var centerListId =await _discountCenter.Where(w => w.DiscountId == data.DiscountId && w.CenterIsActive).Select(s => s.CenterID).ToListAsync();
                  
                    //لیست مراکز تحویل
                    data.CenterList = _organizationalUnit.Where(w => centerListId.Contains(w.Id) && w.IsActive && w.IsDeleted != true).Select(s => new BaseDataModel()
                    {
                        Text = s.Name,
                        Key = s.Id.ToString(), 
                    }).ToList();

                }


                //هزینه کارت
                var tax = data.VATForCardCost.IntToPercent();
                var pricetax = Convert.ToInt32(data.CardCost * tax);
                var totalamount = pricetax + data.CardCost;


                //هزینه پست
                var posttax = data.VATForPost.IntToPercent();
                var postpricetax = Convert.ToInt32(data.PostalCostInCity * posttax);
                var posttotalamount = postpricetax + data.PostalCostInCity;


                var carddiscount = 0;
                var postdiscount = 0;
                if (CardDiscountPercent !=null && CardDiscountPercent!=0)
                {
                    var percent= CardDiscountPercent.Value.IntToPercent();
                    carddiscount = Convert.ToInt32(totalamount * percent);
                }
                 
                
                if (PostDiscountPercent != null && PostDiscountPercent != 0)
                {
                    var percent = PostDiscountPercent.Value.IntToPercent();
                    postdiscount = Convert.ToInt32(posttotalamount * percent);
                }

                if (carddiscount < 0)
                    carddiscount = 0;

                if (postdiscount < 0)
                    postdiscount = 0;



                data.CardDiscountAmount = carddiscount;
                data.PostageDiscountAmount = postdiscount;

                data.TotalDiscountAmount = carddiscount+ postdiscount;

                data.PostTotalAmount = posttotalamount;
                data.CardTotalAmount = totalamount;

                if (data.TotalDiscountAmount < 0)
                    data.TotalDiscountAmount = 0;

               


                data.TotalAmount = totalamount + posttotalamount - data.TotalDiscountAmount;
                data.TotalStr = data.TotalAmount.ToCurrency();


                res.Data = data;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
                var st = new StackTrace(er, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber().ToString();
                var fileName = frame.GetFileName();
                var methodName = frame.GetMethod().Name;
               await  _event.AddAsync(new Event()
                {
                    Description="خطا در خرید کارت" +" در خط  "+ line , 
                    CreateDate=DateTime.Now,
                    ActionName= methodName,
                    EventPriority=EventPriorityEnum.Necessary,
                    EventType=EventTypeEnum.Error,
                    OperationId=citizenId,
                    StrCode= id,
                    UserId= citizenId,
                    EventSection=EventSectionEnum.خطای_سیستمی,
                    

               });
                await _uow.SaveChangesAsync();

            }

            return res;
        }

        public async Task<ApiResult<CardInfoViewModel>> BuyFreeCard(string id, int groupId)
        {
            var res = new ApiResult<CardInfoViewModel>(true, ApiResultStatusCode.Success, new CardInfoViewModel());
            try
            {

                if (string.IsNullOrWhiteSpace(id))
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه کارت را مشخص نمایید";
                    return res;
                }




                var data = await _cardInfo.Where(w => w.CardInfoId == id).Select(s => new CardInfoViewModel()
                {
                    AttachmentGroup = s.AttachmentGroup,
                    CardInfoId = s.CardInfoId,
                    CardTypeId = s.CardTypeId,
                    BuyCardDescription = s.BuyCardDescription,
                    CardIsActive = s.CardIsActive,
                    CardType = s.CardType.Title,
                    CreationDate = s.CreationDate,
                    DoubleCardCost = s.DoubleCardCost,
                    ExpirationDate = s.ExpirationDate,
                    UserName = s.Operation.Username,
                    OperationId = s.OperationId,
                    StartFromDate = s.StartFromDate,
                    CardCost = s.CardCost,
                    VATForCardCost = s.VATForCardCost.PercentToInt(),
                    VATForPost = s.VATForPost.PercentToInt(),
                    PostalCostInCity = s.PostalCostInCity,
                    PostalCostOutCity = s.PostalCostOutCity,
                    

                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی  یافت نشد";
                    return res;
                }

                //چک کن گروه تیک کارت رایگان را داشته باشد

                var groupInfo = await _group.Include(g=>g.GroupsCitizens).FirstOrDefaultAsync(w => w.IsDeleted != true  && w.Id == groupId);
                if (!groupInfo.GroupsCitizens.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = "گروه انتخابی فاقد شهروند می باشد";
                    return res;
                }

                if (groupInfo.CanBuyFreeCard!=true)
                {
                    res.IsSuccess = false;
                    res.Messages = "این گروه برای صدور کارت رایگان تنظیم نشده است";
                    return res;
                }




                //هزینه کارت
                var tax = data.VATForCardCost.IntToPercent();
                var pricetax = Convert.ToInt32(data.CardCost * tax);
                var totalamount = pricetax + data.CardCost;


                //هزینه پست
                var posttax = data.VATForPost.IntToPercent();
                var postpricetax = Convert.ToInt32(data.PostalCostInCity * posttax);
                var posttotalamount = postpricetax + data.PostalCostInCity;


                var carddiscount = 0;
                var postdiscount = 0;
                //if (CardDiscountPercent != null && CardDiscountPercent != 0)
                //{
                //    var percent = CardDiscountPercent.Value.IntToPercent();
                //    carddiscount = Convert.ToInt32(totalamount * percent);
                //}


                //if (PostDiscountPercent != null && PostDiscountPercent != 0)
                //{
                //    var percent = PostDiscountPercent.Value.IntToPercent();
                //    postdiscount = Convert.ToInt32(posttotalamount * percent);
                //}

                if (carddiscount < 0)
                    carddiscount = 0;

                if (postdiscount < 0)
                    postdiscount = 0;



                data.CardDiscountAmount = carddiscount;
                data.PostageDiscountAmount = postdiscount;

                data.TotalDiscountAmount = carddiscount + postdiscount;

                data.PostTotalAmount = posttotalamount;
                data.CardTotalAmount = totalamount;

                if (data.TotalDiscountAmount < 0)
                    data.TotalDiscountAmount = 0;




                data.TotalAmount = totalamount + posttotalamount - data.TotalDiscountAmount;
                data.TotalStr = data.TotalAmount.ToCurrency();


                res.Data = data;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
                var st = new StackTrace(er, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber().ToString();
                var fileName = frame.GetFileName();
                var methodName = frame.GetMethod().Name;
                 

            }

            return res;
        }



        public async Task<ApiResult<CardTypeViewModel>> GetCardTypeInfo(int id)
        {
            var res = new ApiResult<CardTypeViewModel>(true, ApiResultStatusCode.Success, new CardTypeViewModel());
            try
            {

                var data = await _cardType.Where(w => w.Id  == id).Select(s => new CardTypeViewModel()
                {
                    CreationDate=s.CreationDate,
                    Description=s.Description,
                    Id=s.Id,
                    ExportQuery=s.ExportQuery,
                    ImageUrl=s.ImageUrl,
                    IsActive=s.IsActive,
                    LastUpdateDate=s.LastUpdateDate, 
                    Title=s.Title,
                    ViewIcon=s.ViewIcon,
                    ViewOrder=s.ViewOrder,
                    

                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
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



        public async Task<ApiResult<List<BaseDataModel>>> GetCardTypeBaseList(bool? isActive=null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {

                if(isActive.HasValue)
                {
                    res.Data = await _cardType.Where(w=>w.IsActive== isActive.Value)
                                      .OrderByDescending(o => o.ViewOrder).ThenBy(o => o.Title).Select
                                      (s => new BaseDataModel()
                                      {
                                          Text = s.Title,
                                          Key = s.Id.ToString(),
                                          Selected = false,

                                      })
                                      .ToListAsync();
                }
                else
                {

                    res.Data = await _cardType
                                      .OrderByDescending(o => o.ViewOrder).ThenBy(o => o.Title).Select
                                      (s => new BaseDataModel()
                                      {
                                          Text = s.Title,
                                          Key = s.Id.ToString(),
                                          Selected = false,

                                      })
                                      .ToListAsync();
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



        public async Task<ApiResult<string>> CheckCanOrderCard(CheckCanOrderCardDto model)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success,
                "امکان خرید کارت برای شما وجود دارد");
            /*
            1=آیا دارای کارت فعالی از این نوع است ؟
            2= آیا در گروه مجاز وجود دارد ؟
            3= آیا کارت دیگری خرید کرده ؟
             */
            try
            {

                var cardInfo = await _cardInfo.FirstOrDefaultAsync(w=>w.CardInfoId==model.CardInfoId);
                if(cardInfo==null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "کارتی یافت نشد";
                    return res;

                }
              //آیا تصویر پرسنلی بارگذاری شده است؟
                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId); 
                if (citizen.PersonalPicture_Confirmed == null)
                {
                    return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, 
                        "ابتدا تصویر پرسنلی خود را بارگذاری نمایید", "ابتدا تصویر پرسنلی خود را بارگذاری نمایید");
                }


                if (citizen.NationId != 0)
                {
                    //در صورتیکه اتباع ایرانی نبود امکان خرید کارت وجود ندارد
                    return new ApiResult<string>(false, ApiResultStatusCode.BadRequest,
                        "در حال حاضر خرید کارت برای شما امکان پذیر نمی باشد #اتباع");
                }



                //آیا شهروند در حال حاظر کارت فعالی از نوع کارت را دارد یا نه؟
                if (await _citizensCard.AnyAsync(w =>
                 w.CitizenId == model.CitizenId //مربوط به این شهروند
                 // && w.RequestStatuse  !=CardRequestStatusEnum.وضعیت_نامعلوم
                  && w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه
                ) )
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "در حال حاظر این کارت برای شما صادر شده است";
                    return res; 
                }
                //چک شود شهروند در گروه مجوز خرید این کارت هست یا نه

                //لیست گروههای شهروند
                var citizenGroupIds =await _groupCitizens.Where(w => w.CitizenId == model.CitizenId).Select(s => s.GroupId).ToListAsync();

           
               // لیست گروههای مجاز خرید کارت
                if (! await _cardInfo_PermissionsForGroups.AnyAsync(w =>
                w.CardTypeId == cardInfo.CardTypeId   &&
                 citizenGroupIds.Contains(w.PermissionGroupId)))
                {
                    //این شهروند به دلیل اینکه در گروههای مجاز خرید کارت نیست امکان خرید کارت را ندارد
                    //قبلا کارتی از این نوع داشته 
                     
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شما مجوز درخواست این کارت را ندارید";
                    return res; 
                }
                //چک شود که شهروند آیا دارای کارت دیگری هست ؟
                
              //  //که امکان خرید این کارت را نداشته باشد
              //  var cardTypeIdList =await _citizensCard.Where(w => w.CardInfoId == model.CardInfoId
              //  && w.CitizenId == model.CitizenId //کارت های مربوط به است شهروند
              // // && w.RequestStatuse !=CardRequestStatusEnum.وضعیت_نامعلوم //کارت درخواست داده باشد

              //  ).Select(s => s.CardInfo.CardTypeId).ToListAsync();

              //  if ( await _cardInfo_PermissionsForGroups.AnyAsync(w => 
              ////  w.CardTypeId == cardInfo.CardTypeId && TODO
              // cardTypeIdList.Contains(w.CardTypeId)) ) 
              //  {

              //      res.IsSuccess = false;
              //      res.StatusCode = ApiResultStatusCode.ServerError;
              //      res.Messages = "شما مجوز درخواست این کارت را ندارد";
              //      return res; 

              //  } 
                 
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


        public async Task<ApiResult<List<CardInfoViewModel>>> ListAvailableCards()
        {
            var res = new ApiResult<List<CardInfoViewModel>>(true, ApiResultStatusCode.Success, new List<CardInfoViewModel>());
            try
            {


                 
                var data = await _cardInfo.Where(w => w.CardType.IsActive  && w.CardIsActive ).Select(s => new CardInfoViewModel()
                {
                    AttachmentGroup=s.AttachmentGroup,
                    BuyCardDescription=s.BuyCardDescription,
                    CardCost=s.CardCost,
                    CardIsActive=s.CardIsActive,
                    CardType=s.CardType.Title,
                    CardInfoId=s.CardInfoId,
                    CreationDate=s.CreationDate,
                    CardTypeId=s.CardTypeId,
                    DoubleCardCost=s.DoubleCardCost,
                    ExpirationDate=s.ExpirationDate,
                    PostalCostInCity=s.PostalCostInCity,
                    PostalCostOutCity=s.PostalCostOutCity,
                    VATForCardCost=s.VATForCardCost.PercentToInt(),
                    VATForPost=s.VATForPost.PercentToInt(),
                    OperationId =s.OperationId, 
                }).ToListAsync();
                
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





        public async Task<ApiResult<CardInfoDto>> AddOrUpdate(CardInfoDto model, int userId)
        {

            var res = new ApiResult<CardInfoDto>(true, ApiResultStatusCode.Success, new CardInfoDto());


            if (model  == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = " مدل ورودی معتبر نمی باشد";
                return res;
            }


            if (model.CardTypeId == 0)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = " نوع کارت را مشخص نمایید ";
                return res;
            }
            
            
            try
            {
                if(string.IsNullOrWhiteSpace(model.CardInfoId))
                {
                    var add = new CardInfo()
                    {
                        CardInfoId = Guid.NewGuid().ToString(), 
                        AttachmentGroup = Guid.NewGuid().ToString(),
                        BuyCardDescription= model.BuyCardDescription,
                        CardCost =model.CardCost,
                        CardIsActive=model.CardIsActive, 
                        CardTypeId=model.CardTypeId,
                        CreationDate=DateTime.Now,
                        DoubleCardCost=model.DoubleCardCost,
                        OperationId= userId,
                        PostalCostInCity=model.PostalCostInCity,
                        PostalCostOutCity=model.PostalCostOutCity,
                        VATForCardCost=model.VATForCardCost.IntToPercent(),
                        VATForPost = model.VATForPost.IntToPercent(), 

                    };
                    await _cardInfo.AddAsync(add);
                    await _uow.SaveChangesAsync(); 
                }
                else
                {
                    var carInfo = await _cardInfo.FirstOrDefaultAsync(w => w.CardInfoId == model.CardInfoId);
                    if(carInfo == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "کارتی یافت نشد";
                        return res;
                    }


                    carInfo.BuyCardDescription = model.BuyCardDescription;
                    carInfo.CardCost = model.CardCost;
                    carInfo.CardIsActive = model.CardIsActive;
                    carInfo.CardTypeId = model.CardTypeId;
                    carInfo.DoubleCardCost = model.DoubleCardCost;
                    carInfo.PostalCostInCity = model.PostalCostInCity;
                    carInfo.PostalCostOutCity = model.PostalCostOutCity;
                    carInfo.PostalCostOutCity = model.PostalCostOutCity;
                    carInfo.VATForCardCost = model.VATForCardCost.IntToPercent();
                    carInfo.VATForPost = model.VATForPost.IntToPercent();
                     _cardInfo.Update(carInfo);
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


        public async Task<ApiResult<PagedCardInfoViewModel>> GetPagedCardInfo( 
            int pageNumber, int pageSize, 
            int? cardTypeId = null,
         DateTime? FromDate = null,
         DateTime? ToDate = null 
         )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedCardInfoViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {
                var cardTypeList =await _cardType.ToListAsync();
                var list = new List<CardInfoViewModel>();
                foreach (var cardtype in cardTypeList)
                {
                    //ببین برای این کارت قیمت گذاری وجود دارد یا نه ؟
                    var cardInfo =await _cardInfo.Where(w=>w.CardTypeId== cardtype.Id).OrderByDescending(o => o.CreationDate).FirstOrDefaultAsync();
                    if(cardInfo != null)
                    {
                        list.Add(new CardInfoViewModel()
                        {
                            AttachmentGroup = cardInfo.AttachmentGroup,
                            BuyCardDescription = cardInfo.BuyCardDescription,
                            CardCost = cardInfo.CardCost,
                            CardIsActive = cardInfo.CardIsActive,
                            CardType = cardInfo.CardType.Title,
                            CardInfoId = cardInfo.CardInfoId,
                            CreationDate = cardInfo.CreationDate,
                            CardTypeId = cardInfo.CardTypeId,
                            DoubleCardCost = cardInfo.DoubleCardCost,
                            ExpirationDate = cardInfo.ExpirationDate,
                            PostalCostInCity = cardInfo.PostalCostInCity,
                            PostalCostOutCity = cardInfo.PostalCostOutCity,
                            VATForCardCost = cardInfo.VATForCardCost.PercentToInt(),
                            VATForPost = cardInfo.VATForPost.PercentToInt(),
                            OperationId = cardInfo.OperationId,
                            SetPrice=true
                        });
                    }
                    else
                    {
                        list.Add(new CardInfoViewModel()
                        {
                            AttachmentGroup = Guid.NewGuid().ToString(),
                            BuyCardDescription = "",
                            CardCost = 0,
                            CardIsActive = true,
                            CardType = cardtype.Title,  
                            CardTypeId = cardtype.Id, 
                        });
                    }

                }

                   

                
                res.Data = new PagedCardInfoViewModel
                {
                    TotalItems =   list.Count,
                    Items = list
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



    }


}
