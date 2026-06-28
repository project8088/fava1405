using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.Refund;
using Nikan.DomainClasses.UserCompanes;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.ImportExcelFile;
using Nikan.ViewModel.Refund;
using Nikan.ViewModel.UserCompanes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.ImportFile
{
    public interface IImportExcelFileService
    {
        Task<ApiResult<UploadFileResult>> AddCompanyPersonnelImportFile(List<AddCompanyPersonnelInfo> list, string fileName, int companyId, int userId);
         Task<ApiResult<UploadFileResult>> AddRefundImportFile(List<RefundExcelFileColumns> list, string fileName,   int userId);
        Task<ApiResult> ConfirmNationCodeImportListToGroup(int importId, int userId);
        Task<ApiResult<List<ImportExcelFileInfo>>> GetAll();
        Task<ApiResult<List<ImportExcelFileInfo>>> GetAllCompanyFile(int companyId);
        Task<ApiResult<List<ImportFileInfo>>> GetAllGroupImportList();
        Task<ApiResult<ImportFileInfo>> GroupImportFileDetails(int importId);
        Task<ApiResult<ImportFileInfo>> PersonnelImportFileDetails(int importId);
        Task<ApiResult<RefundImportFileInfo>> RefundImportFileDetails(int importId);
        Task<ApiResult<List<RefundImportFileInfo>>> RefundImportFileList();
        Task<ApiResult<string>> Remove(int Id);
    }

    public class ImportExcelFileService : IImportExcelFileService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ImportExcelFile> _fileExcel;
        private readonly DbSet<ImportExcelFileDetails> _fileDetails;

        private readonly DbSet<CompanyPersonnel> _companyPersonnel;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<GroupsCitizens> _citizenGroup;
        private readonly DbSet<CitizensQueue> _citizenQueue;
        private readonly DbSet<RefundImportFileDetails> _refundImportFileDetails;
        private readonly DbSet<AppServices> _app;
        private readonly DbSet<Group> _groups;

        public ImportExcelFileService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _fileExcel = _uow.Set<ImportExcelFile>();
            _fileDetails = _uow.Set<ImportExcelFileDetails>();
            _citizen = _uow.Set<Citizen>();
            _app = _uow.Set<AppServices>();
            _citizenGroup = _uow.Set<GroupsCitizens>();
            _citizenQueue = _uow.Set<CitizensQueue>();
            _companyPersonnel = _uow.Set<CompanyPersonnel>();
            _refundImportFileDetails = _uow.Set<RefundImportFileDetails>();
        }


     public async Task<ApiResult<List<ImportExcelFileInfo>>> GetAll()
        {
            var res = new ApiResult<List<ImportExcelFileInfo>>(true, ApiResultStatusCode.Success, new List<ImportExcelFileInfo>());
            try
            {

                res.Data = await _fileExcel.Where(w => w.IsDeleted != true).Select(s => new ImportExcelFileInfo()
                {
                    CountRow = s.CountRow,
                    CreationDate = s.CreationDate,
                    ExportFileName = s.ExportFileName,
                    ExportFilePath = s.ExportFilePath,
                    FileAccept = s.IsConfirmed,
                    Id = s.Id,
                    ImportByUser = s.ImportByUser.Username,
                    ImportByUserId = s.ImportByUserId,
                    ImportExcelFileType = s.ImportExcelFileType,
                    ReviewByUser = s.ReviewByUser.Username,
                    ReviewByUserId = s.ReviewByUserId,
                    ReviewOnData = s.ReviewOnData,
                    UserCompanyId = s.UserCompanyId,
                    UserCompany = s.UserCompany.CompanyName,

                }).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }

    public async Task<ApiResult<List<ImportFileInfo>>> GetAllGroupImportList()
        {
            var res = new ApiResult<List<ImportFileInfo>>(true, ApiResultStatusCode.Success, new List<ImportFileInfo>());
            try
            {

                res.Data = await _fileExcel.Where(w =>w.ImportExcelFileType==Common.GlobalEnum.ImportExcelFileTypeEnum.گروه_شهروندی && w.IsDeleted != true).Select(s => new ImportFileInfo()
                {
                     FileName=s.ExportFileName,
                     GroupName=s.Group.GroupName,
                     ImportId=s.Id,
                     IsConfirm=s.IsConfirmed,
                     OnDate=s.CreationDate,
                     ImportBy=s.ImportByUser.DisplayName, 
                }).ToListAsync(); 

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
    public async Task<ApiResult<List<ImportExcelFileInfo>>> GetAllCompanyFile(int companyId)
        {
            var res = new ApiResult<List<ImportExcelFileInfo>>(true, ApiResultStatusCode.Success, new List<ImportExcelFileInfo>());
            try
            {

                res.Data = await _fileExcel.Where(w =>w.UserCompanyId==companyId &&   w.IsDeleted != true).Select(s => new ImportExcelFileInfo()
                {
                    CountRow = s.CountRow,
                    CreationDate = s.CreationDate,
                    ExportFileName = s.ExportFileName,
                    ExportFilePath = s.ExportFilePath,
                    FileAccept = s.IsConfirmed,
                    Id = s.Id,
                    ImportByUser = s.ImportByUser.Username,
                    ImportByUserId = s.ImportByUserId,
                    ImportExcelFileType = s.ImportExcelFileType,
                    ReviewByUser = s.ReviewByUser.Username,
                    ReviewByUserId = s.ReviewByUserId,
                    ReviewOnData = s.ReviewOnData,
                    UserCompanyId = s.UserCompanyId,
                    UserCompany = s.UserCompany.CompanyName,

                }).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }
    public async Task<ApiResult<string>> Remove(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _fileExcel.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                item.IsDeleted = true;
                _fileExcel.Update(item);
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

    public async Task<ApiResult<UploadFileResult>> AddCompanyPersonnelImportFile(List<AddCompanyPersonnelInfo> list, string fileName,int companyId, int userId)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            var listAdd = new List<CompanyPersonnel>();
            try
            {
                var file = new ImportExcelFile()
                {
                    ExportFileName = fileName,
                    ImportByUserId = userId,
                    ImportExcelFileType = Common.GlobalEnum.ImportExcelFileTypeEnum.لیست_شهروندان_اشخاص_حقوقی,
                    CreationDate = DateTime.Now,
                    UserCompanyId= companyId,
                    CountRow=list.Count

                };

                foreach (var item in list)
                {

                    if(string.IsNullOrWhiteSpace(item.NationCode)
                        ||
                        string.IsNullOrWhiteSpace(item.FirstName)
                         ||
                        string.IsNullOrWhiteSpace(item.LastName)
                         ||
                        string.IsNullOrWhiteSpace(item.FatherName)
                         ||
                        string.IsNullOrWhiteSpace(item.Gender)
                         ||
                        string.IsNullOrWhiteSpace(item.Mobile)
                            
                        )
                    {
                        continue;
                    }
                    //چک کن ببین کد ملی اوکی هست
                    //چک کن ببین شماره موبایل اوکی هست
                    var birthDate = DateTime.Now;
                    if(! DateTime.TryParse(item.BirthDate,out birthDate)) continue; 
                    var nationCode = item.NationCode.Fa2En();
                    var mobileNumber = item.Mobile.Fa2En();
                    var check = nationCode.IsValidNationalCode();
                    if(check!="") continue;
                    listAdd.Add(new CompanyPersonnel()
                    {
                        FatherName=item.FatherName,
                        Mobile=item.Mobile,
                        Gender=item.Gender=="1"  ? true :false,
                        LastName=item.LastName,
                        FirstName=item.FirstName,
                        NationCode=item.NationCode,
                        BirthDate= birthDate,
                        ImportExcelFile= file,
                        JobTitle=item.JobTitle,
                        Address=item.Address
                    }); 
                }

                if (listAdd.Any())
                {
                    file.CountRow = listAdd.Count;
                    await _fileExcel.AddAsync(file);
                    _companyPersonnel.AddRange(listAdd);
                    await _uow.SaveChangesAsync();
                    res.Data.ImportId = file.Id;
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

   

    public async Task<ApiResult<ImportFileInfo>> PersonnelImportFileDetails(int importId)
        {
            var res = new ApiResult<ImportFileInfo>(true, ApiResultStatusCode.Success, new ImportFileInfo(), "");
           
            try
            {
                var file = await _fileExcel.Include(w => w.ImportByUser).Where(w => w.Id == importId).FirstOrDefaultAsync();
                if (file != null)
                {
                    res.Data.ImportId = file.Id;
                    res.Data.IsConfirm = file.IsConfirmed;
                    res.Data.ImportBy = file.ImportByUser.DisplayName;
                    res.Data.OnDate = file.CreationDate;
                    res.Data.PersonnelInfo = await _companyPersonnel.Where(w => w.ImportExcelFileId == importId)
                        .Select(s => new CompanyPersonnelInfo()
                    {
                            FatherName=s.FatherName,
                            FirstName=s.FirstName,
                            Gender=s.Gender,
                            Address=s.Address,
                            BirthDate=s.BirthDate,
                            ImportExcelFileId=s.ImportExcelFileId,
                            NationCode=s.NationCode,
                            Mobile=s.Mobile,
                            LastName=s.LastName,
                            JobTitle=s.JobTitle,
                            
                         
                    }).ToListAsync();

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

      public async Task<ApiResult<ImportFileInfo>> GroupImportFileDetails(int importId)
        {
            var res = new ApiResult<ImportFileInfo>(true, ApiResultStatusCode.Success, new ImportFileInfo(), "");

            try
            {
                var file = await _fileExcel.Include(w => w.ImportByUser).Where(w => w.Id == importId).FirstOrDefaultAsync();
                if (file != null)
                {
                    res.Data.ImportId = file.Id;
                    res.Data.IsConfirm = file.IsConfirmed;
                    res.Data.ImportBy = file.ImportByUser.DisplayName;
                    res.Data.OnDate = file.CreationDate;
                    res.Data.ImportFileGroupDetails = await _fileDetails.Where(w => w.ImportExcelFileId == importId)
                        .Select(s => new ImportFileGroupDetails()
                        {
                             Citizen=s.Citizen==null ? "":s.Citizen.FirstName +" "+ s.Citizen.LastName,
                             CitizenId=s.CitizenId,
                             ImportId=s.ImportExcelFileId,
                             NationCode=s.NationCode,
                             Id=s.Id


                        }).ToListAsync();

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



        public async Task<ApiResult> ConfirmNationCodeImportListToGroup(int importId, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");

            var listGroupAdd = new List<GroupsCitizens>();
            var listQueueAdd = new List<CitizensQueue>();
         
            try
            {
                var importFile = await _fileExcel.Include(w => w.ImportExcelFileDetails).FirstOrDefaultAsync(w => w.Id == importId);
                if (importFile == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "فایلی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if (importFile.IsConfirmed == true)
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا تایید شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }


                var list = importFile.ImportExcelFileDetails;
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var citizenId = item.CitizenId;  
                        if(citizenId==null)
                        {
                            //یک بار دیگه چک کن شهروند ثبت نام نکرده باشد 
                            var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == item.NationCode);
                            if (citizen != null)
                                citizenId = citizen.CitizenId;
                        } 

                        if (citizenId  != null)
                        {
                            if (!await _citizenGroup.AnyAsync(w => w.GroupId == importFile.GroupId
                            && w.CitizenId== citizenId
                            ))
                            {
                                //اگر شهروند در گروه وجود نداشت اونو به  گروه اضافه کن
                                listGroupAdd.Add(new  GroupsCitizens()
                                {
                                     AddByUserId=userId,
                                     CitizenId= citizenId.Value,
                                     CreationDate=DateTime.Now,
                                     GroupId= importFile.GroupId.Value,
                                     

                                });
                            }

                        }
                        else
                        {
                            //کد ملی به صف شهروندی اضافه کن
                            listQueueAdd.Add(new CitizensQueue()
                            {
                                AddByUserId=userId,
                                NationCode=item.NationCode,
                                CreationDate=DateTime.Now,
                                GroupId= importFile.GroupId.Value, 
                            });
                        }
                    
                    
                    
                    }

                    if (listGroupAdd.Any() || listQueueAdd.Any())
                    {
                        importFile.IsConfirmed = true;
                        if (listGroupAdd.Any())
                        {
                            _citizenGroup.AddRange(listGroupAdd);
                        }
                        if (listQueueAdd.Any())
                        {
                            listQueueAdd.AddRange(listQueueAdd);
                        } 
                        await _uow.SaveChangesAsync();
                    }
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



        #region RefundList
       
        public async Task<ApiResult<List<RefundImportFileInfo>>> RefundImportFileList()
        {
            var res = new ApiResult<List<RefundImportFileInfo>>(true, ApiResultStatusCode.Success, new List<RefundImportFileInfo>(), "");
            
            try
            {
                res.Data = await _fileExcel
                    .Where(w => 
                    w.IsDeleted!=true
                    &&
                    w.ImportExcelFileType == Common.GlobalEnum.ImportExcelFileTypeEnum.لیست_استرداد).Select(s => new RefundImportFileInfo()
                {
                    FileName=s.ExportFileName,
                    ImportBy=s.ImportByUser.DisplayName,
                    ImportId=s.Id,
                    IsConfirm=s.IsConfirmed,
                    OnDate=s.CreationDate, 
                }).OrderByDescending(o=>o.ImportId).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;

        }

     


        public async Task<ApiResult<UploadFileResult>> AddRefundImportFile(List<RefundExcelFileColumns> list, string fileName, int userId)
        {
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            var listAdd = new List<RefundImportFileDetails>();
            try
            {
                var file = new ImportExcelFile()
                {
                    ExportFileName = fileName,
                    ImportByUserId = userId,
                    ImportExcelFileType = Common.GlobalEnum.ImportExcelFileTypeEnum.لیست_استرداد,
                    CreationDate = DateTime.Now,
                    CountRow = list.Count

                };

                foreach (var item in list)
                {

                    if (string.IsNullOrWhiteSpace(item.NationalCode) )
                    {
                        continue;
                    }
                    //چک کن ببین کد ملی اوکی هست
                    //چک کن ببین شماره موبایل اوکی هست

                    var nationCode = item.NationalCode.Trim();
                    var citizenid = 0;
                    var citizen = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                    if (citizen == null)
                    {
                        nationCode = nationCode.Fa2En();
                        var citizen3 = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                        if (citizen3 != null)
                            citizenid = citizen3.CitizenId;
                    }
                    if(citizenid == 0)
                    {
                        if (citizen != null)
                            citizenid = citizen.CitizenId;
                        else if (nationCode.Length < 10)
                        {
                            if (nationCode.Length == 9)
                                nationCode = "0" + nationCode;
                            else if (nationCode.Length == 8)
                                nationCode = "00" + nationCode;
                            var citizen2 = await _citizen.FirstOrDefaultAsync(w => w.NationCode == nationCode);
                            if (citizen2 != null)
                                citizenid = citizen2.CitizenId;
                        }
                    }
                    

                    if (citizenid != 0)
                    {
                        listAdd.Add(new RefundImportFileDetails()
                        { 
                            ImportExcelFile = file,
                            CitizenId= citizenid,
                            Description=item.Description,
                            OrderId=item.OrderId,
                            SaleReferenceId=item.SaleReferenceId,
                            OtherDescription=item.OtherDescription,
                            RefundAmount=item.RefundAmount,
                            TotalRefundAmount=item.TotalRefundAmount, 
                        });
                    }
                     
                  
                }

                if (listAdd.Any())
                {
                    file.CountRow = listAdd.Count;
                    await _fileExcel.AddAsync(file);
                    _refundImportFileDetails.AddRange(listAdd);
                    await _uow.SaveChangesAsync();
                    res.Data.ImportId = file.Id;
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

        public async Task<ApiResult<RefundImportFileInfo>> RefundImportFileDetails(int importId)
        {
            var res = new ApiResult<RefundImportFileInfo>(true, ApiResultStatusCode.Success, new RefundImportFileInfo(), "");
            if(importId==0)
            {
                res.IsSuccess = false;
                res.Messages = "شناسه فایل مشخص نشده است";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }
            
            
            
            
            var listAdd = new List<RefundExcelFileDetails>();
            try
            {
                var file = await _fileExcel.Include(w => w.ImportByUser).Where(w => w.Id == importId).FirstOrDefaultAsync();
                if (file != null)
                {
                    res.Data.ImportId = file.Id;
                    res.Data.IsConfirm = file.IsConfirmed;
                    res.Data.ImportBy = file.ImportByUser.DisplayName;
                    res.Data.OnDate = file.CreationDate;
                    res.Data.RefundList = await _refundImportFileDetails.Where(w => w.ImportExcelFileId == importId).Select(s => new RefundExcelFileDetails()
                    {
                        Citizen=s.Citizen.FirstName +" "+s.Citizen.LastName,
                        CitizenId=s.CitizenId.Value,
                        UserCode=s.Citizen.UserCode,
                        Description=s.Description,
                        NationalCode=s.Citizen.NationCode,
                        OrderId=s.OrderId,
                        OtherDescription=s.OtherDescription,
                        RefundAmount=s.RefundAmount,
                        SaleReferenceId=s.SaleReferenceId,
                        TotalRefundAmount=s.TotalRefundAmount,
                        RefundCardNumber=s.RefundCardNumber,
                        Id=s.Id

                        
                         
                    }).OrderBy(o => o.CitizenId).ToListAsync();

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




        #endregion

      


    }

}
