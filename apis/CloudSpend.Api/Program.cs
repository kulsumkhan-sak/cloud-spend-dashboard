using CloudSpend.Api.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://calm-mud-00bc83d001.azurestaticapps.net"            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseRouting();                 // ✅ FIRST
app.UseCors("FrontendPolicy");    // ✅ AFTER routing, BEFORE auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.WriteLine("🔥 NEW CORS VERSION DEPLOYED 🔥");
app.Run();                        // ✅ ONLY ONE Run





