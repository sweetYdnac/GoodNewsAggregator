using by.Reba.Application.Helpers;
using by.Reba.DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace by.Reba.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.File(@"f:\GoodNewsAggregator\by.Reba\Logs\log.txt", LogEventLevel.Error)
                .WriteTo.Console(LogEventLevel.Debug));

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString(@"/Account/Login");
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });

            var connectionString = builder.Configuration.GetConnectionString("RebaDbConnection");
            builder.Services.AddDbContext<RebaDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString));

            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Article/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Article}/{action=Index}/{page?}");

            app.MapControllerRoute(
                name: "paging",
                pattern: "{page}/{controller=Article}/{action=Index}");

            app.Run();
        }
    }
}