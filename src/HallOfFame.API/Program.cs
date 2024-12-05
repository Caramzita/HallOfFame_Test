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
using HallOfFame.API.Behaviors;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

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

        services.AddHealthChecks();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddControllers();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(xmlFilePath);

            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"My API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated ? "This API version is deprecated." : ""
                });
            }

            options.DocInclusionPredicate((version, desc) =>
            {
                var actionApiVersion = desc.ActionDescriptor.EndpointMetadata
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return actionApiVersion.Any(v => $"v{v}" == version || v.ToString() == version || $"v{v.MajorVersion}" == version);
            });
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
        services.AddScoped<ISkillRepository, SkillRepository>();

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
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthorization();

        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var db = serviceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Database migration applied successfully in Development environment.");
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }

        await app.RunAsync();
    }
}