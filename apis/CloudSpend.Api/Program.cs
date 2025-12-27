using CloudSpend.Api.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
    "https://cloudspend-frontend.azurestaticapps.net",
    "https://calm-mud-00bc83d001.azurestaticapps.net",
    "http://localhost:3000"
)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseRouting();                 // ✅ FIRST
app.UseCors("FrontendPolicy");    // ✅ AFTER routing, BEFORE auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();                        // ✅ ONLY ONE Run

