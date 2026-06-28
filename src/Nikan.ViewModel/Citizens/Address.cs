using System;
using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;

namespace Nikan.ViewModel.Citizens
{

    /// <summary>
    /// مشخصات آدرس
    /// </summary>
    public class AddressInfo
    {

        public int Id { get; set; }
        public AddressTypeEnum AddressType { get; set; }
        public string  Citizen { get; set; }
        public int?  CitizenId { get; set; }

        public int? CityId { get; set; }
        public BaseDataModel City { get; set; } 
        public int? Region { get; set; }
        public string Street { get; set; }
        public string Alley { get; set; }
        public string PostalCode { get; set; }
        public string Plaque { get; set; }
        public string FullAddress { get; set; }


        /// <summary>
        /// تلفن ثابت
        /// </summary>
        public string Phone { get; set; }


        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LasteUpdateOnDate { get; set; }
    }


    public class  AddressDto
    {

        public int? Id { get; set; }


        public int? CardId { get; set; }



        /// <summary>
        /// نوع آدرس
        /// </summary>
        public AddressTypeEnum AddressType { get; set; }
        
        /// <summary>
        /// استان و شهر محل سکونت
        /// </summary>
        public int  CityId { get; set; }
        
        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }


        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public string  UserCode { get; set; }



        /// <summary>
        /// منطقه
        /// </summary>
        public int? Region { get; set; }


        /// <summary>
        /// خیابان
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// کوچه
        /// </summary>
        public string Alley { get; set; }


        /// <summary>
        /// کدپستی
        /// </summary>
        public string PostalCode { get; set; }


        /// <summary>
        /// پلاک
        /// </summary>
        public string Plaque { get; set; }


        /// <summary>
        /// آدرس کامل شهروند
        /// </summary>
        public string FullAddress { get; set; }




        /// <summary>
        /// تلفن ثابت
        /// </summary>
        public string Phone { get; set; }



    }



}
