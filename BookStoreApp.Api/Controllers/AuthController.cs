using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.User;
using BookStoreApp.Api.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookStoreApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;
		private readonly IMapper _mapper;
		private readonly UserManager<ApiUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthController(ILogger<AuthController> logger,
			IMapper mapper, UserManager<ApiUser> userManager,
			RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		[HttpPost(template: "register")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async Task<IActionResult> Register(UserDto userDto)
		{
			try
			{
				var apiUser = _mapper.Map<ApiUser>(userDto);
				apiUser.UserName = userDto.Email;
				var result = await _userManager.CreateAsync(apiUser, userDto.Password);

				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}

					return BadRequest(ModelState);
				}

				result = await _userManager.AddToRoleAsync(apiUser, userDto.Role);
				if (!result.Succeeded)
				{
					await _userManager.DeleteAsync(apiUser);
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}

					return BadRequest(ModelState);
				}
				return Accepted();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return Problem(Messages.Error500Message);
			}
		}

		[HttpPost(template: "login")]
		public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
		{
			try
			{
				_logger.LogInformation($"Login attempt for {userDto.Email}");
				var user = await _userManager.FindByEmailAsync(userDto.Email);
				var isPasswordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

				if (user == null || !isPasswordValid)
				{
					return Unauthorized(userDto);
				}

				var tokenString = await GenerateToken(user);
				var response = new AuthResponse()
				{
					Email = userDto.Email,
					UserId = user.Id,
					Token = tokenString,
				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return Problem(Messages.Error500Message);
			}
		}

		private async Task<string> GenerateToken(ApiUser user)
		{
			var securityKey =
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
					_configuration["JwtSettings:Key"]
				));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles
				.Select(q => new Claim(ClaimTypes.Role, q))
				.ToList();

			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(CustomClaimTypes.Uid, user.Id),
			}
				.Union(roleClaims);
			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				expires: DateTime.UtcNow.AddHours(
					Convert.ToInt32(_configuration["JwtSettings:Duration"])),
				signingCredentials: credentials,
				claims: claims
				);
			return new JwtSecurityTokenHandler().WriteToken(token);

		}
	}
}
