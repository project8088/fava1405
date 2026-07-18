export class BaseUrl {
  public baseUrl = '/';
  public mainApi = '/api/';

  constructor() {
    let host = document.location.host;
    if (host.includes('localhost')) {
      this.baseUrl = 'https://localhost:44381';
      this.mainApi = 'https://localhost:44381/api/';
      // this.baseUrl = 'https://profile.isfahan.ir/';
      // this.mainApi = 'https://profile.isfahan.ir/api/';
    }
  }
}

const base = new BaseUrl();

export const ServerApis = {
  getCurrentDateTime: 'https://worldtimeapi.org/api/timezone/Asia/Tehran',

  baseUrl: base.baseUrl,
  captcha: base.mainApi + 'account/captcha',
  registercaptcha: base.mainApi + 'citzens/registercaptcha',
  //base data
  getProvinces: base.mainApi + 'BaseData/getProvinces',
  getCitesByParent: base.mainApi + 'BaseData/GetCitesByParent',
  getAllCites: base.mainApi + 'BaseData/GetAllCites',
  getBaseListJobOffice: base.mainApi + 'BaseData/GetBaseListJobOffice',
  GetJobOfficeAndCheck: base.mainApi + 'BaseData/GetJobOfficeAndCheck',
  getlistBase: base.mainApi + 'BaseData/GetlistBase',
  getJobWorkTime: base.mainApi + 'BaseData/GetJobWorkTime',
  getMajors: base.mainApi + 'BaseData/GetMajors',
  getlistOfMajors: base.mainApi + 'BaseData/GetlistOfMajors',
  getNationalities: base.mainApi + 'Basedata/GetNationality',
  getIsFahanCites: base.mainApi + 'Basedata/GetIsFahanCites',
  getIsfahanInfo: base.mainApi + 'Basedata/GetIsfahanInfo',
  getlistOfComputerSkills: base.mainApi + 'BaseData/GetlistOfComputerSkills',
  getlistOfSkills: base.mainApi + 'BaseData/GetlistOfSkills',
  getSkills: base.mainApi + 'BaseData/GetSkills',

  getAllServices: base.mainApi + 'content/GetAllServices',
  getTopCompanies: base.mainApi + 'companies/GetTopCompanies',
  configPortal: base.mainApi + 'BaseData/ConfigPortal',
  //citizen

  checkRegisterCitizenByNtionCode: base.mainApi + 'Citzens/CheckRegisterCitizenByNtionCode',
  checkValidMobileNumberForCitzenRegister:
    base.mainApi + 'Citzens/CheckValidMobileNumberForCitzenRegister',
  updteCitizenMobileNumber: base.mainApi + 'Citzens/UpdateCitizenMobileNumber',
  updteCitizenMobileNumberByAdmin: base.mainApi + 'Citzens/UpdateCitizenMobileNumberByAdmin',
  updateCitizenMobileNumberByCard: base.mainApi + 'Citzens/UpdateCitizenMobileNumberByCard',
  updateSabtStatus: base.mainApi + 'Citzens/UpdateSabtStatus',
  updateSabtStatusByCard: base.mainApi + 'Citzens/UpdateSabtStatusByCard',

  getCitizenMobileNumber: base.mainApi + 'Citzens/GetCitizenMobileNumber',
  checkValidMobileNumberAndGetVerfiyCodeForChangeMobileNumber:
    base.mainApi + 'Citzens/CheckValidMobileNumberAndGetVerfiyCodeForChangeMobileNumber',
  getVerfiCodeByCitizen: base.mainApi + 'Citzens/GetVerfiCodeByCitizen',
  getCitizenEmail: base.mainApi + 'Citzens/GetCitizenEmail',
  updteCitizenEmailAddress: base.mainApi + 'Citzens/UpdateCitizenEmailAddress',
  getEducationGroups: base.mainApi + 'BaseData/GetEducationGroups',
  getCitizenBankCardNumber: base.mainApi + 'Citzens/GetCitizenBankCardNumber',
  updteCitizenBankCardNumber: base.mainApi + 'Citzens/UpdateCitizenBankCardNumber',
  uploadPersonalPicture: base.mainApi + 'Citzens/UploadPersonalPicture',
  addOrUpdateCitizenProfile: base.mainApi + 'Citzens/AddOrUpdateCitizenProfile',
  updteCitizenByAdmin: base.mainApi + 'Citzens/UpdateCitizenByAdmin',
  addOrUpdateCitizenProfileByAdmin: base.mainApi + 'Citzens/AddOrUpdateCitizenProfileByAdmin',
  geCitizenProfile: base.mainApi + 'Citzens/GeCitizenProfile',

  geCitizenProfileByCitizen: base.mainApi + 'Citzens/GetCitizenProfileByCitizen',
  geCitizenProfileByAdmin: base.mainApi + 'Citzens/GetCitizenProfileByAdmin',

  getCitizenFullInfo: base.mainApi + 'Citzens/GetCitizenFullInfo',
  getMyFullInfo: base.mainApi + 'Citzens/GetMyFullInfo',
  getAppRegisterList: base.mainApi + 'Citzens/GetAppRegisterList',
  getAppRegisterListForMainPage: base.mainApi + 'Citzens/GetAppRegisterListForMainPage',
  checkCitzenRegister: base.mainApi + 'Account/CheckCitizenRegister',
  citizenRegister: base.mainApi + 'Citzens/CitizenRegister',
  validatePhoneNumber: base.mainApi + 'Citzens/ValidatePhoneNumber',
  reSendVerfiyCode: base.mainApi + 'Citzens/ReSendVerfiyCode',

  getCitizenBaseInfoByAdmin: base.mainApi + 'Citzens/GetCitizenBaseInfoByAdmin',
  getCitizenBaseInfoByCitizen: base.mainApi + 'Citzens/GetCitizenBaseInfoByCitizen',
  getIdentityInformationByCitizen: base.mainApi + 'Citzens/GetIdentityInformationByCitizen',
  getIdentityInformationByAdmin: base.mainApi + 'Citzens/GetIdentityInformationByAdmin',

  updteOtherBaseInfoByCitizen: base.mainApi + 'Citzens/UpdateOtherBaseInfoByCitizen',
  updteIdentityInformationByCitizen: base.mainApi + 'Citzens/UpdateIdentityInformationByCitizen',
  updteIdentityInformationByAdmin: base.mainApi + 'Citzens/UpdateIdentityInformationByAdmin',

  searchCitizens: base.mainApi + 'Citzens/SearchCitizens',
  searchCitizenByCardUser: base.mainApi + 'Citzens/SearchCitizenByCardUser',
  citizenAdvancedSearch: base.mainApi + 'Citzens/CitizenAdvancedSearch',
  citizenAdvancedSearch_Export: base.mainApi + 'Citzens/CitizenAdvancedSearch_Export',

  checkIsDead: base.mainApi + 'Citzens/CheckIsDead',
  searchCitizensAuthentication: base.mainApi + 'Citzens/SearchCitizensAuthentication',

  getCitizenHomeAddress: base.mainApi + 'Citzens/GetCitizenHomeAddress',
  getCitizenHomeAddressByAdmin: base.mainApi + 'Citzens/GetCitizenHomeAddressByAdmin',
  getCitizenOfficeAddressByAdmin: base.mainApi + 'Citzens/GetCitizenOfficeAddressByAdmin',
  getCitizenAddress: base.mainApi + 'Citzens/GetCitizenAddress',

  getCitizenOfficeAddress: base.mainApi + 'Citzens/GetCitizenOfficeAddress',
  updteCitizenAddressByCitizen: base.mainApi + 'Citzens/UpdateCitizenAddressByCitizen',
  addOrUpdteCitizenAddressByCitizen: base.mainApi + 'Citzens/AddOrUpdateCitizenAddressByCitizen',
  addOrUpdteCitizenAddressByCitizenForCardAddress:
    base.mainApi + 'Citzens/AddOrUpdateCitizenAddressByCitizenForCardAddress',
  getAllCitizenFamilyByFamily: base.mainApi + 'Citzens/GetAllCitizenFamilyByFamily',
  addOrUpdteCitizenAddress: base.mainApi + 'Citzens/AddOrUpdateCitizenAddress',
  //citizen-Family
  addFamilyMemberIfAny: base.mainApi + 'Citzens/AddFamilyMemberIfAny',
  addFamilyMemberIfNotAny: base.mainApi + 'Citzens/AddFamilyMemberIfNotAny',
  getMyFamilyBaseInfo: base.mainApi + 'Citzens/GetMyFamilyBaseInfo',
  updateFamilyMemberByCitizen: base.mainApi + 'Citzens/UpdateFamilyMemberByCitizen',
  getAllCitizenFamily: base.mainApi + 'Citzens/GetAllCitizenFamily',

  searchFamilyCitizens: base.mainApi + 'Citzens/SearchFamilyCitizens',

  getAllCitizenFamilyByAdmin: base.mainApi + 'Citzens/GetAllCitizenFamilyByAdmin',
  confirmFamilyByAdmin: base.mainApi + 'Citzens/ConfirmFamilyByAdmin',
  removeFamily: base.mainApi + 'Citzens/RemoveFamily',
  removeFamilyByCitizen: base.mainApi + 'Citzens/RemoveFamilyByCitizen',
  copyPicture: base.mainApi + 'Citzens/CopyPicture',

  //admin citizen manzelat
  searchManzaltCitizens: base.mainApi + 'Manzalat/SearchManzaltCitizens',

  searchManzaltCitizens_Export: base.mainApi + 'Manzalat/SearchManzaltCitizens_Export',

  getCitizenInfoAndManzaltForm: base.mainApi + 'Manzalat/GetCitizenInfoAndManzaltForm',
  confirmManzaltByAdmin: base.mainApi + 'Manzalat/ConfirmManzaltByAdmin',
  removeManzalatForm: base.mainApi + 'Manzalat/Remove',

  // citizens pictures
  searchImageCardCitizens: base.mainApi + 'Citzens/SearchImageCardCitizens',
  searchImageCitizens: base.mainApi + 'Citzens/SearchImageCitizens',
  acceptCitizenPicture: base.mainApi + 'Citzens/AcceptCitizenPicture',
  rejectCitizenPicture: base.mainApi + 'Citzens/RejectCitizenPicture',
  getCitizenRejectImageSmsList: base.mainApi + 'Citzens/GetCitizenRejectImageSmsList',
  uploadCitizenPicture: base.mainApi + 'Citzens/UploadCitizenPicture',
  uploadPersonalPictureByAdmin: base.mainApi + 'Citzens/UploadPersonalPictureByAdmin',

  //citizen Feedback
  addFeedbacke: base.mainApi + 'Citzens/AddFeedbacke',
  getAllCitizenFeedbacks: base.mainApi + 'Citzens/GetAllCitizenFeedbacks',
  getBaseListFeedbacke: base.mainApi + 'Citzens/GetBaseListFeedbacke',
  getFeedback: base.mainApi + 'Citzens/GetFeedback',
  searchfeedbacks: base.mainApi + 'Citzens/Searchfeedbacks',
  removefeedback: base.mainApi + 'Citzens/Removefeedback',
  //Groups Citizens
  getGroupsCitizensInfo: base.mainApi + 'Citzens/GetGroupsCitizensInfo',
  getPagedGroupsCitizensInfo: base.mainApi + 'Citzens/GetPagedGroupsCitizensInfo',
  addCitizenToGroup: base.mainApi + 'Citzens/AddCitizenToGroup',
  removeCitizenFromGroup: base.mainApi + 'Citzens/RemoveCitizenFromGroup',
  //citizen Import From File
  citizenImportFileList: base.mainApi + 'Citzens/CitizenImportFileList',
  importCitizenListFromExcel: base.mainApi + 'Citzens/ImportCitizenListFromExcel',
  citizenImportFileDetails: base.mainApi + 'Citzens/CitizenImportFileDetails',
  removeCitizenImportFile: base.mainApi + 'Citzens/RemoveCitizenImportFile',
  confirmCitizenfileexcel: base.mainApi + 'Citzens/ConfirmCitizenFile',

  //import file
  groupImportFileDetails: base.mainApi + 'GroupsCitizens/GroupImportFileDetails',
  getAllGroupImportList: base.mainApi + 'GroupsCitizens/GetAllGroupImportList',
  importGroupNationCodeFromExcel: base.mainApi + 'GroupsCitizens/ImportGroupNationCodeFromExcel',
  confirmNationCodeImportListToGroup:
    base.mainApi + 'GroupsCitizens/ConfirmNationCodeImportListToGroup',
  groupTransfer: base.mainApi + 'GroupsCitizens/GroupTransfer',

  searchCitizensQueue: base.mainApi + 'GroupsCitizens/SearchCitizensQueue',
  removeQueue: base.mainApi + 'GroupsCitizens/RemoveQueue',

  //event
  getTopEvent: base.mainApi + 'Portal/GetTopEvent',
  getEvent: base.mainApi + 'Portal/GetEvent',
  getCitizenTopEvent: base.mainApi + 'Portal/GetCitizenTopEvent',

  //Groups
  addOrUpdateGroup: base.mainApi + 'GroupsCitizens/AddOrUpdateGroup',
  removeGroup: base.mainApi + 'GroupsCitizens/RemoveGroup',
  reviewGroups: base.mainApi + 'GroupsCitizens/ReviewGroups',
  groupInfo: base.mainApi + 'GroupsCitizens/GroupInfo',
  getAllGroups: base.mainApi + 'GroupsCitizens/GetAllGroups',
  searchGroups: base.mainApi + 'GroupsCitizens/SearchGroups',
  //refund

  refundImportFileList: base.mainApi + 'Refund/RefundImportFileList',
  importRefundListFromExcel: base.mainApi + 'Refund/ImportRefundListFromExcel',
  refundImportFileDetails: base.mainApi + 'Refund/RefundImportFileDetails',
  removeRefundImportFile: base.mainApi + 'Refund/RemoveRefundImportFile',
  removeRefundAccess: base.mainApi + 'Refund/RemoveRefundAccess',
  addRefundAccess: base.mainApi + 'Refund/AddRefundAccess',
  updateRefundAccess: base.mainApi + 'Refund/UpdateRefundAccess',
  updateRefund: base.mainApi + 'Refund/UpdateRefund',
  addRefund: base.mainApi + 'Refund/AddRefund',
  updateRefundSaveCardNmber: base.mainApi + 'Refund/UpdateRefundSaveCardNmber',
  getAllRefunAccessUsers: base.mainApi + 'Refund/GetAllRefunAccessUsers',

  getCardsNumber: base.mainApi + 'Refund/GetCardsNumber',
  getCardNumber: base.mainApi + 'Refund/GetCardNumber',

  getAllRefundAccessPageList: base.mainApi + 'Refund/GetAllRefundAccessPageList',
  refundAccessDetailsList: base.mainApi + 'Refund/RefundAccessDetailsList',
  allRefundAccessPagesList: base.mainApi + 'Refund/AllRefundAccessPagesList',

  getRefundCitizenAccessPageList: base.mainApi + 'Refund/GetRefundCitizenAccessPageList',
  getReportRefund: base.mainApi + 'Refund/GetReportRefund',
  getRefundInfoDetailsByAdmin: base.mainApi + 'Refund/GetRefundInfoDetailsByAdmin',
  commitRefundByAdmin: base.mainApi + 'Refund/CommitRefundByAdmin',

  searchRefundUser: base.mainApi + 'Refund/SearchRefundUser',
  deleteRefundUser: base.mainApi + 'Refund/DeleteRefundUser',
  addRefundUser: base.mainApi + 'Refund/AddRefundUser',

  //citizen app service
  addOrUpdateAppService: base.mainApi + 'AppServiceCitizen/AddOrUpdate',
  removeAppService: base.mainApi + 'AppServiceCitizen/Remove',
  getAppInfo: base.mainApi + 'AppServiceCitizen/GetAppInfo',
  getAllAppService: base.mainApi + 'AppServiceCitizen/GetAllApp',
  getAppDashbordList: base.mainApi + 'AppServiceCitizen/GetAppDashbordList',
  getBaseListAppService: base.mainApi + 'AppServiceCitizen/GetBaseListAppService',
  // Documents
  getDocGroupsBaseList: base.mainApi + 'Users/GetDocGroupsBaseList',
  getAllDocGrpupsAndUserDocuments: base.mainApi + 'Users/GetAllDocGrpupsAndUserDocuments',
  uploadDocGroupAttachment: base.mainApi + 'Attachment/uploadDocGroupAttachment',
  uploadManzalatAttachment: base.mainApi + 'Attachment/uploadManzalatAttachment',

  getUserDocuments: base.mainApi + 'Users/GetUserDocuments',
  removeUserDocument: base.mainApi + 'Users/RemoveUserDocument',
  searchUsers: base.mainApi + 'Users/SearchUsers',
  searchCardUser: base.mainApi + 'Users/SearchCardUser',
  searchAminUser: base.mainApi + 'Users/SearchAminUser',

  deleteCardUser: base.mainApi + 'Users/DeleteCardUser',

  //card info

  updateCitizenCardAddressByCitizen: base.mainApi + 'CardInfo/UpdateCitizenCardAddressByCitizen',
  updateCitizenCardAddress: base.mainApi + 'CardInfo/UpdateCitizenCardAddress',

  getCardInfo: base.mainApi + 'CardInfo/GetCardInfo',
  getPagedCardInfo: base.mainApi + 'CardInfo/GetPagedCardInfo',
  getCitizenCardInfo: base.mainApi + 'CardInfo/GetCitizenCardInfo',
  changeLockDistributionQueue: base.mainApi + 'CardInfo/ChangeLockDistributionQueue',
  addCardDistributionQueue: base.mainApi + 'CardInfo/AddCardDistributionQueue',
  updateCardDistributionQueue: base.mainApi + 'CardInfo/UpdateCardDistributionQueue',
  getDistributionQueueInfo: base.mainApi + 'CardInfo/GetDistributionQueueInfo',
  getPagedeDistributionQueue: base.mainApi + 'CardInfo/GetPagedeDistributionQueue',
  removeCardQueue: base.mainApi + 'CardInfo/RemoveCardQueue',
  searchCardInQueue: base.mainApi + 'CardInfo/SearchCardInQueue',
  searchCardInQueue_Export: base.mainApi + 'CardInfo/SearchCardInQueue_Export',
  deliveryQueueToOperator: base.mainApi + 'CardInfo/DeliveryQueueToOperator',
  searchCardForQueue: base.mainApi + 'CardInfo/SearchCardForQueue',
  getQueueListInCourse: base.mainApi + 'CardInfo/GetQueueListInCourse',
  removeCardFromQueue: base.mainApi + 'CardInfo/RemoveCardFromQueue',
  removeCardFromQueueByCourseId: base.mainApi + 'CardInfo/RemoveCardFromQueueByCourseId',
  sendCardToQueue: base.mainApi + 'CardInfo/SendCardToQueue',

  getCardTypeBaseList: base.mainApi + 'CardInfo/GetCardTypeBaseList',
  getActiveCardTypeBaseList: base.mainApi + 'CardInfo/GetActiveCardTypeBaseList',

  addOrUpdateCard: base.mainApi + 'CardInfo/AddOrUpdateCard',
  citizencardAdvSearch: base.mainApi + 'CardInfo/CitizencardAdvSearch',
  citizencardAdvSearch_Export: base.mainApi + 'CardInfo/CitizencardAdvSearch_Export',

  getPagedCardInfoExport: base.mainApi + 'CardInfo/GetPagedCardInfoExport',
  newExportCard: base.mainApi + 'CardInfo/NewExportCard',
  getPagedCardInfoExportDetails: base.mainApi + 'CardInfo/GetPagedCardInfoExportDetails',
  getPagedCardInfoExportDetails_Export:
    base.mainApi + 'CardInfo/GetPagedCardInfoExportDetails_Export',

  getPagedShahrvandiCardInfoExportDetailsForSend:
    base.mainApi + 'CardInfo/GetPagedShahrvandiCardInfoExportDetailsForSend',
  getPagedManzalatCardInfoExportDetailsForSend:
    base.mainApi + 'CardInfo/GetPagedManzalatCardInfoExportDetailsForSend',
  getExportCardPicture: base.mainApi + 'CardInfo/GetExportCardPicture',
  sendToPrint: base.mainApi + 'CardInfo/SendToPrint',
  removeExportCard: base.mainApi + 'CardInfo/RemoveExportCard',
  removeCardInExportList: base.mainApi + 'CardInfo/RemoveCardInExportList',
  getCitizenCardInfoByCardId: base.mainApi + 'CardInfo/GetCitizenCardInfoByCardId',
  backCard: base.mainApi + 'CardInfo/BackCard',
  deliveredCard: base.mainApi + 'CardInfo/DeliveredCard',
  cardCancellation: base.mainApi + 'CardInfo/CardCancellation',
  importExcelFileCardNumber: base.mainApi + 'CardInfo/ImportExcelFileCardNumber',

  //توزیع کارت
  getPagedeCardCourses: base.mainApi + 'CardInfo/GetPagedeCardCourses',
  addCardCourses: base.mainApi + 'CardInfo/AddCardCourses',
  closeCardCourses: base.mainApi + 'CardInfo/CloseCardCourses',
  removeCardCourses: base.mainApi + 'CardInfo/RemoveCardCourses',

  //تخفیف کارت

  addCardDiscount: base.mainApi + 'CardInfo/AddCardDiscount',
  updateCardDiscount: base.mainApi + 'CardInfo/UpdateCardDiscount',
  changeDiscountCenterState: base.mainApi + 'CardInfo/ChangeDiscountCenterState',
  changeDiscountGroupState: base.mainApi + 'CardInfo/ChangeDiscountGroupState',
  changeDiscountState: base.mainApi + 'CardInfo/ChangeDiscountState',
  addDiscountCenter: base.mainApi + 'CardInfo/AddDiscountCenter',
  removeDiscount: base.mainApi + 'CardInfo/RemoveDiscount',
  removeDisCountItem: base.mainApi + 'CardInfo/RemoveDisCountItem',
  pagedDiscountCardList: base.mainApi + 'CardInfo/PagedDiscountCardList',
  getCardDiscountInfo: base.mainApi + 'CardInfo/GetCardDiscountInfo',

  getDisCountGroupBaseList: base.mainApi + 'CardInfo/GetDisCountGroupBaseList',

  pagedRequestFreeCardLsit: base.mainApi + 'CardInfo/PagedRequestFreeCardLsit',
  addRequestFreeCard: base.mainApi + 'CardInfo/AddRequestFreeCard',
  updateRequestFreeCard: base.mainApi + 'CardInfo/UpdateRequestFreeCard',
  getRequestFreeCardCitizens: base.mainApi + 'CardInfo/GetRequestFreeCardCitizens',
  getRequestFreeCard: base.mainApi + 'CardInfo/GetRequestFreeCard',
  removeRequestFreeCard: base.mainApi + 'CardInfo/RemoveRequestFreeCard',
  acceptedRequestFreeCard: base.mainApi + 'CardInfo/AcceptedRequestFreeCard',

  //Order
  listAvailableCards: base.mainApi + 'Order/ListAvailableCards',
  checkCanOrderCard: base.mainApi + 'Order/CheckCanOrderCard',

  cardPriceInfo: base.mainApi + 'Order/CardPriceInfo',

  // AppServiceCitizen
  callCitizenService: base.mainApi + 'AppServiceCitizen/CallCitizenService',

  //slide show

  addOrUpdateSlideShow: base.mainApi + 'content/AddOrUpdateSlideShow',
  removeSlideShow: base.mainApi + 'content/RemoveSlideShow',
  getSlideShow: base.mainApi + 'content/GetSlideShow',
  getAllSlideShow: base.mainApi + 'content/GetAllSlideShow',
  getAllMainPageSlideShow: base.mainApi + 'content/GetAllMainPageSlideShow',

  //Admin Users
  addOrUpdateUserGroups: base.mainApi + 'users/AddOrUpdateGroups',
  removeUserGroups: base.mainApi + 'users/RemoveGroups',
  getUserGroups: base.mainApi + 'users/GetGroup',
  getAllUserGroups: base.mainApi + 'users/GetAllGroups',
  getPermissionList: base.mainApi + 'users/GetPermissionList',
  addPermissions: base.mainApi + 'users/AddPermissions',
  addWebApiUserPermissions: base.mainApi + 'users/AddWebApiUserPermissions',
  getWebApiPermissionList: base.mainApi + 'users/GetWebApiPermissionList',
  getCardPermissionList: base.mainApi + 'users/GetCardPermissionList',
  addCardUserPermissions: base.mainApi + 'users/AddCardUserPermissions',

  getAllRols: base.mainApi + 'users/GetAllRols',
  getAllUserRoles: base.mainApi + 'users/GetAllUserRoles',
  deleteUserRole: base.mainApi + 'users/DeleteUserRole',
  addUserRole: base.mainApi + 'users/AddUserRole',
  getAccessGroups: base.mainApi + 'users/GetAccessGroups',
  getAllUserPermissionGroup: base.mainApi + 'users/GetAllUserPermissionGroup',

  //manzelat
  getAllAvailableManzaltForm: base.mainApi + 'Manzalat/GetAllAvailableManzaltForm',

  updateManzalatBaseForm: base.mainApi + 'Manzalat/UpdateManzalatBaseForm',
  getCitizenManzalat: base.mainApi + 'Manzalat/GetCitizenManzalat',
  addOrUpdateManzalat: base.mainApi + 'Manzalat/AddOrUpdateManzalat',

  getManzalatBaseForm: base.mainApi + 'Manzalat/GetManzalatBaseForm',
  getManzalatBaseForms: base.mainApi + 'Manzalat/GetManzalatBaseForms',

  getCitizenRegisterManzalatForm: base.mainApi + 'Manzalat/GetCitizenRegisterManzalatForm',

  getJobTitles: base.mainApi + 'BaseData/GetJobTitles',
  getUserInfo: base.mainApi + 'BaseData/GetUserInfo',
  citySearch: base.mainApi + 'BaseData/CitySearch',
  getAllOrganCites: base.mainApi + 'BaseData/GetAllOrganCites',
  getCurrentShorDate: base.mainApi + 'BaseData/GetCurrentShorDate',

  getExitDoorsAsync: base.mainApi + 'BaseData/GetExitDoorsAsync',
  getlistOfCarType: base.mainApi + 'BaseData/GetlistOfCarType',
  getAllOrganizational: base.mainApi + 'BaseData/GetAllOrganizational',
  getAllOrganizationalUnitByOrganId: base.mainApi + 'BaseData/GetAllOrganizationalUnitByOrganId',
  getAllCardDeliveryCenters: base.mainApi + 'BaseData/GetAllCardDeliveryCenters',
  getAllSupportCenter: base.mainApi + 'BaseData/GetAllSupportCenter',

  getActivitiyList: base.mainApi + 'BaseData/GetActivitiyList',
  getPositionList: base.mainApi + 'BaseData/GetPositionList',
  getProductUnitList: base.mainApi + 'BaseData/GetProductUnitList',

  getProductParentGroups: base.mainApi + 'BaseData/GetProductParentGroups',
  getGroups: base.mainApi + 'BaseData/GetGroups',
  getFreeCardGroups: base.mainApi + 'BaseData/GetFreeCardGroups',
  searchBaseDataGroups: base.mainApi + 'BaseData/searchBaseDataGroups',

  login: base.mainApi + 'Account/Login',
  refreshToken: base.mainApi + 'Account/RefreshToken',
  logout: base.mainApi + 'Account/Logout',

  karjoRegister: base.mainApi + 'Account/KarjoRegister',

  companyRegister: base.mainApi + 'Account/CompanyRegister',
  getAdminUser: base.mainApi + 'Users/GetAdminUser',
  getCardUser: base.mainApi + 'Users/GetCardUser',
  getUserAccountInfo: base.mainApi + 'Users/GetUserAccountInfo',
  updateAccount: base.mainApi + 'Users/UpdateAccount',
  changeUserPassword: base.mainApi + 'Users/ChangeUserPassword',
  adminRegisterUser: base.mainApi + 'Users/AdminRegister',
  addCardUser: base.mainApi + 'Users/AddCardUser',
  webApiUserRegister: base.mainApi + 'Users/WebApiUserRegister',
  searchWebApiUsers: base.mainApi + 'Users/SearchWebApiUsers',
  getWebApiUsers: base.mainApi + 'Users/GetWebApiUsers',
  addUserAccessService: base.mainApi + 'Users/AddUserAccessService',
  deleteUserAccessService: base.mainApi + 'Users/DeleteUserAccessService',
  getAllUserAppService: base.mainApi + 'Users/GetAllUserAppService',
  addUserToPermissionGroup: base.mainApi + 'Users/AddUserToPermissionGroup',
  removeUserPermissionGroup: base.mainApi + 'Users/RemoveUserPermissionGroup',

  getWebUserAccessRangeIp: base.mainApi + 'Users/GetWebUserAccessRangeIp',
  deleteUserAccessRangeIp: base.mainApi + 'Users/DeleteUserAccessRangeIp',
  addWebUserAccessRangeIp: base.mainApi + 'Users/AddWebUserAccessRangeIp',

  changeCurrentUserPassword: base.mainApi + 'Account/ChangeCurrentUserPassword',
  sendSmsForgotPassword: base.mainApi + 'Account/SendSmsForgotPassword',
  checkForgotVerifyCode: base.mainApi + 'Account/CheckForgotVerifyCode',
  setNewPassword: base.mainApi + 'Account/SetNewPassword',

  addCompanyUser: base.mainApi + 'Account/AddCompanyUser',
  updateCompanyUserAccount: base.mainApi + 'Account/UpdateCompanyUserAccount',
  getCompanyUsers: base.mainApi + 'Account/GetCompanyUser',

  getCitizenAllEducation: base.mainApi + 'Citzens/GetAllEducation',

  getAllEducationByCitizen: base.mainApi + 'Citzens/GetAllEducationByCitizen',

  saveCitizenEducation: base.mainApi + 'Citzens/SaveEducation',
  deleteCitizenEducation: base.mainApi + 'Citzens/DeleteEducation',

  getCitizenPagedSmsList: base.mainApi + 'Citzens/GetCitizenPagedSmsList',

  //SabtAhval
  getAllExportSabtAhval: base.mainApi + 'ExportCitizen/GetAllExportSabtAhval',
  getAllCitizenExported: base.mainApi + 'ExportCitizen/GetAllCitizenExported',
  exportCitizenForSabtAhval: base.mainApi + 'ExportCitizen/ExportCitizenForSabtAhval',
  removeExport: base.mainApi + 'ExportCitizen/RemoveExport',
  sendSabtAhvalCitizensSms: base.mainApi + 'ExportCitizen/SendSabtAhvalCitizensSms',
  sendOnlineAuthentication: base.mainApi + 'ExportCitizen/SendOnlineAuthentication',
  sendOnlineAuthenticationByBagRezvanService:
    base.mainApi + 'ExportCitizen/SendOnlineAuthenticationByBagRezvanService',

  citizenForAuthenticationByAdmin: base.mainApi + 'ExportCitizen/CitizenForAuthenticationByAdmin',
  citizenForAuthenticationByCitizenId:
    base.mainApi + 'ExportCitizen/CitizenForAuthenticationByCitizenId',

  //Karjo
  getJobseekerProfile: base.mainApi + 'Karjo/GetProfile',
  updateJobseekerWorkStatus: base.mainApi + 'Karjo/UpdateWorkStatus',
  updateGlobalInformation: base.mainApi + 'Karjo/UpdateGlobalInformation',
  addOrUpdateJobseekerAmozeshs: base.mainApi + 'Karjo/AddOrUpdateAmozeshs',
  saveJobseekerEducation: base.mainApi + 'Karjo/SaveEducation',

  getShortCitizenInfoByCitizen: base.mainApi + 'Citzens/GetShortCitizenInfoByCitizen',
  getShortCitizenInfoByAdmin: base.mainApi + 'Citzens/GetShortCitizenInfoByAdmin',
  getShortCitizenInfoByCard: base.mainApi + 'Citzens/GetShortCitizenInfoByCard',

  getjobSeekerPhoneNumbers: base.mainApi + 'Karjo/GetPhoneNumbers',

  uploadJobseekerImage: base.mainApi + 'karjo/UploadImage',
  getKarjoImage: base.mainApi + 'karjo/GetKarjoImage',

  searchKarjo: base.mainApi + 'karjo/SearchKarjo',

  getGlobalInformation: base.mainApi + 'Karjo/GetGlobalInformation',

  getJobseekerAllEducation: base.mainApi + 'Karjo/GetAllEducation',
  deleteJobseekerEducation: base.mainApi + 'Karjo/DeleteEducation',

  getJobseekerListSkills: base.mainApi + 'Karjo/GetListSkills',
  addJobseekerSkills: base.mainApi + 'Karjo/AddSkills',
  deleteJobseekerSkills: base.mainApi + 'Karjo/DeleteSkills',

  getJobseekerListLanguage: base.mainApi + 'Karjo/GetListLanguage',
  addJobseekerLanguage: base.mainApi + 'Karjo/AddLanguage',
  deleteJobseekerLanguage: base.mainApi + 'Karjo/DeleteLanguage',

  getJobseekerListWork: base.mainApi + 'Karjo/GetListWork',
  addOrUpdateJobseekerWorks: base.mainApi + 'Karjo/AddOrUpdateWorks',
  deleteJobseekerWrok: base.mainApi + 'Karjo/DeleteWrok',

  getJobseekerListAmozesh: base.mainApi + 'Karjo/GetListAmozesh',
  deleteJobseekerAmozesh: base.mainApi + 'Karjo/DeleteAmozesh',

  getJobseekerWorkStatus: base.mainApi + 'Karjo/GetWorkStatus',

  //companies
  getListCompany: base.mainApi + 'companies/GetListCompany',
  searchCompanies: base.mainApi + 'companies/SearchCompanies',
  getCompanyAddressInfo: base.mainApi + 'companies/GetAddressInfo',
  updateCompanyAddressInfo: base.mainApi + 'companies/UpdateAddressInfo',
  getCompanyMainInfo: base.mainApi + 'companies/GetMainInfo',
  updateCompanyMainInfo: base.mainApi + 'companies/UpdateMainInfo',
  getCompanyBaseInfo: base.mainApi + 'companies/GetBaseInfo',
  updateCompnayBaseInfo: base.mainApi + 'companies/UpdateBaseInfo',
  getFullCompanyInfo: base.mainApi + 'companies/GetFullCompanyInfo',
  fullCompanyInfo: base.mainApi + 'companies/FullCompanyInfo',
  addExitPermit: base.mainApi + 'companies/AddExitPermit',
  getAllExitPermit: base.mainApi + 'companies/GetAllExitPermit',
  getExitPermitForCompany: base.mainApi + 'companies/GetExitPermit',
  companyUpdateExitPermit: base.mainApi + 'companies/UpdateExitPermit',
  companyDeleteExitPermit: base.mainApi + 'companies/DeleteExitPermit',
  addCompanyActivity: base.mainApi + 'companies/AddCompanyActivity',
  removeActivitiy: base.mainApi + 'companies/RemoveActivitiy',
  getCompanyActivity: base.mainApi + 'companies/GetCompanyActivity',
  getManagersList: base.mainApi + 'companies/GetManagersList',
  getPersonelInfo: base.mainApi + 'companies/GetPersonelInfo',
  searchCompanyPersonel: base.mainApi + 'companies/SearchCompanyPersonel',
  searchAdminPersonel: base.mainApi + 'companies/SearchAdminPersonel',
  allCompanyPersonel: base.mainApi + 'companies/AllCompanyPersonel',
  removePersonel: base.mainApi + 'companies/RemovePersonel',
  addOrUpdatePersonelByAdmin: base.mainApi + 'companies/AddOrUpdatePersonelByAdmin',
  addOrUpdatePersonel: base.mainApi + 'companies/AddOrUpdatePersonel',

  getPersonelInfoForView: base.mainApi + 'companies/GetPersonelInfoForView',

  changeCompanyAccount: base.mainApi + 'companies/ChangeCompanyAccount',

  uploadCompanyImage: base.mainApi + 'companies/UploadImage',
  getCompanyLogo: base.mainApi + 'companies/GetCompanyLogo',
  companyUploadSignature: base.mainApi + 'companies/UploadSignature',
  getCompanySignature: base.mainApi + 'companies/GetCompanySignature',
  getCompanyContract: base.mainApi + 'companies/GetCompanyContract',
  companyUploadContract: base.mainApi + 'companies/UploadContract',

  companyRegisterAsync: base.mainApi + 'companies/CompanyRegister',
  getAdditionalInfo: base.mainApi + 'companies/GetAdditionalInfo',
  updateAdditionalInfo: base.mainApi + 'companies/UpdateAdditionalInfo',
  removeCompany: base.mainApi + 'companies/RemoveCompany',

  personnelImportFileList: base.mainApi + 'companies/PersonnelImportFileList',
  importPersonnelFromExcel: base.mainApi + 'companies/ImportPersonnelFromExcel',
  removeImportFile: base.mainApi + 'companies/removeImportFile',
  personnelImportFileDetails: base.mainApi + 'companies/PersonnelImportFileDetails',
  confirmCitizenFile: base.mainApi + 'companies/PersonnelImportFileDetails',

  addOrUpdateJob: base.mainApi + 'jobs/AddOrUpdate',
  getJobDataForEdit: base.mainApi + 'jobs/GetForEdit',
  jobsList: base.mainApi + 'jobs/SearchJobs',
  companyJobsList: base.mainApi + 'jobs/CompanySearchJobs',
  getJobForView: base.mainApi + 'jobs/GetForView',
  sendResumeForJob: base.mainApi + 'jobs/SendResume',
  searchActiveJobs: base.mainApi + 'jobs/SearchActiveJobs',
  searchSendResume: base.mainApi + 'jobs/SearchSendResume',

  //Guard
  guardGetAllExitPermit: base.mainApi + 'GuardPanel/GetAllExitPermit',
  guardChangeStatusExitPermit: base.mainApi + 'GuardPanel/ChangeStatus',
  guardGetExitPermit: base.mainApi + 'GuardPanel/GetExitPermit',

  //content

  getPagedNewsItems: base.mainApi + 'content/GetPagedNewsItems',
  addOrUpdateNews: base.mainApi + 'content/AddOrUpdateNews',
  publishComment: base.mainApi + 'content/PublishComment',

  getListNewsGroups: base.mainApi + 'content/GetListNewsGroups',
  getAllNewGroups: base.mainApi + 'content/GetAllNewGroups',
  addOrUpdateNewsGroup: base.mainApi + 'content/AddOrUpdateNewsGroup',
  removeNewsGroups: base.mainApi + 'content/RemoveNewsGroups',
  getLastNews: base.mainApi + 'content/GetLastNews',

  getLastApiHelp: base.mainApi + 'content/GetLastApiHelp',

  getMostVisitedNews: base.mainApi + 'content/GetMostVisitedNews',
  getNews: base.mainApi + 'content/GetNews',
  addNewsComments: base.mainApi + 'content/AddComments',
  getNewsComments: base.mainApi + 'content/GetComments',
  getNewsPublishComments: base.mainApi + 'content/GetPublishComments',

  addOrUpdateMenuItem: base.mainApi + 'content/AddOrUpdateMenuItem',
  updateSortMenuItems: base.mainApi + 'content/UpdateSort',
  removeMenu: base.mainApi + 'content/RemoveMenu',
  getMainMenuItems: base.mainApi + 'content/GetMainMenuItems',
  getAllMenuItems: base.mainApi + 'content/getAllMenuItems',
  getMenu: base.mainApi + 'content/getMenu',
  getAllPagePath: base.mainApi + 'content/GetAllPagePath',

  removeFaqGroupType: base.mainApi + 'content/RemoveFaqGroupType',
  addOrUpdateFaqGroup: base.mainApi + 'content/AddOrUpdateFaqGroup',
  getAllFaqGroups: base.mainApi + 'content/GetAllFaqGroups',
  getFaqList: base.mainApi + 'content/GetFaqList',
  getFaq: base.mainApi + 'content/GetFaq',
  getFaqGroups: base.mainApi + 'content/GetFaqGroups',
  removeFaq: base.mainApi + 'content/RemoveFaq',
  addOrUpdateFaq: base.mainApi + 'content/AddOrUpdateFaq',

  getPagedWebPageItems: base.mainApi + 'content/GetPagedWebPageItems',
  removeWebPage: base.mainApi + 'content/RemoveWebPage',
  addOrUpdateWebPage: base.mainApi + 'content/AddOrUpdateWebPage',
  getWebPage: base.mainApi + 'content/GetWebPage',
  getPageWithSlug: base.mainApi + 'content/page/',

  getPagedNotificationsItems: base.mainApi + 'content/GetPagedNotificationsItems',
  getLastNotifications: base.mainApi + 'content/GetLastNotifications',
  getNotification: base.mainApi + 'content/GetNotification',
  addOrUpdateNotifications: base.mainApi + 'content/AddOrUpdateNotifications',
  getCitizenNotifications: base.mainApi + 'content/GetCitizenNotifications',
  addCitizensNotifactions: base.mainApi + 'content/AddCitizensNotifactions',
  removeCitizensNotifactions: base.mainApi + 'content/RemoveCitizensNotifactions',

  uploaderUrl: base.mainApi + 'Attachment/uploadFile',
  uploadAttachment: base.mainApi + 'Attachment/uploadAttachment',
  getAttachmentsForUser: base.mainApi + 'Attachment/GetAttachments',
  getAttachmentsForAdmin: base.mainApi + 'Attachment/GetAll',
  removeAttachment: base.mainApi + 'Attachment/RemoveAttachment',
  removeManzalatAttachment: base.mainApi + 'Attachment/RemoveManzalatAttachment',

  //Tickets
  getAllTicketSubject: base.mainApi + 'Tickets/GetAllTicketSubject',
  getListTicketSubject: base.mainApi + 'Tickets/GetListTicketSubject',
  getTicketSubject: base.mainApi + 'Tickets/GetTicketSubject',
  removeTicketSubject: base.mainApi + 'Tickets/RemoveTicketSubject',
  addOrUpdateTicketSubject: base.mainApi + 'Tickets/AddOrUpdateTicketSubject',
  getListTicketSubjectByUnitId: base.mainApi + 'Tickets/GetListTicketSubjectByUnitId',

  sendUserTicket: base.mainApi + 'Tickets/SendUserTicket',
  getAdminTicketsList: base.mainApi + 'Tickets/GetAdminTicketsList',
  getCardTicketsList: base.mainApi + 'Tickets/GetCardTicketsList',
  getCompanyTicketsList: base.mainApi + 'Tickets/GetCompanyTicketsList',
  getUserTicketsList: base.mainApi + 'Tickets/GetUserTicketsList',
  getTicketById: base.mainApi + 'Tickets/GetById',
  updateTicketsStatues: base.mainApi + 'Tickets/UpdateTicketsStatues',
  sendAnswerTicket: base.mainApi + 'Tickets/SendAnswerTicket',
  addTicketComments: base.mainApi + 'Tickets/AddComments',
  getTicketComments: base.mainApi + 'Tickets/GetTicketComments',
  removeTicketComments: base.mainApi + 'Tickets/RemoveComments',
  addTicketActivity: base.mainApi + 'Tickets/AddActivity',
  getTicketActivity: base.mainApi + 'Tickets/GetTicketActivity',
  removeTicketActivity: base.mainApi + 'Tickets/RemoveActivity',
  getAnswerTicket: base.mainApi + 'Tickets/GetAnswerTicket',
  addContact: base.mainApi + 'Tickets/AddContact',
  getContactList: base.mainApi + 'Tickets/GetContactList',
  getComapnyContactList: base.mainApi + 'Tickets/GetComapnyContactList',

  //organizations
  removeOrganization: base.mainApi + 'Organ/RemoveOrganization',
  addOrUpdateOrganization: base.mainApi + 'Organ/AddOrUpdateOrganization',
  getAllOrganization: base.mainApi + 'Organ/GetAllOrganization',
  getOrganizationForEdit: base.mainApi + 'Organ/GetOrganizationForEdit',
  removeUnit: base.mainApi + 'Organ/RemoveUnit',
  addOrUpdateUnit: base.mainApi + 'Organ/AddOrUpdateUnit',
  getUnitForEdit: base.mainApi + 'Organ/GetUnitForEdit',
  getAllUnit: base.mainApi + 'Organ/GetAllUnit',

  addGroupToUnitGroup: base.mainApi + 'Organ/AddGroupToUnitGroup',
  removeUnitGroup: base.mainApi + 'Organ/RemoveUnitGroup',
  getAllUnitGroups: base.mainApi + 'Organ/GetAllUnitGroups',

  // / Portal
  getExitDoors: base.mainApi + 'Portal/GetExitDoors',
  updateDoors: base.mainApi + 'Portal/UpdateDoors',
  getSettings: base.mainApi + 'Portal/GetSettings',
  getSiteInfo: base.mainApi + 'Portal/GetSiteInfo',
  updateSettings: base.mainApi + 'Portal/UpdateSettings',
  uploadSiteLogo: base.mainApi + 'Portal/UploadImage',

  getPagedSmsList: base.mainApi + 'Portal/GetPagedSmsList',

  getFinancialSettings: base.mainApi + 'Portal/GetFinancialSettings',
  updateFinancialSettings: base.mainApi + 'Portal/UpdateFinancialSettings',

  updateSmsSettings: base.mainApi + 'Portal/UpdateSmsSettings',
  geSmsSettings: base.mainApi + 'Portal/GeSmsSettings',

  getManzalatSettings: base.mainApi + 'Manzalat/GetManzalatSettings',
  updateManzalatSettings: base.mainApi + 'Manzalat/UpdateManzalatSettings',

  //order
  getAllStoreList: base.mainApi + 'Order/GetAll',
  getStoreSaleItems: base.mainApi + 'Order/GetSaleItems',
  removeStoreItem: base.mainApi + 'Order/Remove',
  addOrUpdateStore: base.mainApi + 'Order/AddOrUpdate',
  getAllStoreItemForComany: base.mainApi + 'Order/GetAllForComany',
  testPay: base.mainApi + 'Order/TestPay',
  showPayResult: base.mainApi + 'Order/ShowPayResult',
  getAllTransactions: base.mainApi + 'Order/GetAllTransactions',
  getAllTransactionsForCitizens: base.mainApi + 'Order/GetAllTransactionsForCitizens',
  getAllCitizenTransactions: base.mainApi + 'Order/GetAllCitizenTransactions',
  getAllTransactionsForCompany: base.mainApi + 'Order/GetAllTransactionsForCompany',
  getTransaction: base.mainApi + 'Order/GetTransaction',
  buyCardByCitizens: base.mainApi + 'Order/BuyCardByCitizens',
  checkTransaction: base.mainApi + 'Order/CheckTransaction',
  createCardForCitizen: base.mainApi + 'Order/CreateCardForCitizen',
  getCitizenCardPriceInfo: base.mainApi + 'Order/GetCitizenCardPriceInfo',
  //report
  getStatisticalReport: base.mainApi + 'report/GetStatisticalReport',
  getStatisticalCardReport: base.mainApi + 'report/GetStatisticalCardReport',
  getCitizenRegisterChartReport: base.mainApi + 'report/GetCitizenRegisterChartReport',
  //Products
  getAllCompanyProductGroups: base.mainApi + 'Products/GetAllCompanyGroups',
  getProductGroupsByParentId: base.mainApi + 'Products/GetProductGroupsByParentId',
  addOrUpdateProductGroup: base.mainApi + 'Products/AddOrUpdateGroup',
  removeProductGroup: base.mainApi + 'Products/RemoveGroup',

  addOrUpdateCompnayProduct: base.mainApi + 'Products/AddOrUpdate',
  removeCompanyProduct: base.mainApi + 'Products/Remove',
  getAllCompanyProducts: base.mainApi + 'Products/GetAllCompanyProducts',
  getCompanyProduct: base.mainApi + 'Products/GetProduct',

  getAllProductForView: base.mainApi + 'Products/GetAllForView',
  buyProduct: base.mainApi + 'Store/Buy',

  prinQueueForPost: base.baseUrl + '/print/QueueForPost',
};
