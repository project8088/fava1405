using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel;
 
using Nikan.ViewModel.Ticket;
 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nikan.ViewModel.Report;
using Nikan.DomainClasses.Citizens;

namespace cle.Services
{

    public  class TicketService:ITicketService
    {
        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Ticket> _ticket;
        private readonly DbSet<TicketComments> _ticketComments;
        private readonly DbSet<TicketActivity> _ticketActivity;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<GroupsCitizens> _groupcitizen; 
        private readonly DbSet<OrganizationalUnitGroups> _unitgroup;



        #endregion
        #region Constractor

        public TicketService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _ticket = _uow.Set<Ticket>();
            _citizen = _uow.Set<Citizen>();
            _unitgroup = _uow.Set<OrganizationalUnitGroups>();
            _groupcitizen = _uow.Set<GroupsCitizens>();
            _ticketComments = _uow.Set<TicketComments>();
            _ticketActivity = _uow.Set<TicketActivity>();
        }
        #endregion


        public async Task<ApiResult<AdminDashbordStatisticalReport>> CountForReport()
        {
            var res = new ApiResult<AdminDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new AdminDashbordStatisticalReport());
            try
            {
                res.Data.AllTicketCount = await _ticket.CountAsync();
                res.Data.ClosedTicketCount = await _ticket.CountAsync(w => w.IsColsed );
                res.Data.NewTicketCount = await _ticket.CountAsync(w =>   w.TicketStatus ==  TicketStatusEnum.جدید);


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }




        public async Task<ApiResult<TicketResultDto>> AddTicket(SendUserTicketDto model, TicketSectionEnum section)
        {
            var res = new ApiResult<TicketResultDto>(true, ApiResultStatusCode.Success, new TicketResultDto());


            if (string.IsNullOrWhiteSpace(model.TicketMessage)  )
            {
                res.IsSuccess = false;
                res.Messages = "متن پیام  را وارد نمایید";
                return res;
            }



            if ( model.TicketMessage.Length < 35 )
            {
                res.IsSuccess = false;
                res.Messages = "شرح درخواست خود را به صورت کامل وارد نمایید";
                return res;
            }


            if (string.IsNullOrWhiteSpace(model.TicketSubjectId))
            {
                model.TicketSubjectId = null;
            }
            if (model.UserId == 0)
                model.UserId = null;



            try
            {


                if(model.UserId==null && ! string.IsNullOrWhiteSpace(model.NationCode))
                {
                    model.NationCode = model.NationCode.Fa2En();
                    var citizen =await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.NationCode == model.NationCode);
                    if(citizen!=null)
                    {
                        model.UserId = citizen.CitizenId;
                    }

                }


               
                var item = new Ticket
                {
                    TicketId = Guid.NewGuid().ToString(),
                    OnDate = DateTime.Now,
                    UserOwnerId = model.UserId,
                    NationCode = model.NationCode,
                    FullName = model.Name,
                    TicketSubjectId = model.TicketSubjectId,
                    Phone = model.MobileNumber,
                    Title = model.Subject,
                    OrganizationalUnitId=model.OrganizationalUnitId,
                    TicketPriority = model.Priority,
                    TicketSection = section,
                    TicketStatus = TicketStatusEnum.جدید,
                    Code = DateTime.Now.ToString("yyyyMMddHHMMssff"),
                   
                };

                item.TicketMessages = new List<TicketMessage>();
                var message = new List<TicketMessage>
            {
                new TicketMessage()
                {
                    Description = model.TicketMessage,
                    OnDate =DateTime.Now,
                    TicketId = item.TicketId,
                    TicketMessageId=item.TicketId,
                    UserOwnerId=model.UserId,
                    AttachmentGuid=model.AttachmentGuid,
                    
                }
            };
                item.TicketMessages = message;
                await _ticket.AddAsync(item);
                await _uow.SaveChangesAsync();
                res.Data.Code = item.Code;
                res.Data.TicketId = item.TicketId;
                res.Messages = item.TicketId;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;
        }

