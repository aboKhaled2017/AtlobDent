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
using Microsoft.AspNetCore.Http;
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
            services.AddCustomThirdPartyServices(Configuration);
            services.AddCustomHelperServices();
            services.AddCustomDB_Context_identityServices(Configuration);
            services.AddCustomAuthenticationServices();
            services.AddCustomAuthorizationServices();
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
