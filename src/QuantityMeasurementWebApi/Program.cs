using Microsoft.EntityFrameworkCore;
using QuantityMeasurementWebApi.Data;
using QuantityMeasurementWebApi.Middleware;
using RepositoryLayer;
using BusinessLayer;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context Setup (Phase 2)
builder.Services.AddDbContext<QuantityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register Repository and Service (Phase 4 - Dependency Injection)
// Note: Ensure that QuantityMeasurementDatabaseRepository and QuantityMeasurementService are implemented in the RepositoryLayer and BusinessLayer projects respectively, and that they implement the IQuantityMeasurementRepository and IQuantityMeasurementService interfaces.
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService>();

// 3. Add Controller support (Phase 5)
builder.Services.AddControllers();

// 4. Configure Swagger/OpenAPI (Phase 1 & UC17 Requirement)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

// 6. Map Controllers (Sare API Endpoints ko register karta hai)
app.MapControllers();

app.Run();

// dotnet run --project .\src\QuantityMeasurementWebApi\QuantityMeasurementWebApi.csproj