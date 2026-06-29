using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
 
using cle.Services.UserCompanyServices;
using cle.Services;
using cle.Services.Faq;
using cle.Services.BaseEntity; 
 
using Nikan.Services.BaseEntity;
 
using Nikan.Services.Citizens;
using cle.Services.Citizens;
using Nikan.Services.SlidShow;
using Nikan.Services.CitizenCards;
using cle.Services.CitizensGroups;
 
using Nikan.Services.Permissions;
using Nikan.Services.ImportFile;
using Nikan.Services.UserDocuments;
using Nikan.Services.ExportCitizen;
using Nikan.Services.Events;
using Nikan.Services.Refund;
using Nikan.Services.RateLimiter;

namespace Nikan.IoCConfig
{
    public static class ConfigureServicesExtensions
    {
        public static void AddCustomAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
        }

        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:9600") //Note:  The URL must be specified without a trailing slash (/).
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
            });
        }

        public static void AddCustomJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            // Only needed for custom roles.
            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomRoles.Admin, policy => policy.RequireRole(CustomRoles.Admin));
                options.AddPolicy(CustomRoles.User, policy => policy.RequireRole(CustomRoles.User));
                options.AddPolicy(CustomRoles.Editor, policy => policy.RequireRole(CustomRoles.Editor));
                options.AddPolicy(CustomRoles.Citizen, policy => policy.RequireRole(CustomRoles.Citizen));
                options.AddPolicy(CustomRoles.Card, policy => policy.RequireRole(CustomRoles.Card));
                options.AddPolicy(CustomRoles.Company, policy => policy.RequireRole(CustomRoles.Company));
                options.AddPolicy(CustomRoles.WebApiUser, policy => policy.RequireRole(CustomRoles.WebApiUser));





            });

            // Needed for jwt auth.
            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["BearerTokens:Issuer"], // site that makes the token
                        ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
                        ValidAudience = configuration["BearerTokens:Audience"], // site that consumes the token
                        ValidateAudience = false, // TODO: change this to avoid forwarding attacks
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                        ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError("Authentication failed.", context.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                            return tokenValidatorService.ValidateAsync(context);
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                                .Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data")),
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    });
            });
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAntiForgeryCookieService, AntiForgeryCookieService>();
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddScoped<IDbInitializerService, DbInitializerService>();
            services.AddScoped<ITokenStoreService, TokenStoreService>();
            services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            services.AddScoped<ITokenFactoryService, TokenFactoryService>();
            services.AddScoped<IUserLoginTicketsService, UserLoginTicketsService>();





            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IOrganizationalUnitService, OrganizationalUnitService>();
            services.AddScoped<ITicketService, TicketService>();


            services.AddScoped<IFaqQuestionService, FaqQuestionService>();
            services.AddScoped<IFaqQuestionGroupTypeService, FaqQuestionGroupTypeService>();


            services.AddScoped<ISiteSettingService, SiteSettingService>();

            services.AddScoped<IAttachmentService, AttachmentService>();

            services.AddScoped<IOrganizationalPositionService, OrganizationalPositionService>();

            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IWebPageService, WebPageService>(); 
            services.AddScoped<INewsGroupService, NewsGroupService>(); 
            services.AddScoped<ICityService, CityService>(); 
            services.AddScoped<IUserCompanyService, UserCompanyService>(); 
            services.AddScoped<IBaseDataService, BaseDataService>(); 
            services.AddScoped<ITicketSubjecteService, TicketSubjecteService>();
            services.AddScoped<IMenuItemService, MenuItemService>(); 
            services.AddScoped<ISmsInfoService, SmsInfoService>();  
            services.AddScoped<ITransactionService, TransactionService>(); 
            services.AddScoped<ICitizenFeedbackService,  CitizenFeedbackService>(); 
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<ICitizenFamiliesService,  CitizenFamiliesService>();
            services.AddScoped<ICitizenFeedbackService,  CitizenFeedbackService>();
            services.AddScoped<ICitizenService,  CitizenService>();
            services.AddScoped<IManzalatService, ManzalatService>();
            services.AddScoped<ICitizenSummaryEducationServices, CitizenSummaryEducationServices>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<ISlideShowService, SlideShowService>();
            services.AddScoped<IEducationGroupService, EducationGroupService>(); 
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IPermissionGroupService, PermissionGroupService>();
            services.AddScoped<IPermissionService, PermissionService>(); 
            services.AddScoped<IImportExcelFileService, ImportExcelFileService >();
            services.AddScoped<IUserDocumentGroupService,  UserDocumentGroupService>();
            services.AddScoped<IUserDocumentService, UserDocumentService>();
            services.AddScoped<IExportCitizenService, ExportCitizenService>();
            services.AddScoped<IEventService,  EventService>();
            services.AddScoped<ICitizenCardService, CitizenCardService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<ICardInfoExportService, CardInfoExportService>();

            services.AddScoped<IDistributeCardService, DistributeCardService>();
            services.AddScoped<IDiscountCardService, DiscountCardService>();
            services.AddScoped<IRequestFreeCardService,  RequestFreeCardService>();
            services.AddScoped<IMemoryRateLimiterService, MemoryRateLimiterService>();
            




        }

        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BearerTokensOptions>()
                                .Bind(configuration.GetSection("BearerTokens"))
                                .Validate(bearerTokens =>
                                {
                                    return bearerTokens.AccessTokenExpirationMinutes < bearerTokens.RefreshTokenExpirationMinutes;
                                }, "RefreshTokenExpirationMinutes is less than AccessTokenExpirationMinutes. Obtaining new tokens using the refresh token should happen only if the access token has expired.");
            services.AddOptions<ApiSettings>()
                    .Bind(configuration.GetSection("ApiSettings"));
        }
    }
}