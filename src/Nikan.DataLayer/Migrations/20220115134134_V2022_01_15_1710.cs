using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nikan.DataLayer.Migrations
{
    public partial class V2022_01_15_1710 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    LanguageName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Disabled = table.Column<bool>(nullable: true),
                    Selected = table.Column<bool>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ViewOrder = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ViewIcon = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastUpdateDate = table.Column<DateTime>(nullable: true),
                    ExportQuery = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    PostCode = table.Column<int>(nullable: true),
                    ItemLevel = table.Column<int>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_City_ParentId",
                        column: x => x.ParentId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EducationGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FeedbackTitle = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobTitle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    SearchTitle = table.Column<string>(maxLength: 100, nullable: true),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitle_JobTitle_ParentId",
                        column: x => x.ParentId,
                        principalTable: "JobTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    SearchTitle = table.Column<string>(maxLength: 100, nullable: true),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    PageType = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PermissionType = table.Column<int>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    UserPermissionType = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueCheckingCitizensDead",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationCode = table.Column<string>(nullable: true),
                    AddOnDate = table.Column<DateTime>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueCheckingCitizensDead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyFieldOfActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyFieldOfActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocumentGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 2000, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocumentGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCompany",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(maxLength: 500, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: true),
                    EnglishName = table.Column<string>(maxLength: 40, nullable: true),
                    SlagUrl = table.Column<string>(maxLength: 40, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    InsuranceNumber = table.Column<string>(nullable: true),
                    CompanyRepresentative = table.Column<string>(maxLength: 100, nullable: true),
                    EstablishedYear = table.Column<string>(maxLength: 10, nullable: true),
                    TxtTinNo = table.Column<string>(nullable: true),
                    TxtRegNO = table.Column<string>(nullable: true),
                    CompanyOwnerType = table.Column<int>(nullable: false),
                    ActivityLicenseType = table.Column<int>(nullable: false),
                    ActivityLicense = table.Column<int>(nullable: false),
                    ActivityLicenseDate = table.Column<DateTime>(nullable: true),
                    FieldOfActivity = table.Column<int>(nullable: false),
                    ManagerNationCode = table.Column<string>(maxLength: 10, nullable: true),
                    ManagerName = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: true),
                    RegistrationCode = table.Column<string>(nullable: true),
                    UserCompanyStatus = table.Column<int>(nullable: false),
                    RejectDesription = table.Column<string>(nullable: true),
                    UserCompanyAccountStatus = table.Column<int>(nullable: false),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    MobileNumber2 = table.Column<string>(maxLength: 50, nullable: true),
                    MobileNumber3 = table.Column<string>(maxLength: 50, nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lng = table.Column<string>(nullable: true),
                    CellNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CellNumber2 = table.Column<string>(maxLength: 50, nullable: true),
                    CellNumber3 = table.Column<string>(maxLength: 50, nullable: true),
                    SMSNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Fax = table.Column<string>(maxLength: 50, nullable: true),
                    Website = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Telegram = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    ZipCode = table.Column<string>(maxLength: 100, nullable: true),
                    Street = table.Column<string>(maxLength: 100, nullable: true),
                    FullAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    Pelak = table.Column<string>(maxLength: 100, nullable: true),
                    EarthCondition = table.Column<int>(nullable: false),
                    UnitArea = table.Column<decimal>(nullable: false),
                    AreaOfGreenSpace = table.Column<int>(nullable: false),
                    BuildingArea = table.Column<int>(nullable: false),
                    BuildingLicenseArea = table.Column<int>(nullable: false),
                    NumberOfEmployees = table.Column<int>(nullable: false),
                    ContractCode = table.Column<int>(nullable: true),
                    ContractOnDate = table.Column<DateTime>(nullable: true),
                    WaterContractOnDate = table.Column<DateTime>(nullable: true),
                    FileCode = table.Column<string>(nullable: true),
                    WaterDepositId = table.Column<string>(nullable: true),
                    WaterCode = table.Column<string>(nullable: true),
                    ChargeDepositId = table.Column<string>(nullable: true),
                    ChargeCode = table.Column<string>(nullable: true),
                    ChargeMoeinCode = table.Column<string>(nullable: true),
                    WaterMoeinCode = table.Column<string>(nullable: true),
                    VolumeAirTanks = table.Column<int>(nullable: false),
                    IsBusinessUnit = table.Column<bool>(nullable: false),
                    IsBuildingCompany = table.Column<bool>(nullable: false),
                    IssueWaterBill = table.Column<bool>(nullable: false),
                    IssueChargeBill = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 500, nullable: true),
                    ThumbnailUrl = table.Column<string>(maxLength: 500, nullable: true),
                    ContractUrl = table.Column<string>(nullable: true),
                    SignatureUrl = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    LastModifiedOnDate = table.Column<DateTime>(nullable: true),
                    LockEdit = table.Column<bool>(nullable: false),
                    LockOnDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCompany_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionGroupId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermission_PermissionGroup_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(nullable: false),
                    UserCompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCompanyActivities_UserCompanyFieldOfActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "UserCompanyFieldOfActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCompanyActivities_UserCompany_UserCompanyId",
                        column: x => x.UserCompanyId,
                        principalTable: "UserCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyPersonel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelCode = table.Column<string>(nullable: true),
                    UserCompanyId = table.Column<int>(nullable: true),
                    OrganizationalPositionId = table.Column<int>(nullable: false),
                    NamePrefix = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(maxLength: 50, nullable: true),
                    NationCode = table.Column<string>(maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CellNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(maxLength: 100, nullable: true),
                    Street = table.Column<string>(maxLength: 100, nullable: true),
                    FullAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    Pelak = table.Column<string>(maxLength: 100, nullable: true),
                    Office = table.Column<string>(nullable: true),
                    OfficePhoneNumber = table.Column<string>(nullable: true),
                    EmployeementOnDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Biography = table.Column<string>(nullable: true),
                    IsManagementMembers = table.Column<bool>(nullable: false),
                    HasSpecificDisease = table.Column<bool>(nullable: false),
                    DescriptionDisease = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyPersonel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCompanyPersonel_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCompanyPersonel_OrganizationalPosition_OrganizationalPositionId",
                        column: x => x.OrganizationalPositionId,
                        principalTable: "OrganizationalPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCompanyPersonel_UserCompany_UserCompanyId",
                        column: x => x.UserCompanyId,
                        principalTable: "UserCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizensCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCode = table.Column<string>(nullable: true),
                    CitizenId = table.Column<int>(nullable: false),
                    CardInfoId = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    DiscountGroupId = table.Column<int>(nullable: true),
                    TransactionId = table.Column<long>(nullable: true),
                    RequestByCitizenId = table.Column<int>(nullable: true),
                    DeliverType = table.Column<int>(nullable: false),
                    RequestStatuse = table.Column<int>(nullable: false),
                    DistributeCardOnDate = table.Column<DateTime>(nullable: true),
                    DeliveredOnDate = table.Column<DateTime>(nullable: true),
                    DeliveredDescription = table.Column<string>(nullable: true),
                    DeliveringAddressId = table.Column<int>(nullable: true),
                    DeliveringCenterId = table.Column<string>(nullable: true),
                    CardExpirationDate = table.Column<DateTime>(nullable: true),
                    CardActivationDate = table.Column<DateTime>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    PreCardNumber = table.Column<string>(nullable: true),
                    CardSerial = table.Column<string>(nullable: true),
                    DeliveredByOperationId = table.Column<int>(nullable: true),
                    BarCode = table.Column<string>(nullable: true),
                    IsSetBarCode = table.Column<bool>(nullable: true),
                    CardRequestType = table.Column<int>(nullable: true),
                    AdminDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Citizen",
                columns: table => new
                {
                    CitizenId = table.Column<int>(nullable: false),
                    NationId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    RegisterByServiceId = table.Column<int>(nullable: false),
                    IdentityId = table.Column<string>(nullable: true),
                    NationCode = table.Column<string>(nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    MariageStatus = table.Column<int>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    EducationLevel = table.Column<int>(nullable: true),
                    EducationGroupId = table.Column<int>(nullable: true),
                    EducationStatues = table.Column<int>(nullable: true),
                    EducationField = table.Column<string>(nullable: true),
                    JobGroupId = table.Column<int>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Date_SabtConfirm = table.Column<DateTime>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    SabtStatus = table.Column<int>(nullable: false),
                    FaceAuthentication = table.Column<bool>(nullable: true),
                    FaceAuthenticationOnDate = table.Column<DateTime>(nullable: true),
                    FaceAuthenticationById = table.Column<int>(nullable: true),
                    LastPictureUploadOnDate = table.Column<DateTime>(nullable: true),
                    PersonalPicture_Confirmed = table.Column<int>(nullable: true),
                    PersonalPicture_DisapprovalReason = table.Column<string>(nullable: true),
                    LastUpdateOnDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizen", x => x.CitizenId);
                    table.ForeignKey(
                        name: "FK_Citizen_EducationGroup_EducationGroupId",
                        column: x => x.EducationGroupId,
                        principalTable: "EducationGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citizen_JobGroup_JobGroupId",
                        column: x => x.JobGroupId,
                        principalTable: "JobGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citizen_Nationality_NationId",
                        column: x => x.NationId,
                        principalTable: "Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressType = table.Column<int>(nullable: false),
                    CitizenId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Region = table.Column<int>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    Alley = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Plaque = table.Column<string>(nullable: true),
                    FullAddress = table.Column<string>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LasteUpdateOnDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizenProfile",
                columns: table => new
                {
                    CitizenId = table.Column<int>(nullable: false),
                    PersonnelCode = table.Column<string>(maxLength: 100, nullable: true),
                    ShCode = table.Column<string>(maxLength: 100, nullable: true),
                    ShSerial = table.Column<string>(maxLength: 100, nullable: true),
                    ShDate = table.Column<DateTime>(nullable: true),
                    ShCityId = table.Column<int>(nullable: true),
                    ShCitySection = table.Column<string>(nullable: true),
                    ShNote = table.Column<string>(nullable: true),
                    DateOfEmployeement = table.Column<DateTime>(nullable: true),
                    SoldierState = table.Column<int>(nullable: true),
                    EndOfMilitary = table.Column<DateTime>(nullable: true),
                    Religion = table.Column<int>(nullable: true),
                    VillageOfBirth = table.Column<string>(nullable: true),
                    CityOfBirthId = table.Column<int>(nullable: true),
                    DateOfMarriage = table.Column<DateTime>(nullable: true),
                    BirthCitySection = table.Column<string>(nullable: true),
                    BaseEducation = table.Column<string>(nullable: true),
                    UniversityName = table.Column<string>(nullable: true),
                    AcademicGrade = table.Column<string>(nullable: true),
                    AcademicNote = table.Column<string>(nullable: true),
                    EndOfEducation = table.Column<DateTime>(nullable: true),
                    EducationStatues = table.Column<int>(nullable: true),
                    InsuranceNumber = table.Column<string>(nullable: true),
                    BankCardNumber_Confirmed = table.Column<bool>(nullable: false),
                    BankCardNumber = table.Column<string>(nullable: true),
                    ShabaNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenProfile", x => x.CitizenId);
                    table.ForeignKey(
                        name: "FK_CitizenProfile_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenProfile_City_CityOfBirthId",
                        column: x => x.CityOfBirthId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CitizenProfile_City_ShCityId",
                        column: x => x.ShCityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizenSummaryEducation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    University = table.Column<string>(maxLength: 50, nullable: true),
                    ThesisTitle = table.Column<string>(maxLength: 50, nullable: true),
                    DateOfStart = table.Column<DateTime>(nullable: true),
                    DateOfEnd = table.Column<DateTime>(nullable: true),
                    Average = table.Column<string>(nullable: true),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    UniversityLocation = table.Column<string>(maxLength: 50, nullable: true),
                    EduOrientation = table.Column<string>(maxLength: 50, nullable: true),
                    MajorId = table.Column<int>(nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    TypeUniversity = table.Column<int>(nullable: false),
                    CitizenId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenSummaryEducation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizenSummaryEducation_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenSummaryEducation_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAppServiceAccess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    AccessServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppServiceAccess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    AppServicesId = table.Column<int>(nullable: false),
                    UserTicket = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: true),
                    ReturnUrl = table.Column<string>(nullable: true),
                    ParamName1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamName2 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue2 = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginTickets_Archive",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    AppServicesId = table.Column<int>(nullable: false),
                    UserTicket = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: true),
                    ReturnUrl = table.Column<string>(nullable: true),
                    ParamName1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamName2 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue2 = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginTickets_Archive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_Discount_Center",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(nullable: false),
                    CenterID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_Discount_Center", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_Discount_Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    DiscountGroupIsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_Discount_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_DistributeCard_QueueInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IndexOrder = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: true),
                    QueueInputType = table.Column<int>(nullable: false),
                    DeliveredByOperationId = table.Column<int>(nullable: true),
                    QueueStatues = table.Column<int>(nullable: false),
                    DeliveredDescription = table.Column<string>(nullable: true),
                    DeliveredOnDate = table.Column<DateTime>(nullable: true),
                    CardTypeId = table.Column<int>(nullable: true),
                    IsLock = table.Column<bool>(nullable: false),
                    PostTownType = table.Column<int>(nullable: true),
                    DefaultColor = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_DistributeCard_QueueInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_DistributeCard",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueueInfoId = table.Column<long>(nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false),
                    IsPrinted = table.Column<bool>(nullable: true),
                    QueueByGroupId = table.Column<int>(nullable: true),
                    CitizenCardInfoId = table.Column<int>(nullable: false),
                    PrintCode = table.Column<string>(nullable: true),
                    IsInconsistency = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_DistributeCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_DistributeCard_CitizensCard_CitizenCardInfoId",
                        column: x => x.CitizenCardInfoId,
                        principalTable: "CitizensCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInfo_DistributeCard_CardInfo_DistributeCard_QueueInfo_QueueInfoId",
                        column: x => x.QueueInfoId,
                        principalTable: "CardInfo_DistributeCard_QueueInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_DistributeCard_Queue_Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    QueueInfoId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_DistributeCard_Queue_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_DistributeCard_Queue_Groups_CardInfo_DistributeCard_QueueInfo_QueueInfoId",
                        column: x => x.QueueInfoId,
                        principalTable: "CardInfo_DistributeCard_QueueInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_Export_Citizen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenCardInfoId = table.Column<int>(nullable: false),
                    ExportCardId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_Export_Citizen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_Export_Citizen_CitizensCard_CitizenCardInfoId",
                        column: x => x.CitizenCardInfoId,
                        principalTable: "CitizensCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo",
                columns: table => new
                {
                    CardInfoId = table.Column<string>(nullable: false),
                    CardTypeId = table.Column<int>(nullable: false),
                    BuyCardDescription = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    CardCost = table.Column<int>(nullable: false),
                    DoubleCardCost = table.Column<int>(nullable: false),
                    PostalCostInCity = table.Column<int>(nullable: false),
                    PostalCostOutCity = table.Column<int>(nullable: false),
                    CardIsActive = table.Column<bool>(nullable: false),
                    StartFromDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    AttachmentGroup = table.Column<string>(nullable: true),
                    VATForCardCost = table.Column<decimal>(nullable: false),
                    VATForPost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo", x => x.CardInfoId);
                    table.ForeignKey(
                        name: "FK_CardInfo_CardType_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_Discount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountTitle = table.Column<string>(nullable: true),
                    CardTypeId = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    DiscountPercent = table.Column<int>(nullable: true),
                    PostalPercentInCity = table.Column<int>(nullable: true),
                    PostalPercentOutCity = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PostDeliveryPossibility = table.Column<bool>(nullable: false),
                    CenterDeliveryPossibility = table.Column<bool>(nullable: false),
                    DiscountIsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AttachmentGroup = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_Discount_CardType_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_PermissionsForGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardTypeId = table.Column<int>(nullable: false),
                    CardPermissionType = table.Column<int>(nullable: false),
                    PermissionGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_PermissionsForGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_PermissionsForGroups_CardType_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizenFamily",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(nullable: false),
                    FamilyCitizenId = table.Column<int>(nullable: true),
                    FamilyRelation = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    AcceptRelative = table.Column<bool>(nullable: true),
                    AcceptDate = table.Column<DateTime>(nullable: true),
                    Confirm = table.Column<bool>(nullable: true),
                    ConfirmDate = table.Column<DateTime>(nullable: true),
                    ConfirmerUserId = table.Column<int>(nullable: true),
                    UnderProtection = table.Column<bool>(nullable: true),
                    Heirs = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReasonFordeleting = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizenFamily_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenFamily_Citizen_FamilyCitizenId",
                        column: x => x.FamilyCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizenFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedbackId = table.Column<int>(nullable: false),
                    FeedbackDescription = table.Column<string>(nullable: true),
                    CitizenId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizenFeedback_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenFeedback_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExportedCitizens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(nullable: false),
                    ExportId = table.Column<int>(nullable: false),
                    Verified = table.Column<bool>(nullable: true),
                    VerifyDate = table.Column<DateTime>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportedCitizens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportedCitizens_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsCitizens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    CitizenId = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    AddByUserId = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeletedByUserId = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsCitizens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsCitizens_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportExcelFileDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportExcelFileId = table.Column<int>(nullable: false),
                    CitizenId = table.Column<int>(nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    NationCode = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    IsValidRow = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportExcelFileDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportExcelFileDetails_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manzalat",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<int>(nullable: false),
                    Chk_Maloulin = table.Column<bool>(nullable: true),
                    Chk_Maloulin_JesmiHarekati_NoWheelChair = table.Column<bool>(nullable: true),
                    Typ_Maloulin_JesmiHarekati_NoWheelChair = table.Column<int>(nullable: true),
                    Chk_Maloulin_JesmiHarekati_WheelChair = table.Column<bool>(nullable: true),
                    Typ_Maloulin_JesmiHarekati_WheelChair = table.Column<int>(nullable: true),
                    Chk_Maloulin_Zehni = table.Column<bool>(nullable: true),
                    Typ_Maloulin_Zehni = table.Column<int>(nullable: true),
                    Chk_Maloulin_AsabRavan = table.Column<bool>(nullable: true),
                    Typ_Maloulin_AsabRavan = table.Column<int>(nullable: true),
                    Chk_Maloulin_Binaei = table.Column<bool>(nullable: true),
                    Typ_Maloulin_Binaei = table.Column<int>(nullable: true),
                    Chk_Maloulin_Shenavaei = table.Column<bool>(nullable: true),
                    Typ_Maloulin_Shenavaei = table.Column<int>(nullable: true),
                    Chk_Maloulin_Sayer = table.Column<bool>(nullable: true),
                    Fu_Maloulin = table.Column<string>(nullable: true),
                    Chk_Janbazan = table.Column<bool>(nullable: true),
                    Chk_Janbazan_JesmiHarekati_NoWheelChair = table.Column<bool>(nullable: true),
                    Typ_Janbazan_JesmiHarekati_NoWheelChair = table.Column<int>(nullable: true),
                    Chk_Janbazan_JesmiHarekati_WheelChair = table.Column<bool>(nullable: true),
                    Typ_Janbazan_JesmiHarekati_WheelChair = table.Column<int>(nullable: true),
                    Chk_Janbazan_Zehni = table.Column<bool>(nullable: true),
                    Typ_Janbazan_Zehni = table.Column<int>(nullable: true),
                    Chk_Janbazan_AsabRavan = table.Column<bool>(nullable: true),
                    Typ_Janbazan_AsabRavan = table.Column<int>(nullable: true),
                    Chk_Janbazan_Binaei = table.Column<bool>(nullable: true),
                    Typ_Janbazan_Binaei = table.Column<int>(nullable: true),
                    Chk_Janbazan_Shenavaei = table.Column<bool>(nullable: true),
                    Typ_Janbazan_Shenavaei = table.Column<int>(nullable: true),
                    Chk_Janbazan_Sayer = table.Column<bool>(nullable: true),
                    Fu_Janbazan = table.Column<string>(nullable: true),
                    Chk_ZananSarparast = table.Column<bool>(nullable: true),
                    Typ_ZananSarparast = table.Column<int>(nullable: true),
                    Fu_ZananSarparast = table.Column<string>(nullable: true),
                    Chk_Bazneshasteh = table.Column<bool>(nullable: true),
                    Fu_Bazneshasteh = table.Column<string>(nullable: true),
                    Chk_Salmand = table.Column<bool>(nullable: true),
                    Fu_Salmand = table.Column<string>(nullable: true),
                    BazneshastehDenyDesc = table.Column<string>(nullable: true),
                    BazneshastehResult = table.Column<bool>(nullable: true),
                    JanbazanDenyDesc = table.Column<string>(nullable: true),
                    JanbazanResult = table.Column<bool>(nullable: true),
                    MaloulinDenyDesc = table.Column<string>(nullable: true),
                    MaloulinResult = table.Column<bool>(nullable: true),
                    SalmandDenyDesc = table.Column<string>(nullable: true),
                    SalmandResult = table.Column<bool>(nullable: true),
                    ZananSarparastDenyDesc = table.Column<string>(nullable: true),
                    ZananSarparastResult = table.Column<bool>(nullable: true),
                    CkeckOperationId = table.Column<int>(nullable: true),
                    FormStatuse = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CheckDate = table.Column<DateTime>(nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manzalat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manzalat_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefundImportFileDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportExcelFileId = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    SaleReferenceId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CitizenId = table.Column<int>(nullable: true),
                    RefundCardNumber = table.Column<string>(nullable: true),
                    OtherDescription = table.Column<string>(nullable: true),
                    TotalRefundAmount = table.Column<long>(nullable: false),
                    RefundAmount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundImportFileDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundImportFileDetails_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRefund",
                columns: table => new
                {
                    RefundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(nullable: true),
                    TransactionCode = table.Column<string>(nullable: true),
                    RefundByUserId = table.Column<int>(nullable: true),
                    CitizenId = table.Column<int>(nullable: false),
                    RefundAmount = table.Column<long>(nullable: false),
                    OwnerBankCardNumber = table.Column<string>(nullable: true),
                    RefundCardNumber = table.Column<string>(nullable: true),
                    RefundOnDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OtherDescription = table.Column<string>(nullable: true),
                    RefundIssuccessful = table.Column<bool>(nullable: true),
                    RefundState = table.Column<int>(nullable: true),
                    TransactionRefundImportId = table.Column<int>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    TotalRefundAmount = table.Column<long>(nullable: false),
                    UserIsAcceptRefund = table.Column<bool>(nullable: false),
                    RefundRefCode = table.Column<string>(nullable: true),
                    AdminDescription = table.Column<string>(nullable: true),
                    TransactionXmlInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRefund", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_TransactionRefund_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRefundImport",
                columns: table => new
                {
                    RefundImportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnDate = table.Column<DateTime>(nullable: false),
                    ImportByUserId = table.Column<int>(nullable: true),
                    LetterNumber = table.Column<string>(nullable: true),
                    ImportDescription = table.Column<string>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    AccessByCitizenId = table.Column<int>(nullable: true),
                    CitizenAccess = table.Column<bool>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRefundImport", x => x.RefundImportId);
                    table.ForeignKey(
                        name: "FK_TransactionRefundImport_Citizen_AccessByCitizenId",
                        column: x => x.AccessByCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizensCardBackCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenCardInfoId = table.Column<int>(nullable: false),
                    ReasonBackDescription = table.Column<string>(nullable: true),
                    DeliveringCenterId = table.Column<string>(nullable: true),
                    PreRequestStatuse = table.Column<int>(nullable: false),
                    BackCardByOperationId = table.Column<int>(nullable: true),
                    BackCardOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensCardBackCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizensCardBackCard_CitizensCard_CitizenCardInfoId",
                        column: x => x.CitizenCardInfoId,
                        principalTable: "CitizensCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizensCardCancellation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenCardInfoId = table.Column<int>(nullable: false),
                    CardCancellationByOperationId = table.Column<int>(nullable: true),
                    ReasonCardCancellation = table.Column<string>(nullable: true),
                    CardCancellationOnDate = table.Column<DateTime>(nullable: false),
                    CardCancellationType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensCardCancellation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizensCardCancellation_CitizensCard_CitizenCardInfoId",
                        column: x => x.CitizenCardInfoId,
                        principalTable: "CitizensCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CitizensCardConvertType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenCardInfoId = table.Column<int>(nullable: false),
                    ConvertType = table.Column<int>(nullable: false),
                    ByOperationId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ApprovalOnDate = table.Column<DateTime>(nullable: true),
                    ApprovalByOperationId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensCardConvertType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizensCardConvertType_CitizensCard_CitizenCardInfoId",
                        column: x => x.CitizenCardInfoId,
                        principalTable: "CitizensCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactorDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FactorMasterId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<long>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Discountamount = table.Column<int>(nullable: false),
                    VAT = table.Column<int>(nullable: false),
                    Total = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    QuestionGroupTypeId = table.Column<int>(nullable: true),
                    TagNames = table.Column<string>(nullable: true),
                    ViewCount = table.Column<int>(nullable: false),
                    OrganizationalUnitId = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    CreateOnDate = table.Column<DateTime>(nullable: false),
                    ModifiedOnDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsMainFaq = table.Column<bool>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CitizensQueue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    NationCode = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    AddByUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportExcelFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountRow = table.Column<int>(nullable: false),
                    ImportExcelFileType = table.Column<int>(nullable: false),
                    ExportFileName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExportFilePath = table.Column<string>(nullable: true),
                    ImportByUserId = table.Column<int>(nullable: true),
                    UserCompanyId = table.Column<int>(nullable: true),
                    ReviewByUserId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: true),
                    ReviewOnData = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportExcelFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportExcelFile_UserCompany_UserCompanyId",
                        column: x => x.UserCompanyId,
                        principalTable: "UserCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPersonnel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<bool>(nullable: false),
                    NationCode = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ImportExcelFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPersonnel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPersonnel_ImportExcelFile_ImportExcelFileId",
                        column: x => x.ImportExcelFileId,
                        principalTable: "ImportExcelFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsForCitizen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsForCitizen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnitGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationalUnitId = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnitGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsComment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyId = table.Column<long>(nullable: true),
                    ReplyId1 = table.Column<int>(nullable: true),
                    NewsItemId1 = table.Column<int>(nullable: true),
                    NewsItemId = table.Column<long>(nullable: false),
                    CommentMessage = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: true),
                    PublishByUserId = table.Column<int>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: true),
                    UserIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsComment_NewsComment_ReplyId1",
                        column: x => x.ReplyId1,
                        principalTable: "NewsComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsReads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    UserIP = table.Column<string>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsReads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 2000, nullable: false),
                    Code = table.Column<string>(maxLength: 2000, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Clicks = table.Column<int>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoTags = table.Column<string>(nullable: true),
                    CommentIsActive = table.Column<bool>(nullable: false),
                    AttachmentFileGroup = table.Column<string>(nullable: true),
                    AttachmentImageGroup = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    NewsGroupId = table.Column<int>(nullable: true),
                    Slug = table.Column<string>(maxLength: 200, nullable: true),
                    LanguageName = table.Column<string>(maxLength: 3, nullable: true),
                    PageType = table.Column<int>(nullable: false),
                    IsSpecial = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_NewsGroup_NewsGroupId",
                        column: x => x.NewsGroupId,
                        principalTable: "NewsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnit",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FullAddress = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ThumbUrl = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketSubject",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrganizationalUnitId = table.Column<string>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ThumbUrl = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedOnDate = table.Column<DateTime>(nullable: true),
                    CreateOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSubject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketSubject_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 450, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    OldPassword = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    EmailVerification = table.Column<bool>(nullable: false),
                    MobileNumber = table.Column<string>(nullable: true),
                    MobileNumberVerification = table.Column<bool>(nullable: false),
                    LastLoggedIn = table.Column<DateTime>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: true),
                    SerialNumber = table.Column<string>(maxLength: 450, nullable: true),
                    UserCompanyId = table.Column<int>(nullable: true),
                    PasswordQuestion = table.Column<string>(nullable: true),
                    PasswordAnswer = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    UserAccountState = table.Column<int>(nullable: false),
                    DeactivationDate = table.Column<DateTime>(nullable: true),
                    OrganizationalUnitId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_UserCompany_UserCompanyId",
                        column: x => x.UserCompanyId,
                        principalTable: "UserCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    ServicePicture = table.Column<string>(maxLength: 500, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsLinkService = table.Column<bool>(nullable: false),
                    IsNeedAuthenticate = table.Column<bool>(nullable: false),
                    IsShowInDashbordCitizen = table.Column<bool>(nullable: false),
                    OpenInNewWindow = table.Column<bool>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    ParamName1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamName2 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue1 = table.Column<string>(maxLength: 100, nullable: true),
                    ParamValue2 = table.Column<string>(maxLength: 100, nullable: true),
                    IsMain = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedOnDate = table.Column<DateTime>(nullable: true),
                    CssClass = table.Column<string>(maxLength: 100, nullable: true),
                    Icon = table.Column<string>(maxLength: 100, nullable: true),
                    HaveTerms = table.Column<bool>(nullable: false),
                    Terms = table.Column<string>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppServices_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppServices_AppServices_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AppServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    AttachmentGroup = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Extension = table.Column<string>(nullable: true),
                    AttachedOn = table.Column<DateTime>(nullable: false),
                    DownloadsCount = table.Column<long>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DurationMinute = table.Column<int>(nullable: false),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    ThumnailPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_DistributeCard_Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseNumber = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    OperationId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_DistributeCard_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_DistributeCard_Courses_Users_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardInfo_Export",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExporterByUserId = table.Column<int>(nullable: false),
                    ExportNumber = table.Column<int>(nullable: true),
                    ImporterByUserId = table.Column<int>(nullable: true),
                    DateSend = table.Column<DateTime>(nullable: true),
                    DateReceive = table.Column<DateTime>(nullable: true),
                    ConfirmedData = table.Column<DateTime>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CitizenPictureFilePath = table.Column<string>(nullable: true),
                    CitizenPictureCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInfo_Export", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInfo_Export_Users_ExporterByUserId",
                        column: x => x.ExporterByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInfo_Export_Users_ImporterByUserId",
                        column: x => x.ImporterByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CitizensDead",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    DeadOnDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OperationId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizensDead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CitizensDead_Users_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CitizensDead_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ActionName = table.Column<string>(nullable: true),
                    EventSection = table.Column<int>(nullable: false),
                    EventPriority = table.Column<int>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    StrCode = table.Column<string>(nullable: true),
                    OperationId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserIp = table.Column<string>(nullable: true),
                    JsonValue = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_Users_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExportCitizens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExportNumber = table.Column<int>(nullable: false),
                    ExportById = table.Column<int>(nullable: true),
                    CountRow = table.Column<int>(nullable: false),
                    ExportType = table.Column<int>(nullable: false),
                    ExportFileName = table.Column<string>(nullable: true),
                    HistoryLog = table.Column<string>(nullable: true),
                    SendOnDate = table.Column<DateTime>(nullable: true),
                    ReceiveById = table.Column<int>(nullable: true),
                    ReceiveOnDate = table.Column<DateTime>(nullable: true),
                    ExportedForeignId = table.Column<int>(nullable: true),
                    AcceptCount = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    ConfirmedData = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportCitizens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExportCitizens_Users_ExportById",
                        column: x => x.ExportById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExportCitizens_Users_ReceiveById",
                        column: x => x.ReceiveById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FaqQuestionGroupType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 300, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    OrganizationalUnitId = table.Column<string>(nullable: true),
                    CreateOnDate = table.Column<DateTime>(nullable: false),
                    ModifiedOnDate = table.Column<DateTime>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IndexOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestionGroupType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqQuestionGroupType_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaqQuestionGroupType_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaqQuestionGroupType_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    MainGroupId = table.Column<int>(nullable: true),
                    GroupCategory = table.Column<int>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    AutoAddMembers = table.Column<bool>(nullable: false),
                    ShowToMembers = table.Column<bool>(nullable: false),
                    ShowToAddCitizen = table.Column<bool>(nullable: false),
                    MaxMembers = table.Column<int>(nullable: true),
                    SpecialRules = table.Column<bool>(nullable: false),
                    Law_Gender = table.Column<bool>(nullable: false),
                    Law_AgeFrom = table.Column<int>(nullable: true),
                    Law_AgeTo = table.Column<int>(nullable: true),
                    Law_MariageStatus = table.Column<bool>(nullable: false),
                    Law_EducationLeve = table.Column<bool>(nullable: false),
                    Law_City = table.Column<bool>(nullable: false),
                    Law_JobGroup = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UseForServices = table.Column<bool>(nullable: true),
                    ViewCssClass = table.Column<string>(nullable: true),
                    ViewIcon = table.Column<string>(nullable: true),
                    MunicipalPersonnelGroup = table.Column<bool>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Group_Group_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(maxLength: 50, nullable: true),
                    MenuPath = table.Column<string>(nullable: true),
                    TabOrder = table.Column<int>(nullable: true),
                    IconFile = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    ModifiedOnDate = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    LastModifiedByUserId = table.Column<int>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OpenInNewPage = table.Column<bool>(nullable: false),
                    DisableLink = table.Column<bool>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItem_Users_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    OrganizationId = table.Column<string>(nullable: false),
                    OrganizationName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    ThumbUrl = table.Column<string>(maxLength: 255, nullable: true),
                    UserOwnerId = table.Column<int>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    BanerUrl = table.Column<string>(nullable: true),
                    CardDistributionCenters = table.Column<bool>(nullable: true),
                    SupportCenters = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Organization_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SlideShow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IndexOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    CreatedOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlideShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlideShow_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageText = table.Column<string>(nullable: true),
                    Mobiles = table.Column<string>(nullable: true),
                    GroupListId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    SmsStatus = table.Column<int>(nullable: false),
                    StatusText = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    SendOnDate = table.Column<DateTime>(nullable: false),
                    Date = table.Column<long>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Token20 = table.Column<string>(nullable: true),
                    Token10 = table.Column<string>(nullable: true),
                    Token3 = table.Column<string>(nullable: true),
                    Token2 = table.Column<string>(nullable: true),
                    Token1 = table.Column<string>(nullable: true),
                    TempleteName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<string>(nullable: false),
                    TicketSubjectId = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    NationCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    UserOwnerId = table.Column<int>(nullable: true),
                    TicketStatus = table.Column<int>(nullable: false),
                    TicketType = table.Column<int>(nullable: false),
                    TicketPriority = table.Column<int>(nullable: false),
                    TicketChannel = table.Column<int>(nullable: false),
                    TicketSection = table.Column<int>(nullable: false),
                    IsColsed = table.Column<bool>(nullable: false),
                    ColsedById = table.Column<int>(nullable: true),
                    ColsedOnDate = table.Column<DateTime>(nullable: true),
                    IsArchive = table.Column<bool>(nullable: false),
                    IsSolved = table.Column<bool>(nullable: false),
                    OrganizationalUnitId = table.Column<string>(nullable: true),
                    UserCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Ticket_Users_ColsedById",
                        column: x => x.ColsedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_OrganizationalUnit_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalTable: "OrganizationalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketSubject_TicketSubjectId",
                        column: x => x.TicketSubjectId,
                        principalTable: "TicketSubject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_UserCompany_UserCompanyId",
                        column: x => x.UserCompanyId,
                        principalTable: "UserCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    FileName = table.Column<string>(maxLength: 200, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Extension = table.Column<string>(maxLength: 50, nullable: true),
                    AttachedOnDate = table.Column<DateTime>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 1000, nullable: true),
                    ThumnailPath = table.Column<string>(maxLength: 1000, nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    DocumentGroupId = table.Column<int>(nullable: false),
                    DocumentStatus = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDocument_UserDocumentGroup_DocumentGroupId",
                        column: x => x.DocumentGroupId,
                        principalTable: "UserDocumentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDocument_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionGroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissionGroup_PermissionGroup_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissionGroup_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessTokenHash = table.Column<string>(nullable: true),
                    AccessTokenExpiresDateTime = table.Column<DateTime>(nullable: false),
                    RefreshTokenIdHash = table.Column<string>(maxLength: 450, nullable: false),
                    RefreshTokenIdHashSource = table.Column<string>(maxLength: 450, nullable: true),
                    RefreshTokenExpiresDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerminalId = table.Column<string>(maxLength: 64, nullable: true),
                    PaymentType = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(maxLength: 64, nullable: true),
                    TransactionBankReferenceId = table.Column<string>(maxLength: 64, nullable: true),
                    AmountTransaction = table.Column<long>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    PaymentDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    TransactionOnDate = table.Column<DateTime>(nullable: false),
                    TransactionState = table.Column<int>(nullable: false),
                    TransactionBank = table.Column<int>(nullable: false),
                    TransactionFor = table.Column<int>(nullable: false),
                    AcceptationTransactionOnDate = table.Column<DateTime>(nullable: true),
                    FileUrl = table.Column<string>(maxLength: 1000, nullable: true),
                    OperationDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    TransactionById = table.Column<int>(nullable: true),
                    ReviewById = table.Column<int>(nullable: true),
                    BankName = table.Column<string>(maxLength: 50, nullable: true),
                    BankAccountNumber = table.Column<string>(maxLength: 16, nullable: true),
                    BranchName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_UserTransaction_Users_ReviewById",
                        column: x => x.ReviewById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTransaction_Users_TransactionById",
                        column: x => x.TransactionById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebPage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(maxLength: 200, nullable: true),
                    Clicks = table.Column<int>(nullable: false),
                    SeoTags = table.Column<string>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    AttachmentFileGroup = table.Column<string>(nullable: true),
                    AttachmentImageGroup = table.Column<string>(nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebPage_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebPage_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebUserAccessRangeIp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Start = table.Column<string>(nullable: true),
                    End = table.Column<string>(nullable: true),
                    ExceptionIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebUserAccessRangeIp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebUserAccessRangeIp_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebUserPermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebUserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebUserPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebUserPermission_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<string>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false),
                    PeriodOfMinutes = table.Column<int>(nullable: false),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    UserOwnerId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsSubtractFromContract = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketActivity_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketActivity_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<string>(nullable: true),
                    UserOwnerId = table.Column<int>(nullable: true),
                    OnDate = table.Column<DateTime>(nullable: false),
                    CommentText = table.Column<string>(maxLength: 1000, nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketComments_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketComments_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessage",
                columns: table => new
                {
                    TicketMessageId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    OnDate = table.Column<DateTime>(nullable: false),
                    TicketId = table.Column<string>(nullable: true),
                    UserOwnerId = table.Column<int>(nullable: true),
                    AttachmentGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessage", x => x.TicketMessageId);
                    table.ForeignKey(
                        name: "FK_TicketMessage_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMessage_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactorMaster",
                columns: table => new
                {
                    FactorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerName = table.Column<string>(maxLength: 150, nullable: true),
                    SellerEconomicalNO = table.Column<string>(maxLength: 50, nullable: true),
                    SellerRegNO = table.Column<string>(maxLength: 50, nullable: true),
                    SellerAddress = table.Column<string>(maxLength: 500, nullable: true),
                    SellerZipCode = table.Column<string>(maxLength: 20, nullable: true),
                    SellerCity = table.Column<string>(maxLength: 50, nullable: true),
                    SellerPhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    SellerFaxNumber = table.Column<string>(maxLength: 50, nullable: true),
                    SellerSheba = table.Column<string>(nullable: true),
                    SellerCardNo = table.Column<string>(maxLength: 16, nullable: true),
                    SellerBankAccountNumber = table.Column<string>(maxLength: 16, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 150, nullable: true),
                    BuyerEconomicalNO = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerRegNO = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerAddress = table.Column<string>(maxLength: 500, nullable: true),
                    BuyerZipCode = table.Column<string>(maxLength: 20, nullable: true),
                    BuyerCity = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerPhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerFaxNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CrateOnDate = table.Column<DateTime>(nullable: false),
                    FactorNumber = table.Column<string>(maxLength: 100, nullable: true),
                    FactorDate = table.Column<DateTime>(nullable: true),
                    BarCode = table.Column<string>(nullable: true),
                    TransactionById = table.Column<long>(nullable: true),
                    CreatedById = table.Column<int>(nullable: true),
                    IsConfirm = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorMaster", x => x.FactorId);
                    table.ForeignKey(
                        name: "FK_FactorMaster_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactorMaster_UserTransaction_TransactionById",
                        column: x => x.TransactionById,
                        principalTable: "UserTransaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CitizenId",
                table: "Address",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityId",
                table: "Address",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppServices_CreatedById",
                table: "AppServices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppServices_ParentId",
                table: "AppServices",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UserId",
                table: "Attachment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_CardTypeId",
                table: "CardInfo",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_OperationId",
                table: "CardInfo",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_CardTypeId",
                table: "CardInfo_Discount",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_OperationId",
                table: "CardInfo_Discount",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_Center_CenterID",
                table: "CardInfo_Discount_Center",
                column: "CenterID");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_Center_DiscountId",
                table: "CardInfo_Discount_Center",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_Group_DiscountId",
                table: "CardInfo_Discount_Group",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Discount_Group_GroupId",
                table: "CardInfo_Discount_Group",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_CitizenCardInfoId",
                table: "CardInfo_DistributeCard",
                column: "CitizenCardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_QueueByGroupId",
                table: "CardInfo_DistributeCard",
                column: "QueueByGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_QueueInfoId",
                table: "CardInfo_DistributeCard",
                column: "QueueInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_Courses_OperationId",
                table: "CardInfo_DistributeCard_Courses",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_Queue_Groups_GroupId",
                table: "CardInfo_DistributeCard_Queue_Groups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_Queue_Groups_QueueInfoId",
                table: "CardInfo_DistributeCard_Queue_Groups",
                column: "QueueInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_QueueInfo_CourseId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_QueueInfo_DeliveredByOperationId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "DeliveredByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_DistributeCard_QueueInfo_OperationId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Export_ExporterByUserId",
                table: "CardInfo_Export",
                column: "ExporterByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Export_ImporterByUserId",
                table: "CardInfo_Export",
                column: "ImporterByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Export_Citizen_CitizenCardInfoId",
                table: "CardInfo_Export_Citizen",
                column: "CitizenCardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_Export_Citizen_ExportCardId",
                table: "CardInfo_Export_Citizen",
                column: "ExportCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_PermissionsForGroups_CardTypeId",
                table: "CardInfo_PermissionsForGroups",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInfo_PermissionsForGroups_PermissionGroupId",
                table: "CardInfo_PermissionsForGroups",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_EducationGroupId",
                table: "Citizen",
                column: "EducationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_FaceAuthenticationById",
                table: "Citizen",
                column: "FaceAuthenticationById");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_JobGroupId",
                table: "Citizen",
                column: "JobGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_NationId",
                table: "Citizen",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_RegisterByServiceId",
                table: "Citizen",
                column: "RegisterByServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFamily_CitizenId",
                table: "CitizenFamily",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFamily_ConfirmerUserId",
                table: "CitizenFamily",
                column: "ConfirmerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFamily_FamilyCitizenId",
                table: "CitizenFamily",
                column: "FamilyCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFeedback_CitizenId",
                table: "CitizenFeedback",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFeedback_FeedbackId",
                table: "CitizenFeedback",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenFeedback_OperationId",
                table: "CitizenFeedback",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenProfile_CityOfBirthId",
                table: "CitizenProfile",
                column: "CityOfBirthId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenProfile_ShCityId",
                table: "CitizenProfile",
                column: "ShCityId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_CardInfoId",
                table: "CitizensCard",
                column: "CardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_CitizenId",
                table: "CitizensCard",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_DeliveredByOperationId",
                table: "CitizensCard",
                column: "DeliveredByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_DeliveringAddressId",
                table: "CitizensCard",
                column: "DeliveringAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_DeliveringCenterId",
                table: "CitizensCard",
                column: "DeliveringCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_DiscountGroupId",
                table: "CitizensCard",
                column: "DiscountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_RequestByCitizenId",
                table: "CitizensCard",
                column: "RequestByCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCard_TransactionId",
                table: "CitizensCard",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardBackCard_BackCardByOperationId",
                table: "CitizensCardBackCard",
                column: "BackCardByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardBackCard_CitizenCardInfoId",
                table: "CitizensCardBackCard",
                column: "CitizenCardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardBackCard_DeliveringCenterId",
                table: "CitizensCardBackCard",
                column: "DeliveringCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardCancellation_CardCancellationByOperationId",
                table: "CitizensCardCancellation",
                column: "CardCancellationByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardCancellation_CitizenCardInfoId",
                table: "CitizensCardCancellation",
                column: "CitizenCardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardConvertType_ApprovalByOperationId",
                table: "CitizensCardConvertType",
                column: "ApprovalByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardConvertType_ByOperationId",
                table: "CitizensCardConvertType",
                column: "ByOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensCardConvertType_CitizenCardInfoId",
                table: "CitizensCardConvertType",
                column: "CitizenCardInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensDead_OperationId",
                table: "CitizensDead",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensDead_UserId",
                table: "CitizensDead",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensQueue_AddByUserId",
                table: "CitizensQueue",
                column: "AddByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizensQueue_GroupId",
                table: "CitizensQueue",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenSummaryEducation_CitizenId",
                table: "CitizenSummaryEducation",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenSummaryEducation_MajorId",
                table: "CitizenSummaryEducation",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ParentId",
                table: "City",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPersonnel_ImportExcelFileId",
                table: "CompanyPersonnel",
                column: "ImportExcelFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OperationId",
                table: "Event",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_UserId",
                table: "Event",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportCitizens_ExportById",
                table: "ExportCitizens",
                column: "ExportById");

            migrationBuilder.CreateIndex(
                name: "IX_ExportCitizens_ReceiveById",
                table: "ExportCitizens",
                column: "ReceiveById");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedCitizens_CitizenId",
                table: "ExportedCitizens",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportedCitizens_ExportId",
                table: "ExportedCitizens",
                column: "ExportId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorDetail_FactorMasterId",
                table: "FactorDetail",
                column: "FactorMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorMaster_CreatedById",
                table: "FactorMaster",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FactorMaster_TransactionById",
                table: "FactorMaster",
                column: "TransactionById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestion_CreatedById",
                table: "FaqQuestion",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestion_ModifiedById",
                table: "FaqQuestion",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestion_OrganizationalUnitId",
                table: "FaqQuestion",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestion_QuestionGroupTypeId",
                table: "FaqQuestion",
                column: "QuestionGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestionGroupType_CreatedById",
                table: "FaqQuestionGroupType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestionGroupType_ModifiedById",
                table: "FaqQuestionGroupType",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestionGroupType_OrganizationalUnitId",
                table: "FaqQuestionGroupType",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_CreatedByUserId",
                table: "Group",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ParentId",
                table: "Group",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsCitizens_AddByUserId",
                table: "GroupsCitizens",
                column: "AddByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsCitizens_CitizenId",
                table: "GroupsCitizens",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsCitizens_DeletedByUserId",
                table: "GroupsCitizens",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsCitizens_GroupId",
                table: "GroupsCitizens",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFile_GroupId",
                table: "ImportExcelFile",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFile_ImportByUserId",
                table: "ImportExcelFile",
                column: "ImportByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFile_ReviewByUserId",
                table: "ImportExcelFile",
                column: "ReviewByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFile_UserCompanyId",
                table: "ImportExcelFile",
                column: "UserCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFileDetails_CitizenId",
                table: "ImportExcelFileDetails",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportExcelFileDetails_ImportExcelFileId",
                table: "ImportExcelFileDetails",
                column: "ImportExcelFileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitle_ParentId",
                table: "JobTitle",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Manzalat_CitizenId",
                table: "Manzalat",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Manzalat_CkeckOperationId",
                table: "Manzalat",
                column: "CkeckOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_CreatedByUserId",
                table: "MenuItem",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_LastModifiedByUserId",
                table: "MenuItem",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_ParentId",
                table: "MenuItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedByUserId",
                table: "News",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsGroupId",
                table: "News",
                column: "NewsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_News_OrganizationId",
                table: "News",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComment_NewsItemId1",
                table: "NewsComment",
                column: "NewsItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComment_PublishByUserId",
                table: "NewsComment",
                column: "PublishByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsComment_ReplyId1",
                table: "NewsComment",
                column: "ReplyId1");

            migrationBuilder.CreateIndex(
                name: "IX_NewsForCitizen_GroupId",
                table: "NewsForCitizen",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsForCitizen_NewsId",
                table: "NewsForCitizen",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsForCitizen_UserId",
                table: "NewsForCitizen",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsReads_NewsId",
                table: "NewsReads",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsReads_UserId",
                table: "NewsReads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_UserOwnerId",
                table: "Organization",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnit_OrganizationId",
                table: "OrganizationalUnit",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnitGroups_GroupId",
                table: "OrganizationalUnitGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnitGroups_OrganizationalUnitId",
                table: "OrganizationalUnitGroups",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundImportFileDetails_CitizenId",
                table: "RefundImportFileDetails",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundImportFileDetails_ImportExcelFileId",
                table: "RefundImportFileDetails",
                column: "ImportExcelFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SlideShow_CreatedById",
                table: "SlideShow",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmsInfo_UserId",
                table: "SmsInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ColsedById",
                table: "Ticket",
                column: "ColsedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OrganizationalUnitId",
                table: "Ticket",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketSubjectId",
                table: "Ticket",
                column: "TicketSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserCompanyId",
                table: "Ticket",
                column: "UserCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserOwnerId",
                table: "Ticket",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivity_TicketId",
                table: "TicketActivity",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivity_UserOwnerId",
                table: "TicketActivity",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_UserOwnerId",
                table: "TicketComments",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessage_TicketId",
                table: "TicketMessage",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessage_UserOwnerId",
                table: "TicketMessage",
                column: "UserOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubject_OrganizationalUnitId",
                table: "TicketSubject",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRefund_CitizenId",
                table: "TransactionRefund",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRefund_RefundByUserId",
                table: "TransactionRefund",
                column: "RefundByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRefund_TransactionRefundImportId",
                table: "TransactionRefund",
                column: "TransactionRefundImportId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRefundImport_AccessByCitizenId",
                table: "TransactionRefundImport",
                column: "AccessByCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRefundImport_ImportByUserId",
                table: "TransactionRefundImport",
                column: "ImportByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppServiceAccess_AccessServiceId",
                table: "UserAppServiceAccess",
                column: "AccessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppServiceAccess_UserId",
                table: "UserAppServiceAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompany_CityId",
                table: "UserCompany",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyActivities_ActivityId",
                table: "UserCompanyActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyActivities_UserCompanyId",
                table: "UserCompanyActivities",
                column: "UserCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyPersonel_CityId",
                table: "UserCompanyPersonel",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyPersonel_OrganizationalPositionId",
                table: "UserCompanyPersonel",
                column: "OrganizationalPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyPersonel_UserCompanyId",
                table: "UserCompanyPersonel",
                column: "UserCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocument_DocumentGroupId",
                table: "UserDocument",
                column: "DocumentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocument_OwnerId",
                table: "UserDocument",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_AppServicesId",
                table: "UserLoginTickets",
                column: "AppServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_CreatedByUserId",
                table: "UserLoginTickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_UserId",
                table: "UserLoginTickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_Archive_AppServicesId",
                table: "UserLoginTickets_Archive",
                column: "AppServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_Archive_CreatedByUserId",
                table: "UserLoginTickets_Archive",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginTickets_Archive_UserId",
                table: "UserLoginTickets_Archive",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionGroupId",
                table: "UserPermission",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionId",
                table: "UserPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionGroup_PermissionGroupId",
                table: "UserPermissionGroup",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionGroup_UserId",
                table: "UserPermissionGroup",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationalUnitId",
                table: "Users",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCompanyId",
                table: "Users",
                column: "UserCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransaction_ReviewById",
                table: "UserTransaction",
                column: "ReviewById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransaction_TransactionById",
                table: "UserTransaction",
                column: "TransactionById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransaction_TransactionId",
                table: "UserTransaction",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebPage_CreatedById",
                table: "WebPage",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WebPage_ModifiedById",
                table: "WebPage",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WebUserAccessRangeIp_UserId",
                table: "WebUserAccessRangeIp",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebUserPermission_PermissionId",
                table: "WebUserPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_WebUserPermission_UserId",
                table: "WebUserPermission",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_Citizen_CitizenId",
                table: "CitizensCard",
                column: "CitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_Citizen_RequestByCitizenId",
                table: "CitizensCard",
                column: "RequestByCitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_Users_DeliveredByOperationId",
                table: "CitizensCard",
                column: "DeliveredByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_OrganizationalUnit_DeliveringCenterId",
                table: "CitizensCard",
                column: "DeliveringCenterId",
                principalTable: "OrganizationalUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_CardInfo_CardInfoId",
                table: "CitizensCard",
                column: "CardInfoId",
                principalTable: "CardInfo",
                principalColumn: "CardInfoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_Address_DeliveringAddressId",
                table: "CitizensCard",
                column: "DeliveringAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_CardInfo_Discount_Group_DiscountGroupId",
                table: "CitizensCard",
                column: "DiscountGroupId",
                principalTable: "CardInfo_Discount_Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCard_UserTransaction_TransactionId",
                table: "CitizensCard",
                column: "TransactionId",
                principalTable: "UserTransaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizen_Users_CitizenId",
                table: "Citizen",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizen_Users_FaceAuthenticationById",
                table: "Citizen",
                column: "FaceAuthenticationById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizen_AppServices_RegisterByServiceId",
                table: "Citizen",
                column: "RegisterByServiceId",
                principalTable: "AppServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAppServiceAccess_Users_UserId",
                table: "UserAppServiceAccess",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAppServiceAccess_AppServices_AccessServiceId",
                table: "UserAppServiceAccess",
                column: "AccessServiceId",
                principalTable: "AppServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_Users_CreatedByUserId",
                table: "UserLoginTickets",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_Users_UserId",
                table: "UserLoginTickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_AppServices_AppServicesId",
                table: "UserLoginTickets",
                column: "AppServicesId",
                principalTable: "AppServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_Archive_Users_CreatedByUserId",
                table: "UserLoginTickets_Archive",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_Archive_Users_UserId",
                table: "UserLoginTickets_Archive",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginTickets_Archive_AppServices_AppServicesId",
                table: "UserLoginTickets_Archive",
                column: "AppServicesId",
                principalTable: "AppServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Center_OrganizationalUnit_CenterID",
                table: "CardInfo_Discount_Center",
                column: "CenterID",
                principalTable: "OrganizationalUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Center_CardInfo_Discount_DiscountId",
                table: "CardInfo_Discount_Center",
                column: "DiscountId",
                principalTable: "CardInfo_Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Group_CardInfo_Discount_DiscountId",
                table: "CardInfo_Discount_Group",
                column: "DiscountId",
                principalTable: "CardInfo_Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Group_Group_GroupId",
                table: "CardInfo_Discount_Group",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_DistributeCard_QueueInfo_Users_DeliveredByOperationId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "DeliveredByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_DistributeCard_QueueInfo_Users_OperationId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "OperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_DistributeCard_QueueInfo_CardInfo_DistributeCard_Courses_CourseId",
                table: "CardInfo_DistributeCard_QueueInfo",
                column: "CourseId",
                principalTable: "CardInfo_DistributeCard_Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_DistributeCard_Group_QueueByGroupId",
                table: "CardInfo_DistributeCard",
                column: "QueueByGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_DistributeCard_Queue_Groups_Group_GroupId",
                table: "CardInfo_DistributeCard_Queue_Groups",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Export_Citizen_CardInfo_Export_ExportCardId",
                table: "CardInfo_Export_Citizen",
                column: "ExportCardId",
                principalTable: "CardInfo_Export",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Users_OperationId",
                table: "CardInfo",
                column: "OperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_Discount_Users_OperationId",
                table: "CardInfo_Discount",
                column: "OperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardInfo_PermissionsForGroups_Group_PermissionGroupId",
                table: "CardInfo_PermissionsForGroups",
                column: "PermissionGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizenFamily_Users_ConfirmerUserId",
                table: "CitizenFamily",
                column: "ConfirmerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizenFeedback_Users_OperationId",
                table: "CitizenFeedback",
                column: "OperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExportedCitizens_ExportCitizens_ExportId",
                table: "ExportedCitizens",
                column: "ExportId",
                principalTable: "ExportCitizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsCitizens_Users_AddByUserId",
                table: "GroupsCitizens",
                column: "AddByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsCitizens_Users_DeletedByUserId",
                table: "GroupsCitizens",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsCitizens_Group_GroupId",
                table: "GroupsCitizens",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportExcelFileDetails_ImportExcelFile_ImportExcelFileId",
                table: "ImportExcelFileDetails",
                column: "ImportExcelFileId",
                principalTable: "ImportExcelFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manzalat_Users_CkeckOperationId",
                table: "Manzalat",
                column: "CkeckOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundImportFileDetails_ImportExcelFile_ImportExcelFileId",
                table: "RefundImportFileDetails",
                column: "ImportExcelFileId",
                principalTable: "ImportExcelFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionRefund_Users_RefundByUserId",
                table: "TransactionRefund",
                column: "RefundByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionRefund_TransactionRefundImport_TransactionRefundImportId",
                table: "TransactionRefund",
                column: "TransactionRefundImportId",
                principalTable: "TransactionRefundImport",
                principalColumn: "RefundImportId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionRefundImport_Users_ImportByUserId",
                table: "TransactionRefundImport",
                column: "ImportByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCardBackCard_Users_BackCardByOperationId",
                table: "CitizensCardBackCard",
                column: "BackCardByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCardBackCard_OrganizationalUnit_DeliveringCenterId",
                table: "CitizensCardBackCard",
                column: "DeliveringCenterId",
                principalTable: "OrganizationalUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCardCancellation_Users_CardCancellationByOperationId",
                table: "CitizensCardCancellation",
                column: "CardCancellationByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCardConvertType_Users_ApprovalByOperationId",
                table: "CitizensCardConvertType",
                column: "ApprovalByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensCardConvertType_Users_ByOperationId",
                table: "CitizensCardConvertType",
                column: "ByOperationId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorDetail_FactorMaster_FactorMasterId",
                table: "FactorDetail",
                column: "FactorMasterId",
                principalTable: "FactorMaster",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FaqQuestion_Users_CreatedById",
                table: "FaqQuestion",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FaqQuestion_Users_ModifiedById",
                table: "FaqQuestion",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FaqQuestion_OrganizationalUnit_OrganizationalUnitId",
                table: "FaqQuestion",
                column: "OrganizationalUnitId",
                principalTable: "OrganizationalUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FaqQuestion_FaqQuestionGroupType_QuestionGroupTypeId",
                table: "FaqQuestion",
                column: "QuestionGroupTypeId",
                principalTable: "FaqQuestionGroupType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensQueue_Users_AddByUserId",
                table: "CitizensQueue",
                column: "AddByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CitizensQueue_Group_GroupId",
                table: "CitizensQueue",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportExcelFile_Users_ImportByUserId",
                table: "ImportExcelFile",
                column: "ImportByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportExcelFile_Users_ReviewByUserId",
                table: "ImportExcelFile",
                column: "ReviewByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportExcelFile_Group_GroupId",
                table: "ImportExcelFile",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsForCitizen_Users_UserId",
                table: "NewsForCitizen",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsForCitizen_Group_GroupId",
                table: "NewsForCitizen",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsForCitizen_News_NewsId",
                table: "NewsForCitizen",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationalUnitGroups_OrganizationalUnit_OrganizationalUnitId",
                table: "OrganizationalUnitGroups",
                column: "OrganizationalUnitId",
                principalTable: "OrganizationalUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationalUnitGroups_Group_GroupId",
                table: "OrganizationalUnitGroups",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComment_Users_PublishByUserId",
                table: "NewsComment",
                column: "PublishByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComment_News_NewsItemId1",
                table: "NewsComment",
                column: "NewsItemId1",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsReads_Users_UserId",
                table: "NewsReads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsReads_News_NewsId",
                table: "NewsReads",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Users_CreatedByUserId",
                table: "News",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Organization_OrganizationId",
                table: "News",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationalUnit_Organization_OrganizationId",
                table: "OrganizationalUnit",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompany_City_CityId",
                table: "UserCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Users_UserOwnerId",
                table: "Organization");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "BaseData");

            migrationBuilder.DropTable(
                name: "CardInfo_Discount_Center");

            migrationBuilder.DropTable(
                name: "CardInfo_DistributeCard");

            migrationBuilder.DropTable(
                name: "CardInfo_DistributeCard_Queue_Groups");

            migrationBuilder.DropTable(
                name: "CardInfo_Export_Citizen");

            migrationBuilder.DropTable(
                name: "CardInfo_PermissionsForGroups");

            migrationBuilder.DropTable(
                name: "CitizenFamily");

            migrationBuilder.DropTable(
                name: "CitizenFeedback");

            migrationBuilder.DropTable(
                name: "CitizenProfile");

            migrationBuilder.DropTable(
                name: "CitizensCardBackCard");

            migrationBuilder.DropTable(
                name: "CitizensCardCancellation");

            migrationBuilder.DropTable(
                name: "CitizensCardConvertType");

            migrationBuilder.DropTable(
                name: "CitizensDead");

            migrationBuilder.DropTable(
                name: "CitizensQueue");

            migrationBuilder.DropTable(
                name: "CitizenSummaryEducation");

            migrationBuilder.DropTable(
                name: "CompanyPersonnel");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "ExportedCitizens");

            migrationBuilder.DropTable(
                name: "FactorDetail");

            migrationBuilder.DropTable(
                name: "FaqQuestion");

            migrationBuilder.DropTable(
                name: "GroupsCitizens");

            migrationBuilder.DropTable(
                name: "ImportExcelFileDetails");

            migrationBuilder.DropTable(
                name: "JobTitle");

            migrationBuilder.DropTable(
                name: "Manzalat");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "NewsComment");

            migrationBuilder.DropTable(
                name: "NewsForCitizen");

            migrationBuilder.DropTable(
                name: "NewsReads");

            migrationBuilder.DropTable(
                name: "OrganizationalUnitGroups");

            migrationBuilder.DropTable(
                name: "QueueCheckingCitizensDead");

            migrationBuilder.DropTable(
                name: "RefundImportFileDetails");

            migrationBuilder.DropTable(
                name: "SiteOption");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "SlideShow");

            migrationBuilder.DropTable(
                name: "SmsInfo");

            migrationBuilder.DropTable(
                name: "TicketActivity");

            migrationBuilder.DropTable(
                name: "TicketComments");

            migrationBuilder.DropTable(
                name: "TicketMessage");

            migrationBuilder.DropTable(
                name: "TransactionRefund");

            migrationBuilder.DropTable(
                name: "UserAppServiceAccess");

            migrationBuilder.DropTable(
                name: "UserCompanyActivities");

            migrationBuilder.DropTable(
                name: "UserCompanyPersonel");

            migrationBuilder.DropTable(
                name: "UserDocument");

            migrationBuilder.DropTable(
                name: "UserLoginTickets");

            migrationBuilder.DropTable(
                name: "UserLoginTickets_Archive");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "UserPermissionGroup");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WebPage");

            migrationBuilder.DropTable(
                name: "WebUserAccessRangeIp");

            migrationBuilder.DropTable(
                name: "WebUserPermission");

            migrationBuilder.DropTable(
                name: "CardInfo_DistributeCard_QueueInfo");

            migrationBuilder.DropTable(
                name: "CardInfo_Export");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "CitizensCard");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "ExportCitizens");

            migrationBuilder.DropTable(
                name: "FactorMaster");

            migrationBuilder.DropTable(
                name: "FaqQuestionGroupType");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "ImportExcelFile");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TransactionRefundImport");

            migrationBuilder.DropTable(
                name: "UserCompanyFieldOfActivity");

            migrationBuilder.DropTable(
                name: "OrganizationalPosition");

            migrationBuilder.DropTable(
                name: "UserDocumentGroup");

            migrationBuilder.DropTable(
                name: "PermissionGroup");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "CardInfo_DistributeCard_Courses");

            migrationBuilder.DropTable(
                name: "CardInfo");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "CardInfo_Discount_Group");

            migrationBuilder.DropTable(
                name: "UserTransaction");

            migrationBuilder.DropTable(
                name: "NewsGroup");

            migrationBuilder.DropTable(
                name: "TicketSubject");

            migrationBuilder.DropTable(
                name: "Citizen");

            migrationBuilder.DropTable(
                name: "CardInfo_Discount");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "EducationGroup");

            migrationBuilder.DropTable(
                name: "JobGroup");

            migrationBuilder.DropTable(
                name: "Nationality");

            migrationBuilder.DropTable(
                name: "AppServices");

            migrationBuilder.DropTable(
                name: "CardType");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "OrganizationalUnit");

            migrationBuilder.DropTable(
                name: "UserCompany");

            migrationBuilder.DropTable(
                name: "Organization");
        }
    }
}
