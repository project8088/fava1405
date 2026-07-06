
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Nikan.Common.WebToolkit;
using Nikan.IoCConfig;
using Nikan.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace Nikan.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddControllers().AddNewtonsoftJson(options =>
           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).ConfigureApiBehaviorOptions(o => {
                o.SuppressModelStateInvalidFilter = true; 
            });
            services.AddCustomOptions(Configuration);
            services.AddCustomServices();
            services.AddCustomDbContext(Configuration);
            services.AddCustomJwtBearer(Configuration);
            services.AddCustomCors();
            services.AddCustomAntiforgery();
            services.AddSession();// add session
            services.AddAutoMapper(typeof(Startup));

              


            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("ContactLimit", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));
            });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                   name: "nikan",
                   info: new Microsoft.OpenApi.Models.OpenApiInfo()
                   {
                       Title = "سامانه پروفایل شهروندی",
                       Version = "1",
                       Description = "گروه  مهندسی نیکان",
                       Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                       {
                           Email = "info@nikanit-co.ir",
                           Name = "nikanit",
                           Url = new Uri("https://nikanit-co.ir/")
                       },
                       License = new Microsoft.OpenApi.Models.OpenApiLicense()
                       {
                           Name = "MIT License",
                           Url = new Uri("https://opensource.org/licenses/MIT")
                       }

                   });
                setupAction.SchemaFilter<AddEnumNamesSchemaFilter>();
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => setupAction.IncludeXmlComments(xmlFile));
                setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            services.AddMemoryCache(); // Add this line
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializerService>();
                dbInitializer.Initialize();
                dbInitializer.SeedData();
                dbInitializer.SetSettings();



            }

            
            if (!env.IsDevelopment())
            {

                app.UseHsts();
               // app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error?.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            State = 401,
                            Msg = "token expired"
                        }));
                    }
                    else if (error?.Error != null)
                    {
                        
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            State = 500,
                            Msg = error.Error.Message
                        }));
                    }
                    else
                    {
                        await next();
                    }
                });
            });

            app.UseStatusCodePages();
             
          
            app.UseSession();
            app.UseRouting();
            app.UseRateLimiter();  // قبل از auth
            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();


          


            StimulSoftLicense.LoadLicense(env);
            app.UseCustomHeaders((opt) =>
            {
                opt.HeadersToRemove.Add("X-Powered-By");
                opt.HeadersToRemove.Add("x-aspnet-version");
            });


            app.UseDeveloperExceptionPage();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseHangfireDashboard(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
                // HangFire Dashboard endpoint
                endpoints.MapHangfireDashboard();
            });
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(setupAction =>
                {
                    setupAction.SwaggerEndpoint(
                        url: "/swagger/nikan/swagger.json",
                        name: "nikan");
                    //setupAction.RoutePrefix = "";
                });
            }

            app.UseStaticFiles();

            // catch-all handler for HTML5 client routes - serve index.html
            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });





        }
    }



    public static class StimulSoftLicense
    {
        public static void LoadLicense(IWebHostEnvironment environment)
        {

            try
            {
                var contentRoot = environment.ContentRootPath;
                var licenseFile = System.IO.Path.Combine(contentRoot, "Reports", "license.key");
                Stimulsoft.Base.StiLicense.LoadFromFile(licenseFile);

            }
            catch (Exception er)
            {


            }





        }
    }












}


public class AddEnumNamesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        if (type.IsEnum)
        {
            var enumNames = Enum.GetNames(type);

            // پشتیبانی از مقدارهای فارسی
            var enumNamesArray = new OpenApiArray();
            foreach (var name in enumNames)
            {
                enumNamesArray.Add(new OpenApiString(name));
            }

            schema.Extensions.Add("x-enumNames", enumNamesArray);
        }
    }
}
