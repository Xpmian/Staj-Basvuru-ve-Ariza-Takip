using System.ComponentModel.DataAnnotations;

namespace Aibu.InternshipAutomation.Models
{
    public class StudentModel
    {

        [RegularExpression(@"^\d{9}$", ErrorMessage = "Lütfen 9 haneli bir sayı girin.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Adınızı Giriniz")]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soyadınızı Giriniz")]
        [Display(Name = "SurName")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Sınıfınızı Giriniz")]
        [Display(Name = "Class")]
        public short Class { get; set; }


        [RegularExpression(@"^\d{11}$", ErrorMessage = "Lütfen 11 haneli bir sayı girin.")]
        [Display(Name = "IdentificationNumber")]
        public string IdentificationNumber { get; set; }


        [Required(ErrorMessage = "Email Adresinizi Giriniz")]
        [Display(Name = "StudentEmail")]
        [EmailAddress(ErrorMessage = "Geçersiz Email Adresi!")]
        public string StudentEmail { get; set; }


        [Required(ErrorMessage = "Staj Yapacağınız Kurumun İsmini Giriniz")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Staj Yapacağınız Kurumun Email Adresini Giriniz")]
        [Display(Name = "CompanyEmail")]
        [EmailAddress(ErrorMessage = "Geçersiz Email Adresi!")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "Adresinizi Girin")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Lütfen 11 haneli telefon numaranızı giriniz.Fazla veya eksik rakam girdiniz!")]
        [Display(Name = "TelephoneNumber")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Doğum Tarihinizi Giriniz")]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Tarihleri Seçiniz")]
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "MotherName")]
        public string MotherName { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "FatherName")]
        public string FatherName { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "InternType")]
        public int InternTypesId { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "IntershipStartDate")]
        public DateTime IntershipStartDate { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "IntershipEndDate")]
        public DateTime IntershipEndDate { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "InternPeriodId")]
        public int InternPeriodId { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "PlaceOfBirthId")]
        public int PlaceOfBirthId { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "DepartmentId")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "StateId")]
        public int StateId { get; set; }


        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "AcceptanceStatusId")]
        public int AcceptanceStatusId { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "ImageData")]
        public IFormFile ImageData { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        [Display(Name = "ImageMimeType")]
        public string ImageMimeType { get; set; }

        [Required(ErrorMessage = "İSG Belgenizi Yükleyin")]
        [Display(Name = "Isg")]
        public IFormFile Isg { get; set; }

        [Required(ErrorMessage = "Provüzyon Belgenizi Yükleyin.")]
        [Display(Name = "Provuzyon")]
        public IFormFile Provuzyon { get; set; }

        [Display(Name = "InTermFile")]
        public IFormFile? InTermFile { get; set; }

    }
}