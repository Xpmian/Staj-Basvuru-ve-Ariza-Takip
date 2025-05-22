using System.Reflection.Metadata;
using FaultTracking.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace FaultTracking.Helper
{
    public interface IPdfHelper
    {
        Document GetPdf(int id);
        public FormStatusViews? GetFaults(int id);
    }
}
