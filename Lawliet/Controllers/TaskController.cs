using Lawliet.Data;
using Lawliet.Helpers;
using Lawliet.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lawliet.Controllers {
    public class TaskController : Controller {

        private UserDataContext _context;
        public TaskController(UserDataContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddTask() => View();

        [HttpPost]
        public async Task<IActionResult> AddTask(StudentTask task) {
            task.Id = Guid.NewGuid().ToString();
            task.StartDate = DateTime.Now;

            var users = _context.Users.Where(x => x.Group == task.Group).ToList();
            users.ForEach(user => user.StudentTasks.Add(task));
            _context.Users.UpdateRange(users);
            await _context.SaveChangesAsync();

            var timer = HttpContext.RequestServices.GetRequiredService<TimerHelper>();
            timer.StartMessageSendTimer();

            return View();
        }
    }
}