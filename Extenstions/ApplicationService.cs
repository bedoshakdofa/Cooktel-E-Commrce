using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Helper;
using Cooktel_E_commrece.Interfaces;
using Cooktel_E_commrece.Repository;
using Cooktel_E_commrece.Services;
using Microsoft.EntityFrameworkCore;


namespace Cooktel_E_commrece.Extenstions
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration config) {

            services.AddDbContext<AppDbContext>(opt=>opt.UseNpgsql(config.GetConnectionString("Database")));
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFileService, FileServices>();
            services.AddScoped<IReviewsRepository,ReviewRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
