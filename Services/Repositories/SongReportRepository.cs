using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DAO.Report;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    /// <summary>
    /// SongReport repository interface
    /// </summary>
    public interface ISongReportRepository
    {
        Task<SongReport?> GetReportByID(int id);
        Task<int> CreateReport(CreateReportDAO reportDAO);
        Task<int> AcceptReport(int id, SongReport report);
        Task<int> RejectReport(int id, SongReport report);
    }

    public class SongReportRepository : ISongReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// SongReportRepository constructor
        /// </summary>
        /// <param name="dbContext">App db context</param>
        /// <param name="mapper">mapper</param>
        /// <param name="accountRepository"></param>
        public SongReportRepository(ApplicationDbContext dbContext, IMapper mapper, IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }


        public async Task<SongReport?> GetReportByID(int id)
        {
            return await _dbContext.SongReports.Where(s => s.ReportId == id).FirstOrDefaultAsync();
        }

        public async Task<int> CreateReport(CreateReportDAO reportDAO)
        {
            SongReport newReport = _mapper.Map<SongReport>(reportDAO);
            int rowInsert = 0;
            _dbContext.SongReports.Add(newReport);
            rowInsert = await _dbContext.SaveChangesAsync();
            return rowInsert;           

        }

        public async Task<int> AcceptReport(int id, SongReport report)
        {
            var getReport = await _dbContext.SongReports.Where(s => s.ReportId == id).FirstOrDefaultAsync();
            var tagUpdate = 0;
            try
            {
                if (getReport != null)
                {
                    getReport.ReportStatus = report.ReportStatus;
                    tagUpdate = await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Report not exist!");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return tagUpdate;
        }

        public async Task<int> RejectReport(int id, SongReport report)
        {
            var getReport = await _dbContext.SongReports.Where(s => s.ReportId == id).FirstOrDefaultAsync();
            var tagUpdate = 0;
            try
            {
                if (getReport != null)
                {
                    getReport.ReportStatus = report.ReportStatus;
                    tagUpdate = await _dbContext.SaveChangesAsync();
                }
                else
                {
                    getReport.ReportStatus = report.ReportStatus;
                    tagUpdate = await _dbContext.Remove(report);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return tagUpdate;
        }
    }
}