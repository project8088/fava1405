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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.CitizenCards
{



    public interface IDistributeCardService
    {
        Task<ApiResult<CardInfo_DistributeCard_CoursesDto>> AddCardCourses(CardInfo_DistributeCard_CoursesDto model, int userId);
        Task<ApiResult<CardDistributionQueueDto>> AddCardDistributionQueue(CardDistributionQueueDto model, int userId);
        Task<ApiResult<PrintqueueInfoViewModel>> AddCardToDeliveredQueue(int cardId, string centerId, int courseId, int operationId);
        Task<ApiResult<PrintqueueInfoViewModel>> AddCardToPostQueue(int operationId, int citizenId, int cardTypeId, int cardId, int courseId);
        Task<ApiResult<string>> ChangeLockDistributionQueue(int id);
        Task<ApiResult<string>> CloseCardCourses(int id);
        Task<ApiResult> DeliveryQueueToOperator(DeliveryQueueToOperatorViewModel model);
        Task<ApiResult<CardInfo_DistributeCard_QueueInfoViewModel>> GetDistributionQueueInfo(int id);
        Task<ApiResult<PagedCardInfoDistributeCardCoursesViewModel>> GetPagedeCardCourses(int pageNumber, int pageSize, int? courseNumber = null, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<ApiResult<PagedCardInfoDistributeCardQueue>> GetPagedeDistributionQueue(int pageNumber, int pageSize, int courseId, string name = null);
        Task<ApiResult<List<PrintForPostViewModel>>> GetPrintQueueForPostList(string guidId);
        Task<ApiResult<List<DistributeCardQueueShortInfoViewModel>>> GetQueueListInCourse(int courseId);
        Task<ApiResult<string>> RemoveCardCourses(int id);
        Task<ApiResult<string>> RemoveCardFromQueue(int id);
        Task<ApiResult<string>> RemoveCardFromQueueByCourseId(int courseId, int cardId);
        Task<ApiResult<string>> RemoveQueue(int id);
        Task<ApiResult<CitizensCardInfo>> SearchCardForQueue(MiniSearchCitizensViewModel model);
        Task<ApiResult<PagedCitizenCardsInQueueInfo>> SearchCardInQueue(int pageNumber, int pageSize,
            string name = null, string nationCode = null, 
            int? courseId = null,
             int? courseNumber = null, 
             int? queueId = null, string cardNumber = null, int? cardTypeId = null, QueueInputTypeEnum? queueInputType = null);
        Task<ApiResult<CardDistributionQueueDto>> UpdateCardDistributionQueue(CardDistributionQueueDto model, int userId);
    }
    public class DistributeCardService : IDistributeCardService
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
        private readonly DbSet<CardInfo_DistributeCard_QueueInfo> _distributeCard_QueueInfo;
        private readonly DbSet<CardInfo_DistributeCard_Courses> _cardInfo_DistributeCard_Courses;
        private readonly DbSet<CardInfo_DistributeCard_Queue_Groups> _cardInfo_DistributeCard_Queue_Groups;
        private readonly DbSet<CardInfo_DistributeCard> _cardInfo_DistributeCard;
        private readonly DbSet<GroupsCitizens> _citizenGroups;
        private readonly DbSet<OrganizationalUnit> _organizationalUnit;





        public DistributeCardService(IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _profile = _uow.Set<CitizenProfile>();
            _cardInfo_DistributeCard_Courses = _uow.Set<CardInfo_DistributeCard_Courses>();
            _SiteOptions = _uow.Set<SiteOption>();
            _distributeCard_QueueInfo = _uow.Set<CardInfo_DistributeCard_QueueInfo>();
            _users = _uow.Set<User>();
            _citizensCard = _uow.Set<CitizensCard>();
            _organizationalUnit = _uow.Set<OrganizationalUnit>();
            _cardInfo_PermissionsForGroups = _uow.Set<CardInfo_PermissionsForGroups>();
            _group = _uow.Set<Group>();
            _groupCitizens = _uow.Set<GroupsCitizens>();
            _cardInfo = _uow.Set<CardInfo>();
            _cardType = _uow.Set<CardType>();
            _cardInfo_DistributeCard = _uow.Set<CardInfo_DistributeCard>();
            _cardInfo_DistributeCard_Queue_Groups = _uow.Set<CardInfo_DistributeCard_Queue_Groups>();

        }

        #endregion 
        #region لیست دوره های توزیع کارت

        public async Task<ApiResult<PagedCardInfoDistributeCardCoursesViewModel>> GetPagedeCardCourses(
              int pageNumber, int pageSize,
              int? courseNumber = null,
           DateTime? FromDate = null,
           DateTime? ToDate = null
           )
        {

            var offset = (pageNumber) * pageSize;
            var res = new ApiResult<PagedCardInfoDistributeCardCoursesViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {

                var query = _cardInfo_DistributeCard_Courses.AsQueryable();
                if (courseNumber.HasValue)
                {
                    query = query.Where(w => w.CourseNumber == courseNumber.Value);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.StartDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.StartDate <= ToDate);
                }


                var count = await query.CountAsync();

                var list = await query.Select(s => new CardInfo_DistributeCard_CoursesViewModel()
                {

                    CourseNumber = s.CourseNumber,
                    Description = s.Description,
                    EndDate = s.EndDate,
                    Id = s.Id,
                    OperationId = s.OperationId,
                    StartDate = s.StartDate,
                    User = s.Operation.DisplayName,
                    CardQueueCount = s.CardInfo_DistributeCard_QueueInfo.Count,
                    IsColsed=s.EndDate !=null 



                }).OrderByDescending(o => o.Id).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedCardInfoDistributeCardCoursesViewModel
                {
                    TotalItems = count,
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


        /// <summary>
        /// اضافه کردن دوره توزیع کارت
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<CardInfo_DistributeCard_CoursesDto>> AddCardCourses(CardInfo_DistributeCard_CoursesDto model, int userId)
        {

            var res = new ApiResult<CardInfo_DistributeCard_CoursesDto>(true, ApiResultStatusCode.Success, new CardInfo_DistributeCard_CoursesDto());



            try
            {


                if (await _cardInfo_DistributeCard_Courses.AnyAsync(w => w.EndDate == null))
                {
                    var msg = " دوره توزیع باز  وجود دارد. وضعیت دوره را مشخص نمایید ";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = msg;
                    return res;
                }


                var number = 1;
                if (await _cardInfo_DistributeCard_Courses.AnyAsync())
                {
                    number = await _cardInfo_DistributeCard_Courses.MaxAsync(m => m.CourseNumber);
                    number++;
                }

                var add = new CardInfo_DistributeCard_Courses()
                {
                    GuidId=Guid.NewGuid().ToString(),
                    Description = model.Description,
                    OperationId = userId,
                    StartDate = DateTime.Now,
                    CourseNumber = number,
                };
                await _cardInfo_DistributeCard_Courses.AddAsync(add);
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


        public async Task<ApiResult<string>> CloseCardCourses(int id)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "دوره با موفقیت بسته شد");

            try
            {

                //شرایط بستن صف
                //دارای صف دارای کارت باشد
                //1- تکلیف صفهای داخل صف مشخص شده باشد
                var cardCourses = await _cardInfo_DistributeCard_Courses.Include(w => w.CardInfo_DistributeCard_QueueInfo).FirstOrDefaultAsync(w => w.Id == id);
                if (cardCourses == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "دوره یافت نشد";
                    return res;
                }

                if (cardCourses.CardInfo_DistributeCard_QueueInfo.Any())
                {

                    if (cardCourses.CardInfo_DistributeCard_QueueInfo.Any(w => w.IsActive))
                    {
                        //اگر داخلش صفی نبود 
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        res.Messages = " ابتدا وضعیت صفهای داخل دوره را مشخص نمایید";
                        return res;
                    }


                }
                else
                {
                    //اگر داخلش صفی نبود 
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "دوره فاقد صف می باشد .";
                    return res;

                }


                cardCourses.EndDate = DateTime.Now;
                _cardInfo_DistributeCard_Courses.Update(cardCourses);
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

        public async Task<ApiResult<string>> RemoveCardCourses(int id)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "دوره با موفقیت حذف شد");

            try
            {


                var cardCourses = await _cardInfo_DistributeCard_Courses.Include(w => w.CardInfo_DistributeCard_QueueInfo).FirstOrDefaultAsync(w => w.Id == id);
                if (cardCourses == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "دوره یافت نشد";
                    return res;
                }

                if (cardCourses.CardInfo_DistributeCard_QueueInfo.Any())
                {
                    //اگر داخلش صفی بود 
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " امکان حذف وجود ندارد . دوره دارای صف می باشد.";
                    return res;
                }

                _cardInfo_DistributeCard_Courses.Remove(cardCourses);
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


        #endregion
        #region مدیریت صف های دوره توزیع کارت

        /// <summary>
        /// اضافه کردن صف در توزیع کارت
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<CardDistributionQueueDto>> AddCardDistributionQueue(CardDistributionQueueDto model, int userId)
        {

            var res = new ApiResult<CardDistributionQueueDto>(true, ApiResultStatusCode.Success, new CardDistributionQueueDto());

            if (model == null)
            {
                var msg = "مدل ورودی نامعتبر می باشد";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                res.Messages = msg;
                return res;


            }

            try
            {



                if (await _distributeCard_QueueInfo.AnyAsync(w => w.IsLock != true
                && w.CourseId == model.CoursesId
                && w.Name == model.Name

                ))
                {
                    var msg = "  نام صف وارد شده تکراری می باشد ";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = msg;
                    return res;
                }




                var add = new CardInfo_DistributeCard_QueueInfo()
                {
                    GuidId=Guid.NewGuid().ToString(),
                    Description = model.Description,
                    OperationId = userId,
                    IndexOrder = model.IndexOrder,
                    Name = model.Name,
                    CourseId = model.CoursesId,
                    IsLock = model.ISLock,
                    CardTypeId = model.CardTypeId,
                    DefaultColor = model.DefaultColor,
                    IsActive=true,
                    OnDate = DateTime.Now

                };
                await _distributeCard_QueueInfo.AddAsync(add);


                if (model.GroupIds != null)
                {
                    foreach (var serid in model.GroupIds)
                    {
                        await _cardInfo_DistributeCard_Queue_Groups.AddAsync(new
                            CardInfo_DistributeCard_Queue_Groups()
                        { GroupId = serid, QueueInfo = add });
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


        public async Task<ApiResult<CardDistributionQueueDto>> UpdateCardDistributionQueue(CardDistributionQueueDto model, int userId)
        {
            var res = new ApiResult<CardDistributionQueueDto>(true, ApiResultStatusCode.Success, new CardDistributionQueueDto());
            try
            {


                var item = await _distributeCard_QueueInfo.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد.";
                    return res;
                }





                if (item.DeliveredOnDate != null)
                {
                    res.IsSuccess = false;
                    res.Messages = " امکان ویرایش برای صف هایی که تحویل داده شده اند وجود ندارد";
                    return res;

                }




                item.Description = model.Description;
                item.IndexOrder = model.IndexOrder;
                item.Name = model.Name;
                item.CourseId = model.CoursesId;
                item.IsLock = model.ISLock;
                item.CardTypeId = model.CardTypeId;
                item.DefaultColor = model.DefaultColor;



                var serviceList = await _cardInfo_DistributeCard_Queue_Groups.Where(w => w.QueueInfoId == item.Id).ToListAsync();
                if (serviceList.Any()) _cardInfo_DistributeCard_Queue_Groups.RemoveRange(serviceList);

                if (model.GroupIds != null)
                {
                    foreach (var serid in model.GroupIds)
                    {
                        await _cardInfo_DistributeCard_Queue_Groups.AddAsync(new CardInfo_DistributeCard_Queue_Groups() { GroupId = serid, QueueInfoId = item.Id });
                    }
                }


                _distributeCard_QueueInfo.Update(item);
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


        public async Task<ApiResult<string>> ChangeLockDistributionQueue(int id)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "تغییر وضعیت با موفقیت صورت گرفت");

            try
            {


                var cardQueue = await _distributeCard_QueueInfo.FirstOrDefaultAsync(w => w.Id == id);
                if (cardQueue == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "صف یافت نشد";
                    return res;
                }


                bool IsLock = cardQueue.IsLock;
                cardQueue.IsLock = !IsLock;
                _distributeCard_QueueInfo.Update(cardQueue);
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

        public async Task<ApiResult<PagedCardInfoDistributeCardQueue>> GetPagedeDistributionQueue(
            int pageNumber, int pageSize,
            int courseId,
            string name = null

         )
        {

            var offset = (pageNumber) * pageSize;
            var res = new ApiResult<PagedCardInfoDistributeCardQueue>(true, ApiResultStatusCode.Success, null);

            try
            {
                var query = _distributeCard_QueueInfo.Where(w => w.CourseId == courseId);
                if (!string.IsNullOrEmpty(name))
                {

                    query = query.Where(w => EF.Functions.Like(w.Name, "%" + name + "%"));
                }


                var count = await query.CountAsync();
                var list = await query.Select(s => new CardInfo_DistributeCard_QueueInfoViewModel()
                {

                    Name = s.Name,
                    GuidId = s.GuidId,
                    CourseId = s.CourseId,
                    DefaultColor = s.DefaultColor,
                    CardTypeId = s.CardTypeId,
                    DeliveredByOperationId = s.DeliveredByOperationId,
                    Description = s.Description,
                    Id = s.Id,
                    IndexOrder = s.IndexOrder,
                    IsActive = s.IsActive,
                    OnDate = s.OnDate,
                    IsLock = s.IsLock,
                    DeliveredOnDate = s.DeliveredOnDate,
                    QueueInputType = s.QueueInputType,
                    AllGroups = s.GroupsCitizens.Select(g => g.Group.GroupName),
                    CardCount = s.CardInfo_DistributeCard.Count,




                }).OrderBy(o => o.IndexOrder).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedCardInfoDistributeCardQueue
                {
                    TotalItems = count,
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

        public async Task<ApiResult<List<DistributeCardQueueShortInfoViewModel>>> GetQueueListInCourse(int courseId)
        {



            var res = new ApiResult<List<DistributeCardQueueShortInfoViewModel>>(true, ApiResultStatusCode.Success, null);

            try
            {
                var query = _distributeCard_QueueInfo.Where(w => w.CourseId == courseId);
                var count = await query.CountAsync();
                var list = await query.Select(s => new DistributeCardQueueShortInfoViewModel()
                {

                    Name = s.Name,
                    CourseId = s.CourseId,
                    DefaultColor = s.DefaultColor,
                    CardTypeId = s.CardTypeId,
                    Id = s.Id,
                    IndexOrder = s.IndexOrder,
                    IsActive = s.IsActive,
                    IsLock = s.IsLock,
                    QueueInputType = s.QueueInputType,
                    CardCount = s.CardInfo_DistributeCard.Count,

                }).OrderBy(o => o.IndexOrder).ToListAsync();

                res.Data = list;




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }
        public async Task<ApiResult<CardInfo_DistributeCard_QueueInfoViewModel>> GetDistributionQueueInfo(int id)
        {


            var res = new ApiResult<CardInfo_DistributeCard_QueueInfoViewModel>(true, ApiResultStatusCode.Success, new CardInfo_DistributeCard_QueueInfoViewModel());

            try
            {
                var query = _distributeCard_QueueInfo.Where(w => w.Id == id);


                var item = await query.Include(i => i.CardInfo_DistributeCard).Select(s => new CardInfo_DistributeCard_QueueInfoViewModel()
                {

                    Name = s.Name,
                    CourseId = s.CourseId,
                    DefaultColor = s.DefaultColor,
                    CardTypeId = s.CardTypeId,
                    DeliveredByOperationId = s.DeliveredByOperationId,
                    Description = s.Description,
                    Id = s.Id,
                    IndexOrder = s.IndexOrder,
                    IsActive = s.IsActive,
                    OnDate = s.OnDate,
                    IsLock = s.IsLock,
                    DeliveredOnDate = s.DeliveredOnDate,
                    QueueInputType = s.QueueInputType,
                    OperationId = s.OperationId,
                    UserOperation = s.Operation.DisplayName,
                    CardCount = s.CardInfo_DistributeCard.Count,
                    DeliveredByOperation = s.DeliveredByOperation.DisplayName,
                    DeliveredDescription = s.DeliveredDescription,




                }).FirstOrDefaultAsync();





                if (item != null)
                {
                    var cityList = await _cardInfo_DistributeCard_Queue_Groups.Include(w => w.Group).Where(w => w.QueueInfoId == id).ToArrayAsync();
                    if (cityList.Any())
                    {
                        item.GroupIds = cityList.Select(s => s.GroupId).ToList();
                        item.Groups = cityList.Select(s => new BaseDataModel { Text = s.Group.GroupName, Key = s.GroupId.ToString() }).AsEnumerable();

                    }
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






        public async Task<ApiResult<string>> RemoveQueue(int id)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "صف با موفقیت حذف شد");

            try
            {


                var cardCourses = await _distributeCard_QueueInfo.FirstOrDefaultAsync(w => w.Id == id);
                if (cardCourses == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "صف یافت نشد";
                    return res;
                }
                var listCard = await _cardInfo_DistributeCard.Where(w => w.QueueInfoId == id).ToListAsync();
                if (listCard.Any())
                {
                    _cardInfo_DistributeCard.RemoveRange(listCard);
                }

                var groupList = await _cardInfo_DistributeCard_Queue_Groups.Where(w => w.QueueInfoId == id).ToListAsync();
                if (groupList.Any()) _cardInfo_DistributeCard_Queue_Groups.RemoveRange(groupList);

                _distributeCard_QueueInfo.Remove(cardCourses);
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









        #endregion
        #region جستجوی شهروند جهت اضافه کردن به صف توزیع
        public async Task<ApiResult<CitizensCardInfo>> SearchCardForQueue(MiniSearchCitizensViewModel model)
        {


            var res = new ApiResult<CitizensCardInfo>(true, ApiResultStatusCode.Success, null);
            try
            {
                // کد_ملی=1,
                //اصفهان_کارت = 2,
                var find = model.Value;
                if (string.IsNullOrEmpty(model.Value))
                {
                    res.IsSuccess = false;
                    res.Messages = "کد ملی یا اصفهان کارت را وارد نمایید";
                    return res;
                }
                else
                {
                    if (model.SearchCitizensType == SearchCitizensTypeEnum.کد_ملی)
                    {
                        model.Value = model.Value.FixFullString();
                        var check = model.Value.IsValidNationalCode();
                        if (check != "")
                        {
                            res.IsSuccess = false;
                            res.Messages = check;
                            return res;
                        }
                        //
                        res.Data = await _citizensCard.AsNoTracking()
                            .Where(w => w.RequestStatuse == CardRequestStatusEnum.چاپ_کارت && w.Citizen.NationCode == model.Value).Select(s => new CitizensCardInfo()
                            {
                                AdminDescription = s.AdminDescription,
                                BarCode = s.BarCode,
                                CardActivationDate = s.CardActivationDate,
                                CardExpirationDate = s.CardExpirationDate,
                                CardInfoId = s.CardInfoId,
                                CardNumber = s.CardNumber,
                                CardRequestType = s.CardRequestType,
                                CardSerial = s.CardSerial,
                                CitizenId = s.CitizenId,
                                Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                                DeliveredByOperationId = s.DeliveredByOperationId,
                                DeliveredDescription = s.DeliveredDescription,
                                DeliveredOnDate = s.DeliveredOnDate,
                                DeliveringAddress = s.DeliveringAddress == null ? "" : s.DeliveringAddress.FullAddress,
                                DeliveringAddressId = s.DeliveringAddressId,
                                DeliveringCenter = s.DeliveringCenter == null ? "" : s.DeliveringCenter.Name,
                                DeliveringCenterId = s.DeliveringCenterId,
                                DeliverType = s.DeliverType,
                                IsSetBarCode = s.IsSetBarCode,
                                RequestCode = s.RequestCode,
                                RequestDate = s.RequestDate,
                                RequestStatuse = s.RequestStatuse,
                                RequestByCitizenId = s.RequestByCitizenId,
                                DiscountGroupId = s.DiscountGroupId,
                                PreCardNumber = s.PreCardNumber,
                                Id = s.Id,
                                CitizenCardInfoId = s.Id,
                                SabtStatus = s.Citizen.SabtStatus,
                                NationCode = s.Citizen.NationCode,
                                CardTitle = s.CardInfo.CardType.Title,
                                CardTypeId = s.CardInfo.CardTypeId,
                                Groups = s.Citizen.GroupsCitizens.Where(w => w.Group.IsDeleted != true).Select(a =>a.Group.GroupName)

                            }).FirstOrDefaultAsync();


                    }
                    else
                    {
                        if (model.SearchCitizensType == SearchCitizensTypeEnum.اصفهان_کارت)
                        {

                            var hex = ConvertDecToHex.Convert(model.Value);
                            if (string.IsNullOrEmpty(hex))
                            {
                                res.IsSuccess = false;
                                res.Messages = "امکان پردازش کارت وجود ندارد لطفا کد ملی را وارد نمایید";
                                return res;

                            }
                            else
                            {
                                find = "DC" + hex.ToUpper();

                            }

                        }

                        res.Data = await _citizensCard.AsNoTracking()
                              .Where(w => w.RequestStatuse == CardRequestStatusEnum.چاپ_کارت && w.CardNumber == find).Select(s => new CitizensCardInfo()
                              {
                                  AdminDescription = s.AdminDescription,
                                  BarCode = s.BarCode,
                                  CardActivationDate = s.CardActivationDate,
                                  CardExpirationDate = s.CardExpirationDate,
                                  CardInfoId = s.CardInfoId,
                                  CardNumber = s.CardNumber,
                                  CardRequestType = s.CardRequestType,
                                  CardSerial = s.CardSerial,
                                  CitizenId = s.CitizenId,
                                  Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                                  SabtStatus = s.Citizen.SabtStatus,
                                  DeliveredByOperationId = s.DeliveredByOperationId,
                                  DeliveredByOperation = s.DeliveredByOperation == null ? "" : s.DeliveredByOperation.DisplayName,
                                  DeliveredDescription = s.DeliveredDescription,
                                  DeliveredOnDate = s.DeliveredOnDate,
                                  DeliveringAddress = s.DeliveringAddress == null ? "" : s.DeliveringAddress.FullAddress,
                                  DeliveringAddressId = s.DeliveringAddressId,
                                  DeliveringCenter = s.DeliveringCenter == null ? "" : s.DeliveringCenter.Name,
                                  DeliveringCenterId = s.DeliveringCenterId,
                                  DeliverType = s.DeliverType,
                                  IsSetBarCode = s.IsSetBarCode,
                                  RequestCode = s.RequestCode,
                                  RequestDate = s.RequestDate,
                                  RequestStatuse = s.RequestStatuse,
                                  RequestByCitizenId = s.RequestByCitizenId,
                                  DiscountGroupId = s.DiscountGroupId,
                                  PreCardNumber = s.PreCardNumber,
                                  Id = s.Id,
                                  NationCode = s.Citizen.NationCode,
                                  CardTitle = s.CardInfo.CardType.Title

                              }).FirstOrDefaultAsync();




                    }

                }



                if (res.Data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشده";
                    return res;
                }



                if (res.Data.SabtStatus==SabtStatusEnum.فوتی)
                {
                    res.IsSuccess = false;
                    res.Messages = "این شهروند در قید حیات نمی باشد";
                    return res;
                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
                return res;
            }
            return res;
        }



        #endregion


        #region مدیریت کارهای داخل صف   توزیع
        public async Task<ApiResult<PagedCitizenCardsInQueueInfo>> SearchCardInQueue(
       int pageNumber, int pageSize,
       string name = null,
       string nationCode = null,
       int? courseId = null,
       int? courseNumber = null,
       int? queueId = null,
       string cardNumber = null,
       int? cardTypeId = null,
       QueueInputTypeEnum? queueInputType = null


       )
        {

            var res = new ApiResult<PagedCitizenCardsInQueueInfo>(true, ApiResultStatusCode.Success, new PagedCitizenCardsInQueueInfo());
            try
            {
                var offset = (pageNumber) * pageSize;
                var query = _cardInfo_DistributeCard.AsQueryable();

                if (queueId != null)
                {
                    query = query.Where(w => w.QueueInfoId == queueId);

                }

                if (courseId != null)
                {
                    query = query.Where(w => w.QueueInfo.CourseId == courseId);

                }
                if (courseNumber != null)
                {
                    query = query.Where(w => w.QueueInfo.Course.CourseNumber == courseNumber);

                }



                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => w.CitizenCardInfo.Citizen.NationCode == nationCode);
                }


                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.CitizenCardInfo.Citizen.FirstName, "%" + name + "%")
                    ||
                    EF.Functions.Like(w.CitizenCardInfo.Citizen.LastName, "%" + name + "%")

                    );
                }



                if (cardTypeId != null)
                {
                    query = query.Where(w => w.CitizenCardInfo.CardInfo.CardTypeId == cardTypeId);
                }

                if (queueInputType != null)
                {
                    query = query.Where(w => w.QueueInfo.QueueInputType == queueInputType);
                }



                if (!string.IsNullOrEmpty(cardNumber))
                {
                    query = query.Where(w => w.CitizenCardInfo.CardNumber == cardNumber);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new CitizenCardsInQueueInfo()
                {
                    DeliveringCenterId = s.CitizenCardInfo.DeliveringCenterId,
                    DeliveringCenter = s.CitizenCardInfo.DeliveringCenter.Name,
                    DeliveringAddressId = s.CitizenCardInfo.DeliveringAddressId,
                    RequestId = s.CitizenCardInfo.Id,
                    CourseId = s.QueueInfo.CourseId,
                    CourseNumber = s.QueueInfo.Course.CourseNumber,
                    QueueName = s.QueueInfo.Name,
                    QueueId = s.QueueInfoId,
                    QueueOnDate = s.OnDate,
                    BarCode = s.CitizenCardInfo.BarCode,
                    PrintId = s.Id,
                    CardInfoId = s.CitizenCardInfo.CardInfoId,
                    CardNumber = s.CitizenCardInfo.CardNumber,
                    DeliveredOnDate = s.CitizenCardInfo.DeliveredOnDate,
                    CardRequestType = s.CitizenCardInfo.CardRequestType,
                    CitizenId = s.CitizenCardInfo.CitizenId,
                    CardSerial = s.CitizenCardInfo.CardSerial,
                    DeliveredByOperationId = s.CitizenCardInfo.DeliveredByOperationId,
                    DeliveredByOperation = s.CitizenCardInfo.DeliveredByOperation.DisplayName,
                    DeliveredDescription = s.CitizenCardInfo.DeliveredDescription,
                    NationCode = s.CitizenCardInfo.Citizen.NationCode,
                    Citizen = s.CitizenCardInfo.Citizen.FirstName + " " + s.CitizenCardInfo.Citizen.LastName,
                    FirstName = s.CitizenCardInfo.Citizen.FirstName,
                    LastName = s.CitizenCardInfo.Citizen.LastName,
                    Alley=s.CitizenCardInfo.DeliveringAddress==null ? "": s.CitizenCardInfo.DeliveringAddress.Alley,
                    Plaque = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Plaque,
                    PostalCode = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.PostalCode,
                    FullAddress = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.FullAddress,
                    City = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.City.Title,
                    Mobile=s.CitizenCardInfo.Citizen.Mobile,
                    Phone=s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Phone,
                    Street = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Street,
                    SabtStatus= s.CitizenCardInfo.Citizen.SabtStatus,
                    Region = s.CitizenCardInfo.DeliveringAddress == null ? null : s.CitizenCardInfo.DeliveringAddress.Region,
                    DeliverType = s.CitizenCardInfo.DeliverType,
                    RequestCode = s.CitizenCardInfo.RequestCode,
                    RequestByCitizen = s.CitizenCardInfo.RequestByCitizen.FirstName + " " + s.CitizenCardInfo.RequestByCitizen.LastName,
                    RequestStatuse = s.CitizenCardInfo.RequestStatuse,
                    RequestDate = s.CitizenCardInfo.RequestDate,
                    RequestByCitizenId = s.CitizenCardInfo.RequestByCitizenId,
                    IsSetBarCode = s.CitizenCardInfo.IsSetBarCode,
                    DiscountGroupId = s.CitizenCardInfo.DiscountGroupId,
                    CardTitle = s.CitizenCardInfo.CardInfo.CardType.Title,
                }).OrderByDescending(w => w.PrintId).Skip(offset).Take(pageSize).ToListAsync();



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

        public async Task<ApiResult<string>> RemoveCardFromQueue(int id)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "کارت با موفقیت از صف  حذف شد");

            try
            {


                var card = await _cardInfo_DistributeCard.Include(i => i.QueueInfo).FirstOrDefaultAsync(w => w.Id == id);
                if (card == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "کارتی یافت نشد";
                    return res;
                }
                if (!card.QueueInfo.IsActive)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "به دلیل تحویل صف امکان حذف کارت وجود ندارد";
                    return res;

                }
                _cardInfo_DistributeCard.Remove(card);
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

        public async Task<ApiResult<string>> RemoveCardFromQueueByCourseId(int courseId, int cardId)
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "کارت با موفقیت از صف  حذف شد");

            try
            {


                var card = await _cardInfo_DistributeCard.Include(i => i.QueueInfo).FirstOrDefaultAsync(w => w.QueueInfo.CourseId == courseId && w.CitizenCardInfoId == cardId);
                if (card == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "کارتی یافت نشد";
                    res.Data = "کارتی یافت نشد";
                    return res;
                }
                if (!card.QueueInfo.IsActive)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "به دلیل تحویل صف امکان حذف کارت وجود ندارد";
                    return res;

                }
                _cardInfo_DistributeCard.Remove(card);
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


        public async Task<ApiResult> DeliveryQueueToOperator(DeliveryQueueToOperatorViewModel model)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "صف با موفقیت تحویل داده شد");

            try
            {

                var citizenId = 0;
                if (string.IsNullOrWhiteSpace(model.NationalCode))
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "کد ملی مسئول تحویل صف را مشخص کنید";
                    return res;

                }
                else
                {
                    var nationCode = model.NationalCode.Fa2En();
                    var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                    if (citizen == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "شهروندی با کد ملی وارد شده یافت نشد";
                        return res;
                    }
                    citizenId = citizen.CitizenId;
                }




                var queue = await _distributeCard_QueueInfo.Include(i => i.CardInfo_DistributeCard).FirstOrDefaultAsync(w => w.Id == model.QueueId);
                if (queue == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "صفی یافت نشد";
                    return res;
                }

                if (!queue.CardInfo_DistributeCard.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "داخل این صف کارتی برای تحویل وجود ندارد";
                    return res;
                }



                var cardIds = queue.CardInfo_DistributeCard.Select(s => s.CitizenCardInfoId).ToList();


                var citizenCards = await _citizensCard.Where(w => cardIds.Contains(w.Id)).ToListAsync();
                foreach (var card in citizenCards)
                {
                    if (card.RequestStatuse == CardRequestStatusEnum.چاپ_کارت)
                    {
                        if (card.DeliverType == DeliverTypeEnum.تحویل_در_مرکز)
                        {

                            card.RequestStatuse = CardRequestStatusEnum.ارسال_به_مرکز_تحویل;
                        }
                        else
                        {
                            card.RequestStatuse = CardRequestStatusEnum.ارسال_به_پست;
                        }

                        card.DistributeCardOnDate = DateTime.Now;
                        _citizensCard.Update(card);


                    }


                }






                queue.DeliveredByOperationId = citizenId;
                queue.DeliveredDescription = model.Description;
                queue.DeliveredOnDate = DateTime.Now;
                queue.IsActive = false;
                _distributeCard_QueueInfo.Update(queue);
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


        #region اضافه کردن کارت به صف  توزیع کارت
        public async Task<ApiResult<PrintqueueInfoViewModel>>
          AddCardToPostQueue(int operationId, int citizenId, int cardTypeId, int cardId, int courseId)
        {

            var res = new ApiResult<PrintqueueInfoViewModel>(true, ApiResultStatusCode.Success, null, "عملیات با موفقیت انجام گردید");

            try
            {


                var checkAdd = await _cardInfo_DistributeCard.Include(i => i.QueueInfo).FirstOrDefaultAsync(w => w.QueueInfo.CourseId == courseId
                   && w.CitizenCardInfoId == cardId
                );
                if (checkAdd != null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " این کارت قبلا به صف " + checkAdd.QueueInfo.Name + " اضافه شده است  ";
                    return res;
                }


                var distributeQueuelist = await _distributeCard_QueueInfo.Include(i=>i.GroupsCitizens).Where(w =>
                 w.IndexOrder != -1 //صف پیش فرض نباشد
               && w.CourseId == courseId
               //مربوط به این دوره باشد
               && w.IsActive //فعال باشد
               && w.IsLock != true //قفل نشده باشد
               && w.IsDeleted != true //حذف نشده باشد
               && w.CardTypeId == cardTypeId  //مربوط به این نوع کارت باشد
               && w.DeliveredByOperationId == null  //این صف فعال باشد و تحویل هیچ اپراتوری داده نشده است
               && w.QueueInputType == QueueInputTypeEnum.تحویل_پستی

              ).OrderBy(o => o.IndexOrder).ToListAsync();

                if (!distributeQueuelist.Any())
                {
                    //اگر صف  مرتبط با این درخواست وجود نداشت صف پیش فرض را در نظر بگیر
                    //برو صف پیش فرض ببین برای این نوع کارت وجود دارد یه نه؟
                    var defultQueue = await _distributeCard_QueueInfo.Where(w =>

            w.CourseId == courseId
           && w.CardTypeId == cardTypeId
           && w.IndexOrder == -1
           && w.QueueInputType != QueueInputTypeEnum.صف_موقت
           && w.IsActive
           && w.IsLock != true
           && w.IsDeleted != true
          ).Select(s => new PrintqueueInfoViewModel()
          {
              QueueId = s.Id,
              QueueName = s.Name,
              GroupId = null,
              DefaultColor = s.DefaultColor
          }).FirstOrDefaultAsync();


                    if (defultQueue != null)
                    {
                        res.Data = defultQueue;
                        res.Messages = " با موفقیت به صف پستی    " + defultQueue.QueueName + "  اضافه گردید ";

                        await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                        {
                            GuidId = Guid.NewGuid().ToString(),
                            CitizenCardInfoId = cardId,
                            OnDate = DateTime.Now,
                            QueueInfoId = defultQueue.QueueId,
                        });
                        await _uow.SaveChangesAsync();
                        return res;

                    }

                    else
                    {
                        //اگر صف پیش فرض وجود نداشت اونو اضافه کن
                        //ایجاد صف پیش فرض
                        var mappingModel = new CardInfo_DistributeCard_QueueInfo()
                        {
                            CourseId = courseId,
                            OperationId = operationId,
                            Name = "صف پیش فرض تحویل پستی ",
                            IndexOrder = -1,
                            IsActive = true,
                            IsLock = false,
                            OnDate = DateTime.Now,
                            CardTypeId = cardTypeId,
                            DefaultColor = "#ffa7a8",
                            QueueInputType = QueueInputTypeEnum.تحویل_پستی

                        };
                        await _distributeCard_QueueInfo.AddAsync(mappingModel);
                        await _uow.SaveChangesAsync();

                        var nextitem = await _distributeCard_QueueInfo.Where(w =>

                w.CourseId == courseId
               && w.CardTypeId == cardTypeId
               && w.IndexOrder == -1
               && w.QueueInputType != QueueInputTypeEnum.صف_موقت
               && w.IsActive
               && w.IsLock != true
               && w.IsDeleted != true
              ).Select(s => new PrintqueueInfoViewModel()
              {

                  QueueId = s.Id,
                  QueueName = s.Name,
                  GroupId = null,
                  DefaultColor = s.DefaultColor

              }).FirstOrDefaultAsync();
                        res.Data = nextitem;
                        res.Messages = " با موفقیت به صف پستی    " + nextitem.QueueName + "  اضافه گردید ";

                        await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                        {
                            GuidId = Guid.NewGuid().ToString(), 
                            CitizenCardInfoId = cardId,
                            OnDate = DateTime.Now,
                            QueueInfoId = nextitem.QueueId,
                        });
                        await _uow.SaveChangesAsync();
                        return res;

                    }




                }
                else
                {


                    if (distributeQueuelist.Count == 1)
                    {
                        var firstQ = distributeQueuelist.FirstOrDefault();
                        var fmodelQ = new PrintqueueInfoViewModel()
                        {
                            QueueId = firstQ.Id,
                            QueueName = firstQ.Name,
                            GroupId = null,
                            DefaultColor = firstQ.DefaultColor
                        };
                        res.Data = fmodelQ;
                        res.Messages = " با موفقیت به صف پستی    " + firstQ.Name + "  اضافه گردید ";

                        await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                        {
                            GuidId = Guid.NewGuid().ToString(),
                            CitizenCardInfoId = cardId,
                            OnDate = DateTime.Now,
                            QueueInfoId = firstQ.Id,
                        });
                        await _uow.SaveChangesAsync();
                        return res;

                    }
                    else
                    {
                        var citizengroupIds = await _citizenGroups.Where(w => w.CitizenId == citizenId).Select(s => s.GroupId).ToListAsync();
                        if (citizengroupIds.Any())
                        {
                            foreach (var queueinfo in distributeQueuelist)
                            {
                                if (queueinfo.GroupsCitizens.Any())
                                {
                                    foreach (var grp in queueinfo.GroupsCitizens)
                                    {
                                        if (citizengroupIds.Contains(grp.GroupId))
                                        {
                                            var model = new PrintqueueInfoViewModel()
                                            {
                                                QueueId = grp.QueueInfoId,
                                                QueueName = queueinfo.Name,
                                                GroupId = grp.GroupId,
                                                DefaultColor = queueinfo.DefaultColor
                                            };
                                            res.Data = model;
                                            res.Messages = " با موفقیت به صف پستی    " + queueinfo.Name + "  اضافه گردید ";

                                            await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                                            {
                                                GuidId = Guid.NewGuid().ToString(),
                                                CitizenCardInfoId = cardId,
                                                OnDate = DateTime.Now,
                                                QueueInfoId = queueinfo.Id,
                                                QueueByGroupId = grp.GroupId,

                                            });
                                            await _uow.SaveChangesAsync();
                                            return res;
                                        }
                                    }
                                }
                            }
                        }
                    } 
                }



                //اگر صف وجود داشت ولی گروه متناسب یافت نشد اولین صف را انتخاب کن
                var notasGroups = distributeQueuelist.Where(w => !w.GroupsCitizens.Any());

                if(notasGroups.Any())
                {
                    var firstQueue = notasGroups.FirstOrDefault();
                    var fmodel = new PrintqueueInfoViewModel()
                    {
                        QueueId = firstQueue.Id,
                        QueueName = firstQueue.Name,
                        GroupId = null,
                        DefaultColor = firstQueue.DefaultColor
                    };
                    res.Data = fmodel;
                    res.Messages = " با موفقیت به صف پستی    " + firstQueue.Name + "  اضافه گردید ";

                    await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                    {
                        GuidId = Guid.NewGuid().ToString(),
                        CitizenCardInfoId = cardId,
                        OnDate = DateTime.Now,
                        QueueInfoId = firstQueue.Id,
                    });
                    await _uow.SaveChangesAsync();
                    return res;

                }

                else
                {
                    var firstQueue = distributeQueuelist.FirstOrDefault();
                    var fmodel = new PrintqueueInfoViewModel()
                    {
                        QueueId = firstQueue.Id,
                        QueueName = firstQueue.Name,
                        GroupId = null,
                        DefaultColor = firstQueue.DefaultColor
                    };
                    res.Data = fmodel;
                    res.Messages = " با موفقیت به صف پستی    " + firstQueue.Name + "  اضافه گردید ";

                    await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                    {
                        GuidId = Guid.NewGuid().ToString(),
                        CitizenCardInfoId = cardId,
                        OnDate = DateTime.Now,
                        QueueInfoId = firstQueue.Id,
                    });
                    await _uow.SaveChangesAsync();
                    return res;
                }






                




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;
            }



        }

        public async Task<ApiResult<PrintqueueInfoViewModel>> AddCardToDeliveredQueue(int cardId, string centerId, int courseId, int operationId)
        {

            var res = new ApiResult<PrintqueueInfoViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {

                //چک کن قبلا به صف اضافه نشده باشه


                var checkAdd = await _cardInfo_DistributeCard.Include(i => i.QueueInfo).FirstOrDefaultAsync(w => w.QueueInfo.CourseId == courseId
                   && w.CitizenCardInfoId == cardId
                );
                if (checkAdd != null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " این کارت قبلا به صف " + checkAdd.QueueInfo.Name + " اضافه شده است  ";
                    return res;
                }

                var centerName = "";
                var center = await _organizationalUnit.FirstOrDefaultAsync(w => w.Id == centerId);
                if (center == null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه مرکز نا معتبر می باشد ";
                    return res;

                }


                centerName = center.Name.Trim();
                var msg = " با موفقیت به صف مرکز تحویل  " + centerName + "  اضافه گردید ";

                //برو ببین واحدی با این نام وجود دارد ؟

                var item = await _distributeCard_QueueInfo.Where(w =>
                w.CourseId == courseId
           && w.QueueInputType == QueueInputTypeEnum.مرکز_تحویل
           && w.IsActive
           && w.IsLock != true
           && w.IsDeleted != true
           && w.Name == centerName //نام واحد با نام صف یکی باشد
          ).Select(s => new PrintqueueInfoViewModel()
          {

              QueueId = s.Id,
              QueueName = s.Name,
              GroupId = null,
              DefaultColor = s.DefaultColor

          }).FirstOrDefaultAsync();

                if (item != null)
                {
                    //اضافه کردن کارت به این صف
                    await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                    {
                        GuidId = Guid.NewGuid().ToString(),
                        CitizenCardInfoId = cardId,
                        OnDate = DateTime.Now,
                        QueueInfoId = item.QueueId,
                    });
                    await _uow.SaveChangesAsync();

                    res.Data = item;
                    res.Messages =msg;
                    return res;

                }
                else
                {
                    //ایجاد صف مربوط به این مرکز
                    var mappingModel = new CardInfo_DistributeCard_QueueInfo()
                    {
                        CourseId = courseId,
                        OperationId = operationId,
                        Name = centerName,
                        IndexOrder = 1,
                        IsActive = true,
                        IsLock = false,
                        OnDate = DateTime.Now,
                        DefaultColor = "#a62626",//رنگ پیش فرض مراکز تحویل
                        QueueInputType = QueueInputTypeEnum.مرکز_تحویل,


                    };
                    await _distributeCard_QueueInfo.AddAsync(mappingModel);
                    //اضافه کردن کارت به این صف
                    await _cardInfo_DistributeCard.AddAsync(new CardInfo_DistributeCard()
                    {
                        GuidId = Guid.NewGuid().ToString(),
                        CitizenCardInfoId = cardId,
                        OnDate = DateTime.Now,
                        QueueInfo = mappingModel,
                    });

                    await _uow.SaveChangesAsync();

                    var nextitem = await _distributeCard_QueueInfo.Where(w =>
           w.OperationId == operationId
           && w.CourseId == courseId
           && w.IndexOrder == 1
           && w.Name == centerName
           && w.QueueInputType == QueueInputTypeEnum.مرکز_تحویل
           && w.IsActive
           && w.IsLock != true
           && w.IsDeleted != true
          ).Select(s => new PrintqueueInfoViewModel()
          {

              QueueId = s.Id,
              QueueName = s.Name,
              GroupId = null,
              DefaultColor = s.DefaultColor
          }).FirstOrDefaultAsync();

                    res.Data = nextitem;
                    res.Messages = msg;
                    return res;

                }
            }

            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;

            }



        }


        #endregion


        #region چاپ صف و دوره

        public async Task<ApiResult<List<PrintForPostViewModel>>> GetPrintQueueForPostList( string guidId)
         
        {

           
            var res = new ApiResult<List<PrintForPostViewModel>>(true, ApiResultStatusCode.Success, new List<PrintForPostViewModel>());

            try
            {

                var query = _cardInfo_DistributeCard.Where(w=>w.QueueInfo.GuidId== guidId);
                

                var list = await query.Select(s => new PrintForPostViewModel()
                {
                    Address=s.CitizenCardInfo.DeliveringAddress==null ? "":s.CitizenCardInfo.DeliveringAddress.City.Title + " "+ s.CitizenCardInfo.DeliveringAddress.FullAddress,
                    City= s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.City.Title  ,
                    BarCode= s.CitizenCardInfo.DeliveringAddress == null ? s.CitizenCardInfo.BarCode  : s.CitizenCardInfo.DeliveringAddress.PostalCode,
                    Mobile=s.CitizenCardInfo.Citizen.Mobile,
                    Name=s.CitizenCardInfo.Citizen.FirstName +" "+ s.CitizenCardInfo.Citizen.LastName,
                    NationalCode=s.CitizenCardInfo.Citizen.NationCode,
                    CardNumber=s.CitizenCardInfo.CardNumber,
                    ZipCode = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.PostalCode, 
                    QueueName=s.QueueInfo.Name,
                    Id=  s.Id  ,
                    


                }).OrderByDescending(o => o.Id).ToListAsync();

                var index = 1;
                foreach (var item in list)
                {
                    item.RowId = index.ToString();
                    res.Data.Add(item);
                    index++;

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


        #endregion 



    }

}
