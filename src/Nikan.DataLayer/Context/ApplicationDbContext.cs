using Nikan.DomainClasses;
using Microsoft.EntityFrameworkCore; 
using Nikan.DomainClasses.UserCompanes;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Job;
using Nikan.DomainClasses.NewsItem;
using Nikan.DomainClasses.Faq; 
using Nikan.DomainClasses.Factor;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.CitizensCard;
using Nikan.DomainClasses.Permissions;
using Nikan.DomainClasses.UserDocuments;
using Nikan.DomainClasses.Refund;
 
 

namespace Nikan.DataLayer.Context
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Role> Roles { set; get; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<OrganizationalPosition> OrganizationalPosition { set; get; }

        public virtual DbSet<Organization> Organization { set; get; } 
         public virtual DbSet<OrganizationalUnit> OrganizationalUnit { set; get; }
        public virtual DbSet<OrganizationalUnitGroups> OrganizationalUnitGroups { set; get; }
        

        public virtual DbSet<TicketMessage> TicketMessage { set; get; }
        public virtual DbSet<TicketComments> TicketComments { set; get; }
        public virtual DbSet<TicketActivity> TicketActivity { set; get; }




        public virtual DbSet<Ticket> Ticket { set; get; }
        public virtual DbSet<FaqQuestion> FaqQuestion { set; get; }

        public virtual DbSet<FaqQuestionGroupType> FaqQuestionGroupType { set; get; }


        public virtual DbSet<BaseData> BaseData { set; get; }


        public virtual DbSet<SiteOption> SiteOption { set; get; }

        public virtual DbSet<Permission> Permission { set; get; }
        public virtual DbSet<UserPermission> UserPermission { set; get; }
        public virtual DbSet<PermissionGroup> PermissionGroup { set; get; }
        public virtual DbSet<WebUserPermission> WebUserPermission { set; get; }

        public virtual DbSet<UserPermissionGroup> UserPermissionGroup { set; get; }
        public virtual DbSet<WebUserAccessRangeIp> WebUserAccessRangeIp { set; get; }

        

        public virtual DbSet<Attachment> Attachment { set; get; }



         public virtual DbSet<ImportExcelFile> ImportExcelFile { set; get; }
         public virtual DbSet<ImportExcelFileDetails> ImportExcelFileDetails { set; get; }
          public virtual DbSet<UserAppServiceAccess> UserAppServiceAccess { set; get; }

        


         public virtual DbSet<ExportCitizens> ExportCitizens { set; get; }
         public virtual DbSet<ExportedCitizens> ExportedCitizens { set; get; }


        public virtual DbSet<Event> Event { set; get; }

        public virtual DbSet<ArchiveEvent> ArchiveEvent { set; get; }





        #region News
        public virtual DbSet<News> News { set; get; }

        public virtual DbSet<NewsGroup> NewsGroup { set; get; }

        public virtual DbSet<NewsComment> NewsComment { set; get; }

        public virtual DbSet<MenuItem> MenuItem { set; get; }

        public virtual DbSet<WebPage> WebPage { set; get; }
        public virtual DbSet<NewsForCitizen> NewsForCitizen { set; get; }
        public virtual DbSet<NewsReads> NewsReads { set; get; }

       
         public virtual DbSet<Manzalat> Manzalat { set; get; }
         public virtual DbSet<ManzalatDocumentInfo> ManzalatDocumentInfo { set; get; } 
         public virtual DbSet<ManzalatBaseForm> ManzalatBaseForm { set; get; }




        #endregion


        #region Job

        public virtual DbSet<JobTitle> JobTitle { set; get; }
        public virtual DbSet<Major> Major { set; get; }
        public virtual DbSet<Skill> Skill { set; get; }
         

        #endregion

        #region Company
        public virtual DbSet<UserCompanyPersonel> UserCompanyPersonel { set; get; }
        public virtual DbSet<UserCompany> UserCompany { set; get; }
        public virtual DbSet<UserCompanyFieldOfActivity> UserCompanyFieldOfActivity { set; get; }
         public virtual DbSet<UserCompanyActivities> UserCompanyActivities { set; get; }
    
        
        
        #endregion 

      
        public virtual DbSet<City> City { set; get; }
        public virtual DbSet<SmsInfo> SmsInfo { set; get; } 
       public virtual DbSet<ArchiveSmsInfo> ArchiveSmsInfo { set; get; } 
        public virtual DbSet<FactorMaster> FactorMaster { set; get; }
        public virtual DbSet<FactorDetail> FactorDetail { set; get; }
        public virtual DbSet<UserTransaction> UserTransaction { set; get; }
       


      
  
        public virtual DbSet<AppServices> AppServices { set; get; }
        public virtual DbSet<Citizen> Citizen { set; get; }
        public virtual DbSet<CitizenProfile> CitizenProfile { set; get; } 
        public virtual DbSet<Nationality> Nationality { set; get; }
       public virtual DbSet<CitizensAuthentication> CitizensAuthentication { set; get; }








        public virtual DbSet<CitizensDead> CitizensDead { set; get; }
        public virtual DbSet<QueueCheckingCitizensDead> QueueCheckingCitizensDead { set; get; }



        public virtual DbSet<Feedback> Feedback { set; get; }

        public virtual DbSet<CitizenFeedback> CitizenFeedback { set; get; }

        public virtual DbSet<JobGroup> JobGroup { set; get; }
       public virtual DbSet<EducationGroup> EducationGroup { set; get; }

        public virtual DbSet<CitizenSummaryEducation> CitizenSummaryEducation { set; get; }

        public virtual DbSet<CitizenFamily> CitizenFamily { set; get; }


         public virtual DbSet<GroupsCitizens> GroupsCitizens { set; get; } 
         public virtual DbSet<Group> Group { set; get; }
        
           
        
        public virtual DbSet<CitizensQueue> CitizensQueue { set; get; }

         public virtual DbSet<SlideShow> SlideShow { set; get; }

         public virtual DbSet<UserLoginTickets> UserLoginTickets { set; get; }
         public virtual DbSet<UserLoginTickets_Archive> UserLoginTickets_Archive { set; get; }


        public virtual DbSet<CompanyPersonnel> CompanyPersonnel { set; get; }

        public virtual DbSet<UserDocumentGroup> UserDocumentGroup { set; get; }

        public virtual DbSet<UserDocument> UserDocument { set; get; }


        public virtual DbSet<TransactionRefund> TransactionRefund { set; get; }
        public virtual DbSet<TransactionRefundImport> TransactionRefundImport { set; get; }
        public virtual DbSet<RefundImportFileDetails> RefundImportFileDetails { set; get; }




         public virtual DbSet<CardInfo_RequestFreeCard> CardInfo_RequestFreeCard { set; get; } 
         public virtual DbSet<CardInfo_RequestFreeCard_Citizens> CardInfo_RequestFreeCard_Citizens { set; get; }








        #region Card
        public virtual DbSet<CardType> CardType { set; get; }
        public virtual DbSet<CardInfo> CardInfo { set; get; }
        public virtual DbSet<CitizensCard> CitizensCard { set; get; }
        public virtual DbSet<CardInfo_Discount> CardInfo_Discount { set; get; }

       public virtual DbSet<CardInfo_Discount_Group> CardInfo_Discount_Group { set; get; }
       public virtual DbSet<CardInfo_PermissionsForGroups> CardInfo_PermissionsForGroups { set; get; }

       public virtual DbSet<CitizensCardBackCard> CitizensCardBackCard { set; get; }
       public virtual DbSet<CitizensCardCancellation> CitizensCardCancellation { set; get; }

     public virtual DbSet<CitizensCardConvertType> CitizensCardConvertType { set; get; }

     public virtual DbSet<CardInfo_Discount_Center> CardInfo_Discount_Center { set; get; }

        

public virtual DbSet<CardInfo_DistributeCard_QueueInfo> CardInfo_DistributeCard_QueueInfo { set; get; }

        public virtual DbSet<CardInfo_DistributeCard_Courses> CardInfo_DistributeCard_Courses { set; get; }

    public virtual DbSet<CardInfo_DistributeCard_Queue_Groups> CardInfo_DistributeCard_Queue_Groups { set; get; }

     public virtual DbSet<CardInfo_DistributeCard> CardInfo_DistributeCard { set; get; }




        
      public virtual DbSet<CardInfo_Export> CardInfo_Export { set; get; } 
      public virtual DbSet<CardInfo_Export_Citizen> CardInfo_Export_Citizen { set; get; }
         

#endregion



protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            builder.Entity<UserTransaction>(entity =>
            { 
                entity.HasIndex(e => e.TransactionId).IsUnique();
                entity.Property(e => e.TransactionId).IsRequired();
               
            });

            // Custom application mappings
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.SerialNumber).HasMaxLength(450);
            });

            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoleId);
                entity.Property(e => e.UserId);
                entity.Property(e => e.RoleId);
                entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
                entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
            });

            builder.Entity<UserToken>(entity =>
            {
                entity.HasOne(ut => ut.User)
                      .WithMany(u => u.UserTokens)
                      .HasForeignKey(ut => ut.UserId);

                entity.Property(ut => ut.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
                entity.Property(ut => ut.RefreshTokenIdHashSource).HasMaxLength(450);
            });



            builder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MenuName);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });


             



        }






    








    }
}