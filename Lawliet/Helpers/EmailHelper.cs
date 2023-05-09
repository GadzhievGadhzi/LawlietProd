using MailKit.Net.Smtp;
using MimeKit;

namespace Lawliet.Helpers {
    public class EmailMessage {
        public string To { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public EmailMessage() { }
        public EmailMessage(string to, string title, string text) {
            this.To = to;
            this.Title = title;
            this.Text = text;
        }

        public void SendEmailMessage() {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Lawliet", "gadzhievgadzhi18@gmail.com"));
            message.To.Add(new MailboxAddress("Tom", this.To));

            message.Subject = this.Title;
            message.Body = new TextPart("plain") {
                Text = this.Text
            };

            using (var client = new SmtpClient()) {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("gadzhievgadzhi18@gmail.com", "sawvormqfzuigfgt");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    };
}
