using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace KASHOP.PL.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";
            var supportedCulture = new[] {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar"),
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCulture;
                options.SupportedUICultures = supportedCulture;
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
            });
            return services;
        }
    }
}
