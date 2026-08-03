using HealthDeskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDeskAPI.Services;

public class QueueNumberGenerator(HealthDeskApiContext context, ILogger<QueueNumberGenerator>  logger)
{
    private static readonly SemaphoreSlim Lock = new(1, 1);

    public async Task<int> Generate(Appointment appointment)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        await Lock.WaitAsync();
        try
        {
            var counter = await context.Appointments.FirstOrDefaultAsync(q => q.AppointmentDate == today && q.DoctorId == appointment.DoctorId);

            if (counter == null)
            {
                return 1;
            }

            counter.QueueNumber++;
            return counter.QueueNumber;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw;
        }
        finally
        {
            Lock.Release();
        }
    }
}