using System.Text.Json;
using APBD_s31722_TEST_TEMPLATE.DataLayer;
using APBD_s31722_TEST_TEMPLATE.Exceptions;
using APBD_s31722_TEST_TEMPLATE.Interfaces;
using APBD_s31722_TEST_TEMPLATE.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbClient, DbClient>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddControllers();
var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

app.MapControllers();
app.Run();