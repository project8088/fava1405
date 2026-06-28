using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.DomainClasses.Factor;
using Nikan.ViewModel.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services 
{
    public interface ITransactionService
    {
        Task<ApiResult<TransactionDto>> ResultTransaction(string orderId, string refId, TransactionStateEnum state, string description);
        Task<ApiResult<TransactionDto>> Add(TransactionDto model, TransactionStateEnum transactionState = TransactionStateEnum.پرداخت_نشده);
        Task<ApiResult<TransactionInfo>> GetItem(long id);
        Task<ApiResult<PagedTransactionsViewModel>> GetAll(int pageNumber, int pageSize,
            int? companyId = null, DateTime? FromDate = null, 
            DateTime? ToDate = null, string orderId = null,
            string referenceId = null, TransactionStateEnum? transactionState = null,
            TransactionForEnum? transactionFor = null,string  nationCode = null);
        Task<ApiResult> CheckTransaction(string orderId, string refId, TransactionStateEnum state);
        Task<ApiResult> CreateCardForCitizen(int transactionId, int userId);
    }

    public class TransactionService: ITransactionService
    {

        #region Field
        private readonly IUnitOfWork _uow; 
        private readonly DbSet<UserTransaction> _transaction;
        private readonly DbSet<CitizensCard> _citizensCard;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<GroupsCitizens> _citizenGroups;
        private readonly DbSet<Address> _address;



        private readonly DbSet<Group> _group;
        private readonly DbSet<GroupsCitizens> _groupCitizens;


        #endregion
        #region Constractor

        public TransactionService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _transaction = _uow.Set<UserTransaction>();
            _citizensCard = _uow.Set<CitizensCard>();
            _SiteOptions = _uow.Set<SiteOption>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _address = _uow.Set<Address>();


            _group = _uow.Set<Group>();
            _groupCitizens = _uow.Set<GroupsCitizens>();

        }
        #endregion


        public async Task<ApiResult<TransactionDto>> Add(TransactionDto model, TransactionStateEnum transactionState=TransactionStateEnum.پرداخت_نشده)
        {
            var res = new ApiResult<TransactionDto>(true, ApiResultStatusCode.Success, new TransactionDto());
            try
            { 
                    var add = new UserTransaction()
                    {
                         AmountTransaction=model.AmountTransaction,
                         Description=model.Description,
                         OrderId=model.OrderId,
                         TransactionById=model.TransactionById,
                         TransactionState= transactionState,
                         TransactionOnDate =DateTime.Now,
                         TransactionFor=model.TransactionFor, 
                         PaymentDescription= model.Description,
                    };

                    await _transaction.AddAsync(add);
                    await _uow.SaveChangesAsync();
                    res.Data = model;
                    res.Data.Id = add.TransactionId;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult> CreateCardForCitizen(int transactionId,int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت انجام گردید");
            try
            {


                var item =await _transaction.FirstOrDefaultAsync(w => w.TransactionId == transactionId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنشی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                if(item.TransactionState!=TransactionStateEnum.تایید_شده)
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنش  تایید نشده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                if (item.TransactionFor !=   TransactionForEnum.خرید_کارت )
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنش  بابت خرید کارت نیست";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }


                var cards =await _citizensCard.Where(w => w.CitizenId == item.TransactionById).ToListAsync();
                if(cards.Any())
                {
                    if (cards.Any(w => w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه))
                    {
                        res.IsSuccess = false;
                        res.Messages = "قبلا برای این شهروند کارت صادر شده است";
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        return res;
                    }

                    //اخرین درخواست خرید را به وضعیت تاییده شده تبدیل کن
                    var lastCard = cards.LastOrDefault();
                    lastCard.RequestStatuse = CardRequestStatusEnum.درخواست_جدید;
                    _citizensCard.Update(lastCard);
                    await _uow.SaveChangesAsync();
                    return res;
                }
                else
                {

                    var address = await _address.Where(w => w.IsDeleted != true &&
               w.CitizenId == item.TransactionById
               && w.AddressType == AddressTypeEnum.منزل
               && w.IsActive 
               ) .FirstOrDefaultAsync();



                    if(address!=null)
                    {
                        //ببین شهروند منزلت هست یا شهروندی
                        var isManzalat = false;
                        var settings = await _SiteOptions.AsNoTracking().FirstOrDefaultAsync(option => option.Key == "ManzalatGroupId");
                        if (settings != null)
                        {
                            var groupId = int.Parse(settings.Value);
                            isManzalat = await _citizenGroups.AnyAsync(w => w.CitizenId == item.TransactionById && w.GroupId == groupId);
                        }
                        if (isManzalat)
                        {
                            //صدور کارت منزلت
                            var lastCard = await _citizensCard.OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.CardInfo.CardType.Id == 9);
                            if (lastCard != null)
                            {
                                var card = new CitizensCard()
                                {
                                    AdminDescription = "ایجاد شده در بازبینی تراکنش های موفق",
                                    CardInfoId = lastCard.CardInfoId,
                                    CardRequestType = CardRequestTypeEnum.درخواست_جدید,
                                    CitizenId = item.TransactionById.Value,
                                    RequestByCitizenId = userId,
                                    DeliveringAddressId = address.Id,
                                    RequestCode = Guid.NewGuid().ToString(),
                                    RequestDate = DateTime.Now,
                                    RequestStatuse = CardRequestStatusEnum.درخواست_جدید,
                                    TransactionId = item.TransactionId,
                                    DeliverType = DeliverTypeEnum.پستی,
                                };
                                await _citizensCard.AddAsync(card);
                                await _uow.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            //صدور کارت شهروندی
                            var lastCard = await _citizensCard.OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.CardInfo.CardType.Id == 8);
                            if (lastCard != null)
                            {
                                var card = new CitizensCard()
                                {
                                    AdminDescription = "ایجاد شده در بازبینی تراکنش های موفق",
                                    CardInfoId = lastCard.CardInfoId,
                                    CardRequestType = CardRequestTypeEnum.درخواست_جدید,
                                    CitizenId = item.TransactionById.Value,
                                    RequestByCitizenId = userId,
                                    DeliveringAddressId = address.Id,
                                    RequestCode = Guid.NewGuid().ToString(),
                                    RequestDate = DateTime.Now,
                                    RequestStatuse = CardRequestStatusEnum.درخواست_جدید,
                                    TransactionId = item.TransactionId,
                                    DeliverType = DeliverTypeEnum.پستی,
                                };
                                await _citizensCard.AddAsync(card);
                                await _uow.SaveChangesAsync();
                            }

                        }


                    }







                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است"+er.Message;
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }
      


        public async Task<ApiResult<TransactionInfo>> GetItem(long id)
        {
            var res = new ApiResult<TransactionInfo>(true, ApiResultStatusCode.Success, new TransactionInfo());
            try
            {

                var transaction =await _transaction.Where(w => w.TransactionId == id).Select(s => new TransactionInfo()
                {
                    Id=s.TransactionId,
                    AcceptationTransactionOnDate=s.AcceptationTransactionOnDate,
                    OrderId=s.OrderId,
                    AmountTransaction=s.AmountTransaction,
                    TransactionBankReferenceId=s.TransactionBankReferenceId,
                    TransactionBy=s.TransactionBy==null ? "" :s.TransactionBy.Username,
                    TransactionById=s.TransactionById,
                    TransactionFor=s.TransactionFor,
                    TransactionOnDate=s.TransactionOnDate,
                    TransactionState=s.TransactionState,
                    PaymentDescription=s.PaymentDescription 

                }).FirstOrDefaultAsync();

                if(transaction==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنشی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

              

                res.Data = transaction;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }






        public async Task<ApiResult<PagedTransactionsViewModel>> GetAll( 
            int pageNumber, int pageSize, int? citizenId = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string orderId = null,
            string referenceId = null, 
            TransactionStateEnum? transactionState=null,
            TransactionForEnum? transactionFor = null ,
            string  nationCode = null 

            )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedTransactionsViewModel>(true, ApiResultStatusCode.Success, new PagedTransactionsViewModel());
            try
            {

                var query = _transaction.Where(w => w.IsDeleted != true);
                if(citizenId.HasValue)
                {
                    query = query.Where(w => w.TransactionById  == citizenId);
                }
                if (transactionState.HasValue)
                {
                    query = query.Where(w => w.TransactionState == transactionState);
                }

                if (transactionFor.HasValue)
                {
                    query = query.Where(w => w.TransactionFor == transactionFor);
                }
                if (FromDate != null)
                {
                    query = query.Where(w => w.TransactionOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.TransactionOnDate <= ToDate);
                }


                if (!string.IsNullOrWhiteSpace(nationCode))
                {
                    query = query.Where(w => w.TransactionBy.Username == nationCode);
                }

                if (!string.IsNullOrWhiteSpace(orderId))
                {
                    query = query.Where(w => w.OrderId== orderId);
                }

                if (!string.IsNullOrWhiteSpace(referenceId))
                {
                    query = query.Where(w => w.TransactionBankReferenceId == referenceId);
                }

              //  var test = query.AsQueryable();

                var transactions = await query.Select(s => new TransactionInfo()
                {
                    Id = s.TransactionId,
                    AcceptationTransactionOnDate = s.AcceptationTransactionOnDate,
                    OrderId = s.OrderId,
                    AmountTransaction = s.AmountTransaction,
                    TransactionBankReferenceId = s.TransactionBankReferenceId,
                    TransactionBy = s.TransactionBy == null ? "" : s.TransactionBy.Username,
                    TransactionByName = s.TransactionBy == null ? "" : s.TransactionBy.DisplayName,
                    TransactionById = s.TransactionById,
                    TransactionFor = s.TransactionFor,
                    TransactionOnDate = s.TransactionOnDate,
                    TransactionState = s.TransactionState,
                    PaymentDescription=s.PaymentDescription,
                    


                }).OrderByDescending(o=>o.Id) .Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedTransactionsViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Transactions = transactions
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


       

        public async Task<ApiResult<TransactionDto>> ResultTransaction(string orderId,string refId, TransactionStateEnum state,  string description )
        {
            var res = new ApiResult<TransactionDto>(true, ApiResultStatusCode.Success, new TransactionDto());
            try
            {

                var transaction = await _transaction.FirstOrDefaultAsync(w => w.OrderId == orderId);
                if (transaction == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنشی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                if(transaction.TransactionState==TransactionStateEnum.تایید_شده)
                {
                    res.IsSuccess = false;
                    res.Messages = "این تراکنش قبلا تایید شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                transaction.TransactionState = state;
                
                transaction.TransactionBankReferenceId = refId;
                transaction.PaymentDescription = description;
                _transaction.Update(transaction);
                //تایید خرید کارت 
                if (transaction.TransactionFor == TransactionForEnum.خرید_کارت)
                {
                    var card = await _citizensCard.FirstOrDefaultAsync(w => w.TransactionId == transaction.TransactionId);
                    if (card != null)
                    {
                        if (state == TransactionStateEnum.تایید_شده)
                        {
                            card.RequestStatuse = CardRequestStatusEnum.درخواست_جدید;
                            _citizensCard.Update(card);

                            //اضافه کردن شهروند به گروههای خرید کارت
                            //246
                            if (await _group.AnyAsync(a => a.Id == 246))
                            {
                                if (await _groupCitizens.AnyAsync(a => a.Id == 246 && a.CitizenId == card.CitizenId))
                                {
                                    await _groupCitizens.AddAsync(new GroupsCitizens()
                                    {
                                        AddByUserId = 1,
                                        CitizenId = card.CitizenId,
                                        GroupId = 246,
                                        CreationDate = DateTime.Now,
                                    });

                                }

                            }






                        }
                        else
                        {
                           // _citizensCard.Remove(card);
                        }
                    }



                }


                await _uow.SaveChangesAsync();
                res.Data.Id = transaction.TransactionId;
                res.Data.TransactionFor = transaction.TransactionFor;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult> CheckTransaction(string orderId, string refId, TransactionStateEnum state )
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"");
            try
            {

                var transaction = await _transaction.FirstOrDefaultAsync(w => w.OrderId == orderId);
                if (transaction == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "تراکنشی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                transaction.TransactionState = state; 
                transaction.TransactionBankReferenceId = refId;

                if (state == TransactionStateEnum.تایید_شده)
                {
                    //تایید خرید کارت 
                    if (transaction.TransactionFor == TransactionForEnum.خرید_کارت)
                    {
                        var card = await _citizensCard.FirstOrDefaultAsync(w => w.TransactionId == transaction.TransactionId);
                        if (card != null)
                        {
                            if (card.RequestStatuse==CardRequestStatusEnum.درخواست_اولیه)
                            {
                                card.RequestStatuse = CardRequestStatusEnum.درخواست_جدید;
                                _citizensCard.Update(card);
                            }
                            
                        } 
                    } 
                }


                _transaction.Update(transaction); 
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



    }


}
