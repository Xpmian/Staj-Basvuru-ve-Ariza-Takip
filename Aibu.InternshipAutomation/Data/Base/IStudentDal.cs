using Aibu.InternshipAutomation.Data.Entities;

namespace Aibu.InternshipAutomation.Data.Base
{
    public interface IStudentDal
    {
        List<Students> GetAll();
        public Students? Add(Students student);
        public Students? Delete(int id);
        public Students? UpdateAcceptanceStatus(Students student);
        List<PastInternShipViews> GetAllView();

        public Students? GetStudentById(int id);

        public List<PastInternShipViews> PastInternShip(string number);
        public List<Students> InformationByStudentNumber(string number);
        public string GetStudentByName(string name);
        public List<StateViews> GetAllState(string studentNumber);
        public Students UpdateAcceptState(int id, int stadeId);
        public Students? UpdateRejectState(int id, string description);

        public Students? RejectStudent(int id);

        public async Task SendMailAsync(string userEmail, string mailBody, string subject) { }
        public List<Students> GetAll(string number);
        public Boolean GetStatus(string number);
        public List<AuthorizedViews> GetStudentForPdf(string number);

        public List<AuthorizedViews> GetStudentContinuing(string number);
        public string ExtractStudentNumber(string email);
        public bool InternType1(string studentNumber);
        public bool InternType2(string studentNumber);
        }
}
