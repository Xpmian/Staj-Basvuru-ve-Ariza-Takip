using System.Net;


namespace Aibu.InternshipAutomation.Helper
{
    public class LdapService : ILdapService
    {

        [Obsolete("Obsolete")]
        public string AuthenticateStudentWithLdap(string username, string password,string tur)
        {

            //http://onlinemuhendislik.ibu.edu.tr/ldapcheck/ldapapi.php?kullaniciTuru="ogrenci|personel"&kullaniciAdi="Ogrenci/personel numarasi"&kullaniciSifresi="sifre"
            var apiUrl = $"http://onlinemuhendislik.ibu.edu.tr/ldapcheck/ldapApi.php?kullaniciTuru={tur}&kullaniciAdi={username}&kullaniciSifresi={password}";
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            var responseData = string.Empty;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                var responseText = reader.ReadToEnd();
                responseData = responseText;
            }
            catch (WebException ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            return responseData;
        }
    }
}
