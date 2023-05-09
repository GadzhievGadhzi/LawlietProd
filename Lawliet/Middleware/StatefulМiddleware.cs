namespace Lawliet.Middleware {
    public class StatefulМiddleware {
        private readonly RequestDelegate _next;
        public StatefulМiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var routeValues = context.Request.RouteValues;
            var controller = routeValues["controller"]?.ToString();
            var action = routeValues["action"]?.ToString();

            if(controller != null && action != null) {
                var previousPage = context.Request.Cookies["previousPage"];
                if (previousPage != $"{controller}/{action}" && controller.ToLower() != "auth") {
                    context.Response.Cookies.Append("previousPage", $"{controller}/{action}");
                }
            }
            await _next.Invoke(context);
        }
    }
}