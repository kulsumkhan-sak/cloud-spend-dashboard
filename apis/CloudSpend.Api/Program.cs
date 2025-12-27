using CloudSpend.Api.Repos;
Console.WriteLine("🔥🔥🔥 API BOOTED WITH NEW BUILD 🔥🔥🔥");

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
app.UseCors("FrontendPolicy");    // ✅ AFTER routing, BEFORE auth
app.UseRouting();                 // ✅ FIRST
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.WriteLine("🔥 NEW CORS VERSION DEPLOYED 🔥");
app.Run();                        // ✅ ONLY ONE Run







