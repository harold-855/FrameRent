FrameRent --- Registro de avances
1. Contexto del proyecto
   FrameRent es una aplicación de alquiler de equipos desarrollada con
   ASP.NET Core Web API, C# y Clean Architecture. El frontend será
   independiente de la API y la base de datos será PostgreSQL.
   FrameRent/
   ├── FrameRent.sln
   ├── .gitignore
   └── src/
   ├── FrameRent.Api/
   ├── FrameRent.Application/
   ├── FrameRent.Domain/
   └── FrameRent.Infrastructure/
2. Trabajo realizado anteriormente
   Domain
   Se crearon las entidades BaseEntity, Equipment, Customer,
   Rental, RentalDetail, Penalty y el enum RentalStatus.

BaseEntity es una clase abstracta que contiene Guid Id. Domain
representa las entidades y reglas centrales del negocio y no debe
depender de Infrastructure ni de la API.
Application
Se creó IEquipmentRepository en Interfaces/Repositories. Define
operaciones para agregar, consultar, actualizar y comprobar la
existencia de equipos.

También se crearon los DTOs CreateEquipmentRequest y
UpdateEquipmentRequest, junto con estos casos de uso:
- CreateEquipmentUseCase
- GetEquipmentByIdUseCase
- GetAllEquipmentUseCase
- UpdateEquipmentUseCase
- ActivateEquipmentUseCase
- DeactivateEquipmentUseCase
  Application contiene los casos de uso y las interfaces que necesita para
  acceder a datos, sin depender de la implementación concreta de la base
  de datos.
  Infrastructure
  Se instaló el paquete NuGet Npgsql.EntityFrameworkCore.PostgreSQL de
  la línea 10.0.x.

Se creó FrameRentDbContext.cs, que hereda de DbContext y expone los
conjuntos Equipment, Customers, Rentals, RentalDetails y
Penalties.

También se creó EquipmentRepository.cs, que implementa
IEquipmentRepository mediante Entity Framework Core y guarda cambios
con SaveChangesAsync.

Infrastructure implementa los detalles técnicos de persistencia y
conecta las interfaces de Application con Entity Framework Core y
PostgreSQL.
PostgreSQL y API
Se creó la base de datos framerent_db desde pgAdmin. En
FrameRent.Api/appsettings.json se configuró una cadena de conexión
similar a esta:
{
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=framerent_db;Username=postgres;Password=TU_CONTRASENA"
}
}
TU_CONTRASENA es solo un marcador de ejemplo: debe reemplazarse
localmente por la contraseña real y no compartirse públicamente.

En Program.cs se configuraron los controladores, el contexto de Entity
Framework Core con PostgreSQL, OpenAPI, HTTPS y el mapeo de
controladores. La API debe tener las referencias de proyecto necesarias
a Application e Infrastructure.
3. Trabajo más reciente: inyección de dependencias en Application
   Se creó FrameRent.Application/DependencyInjection.cs. Su objetivo es
   centralizar el registro de los servicios de Application mediante un
   método de extensión llamado AddApplication.
   Paquete NuGet
   Como el archivo utiliza IServiceCollection, el proyecto necesita la
   abstracción de inyección de dependencias. Se buscó el paquete:

Microsoft.Extensions.DependencyInjection.Abstractions

Debe elegirse el paquete oficial de Microsoft con una versión estable
compatible con .NET 10, normalmente de la línea 10.0.x. No debe
confundirse con paquetes de terceros o paquetes antiguos para .NET
Framework 4.5.1, como
NET451.Microsoft.Extensions.DependencyInjection.Abstractions.
Código de DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;

namespace FrameRent.Application
{
public static class DependencyInjection
{
public static IServiceCollection AddApplication(
this IServiceCollection services)
{
return services;
}
}
}
Por ahora, AddApplication solo devuelve services; todavía no
registra los casos de uso.
¿Qué significa el error del método de extensión?
Se presentó el error
Extension method can only be declared in non-generic, non-nested static class.

Un método de extensión debe estar dentro de una clase static, no
genérica y no anidada en otra clase. Por eso se usa
public static class DependencyInjection. El método también es static
y el primer parámetro lleva this IServiceCollection services.

Si el error continúa, hay que revisar el archivo completo y sus llaves
{ } para encontrar alguna declaración fuera de lugar.
4. ¿Cómo funciona la inyección de dependencias?
   La inyección de dependencias permite que el contenedor de ASP.NET Core
   cree y proporcione los objetos que necesita cada clase.
1. La API registra los servicios durante el inicio.
2. Application contiene los casos de uso y las interfaces necesarias.
3. Infrastructure registra las implementaciones de repositorios y el
   contexto de PostgreSQL.
4. Un controlador puede recibir por constructor un caso de uso
   registrado en el contenedor.
   La interfaz IEquipmentRepository está en Application y su
   implementación EquipmentRepository está en Infrastructure. Así,
   Application no necesita conocer los detalles de PostgreSQL.
5. Estado y próximos pasos
- [x] Solución organizada por capas.
- [x] Entidades principales y enum creados en Domain.
- [x] DTOs, casos de uso e interfaz del repositorio creados en
  Application.
- [x] FrameRentDbContext creado en Infrastructure.
- [x] Repositorio de equipos implementado con Entity Framework Core.
- [x] Base de datos PostgreSQL creada.
- [x] Configuración inicial de conexión y API preparada.
- [x] Archivo DependencyInjection.cs creado y paquete necesario
  identificado.
- [ ] Confirmar que DependencyInjection.cs compila sin errores.
- [ ] Registrar los casos de uso dentro de AddApplication.
- [ ] Crear un método de extensión en Infrastructure para registrar
  DbContext y repositorios.
- [ ] Invocar ambos métodos de registro desde Program.cs.
- [ ] Crear o revisar los controladores de la API.
- [ ] Configurar y ejecutar las migraciones de Entity Framework Core.
- [ ] Probar la conexión y los endpoints.
  Importante: no debemos considerar terminado el registro de
  dependencias ni confirmar que todo compila hasta que Build Solution
  termine sin errores en Rider.
6. Resumen para la sustentación
   FrameRent utiliza Clean Architecture para separar responsabilidades.
   Domain modela el negocio; Application contiene los casos de uso y las
   interfaces; Infrastructure implementa el acceso a datos con Entity
   Framework Core y PostgreSQL; y API expone funcionalidades mediante HTTP.

La inyección de dependencias conecta estas piezas durante la ejecución.
La API es el punto de entrada donde se configura el contenedor, mientras
cada capa conserva su responsabilidad. Esta separación facilita las
pruebas, el mantenimiento y los cambios técnicos sin modificar la lógica
central del negocio.