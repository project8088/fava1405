using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.UserDocuments;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.UserDocuments;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nikan.Controllers.Api
{

    /// <summary>
    /// مدیریت پیوست
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AttachmentController : ControllerBase
    {

        private readonly IUsersService _usersService;
        private readonly IAttachmentService _attachmentService;
        private readonly IUserDocumentService _userDocument;
        private readonly IUserDocumentGroupService _userDocumentGroup;
        private readonly IManzalatService _manzalat;

        public static IHostingEnvironment _environment;
        public AttachmentController(IHostingEnvironment environment,
             IUsersService userManager,
             IManzalatService manzalat,
              IAttachmentService attachmentService ,
              IUserDocumentService userDocument,
              IUserDocumentGroupService userDocumentGroup 
            )
        {
            _environment = environment;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _attachmentService = attachmentService ?? throw new ArgumentNullException(nameof(_attachmentService));
            _userDocument = userDocument;
            _manzalat = manzalat;
            _userDocumentGroup = userDocumentGroup; 
        }


      
         
         
        /// <summary>
        /// بارگذاری فایل توسط کاربران عضو
        /// </summary>
        /// <param name="file">فایل</param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadFile")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize()] 
        public  ApiResult<UploadFileResult>  UploadFile(IFormFile file)
        {
            var response = new ApiResult<UploadFileResult>(false, ApiResultStatusCode.Success, new UploadFileResult());
            try
            {
                var ticks = DateTime.Now.Ticks;
                var filesPath = _environment.WebRootPath + "/uploads/Resources/File/"+ ticks;
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }

              
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    var path = "/uploads/Resources/File/" + ticks   + $@"/{ fileName}";
                    string FilePath = _environment.WebRootPath + path; 
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    { 
                        file.CopyTo(fs);
                        fs.Flush();

                    } 
                    response.IsSuccess = true;
                    response.Data.UploadUrl = path;   
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
                return response; 
            }
        }


        /// <summary>
        /// بارگذاری تصویر توسط شهروند
        /// </summary>
        /// <param name="file">مشخصات تصویر</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadImage")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Citizen,Admin")]
        public  ApiResult<UploadFileResult>  UploadImage(IFormFile file)
        {
            var response = new ApiResult<UploadFileResult>(false, ApiResultStatusCode.Success, new UploadFileResult());
            try
            {
                var ticks = DateTime.Now.Ticks;
                var filesPath = _environment.WebRootPath + "/uploads/Resources/Images/" + ticks;
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }


                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string[] supportedTypes = new string[3] { ".jpg", ".jpeg", ".png" };
                    if (!((IEnumerable<string>)supportedTypes).Contains<string>(extension.ToLower()))
                    {
                        response.IsSuccess = false;
                        response.Messages = "  پسوند تصویر ارسالی معتبر نمی باشد";
                        return response;
                    }


                    var path = "/uploads/Resources/Images/" + ticks + $@"/{ fileName}";
                    string FilePath = _environment.WebRootPath + path;
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                    }
                    response.IsSuccess = true; 

                    var shortPath = "/uploads/Resources/Images/" + ticks + "/" + fileName + "_min" + extension;
                    string minPath = _environment.WebRootPath + shortPath;
                    using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
                    image.Mutate(x => x.Resize(208, 264));
                  
                    image.Save(minPath);

                    response.Data.Thumbnail = shortPath;
                    response.Data.UploadUrl = path;

                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
                return response;
            }
        }

    





        /// <summary>
        /// بارگزاری پیوست توسط کاربران عضو همراه با جزئیات فایل
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadAttachment")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize()]
        public async Task<ApiResult<AttachmentInfo>> uploadAttachment([FromForm]AttachmentViewModel model)
        {
            var response = new ApiResult<AttachmentInfo>(true, ApiResultStatusCode.Success, new AttachmentInfo());
            if(model==null)
                 return new ApiResult<AttachmentInfo>(false, ApiResultStatusCode.BadRequest, new AttachmentInfo(),"مدل ورودی خالی است");
         
            if(string.IsNullOrEmpty(model.Guid))
            {
                model.Guid = Guid.NewGuid().ToString();
            }
            
            
            
            try
            {
                var ticks = model.Guid;
                var filesPath = _environment.WebRootPath + "/uploads/Resources/Attachments/" + ticks;
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var file = model.File;
                long filesize = 0;
                filesize = file.Length;
                if (filesize > 0)
                {
                    var extension = Path.GetExtension(file.FileName);

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    if (string.IsNullOrWhiteSpace(model.Caption))
                        model.Caption = fileName;

                    var path = "/uploads/Resources/Attachments/" + ticks + $@"/{ fileName}";
                    string FilePath = _environment.WebRootPath + path;
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                    }



                    return   await _attachmentService.Add(new AttachmentInfo()
                    {
                        AttachmentGroup=model.Guid,
                        Caption=model.Caption,
                        FilePath= path,
                        FileName= fileName,
                        Extension= extension,
                        Size= filesize,
                        ContentType= file.ContentType

                    }, _usersService.GetCurrentUserId());
                     


                    //string minPath = _environment.WebRootPath + "/uploads/news/" + ticks + "/" + filename + "_min" + extension;
                    //using var image = SixLabors.ImageSharp.Image.Load(file[0].OpenReadStream());
                    //image.Mutate(x => x.Resize(208, 264));
                    //image.Save(minPath);


                     
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
                return response;
            }
        }

        /// <summary>
        /// جهت نمایش برای بازدیدکنندگان      
        /// دریافت فایلها
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAttachments")]
        [AllowAnonymous]
        public async Task<ApiResult<List<AttachmentInfo>>> GetAttachments(string guid)
        {
           return  await _attachmentService.GetAll(guid,true,true);
          

        }

        /// <summary>
        /// جهت نمایش برای مدیریت
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<AttachmentInfo>>>  GetAll(string guid)
        {
            return await   _attachmentService.GetAll(guid, null,null);
           

        }

        /// <summary>
        /// جزئیات یک پیوست
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAttachment")]

        public async Task<ApiResult<AttachmentInfo>> GetAttachment(int id  )
        {
            return  await _attachmentService.GetAttachment(id); 
        }


        /// <summary>
        /// حذف پیوست
        /// </summary>
        /// <param name="id">شناسه پیوست</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveAttachment")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<ApiResult<string>> RemoveAttachment(int id)
        {
            return await _attachmentService.Remove(id);
        }


        [HttpGet]
        [Route("RemoveAttachmentByUser")]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<string>> RemoveAttachmentByUser(string guid)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _attachmentService.RemoveByUser(userId,guid);
        }




        [HttpPost]
        [Route("uploadDocGroupAttachment")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<UserDocumentDto>> uploadDocGroupAttachment([FromForm]AttachmentUserDocumentViewModel model)
        {
            var response = new ApiResult<UserDocumentDto>(true, ApiResultStatusCode.Success, new UserDocumentDto());
            if (model == null)
                return new ApiResult<UserDocumentDto>(false, ApiResultStatusCode.BadRequest, new UserDocumentDto(), "مدل ورودی خالی است");
             //فعلا کاربردی نداره به دلایل امنیتی غیرفعال شده است
              return new ApiResult<UserDocumentDto>(false, ApiResultStatusCode.BadRequest, new UserDocumentDto(), "این سرویس غیرفعال می باشد");



            if (model.GroupId == 0)
                return new ApiResult<UserDocumentDto>(false, ApiResultStatusCode.BadRequest, new UserDocumentDto(), "شناسه گروه سند مشخص نشده است");
 
            try
            {
                var userId = _usersService.GetCurrentUserId();
                var ticks = DateTime.Now.Ticks;
                var filesPath = _environment.WebRootPath + "/uploads/Resources/UserDocument/"+ userId+"/" + ticks;
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var file = model.File;
                long filesize = 0;
                filesize = file.Length;
                if (filesize > 0)
                {
                    var extension = Path.GetExtension(file.FileName);

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                   

                    var path = "/uploads/Resources/UserDocument/" + userId + "/" + ticks + $@"/{ fileName}";
                    string FilePath = _environment.WebRootPath + path;
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                    }



                    return await _userDocument.Add(new UserDocumentDto()
                    { 
                        FilePath = path,
                        FileName = fileName,
                        Extension = extension,
                        Size = filesize,
                        AttachedOnDate=DateTime.Now,
                        DocumentGroupId=model.GroupId,
                        OwnerId= userId,
                        Title= fileName, 

                    } ); 

                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
                return response;
            }
        }





        [HttpPost]
        [Route("uploadManzalatAttachment")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<UserDocumentDto>> uploadManzalatAttachment([FromForm]AttachmentUserDocumentViewModel model)
        {
            var response = new ApiResult<UserDocumentDto>(true, ApiResultStatusCode.Success, new UserDocumentDto());
            if (model == null)
                return new ApiResult<UserDocumentDto>(false, ApiResultStatusCode.BadRequest, new UserDocumentDto(), "مدل ورودی خالی است");


           
            try
            {
                var userId = _usersService.GetCurrentUserId();
                //برو ببین کاربر برای این شناسه منزلت ثبت نام داشته است
                var formItem  = await _manzalat.GetCitizenRegisterManzalatForm(userId, model.GroupId);
                if(formItem.IsSuccess)
                {

                    if (formItem.Data.UpoladFiles != null)
                    {
                        response.IsSuccess = false;
                        response.Messages = " برای این طرح قبلا مدارک ارسال شده است ";
                        return response;
                    }
                    if (formItem.Data.UpoladFiles != null)
                    {
                        response.IsSuccess = false;
                        response.Messages = " برای این طرح قبلا مدارک ارسال شده است ";
                        return response;
                    }




                    var ticks = DateTime.Now.Ticks;
                    var filesPath = _environment.WebRootPath + "/uploads/Resources/Manzalat/" + userId + "/"+ model.GroupId + "/" + ticks;
                    if (!System.IO.Directory.Exists(filesPath))
                    {
                        Directory.CreateDirectory(filesPath);
                    }
                    var file = model.File;
                    long filesize = 0;
                    filesize = file.Length;

  


                    if (filesize > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);

                        
                        string[] supportedTypes = new string[3] { ".jpg",".jpeg",".png"};
                        if (!((IEnumerable<string>)supportedTypes).Contains<string>(extension.ToLower()))
                        {
                            response.IsSuccess = false;
                            response.Messages = "  پسوند تصویر ارسالی معتبر نمی باشد";
                            return response;
                        }



                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var path = "/uploads/Resources/Manzalat/" + userId + "/" + model.GroupId + "/" + ticks + $@"/{ fileName}";
                        string FilePath = _environment.WebRootPath + path;
                        using (FileStream fs = System.IO.File.Create(FilePath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();

                        }



                        var add= await _manzalat.AddFile(new ManzalatDocumentInfoViewModel()
                        {
                            FilePath = path,
                            FileName = fileName,
                            Extension = extension,
                            Size = filesize,
                            AttachedOnDate = DateTime.Now, 
                            OwnerId = userId,
                            Title = fileName,
                            ManzalatId= formItem.Data.ManzaltForm.ManzalatRegisterId

                        }, formItem.Data.ManzaltForm.ManzalatRegisterId);
                        if(add.IsSuccess)
                        {
                            await _manzalat.UploadFileForCitizenManzalatForm(userId, model.GroupId);
                            response.IsSuccess = true;
                            response.Messages = "مدارک ارسالی با موفقیت ثبت شد";
                            return response;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Messages = "خطا در   ذخیره مدرک ارسالی!";
                            return response;
                        }
                       

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Messages = "خطا در آپلود فایل!";
                        return response;
                    }
                }
                else if(formItem.StatusCode==ApiResultStatusCode.NotFound)
                {
                    response.Messages = "شما باید ابتدا فرم را تکمیل و در ادامه مدارک را بارگذاری نمایید";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.Messages = formItem.Messages;
                    response.IsSuccess = false;
                    return response;


                }



               
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
                return response;
            }

            
        }


        /*
        [HttpPost]
        [Route("uploadManzalatAttachment2")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<UserDocumentDto>> uploadManzalatAttachment2(
    [FromForm] AttachmentUserDocumentViewModel model)
        {
            ApiResult<UserDocumentDto> response = new ApiResult<UserDocumentDto>(true, ApiResultStatusCode.Success, new UserDocumentDto());
            if (model == null)
                return new ApiResult<UserDocumentDto>(false, ApiResultStatusCode.BadRequest, new UserDocumentDto(), "مدل ورودی خالی است");
            try
            {
                int userId = this._usersService.GetCurrentUserId();
                ApiResult<CitizenReviewManzalatFormItem> formItem = await this._manzalat.GetCitizenRegisterManzalatForm(userId, model.GroupId);
                if (formItem.IsSuccess)
                {
                   
                    long ticks = DateTime.Now.Ticks;
                    string filesPath = AttachmentController._environment.WebRootPath + "/uploads/Resources/Manzalat/" + userId.ToString() + "/" + model.GroupId.ToString() + "/" + ticks.ToString();
                    if (!Directory.Exists(filesPath))
                        Directory.CreateDirectory(filesPath);
                    IFormFile file = model.File;
                    long filesize = 0;
                    filesize = file.Length;
                    if (filesize > 0L)
                    {
                        if (filesize > 485049L)
                        {
                            response.IsSuccess = false;
                            response.Messages = " اندازه مدرک ارسالی بیش از حد مجاز می باشد ";
                            return response;
                        }
                        string extension = Path.GetExtension(file.FileName);
                        string[] supportedTypes = new string[3]
                        {
              ".jpg",
              ".jpeg",
              ".png"
                        };
                        if (!((IEnumerable<string>)supportedTypes).Contains<string>(extension.ToLower()))
                        {
                            response.IsSuccess = false;
                            response.Messages = "  پسوند تصویر ارسالی معتبر نمی باشد";
                            return response;
                        }
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        string path = "/uploads/Resources/Manzalat/" + userId.ToString() + "/" + model.GroupId.ToString() + "/" + ticks.ToString() + "/" + fileName;
                        string FilePath = AttachmentController._environment.WebRootPath + path;
                        using (FileStream fs = File.Create(FilePath))
                        {
                            file.CopyTo((Stream)fs);
                            fs.Flush();
                        }
                        IManzalatService manzalat = this._manzalat;
                        ManzalatDocumentInfoViewModel model1 = new ManzalatDocumentInfoViewModel();
                        model1.FilePath = path;
                        model1.FileName = fileName;
                        model1.Extension = extension;
                        model1.Size = filesize;
                        model1.AttachedOnDate = DateTime.Now;
                        model1.OwnerId = new int?(userId);
                        model1.Title = fileName;
                        model1.ManzalatId = formItem.Data.ManzaltForm.ManzalatRegisterId;
                        int manzalatRegisterId = formItem.Data.ManzaltForm.ManzalatRegisterId;
                        ApiResult add = await manzalat.AddFile(model1, manzalatRegisterId);
                        if (add.IsSuccess)
                        {
                            response.IsSuccess = true;
                            response.Messages = "مدارک ارسالی با موفقیت ثبت شد";
                            return response;
                        }
                        response.IsSuccess = false;
                        response.Messages = "خطا در   ذخیره مدرک ارسالی!";
                        return response;
                    }
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                    return response;
                }
                if (formItem.StatusCode == ApiResultStatusCode.NotFound)
                {
                    response.Messages = "شما باید ابتدا فرم را تکمیل و در ادامه مدارک را بارگذاری نمایید";
                    response.IsSuccess = false;
                    return response;
                }
                formItem = (ApiResult<CitizenReviewManzalatFormItem>)null;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = string.Format("خطا در آپلود فایل: {0}", (object)ex);
                return response;
            }
            return response;
        }
        */

        [HttpGet]
        [Route("RemoveManzalatAttachment")]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult> RemoveManzalatAttachment(int id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult apiResult = await this._manzalat.RemoveManzalatUploadImage(userId, id);
            return apiResult;
        }







    }

}