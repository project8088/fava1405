using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.DomainClasses.Refund
{
	/// <summary>
	/// فایل های استرداد هزینه
	/// </summary>
	public class TransactionRefundImport
	{
		[Key]
		public int RefundImportId { get; set; }
		public DateTime OnDate { get; set; }



		/// <summary>
		/// بارگذاری شده توسط 
		/// </summary>
		public int? ImportByUserId { get; set; }
		public User ImportByUser { get; set; }

		/// <summary>
		/// شماره نامه اتوماسیون
		/// </summary>
        public string LetterNumber { get; set; }
		public string ImportDescription { get; set; }
		public string UnitName { get; set; }

		public string ClassName { get; set; }


		public int? AccessByCitizenId { get; set; }
		public Citizen AccessByCitizen { get; set; }


		public bool CitizenAccess { get; set; }


		public bool IsClosed { get; set; }
		public bool IsDeleted { get; set; }


		public ICollection<TransactionRefund> TransactionRefunds { get; set; }

	}




	public class TransactionRefund
    {
		[Key]
		public int RefundId { get; set; }
		public string  OrderId { get; set; }

		public string TransactionCode { get; set; }


		public int? RefundByUserId { get; set; }
		public User RefundByUser  { get; set; }



		public int CitizenId { get; set; }
		public Citizen Citizen  { get; set; } 


		public long RefundAmount { get; set; }
		  
		/// <summary>
		/// شماره کارت اعلامی توسط شهروند
		/// </summary>
        public string OwnerBankCardNumber { get; set; }

		/// <summary>
		/// شماره کارت اعلامی توسط بانک
		/// </summary>
		public string RefundCardNumber { get; set; }

		public DateTime? RefundOnDate { get; set; }

		public string Description { get; set; }
		public string OtherDescription { get; set; }

		 public bool? RefundIssuccessful { get; set; }
		
		/// <summary>
		/// وضعیت استرداد
		/// </summary>
		public RefundStateEnum? RefundState  { get; set; }

		public int TransactionRefundImportId { get; set; }
		public TransactionRefundImport TransactionRefundImport { get; set; }



		

		public bool IsClosed { get; set; }
		
		public long TotalRefundAmount { get; set; }

		public bool UserIsAcceptRefund { get; set; }

		public string RefundRefCode { get; set; }
		public string AdminDescription { get; set; }

		public string TransactionXmlInfo { get; set; }
 
	}

	/// <summary>
	/// جزئیات فایل اکسل
	/// </summary>
	public class  RefundImportFileDetails
	{
		 
		public int  Id { get; set; }


		/// <summary>
		/// مشخصات فایل
		/// </summary>
        public int ImportExcelFileId { get; set; } 
		public ImportExcelFile ImportExcelFile  { get; set; }

		public string OrderId { get; set; }
		public string SaleReferenceId { get; set; }
		public string Description { get; set; }

		public int? CitizenId { get; set; }
		public Citizen Citizen { get; set; }

		public string RefundCardNumber { get; set; }

		

		public string OtherDescription { get; set; }
		public long TotalRefundAmount { get; set; }
		public long RefundAmount { get; set; }

	}



}
