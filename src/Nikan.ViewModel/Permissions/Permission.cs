using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Permissions
{
    public class PermissionList
    {
        public string Category { get; set; }
        public IEnumerable<BaseDataModel> Permissions { get; set; }

    }

    public class UserPermissions
    {
        public int GroupId { get; set; }
        public List<int> Permissions { get; set; }

    }



    public class WebApiUserPermissions
    {
        public int UserId { get; set; }
        public List<int> Permissions { get; set; }

    }








    public class PermissionGroupDto
    {

        public int? Id { get; set; }
        public string Name { get; set; }
        

    }




    public class UserPermissionGroupDto
    {
        public int? Id { get; set; }
        public int PermissionGroupId { get; set; } 
        public int UserId { get; set; }
      


    }


    public class UserPermissionGroupInfo
    {
        public int? Id { get; set; }
        public int PermissionGroupId { get; set; }
        public string  PermissionGroup { get; set; }
        public int UserId { get; set; }
        public virtual string UserName { get; set; }


    }


    public class WebUserAccessRangeIpDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }


    }

    public class WebUserAccessRangeIpInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Start { get; set; }
        public string End { get; set; }


    }

}
