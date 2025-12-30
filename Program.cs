using Cooktel_E_commrece.Extenstions;
using Cooktel_E_commrece.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddJwtIdentity(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseReDoc(c =>
    {
        c.SpecUrl("/openapi/v1.json"); // Link to OpenAPI JSON
        c.RoutePrefix = "redoc"; // ReDoc UI at /redoc
        c.DocumentTitle = "Cooktel E-commerce API";
    });

}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
