using System;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models.AccountModels;
using Atlob_Dent.Models.ExternalAuthModels;
using Atlob_Dent.Services;
using Atlob_Dent.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atlob_Dent.CRUDControllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : SharedAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly AccountService _accountService;
        private readonly TransactionHelper _transactionHelper;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            TransactionHelper transactionHelper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _accountService = accountService;
            _transactionHelper = transactionHelper;
        }
        #region Regular signin/signup
        /// <summary>
        /// login user to account
        /// </summary>
        /// <example>url=domain/api/account/login,method=post,body={email,password,confirmPassword} </example> 
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SetCurrentContext(HttpContext, Url);
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (await _userManager.UserIdentityExists(user,model.Password,GlobalVariables.CustomerRole))
                    {
                        var response = _accountService.GetSigningInResponseModel(user, GlobalVariables.CustomerRole);
                        _transactionHelper.CommitChanges();
                        return Ok(response);
                    }
                    return Unauthorized(new UnauthorizedObjectResult("email or password was invalid"));
                }
                catch (Exception ex)
                {
                    _transactionHelper.RollBackChanges();
                    return BadRequest(ex.Message);
                }
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
        public async Task<IActionResult> SignUp(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SetCurrentContext(HttpContext, Url);
                    var response=  await _accountService.SignUpAsync(model,(anyErrors,resultError)
                       => {if(anyErrors) AddErrors(resultError);});
                    if (response != null)
                    {
                        _transactionHelper.CommitChanges();
                        return Ok(response);
                    }
                    _transactionHelper.RollBackChanges();
                }
                catch (Exception ex)
                {
                    _transactionHelper.RollBackChanges();
                    return BadRequest(ex.Message);
                }                
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region external signin
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> FacebbookSign(FacebookLoginModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SetCurrentContext(HttpContext, Url);
                    var response = await _accountService.FacebookLoginAsync(model);
                    _transactionHelper.CommitChanges();
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _transactionHelper.RollBackChanges();
                  return  BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleSign(GoogleLoginModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.SetCurrentContext(HttpContext, Url);
                    var response = await _accountService.GoogleLoginAsync(model);
                    _transactionHelper.CommitChanges();
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _transactionHelper.RollBackChanges();
                   return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        #endregion
        #region for email settings
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
            return result.Succeeded ? true : false;
        }
        #endregion
        #region for password setting
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
                await _emailSender.SendEmailAsync(model.Email, "Reset password", $"the reset password code is {code}");
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
        #endregion
    }
}