
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using Nikan.ViewModel;

namespace Nikan.Services
{

    public interface IAttachmentService
    {
        
   
     
        Task<ApiResult<AttachmentInfo>> Add(AttachmentInfo model, int? userOwnerId);
        Task<ApiResult<List<AttachmentInfo>>> GetAll(string guid, bool? isActive , bool? isPublic);
        Task<ApiResult<AttachmentInfo>> GetAttachment(int id);
        Task<ApiResult<string>> Remove(int id);
        Task<ApiResult<string>> RemoveByUser(int userId, string guid);
    }
    public class AttachmentService : IAttachmentService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Attachment> _attachment;
        #endregion
        #region Constractor

        public AttachmentService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _attachment = _uow.Set<Attachment>();
        }
        #endregion

       

        public async   Task<ApiResult<AttachmentInfo>> Add(AttachmentInfo model, int? userOwnerId)
        {
            var res = new ApiResult<AttachmentInfo>(true, ApiResultStatusCode.Success, new AttachmentInfo());

            try
            {
                if (userOwnerId == 0)
                    userOwnerId = null;
                var info = new Attachment()
                {
                    AttachedOn=DateTime.Now,
                    AttachmentGroup=model.AttachmentGroup,
                    Caption=model.Caption,
                    ContentType=model.ContentType,
                    DownloadsCount=0,
                    Extension=model.Extension,
                    FileName=model.FileName,
                    DurationMinute=0,
                    FilePath=model.FilePath,
                    IndexOrder=0,
                    IsActive=true,
                    Size=model.Size,
                    UserId=userOwnerId,
                    ThumnailPath="",
                    IsDeleted=false,
                    ModifiedOn=DateTime.Now

                };
              await  _attachment.AddAsync(info);
              await _uow.SaveChangesAsync();
                res.Data = new AttachmentInfo()
                {
                    Id=info.Id,
                    Caption=info.Caption,
                    AttachedOn=info.AttachedOn,
                    AttachmentGroup=info.AttachmentGroup,
                    FilePath=info.FilePath,
                    Extension=info.Extension,
                    FileName=info.FileName,
                    ContentType=info.ContentType,
                    Size=info.Size,
                    
                };


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است"; 
            }

            return res;
            
        }
       
        

        public async Task<ApiResult<List<AttachmentInfo>>> GetAll(string guid,bool? isActive,bool? isPublic  )
        {
            var res = new ApiResult<List<AttachmentInfo>>(true, ApiResultStatusCode.Success, new List<AttachmentInfo>());

            try
            {
                var query = _attachment.Where(w => w.IsDeleted != true && w.AttachmentGroup == guid);
                if (isActive.HasValue)
                    query = query.Where(w => w.IsActive == isActive.Value);

                if (isPublic.HasValue)
                    query = query.Where(w => w.IsPublic == isPublic.Value);



                res.Data = await query.Select(s => new AttachmentInfo()
            {

                AttachedOn = s.AttachedOn,
                AttachmentGroup = s.AttachmentGroup,
                Caption = s.Caption ?? s.FileName,
                ContentType = s.ContentType,
                Extension = s.Extension,
                FileName = s.FileName,
                FilePath = s.FilePath,
                Id = s.Id,
                Size = s.Size,
                UserName=s.User==null ? "":s.User.Username,
                UserId=s.UserId,
                DownloadsCount=s.DownloadsCount

            }).ToListAsync();

            }
            catch (Exception er)
            {
                res.Data = null;
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                
            }
            return res;
        }

        public async Task<ApiResult<AttachmentInfo>> GetAttachment(int id)
        {
            var res = new ApiResult<AttachmentInfo>(true, ApiResultStatusCode.Success, new AttachmentInfo());

            try
            {
                var query = _attachment.Where(w => w.IsDeleted != true && w.Id == id); 
                res.Data = await query.Select(s => new AttachmentInfo()
                {

                    AttachedOn = s.AttachedOn,
                    AttachmentGroup = s.AttachmentGroup,
                    Caption = s.Caption ?? s.FileName,
                    ContentType = s.ContentType,
                    Extension = s.Extension,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    Id = s.Id,
                    Size = s.Size,
                    UserName = s.User == null ? "" : s.User.Username,
                    UserId = s.UserId,
                    DownloadsCount = s.DownloadsCount

                }).FirstOrDefaultAsync();

            }
            catch (Exception er)
            {
                res.Data = null;
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }
            return res;
        }

        public async Task<ApiResult<string>> Remove(int  id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "حذف   با موفقیت صورت گرفت");
            try
            {
                var item = await _attachment.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }



                item.IsDeleted = true;
                _attachment.Update(item);
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


        public async Task<ApiResult<string>> RemoveByUser(int userId, string guid)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "حذف   با موفقیت صورت گرفت");
            try
            {
                var item = await _attachment.FirstOrDefaultAsync(w => w.UserId == userId && w.AttachmentGroup == guid);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }



                item.IsDeleted = true;
                _attachment.Update(item);
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
