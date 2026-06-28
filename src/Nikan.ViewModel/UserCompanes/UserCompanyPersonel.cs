using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.UserCompanes
{
    public class UserCompanyPersonelDto
    {
        public int? Id { get; set; }
        public string PersonelCode { get; set; }

        /// <summary>
        /// شرکت
        /// </summary>
        public int? UserCompanyId { get; set; }
       

        /// <summary>
        /// پست سازمانی
        /// </summary>
        public int OrganizationalPositionId { get; set; } 
        public virtual NamePrefixEnum NamePrefix { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(50)]
        public string FatherName { get; set; }
        [StringLength(50)]
        public string NationCode { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string CellNumber { get; set; }



        [StringLength(100)]
        public string Email { get; set; }

      
        public int? CityId { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }



        [StringLength(100)]
        public string ZipCode { get; set; }



        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }


        public string Office { get; set; }

        public string OfficePhoneNumber { get; set; }

        public DateTime? EmployeementOnDate { get; set; }


       public string Biography { get; set; }


        public bool IsManagementMembers { get; set; }

        /// <summary>
        /// آیا دارای بیماری خاصی است ؟
        /// </summary>
        public bool HasSpecificDisease { get; set; }

        /// <summary>
        /// توضیحات بیماری
        /// </summary>
        public string DescriptionDisease { get; set; }


    }
    public class UserCompanyPersonelInfo
    {
        public int Id { get; set; }
        public string PersonelCode { get; set; }

        /// <summary>
        /// شرکت
        /// </summary>
        public int? UserCompanyId { get; set; }
        public virtual string  UserCompany { get; set; }

        /// <summary>
        /// پست سازمانی
        /// </summary>
        public int OrganizationalPositionId { get; set; }
        public virtual string  OrganizationalPosition { get; set; }

        public virtual NamePrefixEnum NamePrefix { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(50)]
        public string FatherName { get; set; }
        [StringLength(50)]
        public string NationCode { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string CellNumber { get; set; }



        [StringLength(100)]
        public string Email { get; set; }

        public virtual BaseDataModel City { get; set; }
        public virtual BaseDataModel Province { get; set; }




        public int? CityId { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }



        [StringLength(100)]
        public string ZipCode { get; set; }



        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }


        public string Office { get; set; }

        public string OfficePhoneNumber { get; set; }

        public DateTime? EmployeementOnDate { get; set; }


        public string Biography { get; set; }

        public bool IsManagementMembers { get; set; }

        /// <summary>
        /// آیا دارای بیماری خاصی است ؟
        /// </summary>
        public bool HasSpecificDisease { get; set; }

        /// <summary>
        /// توضیحات بیماری
        /// </summary>
        public string DescriptionDisease { get; set; }

    }
    public class PagedUserCompanyPersonelViewModel
    {

        public List<UserCompanyPersonelInfo> Members { get; set; }
        public int TotalItems { get; set; }


    }



    public class CompanyPersonnelInfo
    {
        public bool Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }

        public int ImportExcelFileId { get; set; } 

        public int CitizenId { get; set; }

        public DateTime  RegisterOnDate  { get; set; }



    }




    public class AddCompanyPersonnelInfo
    {
        public string Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string  BirthDate { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
        public string Address { get; set; }
         
    }




}
