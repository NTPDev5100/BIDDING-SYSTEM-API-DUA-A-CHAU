using Interface.DbContext;
using Interface.Repository;
using Interface.Services;
using Interface.Services.Auth;
using Interface.Services.Catalogue;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service;
using Service.Repository;
using Service.Services;
using Service.Services.Auth;
using Service.Services.Catalogue;
using Service.Services.Configurations;
using System;
using System.Globalization;
using System.IO;

namespace BaseAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IAppDbContext, AppDbContext.AppDbContext>();
            services.AddScoped(typeof(IDomainRepository<>), typeof(DomainRepository<>));
            services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
        }

        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("he")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddTransient<ITokenManagerService, TokenManagerService>();

            #region Authenticate

            services.AddScoped<IWardsService, WardsService>();
            services.AddScoped<IDistrictsService, DistrictsService>();
            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();

            #endregion Authenticate
            services.AddScoped<IBiddingService, BiddingService>();
            services.AddScoped<IBiddingSessionService, BiddingSessionService>();
            services.AddScoped<IBiddingTicketService,BiddingTicketService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITechnicalProductService, TechnicalProductService>();


            #region Customer
            services.AddScoped<IProviderService, ProviderService>();
            #endregion

            #region Catalogue
            services.AddScoped<IProductsService, ProductService>();
            services.AddScoped<ITechnicalOptionService, TechnicalOptionService>();
            #endregion

            #region Configuration

            services.AddScoped<IEmailConfigurationService, EmailConfigurationService>();
            //services.AddScoped<ISMSConfigurationService, SMSConfigurationService>();
            //services.AddScoped<ISMSEmailTemplateService, SMSEmailTemplateService>();
            //services.AddScoped<IOTPHistoryService, OTPHistoryService>();

            #endregion Configuration
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ACP Dừa á châu", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });

                var dir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                foreach (var fi in dir.EnumerateFiles("*.xml"))
                {
                    c.IncludeXmlComments(fi.FullName);
                }

                c.EnableAnnotations();
            });
        }
    }
}