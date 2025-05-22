using Aibu.InternshipAutomation.Data.Context;
using Aibu.InternshipAutomation.Data.Entities;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using Aibu.InternshipAutomation.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using QuestPDF.Drawing;
using QuestPDF.Helpers;

namespace Aibu.InternshipAutomation;

public class PdfHelper : IPdfHelper
{
    private readonly DatabaseContext _context;

    public PdfHelper(DatabaseContext context)
    {
        _context = context;
    }

    public Students? GetStudent(int id)
    {
        return _context.Student.FirstOrDefault(student => student.Id == id);
    }

    public AuthorizedPersonDepartmentViews? GetAuthorizedPerson(int roleId , int departmentId)
    {
        return _context.AuthorizedPersonDepartmentView.FirstOrDefault(person => person.RoleId == roleId && person.DepartmentId == departmentId);
    }

    public CompanyUserss? GetCompany(string name, string email)
    {
        return _context.CompanyUsers.FirstOrDefault(company => company.CompanyName == name && company.Email == email);
    }
    public Departments? GetDepartment(int id)
    {
        return _context.Department.FirstOrDefault(department => department.Id == id);
    }

    public InternTypes? GetInternType(int id)
    {
        return _context.InternTypes.FirstOrDefault(type => type.Id == id);
    }

    public InternPeriods? GetInternPeriods(int id)
    {
        return _context.InternPeriod.FirstOrDefault(period => period.Id == id);
    }

    public Cities? GetCity(int id)
    {
        return _context.City.FirstOrDefault(city => city.Id == id);
    }

    public Document GetPdf(int id, string? role)
    {
        var student = GetStudent(id);

        var company = GetCompany(student.CompanyName, student.CompanyEmail);

        var department = GetDepartment(student.DepartmentId);

        var internType = GetInternType(student.InternTypesId);

        var period = GetInternPeriods(student.InternPeriodId);

        var city = GetCity(student.PlaceOfBirthId);

        var bolumBaskanı = GetAuthorizedPerson(4,student.DepartmentId);
        var stajKomisyon = GetAuthorizedPerson(6,student.DepartmentId);
        var bolumSekreteri = GetAuthorizedPerson(8,student.DepartmentId);
        var fakulteSekreteri = GetAuthorizedPerson(7,student.DepartmentId);

        bool background = true;
        var studentDepartment = student.Department;

        switch (student.DepartmentId)
        {
            case 1 when company.IsComputerEngineer == false:
            case 2 when company.IsEnvironmentalEngineering == false:
            case 4 when company.IsFoodEngineering == false:
            case 5 when company.IsCivilEngineering == false:
            case 6 when company.IsChemicalEngineering == false:
            case 7 when company.IsMechanicalEngineering == false:
            case 8 when company.IsElectricalElectronicsEngineering == false:
                background = false;
                break;
        }

        var departmentTitles = new Dictionary<int, string>
        {
            { 1, "Bilgisayar Mühendisliği Bölüm Başkanlığına"},
            { 2, "Çevre Mühendisliği Bölüm Başkanlığına"},
            { 3, "Endüstri Mühendisliği Bölüm Başkanlığına"},
            { 4, "Gıda Mühendisliği Bölüm Başkanlığına"},
            { 5, "İnşaat Mühendisliği Bölüm Başkanlığına"},
            { 6, "Kimya Mühendisliği Bölüm Başkanlığına"},
            { 7, "Makine Mühendisliği Bölüm Başkanlığına"},
            { 8, "Elektrik Elektronik Mühendisliği Bölüm Başkanlığına"}
        };

        string studentPath = student.ImageData;
        DateTime dateTime = (DateTime)student.CreateTime;
        string dateOnly = dateTime.ToString("dd-MM-yyyy");

        DateTime dateTime2 = (DateTime)student.UpdateTime;
        string updateTime = dateTime2.ToString("dd-MM-yyyy");

        DateTime dateTime3 = DateTime.Now;
        string updateTime2 = dateTime3.ToString("dd-MM-yyyy");

        DateTime dateTime4 = (DateTime)student.BolumBaskanıUpdateTime;
        string updateTime4 = dateTime4.ToString("dd-MM-yyyy");

        DateTime dateTime5 = (DateTime)student.SirketUpdateTime;
        string updateTime5 = dateTime5.ToString("dd-MM-yyyy");

        DateTime dateTime6 = (DateTime)student.StajKomisyonUpdateTime;
        string updateTime6 = dateTime6.ToString("dd-MM-yyyy");

        DateTime dateTime7 = (DateTime)student.FakulteSekreteriUpdateTime;
        string updateTime7 = dateTime7.ToString("dd-MM-yyyy");

        DateTime dateTime8 = (DateTime)student.BolumSekreteriUpdateTime;
        string updateTime8 = dateTime8.ToString("dd-MM-yyyy");

        int fontSize = (student.Name + " " + student.Surname).Length > 25 ? 8 : 12;
        int fontSize1 = student.Address.Length > 10 ? 8 : 12;
        int fontSize2 = company.CompanyName.Length > 10 ? 7 : 12;
        int fontSize3 = company.Adress.Length > 10 ? 6 : 12;
        int fontSizeposta = company.Email.Length > 10 ? 10 : 12;
        int fontSizepostaogrenci = student.StudentEmail.Length > 10 ? 10 : 12;
        int fontSizeAcitiviyArea = company.ActivityArea.Length > 10 ? 7 : 12;
        int fontSizeMissionArea = company.MissionArea.Length > 10 ? 7 : 12;

        var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "Gwendolyn-Regular.ttf");

