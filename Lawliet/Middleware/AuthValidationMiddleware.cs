using Lawliet.Models;
using Lawliet.Services;

namespace Lawliet.Middleware {
    public class AuthValidationMiddleware {
        private readonly RequestDelegate _next;
        public AuthValidationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            string id = context.Request.Cookies["id"]!;
            if (string.IsNullOrEmpty(id)) {
                await _next.Invoke(context);
                return;
            }

            CachingService cachingService = context.RequestServices.GetService<CachingService>()!;
            var user = cachingService.GetObjectFromCache<User>(id);
            if (user == null) {
                context.Response.Redirect("/auth/login/");
            }
            await _next.Invoke(context);
        }
    }
}