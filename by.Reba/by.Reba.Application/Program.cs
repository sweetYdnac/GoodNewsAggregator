using by.Reba.Application.Filters;
using by.Reba.Application.Helpers;
using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
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

            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,
                    }));

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(typeof(AddArticleDataFromRssFeedCommand).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseExceptionHandler("/Article/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions 
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Article}/{action=Index}/{page:int?}");

            app.MapControllerRoute(
                name: "paging",
                pattern: "{page:int}/{controller=Article}/{action=Index}");

            app.MapHangfireDashboard();

            app.Run();
        }
    }
}