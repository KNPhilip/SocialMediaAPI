global using TweetAPI.Dtos;
global using Tweetinvi;
global using Tweetinvi.Core.Web;
global using Tweetinvi.Models;
global using Hangfire;
using TweetAPI.Services.XService;
using Mapster;
using TweetAPI.Models;
using TweetAPI.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

ConfigureMapster();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(configuration => configuration
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IXService, XService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureMapster()
{
    var config = TypeAdapterConfig.GlobalSettings;

    config.ForType<(User baseUser, RegisterDto dto), User>()
        .Map(dest => dest.Username, src => src.dto.Username)
        .Map(dest => dest.PasswordHash, src => BCrypt.Net.BCrypt.HashPassword(src.dto.Password))
        .Map(dest => dest, src => src.baseUser)
        .IgnoreNonMapped(true);
}
