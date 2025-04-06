namespace WebApplication2.DtoModels
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public List<int> ParticipantsIds { get; set; }
    }

    public class UpdateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public List<int> ParticipantsIds { get; set; } = new List<int>();
    }
}
