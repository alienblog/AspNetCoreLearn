using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Galaxy.Entities;
using Galaxy.Infrastructure.Core;
using Galaxy.Infrastructure.Repositories.Abstract;
using Galaxy.Infrastructure.Services.Abstract;
using Galaxy.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Galaxy.Controllers
{
	[Route("api/[controller]")]
    public class AccountController : Controller
    {
	    private readonly IMemberShipService _memberShipService;
	    private readonly IUserRepository _userRepository;

	    public AccountController(
		    IMemberShipService memberShipService,
		    IUserRepository userRepository)
	    {
		    _memberShipService = memberShipService;
		    _userRepository = userRepository;
	    }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> Login([FromBody] LoginViewModel user)
		{
			GenericResult authenticationResult;

			try
			{
				var userContext = _memberShipService.ValidateUser(user.Username, user.Password);

				if (userContext.User != null)
				{
					IEnumerable<Role> roles = _userRepository.GetUserRoles(user.Username);
					var claims = roles
						.Select(role => new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, user.Username))
						.ToList();

					await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
						new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties { IsPersistent = user.RememberMe });


					authenticationResult = new GenericResult
					{
						Succeeded = true,
						Message = "Authentication succeeded"
					};
				}
				else
				{
					authenticationResult = new GenericResult
					{
						Succeeded = false,
						Message = "Authentication failed"
					};
				}
			}
			catch (Exception ex)
			{
				authenticationResult = new GenericResult
				{
					Succeeded = false,
					Message = ex.Message
				};
			}

			IActionResult result = new ObjectResult(authenticationResult);
			return result;
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await HttpContext.Authentication.SignOutAsync("Cookies");
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("register")]
		[HttpPost]
		public IActionResult Register([FromBody] RegistrationViewModel model)
		{
			GenericResult registrationResult = null;

			try
			{
				if (ModelState.IsValid)
				{
					var user = _memberShipService.CreateUser(model.Username, model.Email, model.Password, new[] { 1 });

					if (user != null)
					{
						registrationResult = new GenericResult
						{
							Succeeded = true,
							Message = "Registration succeeded"
						};
					}
				}
				else
				{
					registrationResult = new GenericResult
					{
						Succeeded = false,
						Message = "Invalid fields."
					};
				}
			}
			catch (Exception ex)
			{
				registrationResult = new GenericResult()
				{
					Succeeded = false,
					Message = ex.Message
				};
			}

			IActionResult result = new ObjectResult(registrationResult);
			return result;
		}
	}
}
