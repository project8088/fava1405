using Nikan.Common;
using cle.Services;
using cle.Services.Faq; 
using Nikan.ViewModel;
using Nikan.ViewModel.Faq;
using Nikan.ViewModel.NewsItems;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nikan.Services;
using System;
using Microsoft.AspNetCore.Cors;
 
using Microsoft.AspNetCore.Authorization;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.NewsItem;
using Nikan.Services.SlidShow;
using Nikan.ViewModel.SlidShow;

namespace Nikan.Controllers.Api
{
    /// <summary>
    /// مدیریت محتوا
    /// </summary>
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")] 
    [ApiController]
    [Authorize(Policy = CustomRoles.Admin)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class contentController : ControllerBase
    {

        #region Field
        
        private readonly IFaqQuestionService _faqQuestionService;
        private readonly IFaqQuestionGroupTypeService _faqQuestionGroupTypeService;
        private readonly INewsService _newsService;
        private readonly INewsGroupService _newsGroupService;
        private readonly IWebPageService _webPageService;
        private readonly IUsersService _usersService;
        private readonly IMenuItemService _menuService;
        private readonly ISlideShowService _slidShowService;

        #endregion
        #region Constractor

        public contentController(
            IUsersService usersService,
            INewsService newsService,
            ISlideShowService slidShowService,
            IMenuItemService menuService,
            IWebPageService webPageService,
           
            INewsGroupService newsGroupService,
            IFaqQuestionGroupTypeService FaqQuestionGroupTypeService, IFaqQuestionService faqQuestionService)
        {
            _faqQuestionService = faqQuestionService;
            _faqQuestionGroupTypeService = FaqQuestionGroupTypeService;
            _usersService = usersService;
            _newsService = newsService;
            _newsGroupService = newsGroupService;
           
            _webPageService = webPageService;
            _menuService = menuService;
            _slidShowService = slidShowService;

        }

        #endregion 
        #region Faq 

        /// <summary>
        /// اضافه یا ویرایش پرسش و پاسخ
        /// [p1]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<FaqQuestionGroupTypeDto>> AddOrUpdateFaqGroup([FromBody]FaqQuestionGroupTypeDto model)
        {
            if (model == null)
                return new ApiResult<FaqQuestionGroupTypeDto>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");

            if (string.IsNullOrWhiteSpace(model.Title))
                return new ApiResult<FaqQuestionGroupTypeDto>(false, ApiResultStatusCode.BadRequest, null, "عنوان را وارد نمایید");



            var userId = _usersService.GetCurrentUserId();
            return await _faqQuestionGroupTypeService.AddOrUpdate(model, userId);
        }



        /// <summary>
        /// حذف گروه پرسش و پاسخ
        ///[p2]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("RemoveFaqGroupType")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveFaqGroupType(int id)
        {

            return await _faqQuestionGroupTypeService.Remove(id);

        }

       
        /// <summary>
        /// دریافت لیست گروههای پرسش و پاسخ
        /// [p3]
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaqGroups")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetFaqGroups(int? selected)
        {

            return await _faqQuestionGroupTypeService.GetFaqGroups(selected);

        }


        /// <summary>
        /// لیست گروههای پرسش و پاسخ جهت نمایش در پنل مدیریت
        /// [p4]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllFaqGroups")]
        [AllowAnonymous]
        public async Task<ApiResult<List<FaqQuestionGroupTypeDto>>> GetAllFaqGroups()
        {

            return await _faqQuestionGroupTypeService.GetAllFaqGroups();

        }

        /// <summary>
        /// اضافه یا ویرایش کردن پرسش و پاسخ
        /// [p5]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<FaqQuestionModel>> AddOrUpdateFaq(FaqQuestionModel model)
        {
            if (model == null)
                return new ApiResult<FaqQuestionModel>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");

            if (string.IsNullOrWhiteSpace(model.Title))
                return new ApiResult<FaqQuestionModel>(false, ApiResultStatusCode.BadRequest, null, "عنوان را وارد نمایید");


            var userId = _usersService.GetCurrentUserId();
            return await _faqQuestionService.AddOrUpdate(model, userId);
        }



        /// <summary>
        /// دریافت تمامی پرسش و پاسخ ها
        /// [p6]
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaqList")]
        [AllowAnonymous]
        public async Task<ApiResult<List<faqDto>>> GetFaqList(int? groupId = null)
        {

            return await _faqQuestionService.GetAllFaq(groupId, true);

        }



        /// <summary>
        /// دریافت جزئیات یک پرسش و پاسخ
        /// [p7]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetFaq")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult<faqDto>> GetFaq(int id)
        {

            return await _faqQuestionService.GetFaq(id);

        }


        /// <summary>
        ///حذف پرسش و پاسخ
        /// [p8]
        /// </summary>
        /// <param name="id">شناسه پرسش و پاسخ</param>
        /// <returns></returns>
        [Route("RemoveFaq")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveFaq(int id)
        {

            return await _faqQuestionService.Remove(id);

        }



        #endregion
        #region News

        /// <summary>
        /// اضافه کردن گروه های خبری
        /// [p9]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<NewsGroupDto>> AddOrUpdateNewsGroup([FromBody] NewsGroupDto model)
        {
            if (model == null)
                return new ApiResult<NewsGroupDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
            return await _newsGroupService.AddOrUpdate(model);
        }

        /// <summary>
        /// دریافت لیست گروه خبر
        /// [p10]
        /// </summary>
        /// <param name="selected">گروه انتخاب شده</param>
        /// <returns>دریافت گروه خبری</returns>
        [HttpGet]
        [Route("GetListNewsGroups")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetListNewsGroups(int? selected)
        { 
            return await _newsGroupService.GetList(selected); 
        }
         
        /// <summary>
        /// حذف گروه خبری
        /// [p11]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveNewsGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveNewsGroups(int id)
        {

            return await _newsGroupService.Remove(id);

        }

        /// <summary>
        /// دریافت همه گروه های خبری
        /// [p12]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllNewGroups")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsGroupDto>>> GetAllNewGroups()
        {

            return await _newsGroupService.GetAll();

        }


        /// <summary>
        /// دریافت اطلاعات یک گروه خبری
        /// [p13]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNewsGroup")]
        [AllowAnonymous]
        public async Task<ApiResult<NewsGroupDto>> GetNewsGroup(int id)
        {

            return await _newsGroupService.GetNewsGroup(id);

        }


        /// <summary>
        /// دریافت خبر به صورت صفحه بندی 
        /// [p14]
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedNewsItems")]
        [AllowAnonymous]
        public async Task<ApiResult<PagedNewsItemsViewModel>> GetPagedNewsItems
            (int? offset = 1, int? count = 20, int? groupId = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string title = null
            )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;




            return await _newsService.GetPagedNewsItemsAsync(Common.GlobalEnum.PageType.NewsList,
                offset.Value, count.Value, groupId, FromDate, ToDate, title);

        }








        /// <summary>
        /// دریافت لیست آخرین اخبار
        ///  [p15]
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLastNews")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsList>>> GetLastNews(int? top = 20)
        {
            if (top == null || top < 1) top = 20;
            return await _newsService.GetLastNewsAsync(Common.GlobalEnum.PageType.NewsList, top.Value);

        }

        [HttpGet]
        [Route("GetLastApiHelp")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsList>>> GetLastApiHelp(int? top = 1000)
        {
            if (top == null || top < 1) top = 20;
            return await _newsService.GetLastNewsAsync(Common.GlobalEnum.PageType.NewsList, top.Value);

        }



        /// <summary>
        /// پربازدیدترین
        /// [p16]
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMostVisitedNews")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsList>>> GetMostVisitedNews(int? top = 20)
        {

            if (top == null || top < 1) top = 20;
            return await _newsService.GetMostVisitedNews(Common.GlobalEnum.PageType.NewsList, top.Value);

        }

        /// <summary>
        /// جزئیات یک خبر 
        /// [p17]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forEdit"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetNews")]
        [AllowAnonymous]
        public async Task<ApiResult<NewsInfo>> GetNews(int id, bool forEdit = true)
        {

            return await _newsService.GetNews(id, forEdit);

        }
        /// <summary>
        /// حذف خبر
        ///  [p18]
        /// </summary>
        /// <param name="id">شناسه خبر</param>
        /// <returns></returns> 
        [HttpGet]
        [Route("RemoveNews")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveNews(int id)
        {

            return await _newsService.RemoveNews(id);

        }
        /// <summary>
        /// ثبت کامنت به خبر
        /// [p19]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult> AddComments(NewsCommentDto model)
        {
            return await _newsService.AddComments(model);
        }

        /// <summary>
        /// اضافه یا ویرایش خبر
        ///  [p20]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<NewsDto>> AddOrUpdateNews(NewsDto model)
        {
            if (model == null)
                return new ApiResult<NewsDto>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");

            if (model.PublishDate == null)
                return new ApiResult<NewsDto>(false, ApiResultStatusCode.BadRequest, null, "تاریخ انتشار خبر را مشخص نمایید");



            var userId = _usersService.GetCurrentUserId();
            return await _newsService.AddOrUpdateNews(model, userId);
        }



        /// <summary>
        /// دریافت کامنت های یک خبر
        /// [p21]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComments")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<NewsCommentDto>>> GetComments(int id)
        {

            return await _newsService.GetComments(id);
        }


        /// <summary>
        /// دریافت کامنت های انتشار داده شده یک خبر
        /// [p22]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPublishComments")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsCommentDto>>> GetPublishComments(int id)
        {

            return await _newsService.GetPublishComments(id);
        }


        /// <summary>
        /// انتشار یا عدم انتشار دیدگاه
        /// [p23]
        /// </summary> 
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> PublishComment(PublishCommen model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _newsService.PublishComment(model.CommentId, model.IsPublish, userId);
        }





        #endregion
        #region Notifications

        /// <summary>
        /// ثبت یا ویرایش اطلاعیه
        /// [p24]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<NotificationDto>> AddOrUpdateNotifications(NotificationDto model)
        {
            if (model == null)
                return new ApiResult<NotificationDto>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");

            if (model.PublishDate == null)
                return new ApiResult<NotificationDto>(false, ApiResultStatusCode.BadRequest, null, "تاریخ شروع اطلاعیه را مشخص نمایید");

            if (model.EndDate == null)
                return new ApiResult<NotificationDto>(false, ApiResultStatusCode.BadRequest, null, "تاریخ پایان اطلاعیه را مشخص نمایید");



            var userId = _usersService.GetCurrentUserId();
            return await _newsService.AddOrUpdateNotifications(model, userId);
        }



        /// <summary>
        /// جستجوی اطلاعیه
        /// [p25]
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="groupId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedNotificationsItems")]
        [Authorize]
        public async Task<ApiResult<PagedNewsItemsViewModel>> GetPagedNotificationsItems
           (int? offset = 1, int? count = 20, int? groupId = null,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string title = null
           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;




            return await _newsService.GetPagedNewsItemsAsync(Common.GlobalEnum.PageType.Notifications,
                offset.Value, count.Value, groupId, FromDate, ToDate, title);

        }

        /// <summary>
        /// دریافت آخرین اطلاعیه
        /// [p26]
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLastNotifications")]
        [AllowAnonymous]
        public async Task<ApiResult<List<NewsList>>> GetLastNotifications(int? top = 20)
        {
            if (top == null || top < 1) top = 20;
            var companyId = _usersService.GetCurrentUserCompanyId();
            return await _newsService.GetLastNotifications(top.Value, companyId);

        }

        /// <summary>
        /// دریافت جزئیات یک اطلاعیه
        /// [p27]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forEdit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNotification")]
        [AllowAnonymous]
        public async Task<ApiResult<NotificationInfo>> GetNotification(int id, bool forEdit = true)
        {
            var companyId = _usersService.GetCurrentUserCompanyId();
            var userId = _usersService.GetCurrentUserId();
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var ip = "";
            if (remoteIpAddress != null)
                ip = remoteIpAddress.ToString();
            return await _newsService.GetNotification(id, companyId, userId, ip, forEdit);

        }








        /// <summary>
        /// اضافه کردن اطلاعیه برای شهروند
        ///  [p28]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> AddCitizensNotifactions(CitizensNotifactionsDto model)
        {
            return await _newsService.AddCitizensNotifactions(model);
        }


        /// <summary>
        /// دریافت لیست شهروندانی که اطلاعیه برای آن صادرشده است
        ///[p29]
        /// </summary>
        /// <param name="id">شناسه اطلاعیه</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenNotifications")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<CitizensNotifactions>>> GetCitizenNotifications(int id)
        {

            return await _newsService.GetCitizenNotifications(id);

        }


        /// <summary>
        /// حذف اطلاعیه ارسالی به شهروند
        /// [p30]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveCitizensNotifactions")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveCitizensNotifactions(int id)
        {
            return await _newsService.RemoveCitizensNotifactions(id);
        }


        #endregion   
        #region WebPage
        /// <summary>
        /// اضافه یا ویرایش صفحه
        /// [p31]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<WebPageDto>> AddOrUpdateWebPage(WebPageDto model)
        {
            if (model == null)
                return new ApiResult<WebPageDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
            var userId = _usersService.GetCurrentUserId();
            return await _webPageService.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// حذف صفحه
        ///  [p31]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveWebPage")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveWebPage(int id)
        {

            return await _webPageService.Remove(id);

        }


        /// <summary>
        /// دریافت جزئیات یک صفحه
        /// [p32]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWebPage")]
        public async Task<ApiResult<WebPageInfo>> GetWebPage(int id)
        {
            return await _webPageService.GetItems(id);
        }

        /// <summary>
        /// دریافت اطلاعات همه صفحات
        /// [p33]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllPagePath")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseData>>> GetAllPagePath()
        {
            return await _webPageService.GetAllPagePath();
        }



        /// <summary>
        /// دریافت اطلاعات صفحه به وسیله مسیر ان
        ///[p34]
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("page/{title?}")]
        public async Task<ApiResult<WebPageInfo>> page(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return new ApiResult<WebPageInfo>(false, ApiResultStatusCode.BadRequest, null, "ادرس صفحه را وارد نمایید");
                ;
            }
            return await _webPageService.GetWebPageByPath(title);
        }



        /// <summary>
        /// دریافت لیست صفحات به صورت دسته بندی
        /// [p35]
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedWebPageItems")]
        public async Task<ApiResult<WebPageItemsViewModel>> GetPagedWebPageItemsAsync(
           int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string title = null
           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _webPageService.GetPagedWebPageItemsAsync(
                offset.Value, count.Value, FromDate, ToDate, title);

        }

        #endregion
        #region SlideShow
        /// <summary>
        /// اضافه یا ویرایش کردن اسلاید شو
        /// [p37]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<SlideShowDto>> AddOrUpdateSlideShow(SlideShowDto model)
        {
            if (model == null)
                return new ApiResult<SlideShowDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
            var userId = _usersService.GetCurrentUserId();
            return await _slidShowService.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// حذف یک اسلاید شو
        ///  [p38]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveSlideShow")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveSlideShow(int id)
        {

            return await _slidShowService.Remove(id);

        }

        /// <summary>
        /// دریافت جزئیات یک اسلاید شو
        ///  [p39]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSlideShow")]
        public async Task<ApiResult<SlieShowInfo>> GetSlideShow(int id)
        {
            return await _slidShowService.GetSlidShow(id);
        }

        /// <summary>
        ///  [p40]
        /// دریافت لیست اسلاید شو پنل مدیریت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSlideShow")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<SlideShowDto>>> GetAllSlideShow()
        {
            return await _slidShowService.GetAll();
        }

        /// <summary>
        ///  [p41]
        /// دریافت اسلاید شود صفحه اول
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllMainPageSlideShow")]
        [AllowAnonymous]
        public async Task<ApiResult<List<SlideShowDto>>> GetAllMainPageSlideShow()
        {
            return await _slidShowService.GetAll(true);
        }



        #endregion
        #region Menu

        /// <summary>
        /// دریافت لیست منو
        ///[p42]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllMenuItems")]
        public async Task<ApiResult<List<MenuItemInfo>>> GetAllMenuItems()
        {
            return await _menuService.GetAll();
        }


        /// <summary>
        /// دریافت جزئیات منو
        ///[p43]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenu")]
        public async Task<ApiResult<MenuItemInfo>> GetMenu(int id)
        {
            return await _menuService.GetMenu(id);
        }

        /// <summary>
        /// دریافت منو به صورت درختی
        /// [p44]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetTreeMenus")]
        public async Task<ApiResult<List<TreeMenu>>> GetTreeMenus()
        {
            return await _menuService.GetTreeMenus();

        }


        /// <summary>
        /// دریافت منو به صورت درختی روش دوم
        /// [p45]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetTreeMenus2")]
        public ApiResult<List<MenuItem>> GetTreeMenus2()
        {
            return _menuService.GetTreeMenus2();

        }

        /// <summary>
        /// اضافه یا ویرایش منو
        /// [p46]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<MenuItemDto>> AddOrUpdateMenuItem([FromBody]MenuItemDto model)
        {
            if (model == null)
                return new ApiResult<MenuItemDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
            var userId = _usersService.GetCurrentUserId();
            return await _menuService.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// مرتب سازی منو
        /// [p47]
        /// </summary>
        /// <param name="tabs"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> UpdateSort(List<SortMenuDto> tabs)
        {
            if (tabs == null)
                return new ApiResult<string>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی خالی است");
            return await _menuService.UpdateSort(tabs, null);
        }


        /// <summary>
        /// حذف منو
        /// [p48]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveMenu")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<string>> RemoveMenu(int id)
        {

            return await _menuService.Remove(id);

        }

        /// <summary>
        /// دریافت منوهای اصلی سایت
        ///[p49]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMainMenuItems")]
        public async Task<ApiResult<List<MenuItemsListDto>>> GetMainMenuItems()
        {
            return await _menuService.GetMainMenuItems();

        }

        #endregion 
      

    }
}