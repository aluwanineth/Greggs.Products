using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Greggs.Products.Application;
using Greggs.Products.Infrastructure;
using Greggs.Products.Api.Extentions;
using Serilog;
using Greggs.Products.Application.Settings;
using Microsoft.Extensions.Configuration;

namespace Greggs.Products.Api;


public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CurrencySettings>(Configuration.GetSection("CurrencySettings"));
        services.AddApplicationLayer();
        services.AddInfrastructureLayer();

        services.AddSwaggerExtension();
        services.AddApiVersioningExtension();
        services.AddCors();

        services.AddControllers();

        Log.Logger = new LoggerConfiguration()
        .WriteTo.File("Log/Product.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) 
                .AllowCredentials()); 

        app.UseAuthorization();
        app.UseSwaggerExtension();
        app.UseErrorHandlingMiddleware();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}