#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SongReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongReport>>> GetSongReports()
        {
            return await _context.SongReports.ToListAsync();
        }

        // GET: api/SongReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongReport>> GetSongReport(int id)
        {
            var songReport = await _context.SongReports.FindAsync(id);

            if (songReport == null)
            {
                return NotFound();
            }

            return songReport;
        }

        // PUT: api/SongReports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSongReport(int id, SongReport songReport)
        {
            if (id != songReport.ReportId)
            {
                return BadRequest();
            }

            _context.Entry(songReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SongReports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SongReport>> PostSongReport(SongReport songReport)
        {
            _context.SongReports.Add(songReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSongReport", new { id = songReport.ReportId }, songReport);
        }

        // DELETE: api/SongReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSongReport(int id)
        {
            var songReport = await _context.SongReports.FindAsync(id);
            if (songReport == null)
            {
                return NotFound();
            }

            _context.SongReports.Remove(songReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongReportExists(int id)
        {
            return _context.SongReports.Any(e => e.ReportId == id);
        }
    }
}
