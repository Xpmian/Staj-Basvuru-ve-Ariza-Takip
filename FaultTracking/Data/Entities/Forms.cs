using System.ComponentModel.DataAnnotations;

namespace FaultTracking.Data.Entities
{
    public class Forms
    {
        [Key]

        public int Id { get; set; }

        public string Field { get; set; }

        public string Title { get; set; }
        public int FaultTypeId { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public int FormStatusId { get; set; }
        public int? ColourId { get; set; } 
        public int? AuthorizedPersonId { get; set; }

        public string? UserMail { get; set; }
        public string? studentNumber { get; set; }
        public string? CompletionDate { get; set; }
        public FormStatuss FormStatus { get; set; }
        public FaultTypes FaultType { get; set; }
        public AuthorizedPersons AuthorizedPerson { get; set; }
        public Colours Colour { get; set; }
    }
}
