using TransactionManagement.BLL.Interfaces;
using TransactionManagement.BLL.Services;

namespace webapi.DI
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            string geoNamesUsername = configuration["GeoNames:Username"];

            services.AddTransient<ILocation>(provider =>
            {
                var httpClient = provider.GetRequiredService<HttpClient>();
                return new LocationService(httpClient, geoNamesUsername);
            });

            services.AddTransient<ITransaction, TransactionService>();
            services.AddTransient<IGetListTransaction, GetListTransactionService>();
            services.AddTransient<IExportToExcel, ExportToExcelService>();
        }
    }
}