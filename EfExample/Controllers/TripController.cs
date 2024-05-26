using EfExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trips.Context;
using Trips.DTOs;
using Trips.Models;

namespace EfExample.Controllers;

[ApiController]
[Route("/api/trips")]
public class TripController : ControllerBase

{
    private readonly ITripService _tripService;
    
    private readonly MsdbContext _context;

    public TripController(MsdbContext context, ITripService tripService)
    {
        _context = context;
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync()
    {
        var trips = await _tripService.GetTripsAsync();

        return Ok(trips);
    }

    [HttpPost]
    public async Task<IActionResult> AddTripAsync(CreateTripDTO tripDTO)
    {
        var trip = new Trip()
        {
            Name = tripDTO.Name,
            Description = tripDTO.Description,
            DateFrom = tripDTO.DateFrom,
            DateTo = tripDTO.DateTo,
            MaxPeople = tripDTO.MaxPeople
        };
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete("api/clients/{idClient}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        var res = await _tripService.DeleteClientAsync();
        if (res == 0)
        {
            return NoContent();
        } else if (res == 1)
        {
            return NotFound(new { Message = "Client not found" });
        } else
            return BadRequest(new { Message = "Client has assigned trips and cannot be deleted" });
    }
    
    [HttpPost("api/trips/{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTripAsync(int idTrip, [FromBody] AssignClientToTripDTO dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound(new { Message = "Trip not found" });
        }
        
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (client == null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
        
        var existingClientTrip = await _context.ClientTrips
            .FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
        if (existingClientTrip != null)
        {
            return BadRequest(new { Message = "Client is already assigned to this trip" });
        }
        
        var clientTrip = new
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = null
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    
}