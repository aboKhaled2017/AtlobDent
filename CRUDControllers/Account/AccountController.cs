using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Models.AccountModels;
using Atlob_Dent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Atlob_Dent.CRUDControllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        /// <summary>
        /// login user to account
        /// </summary>
        /// <example>url=domain/api/account/login,method=post,body={email,password,confirmPassword} </example> 
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {            
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);
                if(user!=null&await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return Ok(SecurityHelper.GetTokenResponse(user.UserName));
                }
                return Unauthorized(new UnauthorizedObjectResult("email or password was invalid"));
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// register user for new account
        /// </summary>
        /// <example>url=domain/api/account/register,method=post,body={email,fullName,phone,password,confirmPassword} </example> 
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.email, Email = model.email,PhoneNumber=model.phone };
                var result = await _userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                    _emailSender.SendEmailConfirmation(model.email, callbackUrl);
                    return Ok(SecurityHelper.GetTokenResponse(user.UserName));
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("nameof(ExternalLoginCallback)", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Ok(returnUrl);
                    }
                }
                AddErrors(result);
            }
            return Ok();
        }
        /// <summary>
        /// confirm user email account
        /// </summary>
        /// <example>url=domain/api/account/ConfirmEmail?userId=xxx&code=yyy</example> 
        /// <param name="code"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? true :false;
        }
        /// <summary>
        /// try to retrieve user's forgotton password
        /// </summary>
        /// <example>url=domain/api/account/ForgotPassword method=post,body={email}</example>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest(new BadRequestObjectResult("this email is not found or not confirmed"));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id.ToString(), code, Request.Scheme);
               _emailSender.SendEmail(model.Email,"Reset password", $"the reset password code is {code}");
                return Ok($"a code {code} is sent to this user email {model.Email}");
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(new BadRequestObjectResult("email is not valid"));
        }
        /// <summary>
        /// reset user password
        /// </summary>
        /// <example>url=domain/api/account/ResetPassword method=post,body={email,code,password,confirmPassword}</example>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest(new BadRequestObjectResult($"the email ${model.email} was not found"));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.code, model.password);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
            return BadRequest(new BadRequestObjectResult($"faild to create password for user {user.UserName}"));
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}