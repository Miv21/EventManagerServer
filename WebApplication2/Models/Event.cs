using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("User")]
        public int CreatedBy { get; set; }
        public User? User { get; set; }

        public ICollection<User> Participants { get; set; } = new List<User>();
    }
}
