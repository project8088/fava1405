using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{
    public class MenuItemDto
    {
        public int? Id { get; set; }

        /// <summary>
        /// عنوان منو
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// مسیر منو
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// ترتیب
        /// </summary>
        public int? TabOrder { get; set; }


        /// <summary>
        /// آیکن منو
        /// </summary>
        public string IconFile { get; set; }


        /// <summary>
        /// منوی والد
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// آیا نمایش داده شود ؟
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// آیا در صفحه جدید باز شود ؟
        /// </summary>
        public bool OpenInNewPage { get; set; }
        /// <summary>
        /// آیا منو غیرفعال باشد ؟
        /// </summary>
        public bool DisableLink { get; set; }
        public bool IsSystem { get; set; }



    }
    public class MenuItemInfo
    {

        


        public int Id { get; set; } 
        public string MenuName { get; set; }
        public string MenuPath { get; set; }


        public int? TabOrder { get; set; }

        public string IconFile { get; set; }


        public DateTime CreatedOnDate { get; set; }
        public DateTime? ModifiedOnDate { get; set; }





        public int? ParentId { get; set; }
        public string  Parent { get; set; }




        public int? CreatedByUserId { get; set; }
        public string  CreatedByUser { get; set; }


        public int? LastModifiedByUserId { get; set; }
        public string LastModifiedByUser { get; set; }

        public bool IsVisible { get; set; }
      
        public bool OpenInNewPage { get; set; }

        public bool DisableLink { get; set; }


        public bool IsSystem { get; set; }
         

    }

    public class SortMenuDto
    {

        public int id { get; set; }
        public List<SortMenuDto> children { get; set; }

    }
    public class MenuItemsListDto
    {
        public int  Id { get; set; }

        /// <summary>
        /// عنوان منو
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// مسیر منو
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// ترتیب
        /// </summary>
        public int? TabOrder { get; set; }


        /// <summary>
        /// آیکن منو
        /// </summary>
        public string IconFile { get; set; }

        /// <summary>
        /// منوی والد
        /// </summary>
        public int? ParentId { get; set; } 
        public string  ParentName { get; set; }



        /// <summary>
        /// آیا نمایش داده شود ؟
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// آیا در صفحه جدید باز شود ؟
        /// </summary>
        public bool OpenInNewPage { get; set; }
        /// <summary>
        /// آیا منو غیرفعال باشد ؟
        /// </summary>
        public bool DisableLink { get; set; }

   public bool IsSystem { get; set; }


    }




    public class TreeMenu
    {
        public TreeMenu()
        {

            Children = new HashSet<TreeMenu>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }

        public bool DisableLink { get; set; }
        public bool OpenInNewPage { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsVisible { get; set; }

        public int? ParentId { get; set; }

        public bool IsSystem { get; set; }

        public ICollection<TreeMenu> Children { get; set; }

    }








}
