using Microsoft.EntityFrameworkCore;
using TransactionManagement.DAL.Context;
using webapi.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));


builder.Services.AddHttpClient();

builder.Services.AddTransient<LocationService>(provider =>
    new LocationService(
        provider.GetRequiredService<HttpClient>(),
        builder.Configuration["GeoNames:Username"]));

builder.Services.AddDependencyInjections();

builder.Services.Configure<CsvHelper.Configuration.CsvConfiguration>(options =>
{
    options.HasHeaderRecord = true;
    options.HeaderValidated = null;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();