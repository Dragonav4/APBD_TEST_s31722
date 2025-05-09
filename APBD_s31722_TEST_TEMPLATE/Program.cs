using APBD_s31722_9_APi_2.Exceptions;
using APBD_s31722_TEST_TEMPLATE.DataLayer;
using APBD_s31722_TEST_TEMPLATE.Interfaces;
using APBD_s31722_TEST_TEMPLATE.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddSingleton<IDbClient, DbClient>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseRouting();
app.MapControllers();
app.Run();