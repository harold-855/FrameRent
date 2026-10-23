using FrameRent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de la API.
builder.Services.AddControllers();

// Configurar la conexión con PostgreSQL.
builder.Services.AddDbContext<FrameRentDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Mantener la documentación OpenAPI de la plantilla.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configurar OpenAPI para el entorno de desarrollo.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Habilitar los controladores.
app.MapControllers();

app.Run();