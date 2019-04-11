using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace SendGrid.Helpers.Mail
{
    public static partial class MailMessageExtensions
    {
        public static EmailAddress GetSendGridAddress(this MailAddress address)
        {
            // SendGrid Server-Side API is currently bugged, and messes up when the name has a comma or a semicolon in it
            return String.IsNullOrWhiteSpace(address.DisplayName) ?
                new EmailAddress(address.Address) :
                new EmailAddress(address.Address, address.DisplayName.Replace(",", "").Replace(";", ""));
        }

        public static Attachment GetSendGridAttachment(this System.Net.Mail.Attachment attachment)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    attachment.ContentStream.CopyTo(stream);
                    return new Attachment()
                    {
                        Disposition = attachment.ContentDisposition.Inline ? "inline" : "attachment",
                        Type = attachment.ContentType.MediaType,
                        Filename = attachment.Name,
                        ContentId = attachment.ContentId,
                        Content = Convert.ToBase64String(stream.ToArray())
                    };
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        public static SendGridMessage GetSendGridMessage(this MailMessage message)
        {
            var msg = new SendGridMessage();

            msg.From = message.From.GetSendGridAddress();
            if (message.ReplyToList.Count > 0)
            {
                msg.ReplyTo = message.ReplyToList[0].GetSendGridAddress();
            }

            foreach (var a in message.To)
            {
                msg.AddTo(a.GetSendGridAddress());
            }
            foreach (var a in message.CC)
            {
                msg.AddCc(a.GetSendGridAddress());
            }
            foreach (var a in message.Bcc)
            {
                msg.AddBcc(a.GetSendGridAddress());
            }

            if (!String.IsNullOrWhiteSpace(message.Subject))
            {
                msg.Subject = message.Subject;
            }
            else
            {
                msg.Subject = "";
            }
            if (!String.IsNullOrWhiteSpace(message.Body))
            {
                if (message.IsBodyHtml)
                {
                    var c = new Content();
                    c.Type = "text/html";
                    if (!message.Body.StartsWith("<html"))
                    {
                        c.Value = "<html><body>" + message.Body + "</body></html>";
                    }
                    else
                    {
                        c.Value = message.Body;
                    }
                    msg.Contents = new List<Content>();
                    msg.Contents.Add(c);
                }
                else
                {
                    msg.Contents = new List<Content>();
                    msg.Contents.Add(new Content("text/plain", message.Body));
                }
            }

            foreach (var attachment in message.Attachments)
            {
                msg.AddAttachment(attachment.GetSendGridAttachment());
            }

            return msg;
        }
    }
}