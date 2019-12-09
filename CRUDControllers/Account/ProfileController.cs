using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Atlob_Dent.Data;
using Atlob_Dent.Helpers;
using Atlob_Dent.Models.AccountModels;
using Atlob_Dent.Models.ExternalAuthModels;
using Atlob_Dent.Services;
using Atlob_Dent.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atlob_Dent.CRUDControllers.Account
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("CustomerUserPolicy")]
    public class ProfileController : SharedAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly AccountService _accountService;
        private readonly TransactionHelper _transactionHelper;
        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            AccountService accountService,
            TransactionHelper transactionHelper)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _accountService = accountService;
            _transactionHelper = transactionHelper;
            _accountService.SetCurrentContext(HttpContext,Url);
        }
        [HttpPost]
        public async Task<IActionResult> EditPicture()
        {
            try
            {
                var a = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var id = a.Value;
                var user =await _userManager.FindByIdAsync(id);
                return Ok("image has been successfully updated");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}