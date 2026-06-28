using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel.Users
{

    public class CardUserRegister
    { 
        public string NationCode { get; set; } 
    }



    public class AdminRegister
    {



        public string DisplayName { get; set; }


        public string Email { get; set; }



        public string MobileNumber { get; set; }


        [Required]
        public string UserName { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool Gender { get; set; }
        public string NationCode { get; set; } 
        public string FatherName { get; set; }
       
        public int? AccessServiceId { get; set; }
        public string AccessService { get; set; }



        [Required]
        public string Password { get; set; }

        public virtual string OrganizationalUnitId { get; set; }
        public bool IsGuardRole { get; set; }


    }


    public class CompanyUserRegister
    {

        public int? CompanyId { get; set; }

        public string DisplayName { get; set; }


        public string Email { get; set; }



        public string MobileNumber { get; set; }


        [Required]
        public string UserName { get; set; }



        [Required]
        public string Password { get; set; }

      



    }





    public class UpdateAccount
    {
        /// <summary>
        /// شناسه کاربری
        /// </summary>
        public int UserId { get; set; }

        public string DisplayName { get; set; }


      
        public string Email { get; set; } 
        public string MobileNumber { get; set; } 
        public virtual string OrganizationalUnitId { get; set; }

        public userAccountStateEnum UserAccountState { get; set; }
    }


    public class UpdateCompanyAccount
    {
        /// <summary>
        /// شناسه کاربری
        /// </summary>
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; } 
        public userAccountStateEnum UserAccountState { get; set; }
    }



    public class RegisterModel
    {



        public bool Gender { get; set; }

        [Required]
        public string NationCode { get; set; }



        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
       

        public string Email { get; set; } 
        public string MobileNumber { get; set; }
       
       
        [Required]
        public string Password { get; set; }


      






    }

    public class UserInfoDto
    {
        
         public int UserId { get; set; }
        public Guid? UserCode { get; set; }


        public int? ServiceId { get; set; }
         public string ServiceName { get; set; }

        public string UserName { get; set; }

        public userAccountStateEnum UserAccountState { get; set; }

        public DateTime? LastLoggedIn { get; set; }
        public DateTime? CreatedOnDate { get; set; }

        public string[] Roles { get; set; }
        public string DisplayName { get; set; } 
       
        public string EmailAddress { get; set; }

        public bool EmailVerification { get; set; }

        public string MobileNumber { get; set; }


        public bool MobileNumberVerification { get; set; }
        public virtual BaseData OrganizationalUnit  { get; set; }
        public virtual BaseData Organization   { get; set; }


        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }


        public UserCompanyAccountStatusEnum UserCompanyAccountStatus  { get; set; }
        public string RejectDesription { get; set; }
        /// <summary>
        /// تا چه تاریخی کاربر غیرفعال شود؟
        /// </summary>
        public DateTime? DeactivationDate { get; set; }

        public List<string> permissions { get; set; }
        public List<ShortGroupsCitizensInfo> CitizenGroups { get; set; }

    public string access_token { get; set; }
        public string refresh_token { get; set; }

    }

    public class ChangePasswordViewModel
    {
         
        public string OldPassword { get; set; } 
        public string NewPassword { get; set; } 

         
    }



    public class AdminChangePasswordViewModel
    {
         public int UserId { get; set; } 
        public string NewPassword { get; set; } 
        
         
    }

    public class ForgotVerifyCodeViewModel
    {
        public string VerifyCode { get; set; } 
        public string MobileNumber { get; set; } 
        public int UserId { get; set; }

    }

    public class UserForgotPasswordViewModel
    {
        public string UserName { get; set; } 
        public string MobileNumber { get; set; }


        public string CaptchaId { get; set; }
        public string UserEnteredCaptchaCode { get; set; }

         
    }
    public class SetForgotPasswordViewModel
    {
        public int UserId { get; set; }
        public string VerifyCode { get; set; }
        public string MobileNumber { get; set; } 
        public string Password { get; set; }



    }

    public class PagedUsersViewModel
    {

        public List<UserInfoDto> Items { get; set; }

        public int TotalItems { get; set; }


    }


    public class UserAppServiceAccessDto
    {

        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public string Service { get; set; }
        public string UserName { get; set; }

    }


}
