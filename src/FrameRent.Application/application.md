1. Descripción general

La capa FrameRent.Application contiene los casos de uso de la aplicación y define las operaciones que el sistema puede realizar.

Su responsabilidad principal es coordinar la lógica de aplicación utilizando las entidades definidas en Domain y las interfaces de repositorio que describen cómo se accederá a los datos y cómo se solicitará su persistencia.

Esta capa no accede directamente a la base de datos ni depende de Entity Framework Core. En su lugar, depende de abstracciones, siguiendo los principios de la Arquitectura Limpia (Clean Architecture).

2. Responsabilidades

La capa Application se encarga de:

Definir los casos de uso del sistema.

Coordinar las operaciones que utilizan entidades de Domain.

Definir las interfaces de repositorio que necesita la aplicación.

Recibir datos de entrada mediante DTOs (Data Transfer Objects).

Gestionar condiciones propias de los casos de uso, como comprobar si un equipo existe antes de actualizarlo.

Delegar el acceso y la persistencia de datos a las implementaciones de los repositorios.

3. Estructura del proyecto

FrameRent.Application/
│
├── DTOs/
│   └── Equipment/
│       ├── CreateEquipmentRequest.cs
│       └── UpdateEquipmentRequest.cs
│
├── Interfaces/
│   └── Repositories/
│       └── IEquipmentRepository.cs
│
└── UseCases/
└── EquipmentManagement/
├── CreateEquipment/
│   └── CreateEquipmentUseCase.cs
│
├── GetEquipmentById/
│   └── GetEquipmentByIdUseCase.cs
│
├── GetAllEquipment/
│   └── GetAllEquipmentUseCase.cs
│
├── UpdateEquipment/
│   └── UpdateEquipmentUseCase.cs
│
├── ActivateEquipment/
│   └── ActivateEquipmentUseCase.cs
│
└── DeactivateEquipment/
└── DeactivateEquipmentUseCase.cs

4. Objetos de transferencia de datos (DTOs)

Los DTOs definen los datos que reciben las operaciones de la aplicación. Permiten separar los datos de entrada de las entidades del dominio.

CreateEquipmentRequest

Contiene la información necesaria para crear un equipo:

Name: nombre.

Category: categoría.

Description: descripción.

DailyRate: tarifa diaria.

TotalUnits: cantidad total de unidades.

UpdateEquipmentRequest

Contiene la información que actualmente se permite actualizar en un equipo:

Description: descripción.

DailyRate: tarifa diaria.

TotalUnits: cantidad total de unidades.

La entidad controla cómo se aplican estos cambios y valida las reglas de negocio correspondientes.

5. Interfaz del repositorio

La interfaz IEquipmentRepository define las operaciones de acceso a datos que necesita Application.

Incluye los siguientes métodos:

AddAsync(Equipment equipment): solicita agregar un equipo.

GetByIdAsync(Guid id): solicita obtener un equipo por su identificador.

GetAllAsync(): solicita obtener todos los equipos.

ExistsAsync(Guid id): permite comprobar si existe un equipo.

UpdateAsync(Equipment equipment): solicita actualizar un equipo.

La interfaz no define cómo se implementan estas operaciones. La implementación concreta será responsabilidad de Infrastructure.

6. Casos de uso implementados

CreateEquipmentUseCase

Crea una nueva entidad Equipment utilizando los datos recibidos mediante CreateEquipmentRequest y solicita al repositorio que la persista.

GetEquipmentByIdUseCase

Recibe el identificador de un equipo y solicita recuperar la entidad correspondiente. Si no existe, el repositorio puede devolver null.

GetAllEquipmentUseCase

Solicita al repositorio la colección de equipos registrados.

UpdateEquipmentUseCase

Busca un equipo, comprueba si existe y actualiza su descripción, tarifa diaria y cantidad de unidades utilizando los métodos de la entidad. Finalmente, solicita que el repositorio persista los cambios.

ActivateEquipmentUseCase

Busca un equipo, comprueba si existe, lo activa mediante el método de dominio correspondiente y solicita su actualización a través del repositorio.

DeactivateEquipmentUseCase

Busca un equipo, comprueba si existe, lo desactiva mediante el método de dominio correspondiente y solicita su actualización a través del repositorio.

7. Flujo de dependencias

Application utiliza las entidades de Domain y depende de sus propias abstracciones de acceso a datos.

FrameRent.Application
|
├── Utiliza entidades de Domain
|
└── Depende de IEquipmentRepository
↑
|
Implementado por Infrastructure

Application no depende de la implementación concreta de Infrastructure.

Esto permite cambiar la tecnología de acceso a datos sin modificar los casos de uso para adaptarlos a los detalles de la base de datos.

8. Inyección de dependencias

Los casos de uso reciben IEquipmentRepository a través de sus constructores.

Por ejemplo:

public CreateEquipmentUseCase(
IEquipmentRepository equipmentRepository)
{
_equipmentRepository = equipmentRepository;
}

Este mecanismo se denomina inyección de dependencias por constructor. La dependencia será proporcionada mediante la configuración de inyección de dependencias de la aplicación, que se completará en la capa correspondiente.

9. Principios de Arquitectura Limpia aplicados

La implementación sigue estos principios:

Separación de responsabilidades: cada caso de uso representa una operación específica.

Inversión de dependencias: Application depende de una abstracción del repositorio y no de una implementación concreta de la base de datos.

Encapsulación del dominio: los cambios se realizan mediante métodos de las entidades, respetando sus propiedades con private set.

Facilidad de pruebas: los casos de uso pueden probarse mediante implementaciones simuladas o alternativas de las interfaces de repositorio.

Independencia de Infrastructure: Application no necesita conocer los detalles de Entity Framework Core ni de la base de datos.

10. Estado actual

Se ha implementado la primera versión de la capa Application para la gestión de equipos y la solución ha compilado correctamente.

La siguiente etapa consiste en implementar el repositorio en FrameRent.Infrastructure, configurar la persistencia con Entity Framework Core y conectar los casos de uso con la API mediante inyección de dependencias.

Resumen / Summary

English: The Application layer defines what the system can do, coordinates the use cases, and depends on repository abstractions rather than concrete data access implementations.

Español: La capa Application define qué puede hacer el sistema, coordina los casos de uso y depende de abstracciones de repositorio en lugar de implementaciones concretas de acceso a datos.