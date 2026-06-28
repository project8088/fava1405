using System;

namespace Nikan.DomainClasses.UserCompanes
{
    public class CompanyPersonnel
    {
        public int Id { get; set; }
        public bool Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string FatherName { get; set; } 
        public DateTime? BirthDate { get; set; } 
        public string Mobile { get; set; } 
        public string JobTitle { get; set; } 
        public string Address { get; set; } 

        public int ImportExcelFileId { get; set; } 
        public ImportExcelFile ImportExcelFile { get; set; } 

    }
}
