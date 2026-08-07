using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? MedicalRecordNumber { get; set; }
        public string Nik { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK to ApplicationUser
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}