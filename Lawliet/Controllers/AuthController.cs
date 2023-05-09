using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Lawliet.Models;
using Lawliet.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lawliet.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task = System.Threading.Tasks.Task;
using Newtonsoft.Json;

namespace Lawliet.Controllers {
    public class AuthController : Controller {
        private readonly CachingService _cachingService;
        private readonly UserDataContext _context;

        public AuthController(CachingService cachingService, UserDataContext context) {
            _cachingService = cachingService;
            _context = context;
        }

        public async void Login() {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties() {
                RedirectUri = Url.Action($"loggedIn")
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoggedIn() {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claim = result?.Principal?.Identities?.FirstOrDefault()?.Claims.Select(claim => new {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var user = new User();
            claim?.ToList().ForEach(x => {
                switch (x.Type) {
                    case ClaimTypes.NameIdentifier:
                        user.Id = x.Value;
                        break;

                    case ClaimTypes.Name:
                        user.Name = x.Value;
                        break;

                    case ClaimTypes.Email:
                        user.Email = x.Value;
                        break;

                    case "urn:google:picture":
                        user.PictureUrl = x.Value;
                        break;
                }
            });

            user.CompletedTask = JsonConvert.SerializeObject(new List<string>());

            /*user.Topics.Add(new LessonTopic {
                Id = new Random().Next(10000, 100000).ToString(),
                ShortTitle = "Ядерная физика",
                Description = "Я́дерная фи́зика — раздел физики, изучающий структуру и свойства атомных ядер, а также их столкновения.",
                Url = "https://elar.urfu.ru/bitstream/10995/47002/1/978-5-7996-1992-3_2017.pdf",
                PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/CNO_Cycle.svg/1200px-CNO_Cycle.svg.png",
            });

            user.Topics.Add(new LessonTopic {
                Id = new Random().Next(10000, 100000).ToString(),
                ShortTitle = "Квантовая механика",
                Description = "Ква́нтовая меха́ника — фундаментальная физическая теория, которая описывает природу в масштабе атомов и субатомных частиц. Она лежит в основании всей квантовой физики, включая квантовую химию, квантовую теорию поля, квантовую технологию и квантовую информатику.",
                Url = "https://elar.urfu.ru/bitstream/10995/53042/1/978-5-321-02527-7_2017.PDF",
                PictureUrl = "https://new-science.ru/wp-content/uploads/2021/04/8281-2.jpg",
            });

            user.Topics.Add(new LessonTopic {
                Id = new Random().Next(10000, 100000).ToString(),
                ShortTitle = "Радиово́лны",
                Description = "Радиово́лны — электромагнитные волны с частотами до 3 ТГц, распространяющиеся в пространстве без искусственного волновода. Радиоволны в электромагнитном спектре располагаются от крайне низких частот вплоть до инфракрасного диапазона.",
                Url = "https://kpfu.ru/portal/docs/F1354445323/RWP26_Vol2_Final_compressed.pdf",
                PictureUrl = "https://image.slidesharecdn.com/random-130127210417-phpapp01/75/-14-2048.jpg?cb=1669073551",
            });*/

            var isUserInCache = _cachingService.GetObjectFromCache<User>(user.Id);
            if (isUserInCache != null) {
                HttpContext.Response.Cookies.Append("id", user.Id);
                await _cachingService.AddObjectFromCache(user);
                var previousPage = HttpContext.Request?.Cookies["previousPage"]?.Split('/');
                return (previousPage == null) ? RedirectToAction("Index", "Home") : RedirectToAction(previousPage[1], previousPage[0]);
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> LoggedIn(User user) {
            HttpContext.Response.Cookies.Append("id", user.Id);
            await _cachingService.AddObjectFromCache(user);
            var previousPage = HttpContext.Request?.Cookies["previousPage"]?.Split('/');
            return (previousPage == null) ? RedirectToAction("Index", "Home") : RedirectToAction(previousPage[1], previousPage[0]);
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("id");
            return RedirectToAction("index", "home");
        }

        /*[HttpGet] public IActionResult LoginConfirmation() => View();

        [HttpPost]
        public IActionResult LoginConfirmation(bool isClickedLoginButton) {
            if (isClickedLoginButton) {
                return RedirectToAction("login", "auth");
            }
            return RedirectToAction("index", "home");
        }*/
    }
}