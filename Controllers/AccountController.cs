using _4kTiles_Backend.DataObjects.DAO.Account;
using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.DataObjects.DTO.Auth;
using _4kTiles_Backend.DataObjects.DTO.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _4kTiles_Backend.Services.Auth;

using AutoMapper;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IConfiguration configuration,
            IMapper mapper
        )
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(AccountRegisterDTO dto)
        {
            var dao = _mapper.Map<CreateAccountDAO>(dto);
            dao.Roles.Add("User");

            AccountDAO? account = await _accountRepository.CreateAccount(dao);

            // check if account with provided email already exists (created account is null)
            if (account == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Account with provided email already exists"
                });
            }
            else
            {
                return Created("success", new ResponseDTO 
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Account created"
                });
            }
        }

        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns></returns>
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(AccountRegisterDTO dto)
        {
            var dao = _mapper.Map<CreateAccountDAO>(dto);
            dao.Roles.Add("User");
            dao.Roles.Add("Admin");

            AccountDAO? account = await _accountRepository.CreateAccount(dao);

            // check if account with provided email already exists (created account is null)
            if (account == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Account with provided email already exists"
                });
            }
            else
            {
                return Created("success", new ResponseDTO
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Account created"
                });
            }
        }

        /// <summary>
        /// Account login
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns>User information</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginDTO dto)
        {
            try
            {
                // check if account with provided email exists
                AccountDAO? account = await _accountRepository.Login(dto.Email, dto.Password);

                // check if credentials are valid
                if (account == null)
                    return BadRequest(new ResponseDTO
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid credentials"
                    });

                // generate JWT token
                string token = _jwtService.GenerateAccountToken(
                    _configuration.GetValue<string>("Jwt:securityKey"), 
                    account.AccountId,
                    account.Roles
                    );

                Response.Cookies.Append("token", token);

                // return user information
                return Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success. The token is on Data",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
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
        public async Task<IActionResult> GetAccount()
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Invalid token"
            });
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;
            AccountDAO? account = await _accountRepository.GetAccountById(accountId);
            return account == null
                ? badResponse
                : Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = _mapper.Map<AccountDTO>(account)
                });
        }

        /// <summary>
        /// Deactivate the account from the token
        /// </summary>
        /// <returns>The status response</returns>
        [HttpDelete]
        [Authorize(Policy = "Creator")]
        public async Task<IActionResult> DeactivateAccount()
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Invalid token"
            });
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;
            return await _accountRepository.DeactivateAccount(accountId, "Self-Deletion")
                ? Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account deactivated"
                })
                : badResponse;
        }

        /// <summary>
        /// Deactivate the account based on the account id
        /// </summary>
        /// <param name="id">the account id</param>
        /// <param name="message">the message</param>
        /// <returns></returns>
        [HttpDelete("Admin/{id:int}")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> DeactivateAccountAsManager(int id, string message)
        {
            return await _accountRepository.DeactivateAccount(id, message)
                ? Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account deactivated"
                })
                : BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Account doesn't exist"
                });
        }
    }
}
