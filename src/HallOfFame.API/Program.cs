using HallOfFame.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using HallOfFame.Core.Interfaces;
using HallOfFame.DataAccess.Repositories;
using FluentValidation;
using HallOfFame.API.Middlewares;
using HallOfFame.API.Profiles;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Async(a => a.Console())
            .WriteTo.Async(a => a.File("logs/HallOfFameWebAppLog.txt", rollingInterval: RollingInterval.Day))
            .CreateLogger();

        try
        {
            Log.Information("Starting up the application");
            var builder = ConfigureApp(args);
            await RunApp(builder);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while app initialization");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplicationBuilder ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();

        var services = builder.Services;

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        services.AddSwaggerGen(opts =>
        {
            opts.IncludeXmlComments(xmlFilePath);
        });

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(configuration["DatabaseConnection"]));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddPersonCommand).Assembly));
        services.AddAutoMapper(typeof(ControllerMappingProfile).Assembly);

        services.AddScoped<IPersonRepository, PersonRepository>();

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssemblyContaining<AddPersonCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.Migrate();
        }

        await app.RunAsync();
    }
}