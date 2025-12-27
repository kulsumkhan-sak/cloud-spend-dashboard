using CloudSpend.Api.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

/* ✅ CORS */
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://calm-mud-00bc83d001.azurestaticapps.net",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();   // ❌ NO AllowCredentials
    });
});

/* ✅ DI */
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

/* ✅ ORDER MATTERS */
app.UseCors("FrontendPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
