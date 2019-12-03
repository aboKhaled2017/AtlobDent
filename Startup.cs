using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Atlob_Dent
{
    public class Startup
    {
        public IHostingEnvironment _env { get; set; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<Atlob_dent_Context>(options =>
             // options.UseSqlite(Configuration.GetConnectionString("Atlob_dentDbLite")),ServiceLifetime.Scoped);
             options.UseSqlServer(Configuration.GetConnectionString("Atlob_dentDb")), ServiceLifetime.Scoped);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Atlob_dent_Context>()
                .AddDefaultTokenProviders();
            services.AddTransient<TransactionHelper>();
            services.AddSingleton<IEmailSender>(new EmailSender());
            services.AddCors(options => {
                options.AddPolicy("CorePolicy", builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });
            services.Configure<IdentityOptions>(op => {
                op.Password.RequireNonAlphanumeric = false;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseCustomized_MainAppConfiguration(serviceProvider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDefaultSeededData();
            }
            else
            {
               // app.UseHsts();
                app.UseDefaultSeededData();
            }
            //app.UseHttpsRedirection();
            app.UseCors("CorePolicy");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes=> {
               
            });
        }
    }
}
