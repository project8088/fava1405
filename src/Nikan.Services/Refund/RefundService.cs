using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.Refund;
using Nikan.Services.ImportFile;
using Nikan.ViewModel.Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.Refund
{
    public interface IRefundService
    {
        Task<ApiResult<RefundDto>> AddRefundFromFileList(RefundDto model, int userId);
        Task<ApiResult> DisagreementRefund(int refundId, int citizenId);
        Task<ApiResult<PagedRefundImportCitizenListViewModel>> RefundAccessPagesList(int pageNumber,
            int pageSize, int importId, DateTime? FromDate = null, DateTime? ToDate = null,
            string nationCode = null,
            string transactionCode = null,
            string orderId = null,
            RefundStateEnum? RefundState = null
            );
        Task<ApiResult<PagedRefundViewModel>> RefundAccessPageList(int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            bool? isClosed = null,
            string unitName = null,
            string nationCode = null,
            string letterNumber = null

            );
        Task<ApiResult<List<ImportRefundList>>> ImportListNotSetiingCard(int importId);
        Task<ApiResult<PagedRefundImportCitizenListViewModel>> OperationImportDetailsList(int pageNumber, int pageSize, int userId, DateTime? FromDate = null, DateTime? ToDate = null, string nationCode = null);
        Task<ApiResult<ImportRefundList>> RefundInfo(int refundId, int userId);
        Task<ApiResult> UpdateRefund(int refundId, long amount, int citizenId, string refundRefCode);
        Task<ApiResult> UpdateRefundCardNumber(int refundId, string card, string transactionXmlInfo);
        Task<ApiResult<ImportRefundList>> UserRefundInfo(int citizenId, int refundId);
        Task<ApiResult<PagedRefundViewModel>> RefundCitizenAccessPageList(int citizenId, int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null, string unitName = null, string letterNumber = null);
        Task<ApiResult<ChangeRefundDto>> UpdateRefundAccess(ChangeRefundDto model, int userId);
        Task<ApiResult<ChangeTransactionRefund>> UpdateRefund(ChangeTransactionRefund model, int userId);
        Task<ApiResult<PagedRefundViewModel>> RefundCitizenAccessPageList(int pageNumber, int pageSize, int citizenId, string letterNumber = null);
        Task<ApiResult<ReportRefund>> ReportRefund(int importId);
        Task<ApiResult> UpdateRefundSaveCardNmber(UpdateRefundCardNumber model, int userId);
        Task<ApiResult<ImportRefundList>> RefundInfoByAdmin(int refundId);
        Task<ApiResult<List<ImportRefundList>>> RefundList(int importId);
        Task<ApiResult<PagedRefundImportCitizenListViewModel>> AllRefundAccessPagesList(int pageNumber, int pageSize, DateTime? FromDate = null,
            DateTime? ToDate = null, string nationCode = null, string unitName = null, string transactionCode = null, string orderId = null,
            RefundStateEnum? refundState = null, string name = null);
        Task<ApiResult<string>> Remove(int Id);
        Task<ApiResult> AddRefund(AddTransactionRefund model, int userId);
    }


    public class RefundService : IRefundService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TransactionRefund> _refund;
        private readonly DbSet<ImportExcelFile>    _importFileExcel;
        private readonly DbSet<RefundImportFileDetails> _refundImportFileDetails;

        private readonly DbSet<TransactionRefundImport> _refundImport;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<CitizenProfile> _citizenProfile;

        #endregion
        #region Constractor

        public RefundService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _refund = _uow.Set<TransactionRefund>(); 
            _refundImport = _uow.Set<TransactionRefundImport>();
            _citizen = _uow.Set<Citizen>();
            _citizenProfile = _uow.Set<CitizenProfile>();
            _importFileExcel = _uow.Set<ImportExcelFile>();
            _refundImportFileDetails = _uow.Set<RefundImportFileDetails>();

        }
        #endregion



        public async Task<ApiResult<RefundDto>> AddRefundFromFileList(RefundDto model, int userId)
        {

            var res = new ApiResult<RefundDto>(true, ApiResultStatusCode.Success, new RefundDto());
            try
            {
                if (model  == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }
                if (model.ImportId==0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه فایل ورودی را مشخص نمایید";
                    return res;
                }
                //برسی تکراری نبوده شماره نامه استرداد
                if(await _refundImport.AnyAsync(w=>w.LetterNumber==model.LetterNumber))
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شماره نامه وارد شده تکراری می باشد";
                    return res;
                }
                var importFileExcel =await _importFileExcel.FirstOrDefaultAsync(w => w.Id == model.ImportId);
                if(importFileExcel==null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "فایلی یافت نشد";
                    return res;
                }



                var citizenId = 0;
                if(model.CitizenId==null)
                {
                    if(model.CitizenAccess!=true)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        res.Messages = "وضعیت دسترسی به استرداد را مشخص نمایید";
                        return res;
                    }

                }
                else
                {
                    //var nationCode = model.NationalCode.Fa2En();
                    //var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                    //if (citizen == null)
                    //{
                    //    res.IsSuccess = false;
                    //    res.StatusCode = ApiResultStatusCode.NotFound;
                    //    res.Messages = "شهروندی با کد ملی وارد شده یافت نشد";
                    //    return res;
                    //}
                    citizenId = model.CitizenId.Value;
                }
                
                   

                var addImportFile = new TransactionRefundImport()
                {
                    ClassName = model.ClassName,
                    ImportByUserId = userId,
                    ImportDescription = model.Description,
                    UnitName = model.UnitName,
                    OnDate = DateTime.Now,
                    LetterNumber=model.LetterNumber,
                    CitizenAccess=model.CitizenAccess==true,
                    
                     
                };
                if(citizenId!=0)
                {
                    addImportFile.AccessByCitizenId = citizenId;
                }
                _refundImport.Add(addImportFile);
                var refundlist = await _refundImportFileDetails.Where(w => w.ImportExcelFileId == importFileExcel.Id).ToListAsync();
                // انتقال محتویات فایل به لیست بازگشت هزینه
                if(refundlist.Any())
                {
                    foreach (var item in refundlist)
                    {
                        await _refund.AddAsync(new TransactionRefund()
                        {
                            AdminDescription="",
                            CitizenId=item.CitizenId.Value ,
                            Description=item.Description,
                            OrderId=item.OrderId,
                            OtherDescription=item.OtherDescription,
                            TotalRefundAmount=item.TotalRefundAmount,
                            RefundAmount=item.RefundAmount,
                            TransactionCode=item.SaleReferenceId,
                            TransactionRefundImport= addImportFile, 
                        });
                    }
                }


                importFileExcel.IsConfirmed = true;
                await _uow.SaveChangesAsync();
                res.Data.Id = addImportFile.RefundImportId;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }

            return res;

        }

        public async Task<ApiResult<ChangeRefundDto>> UpdateRefundAccess(ChangeRefundDto model, int userId)
        {

            var res = new ApiResult<ChangeRefundDto>(true, ApiResultStatusCode.Success, new ChangeRefundDto());
            try
            {
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }
                if (model.Id == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه دسترسی را مشخص نمایید";
                    return res;
                }

                var refundImport = await _refundImport.FirstOrDefaultAsync(w => w.RefundImportId == model.Id);
                if (refundImport == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "دسترسی یافت نشد";
                    return res;
                }



                var citizenId = 0;
                if (string.IsNullOrWhiteSpace(model.NationalCode))
                {
                    if (model.CitizenAccess != true)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        res.Messages = "وضعیت دسترسی به استرداد را مشخص نمایید";
                        return res;
                    }

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
                 
                refundImport.ClassName = model.ClassName; 
                refundImport.ImportDescription = model.Description;
                refundImport.UnitName = model.UnitName; 
                refundImport.LetterNumber = model.LetterNumber;
                refundImport.CitizenAccess = model.CitizenAccess;
                refundImport.IsClosed = model.IsClosed;
                if (citizenId != 0)
                {
                    refundImport.AccessByCitizenId = citizenId;
                }
                _refundImport.Update(refundImport); 
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


        public async Task<ApiResult<ChangeTransactionRefund>> UpdateRefund(ChangeTransactionRefund model, int userId)
        {

            var res = new ApiResult<ChangeTransactionRefund>(true, ApiResultStatusCode.Success, new ChangeTransactionRefund());
            try
            {
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }
                if (model.RefundId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه برگشت را مشخص نمایید";
                    return res;
                }

                var refund = await _refund.FirstOrDefaultAsync(w => w.RefundId == model.RefundId);
                if (refund == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }




                refund.OrderId = model.OrderId;
                refund.TransactionCode = model.TransactionCode;
                refund.TotalRefundAmount = model.TotalRefundAmount;
                refund.RefundAmount = model.RefundAmount;
                refund.Description = model.Description;
                refund.OtherDescription = model.OtherDescription;
                refund.AdminDescription = model.AdminDescription;
                refund.IsClosed = model.IsClosed;
               


                _refund.Update(refund);
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


        public async Task<ApiResult> AddRefund(AddTransactionRefund model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت ثبت گردید");
            try
            {
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }
                if (model.ImportId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه فایل استرداد را مشخص نمایید";
                    return res;
                }

                if (string.IsNullOrWhiteSpace(model.NationCode))
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "کد ملی شهروند را وارد نمایید";
                    return res;
                }
                if (await _refund.AnyAsync(w => w.TransactionCode == model.TransactionCode))
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "این تراکنش قبلا ثبت شده است";
                    return res;
                }

                model.NationCode = model.NationCode.Fa2En();
                var citizen =await _citizen.FirstOrDefaultAsync(w => w.NationCode == model.NationCode);

                if (citizen==null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شهروندی با کد ملی وارد شده یافت نشد";
                    return res;
                }








                var refund = new TransactionRefund()
                {

                    OrderId = model.OrderId,
                    TransactionCode = model.TransactionCode,
                    TotalRefundAmount = model.TotalRefundAmount,
                    RefundAmount = model.RefundAmount,
                    Description = model.Description,
                    OtherDescription = model.OtherDescription,
                    AdminDescription = model.AdminDescription,
                    IsClosed = model.IsClosed,
                    CitizenId= citizen.CitizenId,
                    TransactionRefundImportId=model.ImportId,
                    


                };

                await _refund.AddAsync(refund);
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




        public async Task<ApiResult> UpdateRefundSaveCardNmber(UpdateRefundCardNumber model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "ثبت شماره کارت با موفقیت انجام گردید");
            try
            {
                if (model == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    return res;
                }
                if (model.RefundId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه برگشت را مشخص نمایید";
                    return res;
                }

                var refund = await _refund.FirstOrDefaultAsync(w => w.RefundId == model.RefundId);
                if (refund == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }




                refund.IsClosed = true;//بسته شده
                refund.RefundState =  RefundStateEnum.ثبت_شماره_کارت;//ثبت شماره کارت صورت گرفت
                refund.RefundOnDate = DateTime.Now;
                refund.RefundByUserId = userId;
                refund.OwnerBankCardNumber = model.OwnerBankCardNumber;
                refund.AdminDescription = model.AdminDescription;
                _refund.Update(refund);

                var citizenId = refund.CitizenId;
                var citizenProfile = await _citizenProfile.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                if(citizenProfile!=null)
                {
                    citizenProfile.BankCardNumber = model.OwnerBankCardNumber;
                    _citizenProfile.Update(citizenProfile);
                }
                 


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




        public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> OperationImportDetailsList(
      int pageNumber, int pageSize,
      int userId,
      DateTime? FromDate = null,
      DateTime? ToDate = null,
      string nationCode = null)
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundImportCitizenListViewModel>(true, ApiResultStatusCode.Success, new PagedRefundImportCitizenListViewModel());
            try
            {

                var query = _refund.Where(w => w.TransactionRefundImport.ImportByUserId == userId);


                if (FromDate != null)
                {
                    query = query.Where(w => w.RefundOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.RefundOnDate <= ToDate);
                }


                if (!string.IsNullOrWhiteSpace(nationCode))
                {
                    query = query.Where(w => w.Citizen.NationCode == nationCode);
                }

                var items = await query.Select(s => new ImportRefundList()
                {
                    OwnerCitizenId = s.CitizenId,
                    OwnerUserCode=s.Citizen.UserCode,
                    RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                    Description = s.Description,
                    IsClosed = s.IsClosed,
                    OrderId = s.OrderId,
                    OtherDescription = s.OtherDescription,
                  
                    RefundAmount = s.RefundAmount,
                    RefundByUserId = s.RefundByUserId,
                    RefundCardNumber = s.RefundCardNumber,
                    AdminDescription = s.AdminDescription,
                    TransactionXmlInfo = s.TransactionXmlInfo,
                    RefundId = s.RefundId,
                    RefundState = s.RefundState,
                    RefundOnDate = s.RefundOnDate,
                    TransactionCode = s.TransactionCode,
                    TransactionRefundImportId = s.TransactionRefundImportId,
                    RefundRefCode = s.RefundRefCode,
                    // CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber

                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundImportCitizenListViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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



       public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> RefundAccessPagesList(
       int pageNumber, int pageSize,
       int importId,
       DateTime? FromDate = null,
       DateTime? ToDate = null,
       string nationCode = null,
       string transactionCode = null,
       string orderId = null ,
       RefundStateEnum? refundState = null

       )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundImportCitizenListViewModel>(true, ApiResultStatusCode.Success, new PagedRefundImportCitizenListViewModel());
            if(importId==0)
            {
                res.IsSuccess = false;
                res.Messages = "شناسه فایل مشخص نشده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }
            
            
            try
            {

                var query = _refund.Where(w => w.TransactionRefundImportId == importId);


                if (FromDate != null)
                {
                    query = query.Where(w => w.RefundOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.RefundOnDate <= ToDate);
                }

                if (!string.IsNullOrWhiteSpace(transactionCode))
                {
                    query = query.Where(w => w.TransactionCode == transactionCode);
                }

                if (!string.IsNullOrWhiteSpace(orderId))
                {
                    query = query.Where(w => w.OrderId == orderId);
                }

                if (!string.IsNullOrWhiteSpace(nationCode))
                {
                    query = query.Where(w => w.Citizen.NationCode == nationCode);
                }

                if (refundState != null)
                {
                    query = query.Where(w => w.RefundState == refundState);
                }



                var items = await query.Select(s => new ImportRefundList()
                {

                    OwnerCitizenId = s.CitizenId,
                    RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                    OwnerUserCode=s.Citizen.UserCode,
                    Description = s.Description,
                    IsClosed = s.IsClosed,
                    OrderId = s.OrderId,
                    OtherDescription = s.OtherDescription,
                     
                    RefundAmount = s.RefundAmount,
                    RefundByUserId = s.RefundByUserId,
                  
                    AdminDescription = s.AdminDescription,
                    TransactionXmlInfo = s.TransactionXmlInfo,
                    RefundId = s.RefundId,
                    RefundState = s.RefundState,
                    RefundOnDate = s.RefundOnDate,
                    TransactionCode = s.TransactionCode,
                    TransactionRefundImportId = s.TransactionRefundImportId,
                    RefundRefCode = s.RefundRefCode,
                    OwnerName=s.Citizen.FirstName+" "+s.Citizen.LastName,
                    TotalRefundAmount=s.TotalRefundAmount,
                    RefundByUser=s.RefundByUser.DisplayName,
                    OwnerNationCode=s.Citizen.NationCode,
                    OwnerMobileNumber=s.Citizen.Mobile,
                    RefundCardNumber = s.RefundCardNumber,
                    OwnerBankCardNumber =s.OwnerBankCardNumber,
                    DeclarationCardNumber = s.Citizen.CitizenProfile == null ? "" : s.Citizen.CitizenProfile.BankCardNumber

                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundImportCitizenListViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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







        public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> AllRefundAccessPagesList(
      int pageNumber, int pageSize, 
      DateTime? FromDate = null,
      DateTime? ToDate = null,
      string nationCode = null,
      string unitName = null,
      string transactionCode = null,
      string orderId = null,
      RefundStateEnum? refundState = null,
 string name = null 

	  )
        {


             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundImportCitizenListViewModel>(true, ApiResultStatusCode.Success, new PagedRefundImportCitizenListViewModel());
            
            try
            {

                var query = _refund.AsQueryable();

				if (!string.IsNullOrEmpty(name))
				{
					query = query.Where(w => EF.Functions.Like(w.Citizen.FirstName, "%" + name + "%")
					||
					EF.Functions.Like(w.Citizen.LastName, "%" + name + "%")

					);
				}

				if (FromDate != null)
                {
                    query = query.Where(w => w.RefundOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.RefundOnDate <= ToDate);
                }

                if (!string.IsNullOrWhiteSpace(transactionCode))
                {
                    query = query.Where(w => w.TransactionCode == transactionCode);
                }

                if (!string.IsNullOrWhiteSpace(orderId))
                {
                    query = query.Where(w => w.OrderId == orderId);
                }

                if (!string.IsNullOrWhiteSpace(nationCode))
                {
                    query = query.Where(w => w.Citizen.NationCode == nationCode);
                }

                if (refundState != null)
                {
                    query = query.Where(w => w.RefundState == refundState);
                }

                if (!string.IsNullOrEmpty(unitName))
                {
                    query = query.Where(w => EF.Functions.Like(w.TransactionRefundImport.UnitName, "%" + unitName + "%"));
                }






                var items = await query.Select(s => new ImportRefundList()
                {

                    OwnerCitizenId = s.CitizenId,
                    OwnerUserCode=s.Citizen.UserCode,
                    RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                    Description = s.Description,
                    IsClosed = s.IsClosed,
                    OrderId = s.OrderId,
                    OtherDescription = s.OtherDescription,
                    LetterNumber = s.TransactionRefundImport.LetterNumber,
                    RefundAmount = s.RefundAmount,
                    RefundByUserId = s.RefundByUserId,
                    RefundCardNumber = s.RefundCardNumber,
                    AdminDescription = s.AdminDescription,
                    TransactionXmlInfo = s.TransactionXmlInfo,
                    RefundId = s.RefundId,
                    RefundState = s.RefundState,
                    RefundOnDate = s.RefundOnDate,
                    TransactionCode = s.TransactionCode,
                    TransactionRefundImportId = s.TransactionRefundImportId,
                    RefundRefCode = s.RefundRefCode,
                    OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    TotalRefundAmount = s.TotalRefundAmount,
                    RefundByUser = s.RefundByUser.DisplayName,
                    OwnerNationCode = s.Citizen.NationCode,
                    OwnerMobileNumber = s.Citizen.Mobile,
                    OwnerBankCardNumber = s.OwnerBankCardNumber
                    // CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber

                }).OrderByDescending(o=>o.RefundOnDate).ThenByDescending(o=>o.RefundId).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundImportCitizenListViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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








        public async Task<ApiResult<List<ImportRefundList>>> RefundList(  int importId    )
        {

           
            var res = new ApiResult<List<ImportRefundList>>(true, ApiResultStatusCode.Success, new List<ImportRefundList>());
            if (importId == 0)
            {
                res.IsSuccess = false;
                res.Messages = "شناسه فایل مشخص نشده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }


            try
            {

                var query = _refund.Where(w => w.TransactionRefundImportId == importId); 

                var items = await query.Select(s => new ImportRefundList()
                {

                    OwnerCitizenId = s.CitizenId,
                    OwnerUserCode=s.Citizen.UserCode,
                    RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                    Description = s.Description,
                    IsClosed = s.IsClosed,
                    OrderId = s.OrderId,
                    OtherDescription = s.OtherDescription,
                    RefundAmount = s.RefundAmount,
                    RefundByUserId = s.RefundByUserId,
                    RefundCardNumber = s.RefundCardNumber,
                    AdminDescription = s.AdminDescription,
                    TransactionXmlInfo = s.TransactionXmlInfo,
                    RefundId = s.RefundId,
                    RefundState = s.RefundState,
                    RefundOnDate = s.RefundOnDate,
                    TransactionCode = s.TransactionCode,
                    TransactionRefundImportId = s.TransactionRefundImportId,
                    RefundRefCode = s.RefundRefCode,
                    OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    TotalRefundAmount = s.TotalRefundAmount,
                    RefundByUser = s.RefundByUser.DisplayName,
                    OwnerNationCode = s.Citizen.NationCode,
                    OwnerMobileNumber = s.Citizen.Mobile,
                    OwnerBankCardNumber = s.OwnerBankCardNumber
                   

                }).ToListAsync();
                res.Data = items;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult<ImportRefundList>> RefundInfo(int refundId, int userId)
        {



            var res = new ApiResult<ImportRefundList>(true, ApiResultStatusCode.Success, new ImportRefundList());
            try
            {
                
                if(refundId==0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " شناسه استرداد را مشخص نمایید ";
                    return res;
                }

                var data = await _refund.Where(w => w.TransactionRefundImport.ImportByUserId == userId && w.RefundId == refundId)
                    .Select(s =>

                     new ImportRefundList()
                     {
                         OwnerCitizenId = s.CitizenId,
                         OwnerUserCode=s.Citizen.UserCode,
                         RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                         Description = s.Description,
                         IsClosed = s.IsClosed,
                         OrderId = s.OrderId,
                         OtherDescription = s.OtherDescription,
                          OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                         OwnerNationCode = s.Citizen.NationCode,
                         RefundAmount = s.RefundAmount,
                         RefundByUserId = s.RefundByUserId,
                         RefundCardNumber = s.RefundCardNumber,
                         AdminDescription = s.AdminDescription,
                         TransactionXmlInfo = s.TransactionXmlInfo,
                         RefundId = s.RefundId,
                         RefundState = s.RefundState,
                         RefundOnDate = s.RefundOnDate,
                         TransactionCode = s.TransactionCode,
                         TransactionRefundImportId = s.TransactionRefundImportId,
                         RefundRefCode = s.RefundRefCode,
                         OwnerMobileNumber = s.Citizen.Mobile,
                         TotalRefundAmount = s.TotalRefundAmount,
                         //CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber


                     }).FirstOrDefaultAsync();


                if(data==null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "اطلاعاتی یافت نشد ";
                    return res;
                }


                res.Data = data;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            } 

            return res;

        }



        public async Task<ApiResult<ImportRefundList>> RefundInfoByAdmin(int refundId )
        {




            var res = new ApiResult<ImportRefundList>(true, ApiResultStatusCode.Success, new ImportRefundList());
            try
            {


                if (refundId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " شناسه استرداد را مشخص نمایید ";
                    return res;
                }

                var data = await _refund.Where(w =>   w.RefundId == refundId)
                    .Select(s =>

                     new ImportRefundList()
                     {
                         OwnerCitizenId = s.CitizenId,
                         OwnerUserCode=s.Citizen.UserCode,
                         RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                         Description = s.Description,
                         IsClosed = s.IsClosed,
                         OrderId = s.OrderId,
                         OtherDescription = s.OtherDescription,
                         OwnerName = s.Citizen.FirstName + " " + s.Citizen.LastName,
                         OwnerNationCode = s.Citizen.NationCode,
                         RefundAmount = s.RefundAmount,
                         RefundByUserId = s.RefundByUserId,
                         RefundCardNumber = s.RefundCardNumber,
                         AdminDescription = s.AdminDescription,
                         TransactionXmlInfo = s.TransactionXmlInfo,
                         RefundId = s.RefundId,
                         RefundState = s.RefundState,
                         RefundOnDate = s.RefundOnDate,
                         TransactionCode = s.TransactionCode,
                         TransactionRefundImportId = s.TransactionRefundImportId,
                         RefundRefCode = s.RefundRefCode,
                         OwnerMobileNumber = s.Citizen.Mobile,
                         TotalRefundAmount = s.TotalRefundAmount,
                         //CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber



                     }).FirstOrDefaultAsync();


                if (data == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "اطلاعاتی یافت نشد ";
                    return res;
                }



                res.Data = data;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }

            return res;

        }


        public async Task<ApiResult<string>> Remove(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item  = await _refundImport.FirstOrDefaultAsync(w => w.RefundImportId == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var list =await _refund.Where(w => w.TransactionRefundImportId == Id).ToListAsync();
                if(list.Any(w=>w.RefundState!=null))
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان حذف این دسترسی به دلیل برگشت هزینه وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                _refund.RemoveRange(list);
                _refundImport.RemoveRange(item);
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

        public async Task<ApiResult<ReportRefund>> ReportRefund(int importId)
        {
            var res = new ApiResult<ReportRefund>(true, ApiResultStatusCode.Success, new ReportRefund(), "");
           




            var item = new ReportRefund();
            try
            {
                if (importId == 0)
                {
                    var list = await _refund 
                    .Select(s => new
                    {
                        s.TotalRefundAmount,
                        s.RefundState,
                    })
                    .ToListAsync();

                    item.CountRow = list.Count;
                    item.TotalAmount = list.Sum(s => s.TotalRefundAmount);

                    //واریز به کارت مستقیم
                    item.CountRefund = list.Count(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.برگشت_هزینه);
                    item.AmountRefund = list.Where(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.برگشت_هزینه).Sum(s => s.TotalRefundAmount);

                    //باقی مانده
                    item.AmountRemaining = list.Where(w => w.RefundState == null).Sum(s => s.TotalRefundAmount);
                    item.CountRemaining = list.Count(w => w.RefundState == null);

                    //ثبت شماره کارت
                    item.CountRefundCardNumber = list.Count(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.ثبت_شماره_کارت);
                    item.AmountRefundCardNumber = list.Where(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.ثبت_شماره_کارت).Sum(s => s.TotalRefundAmount);



                    res.Data = item;
                }
                else
                {
                    var list = await _refund
                    .Where(w => w.TransactionRefundImportId == importId)
                   .Select(s => new
                   {
                       s.TotalRefundAmount,
                       s.RefundState,
                   })
                   .ToListAsync();

                    item.CountRow = list.Count;
                    item.TotalAmount = list.Sum(s => s.TotalRefundAmount);

                    //واریز به کارت مستقیم
                    item.CountRefund = list.Count(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.برگشت_هزینه);
                    item.AmountRefund = list.Where(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.برگشت_هزینه).Sum(s => s.TotalRefundAmount);

                    //باقی مانده
                    item.AmountRemaining = list.Where(w => w.RefundState == null).Sum(s => s.TotalRefundAmount);
                    item.CountRemaining = list.Count(w => w.RefundState == null);

                    //ثبت شماره کارت
                    item.CountRefundCardNumber = list.Count(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.ثبت_شماره_کارت);
                    item.AmountRefundCardNumber = list.Where(w => w.RefundState == Common.GlobalEnum.RefundStateEnum.ثبت_شماره_کارت).Sum(s => s.TotalRefundAmount);



                    res.Data = item;
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




        public async Task<ApiResult<List<ImportRefundList>>> ImportListNotSetiingCard(int importId)
        {

            var res = new ApiResult<List<ImportRefundList>>(true, ApiResultStatusCode.Success, new List<ImportRefundList>());
            try
            {
                res.Data = _refund.Where(w => w.TransactionRefundImportId == importId)
                    .Select(s =>
                     new ImportRefundList()
                     {
                         OwnerCitizenId = s.CitizenId,
                         OwnerUserCode=s.Citizen.UserCode,
                         RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                         Description = s.Description,
                         IsClosed = s.IsClosed,
                         OrderId = s.OrderId,
                         OtherDescription = s.OtherDescription,  
                         RefundAmount = s.RefundAmount,
                         RefundByUserId = s.RefundByUserId,
                      

                         RefundCardNumber = s.RefundCardNumber,
                         AdminDescription = s.AdminDescription,
                         TransactionXmlInfo = s.TransactionXmlInfo,
                         RefundId = s.RefundId,
                         RefundState = s.RefundState,
                         RefundOnDate = s.RefundOnDate,
                         TransactionCode = s.TransactionCode,
                         TransactionRefundImportId = s.TransactionRefundImportId,
                         RefundRefCode = s.RefundRefCode,
                        // CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "---" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber

                     }).ToList();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;

        }



        public async Task<ApiResult<PagedRefundViewModel>> RefundCitizenAccessPageList(
        int pageNumber, int pageSize,int citizenId, 
         string letterNumber = null
        )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundViewModel>(true, ApiResultStatusCode.Success, new PagedRefundViewModel());
            try
            {

                var query = _refundImport.Where(w =>w.AccessByCitizenId== citizenId && w.IsDeleted != true); 
                if (!string.IsNullOrWhiteSpace(letterNumber))
                {
                    query = query.Where(w => w.LetterNumber == letterNumber);
                }


                
                var items = await query.Select(s => new RefundInfo()
                {
                    Id = s.RefundImportId,
                    Count = s.TransactionRefunds.Count,
                    CountRefund = s.TransactionRefunds.Count(w => w.RefundState  != null),
                    ClassName = s.ClassName,
                    Description = s.ImportDescription,
                    OnDate = s.OnDate,
                    UnitName = s.UnitName,
                    LetterNumber = s.LetterNumber,
                    CitizenAccess = s.CitizenAccess,
                    IsClosed = s.IsClosed,
                    AccessByCitizenId = s.AccessByCitizenId,
                    AccessByUserCode = s.AccessByCitizen == null ? Guid.Empty : s.AccessByCitizen.UserCode,
                    AccessByCitizen = s.AccessByCitizen == null ? "" : s.AccessByCitizen.FirstName + " " + s.AccessByCitizen.LastName,
                    NationalCode = s.AccessByCitizen == null ? "" : s.AccessByCitizen.NationCode 

                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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




        public async Task<ApiResult<PagedRefundViewModel>> RefundAccessPageList(
          int pageNumber, int pageSize ,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          bool? isClosed = null,
          string unitName = null ,
          string nationCode = null, 
           string letterNumber = null 
          )
        {


             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundViewModel>(true, ApiResultStatusCode.Success, new PagedRefundViewModel());
            try
            {

                var query = _refundImport.Where(w => w.IsDeleted != true);
                if (isClosed != null)
                {
                    query = query.Where(w => w.IsClosed == isClosed);
                }
                if (!string.IsNullOrWhiteSpace(nationCode))
                {
                    query = query.Where(w => w.AccessByCitizen.NationCode == nationCode);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                }
                if (!string.IsNullOrWhiteSpace(letterNumber))
                {
                    query = query.Where(w => w.LetterNumber == letterNumber);
                }


               


                if (!string.IsNullOrEmpty(unitName))
                {
                    query = query.Where(w => EF.Functions.Like(w.UnitName, "%" + unitName + "%")   );
                }




                var items = await query.Select(s => new RefundInfo()
                {
                    Id = s.RefundImportId,
                    Count=s.TransactionRefunds.Count,
                    CountRefund=s.TransactionRefunds.Count(w=>w.RefundState != null),
                    ClassName = s.ClassName,
                    Description = s.ImportDescription,
                    OnDate = s.OnDate,
                    UnitName = s.UnitName,
                    LetterNumber=s.LetterNumber,
                    CitizenAccess=s.CitizenAccess,
                    IsClosed=s.IsClosed,
                    AccessByCitizenId=s.AccessByCitizenId,
                    AccessByUserCode = s.AccessByCitizen == null ? Guid.Empty : s.AccessByCitizen.UserCode,
                    AccessByCitizen =s.AccessByCitizen==null ? "":s.AccessByCitizen.FirstName +" "+s.AccessByCitizen.LastName,
                    NationalCode = s.AccessByCitizen == null ? "" : s.AccessByCitizen.NationCode
                     


                }).OrderByDescending(o=>o.Id).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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


        public async Task<ApiResult<PagedRefundViewModel>> RefundCitizenAccessPageList(
         int citizenId, int pageNumber, int pageSize,
         DateTime? FromDate = null,
         DateTime? ToDate = null,
         string unitName = null,
          string letterNumber = null
         )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedRefundViewModel>(true, ApiResultStatusCode.Success, new PagedRefundViewModel());
            try
            {

                var query = _refundImport.Where(w =>w.AccessByCitizenId== citizenId &&  w.IsDeleted != true);

                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                }
                if (!string.IsNullOrWhiteSpace(letterNumber))
                {
                    query = query.Where(w => w.LetterNumber == letterNumber);
                }


                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    query = query.Where(w => w.UnitName == unitName);
                }

                var items = await query.Select(s => new RefundInfo()
                {
                    Id = s.RefundImportId,
                    ClassName = s.ClassName,
                    Description = s.ImportDescription,
                    OnDate = s.OnDate,
                    UnitName = s.UnitName,
                    LetterNumber = s.LetterNumber,
                    CitizenAccess = s.CitizenAccess,
                    IsClosed = s.IsClosed,
                    AccessByCitizenId = s.AccessByCitizenId,
                    AccessByUserCode = s.AccessByCitizen == null ? Guid.Empty : s.AccessByCitizen.UserCode,
                    AccessByCitizen = s.AccessByCitizen == null ? "" : s.AccessByCitizen.FirstName + " " + s.AccessByCitizen.LastName

                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedRefundViewModel
                {
                    TotalItems = await query.CountAsync(),
                    Items = items
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


        public async Task<ApiResult<List<ImportRefundList>>> UserRefundList(int citizenId)
        {

            var res = new ApiResult<List<ImportRefundList>>(true, ApiResultStatusCode.Success, new List<ImportRefundList>());
            try
            {
                res.Data = await _refund.Where(w => w.IsClosed != true
                && w.RefundState  == null && (w.CitizenId == citizenId 
                // || w.TransactionRefundImport.CitizenId == citizenId
                
                ))
                    .Select(s =>
                     new ImportRefundList()
                     {
                         OwnerCitizenId = s.CitizenId,
                         RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                         OwnerUserCode=s.Citizen.UserCode,
                         Description = s.Description,
                         IsClosed = s.IsClosed,
                         OrderId = s.OrderId,
                         OtherDescription = s.OtherDescription,
                        // OwnerName = s.TBL_Citizens.FirstName + " " + s.TBL_Citizens.LastName,
                        // OwnerNationCode = s.TBL_Citizens.NationCode,
                        // OwnerMobileNumber = s.TBL_Citizens.Mobile,
                         RefundAmount = s.RefundAmount,
                         TotalRefundAmount = s.TotalRefundAmount,
                         RefundByUserId = s.RefundByUserId,
                         RefundCardNumber = s.RefundCardNumber,
                         AdminDescription = s.AdminDescription,
                         TransactionXmlInfo = s.TransactionXmlInfo,
                         RefundId = s.RefundId,
                         RefundState = s.RefundState,
                         RefundOnDate = s.RefundOnDate,
                         TransactionCode = s.TransactionCode,
                         TransactionRefundImportId = s.TransactionRefundImportId,
                        // CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "---" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber

                     }).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;

        }


        public async Task<ApiResult<ImportRefundList>> UserRefundInfo(int citizenId, int refundId)
        {

            var res = new ApiResult<ImportRefundList>(true, ApiResultStatusCode.Success, new ImportRefundList());
            try
            {
                var item = await _refund.Where(w => w.IsClosed != true
                && w.RefundState !=  RefundStateEnum.برگشت_هزینه  && w.RefundId == refundId)
                    .Select(s =>
                     new ImportRefundList()
                     {
                         OwnerCitizenId = s.CitizenId,
                         Description = s.Description,
                         IsClosed = s.IsClosed,
                         OwnerUserCode=s.Citizen.UserCode,
                         RefundByUserCode = s.RefundByUser == null ? Guid.Empty : s.RefundByUser.UserCode,
                         OrderId = s.OrderId,
                         OtherDescription = s.OtherDescription,
                      
                         RefundAmount = s.RefundAmount,
                         TotalRefundAmount = s.TotalRefundAmount,
                         RefundByUserId = s.RefundByUserId,
                         RefundCardNumber = s.RefundCardNumber,
                         TransactionXmlInfo = s.TransactionXmlInfo,
                         AdminDescription = s.AdminDescription,

                         RefundId = s.RefundId,
                         RefundState = s.RefundState,
                         RefundOnDate = s.RefundOnDate,
                         TransactionCode = s.TransactionCode,
                         //CitizenCardNumber = s.TBL_Citizens.TBL_Citizens_Profile == null ? "---" : s.TBL_Citizens.TBL_Citizens_Profile.BankCardNumber,

                         TransactionRefundImportId = s.TransactionRefundImportId,
                     }).FirstOrDefaultAsync();

                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "استردادی برای شما مشخص نشده است";
                    return res;
                }
                res.Data = item;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;

        }

        public async Task<ApiResult> UpdateRefund(int refundId, long amount, int citizenId, string refundRefCode)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var item = await _refund.Where(w => w.RefundId == refundId).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "استردادی برای شما مشخص نشده است";
                    return res;
                }

                item.RefundOnDate = DateTime.Now;
                item.TotalRefundAmount = amount;
                item.RefundByUserId = citizenId;
                item.RefundState  =   RefundStateEnum.برگشت_هزینه;
                item.RefundRefCode = refundRefCode;
                _refund.Update(item);
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

        public async Task<ApiResult> UpdateNote(int refundId, string note)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var item = await _refund.Where(w => w.RefundId == refundId).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " رکوردی یافت نشد";
                    return res;
                }
                item.AdminDescription = note;
                _refund.Update(item);
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





        public async Task<ApiResult> DisagreementRefund(int refundId, int citizenId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                var item = await _refund.Where(w => w.RefundId == refundId).FirstOrDefaultAsync();
                item.RefundOnDate = DateTime.Now;
                item.RefundByUserId = citizenId;
                item.RefundState = RefundStateEnum.ثبت_شماره_کارت;
                _refund.Update(item);
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






        public async Task<ApiResult> UpdateRefundCardNumber(int refundId, string card, string transactionXmlInfo)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            { 
                var item = await _refund.Where(w => w.RefundId == refundId).FirstOrDefaultAsync();
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "استردادی برای شما مشخص نشده است";
                    return res;
                }

                item.RefundCardNumber = card; 
                _refund.Update(item);
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