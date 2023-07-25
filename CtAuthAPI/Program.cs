using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CtAuthAPI.Models;
using CtAuthAPI.Services;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Set up in-memory database for testing
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("UserDB"));

// Add authentication for Jwt Token
string? secretKey = builder.Configuration.GetSection("AuthSettings:SecretKey").Value;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false, // Disabled dev only: Don't validate the issuer
            ValidateAudience = false, // Disabled dev only: Don't validate the audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CtAuthAPI", Version = "v1" });

    // Add Swagger auth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization header using Bearer Scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Add Swagger auth requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add controllers
builder.Services.AddControllers();

WebApplication app = builder.Build();

// Set up for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CtAuthAPI v1"); });
    
    // Seed the database
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;
        
        // For development only, the wait will cause thread locking
        var userService = services.GetRequiredService<IUserService>();
        userService.SeedUsersAsync().Wait();
    }
}

// app.UseHttpsRedirection(); // Disabled dev only: Not using Https
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();