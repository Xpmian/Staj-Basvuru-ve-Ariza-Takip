using System.ComponentModel.DataAnnotations;

namespace FaultTracking.Models
{
    public class FormModel
    {

        [Required(ErrorMessage = "Arıza yeri kısmı boş bırakılamaz!")]
        [Display(Name = "Field")]
        public string Field { get; set; }

        [Required(ErrorMessage = "Başlık boş bırakılamaz!")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz!")]
        [Display(Name = "FaultType")]
        public string FaultType { get; set; }

        [Required(ErrorMessage = "Bu alan boş bırakılamaz!")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "FaultTypeId")]
        public int FaultTypeId { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "studentNumber")]
        public string? StudentNumber { get; set; }
    }
}