        using (var fontStream = File.OpenRead(fontPath))
        {
            FontManager.RegisterFontType("Gwendolyn", fontStream);
        }

        var document = Document.Create(document =>
        {
            document.Page(page =>
            {
                #region Header

                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(TextStyle.Default.FontFamily("Times New Roman"));
                page.Header().Padding(0).Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignCenter().Text(text =>
                        {
                            text.AlignCenter();
                            text.Span("T.C. BOLU ABANT İZZET BAYSAL ÜNİVERSİTESİ\n").FontSize(10).Bold();
                            text.Span("Mühendislik Fakültesi\n").FontSize(10).Bold();

                            if (departmentTitles.TryGetValue(student.DepartmentId, out var title))
                            {
                                text.Span(title).FontSize(10).Bold();
                            }
                        });
                    });
                });
                #endregion
                page.Content().Column(column =>
                {
                    if (!background)
                    {
                        page.Background("#FF0000"); 
                    }
                    column.Item().PaddingVertical(10).Text(text =>
                    {
                        text.AlignCenter();
                        text.Span("İLGİLİ MAKAMA").FontSize(7);
                    });
                    column.Item().Text(text =>
                    {
                        text.Span(
                                @"Bölümümüz öğrencilerinin 2547 sayılı Yükseköğretim Kanunu ve Fakültemiz Staj Yönergesi gereği 2. ve 3. Sınıf sonunda 20'şer iş günü (toplam 40 iş günü) kurumunuzda/işletmenizde temel meslek stajı yükümlülüklerini yerine getirmesi gerekmekte olup; staj dönemi süresince 5510 sayılı GSS Kanununun 5/b maddesi ve aynı Kanunun 87/e bendi uyarınca is kazası ve meslek hastalığına karşı sigortalanması, sigorta primlerinin ödenmesi, kurumumuz tarafından karşılanacaktır.")
                            .FontSize(8);
                    });
                    column.Item().Text(text =>
                    {
                        text.Span(
                                "Adı geçen öğrencinin kurumunuzda/işletmenizde 20 (yirmi) iş günü staj yapması konusunda gereken kolaylığın gösterilmesini arz/rica ederim.")
                            .FontSize(7);
                    });

                    column.Item().Text(text =>
                    {
                        text.AlignLeft();
                        text.Span("Saygılarımla").FontSize(7);
                    });

                    column.Item().Text(text =>
                    {
                        text.AlignRight();
                        text.Span("Bölüm Başkanı:").FontSize(10);
                        text.Span(" " + bolumBaskanı.Name + " " + bolumBaskanı.Surname).FontSize(10);
                        text.Line("");
                        text.Span("Onaylıyorum ").FontFamily("Gwendolyn").FontSize(11).Bold();
                        text.Span(updateTime4).Italic().FontSize(10);
                        text.Element().PaddingRight(4);
                        text.Element().Border(1);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("ZORUNLU STAJ BAŞVURU ve KABUL FORMU").FontSize(10).Bold();
                        text.AlignCenter();
                    });

                    column.Item().Border(1).Text(text =>
                    {
                        text.Span("ÖĞRENCİNİN ÖĞRENİM VE STAJ BİLGİLERİ").FontSize(10).Bold();
                        text.AlignCenter();
                        text.Element().Padding(1);
                    });

                    column.Item().Row(row2 =>
                    {
                        row2.AutoItem().Width(445).Border(1).Column(column3 =>
                        {
                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span(" Öğrenci No");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(student.Number); });
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Adı Soyadı");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(student.Name + " " + student.Surname).FontSize(fontSize);});
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Bölümü");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(department.Name); });
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Öğretim Yılı / Sınıfı");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(student.Class.ToString()); });
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Staj Dönemi");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(internType.Type); });
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Staj Türü");
                                });
                                row.AutoItem().Width(325).Border(1).Text(text => { text.Span(period.TypeOfInternship); });
                            });

