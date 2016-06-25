using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Office365.OutlookServices;

namespace Office365Api.Helpers.Office365
{
    public class MailHelper : BaseOffice365Helper
    {
        public MailHelper(AuthenticationHelper authenticationHelper) : 
            base(authenticationHelper)
        {
        }
        
        public async Task<IEnumerable<IMessage>> GetMessages()
        {
            var client = await this.AuthenticationHelper
                .EnsureOutlookServicesClientCreatedAsync(
                Office365Capabilities.Mail.ToString());

            client.Context.IgnoreMissingProperties = true;

            List<IMessage> mails = new List<IMessage>();

            // ***********************************************************
            // Note from @PaoloPia: To not stress the server, limit the
            // the query to no more than 50 email items
            // ***********************************************************

            var query = (from i in client.Me.Messages
                        orderby i.DateTimeSent descending
                        select i).Take(50);

            var messageResults = await query.ExecuteAsync();

            if (messageResults != null)
            {
                do
                {
                    mails.AddRange(messageResults.CurrentPage);
                    messageResults = await messageResults.GetNextPageAsync();
                }
                while (messageResults != null && messageResults.MorePagesAvailable);
            }

            return mails;
        }

        public async Task SendMail(string to, string subject, string body)
        {
            var client = await this.AuthenticationHelper
                .EnsureOutlookServicesClientCreatedAsync(
                Office365Capabilities.Mail.ToString());

            Message mail = new Message();
            mail.ToRecipients.Add(new Recipient() 
            {
                EmailAddress = new EmailAddress
                {
                    Address = to,
                }
            });
            mail.Subject = subject;
            mail.Body = new ItemBody() { Content = body, ContentType = BodyType.HTML };

            await client.Me.SendMailAsync(mail, true);
        }

        public async Task DraftMail(string to, string subject, string body)
        {
            var client = await this.AuthenticationHelper
                .EnsureOutlookServicesClientCreatedAsync(
                Office365Capabilities.Mail.ToString());

            Message mail = new Message();
            mail.ToRecipients.Add(new Recipient()
            {
                EmailAddress = new EmailAddress
                {
                    Address = to,
                }
            });
            mail.Subject = subject;
            mail.Body = new ItemBody() { Content = body, ContentType = BodyType.HTML };

            await client.Me.Messages.AddMessageAsync(mail);
        }
    }
}
