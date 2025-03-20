using System.Text.Json.Serialization;

namespace WebApplication2.Models
{
    public class Role
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string Name { get; set; } = string.Empty;
    }
}
