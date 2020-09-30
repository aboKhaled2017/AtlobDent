using Atlob_Dent.Data;
using Atlob_Dent.Models;
using Atlob_Dent.Models.AccountModels;
using Atlob_Dent.Models.ExternalAuthModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlob_Dent.Services.AuthServices
{
    public class AccountService
    {
        private readonly GoogleService _googleService;
        private readonly FacebookService _facebookService;
        private readonly JWThandlerService _jWThandlerService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Atlob_dent_Context _context;
        private HttpContext _httpContext { get; set; }
        private IUrlHelper _Url { get; set; }
        private readonly IConfigurationSection _JWT = GlobalProperties.configuration.GetSection("JWT");
        public AccountService(GoogleService googleService, IEmailSender emailSender, FacebookService facebookService, JWThandlerService  jWThandlerService,UserManager<ApplicationUser> userManager,Atlob_dent_Context context)
        {
            _googleService = googleService;
            _facebookService = facebookService;
            _jWThandlerService = jWThandlerService;
            _userManager = userManager;
            _emailSender= emailSender;
            _context = context;
        }
        public void SetCurrentContext(HttpContext httpContext,IUrlHelper url)
        {
            _httpContext = httpContext;
            _Url = url;
        }
        public async Task RegisterCustomerUser(ApplicationUser user, RegisterModel model)
        {
            var customer = new Customer
            {
                id = user.Id,
                imgSrc = $"{GlobalProperties.GetUsersImagesPath(_httpContext)}{GlobalVariables.DefaultUserImageName}",
                fullName =model.fullName
            };
            _context.Customers.Add(customer);
            if (await _context.SaveChangesAsync() != 1)
                throw new Exception("cannot add customer user");
        }
        private async Task<ApplicationUser> RegisterFBUser(FacebookUserModel facebookUserModel)
        {
            //await RemoveUserIfNotConfirmed(facebookUserModel.email);
            var user =await _userManager.FindByEmailAsync(facebookUserModel.email);
            if (user != null)
            {
                user.EmailConfirmed = true;
            }
            else
            {
                user = new ApplicationUser
                {
                    UserName = facebookUserModel.email,
                    Email = facebookUserModel.email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("cannot create facebook user");
                }
                var customer = new Customer
                {
                    id=user.Id,
                    imgSrc=facebookUserModel.pictureSrc,
                    fullName=$"{facebookUserModel.fname} {facebookUserModel.lname}"
                };
                _context.Customers.Add(customer);
                if (await _context.SaveChangesAsync() != 1)
                    throw new Exception("cannot add customer user");
            }
                
            var addLoginResult= await _userManager.AddLoginAsync(user, new UserLoginInfo(
            GlobalVariables.FacebookProviderName,
            facebookUserModel.id,
            GlobalVariables.FacebookProviderDisplayName));

            if (!addLoginResult.Succeeded)
            {
                throw new Exception("cannot add facebook user");
            }
            return user;
        }
        private async Task<ApplicationUser> RegisterGoogleUser(GoogleUserModel googleUserModel)
        {
            //await RemoveUserIfNotConfirmed(facebookUserModel.email);
            var user = await _userManager.FindByEmailAsync(googleUserModel.email);
            if (user != null)
            {
                user.EmailConfirmed = true;
            }
            else
            {
                user = new ApplicationUser
                {
                    UserName = googleUserModel.email,
                    Email = googleUserModel.email,
                    EmailConfirmed = googleUserModel.IsEmailConfirmed
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("cannot create google user");
                }
                var customer = new Customer
                {
                    id = user.Id,
                    imgSrc = googleUserModel.pictureSrc,
                    fullName = googleUserModel.name
                };
                _context.Customers.Add(customer);
                if (await _context.SaveChangesAsync() != 1)
                    throw new Exception("cannot add customer user");
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(
            GlobalVariables.GoogleProviderName,
            googleUserModel.id,
            GlobalVariables.GoogleProviderDisplayName));

            if (!addLoginResult.Succeeded)
            {
                throw new Exception("cannot add google user");
            }
            return user;
        }
        public async Task<SigningInResponseModel> FacebookLoginAsync(FacebookLoginModel facebookLoginModel)
        {
            if (string.IsNullOrEmpty(facebookLoginModel.facebookToken))
                throw new Exception("token is null");
            var facebookUser =await _facebookService.GetUserFromFacebookAsync(facebookLoginModel.facebookToken);
            if (facebookUser == null)
            {
                throw new Exception("Invalid OAuth access token,cannot get facebook user");
            }         
            
            var userLoginInfo = new UserLoginInfo(GlobalVariables.FacebookProviderName, facebookUser.id, GlobalVariables.FacebookProviderDisplayName);
            var domainUser = await _userManager.FindByLoginAsync(GlobalVariables.FacebookProviderName,facebookUser.id);
            if (domainUser == null)
            {
                domainUser=await RegisterFBUser(facebookUser);
            }
            return GetSigningInResponseModel(domainUser, GlobalVariables.CustomerRole);
            /*return new AuthTokensModel
            {
                accessToken=_jWThandlerService.CreateAccessToken(domainUser,GlobalVariables.CustomerRole),
                refreshToken=_jWThandlerService.CreateRefreshToken(domainUser)
            };*/
        }
        public async Task<SigningInResponseModel> GoogleLoginAsync(GoogleLoginModel googleLoginModel )
        {
            if (string.IsNullOrEmpty(googleLoginModel.googleToken))
                throw new Exception("token is null");
            var googleUser = await _googleService.GetUserFromGoogleAsync(googleLoginModel.googleToken);
            if (googleUser == null)
            {
                throw new Exception("Invalid OAuth access token,cannot get google user");
            }

            var userLoginInfo = new UserLoginInfo(GlobalVariables.FacebookProviderName, googleUser.id, GlobalVariables.FacebookProviderDisplayName);
            var domainUser = await _userManager.FindByLoginAsync(GlobalVariables.GoogleProviderName, googleUser.id);
            if (domainUser == null)
            {
                domainUser = await RegisterGoogleUser(googleUser);
            }
            return GetSigningInResponseModel(domainUser, GlobalVariables.CustomerRole);
        }
        public async Task<SigningInResponseModel> SignUpAsync(RegisterModel model,Action<bool,IdentityResult>ActionOnResult)
        {
            //the email is already checked at validation if it was existed before for any user
           var user = new ApplicationUser { UserName = model.email, Email = model.email, PhoneNumber = model.phone };
            var result = await _userManager.CreateAsync(user, model.password);       
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, GlobalVariables.CustomerRole);
                if (result.Succeeded)
                {
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = _Url.EmailConfirmationLink(user.Id.ToString(), code, _httpContext.Request.Scheme);
                    var code = await _userManager.GenerateEmailConfirmationCodeAsync(user);
                    await _emailSender.SendEmailAsync(model.email,"get code to confirm your email",$"the code to confirm email is {code}");
                    await RegisterCustomerUser(user, model);
                    ActionOnResult(false, result);
                    return GetSigningInResponseModel(user, GlobalVariables.CustomerRole,UserType.localUser);
                }              
            }
            ActionOnResult(true,result);
            return null;
        }
        public  SigningInResponseModel GetSigningInResponseModel(ApplicationUser user,string role,UserType userType=UserType.externalUser)
        {
            var customerData =_context.Customers.Where(c => c.id == user.Id).Select(c => new { name = c.fullName, imgSrc = c.imgSrc }).FirstOrDefault();
            return new SigningInResponseModel {
                user = new UserResponseModel
                {
                    id = user.Id,
                    email = user.Email,
                    name = customerData.name,
                    phone = user.PhoneNumber,
                    imgSrc = customerData.imgSrc,
                    hasPassword=userType==UserType.externalUser?false:true
                },
                accessToken=_jWThandlerService.CreateAccessToken(user,role,userType)
            };
        }
        public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            var body = new StringBuilder();
            body.Append($"<a href='${callbackUrl}'>confirm your email</a>");
            await _emailSender.SendEmailAsync(email, "confirm your email", body.ToString());
        }
    }
}