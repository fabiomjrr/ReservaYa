USE master;
GO
-- Crear login a nivel servidor si no existe
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'test')
BEGIN
    CREATE LOGIN test WITH PASSWORD = '112255';
    ALTER LOGIN test WITH CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF;
END
GO

-- Crear la base de datos si no existe
IF DB_ID('DEVELOSERS') IS NOT NULL
BEGIN
    ALTER DATABASE DEVELOSERS SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DEVELOSERS;    
END
GO
CREATE DATABASE DEVELOSERS;
-- Crear usuario en la BD y darle permisos
USE DEVELOSERS;
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dev')
BEGIN
    CREATE USER dev FOR LOGIN test;
    ALTER ROLE db_owner ADD MEMBER dev;
END
GO
SET DATEFORMAT dmy; -- para entrada de fechas en formato español
GO

/* =========================
    TABLA: Roles
========================= */
IF OBJECT_ID('Roles') IS NULL
BEGIN
    CREATE TABLE Roles (
        RolID INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );

    INSERT INTO Roles (Nombre) VALUES
    ('Admin'),
    ('Usuario'),
    ('AdminEspacio');
END
GO

/* =========================
    TABLA: Usuarios
========================= */
IF OBJECT_ID('Usuarios') IS NULL
BEGIN
    CREATE TABLE Usuarios (
        UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
        Nombres NVARCHAR(50) NOT NULL,
        Apellidos NVARCHAR(50) NOT NULL,
        FechaNacimiento DATE NOT NULL,
        Correo VARBINARY(MAX) NOT NULL, -- cifrado reversible
        Contrasena VARBINARY(64) NOT NULL, -- hash SHA2_512
        RolID INT NOT NULL 
            CONSTRAINT DF_Usuarios_Rol DEFAULT 2,
        Activo BIT NOT NULL
            CONSTRAINT DF_Usuarios_Activo DEFAULT 1,
        CONSTRAINT FK_Usuarios_Rol FOREIGN KEY (RolID) REFERENCES Roles(RolID)
    );

    -- Usuario de prueba
    INSERT INTO Usuarios (Nombres, Apellidos, FechaNacimiento, Correo, Contrasena, RolID, Activo)
    VALUES (
        'Luis Jonathan',
        'Guevara Ramirez',
        '2003-01-23',
        ENCRYPTBYPASSPHRASE('H225-DEVLSR','prueba@mail.com'),
        HASHBYTES('SHA2_512', CONVERT(VARCHAR(100),'123')),
        1,
        1
    )
END
GO
IF OBJECT_ID('sp_CrearUsuario') IS NOT NULL
    DROP PROCEDURE sp_CrearUsuario;
GO

CREATE PROCEDURE sp_CrearUsuario
    @Nombres NVARCHAR(50),
    @Apellidos NVARCHAR(50),
    @FechaNacimiento DATE,
    @Correo NVARCHAR(200),
    @Contrasena NVARCHAR(200),
    @RolID INT = 2,   -- por defecto Usuario
    @Activo BIT = 1
AS
--=============================================
-- Author: JonathanGuevara
-- Create Date: 08/9/2025
-- Description: RegistroWEA
--=============================================
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuarios (Nombres, Apellidos, FechaNacimiento, Correo, Contrasena, RolID, Activo)
    VALUES (
        @Nombres,
        @Apellidos,
        @FechaNacimiento,

        -- Correo cifrado reversible con frase secreta
        ENCRYPTBYPASSPHRASE('MiClaveDeCifradoUltraSecreta', @Correo),

        -- Contraseña hasheada SHA2_512
        HASHBYTES('SHA2_512', CONVERT(VARBINARY(512), @Contrasena)),

        @RolID,
        @Activo
    );
END
GO
IF OBJECT_ID('sp_Login') IS NOT NULL
    DROP PROCEDURE sp_Login;
GO
CREATE PROCEDURE sp_Login 
    @Correo NVARCHAR(200),
    @Contrasena NVARCHAR(100),
    @Clave NVARCHAR(50) 
AS
--=============================================
-- Author: JonathanGuevara
-- Create Date: 08/9/2025
-- Description: IniciarSesion
--=============================================
BEGIN
    SET NOCOUNT ON; -- evita "N rows affected"

    SELECT 
        u.UsuarioID,
        u.Nombres,
        u.Apellidos,
        u.FechaNacimiento,
        u.RolID,
        r.Nombre AS RolNombre,
        u.Activo
    FROM Usuarios u
    INNER JOIN Roles r ON u.RolID = r.RolID
    WHERE 
        -- Comparar correo (descifrado con la clave)
        CONVERT(NVARCHAR(200), DECRYPTBYPASSPHRASE(@Clave, u.Correo)) = @Correo
        -- Comparar contraseña (hash SHA2_512)
        AND u.Contrasena = HASHBYTES('SHA2_512', CONVERT(NVARCHAR(100), @Contrasena))
        -- Solo usuarios activos
        AND u.Activo = 1;
