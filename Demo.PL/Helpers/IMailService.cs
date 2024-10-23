using Demo.DAL.Models;

namespace Demo.PL.Helpers
{
    public interface IMailService
    {
        public void SendEmail(Email email);
    }
}
