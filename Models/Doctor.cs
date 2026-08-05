namespace HealthDeskAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int SpecializationId { get; set; }
        public Specialization? Specialization {get; set; }
        public bool IsActive { get; set; }
    }
}