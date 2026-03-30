using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using QuantityMeasurementWebApi.Data;
using QuantityMeasurementWebApi.Middleware;
using RepositoryLayer;
using BusinessLayer;
using ModelLayer;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context Setup (Phase 2)
builder.Services.AddDbContext<QuantityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettingsDTO>()
    ?? throw new InvalidOperationException("Jwt configuration is missing.");

if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException("Jwt:Key is required.");
}

builder.Services.AddSingleton(jwtSettings);

// 2. Register Repository and Service (Phase 4 - Dependency Injection)
// Note: Ensure that QuantityMeasurementDatabaseRepository and QuantityMeasurementService are implemented in the RepositoryLayer and BusinessLayer projects respectively, and that they implement the IQuantityMeasurementRepository and IQuantityMeasurementService interfaces.
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 3. Add Controller support (Phase 5)
builder.Services.AddControllers();

// 4. Configure Swagger/OpenAPI (Phase 1 & UC17 Requirement)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Define the security scheme
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Important: must be lowercase 'bearer'
        BearerFormat = "JWT",
    };

    string securitySchemeId = JwtBearerDefaults.AuthenticationScheme;
    options.AddSecurityDefinition(securitySchemeId, securityScheme);

    // 2. Apply the security requirement globally
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(securitySchemeId, document, null)] = new List<string>()
    });
});

var app = builder.Build();

// 5. Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

// 6. Map Controllers (Sare API Endpoints ko register karta hai)
app.MapControllers();

app.Run();

// dotnet run --project .\src\QuantityMeasurementWebApi\QuantityMeasurementWebApi.csproj