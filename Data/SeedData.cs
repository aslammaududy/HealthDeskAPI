using HealthDeskAPI.Models;
using HealthDeskAPI.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthDeskAPI.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<HealthDeskApiContext>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        // Seed roles
        string[] roles = { "Superadmin", "Registration", "Patient" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    logger.LogError("Failed to create role '{Role}': {Errors}", role, errors);
                }
            }
        }

        // Seed admin user if admin does not exist
        if (!await userManager.Users.AnyAsync(u => u.Email == "admin@healthdesk.com"))
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@healthdesk.com",
                Email = "admin@healthdesk.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Superadmin");
                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    logger.LogError("Failed to assign 'Superadmin' role to admin user: {Errors}", errors);
                }
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to create admin user: {Errors}", errors);
            }
        }

        // Seed Registration users
        if (!await userManager.Users.AnyAsync(u => u.Email == "registration1@healthdesk.com"))
        {
            var regUser1 = new ApplicationUser
            {
                UserName = "registration1@healthdesk.com",
                Email = "registration1@healthdesk.com",
                FirstName = "Staff",
                LastName = "Registration1",
                EmailConfirmed = true
            };

            var result1 = await userManager.CreateAsync(regUser1, "Reg@123");
            if (result1.Succeeded)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(regUser1, "Registration");
                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    logger.LogError("Failed to assign 'Registration' role to registration1: {Errors}", errors);
                }
            }
            else
            {
                var errors = string.Join(", ", result1.Errors.Select(e => e.Description));
                logger.LogError("Failed to create registration1 user: {Errors}", errors);
            }
        }

        if (!await userManager.Users.AnyAsync(u => u.Email == "registration2@healthdesk.com"))
        {
            var regUser2 = new ApplicationUser
            {
                UserName = "registration2@healthdesk.com",
                Email = "registration2@healthdesk.com",
                FirstName = "Staff",
                LastName = "Registration2",
                EmailConfirmed = true
            };

            var result2 = await userManager.CreateAsync(regUser2, "Reg@123");
            if (result2.Succeeded)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(regUser2, "Registration");
                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    logger.LogError("Failed to assign 'Registration' role to registration2: {Errors}", errors);
                }
            }
            else
            {
                var errors = string.Join(", ", result2.Errors.Select(e => e.Description));
                logger.LogError("Failed to create registration2 user: {Errors}", errors);
            }
        }

        // Seed Specializations
        if (!context.Specializations.Any())
        {
            context.Specializations.AddRange(
                new Specialization { Code = "GEN", Name = "General Practice" },
                new Specialization { Code = "CAR", Name = "Cardiology" },
                new Specialization { Code = "DER", Name = "Dermatology" },
                new Specialization { Code = "PED", Name = "Pediatrics" },
                new Specialization { Code = "ORT", Name = "Orthopedics" }
            );
            await context.SaveChangesAsync();
        }

        // Seed Doctors
        if (!context.Doctors.Any())
        {
            var specMap = context.Specializations.ToDictionary(s => s.Code, s => s.Id);

            context.Doctors.AddRange(
                new Doctor { FullName = "Dr. Aditya Pratama", SpecializationId = specMap["GEN"], IsActive = true },
                new Doctor { FullName = "Dr. Siti Nurhaliza", SpecializationId = specMap["CAR"], IsActive = true },
                new Doctor { FullName = "Dr. Budi Santoso", SpecializationId = specMap["DER"], IsActive = true },
                new Doctor { FullName = "Dr. Dewi Lestari", SpecializationId = specMap["PED"], IsActive = true }
            );
            await context.SaveChangesAsync();
        }

        // Seed Patient users and Patient records
        var patientData = new[]
        {
            new { Email = "patient1@healthdesk.com", FirstName = "John", LastName = "Doe", Password = "Patient@123",
                   Nik = "3201234567890001", DOB = new DateOnly(1990, 1, 15), Gender = Gender.Male, Phone = "081234567890", Address = "123 Main St" },
            new { Email = "patient2@healthdesk.com", FirstName = "Jane", LastName = "Smith", Password = "Patient@123",
                   Nik = "3201234567890002", DOB = new DateOnly(1985, 6, 20), Gender = Gender.Female, Phone = "081234567891", Address = "456 Oak Ave" },
            new { Email = "patient3@healthdesk.com", FirstName = "Bob", LastName = "Wilson", Password = "Patient@123",
                   Nik = "3201234567890003", DOB = new DateOnly(1978, 11, 30), Gender = Gender.Male, Phone = "081234567892", Address = "789 Pine Rd" }
        };

        foreach (var p in patientData)
        {
            if (!await userManager.Users.AnyAsync(u => u.Email == p.Email))
            {
                var patientUser = new ApplicationUser
                {
                    UserName = p.Email,
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(patientUser, p.Password);
                if (result.Succeeded)
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(patientUser, "Patient");
                    if (!addToRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                        logger.LogError("Failed to assign 'Patient' role to {Email}: {Errors}", p.Email, errors);
                    }

                    var patient = new Patient
                    {
                        Nik = p.Nik,
                        FullName = $"{p.FirstName} {p.LastName}",
                        DateOfBirth = p.DOB,
                        Gender = p.Gender,
                        PhoneNumber = p.Phone,
                        Address = p.Address,
                        UserId = patientUser.Id
                    };

                    using var transaction = await context.Database.BeginTransactionAsync();
                    try
                    {
                        context.Patients.Add(patient);
                        await context.SaveChangesAsync();

                        patient.MedicalRecordNumber = patient.Id.ToString("D6");
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError("Failed to create patient user {Email}: {Errors}", p.Email, errors);
                }
            }
        }

        // Seed Schedules (Monday–Saturday, morning + afternoon for each doctor)
        if (!context.Schedules.Any())
        {
            var days = new[]
            {
                System.DayOfWeek.Monday,
                System.DayOfWeek.Tuesday,
                System.DayOfWeek.Wednesday,
                System.DayOfWeek.Thursday,
                System.DayOfWeek.Friday,
                System.DayOfWeek.Saturday
            };

            var doctors = await context.Doctors.ToListAsync();

            var schedules = new List<Schedule>();

            foreach (var doctor in doctors)
            {
                foreach (var day in days)
                {
                    schedules.Add(new Schedule
                    {
                        DoctorId = doctor.Id,
                        DayOfWeek = day,
                        StartTime = new TimeOnly(8, 0),
                        EndTime = new TimeOnly(12, 0),
                        MaxQuota = 20
                    });

                    schedules.Add(new Schedule
                    {
                        DoctorId = doctor.Id,
                        DayOfWeek = day,
                        StartTime = new TimeOnly(13, 0),
                        EndTime = new TimeOnly(17, 0),
                        MaxQuota = 15
                    });
                }
            }

            await context.Schedules.AddRangeAsync(schedules);
            await context.SaveChangesAsync();
        }
    }
}
