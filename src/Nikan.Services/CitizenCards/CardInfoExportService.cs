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
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.CitizenCards
{


    public interface ICardInfoExportService
    {
        Task<ApiResult<List<CitizensCardInfo>>> GetExportDetailsList(int exportId);
        Task<ApiResult<PagedCardInfoExport>> GetPagedCardInfoExport(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<ApiResult<PagedCitizensCardViewModel>> GetPagedCardInfoExportDetails(int pageNumber, int pageSize, int exportId, string name = null, string nationCode = null, string cardNumber = null, int? cardTypeId = null);
        Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedManzalatCardInfoExportDetailsForSend(int pageNumber, int pageSize, int exportId, string name = null, string nationCode = null, string cardNumber = null, int? cardTypeId = null);
        Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedShahrvandiCardInfoExportDetailsForSend(int pageNumber, int pageSize, int exportId, string name = null, string nationCode = null, string cardNumber = null, int? cardTypeId = null);
        Task<ApiResult<UploadFileResult>> ImportExcelCardNumber(List<ImportExcelCardNumber> listImport, int importId, int userId);
        Task<ApiResult<string>> RemoveCardInExportList(int id);
        Task<ApiResult<string>> RemoveExport(int exportId);
        Task<ApiResult> SendCardToPrint(int exportId);
    }
    public class CardInfoExportService: ICardInfoExportService
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
        private readonly DbSet<CardInfo_Export> _cardExport;
        private readonly DbSet<CardInfo_Export_Citizen> _cardExportDetails;
        private readonly DbSet<Manzalat> _manzalat;



        public CardInfoExportService(IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _profile = _uow.Set<CitizenProfile>();
            _manzalat = _uow.Set<Manzalat>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _citizensCard = _uow.Set<CitizensCard>();
            _cardInfo_PermissionsForGroups = _uow.Set<CardInfo_PermissionsForGroups>();
            _group = _uow.Set<Group>();
            _groupCitizens = _uow.Set<GroupsCitizens>();
            _cardInfo = _uow.Set<CardInfo>();
            _cardType = _uow.Set<CardType>();

            _cardExport = _uow.Set<CardInfo_Export>();
            _cardExportDetails = _uow.Set<CardInfo_Export_Citizen>();


        }

        #endregion


        public async Task<ApiResult<PagedCardInfoExport>> GetPagedCardInfoExport(
              int pageNumber, int pageSize, 
           DateTime? FromDate = null,
           DateTime? ToDate = null
           )
        {


             var offset = (pageNumber ) * pageSize ;
          

            var res = new ApiResult<PagedCardInfoExport>(true, ApiResultStatusCode.Success, null);

            try
            {
                var query = _cardExport.AsQueryable();

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreationDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreationDate <= ToDate);
                }

                var items = await query.Select(s => new CardInfoExportViewModel()
                {

                   ConfirmedData=s.ConfirmedData,
                   CreationDate=s.CreationDate,
                   DateReceive=s.DateReceive,
                   IsReceive=s.DateReceive!=null,
                   IsSend=s.DateSend!=null,
                   DateSend=s.DateSend,
                   Description=s.Description,
                   Id=s.Id,
                   ExporterByUser=s.ExporterByUser==null ? "":s.ExporterByUser.DisplayName,
                   CountExport=s.CardInfo_Export_Citizen.Count,
                   ExporterByUserId=s.ExporterByUserId,
                   ImporterByUser = s.ImporterByUser == null ? "" : s.ImporterByUser.DisplayName,
                   ImporterByUserId=s.ImporterByUserId,
                   Export_Number=s.ExportNumber,
              



                }).OrderByDescending(w => w.Id).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedCardInfoExport
                {
                    TotalItems = items.Count,
                    Items = items
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



        public async Task<ApiResult<PagedCitizensCardViewModel>> GetPagedCardInfoExportDetails(
      int pageNumber, int pageSize,
      int exportId,
      string name = null,
      string nationCode = null,
      string cardNumber = null,
      int? cardTypeId = null
      )
        {

            var res = new ApiResult<PagedCitizensCardViewModel>(true, ApiResultStatusCode.Success, new PagedCitizensCardViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _cardExportDetails.AsQueryable();
                var ids = await _cardExportDetails.Where(w => w.ExportCardId == exportId).Select(s => s.CitizenCardInfoId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(w => ids.Contains(w.CitizenCardInfoId));
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "در این دوره کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.CitizenCardInfo.Citizen.NationCode, "%" + nationCode + "%"));
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




                if (!string.IsNullOrEmpty(cardNumber))
                {
                    query = query.Where(w => w.CitizenCardInfo.CardNumber == cardNumber);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new CitizensCardInfo()
                {
                   
                    Id = s.Id,
                    CardInfoId = s.CitizenCardInfo.CardInfoId,
                    CitizenId = s.CitizenCardInfo.CitizenId,
                    CitizenCardInfoId = s.CitizenCardInfoId,


                   
                    BarCode = s.CitizenCardInfo.BarCode,
                    CardActivationDate = s.CitizenCardInfo.CardActivationDate,
                    CardExpirationDate = s.CitizenCardInfo.CardExpirationDate,
                   
                    CardNumber = s.CitizenCardInfo.CardNumber,//
                    DeliveredOnDate = s.CitizenCardInfo.DeliveredOnDate,
                    CardRequestType = s.CitizenCardInfo.CardRequestType,
                   
                    CardSerial = s.CitizenCardInfo.CardSerial,//
                    DeliveredByOperationId = s.CitizenCardInfo.DeliveredByOperationId,
                    DeliveredByOperation = s.CitizenCardInfo.DeliveredByOperation.DisplayName,
                    DeliveredDescription = s.CitizenCardInfo.DeliveredDescription,
                    NationCode = s.CitizenCardInfo.Citizen.NationCode,//
                    
                    DeliverType = s.CitizenCardInfo.DeliverType,
                    RequestCode = s.CitizenCardInfo.RequestCode,
                  
                    RequestByCitizen = s.CitizenCardInfo.RequestByCitizen.FirstName + " " + s.CitizenCardInfo.RequestByCitizen.LastName,
                    RequestStatuse = s.CitizenCardInfo.RequestStatuse,
                    RequestDate = s.CitizenCardInfo.RequestDate,
                    PreCardNumber = s.CitizenCardInfo.PreCardNumber,
                    RequestByCitizenId = s.CitizenCardInfo.RequestByCitizenId,
                    IsSetBarCode = s.CitizenCardInfo.IsSetBarCode,
                    DiscountGroupId = s.CitizenCardInfo.DiscountGroupId,
                    CardTitle = s.CitizenCardInfo.CardInfo.CardType.Title,
                    UserCode=s.CitizenCardInfo.Citizen.UserCode,

                    Citizen = s.CitizenCardInfo.Citizen.FirstName + " " + s.CitizenCardInfo.Citizen.LastName,
                    FirstName = s.CitizenCardInfo.Citizen.FirstName,
                    LastName = s.CitizenCardInfo.Citizen.LastName,
                    Mobile = s.CitizenCardInfo.Citizen.Mobile,
                    Gender = s.CitizenCardInfo.Citizen.Gender == true ? "آقا" : "خانم", 
                    DeliveringCenterId = s.CitizenCardInfo.DeliveringCenterId,
                    DeliveringCenter = s.CitizenCardInfo.DeliveringCenter.Name,
                    DeliveringAddressId = s.CitizenCardInfo.DeliveringAddressId,

                    Region=s.CitizenCardInfo.DeliveringAddress==null ? 0: s.CitizenCardInfo.DeliveringAddress.Region,
                    FullAddress = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.FullAddress,
                    Street = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Street,
                    Alley = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Alley,
                    PostalCode = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.PostalCode,
                    Plaque = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Plaque,
                    Phone = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.Phone,
                    DeliveringAddress = s.CitizenCardInfo.DeliveringAddress == null ? "" : s.CitizenCardInfo.DeliveringAddress.FullAddress,


                    ImageUrl = "/uploads/Resources/Citizens/" + s.CitizenCardInfo.Citizen.UserCode + "/" + s.CitizenCardInfo.Citizen.NationCode + ".jpg",
                     AdminDescription = s.CitizenCardInfo.AdminDescription,

                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }



        public async Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedShahrvandiCardInfoExportDetailsForSend(
        int pageNumber, int pageSize,
        int  exportId  , 
        string name = null,
        string nationCode = null,  
        string cardNumber = null, 
        int? cardTypeId = null 
        )
        {

            var res = new ApiResult<PagedCitizensCardForSendPrintCard>(true, ApiResultStatusCode.Success, new PagedCitizensCardForSendPrintCard());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _cardExportDetails.AsQueryable(); 
                var ids = await _cardExportDetails.Where(w => w.ExportCardId == exportId).Select(s => s.CitizenCardInfoId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(w => ids.Contains(w.CitizenCardInfoId));
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "در این دوره کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.CitizenCardInfo.Citizen.NationCode, "%" + nationCode + "%"));
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

                
                

                if (!string.IsNullOrEmpty(cardNumber))
                {
                    query = query.Where(w => w.CitizenCardInfo.CardNumber == cardNumber);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new FormatExportCardInfo()
                {
                   
                    Id = s.Id,
                    CitizenCardInfoId = s.CitizenCardInfoId, 
                    CitizenId = s.CitizenCardInfo.CitizenId, 
                    NationCode = s.CitizenCardInfo.Citizen.NationCode,
                    CitizenFullName = s.CitizenCardInfo.Citizen.FirstName + " " + s.CitizenCardInfo.Citizen.LastName, 
                    RequestCode = s.CitizenCardInfo.RequestCode,
                    CitizenFirstName = s.CitizenCardInfo.Citizen.FirstName,
                    CitizenLastName= s.CitizenCardInfo.Citizen.LastName,
                    ExpaireDate="", 

                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }





        public async Task<ApiResult<PagedCitizensCardForSendPrintCard>> GetPagedManzalatCardInfoExportDetailsForSend(
      int pageNumber, int pageSize,
      int exportId,
      string name = null,
      string nationCode = null,
      string cardNumber = null,
      int? cardTypeId = null
      )
        {

            var res = new ApiResult<PagedCitizensCardForSendPrintCard>(true, ApiResultStatusCode.Success, new PagedCitizensCardForSendPrintCard());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _cardExportDetails.AsQueryable();
                var ids = await _cardExportDetails.Where(w => w.ExportCardId == exportId).Select(s => s.CitizenCardInfoId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(w => ids.Contains(w.CitizenCardInfoId));
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "در این دوره کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }


                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.CitizenCardInfo.Citizen.NationCode, "%" + nationCode + "%"));
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




                if (!string.IsNullOrEmpty(cardNumber))
                {
                    query = query.Where(w => w.CitizenCardInfo.CardNumber == cardNumber);
                }

                res.Data.TotalItems = await query.CountAsync();
                var items = await query.Select(s => new FormatExportCardInfo()
                {

                    Id = s.Id,
                    CitizenCardInfoId = s.CitizenCardInfoId,
                    CitizenId = s.CitizenCardInfo.CitizenId,
                    NationCode = s.CitizenCardInfo.Citizen.NationCode,
                    CitizenFullName = s.CitizenCardInfo.Citizen.FirstName + " " + s.CitizenCardInfo.Citizen.LastName,
                    RequestCode = s.CitizenCardInfo.RequestCode,
                    CitizenFirstName = s.CitizenCardInfo.Citizen.FirstName,
                    CitizenLastName = s.CitizenCardInfo.Citizen.LastName,
                    ExpaireDate = "",

                }).OrderByDescending(w => w.CitizenId).Skip(offset).Take(pageSize).ToListAsync();

                if (items.Any())
                {

                    var citizenids = items.Select(s => s.CitizenId).ToList();


                    var manzalatList = await _manzalat.Where(w => citizenids.Contains(w.CitizenId)).ToListAsync();

                    foreach (var item in items)
                    {
                        var manzalat = manzalatList.Where(w =>w.FormStatuse==Common.GlobalEnum.ManzalatFormStatuseEnum.تایید && w.CitizenId == item.CitizenId).FirstOrDefault();
                        if(manzalat!=null)
                        {
                            if (manzalat.FormStatuse == Common.GlobalEnum.ManzalatFormStatuseEnum.تایید)
                                item.TotalResult = "1";
                            else
                                item.TotalResult = "0";


                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.بازنشسته)
                            {
                                item.IsBazneshasteh = "1";
                                item.ManzelatStatus = "B";
                            }
                            else
                                item.IsBazneshasteh = "0";

                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.جانبازان)
                            {
                                item.IsJanbaz = "1";
                                item.ManzelatStatus = "J";
                            } 
                            else
                                item.IsJanbaz = "0";


                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.زنان_سرپرست_خانواده)
                            {
                                item.IsZanSarperst = "1";
                                item.ManzelatStatus = "Z";
                            }
                            else
                                item.IsZanSarperst = "0"; 

                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.سالمند)
                            {
                                item.IsSalmand = "1";
                                item.ManzelatStatus = "S";
                            }
                            else
                                item.IsSalmand = "0";


                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.معلولین)
                            {
                                item.IsMaloul = "1";
                                item.ManzelatStatus = "M";
                            }
                            else
                                item.IsMaloul = "0";



                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.بیماران_خاص)
                            {
                                item.IsBimar = "1";
                                item.ManzelatStatus = "KH";
                            }
                            else
                                item.IsBimar = "0";


                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.مادران_دارای_سه_فرزند)
                            {
                                item.IsMadar = "1";
                                item.ManzelatStatus = "MF";
                            }
                            else
                                item.IsMadar = "0";


                            if (manzalat.ManzalatBaseFormId == (int)ManzalatFormTypeEnum.دانش_آموزان_تحت_پوشش_کمیته_امداد_امام_خمینی_و_سازمان_بهزیستی)
                            {
                                item.IsMadar = "1";
                                item.ManzelatStatus = "ST";
                            }
                            else
                                item.IsMadar = "0";


                        } 
                    
                        else
                        {
                            //بیماران که فرم منزلت را پر نکرده باشند 
                            if (await this._groupCitizens.AnyAsync(a => a.GroupId > 207 && a.GroupId < 215 && a.CitizenId == item.CitizenId))
                            {

                                item.TotalResult = "1";
                                item.IsMaloul = "1";
                                item.ManzelatStatus = "M";
                            }


                        }
                    
                    
                    
                    }
                }



                res.Data.Items = items;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }















        public async Task<ApiResult<UploadFileResult>> ImportExcelCardNumber(List<ImportExcelCardNumber> listImport,int importId,int userId)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success,new UploadFileResult());

            try
            {

                var cardExport = await _cardExport.FirstOrDefaultAsync(w => w.Id == importId);
                if(cardExport==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "خروجی صدور کارت یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var isUpdate = false;
                var list = await _cardExportDetails.Include(i=>i.CitizenCardInfo).ThenInclude(t=>t.Citizen).Where(w => w.ExportCardId == importId).ToListAsync();
                foreach (var item in list)
                {
                    var card  = item.CitizenCardInfo;
                    var import = listImport.Where(w=>w.NationCode == card.Citizen.NationCode).FirstOrDefault();
                    if(import!=null)
                    {
                        card.CardActivationDate = import.CardActivationDate;
                        card.CardNumber = import.CardNumber;
                        card.CardSerial = import.CardSerial; 
                        card.CardExpirationDate = import.CardExpirationDate;
                        card.RequestStatuse = Common.GlobalEnum.CardRequestStatusEnum.چاپ_کارت;
                        _citizensCard.Update(card);
                        isUpdate = true;

                    }


                }
                if(isUpdate)
                {
                    cardExport.ImporterByUserId = userId;
                    cardExport.DateReceive = DateTime.Now;
                    cardExport.ConfirmedData = DateTime.Now;
                    _cardExport.Update(cardExport);
                    await _uow.SaveChangesAsync();
                }
              
                res.Data.ImportId = importId;
              

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
                return res;

            }

            return res;
        }

       public async Task<ApiResult<List<CitizensCardInfo>>> GetExportDetailsList(int exportId )
        {

            var res = new ApiResult<List<CitizensCardInfo>>(true, ApiResultStatusCode.Success, new List<CitizensCardInfo>());
            try
            {
                
                var query = _cardExportDetails.AsQueryable();
                var ids = await _cardExportDetails.Where(w => w.ExportCardId == exportId).Select(s => s.CitizenCardInfoId).ToListAsync();
                if (ids.Any())
                {
                    query = query.Where(w => ids.Contains(w.CitizenCardInfoId));
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "در این دوره کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

 
               
                res.Data = await query.Select(s => new CitizensCardInfo()
                {
                    DeliveringCenterId = s.CitizenCardInfo.DeliveringCenterId,
                    DeliveringCenter = s.CitizenCardInfo.DeliveringCenter.Name,
                    DeliveringAddressId = s.CitizenCardInfo.DeliveringAddressId,
                    Id = s.CitizenCardInfo.Id,//شناسه کارت
                    AdminDescription = s.CitizenCardInfo.AdminDescription,
                    BarCode = s.CitizenCardInfo.BarCode,
                    CardActivationDate = s.CitizenCardInfo.CardActivationDate,
                    CardExpirationDate = s.CitizenCardInfo.CardExpirationDate,
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
                    DeliverType = s.CitizenCardInfo.DeliverType,
                    RequestCode = s.CitizenCardInfo.RequestCode,
                    DeliveringAddress = s.CitizenCardInfo.DeliveringAddress.FullAddress,
                    RequestByCitizen = s.CitizenCardInfo.RequestByCitizen.FirstName + " " + s.CitizenCardInfo.RequestByCitizen.LastName,
                    RequestStatuse = s.CitizenCardInfo.RequestStatuse,
                    RequestDate = s.CitizenCardInfo.RequestDate,
                    PreCardNumber = s.CitizenCardInfo.PreCardNumber,
                    RequestByCitizenId = s.CitizenCardInfo.RequestByCitizenId,
                    IsSetBarCode = s.CitizenCardInfo.IsSetBarCode,
                    DiscountGroupId = s.CitizenCardInfo.DiscountGroupId,
                    CardTitle = s.CitizenCardInfo.CardInfo.CardType.Title,

                }).OrderByDescending(w => w.CitizenId).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }


        public async Task<ApiResult> SendCardToPrint(int exportId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success,"ارسال برای چاپ با موفقیت انجام گردید");
            try
            {

              
                var export  = await _cardExport 
                    .Where(w => w.Id == exportId).FirstOrDefaultAsync();
                if(export==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "خروجی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }
                
                if(export.DateSend!=null)
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا برای چاپ کارت ارسال شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }

                var exportCard =await _cardExportDetails.Include(i=>i.CitizenCardInfo).Where(w => w.ExportCardId == exportId).ToListAsync();
                foreach (var card in exportCard)
                {
                    var citizenCard = card.CitizenCardInfo;
                    citizenCard.RequestStatuse = Common.GlobalEnum.CardRequestStatusEnum.ارسال_برای_چاپ;
                    _citizensCard.Update(citizenCard);
                }
                export.DateSend = DateTime.Now;
                _cardExport.Update(export);
               await  _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }

        public async Task<ApiResult<string>> RemoveExport(int exportId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var export = await _cardExport
                    .Where(w => w.Id == exportId).FirstOrDefaultAsync();
                if (export == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "خروجی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                if (export.DateReceive != null)
                {
                    res.IsSuccess = false;
                    res.Messages = "این خروجی شماره گذاری شده است و امکان حذف وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }

                var exportCard = await _cardExportDetails.Include(i => i.CitizenCardInfo).Where(w => w.ExportCardId == exportId).ToListAsync();
                if(exportCard.Any())
                {
                    foreach (var card in exportCard)
                    {
                        var citizenCard = card.CitizenCardInfo;
                        citizenCard.RequestStatuse = Common.GlobalEnum.CardRequestStatusEnum.درخواست_جدید;
                        _citizensCard.Update(citizenCard);

                    } 
                    _cardExportDetails.RemoveRange(exportCard);
                }
                
               
                _cardExport.Remove(export);
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

        public async Task<ApiResult<string>> RemoveCardInExportList(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شد");
            try
            {
                var exportCard = await _cardExportDetails.Include(i => i.ExportCard).Include(i => i.CitizenCardInfo).Where(w => w.Id == id).FirstOrDefaultAsync();
                if (exportCard == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                var export = exportCard.ExportCard;
                if (export.DateSend != null)
                {
                    res.IsSuccess = false;
                    res.Messages = "این کارت برای چاپ ارسال شده است و امکان حذف وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                if (export.DateReceive != null)
                {
                    res.IsSuccess = false;
                    res.Messages = "این کارت شماره گذاری شده است و امکان حذف وجود ندارد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }


                var citizenCard = exportCard.CitizenCardInfo;
                citizenCard.RequestStatuse = Common.GlobalEnum.CardRequestStatusEnum.درخواست_جدید;
                _citizensCard.Update(citizenCard);
                _cardExportDetails.Remove(exportCard);



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
