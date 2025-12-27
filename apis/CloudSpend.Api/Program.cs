using CloudSpend.Api.Repos;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

/* ✅ REGISTER REPO PROPERLY */
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseRouting();
app.MapControllers();

app.Run();