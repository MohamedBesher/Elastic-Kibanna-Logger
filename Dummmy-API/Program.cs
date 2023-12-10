var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapGet("", (HttpContext httpContext) => {
    return $"the running app has {httpContext.Connection.LocalPort}";
});


app.MapGet("/test",() => {
    return $"working fine.";
});
app.Run();
