-- Tablas del sistema de autenticación
CREATE TABLE Acceso (
    IdAcceso INT IDENTITY(1,1) PRIMARY KEY,
    Sitio VARCHAR(50),
    Contraseña VARCHAR(250)
);

CREATE TABLE Rol (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200)
);

CREATE TABLE Usuario (
    IdUsuario VARCHAR(20) PRIMARY KEY,
    NombreUsuario VARCHAR(100) NOT NULL UNIQUE,
    Contraseña VARCHAR(50) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    RolId INT NOT NULL,
    Activo BIT DEFAULT 1,
    FOREIGN KEY (RolId) REFERENCES Rol(IdRol)
);

CREATE TABLE Token (
    IdToken VARCHAR(500) PRIMARY KEY,
    IdUsuario VARCHAR(20),
    Ip VARCHAR(15),
    FechaAutenticacion DATETIME,
    FechaExpiracion DATETIME,
    Observacion VARCHAR(200),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE TokenExpirado (
    IdToken VARCHAR(500) PRIMARY KEY,
    IdUsuario VARCHAR(20),
    Ip VARCHAR(15),
    FechaAutenticacion DATETIME,
    FechaExpiracion DATETIME,
    Observacion VARCHAR(200),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE Log (
    IdLog INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario VARCHAR(50),
    Fecha DATETIME DEFAULT GETDATE(),
    Tipo VARCHAR(3),
    Ip VARCHAR(15),
    Accion VARCHAR(100),
    Detalle VARCHAR(5000)
);

-- Tablas para la gestión del concesionario
CREATE TABLE TipoVehiculo (
    IdTipoVehiculo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200),
    LimiteInventario INT NOT NULL -- 10 para carros, 15 para motos
);

CREATE TABLE ListaPrecios (
    IdListaPrecios INT IDENTITY(1,1) PRIMARY KEY,
    Modelo VARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    TipoVehiculoId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (TipoVehiculoId) REFERENCES TipoVehiculo(IdTipoVehiculo),
    CONSTRAINT CHK_PrecioMaximo CHECK (Precio <= 250000000)
);

CREATE TABLE Vehiculo (
    IdVehiculo INT IDENTITY(1,1) PRIMARY KEY,
    TipoVehiculoId INT NOT NULL,
    Modelo VARCHAR(100) NOT NULL,
    Color VARCHAR(50) NOT NULL,
    Kilometraje DECIMAL(10,2) NOT NULL,
    Valor DECIMAL(18,2) NOT NULL,
    ImagenUrl VARCHAR(500) NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EsNuevo BIT NOT NULL,
    Cilindraje INT NULL,
    NumeroVelocidades INT NULL,
    Estado VARCHAR(20) DEFAULT 'Disponible',
    FOREIGN KEY (TipoVehiculoId) REFERENCES TipoVehiculo(IdTipoVehiculo),
    CONSTRAINT CHK_Valor CHECK (Valor <= 250000000),
    CONSTRAINT CHK_Cilindraje CHECK (Cilindraje IS NULL OR Cilindraje <= 400)
);

CREATE TABLE Venta (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdVehiculo INT NOT NULL,
    NombreComprador VARCHAR(200) NOT NULL,
    DocumentoComprador VARCHAR(20) NOT NULL,
    FechaVenta DATETIME DEFAULT GETDATE(),
    ValorVenta DECIMAL(18,2) NOT NULL,
    IdUsuarioVendedor VARCHAR(20) NOT NULL,
    FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo),
    FOREIGN KEY (IdUsuarioVendedor) REFERENCES Usuario(IdUsuario),
    CONSTRAINT CHK_ValorVenta CHECK (ValorVenta <= 250000000)
);

-- Crear índices para optimizar consultas
CREATE INDEX IX_Vehiculo_TipoYEstado ON Vehiculo(TipoVehiculoId, Estado);
CREATE INDEX IX_Venta_Fecha ON Venta(FechaVenta);
CREATE INDEX IX_ListaPrecios_Modelo ON ListaPrecios(Modelo, TipoVehiculoId) WHERE Activo = 1;

-- Datos iniciales para pruebas
-- Acceso para la API
INSERT INTO Acceso (Sitio, Contraseña) VALUES 
('AUTOSINCO', 'AutoSinco2024');

-- Roles del sistema
INSERT INTO Rol (Nombre, Descripcion) VALUES 
(N'Administrador', N'Control total del sistema'),
(N'Vendedor', N'Gestión de ventas y clientes'),
(N'Gerente', N'Supervisión y reportes'),
(N'Inventario', N'Gestión de vehículos');

-- Usuarios de prueba
INSERT INTO Usuario (IdUsuario, NombreUsuario, Contraseña, Nombre, Apellido, Email, RolId) VALUES 
('ADMIN000001', 'admin', 'Admin123*', 'Administrador', 'Sistema', 'admin@autosinco.com', 1),
('VEND000001', 'vendedor1', 'Vendedor123*', 'Juan', 'Pérez', 'juan.perez@autosinco.com', 2),
('GERE000001', 'gerente1', 'Gerente123*', 'María', 'González', 'maria.gonzalez@autosinco.com', 3),
('INVE000001', 'inventario1', 'Inventario123*', 'Carlos', 'Rodríguez', 'carlos.rodriguez@autosinco.com', 4);

-- Tipos de vehículo
INSERT INTO TipoVehiculo (Nombre, Descripcion, LimiteInventario) VALUES 
('Carro', 'Vehículos de cuatro ruedas', 10),
('Moto', 'Vehículos de dos ruedas', 15);

-- Lista de precios de vehículos nuevos
INSERT INTO ListaPrecios (Modelo, Precio, TipoVehiculoId, Activo) VALUES 
('Mazda 3 híbrido', 100000000, 1, 1),
('Toyota Corolla', 95000000, 1, 1),
('Honda CBR 250', 25000000, 2, 1),
('Yamaha MT-03', 28000000, 2, 1),
('KTM Duke 390', 35000000, 2, 1);

-- Vehículos de muestra
INSERT INTO Vehiculo (TipoVehiculoId, Modelo, Color, Kilometraje, Valor, EsNuevo, Cilindraje, NumeroVelocidades, Estado) VALUES 
-- Carros nuevos
(1, 'Mazda 3 híbrido', 'Rojo', 0, 100000000, 1, NULL, NULL, 'Disponible'),
(1, 'Toyota Corolla', 'Blanco', 0, 95000000, 1, NULL, NULL, 'Disponible'),
-- Carros usados
(1, 'Mazda 3 híbrido', 'Azul', 15000, 85000000, 0, NULL, NULL, 'Disponible'),
-- Motos nuevas
(2, 'Honda CBR 250', 'Negro', 0, 25000000, 1, 250, 6, 'Disponible'),
(2, 'Yamaha MT-03', 'Azul', 0, 28000000, 1, 321, 6, 'Disponible'),
-- Motos usadas
(2, 'KTM Duke 390', 'Naranja', 5000, 30000000, 0, 373, 6, 'Disponible');

-- Ventas de muestra
INSERT INTO Venta (IdVehiculo, NombreComprador, DocumentoComprador, ValorVenta, IdUsuarioVendedor) VALUES 
(1, 'Pedro Martínez', '123456789', 100000000, 'VEND000001'),
(4, 'Ana López', '987654321', 25000000, 'VEND000001');

-- Actualizar estado de vehículos vendidos
UPDATE Vehiculo SET Estado = 'Vendido' WHERE IdVehiculo IN (1, 4);