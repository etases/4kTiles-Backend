using System.IdentityModel.Tokens.Jwt;
using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.DataObjects.DTO.Auth;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _4kTiles_Backend.Services.Auth;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // AccountRepository is injected by the DI container.
        private readonly IAccountRepository _accountRepository;

        // JwtService is injected by the DI container.
        private readonly IJwtService _jwtService;

        // Configuration is injected by the DI container.
        private readonly IConfiguration _configuration;

        /// <summary>
        /// AuthController constructor
        /// </summary>
        /// <param name="accountRepository">AccountRepository</param>
        /// <param name="jwtService">JWTService</param>
        /// <param name="configuration">Configuration</param>
        public AuthController(
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IConfiguration configuration
        )
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public ActionResult<DynamicResponseDTO> RegisterUser(AccountRegisterDTO dto)
        {
            // check if account with provided email already exists
            if (_accountRepository.getAccountByEmail(dto.Email) != null)
                return BadRequest(new DynamicResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "Account with provided email already exists"
                });

            // create new account
            Account account =
                new Account
                {
                    UserName = dto.UserName,
                    // hash password
                    HashedPassword = dto.Password.Hash(),
                    Email = dto.Email
                };

            // save account
            _accountRepository.createUserAccount(account);
            return Created("success",
            new DynamicResponseDTO
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Account created"
            });
        }

        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns></returns>
        [HttpPost("RegisterAdmin")]
        public ActionResult<DynamicResponseDTO> RegisterAdmin(AccountRegisterDTO dto)
        {
            // check if account with provided email already exists
            if (_accountRepository.getAccountByEmail(dto.Email) != null)
                return BadRequest(new DynamicResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "Account with provided email already exists"
                });

            // create new account
            Account account =
                new Account
                {
                    UserName = dto.UserName,
                    // hash password
                    HashedPassword = dto.Password.Hash(),
                    Email = dto.Email
                };

            // save account
            _accountRepository.createAdminAccount(account);
            return Created("success",
            new DynamicResponseDTO
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Account created"
            });
        }

        /// <summary>
        /// Account login
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns>User information</returns>
        [HttpPost("Login")]
        public ActionResult<DynamicResponseDTO> Login(AccountLoginDTO dto)
        {
            try
            {
                string credentialErrorMessage = "Invalid credentials";

                // check if account with provided email exists
                Account account = _accountRepository.getAccountByEmail(dto.Email);

                Role role = _accountRepository.getAccountRoleById(account.AccountId);

                // check if credentials are valid
                if (
                    account == null || dto.Password.VerifyHash(account.HashedPassword)
                )
                    return BadRequest(new DynamicResponseDTO
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsError = true,
                        Message = credentialErrorMessage
                    });

                // generate JWT token
                string token =
                    _jwtService
                        .GenerateAccountToken(_configuration
                            .GetValue<string>("Jwt:securityKey"),
                        account.AccountId, role.RoleName);

                Response.Cookies.Append("token", token);

                // return user information
                return Ok(new ResponseDTO<dynamic>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = new { Token = token }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new DynamicResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorCode = 2,
                    Message = ex.Message,
                    Data = ex.StackTrace,
                });
            }
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <returns>Account information</returns>
        [HttpGet("Account")]
        [Authorize]
        public ActionResult<DynamicResponseDTO> GetAccount()
        {
            try
            {
                string token = Request.Cookies["token"];

                // check if token is valid
                JwtSecurityToken verifiedToken =
                    _jwtService
                        .VerifyToken(_configuration
                            .GetValue<string>("Jwt:securityKey"),
                        token);

                // get account id from token
                int accountId =
                    int
                        .Parse(verifiedToken
                            .Claims
                            .FirstOrDefault(claim => claim.Type == "accountId")
                            .Value);

                // get user information
                Account account = _accountRepository.getAccountById(accountId);

                // return user information
                return Ok(new DynamicResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data =
                        new
                        {
                            account.AccountId,
                            account.UserName,
                            account.Email
                        }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new DynamicResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "Invalid token" ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <returns>Cleared cookie</returns>
        [HttpPost("Logout")]
        [Authorize]
        public ActionResult<DynamicResponseDTO> Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new DynamicResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
    }
}
