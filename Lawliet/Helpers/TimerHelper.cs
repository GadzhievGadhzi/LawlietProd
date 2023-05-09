using Lawliet.Data;
using Microsoft.EntityFrameworkCore;

namespace Lawliet.Helpers {
    public class TimerHelper {
        public void StartMessageSendTimer() {
            TimerCallback tm = new TimerCallback(StartMessageSendTimer);
            Timer timer = new Timer(tm, 1, 0, 2100000);
        }

        private void StartMessageSendTimer(object _) {
            using (UserDataContext context = new UserDataContext()) {

                var tasks = context.StudentTasks.Include(x => x.Users).ToList();
                tasks.ForEach(task => {
                    task.Users.ForEach(user => {

                        EmailMessage emailMessage = new EmailMessage();
                        emailMessage.To = user.Email!;
                        emailMessage.Title = "Lawliet - Оповещение о окончании задания.";

                        var expression = (task.EndDate - DateTime.Now);
                        if (expression.TotalMinutes < 720 && expression.TotalMinutes > 660) {
                            emailMessage.Text = $"Задание \"{task.Titile}\" закончится через 12 часов!";
                            emailMessage.SendEmailMessage();
                            return;
                        }

                        if (expression.TotalMinutes < 360 && expression.TotalMinutes > 300) {
                            emailMessage.Text = $"Задание \"{task.Titile}\" закончится через 6 часов!";
                            emailMessage.SendEmailMessage();
                            return;
                        }

                        if (expression.TotalMinutes < 60 && expression.TotalMinutes > 0) {
                            emailMessage.Text = $"Задание \"{task.Titile}\" закончится через 1 час!";
                            emailMessage.SendEmailMessage();
                            return;
                        }
                    });
                });
            }
        }
    }
}