                            column3.Item().Row(row =>
                            {
                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Staj Başlangıç Tarihi");
                                });
                                row.AutoItem().Width(102).Border(1).Text(text => { text.Span(student.IntershipStartDate.ToString("dd/MM/yyyy")); });

                                row.AutoItem().Width(120).Border(1).PaddingLeft(5).PaddingRight(10).Text(text =>
                                {
                                    text.Span("Staj Bitiş Tarihi");
                                });
                                row.AutoItem().Width(103).Border(1).Text(text =>
                                {
                                    text.Span(student.IntershipEndDate.ToString("dd/MM/yyyy"));
                                });
                            });
                        });

                        row2.AutoItem().Width(90).Border(1).Column(column4 =>
                        {
                            column4.Item().Row(row =>
                            {
                                if (studentPath != null && File.Exists(studentPath))
                                {
                                    var image = new FileStream(studentPath, FileMode.Open, FileAccess.Read);
                                    row.AutoItem().Width(90).Border(1).Image(image);
                                }
                                else
                                {
                                    row.AutoItem().Width(90).Border(1).AlignCenter().AlignMiddle().Text("");
                                }
                            });
                        });

                    });

                    column.Item().Column(column2 =>
                    {
                        column2.Item().Border(1).Text(text =>
                        {
                            text.Span("ÖĞRENCİNİN NÜFUS VE ADRES KAYIT BİLGİLERİ").FontSize(10).Bold(); ;
                            text.AlignCenter();
                        });
                    });

                    column.Item().Row(row2 =>
                    {
                        row2.AutoItem().Width(269).Border(1).Column(column3 =>
                        {
                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Adı Soyadı");
                                row3.AutoItem().Width(149).Border(1).Text(student.Name + " " + student.Surname);
                            });

                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" T.C.Kimlik No");
                                row3.AutoItem().Width(149).Border(1).Text(student.IdentificationNumber);
                            });

                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("  Baba Adı");
                                row3.AutoItem().Width(149).Border(1).Text(student.FatherName);
                            });

                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("  Anne Adı");
                                row3.AutoItem().Width(149).Border(1).Text(student.MotherName);
                            });
                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("  Doğum Yeri");
                                row3.AutoItem().Width(149).Border(1).Text(city.Name);
                            });
                            column3.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("  Doğum Tarihi");
                                row3.AutoItem().Width(149).Border(1).Text(student.DateOfBirth.ToString("dd/MM/yyyy"));
                            });
                        });

                        row2.AutoItem().Width(269).Border(1).Column(column4 =>
                        {
                            column4.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Height(43).Border(1).Text(text =>
                                {
                                    text.Span(" Adres");
                                    text.AlignCenter();
                                    text.Element().PaddingTop(25);
                                });
                                row3.AutoItem().Width(149).Border(1).Text(student.Address).FontSize(fontSize1);
                            });
                            column4.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Ev Telefonu");
                                row3.AutoItem().Width(149).Border(1).Text("");
                            });
                            column4.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Cep Telefonu");
                                row3.AutoItem().Width(149).Border(1).Text(student.TelephoneNumber);
                            });
                            column4.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" E-Posta");
                                row3.AutoItem().Width(149).Border(1).Text(student.StudentEmail).FontSize(fontSizepostaogrenci);
                            });
                        });
                    });

                    column.Item().Row(row2 =>
                    {
                        row2.AutoItem().Width(269).Border(1).Column(column2 =>
                        {
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(269).Border(1).PaddingTop(4).Text(text =>
                                {
                                    text.Span("STAJ YAPILAN YERİN").FontSize(10).Bold();
                                    text.AlignCenter();
                                });
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Adı");
                                row3.AutoItem().Width(149).Border(1).Text(company.CompanyName).FontSize(fontSize2);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Adresi");
                                ;
                                row3.AutoItem().Width(149).Border(1).Text(company.Adress).FontSize(fontSize3);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Faaliyet Alanı");
                                ;
                                row3.AutoItem().Width(149).Border(1).Text(company.ActivityArea).FontSize(fontSizeAcitiviyArea);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Top. Çal. Sayısı");
                                ;
                                row3.AutoItem().Width(149).Border(1).Text(company.TotalNumberOfEmployees);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("Hafta Sonu Mesai");
                                ;
                                row3.AutoItem().Width(149).Border(1)
                                    .Text((bool)company.AllDayWorkingOnWeekends ? "Evet" : "Hayir");
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Telefon");
                                row3.AutoItem().Width(149).Border(1).Text(company.TelephoneCompany);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Faks");
                                row3.AutoItem().Width(149).Border(1).Text(company.Fax);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" E-Posta");
                                row3.AutoItem().Width(149).Border(1).Text(company.CompanyEmail).FontSize(fontSize1);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text("Gerekli Mühendis");
                                if(background)
                                {
                                    row3.AutoItem().Width(149).Border(1).Text("Gerekli Mühendislere Sahip").FontSize(10);
                                }
                                else
                                {
                                    row3.AutoItem().Width(149).Border(1).Text("Gerekli Mühendislere Sahip Değil").Bold().FontColor("#FFFFFF").FontSize(10);
                                }
                            });
                        });

                        row2.AutoItem().Width(269).Border(1).Column(column2 =>
                        {
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(269).Border(1).PaddingTop(4).Text(text =>
                                {
                                    text.Span("YETKİLİNİN").FontSize(10).Bold();
                                    text.AlignCenter();
                                });
                            });

                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Adı Soyadı");
                                row3.AutoItem().Width(149).Border(1).Text(company.EmployeeName);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Unvanı");
                                row3.AutoItem().Width(149).Border(1).Text(company.Title);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Görev Alanı");
                                row3.AutoItem().Width(149).Border(1).Text(company.MissionArea).FontSize(fontSizeMissionArea);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" Telefon");
                                row3.AutoItem().Width(149).Border(1).Text(company.Telephone);
                            });
                            column2.Item().Row(row3 =>
                            {
                                row3.AutoItem().Width(120).Border(1).Text(" E-Posta");
                                row3.AutoItem().Width(149).Border(1).Text(company.Email).FontSize(fontSize1);
                            });
                            if (role == null || role.Equals("Fakülte Sekreteri") || role.Equals("Staj Komisyonu") || role.Equals("Bölüm Sekreteri") || role.Equals("Öğrenci"))
                            {
                                column2.Item().Height(63).Row(row3 =>
                                {
                                    row3.AutoItem().Width(120).Border(1).Text(" Tarih İmza/Kaşe");
                                    row3.AutoItem().Width(149).Border(1).Text("Onaylıyorum" + " " + updateTime5).FontFamily("Gwendolyn").FontSize(11).Bold();
                                });
                            }
                            //else if(role.Equals("Şirket ") || role.Equals("Fakülte Sekreteri") || role.Equals("Staj Komisyonu"))
                            //{
                            //    column2.Item().Height(48).Row(row3 =>
                            //    {
                            //        row3.AutoItem().Width(120).Border(1).Text(" Tarih İmza/Kaşe").FontSize(8);
                            //        row3.AutoItem().Width(149).Border(1).Text("Onaylıyorun").Italic();
                            //    });
                            //}

                        });
                    });

                    column.Item().Border(1).Column(column2 =>
                    {
                        column2.Item().Text(text =>
                        {
                            text.DefaultTextStyle(TextStyle.Default.FontFamily("Times New Roman"));
                            text.Span(
                                    "Yukarıda belirttiğim bilgilerin doğruluğunu, belirttiğim tarihler arasında 20 günlük stajımı yapacağımı, stajımın başlangıç ve bitiş tarihlerinin değişmesi veya stajıma başlamamam ya da stajdan vazgeçmem hâlinde en az 5 gün önceden, staj yaptığım süre içerisinde herhangi bir nedenden dolayı aldığım sağlık raporunu en geç 2 gün içerisinde “Bölüm Sekreterliğine” bildireceğimi, aksi taktirde SGK prim ödemeleri nedeniyle doğabilecek maddi zararları karşılayacağımı; staj süresince öğrendiğim olayları, kişileri, isimleri ve diğer bilgileri üçüncü kişilerle paylaşmayacağımı, paylaştığım takdirde her türlü sorumluluğu üstleneceğimi beyan ve taahhüt ederim.")
                                .FontSize(8).Italic();
                        });
                        column2.Item().Text(text => { text.EmptyLine(); });
                        column2.Item().Width(500).Row(row =>
                        {
                            row.AutoItem().Width(100).PaddingLeft(12).Text(text =>
                            {
                                text.Span("Tarih:").Italic().FontSize(10);
                                text.Span(dateOnly).Italic().FontSize(8);
                                text.AlignLeft();
                            });

                            row.AutoItem().Width(200).Text(text =>
                            {
                                text.Span("Öğrencinin Adı Soyadı:").Italic().FontSize(10);
                                text.Span(student.Name + " " + student.Surname).Italic().FontSize(8);
                                text.AlignCenter();
                            });

                            row.AutoItem().Width(100).Text(text =>
                            {
                                text.Span(" Öğrencinin İmzası:").Italic().FontSize(10);
                                text.Span("Onaylıyorum").FontSize(10).FontFamily("Gwendolyn").FontSize(8).Bold();
                                text.AlignRight();
                            });
                        });
                    });

                    column.Item().Row(row =>
                    {
                        row.AutoItem().Width(269).Border(1).PaddingTop(4).Column(column2 =>
                        {
                            column2.Item().Text(text =>
                            {
                                text.Span("STAJ KOMİSYON BAŞKANI").FontSize(10).Bold();
                                text.AlignCenter();
                            });
                            if (role == null || role.Equals("Fakülte Sekreteri") || role.Equals("Staj Komisyonu") || role.Equals("Bölüm Sekreteri") || role.Equals("Öğrenci"))
                            {
                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("Tarih:");
                                    row.RelativeItem().Text(updateTime6);
                                });

                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("Adı Soyadı:");
                                    row.RelativeItem().Text(stajKomisyon.Name + " " + stajKomisyon.Surname);
                                });

                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("İmza:");
                                    row.RelativeItem().Text("Onaylıyorum").FontFamily("Gwendolyn").FontSize(11).Bold();
                                });
                            }
                        });
                        row.AutoItem().Width(269).Border(1).PaddingTop(4).Column(column2 =>
                        {
                            column2.Item().Text(text =>
                            {
                                text.Span("GERÇEKLEŞTİRME YETKİLİSİ").FontSize(10).Bold();
                                text.AlignCenter();
                            });
                            if (role == null || role.Equals("Fakülte Sekreteri") || role.Equals("Bölüm Sekreteri") || role.Equals("Öğrenci"))
                            {
                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("Tarih:");
                                    row.RelativeItem().Text(updateTime7);
                                });

                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("Adı Soyadı:");
                                    row.RelativeItem().Text(fakulteSekreteri.Name + " " + fakulteSekreteri.Surname);
                                });

                                column2.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingLeft(12).Text("İmza:");
                                    row.RelativeItem().Text("Onaylıyorum").FontFamily("Gwendolyn").FontSize(11).Bold();
                                });
                            }

                        });
                    });
                });

                #region Footer

                page.Footer().Column(column =>
                {
                    column.Item().Text(text =>
                    {
                        text.Span(
                                "ÖNEMLİ NOT: 1- İlgili öğrenci bu belgeyi 2 adet “ıslak imzalı” olarak doldurur, 2- ilgili öğretim üyesine imzalatır, 3- staj yapacağı kuruma/firmaya onaylatır 4- staj komisyon başkanına imzalattıktan sonra 5- iş kazası ve meslek hastalığına karşı sigortalanması, sigorta primlerinin ödenmesi amacıyla  “Gerçekleştirme Yetkilisine” (Fakülte Sekreteri) imzalatarak, 1 adet provizyon (müstehaklık) belgesi ile birlikte staja başlamadan en geç 20 gün öncesinden Bölüm Sekreterliği’ne teslim eder.\r\nBOLU ABANT İZZET BAYSAL ÜNİVERSİTESİ  MÜHENDİSLİK FAKÜLTESİ \r\n")
                            .FontSize(7);
                    });
                });

                #endregion
            });
        });

        document.GeneratePdf($"{student.Number}.pdf");
        return document;
    }
    public Image ByteArrayToImage(byte[] byteArrayIn)
    {
        using (MemoryStream ms = new MemoryStream(byteArrayIn))
        {
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }

}