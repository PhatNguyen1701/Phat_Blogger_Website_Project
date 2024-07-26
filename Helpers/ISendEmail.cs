namespace Phat_Blogger_Website.Helpers
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
