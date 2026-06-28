using System;

namespace Nikan.ViewModel.Faq
{



    public class FaqQuestionModel
    {

    
        public int? Id { get; set; }  
        public string Title { get; set; } 
        public string Description { get; set; } 
        public virtual int QuestionGroupTypeId { get; set; } 
        public virtual string TagNames { get; set; } 
        public virtual string Icon { get; set; }  
        public bool IsActive { get; set; } 
        public int IndexOrder { get; set; } 
      


    }
 

    public class faqDto
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string QuestionGroup { get; set; }
        public virtual int? QuestionGroupTypeId { get; set; }
        public virtual string TagNames { get; set; }
        public virtual int ViewCount { get; set; }
        public DateTime CreateOnDate { get; set; }
        public string OrganizationId { get; set; }
        public bool IsActive { get; set; }

    }
     

}
