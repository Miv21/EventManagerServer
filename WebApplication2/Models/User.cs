using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
    }
}
