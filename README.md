# AutoSinco - Backend

Este es el backend del sistema de administración de vehículos para un concesionario, desarrollado en **C# con .NET Core** y usando **SQL Server** para almacenamiento de datos.

## Tecnologías utilizadas
- C#
- .NET Core
- SQL Server
- Entity Framework Core

## Requisitos previos

- .NET Core SDK (versión mínima: 6.0)
- SQL Server
- Herramientas de desarrollo como Visual Studio o Visual Studio Code

## Instalación

1. Clona este repositorio en tu máquina local.
2. Instala las dependencias ejecutando el siguiente comando en la terminal:
   ```bash
   dotnet restore
3. Configura la conexión a la base de datos en el archivo appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=Concesionario;Trusted_Connection=True;"
  }
}
## Endpoints principales
- GET /vehiculos - Consultar lista de vehículos.
- POST /vehiculos - Registrar un vehículo.
- PUT /vehiculos/{id} - Editar un vehículo existente.
- DELETE /vehiculos/{id} - Vender un vehículo.
- GET /reportes/suma-valores - Obtener suma del valor por tipo de vehículo y modelo.

## Validaciones implementadas
- Vehículos usados no pueden tener un valor menor al 85% del valor indicado en la lista de precios.
- Máximo 10 carros y 15 motos en el inventario.
- Solo se permiten motos de cilindraje ≤ 400 cc.
- No se aceptan vehículos con valor mayor a $250,000,000.
- Scripts de base de datos
- Se encuentra disponible el archivo Scripts/Concesionario.sql para crear y poblar la base de datos.

## Ejecución
Ejecuta la aplicación:

dotnet run
Accede a la API desde herramientas como Postman.
