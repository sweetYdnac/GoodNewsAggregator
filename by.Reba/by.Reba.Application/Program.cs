using by.Reba.Business.ServicesImplementations;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Repositories;
using by.Reba.Data.Repositories.Abstractions;
using by.Reba.Data.Repositories.Implementations;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



            var connectionString = builder.Configuration.GetConnectionString("RebaDbConnection");
            builder.Services.AddDbContext<RebaDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString));

            // Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepository<T_Article>, Repository<T_Article>>();
            builder.Services.AddScoped<IRepository<T_Category>, Repository<T_Category>>();
            builder.Services.AddScoped<IRepository<T_Comment>, Repository<T_Comment>>();
            builder.Services.AddScoped<IRepository<T_Notification>, Repository<T_Notification>>();
            builder.Services.AddScoped<IRepository<T_PositivityRating>, Repository<T_PositivityRating>>();
            builder.Services.AddScoped<IRepository<T_Role>, Repository<T_Role>>();  
            builder.Services.AddScoped<IRepository<T_Source>, Repository<T_Source>>();
            builder.Services.AddScoped<IRepository<T_User>, Repository<T_User>>();
            builder.Services.AddScoped<IRepository<T_UserPreference>, Repository<T_UserPreference>>();


            // Services
            builder.Services.AddTransient<IArticleService, ArticleService>();


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Article}/{action=Index}/{page?}");

            app.Run();
        }
    }
}