        public async Task<ApiResult<TicketResultDto>> AddContact(ContactDto model, TicketSectionEnum section)
        {

            if (model.UserId == 0)
                model.UserId = null;
            var res = new ApiResult<TicketResultDto>(true, ApiResultStatusCode.Success, new TicketResultDto());
            try
            {
                var item = new Ticket
                {
                    TicketId = Guid.NewGuid().ToString(),
                    OnDate = DateTime.Now,
                    UserCompanyId=model.CompanyId==0 ? null :model.CompanyId,
                    UserOwnerId = model.UserId,
                    Email = model.Email,
                    FullName = model.Name, 
                    Phone = model.MobileNumber,
                    Title = model.Subject, 
                    TicketSection = section,
                    TicketStatus = TicketStatusEnum.جدید,
                    Code = DateTime.Now.ToString("yyyyMMddHHMMssff")
                };

                item.TicketMessages = new List<TicketMessage>();
                var message = new List<TicketMessage>
            {
                new TicketMessage()
                {
                    Description = model.Message,
                    OnDate =DateTime.Now,
                    TicketId = item.TicketId,
                    TicketMessageId=item.TicketId,
                    UserOwnerId=model.UserId, 

                }
            };
                item.TicketMessages = message;
                await _ticket.AddAsync(item);
                await _uow.SaveChangesAsync();
                res.Data.Code = item.Code;
                res.Data.TicketId = item.TicketId;
               
                res.Messages = " ثبت تماس شما با کد پیگیری  " + item.Code + " موفقیت انجام گردید  ";



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;
        }






        public async Task<ApiResult<TicketResultDto>> AddAnswer(ResponseTicketDto model)
        {

            var res = new ApiResult<TicketResultDto>(true, ApiResultStatusCode.Success, new TicketResultDto());

            try
            {

                var ticket = await _ticket.Include(i=>i.TicketMessages)
                    .Include(o=>o.UserOwner)
                    .FirstOrDefaultAsync(w => w.TicketId == model.TicketId) ;
                if (ticket == null)
                {
                    res.Data = null;
                    res.IsSuccess = false;
                    res.Messages = "تیکت یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }


                res.Data.SendSms = model.SendSms;
                res.Data.Code = ticket.Code;
                res.Data.OwnerId = ticket.UserOwnerId;
                if(ticket.UserOwner!=null)
                {
                    res.Data.MobileNumber = ticket.UserOwner.MobileNumber;
                }
               
                if (ticket.TicketMessages != null)
                {
                    ticket.TicketMessages.Add(new TicketMessage()
                    {
                        Description = model.ResponseText,
                        OnDate = DateTime.Now,
                        UserOwnerId = model.OwnerId, 
                        TicketId = model.TicketId,
                        TicketMessageId = Guid.NewGuid().ToString(),
                        AttachmentGuid = model.AttachmentGuid, 

                    });
                }
                 

                if (ticket.UserOwnerId == model.OwnerId)
                {
                    //ارسال پاسخ توسط کاربر
                    ticket.IsSolved = model.Solved;
                    if (ticket.TicketStatus != TicketStatusEnum.جدید)
                    {
                        ticket.TicketStatus = TicketStatusEnum.پاسخ_توسط_کاربر;
                    }
                    res.Data.SendSms = false;


                }
                else
                {
                    //ارسال پاسخ توسط ادمین
                    if (model.Review) ticket.TicketStatus = TicketStatusEnum.در_دست_بررسی;
                    else
                    {
                        ticket.TicketStatus = TicketStatusEnum.پاسخ_داده_شده;
                    }

                }

                ticket.IsSolved = model.Solved; 
                _ticket.Update(ticket);
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


        public async Task<ApiResult<TicketResultDto>> UpdateTicketsStatues(string ticketId, bool IsClosed, int? ownerId)
        {
            var res = new ApiResult<TicketResultDto>(true, ApiResultStatusCode.Success, new TicketResultDto());


            try
            {

                var ticket = await _ticket.FirstOrDefaultAsync(w => w.TicketId == ticketId) ;
                if (ticket == null)
                {
                    res.Data = null;
                    res.IsSuccess = false;
                    res.Messages = "تیکت یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }

                if (IsClosed)
                {
                    ticket.ColsedById = ownerId;
                    ticket.ColsedOnDate = DateTime.Now;
                    ticket.IsColsed = true;
                }
                else
                {
                    ticket.ColsedById = null;
                    ticket.ColsedOnDate = null;
                    ticket.IsColsed = false;
                }

                _ticket.Update(ticket);
                await _uow.SaveChangesAsync();




            }
            catch (Exception)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }
            return res;

        }




        public async Task<ApiResult<string>> AddComments(TicketCommentsDto model,int ownerId )
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = new TicketComments
                {
                    OnDate = DateTime.Now,
                    CommentText = model.CommentText,
                    IsPrivate = model.IsPrivate,
                    TicketId = model.TicketId,
                    UserOwnerId = ownerId,
                };
                await _ticketComments.AddAsync(item);
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

        public async Task<ApiResult<List<TicketCommentsViewModel>>> GetTicketComments(string ticketId,int ownerId )
        {
            var res = new ApiResult<List<TicketCommentsViewModel>>(true, ApiResultStatusCode.Success, new List<TicketCommentsViewModel>());

            try
            {

                var all = new List<TicketCommentsViewModel>();
                var list = await _ticketComments.Where(w => w.TicketId == ticketId).Select(s => new TicketCommentsViewModel()
                {
                    CommentText = s.CommentText,
                    Id = s.Id,
                    IsPrivate = s.IsPrivate,
                    OnDate = s.OnDate,
                    TicketId = s.TicketId,
                    TicketTitle = s.Ticket.Title,
                    UserOwnerId = s.UserOwnerId,
                    UserOwner = s.UserOwner.Username,
                }).ToListAsync();
                foreach (var item in list)
                {
                    if (item.IsPrivate && item.UserOwnerId != ownerId)
                        continue;
                    all.Add(new TicketCommentsViewModel()
                    {
                        CommentText=item.CommentText,
                        Id=item.Id,
                        IsPrivate=item.IsPrivate,
                        OnDate=item.OnDate,
                        TicketId=item.TicketId,
                        TicketTitle=item.TicketTitle,
                        UserOwnerId=item.UserOwnerId,
                        UserOwner=item.UserOwner

                    });

                }

                res.Data = all;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }
            return res;
          
        }


        public async Task<ApiResult<string>> RemoveComments(int id  , int ownerId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item =await _ticketComments.FirstOrDefaultAsync(w => w.Id == id);
                if(item==null)
                {
                    res.Messages = "رکوردی یافت نشد";
                    res.IsSuccess = false;
                    return res;
                }
                if(item.UserOwnerId != ownerId)
                {
                    res.Messages = " شما مجاز به حذف این رکورد نیستید";
                    res.IsSuccess = false;
                    return res;
                }



                _ticketComments.Remove(item);
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



        public async Task<ApiResult<string>> AddActivity(TicketActivityDto model,int  ownerId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            
            try
            {
                var item = new TicketActivity
                {
                    TicketId = model.TicketId,
                    UserOwnerId = ownerId,
                    Description = model.Description,
                    IsSubtractFromContract = model.IsSubtractFromContract,
                    CreatedOnDate = DateTime.Now,
                    OnDate = model.OnDate,
                    PeriodOfMinutes=model.Minute,
                    


                };
                 

               await _ticketActivity.AddAsync(item);
               await _uow.SaveChangesAsync();
                
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;
        }
        public async Task<ApiResult<List<TicketActivityViewModel>>> GetTicketActivity(string ticketId)
        {
            var res = new ApiResult<List<TicketActivityViewModel>>(true, ApiResultStatusCode.Success, new List<TicketActivityViewModel>());

            try
            {
                res.Data =await _ticketActivity.Include(i=>i.UserOwner)
                    .Include(t=>t.Ticket).Where(m => m.TicketId == ticketId) 
           .OrderByDescending(c => c.OnDate).Select(s => new TicketActivityViewModel()
           {
               UserOwnerId=s.UserOwnerId,
               Description=s.Description,
               Id=s.Id,
               IsSubtractFromContract=s.IsSubtractFromContract,
               OnDate=s.OnDate,
               TicketId=s.TicketId,
               UserOwner=s.UserOwner.DisplayName,
               Minute=s.PeriodOfMinutes,
               TicketTitle=s.Ticket.Title

           }).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;


            }

            return res;
           
            
        }

        public async Task<ApiResult<string>> RemoveActivity(int id, int ownerId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");

            try
            {
                var item = await _ticketActivity.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " رکوردی یافت نشد "; 
                }
                if(item.UserOwnerId!= ownerId)
                {
                    res.Messages = " شما مجاز به حذف این رکورد نیستید";
                    res.IsSuccess = false;
                    return res;
                }
                _ticketActivity.Remove(item);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است ";
            }

            return res;
        }



 

     
        public async Task<ApiResult<TicketDetailsDto>> GetTickets(string id)
        {

            ApiResult<TicketDetailsDto> res = new ApiResult<TicketDetailsDto>(true, ApiResultStatusCode.Success, null);

            try
            {
                 


                var info=await _ticket.Where(w=>w.TicketId== id).Select(ticket=>  new TicketDetailsDto()
                {
                    Id = ticket.TicketId,
                    Code = ticket.Code,
                    CreatedOn = ticket.OnDate.ToString(),
                    Priority = ticket.TicketPriority.ToString(),
                    Subject = ticket.Title,
                    OrganizationalUnitId = ticket.OrganizationalUnitId, 
                    TicketSection = ticket.TicketSection.ToString(),
                    TicketStatus = ticket.TicketStatus.ToString(),
                    MobileNumber = ticket.Phone,

                    Email = ticket.Email,
                    City = ticket.City,
                    FullName = ticket.FullName, 
                    ColsedOnDate = ticket.ColsedOnDate == null ? "" : ticket.ColsedOnDate.Value.ToString(),
                    MessageCount = ticket.TicketMessages.Count,
                    IsColsed = ticket.IsColsed,
                    IsSolved = ticket.IsSolved,
                    OrganizationalUnit = new BaseDataModel() { Key = ticket.OrganizationalUnitId, ParentValue = ticket.OrganizationalUnit.OrganizationId, Text = ticket.OrganizationalUnit.Name, ParentText = ticket.OrganizationalUnit.Organization.OrganizationName, },
                    ColsedByUserName = ticket.ColsedBy == null ? "" : ticket.ColsedBy.Username,
                    ColsedById = ticket.ColsedById,
                    OwnerUserName = ticket.UserOwner == null ? "" : ticket.UserOwner.Username,
                    OwnerMobileNumber= ticket.UserOwner == null ? "" : ticket.UserOwner.MobileNumber,
                    OwnerId = ticket.UserOwnerId,
                    ResponseTickets= ticket.TicketMessages.OrderBy(o => o.OnDate).Select(s => new ResponseTicketDto()
                    {
                        MessageId = s.TicketMessageId,
                        ResponseText = s.Description,
                        MobileNumber = ticket.Phone,
                        OwnerId = s.UserOwnerId,
                        AttachmentGuid=s.AttachmentGuid,
                        TicketId=s.TicketId,
                        ResponseTextOnDate = s.OnDate,
                        OwnerDisplayName = s.UserOwner == null ? "" : s.UserOwner.DisplayName
                    }).ToList()

            }).FirstOrDefaultAsync();


                if (info == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تیکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if(info.OwnerId==null)
                {
                    info.OwnerUserName = info.FullName;

                }

                if (string.IsNullOrWhiteSpace(info.MobileNumber))
                {
                    info.MobileNumber = info.OwnerMobileNumber;
                }

                 


                res.Data = info;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی تیکت رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }


        public async Task<ApiResult<ResponseTicketDto>> GetAnswerTicket(string refCode)
        {

            var res = new ApiResult<ResponseTicketDto>(true, ApiResultStatusCode.Success, new ResponseTicketDto());

            try
            {
                var tickets = await _ticket.Include(i=>i.TicketMessages).Where(w => w.Code == refCode).ToListAsync();
                if (tickets == null  || !tickets.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = "تیکتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }



                var ticket = tickets.FirstOrDefault();
                if (tickets.Where(w => w.TicketStatus == TicketStatusEnum.پاسخ_داده_شده).Any())
                {
                    //در صورتیکه تیکت پاسخ داده شده است
                    res.Data = new ResponseTicketDto()
                    {

                        Subject = ticket.Title,
                        ResponseText = ticket.TicketMessages.Any() ? String.Join("###", ticket.TicketMessages.Select(s => s.Description).ToArray()) : " ",
                        ResponseTextOnDate = ticket.TicketMessages.Any() ? ticket.TicketMessages.FirstOrDefault().OnDate : DateTime.Now ,
                    };
                }
                else
                {
                    // در صورتیکه پاسخی برای تیکت ارسال نشده است
                    res.Data = new ResponseTicketDto()
                    {

                        Subject = ticket.Title,
                        ResponseText = "پاسخی برای تیکت شما داده نشده است ",
                        ResponseTextOnDate = DateTime.Now,
                        OwnerDisplayName = ticket.UserOwner == null ? "" : ticket.UserOwner.DisplayName
                    };
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی تیکت رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }

        public async Task<ApiResult<PagedTicketItemsViewModel>> GetAllUserTicket(int  userId)
        {

            var res = new ApiResult<PagedTicketItemsViewModel>(true, ApiResultStatusCode.Success, new PagedTicketItemsViewModel(), "");

            try
            {

                var query = _ticket.Where(w => w.TicketSection == TicketSectionEnum.Ticket
                  && w.UserOwnerId == userId);

                var list = await query.Select(s => new TicketDetailsDto()
                {
                    Code = s.Code,
                    CreatedOn = s.OnDate.ToString(), 
                    Priority = s.TicketPriority.ToString(),
                    Subject = s.Title, 
                    Id = s.TicketId, 
                    TicketSection = s.TicketSection.ToString(),
                    TicketStatus = s.TicketStatus.ToString(),
                    MessageCount = s.TicketMessages.Count,
                    IsColsed = s.IsColsed,
                    IsSolved = s.IsSolved,
                    OwnerId=s.UserOwnerId,
                    OwnerUserName=s.UserOwner.Username,
                    FullName=s.FullName,
                    MobileNumber=s.Phone,
                    OrganizationalUnit = new BaseDataModel() { Key = s.OrganizationalUnitId, ParentValue = s.OrganizationalUnit.OrganizationId, Text = s.OrganizationalUnit.Name, ParentText = s.OrganizationalUnit.Organization.OrganizationName, },
                    OrganizationalUnitId=s.OrganizationalUnitId,
                    

                }).ToListAsync();
                res.Data = new PagedTicketItemsViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Tickets = list
                };
                 

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی تیکت رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }
            return res;




        }
         
        public async Task< ApiResult<PagedTicketItemsViewModel>> 
            GetPagedTicketItemsAsync(int userId,int pageNumber, int pageSize, 
            TicketStatusEnum? ticketStatus=null,
            bool? Iscolsed=null,
            bool? IsSolved=null,
            DateTime? FromDate=null,
            DateTime? ToDate=null,
            string ownerName=null,
             string title = null,
             int? companyId=null
            )
        {
             




            var res = new ApiResult<PagedTicketItemsViewModel>(true, ApiResultStatusCode.Success,  new PagedTicketItemsViewModel() , "");
            try
            {
               


                var groupIds =await _groupcitizen.Where(w => w.CitizenId == userId).Select(s => s.GroupId).ToListAsync();


                if(!groupIds.Any())
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "شما عضو هیچ گروه شهروندی نیستید";
                    return res;
                }

                var unitIds = await _unitgroup.Where(w => groupIds.Contains(w.GroupId)).Select(s => s.OrganizationalUnitId).ToListAsync();

                if(!unitIds.Any())
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "شما عضو هیچ واحد سازمانی نمی باشید";
                    return res;

                }


                 var offset = (pageNumber ) * pageSize ; 
                var query = _ticket.Where(w => unitIds.Contains(w.OrganizationalUnitId) && w.TicketSection == TicketSectionEnum.Ticket);

                if (ticketStatus != null)
                {
                    query = query.Where(w => w.TicketStatus == ticketStatus);
                }

                if (Iscolsed != null)
                {
                    query = query.Where(w => w.IsColsed == Iscolsed);
                }

                if (IsSolved != null)
                {
                    query = query.Where(w => w.IsSolved == IsSolved);
                }

                if (companyId != null)
                {
                    query = query.Where(w => w.UserOwner.UserCompanyId == companyId);
                }


                if (ownerName != null && ! ownerName.Contains("null") )
                {
                    query = query.Where(w => w.UserOwner.Username.Contains(ownerName));
                }
                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                }
                if (!string.IsNullOrEmpty(title) && !title.Contains("null"))
                {
                    query = query.Where(w => EF.Functions.Like(w.Title, "%[(" + title + ")]%"));
                }
                var list = await query.Select(s => new TicketDetailsDto()
                {
                    Code = s.Code,
                    ColsedById = s.ColsedById,
                    ColsedOnDate = s.ColsedOnDate == null ? "" : s.ColsedOnDate.Value.ToString(),
                    ColsedByUserName=s.ColsedBy==null ? "":s.ColsedBy.DisplayName,
                    OrganizationalUnit = new BaseDataModel() { Key = s.OrganizationalUnitId, ParentValue = s.OrganizationalUnit.OrganizationId, Text = s.OrganizationalUnit.Name, ParentText = s.OrganizationalUnit.Organization.OrganizationName, },
                    OrganizationalUnitId = s.OrganizationalUnitId,
                    OwnerId=s.UserOwnerId,
                    TicketSection=s.TicketSection.ToString(),
                    CreatedOn = s.OnDate.ToString(),
                    Email = s.Email,
                    FullName = s.FullName,
                    IsColsed = s.IsColsed,
                    IsSolved = s.IsSolved,
                    Id = s.TicketId,
                    MobileNumber = s.Phone,
                    MessageCount = s.TicketMessages.Count,
                    OwnerUserName = s.UserOwner.Username,
                    Subject = s.Title,
                    Priority = s.TicketPriority.ToString(),
                    TicketStatus = s.TicketStatus.ToString(),
                }).OrderByDescending(o=>o.CreatedOn).Skip(offset).Take(pageSize).ToListAsync();



                res.Data= new PagedTicketItemsViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Tickets = list
                };

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages ="خطایی رخ داده است";


            }



            return res;
      

            

        }

        public async Task<ApiResult<PagedContactViewModel>>
           GetPagedContactAsync(int pageNumber, int pageSize,
           int? companyId = null,
           DateTime? FromDate = null,
           DateTime? ToDate = null, 
            string title = null
           )
        {


            var res = new ApiResult<PagedContactViewModel>(true, ApiResultStatusCode.Success, new PagedContactViewModel(), "");
            try
            {
               


                 var offset = (pageNumber ) * pageSize ;
                var query = _ticket.Where(w => w.TicketSection == TicketSectionEnum.Contact);

                if (companyId != null)
                {
                    query = query.Where(w => w.UserCompanyId == companyId);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                }
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(w => EF.Functions.Like(w.Title, "%[(" + title + ")]%"));
                }
                var list = await query.Select(s => new ContactDetailsDto()
                {
                    
                    Email = s.Email, 
                    Id = s.TicketId,
                    MobileNumber = s.Phone,  
                    Subject = s.Title,
                    CompanyId=s.UserCompanyId,
                    CompanyName=s.UserCompany.CompanyName,
                    Message=s.TicketMessages.FirstOrDefault().Description,
                    Name=s.FullName,
                    OrganizationalUnit=new BaseDataModel() {Key=s.OrganizationalUnitId,ParentValue=s.OrganizationalUnit.OrganizationId,Text=s.OrganizationalUnit.Name,ParentText=s.OrganizationalUnit.Organization.OrganizationName, },
                    UserId=s.UserOwnerId,
                    UserName=s.UserOwner.Username,
                    OrganizationalUnitId=s.OrganizationalUnitId
                }).Skip(offset).Take(pageSize).ToListAsync();



                res.Data = new PagedContactViewModel
                {
                    TotalItems = await query.CountAsync(),
                    ContactList = list
                };

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }



            return res;




        }


        public async Task<ApiResult<ContactDetailsDto>>    GetContactAsync(string  id )
        { 
            var res = new ApiResult<ContactDetailsDto>(true, ApiResultStatusCode.Success, new ContactDetailsDto(), "");
            try
            {
                
                var query = _ticket.Where(w => w.TicketId == id); 
                res.Data= await query.Select(s => new ContactDetailsDto()
                { 
                    
                    Email = s.Email,
                    Id = s.TicketId,
                    MobileNumber = s.Phone,
                    Subject = s.Title,
                    CompanyId = s.UserCompanyId,
                    CompanyName = s.UserCompany.CompanyName,
                    Message = s.TicketMessages.FirstOrDefault().Description,
                    Name = s.FullName,
                    OrganizationalUnit = new BaseDataModel() { Key = s.OrganizationalUnitId, ParentValue = s.OrganizationalUnit.OrganizationId, Text = s.OrganizationalUnit.Name, ParentText = s.OrganizationalUnit.Organization.OrganizationName, },
                    UserId = s.UserOwnerId,
                    UserName = s.UserOwner.Username,
                    OrganizationalUnitId = s.OrganizationalUnitId
                }).FirstOrDefaultAsync();



               

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }



            return res;




        }



        public IEnumerable<Ticket> GetUserTickets(int userId)
      {
          var model = _ticket.Where(m =>   m.UserOwnerId == userId &&
          m.TicketSection== TicketSectionEnum.Ticket ).Include(m=>m.TicketMessages)  .OrderByDescending(c=>c.OnDate);
            return model;
       }

       
        
    
        

       

    }

    public interface ITicketService
    {
        #region Add

        /// <summary>
        /// ارسال تیکت توسط کاربر غیر عضو
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<TicketResultDto>> AddTicket(SendUserTicketDto model, TicketSectionEnum section);


        /// <summary>
        /// ارسال پاسخ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<TicketResultDto>> AddAnswer(ResponseTicketDto model);

      

        /// <summary>
        /// وضعیت جدید تیکت
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="IsClosed"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<ApiResult<TicketResultDto>> UpdateTicketsStatues(string ticketId, bool IsClosed, int? ownerId);



    



        #endregion

       

        



        #region Get


        /// <summary>
        /// دریافت جزئیات یک تیکت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<TicketDetailsDto>> GetTickets(string id);


        /// <summary>
        /// دریافت پاسخ تیکت براساس کد رهگیری
        /// </summary>
        /// <param name="refCode"></param>
        /// <returns></returns>
        Task<ApiResult<ResponseTicketDto>> GetAnswerTicket(string refCode);


        /// <summary>
        /// دریافت  تمامی تیکت های کاربران
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResult<PagedTicketItemsViewModel>> GetAllUserTicket(int userId);




        Task<ApiResult<PagedTicketItemsViewModel>> GetPagedTicketItemsAsync(int userId, int pageNumber, int pageSize,
            TicketStatusEnum? ticketStatus = null,
            bool? Iscolsed = null,
            bool? IsSolved = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string ownerName = null,
             string title = null,
             int? companyId = null
            );





     

        #endregion
        #region Save

        
        Task<ApiResult<string>> AddComments(TicketCommentsDto model, int ownerId);
        Task<ApiResult<List<TicketCommentsViewModel>>> GetTicketComments(string ticketId, int ownerId);
        Task<ApiResult<string>> AddActivity(TicketActivityDto model,int ownerId);
        Task<ApiResult<List<TicketActivityViewModel>>> GetTicketActivity(string ticketId);
        Task<ApiResult<string>> RemoveActivity(int id, int ownerId);
        Task<ApiResult<TicketResultDto>> AddContact(ContactDto model, TicketSectionEnum section);
        Task<ApiResult<PagedContactViewModel>> GetPagedContactAsync(int pageNumber, int pageSize, int? companyId = null, DateTime? FromDate = null, DateTime? ToDate = null, string title = null);
        Task<ApiResult<string>> RemoveComments(int id, int ownerId);
        Task<ApiResult<ContactDetailsDto>> GetContactAsync(string id);
        Task<ApiResult<AdminDashbordStatisticalReport>> CountForReport();

        #endregion
    }






}
