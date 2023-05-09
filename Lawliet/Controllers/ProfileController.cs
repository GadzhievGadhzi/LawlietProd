using Lawliet.Data;
using Lawliet.Models;
using Lawliet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Lawliet.Controllers
{
    public class ProfileController : Controller {
        private readonly UserDataContext _context;
        private readonly CachingService _cachingService;

        public ProfileController(UserDataContext context, CachingService cachingService) {
            _context = context;
            _cachingService = cachingService;
        }

        [HttpGet]
        public IActionResult Edit(string id) {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user) {
            DataRepository<User> repository = new DataRepository<User>(_context);
            User _user = repository.GetWithInclude(x => x.Id == user.Id, z => z.Topics).First();

            repository.Remove(_user);

            _user.AboutMe = user.AboutMe;
            _user.Email = user.Email;
            _user.Status = user.Status;
            _user.Name = user.Name;
            _user.AboutMe = user.AboutMe;
            _user.Group = user.Group;

            await repository.CreateAsync(_user);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index() {
            /*DataRepository<User> repository = new DataRepository<User>(_context);
            var user = repository.GetWithInclude(x => x.Id == HttpContext.Request.Cookies["id"], z => z.Topics).ToList();*/

            var tasks = _context.StudentTasks.Where(x => x.AuthorId == HttpContext.Request.Cookies["id"]);
            var user = _context.Users.Where(x => x.Id == HttpContext.Request.Cookies["id"]).FirstOrDefault();
            user.StudentTasks = tasks.ToList();
            return View(user);
        }
    }
}