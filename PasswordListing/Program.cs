using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordListing.Application.Interfaces;
using PasswordListing.Application.Services;
using PasswordListing.Domain.Repositories;
using PasswordListing.Domain.Security;
using PasswordListing.Infrastructure.Persistence;
using PasswordListing.Infrastructure.Repositories;
using PasswordListing.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);
var jwtConfig = builder.Configuration.GetSection("Jwt");

string key = jwtConfig["key"] ?? throw new Exception("key not found");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IUserMemoryRepository, InMemoryUserRepository>(); // testonly
builder.Services.AddSingleton<IItemMemoryRepository, InMemoryItemRepository>(); // testonly
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IUnitOfWork, PersistenceContext>();
builder.Services.AddScoped<CustomJwtBearerEventsHandler>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins([
            "http://localhost:5173",
            "http://localhost:3000",
            "http://localhost:4200"
        ])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services
    .AddAuthentication(opt=>{
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt=>{
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        opt.RequireHttpsMetadata = false;

        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromSeconds(5)
        };
        opt.EventsType = typeof(CustomJwtBearerEventsHandler);
    });

var app = builder.Build();
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if(db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
            if(app.Environment.IsDevelopment())
            {
                var seedmanager = new SeedManager(db);
                await seedmanager.SeedDataAsync();
            }
        }
    }
}catch(Exception ex)
{
    Console.WriteLine($"[ERROR] Exception on migrate: {ex.Message}");
}

app.UseCors("AllowSpecificOrigins");
// builder.Services.AddEndpointsApiExplorer(); // Line for start documentation
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
