namespace HealthDeskAPI.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
    }
}