using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Portal.Infrastructure.Data;
using ProductManagement.API;
using ProductManagement.API.Mapping;
using ProductManagement.Core.Interfaces;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Core.Services;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Logging;
using ProductManagement.Infrastructure.Repositories;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Info("Starting application...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Add services
    builder.Services.AddControllers();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));
    builder.Services.AddScoped<IProductUOW, ProductUOW>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application startup failed!");
    throw;
}
finally
{
    LogManager.Shutdown();
}