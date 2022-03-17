using _4kTiles_Backend.DataObjects.DAO.Account;
using _4kTiles_Backend.DataObjects.DTO.Account;
using _4kTiles_Backend.DataObjects.DTO.Email;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.DataObjects.DTO.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _4kTiles_Backend.Services.Auth;
using _4kTiles_Backend.Services.Email;

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
        private readonly IEmailService _mailService;

        public AccountController(
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IConfiguration configuration,
            IMapper mapper,
            IEmailService mailService
        )
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _mapper = mapper;
            _mailService = mailService;
        }

        /// <summary>
        /// User account registration
        /// </summary>
        /// <param name="dto">Account information</param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<ResponseDTO>> RegisterUser(AccountRegisterDTO dto)
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
                    IsError = true,
                    Message = "Account with provided email already exists"
                });
            }
            else
            {
                await _mailService.SendEmail(new EmailContent
                {
                    ToEmail = dto.Email,
                    Value = "Your account is created. You can start your journey on our game with more fun than before."
                });
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
        public async Task<ActionResult<ResponseDTO>> RegisterAdmin(AccountRegisterDTO dto)
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
                    IsError = true,
                    Message = "Account with provided email already exists"
                });
            }
            else
            {
                await _mailService.SendEmail(new EmailContent
                {
                    ToEmail = dto.Email,
                    Value = "Your account is created. You can manage other accounts and songs."
                });
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
        public async Task<ActionResult<ResponseDTO<string>>> Login(AccountLoginDTO dto)
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
                        ErrorCode = 1,
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
                return Ok(new ResponseDTO<string>
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
                    ErrorCode = -1,
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
        public async Task<ActionResult<ResponseDTO<AccountDTO>>> GetAccount()
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
                Message = "Invalid token"
            });
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;
            AccountDAO? account = await _accountRepository.GetAccountById(accountId, false);
            return account == null
                ? badResponse
                : Ok(new ResponseDTO<AccountDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = _mapper.Map<AccountDTO>(account)
                });
        }

        /// <summary>
        /// Get the account by the account id
        /// </summary>
        /// <param name="id">the account id</param>
        /// <returns>the account</returns>
        [HttpGet("Account/{id:int}")]
        public async Task<ActionResult<ResponseDTO<AccountDTO>>> GetAccount(int id)
        {
            AccountDAO? account = await _accountRepository.GetAccountById(id, false);
            return account == null
                ? BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "Invalid token"
                })
                : Ok(new ResponseDTO<AccountDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = _mapper.Map<AccountDTO>(account)
                });
        }

        /// <summary>
        /// Get accounts
        /// </summary>
        /// <returns>the accounts</returns>
        [HttpGet("All")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<PaginationResponseDTO<AccountDTO>>> GetAccounts([FromQuery] string? name, [FromQuery] PaginationParameter pagination)
        {
            var accounts = await _accountRepository.GetAccounts(searchName: name, pagination: pagination, getDeleted: false);
            return Ok(new PaginationResponseDTO<AccountDTO>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Success",
                TotalRecords = accounts.TotalRecords,
                Data = _mapper.Map<IEnumerable<AccountDTO>>(accounts.Payload)
            });
        }

        /// <summary>
        /// Deactivate the account from the token
        /// </summary>
        /// <returns>The status response</returns>
        [HttpDelete]
        [Authorize(Policy = "Creator")]
        public async Task<ActionResult<ResponseDTO>> DeactivateAccount()
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
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
        public async Task<ActionResult<ResponseDTO>> DeactivateAccountAsManager(int id, string message)
        {
            return await _accountRepository.DeactivateAccount(id, message)
                ? Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account deactivated"
                })
                : NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsError = true,
                    Message = "Account doesn't exist"
                });
        }

        /// <summary>
        /// Update the account profile
        /// </summary>
        /// <param name="dto">the account update info</param>
        /// <returns>The status response</returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> UpdateAccount(AccountUpdateDTO dto)
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
                Message = "Invalid token"
            });
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;
            var dao = _mapper.Map<UpdateAccountDAO>(dto);
            dao.AccountId = accountId;
            return await _accountRepository.UpdateAccount(dao) < 0
                ? badResponse
                : Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account updated"
                });
        }

        /// <summary>
        /// Create a new reset code
        /// </summary>
        /// <param name="email">the account email</param>
        /// <returns>the status response</returns>
        [HttpPost("Reset/Create")]
        public async Task<ActionResult<ResponseDTO>> CreateNewResetCode(string email)
        {
            var account = await _accountRepository.GetAccountByEmail(email);
            if (account == null) 
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsError = true,
                    Message = "Account doesn't exist"
                });
            
            var resetCode = _accountRepository.CreateNewResetCode(account.AccountId);
            await _mailService.SendEmail(new EmailContent
            {
                ToEmail = email,
                Value = $"You have requested a new reset code for your account. The code is {resetCode}"
            });
            return Created(nameof(ResetAccount), new ResponseDTO
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Reset code created"
            });
        }

        /// <summary>
        /// Reset the account
        /// </summary>
        /// <param name="dto">the account reset info</param>
        /// <returns>the status response</returns>
        [HttpPost("Reset")]
        public async Task<ActionResult<ResponseDTO>> ResetAccount(AccountResetDTO dto)
        {
            var account = await _accountRepository.GetAccountByEmail(dto.Email);
            if (account == null)
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = -1,
                    Message = "Account doesn't exist"
                });

            bool success = await _accountRepository.ResetAccount(account.AccountId, dto.ResetCode, dto.Password);
            if (!success)
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorCode = 1,
                    Message = "Failed to reset the account. Maybe the reset code is incorrect"
                });
            else
            {
                await _mailService.SendEmail(new EmailContent
                {
                    ToEmail = dto.Email, Value = "Your account password was reset."
                });
                return Ok(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Account reset"
                });
            }
        }
    }
}
