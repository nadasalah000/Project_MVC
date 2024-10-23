using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMailService mailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _mailService = mailService;
        }
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = new ApplicationUser()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					Fname = model.FName,
					Lname = model.LName,
					IsAgree = model.IsAgree,
				};
				var Result = await _userManager.CreateAsync(User, model.Password);
				if (Result.Succeeded)
					return RedirectToAction(nameof(Login));
				else
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

			}
			return View(model);
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User is not null)
				{
					var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
					if (Flag)
					{
						var Result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
						if (Result.Succeeded)
						{
							return RedirectToAction("Me", "Home");
						}
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Incorrect Password");
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Is Not Exsits");
				}
			}
			return View(model);
		}
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendResetPassword(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url,
					};
					_mailService.SendEmail(email);
					return RedirectToAction(nameof(CheckYourIndex));
				}
				ModelState.AddModelError(string.Empty, "Invalid Email !");
			}
			return View(nameof(ForgetPassword),model);
		}

        //[HttpPost]
        /*public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid) {
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User is not null)
				{
					var email = new Email()
					{
						Subject = "Reset Password",
						To = model.Email,
						Body = "ResetPasswordLink"
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourIndex));
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Is Not Exists");
				}
			}
		
				return View("ForgetPassword", model);

		}*/

        public IActionResult CheckYourIndex()
		{

		return View();
			
		}
        public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ReserPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var User = await _userManager.FindByNameAsync(email);
				var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
				if(Result.Succeeded) 
					return RedirectToAction(nameof(Login));
				else
					foreach(var error in Result.Errors)
						ModelState.AddModelError(string.Empty,error.Description);
			}
			return View(model);
		}
    }
}
