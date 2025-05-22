using FaultTracking.Data.Entities;

namespace FaultTracking.Data.Base
{
    public interface IFormDal
    {
        public List<FormStatusViews> GetAll();
        public List<FormStatusViews> CompletedFault();
        public List<FormStatusViews> WaitingFault();
        public List<FormStatusViews> AppointedFault();
        public Forms? GetFormtById(int id);
        public Forms? Add(Forms forms);
        public Forms? Delete(int id);
        public Forms? UpdateApprovalStatus(int id, int employeeId, int colourId);
        public Forms? UpdateApprovalAttendantStatus(int id);
        public List<FaultTypes> GetAllFaultTypes();
        public List<AuthorizedPersons> GetAuthorizedPerson();
        public string GetStudent(string studentNumber);
        public List<FormStatusViews>? ContinueprocessOwner(string userMail);
        public List<FormStatusViews>? CompletedprocessOwner(string userMail);

        public List<FormStatusViews> PersonelAppointedFaultList(string personelMail);
        public List<FormStatusViews> PersonelCompaletedFaultList(string personelMail);
        public List<RoleViews> RoleFaultList(string roleMail);
        public List<Colours> GetAllColourCode();
        public async Task SendMailAsync(string userEmail, string mailBody, string subject) { }
        public List<FormStatusViews> ColourFilter(int colourId);
        public List<FormStatusViews> EmployeeFilter(int employeeId);
        public List<FormStatusViews> DescriptionFilter(string description);
        public List<FormStatusViews> TitleFilter(string title);
        public List<FormStatusViews> FieldFilter(string field);

        public void UploadInfoStudent(IFormFile excelFile);

        public void UploadInfoAuthorized(IFormFile excelFile);
    }
}
