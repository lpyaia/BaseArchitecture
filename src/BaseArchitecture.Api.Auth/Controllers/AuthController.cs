using BasicArchitecture.Api.Auth.DTOs;
using BasicArchitecture.Api.Auth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BasicArchitecture.Api.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService,
            ILogger<AuthController> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new AuthResponseDto(
                        Success: false,
                        Errors: ModelState.Values.SelectMany(v =>
                            v.Errors.Select(e => e.ErrorMessage)
                        )
                    )
                );
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(
                    new AuthResponseDto(
                        Success: false,
                        Errors: ["Password and confirmation password do not match."]
                    )
                );
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return BadRequest(
                    new AuthResponseDto(
                        Success: false,
                        Errors: ["User with this email already exists."]
                    )
                );
            }

            var newUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(
                    new AuthResponseDto(
                        Success: false,
                        Errors: result.Errors.Select(e => e.Description)
                    )
                );
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            _logger.LogInformation("User {Email} created a new account", model.Email);

            var token = await _tokenService.GenerateJwtToken(newUser);

            return Ok(
                new AuthResponseDto(
                    Success: true,
                    Token: token,
                    Expiration: DateTime.UtcNow.AddHours(
                        Convert.ToDouble(
                            HttpContext.RequestServices.GetRequiredService<IConfiguration>()[
                                "Jwt:ExpirationHours"
                            ]
                        )
                    )
                )
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new AuthResponseDto(
                        Success: false,
                        Errors: ModelState.Values.SelectMany(v =>
                            v.Errors.Select(e => e.ErrorMessage)
                        )
                    )
                );
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(
                    new AuthResponseDto(Success: false, Errors: ["Invalid email or password."])
                );
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(
                    new AuthResponseDto(Success: false, Errors: ["Invalid email or password."])
                );
            }

            _logger.LogInformation("User {Email} logged in", model.Email);

            var token = await _tokenService.GenerateJwtToken(user);

            return Ok(
                new AuthResponseDto(
                    Success: true,
                    Token: token,
                    Expiration: DateTime.UtcNow.AddHours(
                        Convert.ToDouble(
                            HttpContext.RequestServices.GetRequiredService<IConfiguration>()[
                                "Jwt:ExpirationHours"
                            ]
                        )
                    )
                )
            );
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successful" });
        }
    }
}
