using Microsoft.EntityFrameworkCore;
using Trips.Context;
using Trips.Models;

namespace EfExample.Services;

public interface ITripService
{
    public Task<IEnumerable<Trip>> GetTripsAsync();
    public Task<int?> DeleteClientAsync();
}
public class TripService : ITripService
{
    private readonly MsdbContext _context;
    public TripService(MsdbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trip>> GetTripsAsync()
    {
        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync();
        return trips;
    }

    public async Task<int?> DeleteClientAsync()
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            return 1;
        }

        if (client.ClientTrips.Any())
        {
            return 2;
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return 0;
    }
}