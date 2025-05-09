using APBD_s31722_TEST_TEMPLATE.DataLayer;
using APBD_s31722_TEST_TEMPLATE.Exceptions;
using APBD_s31722_TEST_TEMPLATE.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddScoped<IDbClient,DbClient>();

var app = builder.Build();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseRouting();
app.MapControllers();
app.Run();