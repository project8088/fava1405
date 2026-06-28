using Nikan.Common.GlobalEnum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Permissions
{
	/// <summary>
	/// حق دسترسی کاربران
	/// </summary>
	public class Permission 
    {


		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public int Id { get; set; } 
		public string Title { get; set; }

		public PermissionTypeEnum PermissionType { get; set; }

		public virtual PermissionCategoryEnum  Category { get; set; }

		/// <summary>
		/// نوع دسترسی
		/// مدیریت 
		/// شرکت
		/// </summary>
		public virtual UserPermissionTypeEnum UserPermissionType  { get; set; }

		public bool IsDeleted { get; set; } 

	}



	/// <summary>
	/// گروههای دسترسی
	/// </summary>
	public class PermissionGroup
	{
		public int Id { get; set; }

		public string Name { get; set; }


	}



	/// <summary>
	///  کاربر در چه گروههای دسترسی وجود دارد
	/// </summary>
	public class UserPermissionGroup
	{
		public int Id { get; set; }
		public int PermissionGroupId { get; set; }
		public PermissionGroup PermissionGroup { get; set; }
		public int UserId { get; set; }
		public virtual User User { get; set; }



	}



	public class UserPermission
	{
		public int Id { get; set; }
		public int PermissionGroupId { get; set; }
		public PermissionGroup PermissionGroup { get; set; }
		public int PermissionId { get; set; }
		public virtual Permission Permission { get; set; }


	}



	/// <summary>
	///دسترسی اصفهان کارت
	/// دسترسی توسعه دهندگان
	/// </summary>
	public class WebUserPermission
	{

		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public int PermissionId { get; set; }
		public virtual Permission Permission { get; set; }


	}


	public class WebUserAccessRangeIp
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public string Start { get; set; }
		public string End { get; set; }
		public string ExceptionIP { get; set; }

	}




}
