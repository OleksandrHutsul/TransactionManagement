using TransactionManagement.BLL.Services;

namespace webapi.DI
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddTransient<TransactionService>();
            services.AddTransient<GetListTransactionService>();
            services.AddTransient<ExportToExcelService>();
        }
    }
}