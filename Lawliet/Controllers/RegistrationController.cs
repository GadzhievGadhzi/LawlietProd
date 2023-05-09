using Lawliet.Models;
using Lawliet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lawliet.Controllers {
    public class RegistrationController : Controller {
        private readonly CachingService _cachingService;

        public RegistrationController(CachingService cachingService) {
            _cachingService = cachingService;
        }

        [HttpGet] public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(User user) {

            await _cachingService.AddObjectFromCache(user);
            var previousPage = HttpContext.Request?.Cookies["previousPage"]?.Split('/');
            return (previousPage == null) ? RedirectToAction("Index", "Home") : RedirectToAction(previousPage[1], previousPage[0]);
        }

        
    }
}
