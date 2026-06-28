using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.NewsItems
{
    public class NewsGroupDto
    {


        public int? Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

      
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        

    }

}
