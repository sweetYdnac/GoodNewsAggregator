using by.Reba.Business.ServicesImplementations;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Repositories;
using by.Reba.Data.Repositories.Abstractions;
using by.Reba.Data.Repositories.Implementations;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.Helpers
{
    public static class Extensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
             services.AddScoped<IUnitOfWork, UnitOfWork>();

             services.AddScoped<IRepository<T_Article>, Repository<T_Article>>();
             services.AddScoped<IRepository<T_Category>, Repository<T_Category>>();
             services.AddScoped<IRepository<T_Comment>, Repository<T_Comment>>();
             services.AddScoped<IRepository<T_Notification>, Repository<T_Notification>>();
             services.AddScoped<IRepository<T_PositivityRating>, Repository<T_PositivityRating>>();
             services.AddScoped<IRepository<T_Role>, Repository<T_Role>>();
             services.AddScoped<IRepository<T_Source>, Repository<T_Source>>();
             services.AddScoped<IRepository<T_User>, Repository<T_User>>();
             services.AddScoped<IRepository<T_UserPreference>, Repository<T_UserPreference>>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();
        }
    }
}
