using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class UserEvent
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}
