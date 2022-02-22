using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DAO.Report;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.DataObjects.DTO.Report;
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
        Task<SongReportDAO?> GetReportByID(int id);
        Task<int> CreateReport(CreateReportDAO reportDAO);
        Task<int> ChangeReportStatus(int id, bool status);
        Task<PaginationResponse<SongReportDAO>> GetListReport(ReportFilter filter, PaginationParameter pagination);
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

        /// <summary>
        /// Get Report By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongReportDAO?> GetReportByID(int id)
        {
            var report = await _dbContext.SongReports.Where(s => s.ReportId == id).FirstOrDefaultAsync();
            return _mapper.Map<SongReportDAO>(report);
        }

        public async Task<int> CreateReport(CreateReportDAO reportDAO)
        {
            SongReport newReport = _mapper.Map<SongReport>(reportDAO);
            int rowInsert = 0;
            _dbContext.SongReports.Add(newReport);
            rowInsert = await _dbContext.SaveChangesAsync();
            return rowInsert;           

        }

        public async Task<int> ChangeReportStatus(int id, bool status)
        {
            var getReport = await _dbContext.SongReports.Where(s => s.ReportId == id).FirstOrDefaultAsync();

            if (getReport != null)
            {
                var tagUpdate = 0;
                getReport.ReportStatus = status;
                tagUpdate = await _dbContext.SaveChangesAsync();
                return tagUpdate;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Get List Report
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<PaginationResponse<SongReportDAO>> GetListReport(ReportFilter filter, PaginationParameter pagination)
        {
            var query = _dbContext.SongReports.AsQueryable();
            
            //Filter By Title
            if(filter.Title != null)
            {
                query = query.Where(r => r.ReportTitle.ToLower().Contains(filter.Title.ToLower()));
            }
            
            //Filter By Status
            if (filter.Status == "Accept")
            {
                query = query.Where(r => r.ReportStatus == true);
            }
            else if (filter.Status == "Reject")
            {
                query = query.Where(r => r.ReportStatus == false);
            }
            else if (filter.Status == "Pending")
            {
                query = query.Where(r => r.ReportStatus == null);
            }
            //Sort date
            query = query.OrderByDescending(r => r.ReportDate);
            var list = await query
                .GetCount(out var count)
                .GetPage(pagination)
                .ToListAsync();
            
            return new PaginationResponse<SongReportDAO>
            {
                TotalRecords = count, Payload = _mapper.Map<List<SongReportDAO>>(list)
            };
        }
    }
}