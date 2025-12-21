using Cooktel_E_commrece.Extenstions;
using Cooktel_E_commrece.Middleware;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers().AddNewtonsoftJson(opt =>
//{
//    opt.SerializerSettings.Converters.Add(new  StringEnumConverter());
//});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddJwtIdentity(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
