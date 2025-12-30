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

            services.AddStackExchangeRedisCache(opt => {
                opt.Configuration = config.GetConnectionString("Redis");
                opt.InstanceName = "Cooktel";
            });

            //services.Configure<MailSettings>(config.GetSection("MailSettings"));
            //services.Configure<PaymentSettings>(config.GetSection("Paymob"));

            //OPTIONS
            services.AddOptions<MailSettings>()
                .Bind(config.GetSection("MailSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<PaymentSettings>()
                .Bind(config.GetSection("Paymob"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            //SERVICES
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategroyRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFileService, FileServices>();
            services.AddScoped<IReviewsRepository,ReviewRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICachingService, CachingService>();
            services.AddAutoMapper(cfg => {
                // Global configuration (optional)
            }, typeof(AutoMapperProfilies));
            return services;
        }
    }
}
