#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;
using AutoMapper;
using _4kTiles_Backend.DataObjects.DAO.Report;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.Report;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongReportsController : ControllerBase
    {
        private readonly ISongReportRepository _songReportRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;

        public SongReportsController(ISongReportRepository songReportRepository, IMapper mapper, IAccountRepository accountRepository, ISongRepository songRepository)
        {
            _songRepository = songRepository;
            _accountRepository = accountRepository;
            _songReportRepository = songReportRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Song Report List
        /// </summary>
        /// <param name="reportFilter">ReportFilter</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PaginationResponseDTO<SongReportDTO>> GetSongReportList(ReportFilter reportFilter, [FromQuery] PaginationParameter pagination)
        {
            var reportList = await _songReportRepository.GetListReport(reportFilter, pagination);
            return new PaginationResponseDTO<SongReportDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get song report list",
                TotalRecords = reportList.TotalRecords,
                Data = _mapper.Map<IEnumerable<SongReportDTO>>(reportList.Payload)
            };
        }

        /// <summary>
        /// Get Song Report By ID
        /// </summary>
        /// <param name="id">Report id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<SongReportDTO>>> GetSongReport(int id)
        {
            var songReport = await _songReportRepository.GetReportByID(id);

            if (songReport == null)
            {
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsError = true,
                    Message = "Report does not exist!"
                });
            }
            var dto = _mapper.Map<SongReportDTO>(songReport);
            return Ok(new ResponseDTO<SongReportDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Report exist!",
                Data = dto
            });
        }

        /// <summary>
        /// Create new Report
        /// </summary>
        /// <param name="report">Report DTO</param>
        /// <returns>Success message</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseDTO>> CreateReport(CreateReportDTO report)
        {
            // check if Song not exists
            if (await _songRepository.GetSongByID(report.SongId) == null)
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = -1,
                    Message = "The Song does not exist!"
                });
            // check if Account not exists
            if (await _accountRepository.GetAccountById(report.AccountId) == null)
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = -2,
                    Message = "The Account does not exist!"
                });
            var dao = _mapper.Map<CreateReportDAO>(report);

            // save
            await _songReportRepository.CreateReport(dao);
            return Created("success", new ResponseDTO
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Report created"
            });

        }

        /// <summary>
        /// Change Report Status
        /// </summary>
        /// <param name="id">Report id</param>
        /// <param name="status">Report status</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDTO>> ChangeReportStatus(int id, bool status)
        {
            // check if Report not exists
            if (await _songReportRepository.ChangeReportStatus(id, status) < 0)
                return NotFound(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorCode = -1,
                    Message = "The Report does not exist!"
                });
            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Change Complete!"
            });
        }
    }
}
