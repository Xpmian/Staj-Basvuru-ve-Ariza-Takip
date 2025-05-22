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

        public List<PastInternShipViews> PastInternShip(string number);
        public List<Students> InformationByStudentNumber(string number);
    }
}
