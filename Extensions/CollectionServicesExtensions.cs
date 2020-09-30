using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models.ServicesModels;
using Atlob_Dent.Services;
using Atlob_Dent.Services.AuthServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class CollectionServicesExtensions
    {
        public static IServiceCollection AddCustomAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy(GlobalVariables.CustomerUserPolicy, policyOptions => {
                    policyOptions.RequireAuthenticatedUser().RequireRole(GlobalVariables.CustomerRole).Build();
                });
                options.AddPolicy(GlobalVariables.AdminUserPolicy, policyOptions => {
                    policyOptions.RequireAuthenticatedUser().RequireRole(GlobalVariables.AdminRole).Build();
                });
                options.AddPolicy(GlobalVariables.AuthenticatedUserPolicy, policyOptions => {
                    policyOptions.RequireAuthenticatedUser().Build();
                });
            });
            return services;
        }
        public static IServiceCollection AddCustomAuthenticationServices(this IServiceCollection services)
        {
            services.AddTransient<JWThandlerService>();
            services.AddTransient<FacebookService>();
            services.AddTransient<GoogleService>();
            services.AddTransient<AccountService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var JWTSection = GlobalProperties.configuration.GetSection("JWT");
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;//disabled only in developement
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = JWTSection.GetValue<string>("issuer"),
                    ValidAudience = JWTSection.GetValue<string>("audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSection.GetValue<string>("signingKey")))
                };
            });
            return services;
        }
        public static IServiceCollection AddCustomThirdPartyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(configuration);
            return services;
        }
        public static IServiceCollection AddCustomHelperServices(this IServiceCollection services)
        {
            services.AddTransient<TransactionHelper>();           
            services.AddTransient<HttpContextAccessor>();
            return services;
        }
        public static IServiceCollection AddCustomDB_Context_identityServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<Atlob_dent_Context>(options =>
            options.UseSqlite(configuration.GetConnectionString("Atlob_dentDbLite")),ServiceLifetime.Scoped);
            //options.UseSqlServer(configuration.GetConnectionString("Atlob_dentDb")), ServiceLifetime.Scoped);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Atlob_dent_Context>()
                .AddDefaultTokenProviders();
            return services;
        }
    }
}
