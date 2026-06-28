using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Sale
{
    public class SaleItemDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }


        /// <summary>
        /// شرح کامل محصول 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// شناسه محصول
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal Tax { get; set; }



        public int ProductUnitId { get; set; }

        public long Price { get; set; }

     

        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }

    public class SaleItemInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string ProductUnit { get; set; }
        public int ProductUnitId { get; set; }

        public long Price { get; set; }


        /// <summary>
        /// شرح کامل محصول 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// شناسه محصول
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal Tax { get; set; }

        public string ImageUrl { get; set; }
 public string ThumbnailUrl { get; set; }



        public int? CreatedById { get; set; }
        public virtual string CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }



        public virtual string ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }




        public bool IsActive { get; set; }
    }





}
