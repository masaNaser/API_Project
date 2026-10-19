using KASHOP.BLL.Common;
using KASHOP.BLL.Services.Authentication;
using KASHOP.BLL.Services.Category;
using KASHOP.BLL.Services.FileServices;
using KASHOP.BLL.Services.Product;
using KASHOP.DAL.Repository.Category;
using KASHOP.DAL.Repository.Product;
using KASHOP.DAL.Repository.UnitOfWork;

namespace KASHOP.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
