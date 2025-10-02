using InventoryManagementSystem.Data;
using InventoryManagementSystem.Data.SeedData;
using InventoryManagementSystem.Extensions;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.DbConfiguration(builder.Configuration);
builder.Services.CorsConfiguration();
builder.Services.ServiceCollectionConfiguration();
builder.Services.JwtSchemeConfiguration(builder.Configuration);
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scopeService = app.Services.CreateScope();
    var appDbContext = scopeService.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!appDbContext.Items.Any())
    {
        InventorySystem.SeedData(appDbContext);
        AuthenticationSystem.SeedData(appDbContext);
    }
        
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAny");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
