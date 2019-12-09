using Atlob_Dent.Models.ServicesModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent
{
    public static class GlobalVariables
    {       
        private static IConfigurationSection EmailConfing { get {
                return GlobalProperties.configuration.GetSection("mailSetting");
            } set { } }
        public static EmailConfigModel EmailConfigObject { get {
                return new EmailConfigModel { 
                from=EmailConfing.GetValue<string>("from"),
                password = EmailConfing.GetValue<string>("password"),
                writeAsFile = EmailConfing.GetValue<bool>("writeAsFile")
                };
            } set { } }
        public static string AdminRole { get; set; } = "Admin";
        public static string CustomerRole { get; set; } = "Customer";
        public static string CustomerUserPolicy{ get; set; } = "CustomerUserPolicy";
        public static string AdminUserPolicy { get; set; } = "AdminUserPolicy";
        public static string AuthenticatedUserPolicy { get; set; } = "AuthenticatedUserPolicy";
        public static string AdminSectionNameOfConfigFile { get; set; } = "AdminData";
        public static string LocalProviderName { get; set; } = "local";
        public static string FacebookProviderName { get; set; } = "facebook";
        public static string FacebookProviderKeyName { get; set; } = "Provider_key_for_FB";
        public static string FacebookProviderDisplayName { get; set; } = "Facebook";
        public static string GoogleProviderName { get; set; } = "google";
        public static string GoogleProviderDisplayName { get; set; } = "Google";
        public static string GoogleProviderKeyName { get; set; } = "Provider_key_for_Google";
        public static string DefaultUserImageName { get; set; } = "DefaultUserImg.png";
       
    }
}
