using BaseAPI;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Utilities;
using Hangfire;
using Hangfire.SqlServer;
using Interface.Services;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Log.Logger = new Serilog.LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .CreateLogger();

            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        readonly string SignalROrigins = "SignalROrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("SampleDbContext"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            //SQL
            services.AddDbContext<AppDbContext.AppDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SampleDbContext")));

            //Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(AppAutoMapper).Assembly);

            services.AddHttpClient();

            //Using ..BaseAPI/ServiceExtensions
            services.ConfigureRepositoryWrapper();
            services.ConfigureService();
            services.ConfigureSwagger();

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .WithOrigins("https://localhost:5001")
                    .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   ;
                });
                options.AddPolicy(SignalROrigins,
                builder =>
                {
                    builder
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetIsOriginAllowed(hostName => true)
                   ;
                });
            });
            //Remove SetCompatibilityVersion(CompatibilityVersion.Version_3_0) when Net 6
            services.AddMvc()/*.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)*/.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddSession(options =>
            {
                //// Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                //options.Cookie.HttpOnly = true;
                //// Make the session cookie essential
                //options.Cookie.IsEssential = true;
            });

            var key = Encoding.ASCII.GetBytes(appSettings.secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });

            //services.AddMemoryCache();
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddDetection();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddSerilog();

            //serviceProvider.MigrationDatabase(Configuration);

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //         Path.Combine(env.ContentRootPath, "Upload/QRCode")),
            //    RequestPath = "/Upload/QRCode",
            //});

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(env.ContentRootPath, "Upload")),
                RequestPath = "",
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Template")),
                RequestPath = "/Template",
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticHttpContext();

            app.UseSession();
            app.UseCookiePolicy();
            app.UseDetection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();
            //Hangfire
            var storage1 = new SqlServerStorage(Configuration.GetConnectionString("SampleDbContext"));
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() },
                IgnoreAntiforgeryToken = true
            }, storage1);
            //RecurringJob.AddOrUpdate<IBiddingSessionService>(
            //    "Cập nhật trạng thái ngày nghỉ - Chạy một giờ một lần",
            //    x => x.BiddingSessionAuto(), "*/1 * * * *"
            //    );
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "ACP Dừa á châu");
                c.InjectStylesheet("../css/swagger.min.css");
                //c.RoutePrefix = "swagger";
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/hubs/notifications").RequireCors(SignalROrigins);
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=index}/{id?}"
            //        );
            //});
        }
    }
}
