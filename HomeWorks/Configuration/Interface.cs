namespace HomeWorks.Configuration
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
    }
}
