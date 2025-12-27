using CloudSpend.Api.Repos;

Console.WriteLine("🔥🔥🔥 API BOOTED WITH NEW BUILD 🔥🔥🔥");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("https://calm-mud-00bc83d00.1.azurestaticapps.net")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        return;
    }

    await next();
});

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



