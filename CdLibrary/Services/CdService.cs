using CdLibrary.Data;
using CdLibrary.DTOs;

namespace CdLibrary.Services
{
    public class CdService
    {
        private readonly CdContext _context;

        public CdService(CdContext context)
        {
            _context = context;
        }

        public async Task<CdResponse?> GetCdByIdAsync(int id)
        {
            var cd = await _context.Cd.FindAsync(id);
            if (cd == null)
            {
                return null;
            }

            return new CdResponse(
                cd.Artist ?? string.Empty, 
                cd.Name ?? string.Empty, 
                cd.Description ?? string.Empty, 
                cd.Genre?.Name ?? string.Empty
            );
        }
    }
}