END
GO
IF OBJECT_ID('Categorias') IS NULL
BEGIN
CREATE TABLE Categorias (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

-- Insertamos las categorías válidas
INSERT INTO Categorias (Nombre)
VALUES 
    ('Salon'),
    ('Auditorio'),
    ('Cancha'),
    ('Laboratorio'),
    ('Sala de conferencias')
END
GO

/* =========================
    TABLA: Espacios
========================= */
IF OBJECT_ID('Espacios') IS NULL
BEGIN
    CREATE TABLE Espacios (
    EspacioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    CategoriaID INT NOT NULL,
    Capacidad INT NOT NULL,
    Direccion NVARCHAR(100) NULL,
    UbicacionEnlace NVARCHAR(MAX) NULL, -- enlace de Google Maps
    Estacionamiento BIT NOT NULL DEFAULT 0,
    Sanitarios BIT NOT NULL DEFAULT 0,
    AccesoSillaRuedas BIT NOT NULL DEFAULT 0,
    ImagenPrev NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Espacios_Categorias FOREIGN KEY (CategoriaID)
        REFERENCES Categorias(CategoriaID),
    Disponible bit not null DEFAULT 1
);


INSERT INTO Espacios (Nombre, CategoriaID, Capacidad, Direccion, UbicacionEnlace, Estacionamiento, Sanitarios, AccesoSillaRuedas, ImagenPrev)
VALUES
('Auditorio', 2, 100, 'Av. Central #123', 'https://maps.google.com/?q=Salón+Principal', 1, 1, 1, 'auditorio.jpg'),
('Estadio', 3, 250, 'Calle Universidad #45', 'https://maps.google.com/?q=Auditorio+Magna', 1, 1, 1, 'estadio.jpg'),
('Laboratorio', 4, 50, 'Av. Deportes #10', 'https://maps.google.com/?q=Cancha+Polideportiva', 1, 1, 0, 'Fondo.jpeg'),
('salon', 1, 30, 'Edificio B, Piso 2', 'https://maps.google.com/?q=Laboratorio+de+Computación', 0, 1, 1, 'classroom.jpg'),
('Sala de Conferencias Ejecutiva', 5, 20, 'Calle Conferencias #8', 'https://maps.google.com/?q=Sala+de+Conferencias+Ejecutiva', 1, 1, 1, 'Lab.jpg')

END
GO

/* =========================
    TABLA: Espacios_Detalles
========================= */
IF OBJECT_ID('EspaciosDetalles') IS NULL
BEGIN
    CREATE TABLE EspaciosDetalles (
        EspacioDetalleID INT IDENTITY(1,1) PRIMARY KEY,
        ValorPorHora DECIMAL(10,2) NOT NULL,
        EspacioID INT NOT NULL,
        CONSTRAINT FK_EspaciosDetalles_Espacios FOREIGN KEY (EspacioID)
            REFERENCES Espacios(EspacioID)
    );
END
GO

/* =========================
    TABLA: Fechas Disponibles (DP)
========================= */
IF OBJECT_ID('FechasDisponibles') IS NULL
BEGIN
    CREATE TABLE FechasDisponibles (
        FechaDisponibleID INT IDENTITY(1,1) PRIMARY KEY,
        Fecha DATE NOT NULL,
        HoraInicio TIME NOT NULL,
        HoraFin TIME NOT NULL,
        Disponible BIT NOT NULL DEFAULT 1
    );
END
GO

/* =========================
    TABLA: Reserva_FechasDisponibles
========================= */
IF OBJECT_ID('ReservasFechasDisponibles') IS NULL
BEGIN
    CREATE TABLE ReservasFechasDisponibles (
        ReservaFechaID INT IDENTITY(1,1) PRIMARY KEY,
        EspacioID INT NOT NULL,
        FechaDisponibleID INT NOT NULL,
        CONSTRAINT FK_ReservasFechasDisponibles_Espacios FOREIGN KEY (EspacioID)
            REFERENCES Espacios(EspacioID),
        CONSTRAINT FK_ReservasFechasDisponibles_FechasDisponibles FOREIGN KEY (FechaDisponibleID)
            REFERENCES FechasDisponibles(FechaDisponibleID)
    );
END
GO

/* =========================
    TABLA: Fechas Ocupadas (OCP)
========================= */
IF OBJECT_ID('FechasOcupadas') IS NULL
BEGIN
    CREATE TABLE FechasOcupadas (
        FechaOcupadaID INT IDENTITY(1,1) PRIMARY KEY,
        Fecha DATE NOT NULL,
        HoraInicio TIME NOT NULL,
        HoraFin TIME NOT NULL,
        Activa BIT NOT NULL DEFAULT 1
    );
END
GO

/* =========================
    TABLA: Reservas
========================= */
IF OBJECT_ID('Reservas') IS NULL
BEGIN
    CREATE TABLE Reservas (
        ReservaID INT IDENTITY(1,1) PRIMARY KEY,
        MontoTotal DECIMAL(10,2) NOT NULL,
        UsuarioID INT NOT NULL,
        FechaOcupadaID INT NULL,
        ReservaFechaID INT NULL,
        CONSTRAINT FK_Reservas_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
        CONSTRAINT FK_Reservas_FechasOcupadas FOREIGN KEY (FechaOcupadaID) REFERENCES FechasOcupadas(FechaOcupadaID),
        CONSTRAINT FK_Reservas_ReservasFechasDisponibles FOREIGN KEY (ReservaFechaID) REFERENCES ReservasFechasDisponibles(ReservaFechaID)
    );
END
GO
