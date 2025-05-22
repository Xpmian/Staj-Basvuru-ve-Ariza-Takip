using Aibu.InternshipAutomation.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Aibu.InternshipAutomation.Helper
{
    public interface IPdfHelper
    {
        Document GetPdf(int id , string role);
        Students? GetStudent(int id);
    }
}
