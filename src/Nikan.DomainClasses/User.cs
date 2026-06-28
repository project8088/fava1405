using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.UserCompanes;
using System;
using System.Collections.Generic;

namespace Nikan.DomainClasses
{
    public class LosginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }  
        public int?   ServiceId { get; set; }
        public string CaptchaId { get; set; }
        public string UserEnteredCaptchaCode { get; set; }



    }

    public class WebApiLosginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int? ServiceId { get; set; }
       

    }

    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
            CreatedOnDate = DateTime.Now;
        }




        public int Id { get; set; }


        public Guid? UserCode { get; set; }


        public string Username { get; set; }

        public string Password { get; set; }


        public string OldPassword { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public bool EmailVerification { get; set; }

        public string MobileNumber { get; set; }


        public bool MobileNumberVerification { get; set; }

        public DateTime? LastLoggedIn { get; set; }

        public DateTime? CreatedOnDate { get; set; }
         

        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public virtual UserCompany UserCompany { get; set; }
        public int? UserCompanyId { get; set; }

        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }



        public bool IsAdmin { get; set; }



        public userAccountStateEnum UserAccountState  { get; set; }

        /// <summary>
        /// تا چه تاریخی کاربر غیرفعال شود؟
        /// </summary>
        public DateTime? DeactivationDate { get; set; }


        public bool IsSystem { get; set; }


        public int? RegisterByUserId { get; set; }
        public virtual User RegisterByUser { get; set; }




        public virtual string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }
    }


    /// <summary>
    /// این کاربر به چه سرویس هایی دسترسی دارد؟
    /// </summary>
    public class UserAppServiceAccess
    {
        public int Id { get; set; }

        /// <summary>
        /// کاربر
        /// </summary>
        public User User { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// این کاربر به چه خدمتی دسترسی دارد؟
        /// </summary>
        public int  AccessServiceId { get; set; }
        public AppServices AccessService { get; set; }

    }

}
