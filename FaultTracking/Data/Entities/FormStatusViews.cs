using System.ComponentModel.DataAnnotations;

namespace FaultTracking.Data.Entities
{
    public class FormStatusViews
    {
        public int Id { get; set; }

        public string Field { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public int FormStatusId { get; set; }
        public int? AuthorizedPersonId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? UserMail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? studentNumber { get; set; } = string.Empty;
        public string? CompletionDate { get; set; } 
        public string? ColourCode { get; set; } = string.Empty;
        public int? ColourId { get; set; }
        public string? ColorMeaning { get; set; } = string.Empty;
    }
}
