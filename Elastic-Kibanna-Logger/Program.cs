using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(ConfigureElasticSink(builder.Configuration, builder.Environment.EnvironmentName))
    .CreateBootstrapLogger();

// Add services to the container.
builder.Services.AddRazorPages();


#region ConnectToElastic-Using-Nest
//var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
//       .DefaultIndex("myapp-logs")
//       .DisableDirectStreaming()
//       .PrettyJson();
//var client = new ElasticClient(settings);

//builder.Services.AddSingleton<IElasticClient>(client);
#endregion


builder.Host.UseSerilog();


var app = builder.Build();

app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}
