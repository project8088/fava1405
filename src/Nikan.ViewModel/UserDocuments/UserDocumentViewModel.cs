using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.UserDocuments
{
    public class UserDocumentGroupDto
    {
         
        public int? Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public int IndexOrder { get; set; } 
        public virtual bool IsActive { get; set; }



    }
    public class UserDocumentGroupInfo
    {


        
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public int IndexOrder { get; set; }

        public virtual bool IsActive { get; set; }



    }


    public class UserDocumentDto
    {


        public int Id { get; set; }
        public string Title { get; set; }

        public string FileName { get; set; }




        public long Size { get; set; }


        public string Extension { get; set; }


        public DateTime AttachedOnDate { get; set; }

        public string FilePath { get; set; }

        public string ThumnailPath { get; set; }


        public int OwnerId { get; set; } 
      
        public virtual int DocumentGroupId { get; set; }
         

    }

     
    public class UserDocumentInfo
    {
      

        
        public int Id { get; set; } 
        public string Title { get; set; } 
      
        public string FileName { get; set; }



 
        public long Size { get; set; }

         
        public string Extension { get; set; }


        public DateTime AttachedOnDate { get; set; } 
    
        public string FilePath { get; set; }
         
        public string ThumnailPath { get; set; } 


        public int OwnerId { get; set; }

        public virtual string  Owner { get; set; }


        public virtual string DocumentGroup { get; set; }
        public virtual int DocumentGroupId { get; set; }

        public virtual string DocumentGroupDescription { get; set; }


        public UserDocumentStatusEnum DocumentStatus { get; set; }
        public string Description { get; set; }


    }

  




}
