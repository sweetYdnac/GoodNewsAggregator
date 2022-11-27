using by.Reba.Business.ServicesImplementations;
using by.Reba.Core.Abstractions;
using by.Reba.Data.Abstractions;
using by.Reba.Data.Abstractions.Repositories;
using by.Reba.Data.Repositories;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.Helpers
{
    public static class BuidExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<T_Article>, Repository<T_Article>>();
            services.AddScoped<IRepository<T_Category>, Repository<T_Category>>();
            services.AddScoped<IRepository<T_Comment>, Repository<T_Comment>>();
            services.AddScoped<IRepository<T_History>, Repository<T_History>>();
            services.AddScoped<IRepository<T_Positivity>, Repository<T_Positivity>>();
            services.AddScoped<IRepository<T_Preference>, Repository<T_Preference>>();
            services.AddScoped<IRepository<T_Role>, Repository<T_Role>>();
            services.AddScoped<IRepository<T_Source>, Repository<T_Source>>();
            services.AddScoped<IRepository<T_User>, Repository<T_User>>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IArticleInitializerService, ArticleInitializerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<IPositivityService, PositivityService>();
            services.AddTransient<IPreferenceService, PreferenceService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
