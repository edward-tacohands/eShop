using System.Text;
using eshop.api;
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Skapa MySQL serverversion som variabel...
var serverVersion = new MySqlServerVersion(new Version(9, 1, 0));
// Knyta samman applikation med vår databas...
builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Prod"));
  // options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
  // options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

// Sätta upp hantering av användare...
builder.Services.AddIdentityCore<User>(options =>
{
  options.User.RequireUniqueEmail = true;
})
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<DataContext>();

// Dependency injection...
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddControllers();

// Lägg till stöd för swagger...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aktivera inloggningssäkerhet(Authentication)...
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
    };
  });

builder.Services.AddAuthorization();

// Pipeline...
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
