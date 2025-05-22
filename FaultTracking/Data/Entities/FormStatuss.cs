namespace FaultTracking.Data.Entities
{
    public class FormStatuss
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public ICollection<Forms> Forms { get; set; }

    }
}
