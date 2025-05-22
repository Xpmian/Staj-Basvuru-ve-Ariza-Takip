namespace Aibu.InternshipAutomation.Helper
{
    public interface ILdapService
    {
        public string AuthenticateStudentWithLdap(string username, string password, string tur);
    }
}
