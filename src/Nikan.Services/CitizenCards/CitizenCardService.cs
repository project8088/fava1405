using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.ViewModel.CitizenCards;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.CitizenCards
{
    public interface ICitizenCardService
    {
        Task<ApiResult<PagedCitizensCardViewModel>> AdvancedSearch(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null,
            string name = null, string nationCode = null, int[] groupIds = null, string cardNumber = null, SabtStatusEnum? sabtStatus = null,
            PersonalPictureEnum? pictureConfirmed = null, MaritalStatusEnum? mariageStatus = null, int? cardTypeId = null,
            DeliverTypeEnum? deliverType = null, CardRequestStatusEnum? requestStatuse = null,
          int? discountGroupId = null, bool? hasCard = null, bool? hasdiscount = null, int? groupId = null, bool? gender = null);
        Task<ApiResult> BackCard(BackCardDto model, int userId);
        Task<ApiResult> CardCancellation(CardCancellationDto model, int userId);
        Task<ApiResult<CardDashbordStatisticalReport>> CountForReport();
        Task<ApiResult> DeliveredCard(DeliveredCardDto model, int userId);
        Task<ApiResult<List<CitizensCardInfo>>> GetCitizenCardInfo(int citizenId);
        Task<ApiResult<CitizensCardInfo>> GetCitizenCardInfoByCardId(int id);
        Task<ApiResult> NewExportCard(NewExportCard model, int userId);
        Task<ApiResult> OrderCardByCitizen(OrerCardDto model, bool isFree = false);
        Task<ApiResult<AddressDto>> UpdateCitizenCardAddress(AddressDto model, int citizenId);
    }
    public class CitizenCardService : ICitizenCardService
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
        private readonly DbSet<CitizensCardBackCard> _backCard;
        private readonly DbSet<CardInfo_DistributeCard> _distributeCard;
        private readonly DbSet<CitizensCardCancellation> _cancellationCard;
        private readonly DbSet<CardInfo_Export> _cardExport;
        private readonly DbSet<CardInfo_Export_Citizen> _cardExportDetails;
        private readonly DbSet<Address> _address;





        public CitizenCardService(IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizen = _uow.Set<Citizen>();
            _address = _uow.Set<Address>();
            _profile = _uow.Set<CitizenProfile>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _citizensCard = _uow.Set<CitizensCard>();
            _cardInfo_PermissionsForGroups = _uow.Set<CardInfo_PermissionsForGroups>();
            _group = _uow.Set<Group>();
            _groupCitizens = _uow.Set<GroupsCitizens>();
            _cardInfo = _uow.Set<CardInfo>();
            _backCard = _uow.Set<CitizensCardBackCard>();
            _distributeCard = _uow.Set<CardInfo_DistributeCard>();
            _cardType = _uow.Set<CardType>();
            _cancellationCard = _uow.Set<CitizensCardCancellation>();
            _cardExport = _uow.Set<CardInfo_Export>();
            _cardExportDetails = _uow.Set<CardInfo_Export_Citizen>();
        }

        #endregion






 
        public async Task<ApiResult<List<CitizensCardInfo>>> GetCitizenCardInfo(int citizenId)
        {
            var res = new ApiResult<List<CitizensCardInfo>>(true, ApiResultStatusCode.Success, new List<CitizensCardInfo>());
            try
            {

                var data = await _citizensCard.Where(w => w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه && w.CitizenId == citizenId).Select(s => new CitizensCardInfo()
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
                    UserCode = s.Citizen.UserCode,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    DeliveredByOperationId = s.DeliveredByOperationId,
                    DeliveredByUserCode = s.DeliveredByOperation == null ? Guid.Empty : s.DeliveredByOperation.UserCode,
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


                }).ToListAsync();


                if (data.Any())
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].DeliverType == DeliverTypeEnum.پستی)
                        {
                            var addressId = data[i].DeliveringAddressId;
                            if (addressId != null)
                            {
                                var address = await _address.Where(w => w.Id == addressId).Select(s => new AddressInfo()
                                {
                                    CityId = s.CityId,
                                    CreationDate = s.CreationDate,
                                    FullAddress = s.FullAddress,
                                    IsDeleted = s.IsDeleted,
                                    AddressType = s.AddressType,
                                    Alley = s.Alley,
                                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                                    CitizenId = s.CitizenId,
                                    City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                                    Id = s.Id,
                                    IsVerified = s.IsVerified,
                                    LasteUpdateOnDate = s.LasteUpdateOnDate,
                                    Plaque = s.Plaque,
                                    PostalCode = s.PostalCode,
                                    Region = s.Region,
                                    Street = s.Street,
                                    Phone = s.Phone,

                                }).FirstOrDefaultAsync();
                                if (address != null)
                                {

                                    var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                                    if (!string.IsNullOrWhiteSpace(address.Street))
                                        fullAddresss += " خیابان  " + address.Street;

                                    if (!string.IsNullOrWhiteSpace(address.Alley))
                                        fullAddresss += " کوچه  " + address.Alley;

                                    if (!string.IsNullOrWhiteSpace(address.Plaque))
                                        fullAddresss += "  پلاک " + address.Plaque;


                                    if (!string.IsNullOrWhiteSpace(address.PostalCode))
                                        fullAddresss += "  کدپستی  " + address.PostalCode;

                                    data[i].DeliveringAddress = fullAddresss;
                                }
                                else
                                {

                                    data[i].DeliveringAddress = "آدرسی مشخص نشده است";
                                }

                            }
                        }
                        else
                        {
                            //تحویل در مرکز
                            data[i].DeliveringAddress = data[i].DeliveringCenter;

                        }
                    }
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

        public async Task<ApiResult<AddressDto>>  UpdateCitizenCardAddress(AddressDto model, int citizenId)
        {
            var res = new ApiResult<AddressDto>(true, ApiResultStatusCode.Success, new AddressDto(), "آدرس با موفقیت ثبت گردید");
            try
            {
                if (model.CityId == 0)
                {
                    res.IsSuccess = false;
                    res.Messages = " شناسه شهر را وارد نمایید";
                    return res;
                }

                if (model.CardId == 0  && model.CardId==null  )
                {
                    res.IsSuccess = false;
                    res.Messages = " شناسه کارت خریداری شده  را وارد نمایید";
                    return res;
                }



                var card = await _citizensCard.FirstOrDefaultAsync(w => w.Id == model.CardId);
                if(card==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    return res;

                } 
                if (card.DeliverType==DeliverTypeEnum.تحویل_در_مرکز)
                {
                    res.IsSuccess = false;
                    res.Messages = "امکان ویرایش برای کارتهایی که مرکز تحویل  مشخص شده اند وجود ندارد";
                    return res;

                }
                 
                if (card.RequestStatuse == CardRequestStatusEnum.ارسال_برای_چاپ
                    ||  card.RequestStatuse == CardRequestStatusEnum.چاپ_کارت || card.RequestStatuse == CardRequestStatusEnum.درخواست_جدید)
                {

                    var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == citizenId);
                    if (citizen == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "شهروندی یافت نشد";
                        return res;
                    }

                    //ثبت یک ادرس جدید

                    var address = new Address()
                    {
                        AddressType = model.AddressType,
                        CitizenId = citizenId,
                        CityId = model.CityId,
                        CreationDate = DateTime.Now,
                        FullAddress = model.FullAddress,
                        IsActive = true,
                        Alley = model.Alley,
                        Street = model.Street,
                        Plaque = model.Plaque,
                        IsVerified = false,
                        PostalCode = model.PostalCode,
                        Region = model.Region,
                        LasteUpdateOnDate = DateTime.Now,
                        Phone = model.Phone,
                    };
                    await _address.AddAsync(address);
                    //انتصاب ان به درخواست کارت
                    card.DeliveringAddress = address;
                    _citizensCard.Update(card);




                    await _uow.SaveChangesAsync();
                }
                else
                {

                    res.IsSuccess = false;
                    res.Messages = " امکان ویرایش آدرس تحویل کارت وجود ندارد.";
                    return res;

                }

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در اجرای عملیات رخ داده است";
            }

            return res;
        }

        public async Task<ApiResult> OrderCardByCitizen(OrerCardDto model, bool isFree = false)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {

                var card = new CitizensCard()
                {
                    AdminDescription = "خرید کارت توسط شهروند",
                    CardInfoId = model.CardInfoId,
                    CardRequestType = CardRequestTypeEnum.درخواست_جدید,
                    CitizenId = model.CitizenId,
                    DeliveringAddressId = model.DeliveringAddressId,
                    DeliverType = DeliverTypeEnum.پستی,
                    RequestByCitizenId = model.CitizenId,
                    RequestCode = Guid.NewGuid().ToString(),
                    TransactionId = model.TransactionId,
                    RequestStatuse = CardRequestStatusEnum.درخواست_اولیه,
                    RequestDate=DateTime.Now, 
                    DiscountGroupId=model.DiscountGroupId
                };
                if(isFree)
                {
                    card.RequestStatuse = CardRequestStatusEnum.درخواست_جدید;
                    card.AdminDescription = "این کارت به صورت رایگان صادر شده است"; 
                }

                //اضافه کردن شهروند به گروههای خرید کارت
                //246
                if (await _group.AnyAsync(a => a.Id == 246))
                {
                    if (await _groupCitizens.AnyAsync(a => a.Id == 246 && a.CitizenId == model.CitizenId))
                    {
                        await _groupCitizens.AddAsync(new GroupsCitizens()
                        {
                            AddByUserId = 1,
                            CitizenId = model.CitizenId,
                            GroupId = 246,
                            CreationDate = DateTime.Now,
                        });

                    }

                }


                var citizen = await _citizen.FirstOrDefaultAsync(w => w.CitizenId == model.CitizenId);
                if(citizen!=null)
                {
                    citizen.HasCard = true;
                    _citizen.Update(citizen);

                }

                var add = await _citizensCard.AddAsync(card);
               



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









        public async Task<ApiResult<PagedCitizensCardViewModel>> AdvancedSearch(
          int pageNumber, int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string name = null,
          string nationCode = null, 
          int[] groupIds = null,
          //int? exportId = null,
          string  cardNumber = null,
          SabtStatusEnum? sabtStatus = null,
          PersonalPictureEnum? pictureConfirmed = null,
          MaritalStatusEnum? mariageStatus = null,
          int? cardTypeId = null,
          DeliverTypeEnum? deliverType=null,
          CardRequestStatusEnum? requestStatuse = null ,
          int? discountGroupId = null, bool? hasCard = null, bool? hasdiscount = null,int ? groupId = null, bool? gender = null)
        {
             
            var res = new ApiResult<PagedCitizensCardViewModel>(true, ApiResultStatusCode.Success, new PagedCitizensCardViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _citizensCard.AsQueryable();

                 

                if (groupId != null)
                {
                    query = query.Where(w => w.Citizen.GroupsCitizens.Any(a => a.IsDeleted != true && a.GroupId == groupId));
                }


                if (!string.IsNullOrEmpty(nationCode))
                {
                    query = query.Where(w => EF.Functions.Like(w.Citizen.NationCode, "%" + nationCode + "%"));
                }
                 

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w => EF.Functions.Like(w.Citizen.FirstName, "%" + name + "%")
                    ||
                    EF.Functions.Like(w.Citizen.LastName, "%" + name + "%")

                    );
                }

                if (discountGroupId != null)
                {
                    query = query.Where(w => w.DiscountGroupId== discountGroupId);
                }

                if (sabtStatus!=null)
                {
                    query = query.Where(w => w.Citizen.SabtStatus == sabtStatus);
                }


                if(hasdiscount!=null)
                {
                    if(hasdiscount==true)
                        query = query.Where(w => w.DiscountGroupId != null);
                    else
                        query = query.Where(w => w.DiscountGroupId == null);

                }
                 

                if (mariageStatus != null)
                {
                    query = query.Where(w => w.Citizen.MariageStatus == mariageStatus);
                }

                if (pictureConfirmed != null)
                {
                    query = query.Where(w => w.Citizen.PersonalPicture_Confirmed == pictureConfirmed);
                }

                if (hasCard != null)
                {
                    query = query.Where(w =>w.RequestStatuse!=CardRequestStatusEnum.درخواست_اولیه &&  w.Citizen.HasCard == hasCard.Value);
                }


                if (gender != null)
                {
                    query = query.Where(w => w.Citizen.Gender == gender);
                }

                if (FromDate != null)
                {

                    var date = FromDate.Value.ToShortDateString() + " 00:00:00";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.RequestDate >= datetime);

                }
                if (ToDate != null)
                {
                    var date = ToDate.Value.ToShortDateString() + " 23:59:59";
                    var datetime = DateTime.Parse(date);
                    query = query.Where(w => w.RequestDate <= datetime);

                }
















                if (cardTypeId != null)
                {
                    query = query.Where(w => w.CardInfo.CardTypeId== cardTypeId);
                }

                if (deliverType != null)
                {
                    query = query.Where(w => w.DeliverType== deliverType);
                }

                if (requestStatuse != null)
                {
                    query = query.Where(w => w.RequestStatuse == requestStatuse);
                }

                 



                if (!string.IsNullOrEmpty(cardNumber))
                {
                    query = query.Where(w => w.CardNumber == cardNumber);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new CitizensCardInfo()
                {
                    DeliveringCenterId=s.DeliveringCenterId,
                    DeliveringCenter=s.DeliveringCenter.Name,
                    DeliveringAddressId=s.DeliveringAddressId,
                    Id=s.Id,
                    AdminDescription=s.AdminDescription,
                    BarCode=s.BarCode,
                    CardActivationDate=s.CardActivationDate,
                    CardExpirationDate=s.CardExpirationDate,
                    CardInfoId=s.CardInfoId,
                    CardNumber=s.CardNumber,
                    DeliveredOnDate=s.DeliveredOnDate,
                    CardRequestType=s.CardRequestType,
                    CitizenId=s.CitizenId,
                    UserCode=s.Citizen.UserCode,
                    SabtStatus=s.Citizen.SabtStatus,
                    PersonalPicture_Confirmed=s.Citizen.PersonalPicture_Confirmed,
                    CardSerial =s.CardSerial,
                    DeliveredByOperationId=s.DeliveredByOperationId,
                    DeliveredByUserCode=s.DeliveredByOperation==null ? Guid.Empty :s.DeliveredByOperation.UserCode,
                    DeliveredByOperation=s.DeliveredByOperation.DisplayName,
                    DeliveredDescription=s.DeliveredDescription,
                    NationCode=s.Citizen.NationCode,
                    Citizen=s.Citizen.FirstName+" "+s.Citizen.LastName,
                    DeliverType=s.DeliverType,
                    RequestCode=s.RequestCode,
                    DeliveringAddress=s.DeliveringAddress.FullAddress,
                    RequestByCitizen=s.RequestByCitizen.FirstName+ " "+s.RequestByCitizen.LastName,
                    RequestStatuse=s.RequestStatuse,
                    RequestDate=s.RequestDate,
                    PreCardNumber=s.PreCardNumber,
                    RequestByCitizenId=s.RequestByCitizenId,
                    IsSetBarCode=s.IsSetBarCode,
                    DiscountGroupId=s.DiscountGroupId ,
                    CardTitle=s.CardInfo.CardType.Title,
                    Gender = s.Citizen.Gender==true ? "آقا":"خانم",
                    FirstName = s.Citizen.FirstName,
                    LastName = s.Citizen.LastName,
                    Mobile = s.Citizen.Mobile, 
                    HasDiscount=s.DiscountGroupId!=null,
                    DiscountTitle = s.DiscountGroup.Discount.DiscountTitle, 

                    Region = s.DeliveringAddress == null ? 0 : s.DeliveringAddress.Region,
                    FullAddress = s.DeliveringAddress == null ? "" : s.DeliveringAddress.FullAddress,
                    Street = s.DeliveringAddress == null ? "" : s.DeliveringAddress.Street,
                    Alley = s.DeliveringAddress == null ? "" : s.DeliveringAddress.Alley,
                    PostalCode = s.DeliveringAddress == null ? "" : s.DeliveringAddress.PostalCode,
                    Plaque = s.DeliveringAddress == null ? "" : s.DeliveringAddress.Plaque,
                    Phone = s.DeliveringAddress == null ? "" : s.DeliveringAddress.Phone,
                    


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


        public async Task<ApiResult<CitizensCardInfo>> GetCitizenCardInfoByCardId(int id)
        {

            var res = new ApiResult<CitizensCardInfo>(true, ApiResultStatusCode.Success, new CitizensCardInfo());
            try
            {

                var query = _citizensCard.Where(w => w.Id == id);


                var data = await query.Select(s => new CitizensCardInfo()
                {
                    DeliveringCenterId = s.DeliveringCenterId,
                    DeliveringCenter = s.DeliveringCenter.Name,
                    DeliveringAddressId = s.DeliveringAddressId,
                    Id = s.Id,
                    AdminDescription = s.AdminDescription,
                    BarCode = s.BarCode,
                    CardActivationDate = s.CardActivationDate,
                    CardExpirationDate = s.CardExpirationDate,
                    CardInfoId = s.CardInfoId,
                    CardNumber = s.CardNumber,
                    DeliveredOnDate = s.DeliveredOnDate,
                    CardRequestType = s.CardRequestType,
                    CitizenId = s.CitizenId,
                    UserCode = s.Citizen.UserCode,
                    CardSerial = s.CardSerial,
                    DeliveredByOperationId = s.DeliveredByOperationId,
                    DeliveredByOperation = s.DeliveredByOperation.DisplayName,
                    DeliveredByUserCode = s.DeliveredByOperation == null ? Guid.Empty : s.DeliveredByOperation.UserCode,
                    DeliveredDescription = s.DeliveredDescription,
                    NationCode = s.Citizen.NationCode,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    DeliverType = s.DeliverType,
                    RequestCode = s.RequestCode,
                    DeliveringAddress = s.DeliveringAddress.FullAddress,
                    RequestByCitizen = s.RequestByCitizen.FirstName + " " + s.RequestByCitizen.LastName,
                    RequestStatuse = s.RequestStatuse,
                    RequestDate = s.RequestDate,
                    PreCardNumber = s.PreCardNumber,
                    RequestByCitizenId = s.RequestByCitizenId,
                    IsSetBarCode = s.IsSetBarCode,
                    DiscountGroupId = s.DiscountGroupId,
                    CardTitle = s.CardInfo.CardType.Title,
                    CardCost = s.CardInfo.CardCost,
                    DoubleCardCost = s.CardInfo.DoubleCardCost,
                    PostalCostInCity = s.CardInfo.PostalCostInCity,
                    PostalCostOutCity = s.CardInfo.PostalCostInCity,
                    VATForCardCost = s.CardInfo.VATForCardCost,
                    VATForPost = s.CardInfo.VATForCardCost,
                    AmountTransaction = s.Transaction == null ? 0 : s.Transaction.AmountTransaction,
                    TransactionBankReferenceId = s.Transaction == null ? "" : s.Transaction.TransactionBankReferenceId,
                    TransactionOnDate = s.Transaction == null ? DateTime.Now : s.Transaction.TransactionOnDate,
                    TransactionId = s.TransactionId




                }).FirstOrDefaultAsync();

                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }


                if (data.DeliverType == DeliverTypeEnum.پستی)
                {
                    var addressId = data.DeliveringAddressId;
                    if (addressId != null)
                    {
                        var address = await _address.Where(w => w.Id == addressId).Select(s => new AddressInfo()
                        {
                            CityId = s.CityId,
                            CreationDate = s.CreationDate,
                            FullAddress = s.FullAddress,
                            IsDeleted = s.IsDeleted,
                            AddressType = s.AddressType,
                            Alley = s.Alley,
                            Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                            CitizenId = s.CitizenId,
                            City = new ViewModel.BaseDataModel { Key = s.CityId.ToString(), ParentText = s.City.Parent.Title, Text = s.City.Title, ParentValue = s.City.ParentId.ToString() },
                            Id = s.Id,
                            IsVerified = s.IsVerified,
                            LasteUpdateOnDate = s.LasteUpdateOnDate,
                            Plaque = s.Plaque,
                            PostalCode = s.PostalCode,
                            Region = s.Region,
                            Street = s.Street,
                            Phone = s.Phone,

                        }).FirstOrDefaultAsync();
                        if (address != null)
                        {

                            var fullAddresss = address.City == null ? "" : address.City.ParentText + " - " + address.City.Text;

                            if (!string.IsNullOrWhiteSpace(address.Street))
                                fullAddresss += " خیابان  " + address.Street;

                            if (!string.IsNullOrWhiteSpace(address.Alley))
                                fullAddresss += " کوچه  " + address.Alley;

                            if (!string.IsNullOrWhiteSpace(address.Plaque))
                                fullAddresss += "  پلاک " + address.Plaque;


                            if (!string.IsNullOrWhiteSpace(address.PostalCode))
                                fullAddresss += "  کدپستی  " + address.PostalCode;

                            data.DeliveringAddress = fullAddresss;
                        }
                        else
                        {

                            data.DeliveringAddress = "آدرسی مشخص نشده است";
                        }

                    }
                }
                else
                {
                    //تحویل در مرکز
                    data.DeliveringAddress =data.DeliveringCenter;

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

        public async Task<ApiResult> NewExportCard(NewExportCard model, int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "صدور خروجی کارت با موفقیت انجام شد");


            try
            {
                 

                //درخواست هایی که پرداخت آن انجام شده است =1 
                //شهروند تایید هویت شده باشد=2
                //عکس آن تایید شده باشد=3
                //در خروجی های قبلی نباشند=4
                var listCard = _citizensCard.Where(w =>
                w.RequestStatuse == CardRequestStatusEnum.درخواست_جدید
                &&
                w.Citizen.SabtStatus==SabtStatusEnum.تایید
                &&
                w.Citizen.PersonalPicture_Confirmed==PersonalPictureEnum.تایید_شده
                && !w.CardInfo_Export_Citizen.Any(cx => cx.IsDeleted != true)//4
                );

                //نوع تحویل کارت
                if (model.DeliverType != null)
                {
                    listCard = listCard.Where(w => w.DeliverType == model.DeliverType);

                }

                if (model.GroupIds != null  && model.GroupIds.Any())
                {
                    //درخواست کنندگان در این گروهها باید عضو باشند 
                    listCard = listCard.Where(w => w.Citizen.
                    GroupsCitizens.Any(g => model.GroupIds.Contains(g.GroupId)));
                }

                //اتباع 
                if (model.Nationality != null)
                {
                    if(model.Nationality==0)
                    {
                        listCard = listCard.Where(w => w.Citizen.NationId == 0);
                    }
                    else
                    {
                        listCard = listCard.Where(w => w.Citizen.NationId  > 0);
                    } 
                }

                if (model.InTheCity != null)
                {

                    if (model.InTheCity == true)//درخواست های داخل شهر
                        listCard = listCard.Where(w => w.DeliveringAddress.CityId == 7);//داخل شهر اصفهان
                    else
                        listCard = listCard.Where(w => w.DeliveringAddress.CityId != 7);//خارج از شهر اصفهان

                }

                if (model.StartDate != null && model.EndDate != null)
                    listCard = listCard.Where(w => w.RequestDate >= model.StartDate.Value && w.RequestDate <= model.EndDate.Value);

                else if (model.StartDate != null && model.EndDate == null)
                    listCard = listCard.Where(w => w.RequestDate >= model.StartDate.Value);

                else if (model.StartDate == null && model.EndDate != null)
                    listCard = listCard.Where(w => w.RequestDate <= model.EndDate.Value);

                if (model.CardTypeId != null)
                {
                    listCard = listCard.Where(w => w.CardInfo.CardTypeId == model.CardTypeId);

                }

                var listcards =await listCard.ToListAsync();
                if(listcards.Any())
                {
                    var exportNumber = 1000;
                    if (await _cardExport.AnyAsync())
                    {
                        var maxexportNumber = await _cardExport.MaxAsync(m => m.ExportNumber);
                        if(maxexportNumber!=null)
                        {
                            exportNumber = maxexportNumber.Value;
                            exportNumber++;
                        }
                       
                    }

                    var exportcard = new CardInfo_Export()
                    {
                        CreationDate = DateTime.Now,
                        ExporterByUserId = userId,
                        ExportNumber = exportNumber, 
                    };
                    await  _cardExport.AddAsync(exportcard);
                    var listexport = new List<CardInfo_Export_Citizen>();
                    foreach (var card in listcards)
                    {
                        listexport.Add(new CardInfo_Export_Citizen()
                        {
                            CitizenCardInfoId= card.Id,
                            CreationDate=DateTime.Now,
                            ExportCard= exportcard, 
                        }); 
                    }
                      _cardExportDetails.AddRange(listexport);

                    await _uow.SaveChangesAsync();

                }
                else
                {

                    res.IsSuccess = false;
                    res.Messages = "درخواست کارت جدیدی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
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

    

        public async Task<ApiResult> BackCard(BackCardDto model,int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");


            try
            {

                var item =await _citizensCard.FirstOrDefaultAsync(w=>w.Id== model.Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                    // 
                    //برگشت کارت
                    var backcard = new CitizensCardBackCard()
                    {
                        BackCardByOperationId = userId,
                        BackCardOnDate = model.BackCardOnDate,
                        CitizenCardInfoId = item.Id,
                        PreRequestStatuse = item.RequestStatuse,
                        DeliveringCenterId = model.CenterId,
                        ReasonBackDescription = model.Reason
                    };
                    await _backCard.AddAsync(backcard);
                    //تغییر وضعیت کارت
                    item.RequestStatuse = CardRequestStatusEnum.برگشت_داده_شده;
                    _citizensCard.Update(item);
                    //تو صف توزیع کارت اگر وجود داشت پاک کن
                    var distributeCard =await _distributeCard.FirstOrDefaultAsync(w => w.CitizenCardInfoId == model.Id);
                    if(distributeCard!=null)
                    {
                        //add eevent
                        _distributeCard.Remove(distributeCard);
                    }



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

        public async Task<ApiResult> CardCancellation(CardCancellationDto model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");


            try
            {
                var item = await _citizensCard.FirstOrDefaultAsync(w => w.Id == model.Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                 
                 item.RequestStatuse =CardRequestStatusEnum.باطل_شده;
                _citizensCard.Update(item); 

                var cancelcardinfo = new CitizensCardCancellation()
                {
                    CardCancellationByOperationId = userId,
                    CardCancellationOnDate =model.CardCancellationOnDate,
                    CardCancellationType = (CardCancellationTypeEnum)model.CardCancellationType,
                    CitizenCardInfoId = model.Id,
                    Description = model.Description,
                    ReasonCardCancellation = model.CardCancellationItemId,

                };
                await _cancellationCard.AddAsync(cancelcardinfo);
                if (cancelcardinfo.CardCancellationType == CardCancellationTypeEnum.با_درخواست_مجدد)
                {
                    //اضافه کردن کارت جدید
                    var addcard = new CitizensCard
                    {
                        CitizenId = item.CitizenId,
                        CardInfoId = item.CardInfoId,
                        DeliveringAddressId = item.DeliveringAddressId,
                        DeliveringCenterId = item.DeliveringCenterId,
                        DeliverType = item.DeliverType,
                        DiscountGroupId = item.DiscountGroupId,
                        TransactionId = item.TransactionId,
                        AdminDescription = model.Description, 
                        RequestCode = Guid.NewGuid().ToString(),
                        CardRequestType =  CardRequestTypeEnum.درخواست_مجدد
                    };
                    addcard.RequestCode = Guid.NewGuid().ToString();
                    addcard.RequestByCitizenId = userId;
                    addcard.RequestDate = DateTime.Now;
                    addcard.RequestStatuse =  CardRequestStatusEnum.درخواست_جدید;
                    await _citizensCard.AddAsync(addcard);
                    //اضافه کردن شهروند به گروههای خرید کارت
                    //246
                    if (await _group.AnyAsync(a => a.Id == 246))
                    {
                        if (await _groupCitizens.AnyAsync(a => a.Id == 246 && a.CitizenId == item.CitizenId))
                        {
                            await _groupCitizens.AddAsync(new GroupsCitizens()
                            {
                                AddByUserId = 1,
                                CitizenId = item.CitizenId,
                                GroupId = 246,
                                CreationDate = DateTime.Now,
                            });

                        }

                    }

                }


                //تو صف توزیع کارت اگر وجود داشت پاک کن
                var distributeCard = await _distributeCard.FirstOrDefaultAsync(w => w.CitizenCardInfoId == model.Id);
                if (distributeCard != null)
                {
                    //add eevent
                    _distributeCard.Remove(distributeCard);
                }


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


        public async Task<ApiResult> DeliveredCard(DeliveredCardDto model, int userId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");

            try
            {

                var item = await _citizensCard.FirstOrDefaultAsync(w => w.Id == model.Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کارتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }



                item.RequestStatuse = CardRequestStatusEnum.تحویل_داده_شد;
                item.DeliveredOnDate = model.DeliveredOnDate;
                item.DeliveredDescription = model.DeliveredDescription;
                item.DeliveredByOperationId = userId;
                _citizensCard.Update(item);
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


        #region گزارشات

        public async Task<ApiResult<CardDashbordStatisticalReport>> CountForReport()
        {
            var res = new ApiResult<CardDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new CardDashbordStatisticalReport());
            try
            {
                var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

                DateTime date1 = new DateTime(DateTime.Now.Ticks);




                res.Data.AllCardCount = await _citizensCard.CountAsync(w => w.RequestStatuse !=CardRequestStatusEnum.درخواست_اولیه);
                 res.Data.CardNewRequest = await _citizensCard.CountAsync(w => w.RequestStatuse != CardRequestStatusEnum.درخواست_جدید);
                 res.Data.PersonelPictureCount = await _citizensCard.CountAsync(w =>w.Citizen.PersonalPicture_Confirmed==PersonalPictureEnum.درحال_بررسی &&  w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه);
                res.Data.DeliveredCards = await _citizensCard.CountAsync(w => w.RequestStatuse == CardRequestStatusEnum.تحویل_داده_شد);
                res.Data.CardNewRequestTodayCount = await _citizensCard.CountAsync(w => w.RequestStatuse == CardRequestStatusEnum.درخواست_جدید && w.RequestDate > todaysDate);
                res.Data.ManzalatCardCount = await _citizensCard.CountAsync(w => w.CardInfo.CardTypeId == 9 && w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه);
                res.Data.ShahrvaniCardCount = await _citizensCard.CountAsync(w => w.CardInfo.CardTypeId == 8 && w.RequestStatuse != CardRequestStatusEnum.درخواست_اولیه);
                DateTime date2 = new DateTime(DateTime.Now.Ticks);





                TimeSpan ts = date2 - date1;

                res.Data.TotalSeconds = ts.TotalSeconds.ToString();



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


    }

}
