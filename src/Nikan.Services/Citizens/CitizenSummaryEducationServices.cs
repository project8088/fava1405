using Nikan.Common;
using Nikan.DataLayer.Context;
 
using Nikan.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nikan.DomainClasses.Citizens;

namespace cle.Services.Citizens
{
    public interface ICitizenSummaryEducationServices
    {
        Task<ApiResult<karjoEducationDto>> AddOrUpdate(karjoEducationDto model, int CreatedById);
        Task<ApiResult<List<karjoEducationDto>>> GetAllEducation(int userid);
        Task<ApiResult<karjoEducationDto>> GetItem(int id);
        Task<ApiResult<string>> Remove(int id);
    }
    public class CitizenSummaryEducationServices : ICitizenSummaryEducationServices
    {


        #region Ctor
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<CitizenSummaryEducation> _UserSummaryEducationRepository;

        public CitizenSummaryEducationServices(IUnitOfWork uow)
        {
            _unitOfWork = uow ?? throw new ArgumentNullException(nameof(_unitOfWork));
            _UserSummaryEducationRepository = _unitOfWork.Set<CitizenSummaryEducation>();
        }
        #endregion


        public async Task<ApiResult<List<karjoEducationDto>>> GetAllEducation(int userid)
        {

            var res = new ApiResult<List<karjoEducationDto>>(true, ApiResultStatusCode.Success, null);
            try
            {
                var list = await _UserSummaryEducationRepository.Where(g => g.CitizenId == userid)
              .Select(s => new karjoEducationDto()
              {
                  DateOfEnd = s.DateOfEnd,
                  DateOfStart = s.DateOfStart,
                  GradeId = s.Grade, 
                  Grade = s.Grade.ToString(),
                  MajorId = s.MajorId,
                  Major = s.Major == null ? "" : s.Major.Title,
                  Id = s.Id,
                  University = s.University,
                  CitizenId = s.CitizenId 

              }).ToListAsync();


                res.Data = list;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";



            }

            return res;
        }

        public async Task<ApiResult<karjoEducationDto>> GetItem(int id)
        {

            var res = new ApiResult<karjoEducationDto>(true, ApiResultStatusCode.Success, null);
            try
            {
                var list = await _UserSummaryEducationRepository.Where(g => g.Id == id)
              .Select(s => new karjoEducationDto()
              {
                  DateOfEnd = s.DateOfEnd,
                  DateOfStart = s.DateOfStart,
                  GradeId = s.Grade,
                  Grade = s.Grade.ToString(),
                  MajorId = s.MajorId,
                  Major = s.Major == null ? "" : s.Major.Title,
                  Id = s.Id,
                  University = s.University,
                  CitizenId = s.CitizenId


              }).FirstOrDefaultAsync();


                res.Data = list;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";



            }

            return res;
        }


        public async Task<ApiResult<karjoEducationDto>> AddOrUpdate(karjoEducationDto model, int CreatedById)
        {

            var result = new ApiResult<karjoEducationDto>(true, ApiResultStatusCode.Success, model);

            if (_UserSummaryEducationRepository.Any(w => w.Id != model.Id
           && w.CitizenId == model.CitizenId
           && w.MajorId == model.MajorId
           && w.Grade == model.GradeId))
            {

                result.Messages = "این سابقه تحصیلی قبلا ثبت شده است";
                result.IsSuccess = false;
                result.StatusCode = ApiResultStatusCode.BadRequest;
                return result;
            }


            try
            {
                if (model.Id == null)
                {
                    var add = new CitizenSummaryEducation()
                    {
                        CitizenId = CreatedById, 
                        DateOfEnd = model.DateOfEnd,
                        DateOfStart = model.DateOfStart,
                        Grade = model.GradeId,  
                        MajorId = model.MajorId,
                        University = model.University, 

                    };

                    _UserSummaryEducationRepository.Add(add);
                    await _unitOfWork.SaveChangesAsync();
                    model.Id = add.Id;
                    result.Data = model;
                    

                }
                else
                {

                    var edu =await _UserSummaryEducationRepository.FirstOrDefaultAsync(w=>w.Id== model.Id.Value);
                    if (edu != null)
                    {
                        edu.MajorId = model.MajorId;
                        edu.University = model.University;
                        edu.DateOfEnd = model.DateOfEnd;
                        edu.DateOfStart = model.DateOfStart;
                        edu.Grade = model.GradeId;
                        _UserSummaryEducationRepository.Update(edu);
                        await _unitOfWork.SaveChangesAsync(); 
                        result.Data = model;
                    }
                    

                }



            }
            catch (Exception er)
            {

                result.IsSuccess = false;
                result.StatusCode = ApiResultStatusCode.ServerError;
                result.Messages = "خطایی رخ داده است";


            }


            return result;
        }


        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "حذف شد", "");
            var item =await _UserSummaryEducationRepository.FirstOrDefaultAsync( w=>w.Id== id);
            if (item == null)
            {
                res.IsSuccess = false;
                res.Data = null;
                res.Messages = "رکوردی یافت نشد";
                return res;
            }
             
            _UserSummaryEducationRepository.Remove(item);
            await _unitOfWork.SaveChangesAsync();
         
            return res;
        }















    }
}
