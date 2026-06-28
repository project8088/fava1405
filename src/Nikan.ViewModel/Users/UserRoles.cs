using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Users
{
    public class RolesInfo
    {  
        public int Id { get; set; }
        public string Name { get; set; } 
    }




    public class UserRolesInfo
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; }
        public List<int> RoleIds { get; set; }


    }

    public class RoleInfo
    {
        public int RoleId { get; set; }

        public string Role { get; set; }
    }

    public class UserAllRole
    {

        public int RoleId { get; set; }

        public int UserId { get; set; }


        public string Role { get; set; }

        public string UserName { get; set; }



    }








}
