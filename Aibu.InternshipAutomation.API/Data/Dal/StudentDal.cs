using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class StudentDal : IStudentDal
    {
        
        private readonly DatabaseContext _databaseContext;

        public StudentDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<Students> GetAll()
        {
             return _databaseContext.Student.ToList();
            
        }

        private bool CheckInternshipExist(string number, int internType)
        {
            return _databaseContext.Student.FirstOrDefault(student =>string.Equals(number, student.Number) && student.InternPeriodId == internType) != null;
        }

        public Students? GetStudentById(int id)
        {
            return _databaseContext.Student.FirstOrDefault(student => student.Id == id);
        }
        public Students? Add(Students student)
        {
            if (CheckInternshipExist(student.Number, student.InternPeriodId))
            {
                throw new InvalidOperationException("Staja daha once kayit olundu");
            }      
            var entity = _databaseContext.Student.Add(student);
            return entity.Entity;
        }
        public Students? Delete(int id)
        {
            try
            {
                var student = GetStudentById(id);
                if (student is null)
                {
                    throw new InvalidOperationException("Staj verisi bulunamadi");
                }          
                var entity = _databaseContext.Student.Remove(student);
                return entity.Entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Students? UpdateAcceptanceStatus(Students student)
        {
            var existingStudent = _databaseContext.Student.Find(student.Id);
            if (existingStudent != null)
            {
                existingStudent.AcceptanceStatus = student.AcceptanceStatus;
                _databaseContext.SaveChanges();

                return existingStudent;
            }
            return null;
        }

        public List<PastInternShipViews> GetAllView()
        {
            return _databaseContext.PastInternShipView.ToList();
        }
        public int DateComparison(DateTime InternShipEndDate)
        {
            DateTime utcDateTime = DateTime.UtcNow;
            // InternShipEndDate, utcDateTime'den önceyse sonuç negatif
            // InternShipEndDate ve utcDateTime aynıysa sonuç 0
            // InternShipEndDate, utcDateTime'den sonraysa sonuç pozitif
            int result = InternShipEndDate.CompareTo(utcDateTime);
            return result;
        }
        public List<PastInternShipViews> PastInternShip(string number)
        {
            var a = _databaseContext.PastInternShipView.Where(p => p.Number.Equals(number));           
            List<PastInternShipViews> list = new List<PastInternShipViews>();

            foreach (var item in a)
            {
                int temp = DateComparison(item.IntershipEndDate);
                int temp2 = item.AcceptanceStatus;
                if (temp < 0 && temp2 == 1)
                {
                    list.Add(item);
                }
            }
            return list;           
        }
        public List<Students> InformationByStudentNumber(string number)
        {
            var list = _databaseContext.Student.Where(p => p.Number.Contains(number)).ToList();
            return list;
        }
    }
}

