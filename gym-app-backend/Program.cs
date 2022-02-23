global using Serilog;
using gym_app_backend.Extensions;

// Serilog Logger setup for NET 6 referenced from https://github.com/datalust/dotnet6-serilog-example
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting up Web API...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.AddFirebaseAuthentication(builder.Configuration);
        
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Made extension for Firebase. Research if this is necessary in the future as it's not really using the IServiceCollection in the method at the moment.
    builder.Services.AddFirebaseAdmin(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex)
{
    Log.Error($"Program.cs Error: {ex.Message}");
}
finally
{
    Log.Information("Shutdown complete.");
    Log.CloseAndFlush();
}

