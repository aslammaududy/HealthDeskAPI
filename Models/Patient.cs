using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public required string MedicalRecordNumber { get; set; }
        public required string Nik { get; set; }
        public required string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}