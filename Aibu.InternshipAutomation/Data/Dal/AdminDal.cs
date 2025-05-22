using Aibu.InternshipAutomation.Data.Base;
using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using Aibu.InternshipAutomation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace Aibu.InternshipAutomation.Data.Dal
{
    public class AdminDal : IAdminDal
    {
        private readonly DatabaseContext _databaseContext;

        public AdminDal(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<CompanyInfoViews> GetAll()
        {
            return _databaseContext.CompanyInfoView.ToList();
        }

        public CompanyUserss GetAllInfo(string companyName)
        {
            var company = _databaseContext.CompanyUsers.FirstOrDefault(x => x.CompanyName == companyName);
            if (company is null)
                throw new InvalidOperationException("Company bulunamadi");
            return company;
        }

        public AuthorizedPersons? UpdateAuthorizedPerson(int roleId, string name, string surname, string email , int deparmentId)
        {
            var existingAuthorizedPersons = _databaseContext.AuthorizedDepartments.FirstOrDefault(p => p.RoleId == roleId && p.DepartmentId == deparmentId);
            var existingAuthorizedPersons2 = _databaseContext.AuthorizedPerson.Find(existingAuthorizedPersons.AuthorizedPersonId);

            existingAuthorizedPersons2.Name = name;
            existingAuthorizedPersons2.Surname = surname;
            existingAuthorizedPersons2.Email = email;
            

            _databaseContext.AuthorizedPerson.Update(existingAuthorizedPersons2);
            _databaseContext.SaveChanges();

            return existingAuthorizedPersons2;
        }

        public void UploadInfoAuthorized(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                try
                {
                    using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        _databaseContext.AuthorizedUbys.RemoveRange(_databaseContext.AuthorizedUbys);
                        _databaseContext.SaveChanges();

                        for (int row = 3; row <= rowCount; row++)
                        {
                            string nameSurname = worksheet.Cells[row, 3].Value?.ToString();
                            string email = worksheet.Cells[row, 7].Value?.ToString();
                            if (!nameSurname.IsNullOrEmpty() || !email.IsNullOrEmpty())
                            {
                                (string ad, string soyad) = SplitLastWord(nameSurname);

                                var authorizedPerson = new AuthorizedUbyss { Name = ad, Surname = soyad, Email = email };
                                _databaseContext.AuthorizedUbys.Add(authorizedPerson);
                            }
                        }
                        _databaseContext.SaveChanges();

                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Excel dosyasi islenirken hata olustu.");
                }
            }
        }

        public void UploadInfoStudent(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                try
                {
                    using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        _databaseContext.Ubys.RemoveRange(_databaseContext.Ubys);
                        _databaseContext.SaveChanges();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string studentNo = worksheet.Cells[row, 1].Value?.ToString();
                            string name = worksheet.Cells[row, 2].Value?.ToString();
                            string surname = worksheet.Cells[row, 3].Value?.ToString();
                            string className = worksheet.Cells[row, 6].Value?.ToString() ?? "";
                            string departmentName = worksheet.Cells[row, 7].Value?.ToString();

                            string searchString = " Bölümü";
                            int index = departmentName.IndexOf(searchString);
                            string result = departmentName.Substring(0, index);

                            var department = _databaseContext.Department.FirstOrDefault(d => d.Name == result);

                            if (department == null)
                            {
                                throw new Exception($"Departman '{result}' bulunamadı");
                            }

                            var student = new Ubyss { Number = studentNo, Name = name, Surname = surname, Class = className, Department = department };
                            _databaseContext.Ubys.Add(student);
                        }
                        _databaseContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Excel dosyasi islenirken hata olustu.");
                }
            }
        }

        public static (string remainingText, string lastWord) SplitLastWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return (string.Empty, string.Empty);
            }
            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {

                return (string.Empty, words[0]);
            }

            string lastWord = words[words.Length - 1];

            string remainingText = string.Join(" ", words, 0, words.Length - 1);

            return (remainingText, lastWord);
        }
    }
}
