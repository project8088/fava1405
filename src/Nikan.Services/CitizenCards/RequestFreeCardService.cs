using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.DomainClasses.Factor;
using Nikan.ViewModel;
using Nikan.ViewModel.CitizenCards;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.CitizenCards
{
    public interface IRequestFreeCardService
    {
        Task<ApiResult> AcceptedRequestFreeCard(string id, int userId);
        Task<ApiResult> Add(RequestFreeCardDto model, int userId);
        Task<ApiResult<RequestFreeCardInfoViewModel>> GetRequestFreeCard(string id);
        Task<ApiResult<PagedRequestFreeCardCitizens>> GetRequestFreeCardCitizens(int pageNumber, int pageSize, string id, string nationCode = null);
        Task<ApiResult<PagedRequestFreeCard>> PagedRequestFreeCardLsit(int pageNumber, int pageSize, int? cardTypeId = null, string discountTitle = null, int? groupId = null, string letterNumber = null);
        Task<ApiResult> RemoveRequestFreeCard(string id);
        Task<ApiResult> Update(RequestFreeCardDto model, int userId);
    }
    public class RequestFreeCardService : IRequestFreeCardService
    {

        #region Ctor
        private readonly IUnitOfWork _uow;
      
        private readonly DbSet<CardInfo_RequestFreeCard> _requestFreeCard;
        private readonly DbSet<CardInfo_RequestFreeCard_Citizens> _requestFreeCardCitizens;
        private readonly DbSet<GroupsCitizens> _groupCitizen;
        private readonly DbSet<CitizensCard> _citizensCard;
        private readonly DbSet<Group> _group;
        private readonly DbSet<CardInfo> _cardInfo;


        private readonly DbSet<CardInfo_Discount> _discount;
        private readonly DbSet<CardInfo_Discount_Group> _discountGroups;
        private readonly DbSet<CardInfo_Discount_Center> _discountCenter;



        public RequestFreeCardService(IUnitOfWork uow )
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _requestFreeCard = _uow.Set<CardInfo_RequestFreeCard>();
            _requestFreeCardCitizens = _uow.Set<CardInfo_RequestFreeCard_Citizens>();
            _groupCitizen = _uow.Set<GroupsCitizens>();
            _group = _uow.Set<Group>();
            _cardInfo = _uow.Set<CardInfo>();
            _citizensCard = _uow.Set<CitizensCard>();


            _discount = _uow.Set<CardInfo_Discount>();
            _discountGroups = _uow.Set<CardInfo_Discount_Group>();
            _discountCenter = _uow.Set<CardInfo_Discount_Center>();


        }
        #endregion



        public async Task<ApiResult> Add(RequestFreeCardDto model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            if (model == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "مدل ورودی معتبر نمی باشد";
                return res;
            }
            if (model.CardTypeId == 0)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = " نوع کارت را مشخص نمایید ";
                return res;
            }
            if (model.GroupId == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "گروه شهروندی را مشخص نمائید";
                return res;
            }

            if (model.DeliverType == Common.GlobalEnum.DeliverTypeEnum.تحویل_در_مرکز && model.CenterID == null)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = "   مرکز تحویل را انتخاب نمایید";
                return res;

            }






            try
            {


                var group = await _group.FirstOrDefaultAsync(w =>w.Id==model.GroupId && w.IsDeleted != true);
                if (group == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه شهروندی معتبر نمی باشد.";
                    return res;
                }
                if (group.CanBuyFreeCard != true)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "تنظیمات این گروه اجازه صدور کارت رایگان را نمی دهد";
                    return res;
                }


                /*
                //_______________________________شرایط صدور کارت رایگان__________________
                1- ایرانی باشد
                2- تایید هویت شده باشد
                3- قبلا کارت دیگری نداشته باشد

                */
                var citizenList = await _groupCitizen.Where(w =>
                 //w.Citizen.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید   &&
                   w.Citizen.NationId == 0 && w.GroupId == model.GroupId && w.IsDeleted != true).Select(s => new RequestFreeCardCitizensViewModel()
                  {

                      BirthDate = s.Citizen.BirthDate,
                      CitizenId = s.CitizenId,
                      UserCode = s.Citizen.UserCode,
                      CreationDate = s.Citizen.CreationDate,
                      Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                      Gender = s.Citizen.Gender,
                     
                      PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                      NationCode = s.Citizen.NationCode,
                      PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                      SabtStatus = s.Citizen.SabtStatus,
                      PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed == null ? true : false,
                  }).OrderByDescending(w => w.CitizenId).ToListAsync();

                if (model.ImagerReviewStatusFormFreeCard != Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.همه)
                {
                    if (model.ImagerReviewStatusFormFreeCard == Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.تاییده_شده)
                        citizenList = citizenList.Where(w => w.PersonalPicture_Confirmed == Common.GlobalEnum.PersonalPictureEnum.تایید_شده).ToList();
                    else
                        citizenList = citizenList.Where(w => w.PersonalPictureIsUploaded).ToList();

                }






                if (!citizenList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }


                var citizenIds = citizenList.Select(s => s.CitizenId).ToList();

                var listCard = await _citizensCard.Where(w => w.RequestStatuse != Common.GlobalEnum.CardRequestStatusEnum.درخواست_اولیه
               && citizenIds.Contains(w.CitizenId)).Select(s => s.CitizenId).ToListAsync();

                var newCardList = new List<CardInfo_RequestFreeCard_Citizens>();

                var id = Guid.NewGuid().ToString();


                var mappingModel = new CardInfo_RequestFreeCard()
                {

                    Id = id,
                    CardTypeId = model.CardTypeId,
                    DiscountTitle = model.DiscountTitle,
                    CreationDate = DateTime.Now,
                    Description = model.Description,
                    AttachmentGroup = model.AttachmentGroup,
                    CenterID = model.CenterID,
                    DeliverType = model.DeliverType,
                    GroupId = model.GroupId,
                    FreeCardApplicantOrganization = model.FreeCardApplicantOrganization,
                    ImagerReviewStatusFormFreeCard = model.ImagerReviewStatusFormFreeCard,
                    LetterNumber = model.LetterNumber,
                    CreationById = userId,



                };


                foreach (var item in citizenList)
                {
                    if (listCard.Contains(item.CitizenId)) continue;
                    newCardList.Add(new CardInfo_RequestFreeCard_Citizens()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CitizenId = item.CitizenId,
                        RequestFreeCardId = id
                    });
                }

                if (!newCardList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }

                await _requestFreeCard.AddAsync(mappingModel);
                _requestFreeCardCitizens.AddRange(newCardList);
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
        public async Task<ApiResult> Update(RequestFreeCardDto model, int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"ویرایش تخفیف با موفقیت انجام گردید");
            try
            {


                var item = await _requestFreeCard.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "درخواستی یافت نشد";
                    return res;
                }



                if (item.Accepted==true)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = " درخواست تایید شده است و امکان ویرایش وجود ندارد";
                    return res; 
                }

                if (item.Accepted == true)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = " درخواست تایید شده است و امکان ویرایش وجود ندارد";
                    return res;
                }

                var group = await _group.FirstOrDefaultAsync(w => w.Id == model.GroupId  && w.IsDeleted != true);
                if (group == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه شهروندی معتبر نمی باشد.";
                    return res;
                }
                if (group.CanBuyFreeCard != true)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "تنظیمات این گروه اجازه صدور کارت رایگان را نمی دهد";
                    return res;
                }

                /*
               //_______________________________شرایط صدور کارت رایگان__________________
               1- ایرانی باشد
               2- تایید هویت شده باشد
               3- قبلا کارت دیگری نداشته باشد

               */
                var citizenList = await _groupCitizen.Where(w =>
                //w.Citizen.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید && 
                  w.Citizen.NationId == 0 && w.GroupId == model.GroupId && w.IsDeleted != true).Select(s => new RequestFreeCardCitizensViewModel()
                  {

                      BirthDate = s.Citizen.BirthDate,
                      CitizenId = s.CitizenId,
                      UserCode = s.Citizen.UserCode,
                      CreationDate = s.Citizen.CreationDate,
                      Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                      Gender = s.Citizen.Gender,
                      
                      PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                      NationCode = s.Citizen.NationCode,
                      PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                      SabtStatus = s.Citizen.SabtStatus,
                      PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed == null ? true : false,
                  }).OrderByDescending(w => w.CitizenId).ToListAsync();

                if (model.ImagerReviewStatusFormFreeCard != Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.همه)
                {
                    if (model.ImagerReviewStatusFormFreeCard == Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.تاییده_شده)
                        citizenList = citizenList.Where(w => w.PersonalPicture_Confirmed == Common.GlobalEnum.PersonalPictureEnum.تایید_شده).ToList();
                    else
                        citizenList = citizenList.Where(w => w.PersonalPictureIsUploaded).ToList();

                }
                 

                if (!citizenList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }


                var citizenIds = citizenList.Select(s => s.CitizenId).ToList();

                var listCard = await _citizensCard.Where(w => w.RequestStatuse != Common.GlobalEnum.CardRequestStatusEnum.درخواست_اولیه
               && citizenIds.Contains(w.CitizenId)).Select(s => s.CitizenId).ToListAsync();

                var newCardList = new List<CardInfo_RequestFreeCard_Citizens>();

                foreach (var citizen in citizenList)
                {
                    if (listCard.Contains(citizen.CitizenId)) continue;
                    newCardList.Add(new CardInfo_RequestFreeCard_Citizens()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CitizenId = citizen.CitizenId,
                        RequestFreeCardId = model.Id
                    });
                }

                if (!newCardList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }

              





                item.LastUpdateByUserId = userId;
                item.DiscountTitle = model.DiscountTitle;
                item.CardTypeId = model.CardTypeId;
                item.Description = model.Description;
                item.AttachmentGroup = model.AttachmentGroup;
                item.CenterID = model.CenterID;
                item.DeliverType = model.DeliverType;
                item.GroupId = model.GroupId;
                item.FreeCardApplicantOrganization = model.FreeCardApplicantOrganization;
                item.ImagerReviewStatusFormFreeCard = model.ImagerReviewStatusFormFreeCard;
                item.LetterNumber = model.LetterNumber;


                //کارت های قبلی این درخواست حذف شود
                var prerequestFreeCardCitizens =await  _requestFreeCardCitizens.Where(w => w.RequestFreeCardId == model.Id).ToListAsync();
                _requestFreeCardCitizens.RemoveRange(newCardList);


                await _requestFreeCardCitizens.AddRangeAsync(newCardList);
                await _uow.SaveChangesAsync();




                _requestFreeCard.Update(item);
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
        public async Task<ApiResult<PagedRequestFreeCard>> PagedRequestFreeCardLsit(    int pageNumber, int pageSize,
            int? cardTypeId=null,string discountTitle=null,int? groupId=null,string letterNumber=null)
        {

            var res = new ApiResult<PagedRequestFreeCard>(true, ApiResultStatusCode.Success, new PagedRequestFreeCard());
            try
            {
                var offset = (pageNumber) * pageSize;
                var query = _requestFreeCard.AsQueryable();

                
 
                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new RequestFreeCardInfoViewModel()
                {
                    
                    Id = s.Id, 
                    CardTypeId=s.CardTypeId,
                    CardType=s.CardType.Title,
                    CreationDate=s.CreationDate,
                    AcceptedById=s.AcceptedById, 
                    Description=s.Description, 
                    DiscountTitle=s.DiscountTitle,
                    AttachmentGroup=s.AttachmentGroup,
                    CenterID=s.CenterID,
                    Center=s.Center==null ? "" :s.Center.Name,
                    Accepted=s.Accepted,
                    AcceptedBy=s.AcceptedBy==null ? "":s.AcceptedBy.Username,
                    CreationBy = s.CreationBy == null ? "" : s.CreationBy.Username,
                    FreeCardApplicantOrganization=s.FreeCardApplicantOrganization,
                    GroupId=s.GroupId,
                    Group=s.Group.GroupName,
                    DeliverType=s.DeliverType,
                    CreationById=s.CreationById,
                    LetterNumber=s.LetterNumber,
                    ImagerReviewStatusFormFreeCard=s.ImagerReviewStatusFormFreeCard,
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
      
        
        public async Task<ApiResult<RequestFreeCardInfoViewModel>> GetRequestFreeCard(string id    )
        {

            var res = new ApiResult<RequestFreeCardInfoViewModel>(true, ApiResultStatusCode.Success, new RequestFreeCardInfoViewModel());
            try
            {
              
                var query = _requestFreeCard.Where(w => w.Id == id);




                var data = await query.Select(s => new RequestFreeCardInfoViewModel()
                {

                    Id = s.Id,
                    CardTypeId = s.CardTypeId,
                    CardType = s.CardType.Title,
                    CreationDate = s.CreationDate,
                    AcceptedById = s.AcceptedById,
                    Description = s.Description,
                    DiscountTitle = s.DiscountTitle,
                    AttachmentGroup = s.AttachmentGroup,
                    CenterID = s.CenterID,
                    Center = s.Center == null ? "" : s.Center.Name,
                    Accepted = s.Accepted,
                    AcceptedBy = s.AcceptedBy == null ? "" : s.AcceptedBy.Username,
                    CreationBy = s.CreationBy == null ? "" : s.CreationBy.Username,
                    FreeCardApplicantOrganization = s.FreeCardApplicantOrganization,
                    GroupId = s.GroupId,
                    Group = s.Group.GroupName,
                    DeliverType = s.DeliverType,
                    CreationById = s.CreationById,
                    LetterNumber = s.LetterNumber,
                    ImagerReviewStatusFormFreeCard = s.ImagerReviewStatusFormFreeCard,

                }).FirstOrDefaultAsync();
                if(data==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "درخواستی یافت نشد";
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

        public async Task<ApiResult<PagedRequestFreeCardCitizens>> GetRequestFreeCardCitizens(int pageNumber, int pageSize,   
            string id, string nationCode = null )
        {

            var res = new ApiResult<PagedRequestFreeCardCitizens>(true, ApiResultStatusCode.Success, new PagedRequestFreeCardCitizens());
            try
            {

               

                var offset = (pageNumber) * pageSize;
                var query = _requestFreeCardCitizens.Where(w => w.RequestFreeCardId == id);



                res.Data.TotalItems = await query.CountAsync();  
                res.Data.Items = await query.Select(s => new RequestFreeCardCitizensViewModel()
                {
                    Id=s.Id,
                    BirthDate = s.Citizen.BirthDate,
                    CitizenId = s.CitizenId.Value,
                    UserCode = s.Citizen.UserCode,
                    CreationDate = s.Citizen.CreationDate,
                    Citizen = s.Citizen.FirstName +" " + s.Citizen.LastName,
                    Gender = s.Citizen.Gender,
                    
                    PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                    NationCode = s.Citizen.NationCode,
                    PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                    SabtStatus = s.Citizen.SabtStatus,
                    PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed == null ? true : false,

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



        public async Task<ApiResult> RemoveRequestFreeCard(string  id )
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item =await _requestFreeCard.FirstOrDefaultAsync(w=>w.Id== id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "تخفیفی یافت نشد";
                    return res;

                }
                if (item.Accepted==true)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "این درخواست تایید شده است و امکان حذف وجود ندارد";
                    return res;

                }

                var cardList =await _requestFreeCardCitizens.Where(w => w.RequestFreeCardId == id).ToListAsync();
                _requestFreeCardCitizens.RemoveRange(cardList);
                _requestFreeCard.Remove(item); 
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
        public async Task<ApiResult> AcceptedRequestFreeCard(string id,int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "عملیات با موفقیت صورت گرفت");
            try
            {
                var item = await _requestFreeCard.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " گروه تخفیفی یافت نشد";
                    return res;

                }

                var cardInfo = await _cardInfo.FirstOrDefaultAsync(w => w.CardIsActive && w.CardTypeId == item.CardTypeId);

                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " کارت فعالی جهت صدور یافت نشد ";
                    return res;

                }


                var citizenList = await _groupCitizen.Where(w => 
                //w.Citizen.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید  && 
                w.Citizen.NationId == 0 && w.GroupId == item.GroupId && w.IsDeleted != true).Select(s => new RequestFreeCardCitizensViewModel()
                 {

                     BirthDate = s.Citizen.BirthDate,
                     CitizenId = s.CitizenId,
                     UserCode = s.Citizen.UserCode,
                     CreationDate = s.Citizen.CreationDate,
                     Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                     Gender = s.Citizen.Gender,

                     PersonalPictureUrl = s.Citizen.PersonalPicture_Confirmed == null ? "" : "/uploads/Resources/Citizens/" + s.Citizen.UserCode + "/" + s.Citizen.NationCode + ".jpg",
                     NationCode = s.Citizen.NationCode,
                     PersonalPicture_Confirmed = s.Citizen.PersonalPicture_Confirmed,
                     SabtStatus = s.Citizen.SabtStatus,
                     PersonalPictureIsUploaded = s.Citizen.PersonalPicture_Confirmed == null ? true : false,
                 }).OrderByDescending(w => w.CitizenId).ToListAsync();

                if (item.ImagerReviewStatusFormFreeCard != Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.همه)
                {
                    if (item.ImagerReviewStatusFormFreeCard == Common.GlobalEnum.ImagerReviewStatusFormFreeCardEnum.تاییده_شده)
                        citizenList = citizenList.Where(w => w.PersonalPicture_Confirmed == Common.GlobalEnum.PersonalPictureEnum.تایید_شده).ToList();
                    else
                        citizenList = citizenList.Where(w => w.PersonalPictureIsUploaded).ToList();

                }


                if (!citizenList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }

                //یک تخفیف 100 درصدی تعریف کن


                var discountInfo = new CardInfo_Discount()
                {

                    CardTypeId = cardInfo.CardTypeId,
                    DiscountIsActive = false,
                    DiscountTitle = item.DiscountTitle,
                    EndDate =DateTime.Now,
                    OperationId = userId,
                    StartDate =   DateTime.Now,
                    CreationDate = DateTime.Now,
                    CenterDeliveryPossibility = item.DeliverType==DeliverTypeEnum.تحویل_در_مرکز,
                    PostDeliveryPossibility = item.DeliverType == DeliverTypeEnum.پستی,
                    Description = item.Description, 
                    DiscountPercent = 100,
                    PostalPercentInCity = 100,
                    PostalPercentOutCity = 100,

                };

                await _discount.AddAsync(discountInfo);

                var discountGroup = new CardInfo_Discount_Group()
                {
                    GroupId = item.GroupId.Value,
                    Discount = discountInfo,
                    DiscountGroupIsActive = true,
                };


                await _discountGroups.AddAsync(discountGroup);

                if (item.CenterID != null)
                {
                    await _discountCenter.AddAsync(new CardInfo_Discount_Center()
                    {
                        CenterID = item.CenterID,
                        Discount = discountInfo,
                        CenterIsActive = true
                    });
                }



                var citizenIds = citizenList.Select(s => s.CitizenId).ToList(); 

                var listCard = await _citizensCard.Where(w => w.RequestStatuse != Common.GlobalEnum.CardRequestStatusEnum.درخواست_اولیه
               && citizenIds.Contains(w.CitizenId)).Select(s => s.CitizenId).ToListAsync();

                var newCardList = new List<CardInfo_RequestFreeCard_Citizens>();

                foreach (var citizen in citizenList)
                {
                    if (listCard.Contains(citizen.CitizenId)) continue;
                    newCardList.Add(new CardInfo_RequestFreeCard_Citizens()
                    {
                        Id=Guid.NewGuid().ToString(),
                        CitizenId = citizen.CitizenId,
                        RequestFreeCardId = item.Id
                    });
                }

                if (!newCardList.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "گروه فاقد شهروند واجد شرایط دریافت کارت می باشد";
                    return res;
                }







               


                //کارت های قبلی این درخواست حذف شود
                var prerequestFreeCardCitizens = await _requestFreeCardCitizens.Where(w => w.RequestFreeCardId == id).ToListAsync();
                _requestFreeCardCitizens.RemoveRange(newCardList);  
                await _requestFreeCardCitizens.AddRangeAsync(newCardList);




                foreach (var card in newCardList)
                {
                    var citizensCard = new CitizensCard()
                    {

                        AdminDescription = " صدور کار ت رایگان ",
                        CardInfoId = cardInfo.CardInfoId,
                        CardRequestType = CardRequestTypeEnum.درخواست_جدید,
                        CitizenId = card.CitizenId.Value,
                        DeliveringCenterId=item.CenterID,
                        DeliverType = DeliverTypeEnum.پستی,
                        RequestByCitizenId = userId,
                        RequestCode = card.Id,
                        RequestStatuse = CardRequestStatusEnum.درخواست_جدید,
                        RequestDate = DateTime.Now,
                        DiscountGroup = discountGroup,
                        
                    };
                    await _citizensCard.AddAsync(citizensCard);

                }
               

                //if (await _group.AnyAsync(a => a.Id == 246))
                //{
                //    if (await _groupCitizens.AnyAsync(a => a.Id == 246 && a.CitizenId == model.CitizenId))
                //    {
                //        await _groupCitizens.AddAsync(new GroupsCitizens()
                //        {
                //            AddByUserId = 1,
                //            CitizenId = model.CitizenId,
                //            GroupId = 246,
                //            CreationDate = DateTime.Now,
                //        });

                //    }

                //}

               // var add = await _citizensCard.AddAsync(card);



                 

                item.Accepted =true;
                item.AcceptedById = userId;

                _requestFreeCard.Update(item);
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
