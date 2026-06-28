using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// دسترسی برای تخفیف
    /// </summary>
    public   class  CardInfo_PermissionsForGroups
	{
		public int Id { get; set; }
	   
		public int CardTypeId { get; set; }
        public CardType CardType  { get; set; }



		public CardPermissionTypeEnum CardPermissionType { get; set; }
		public int PermissionGroupId { get; set; }
		public Group PermissionGroup { get; set; }

	}

}
