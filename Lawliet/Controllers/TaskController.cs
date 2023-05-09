using Lawliet.Data;
using Lawliet.Helpers;
using Lawliet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            task.AuthorId = HttpContext.Request.Cookies["id"].ToString();

            var users = _context.Users.Where(x => x.Group == task.Group).ToList();
            users.ForEach(user => user.StudentTasks.Add(task));
            _context.Users.UpdateRange(users);
            await _context.SaveChangesAsync();

            var timer = HttpContext.RequestServices.GetRequiredService<TimerHelper>();
            timer.StartMessageSendTimer();

            return View();
        }

        [HttpGet]
        public IActionResult RemoveUser(string taskId) {
            var user = _context.Users.Where(x => x.Id == HttpContext.Request.Cookies["id"]).FirstOrDefault();
            var completedTask = JsonConvert.DeserializeObject<List<string>>(user.CompletedTask);
            completedTask!.Add(taskId);
            user.CompletedTask = JsonConvert.SerializeObject(completedTask);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}