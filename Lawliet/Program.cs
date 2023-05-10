using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Lawliet.Middleware;
using Lawliet.Data;
using Microsoft.EntityFrameworkCore;
using Lawliet.Services;
using Lawliet.Helpers;

namespace Lawliet {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddTransient<CachingService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddDbContext<UserDataContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("dbLawliet")));

            builder.Services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });

            builder.Services.AddTransient<TimerHelper>();


            var app = builder.Build();
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            //app.UseMiddleware<StatefulÌiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseMiddleware<AuthValidationMiddleware>();
            app.Run();
        }
    }
}