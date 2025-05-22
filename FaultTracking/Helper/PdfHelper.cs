using FaultTracking.Data.Context;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using FaultTracking.Data.Entities;
using System.Reflection.Metadata;
using Document = QuestPDF.Fluent.Document;


namespace FaultTracking.Helper 
{
    public class PdfHelper : IPdfHelper
    {
        private readonly DatabaseContext _context;

        public PdfHelper(DatabaseContext context)
        {
            _context = context;
        }
        public FormStatusViews? GetFaults(int id)
        {
            return _context.FormStatusView.FirstOrDefault(p => p.Id == id);
        }
        public Document GetPdf(int id)
        {
            var faults = GetFaults(id);
            if (faults is null)
                throw new InvalidOperationException("Arıza Bulunamadi");

            var doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(TextStyle.Default.FontFamily("Times New Roman"));
                    page.Header().Row(row =>
                    {
                        // row.AutoItem().Image(logoUrl).WithCompressionQuality(ImageCompressionQuality.Best);
                        row.AutoItem().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("T.C. BOLU ABANT İZZET BAYSAL ÜNİVERSİTESİ\n").FontSize(9).Bold();
                            text.Span("Mühendislik Fakültesi\n").FontSize(9).Bold();
                            text.Span("Bilgisayar Mühendisliği Bölüm Başkanlığı").FontSize(9).Bold();
                        });
                        // row.AutoItem().Image(engineeLogorUrl).WithCompressionQuality(ImageCompressionQuality.Best);
                    });

                    page.Content().Column(column =>
                    {
                        column.Item().Text(text =>
                        {
                            text.Span("Arıza Formu").FontSize(10).Bold();
                            text.AlignCenter();
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Arıza Türü");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Type); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Arıza Yeri");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Field); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Arıza Başlığı");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Title); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Arıza Açıklaması");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Description); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Görevli Kişi");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Name + faults.Surname); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Öğrenci No");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.studentNumber); });
                        });
                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Oluşturulma Tarihi");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.Date); });
                        });

                        column.Item().Row(row =>
                        {
                            row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                            {
                                text.Span(" Bitiş Tarihi");
                            });
                            row.AutoItem().Width(325).Border(1).Text(text => { text.Span(faults.CompletionDate); });
                        });
                    });

                });
            });
            doc.GeneratePdf($"{faults.Title}.pdf");
            return doc;
        }
    }
}
