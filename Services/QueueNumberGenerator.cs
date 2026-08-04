using HealthDeskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDeskAPI.Services;

public class QueueNumberGenerator
{
    private readonly HealthDeskApiContext _context;
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    
    public QueueNumberGenerator(HealthDeskApiContext context)
    {
        _context = context;
    }

    public async Task<int> Generate(Appointment appointment)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        await _lock.WaitAsync();
        try
        {
            var counter = await _context.Appointments.FirstOrDefaultAsync(q => q.AppointmentDate == today && q.DoctorId == appointment.DoctorId);

            if (counter == null)
            {
                return 1;
            }

            return counter.QueueNumber++;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _lock.Release();
        }
    }
}