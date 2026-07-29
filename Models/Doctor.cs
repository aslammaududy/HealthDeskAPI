namespace HealthDeskAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public int? SpecializationId { get; set; }
        public Specialization? Specialization {get; set; }
        public bool IsActive { get; set; }
    }
}