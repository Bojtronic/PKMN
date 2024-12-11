-- Eliminar base de datos
DROP DATABASE PokeDexDb;
GO

-- Crear base de datos y establecer el contexto
CREATE DATABASE PokeDexDb;
USE PokeDexDb;

-- ======================
-- CREACIÓN DE TABLAS
-- ======================

-- Crear tabla de roles
CREATE TABLE rol (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion VARCHAR(50) NOT NULL
);

-- Crear tabla de usuarios
CREATE TABLE usuario (
    Id INT IDENTITY(1000,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    UserName VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(1024) NOT NULL
);

-- Crear tabla Usuario_Rol
CREATE TABLE usuario_rol (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    IdUsuario INT NOT NULL
);

-- Crear tabla de estados
CREATE TABLE estado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Estado VARCHAR(50) NOT NULL
);

-- Crear tabla de Pokémon del usuario
CREATE TABLE usuario_pkm (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    pkm_id INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Estado INT NOT NULL
);

-- Crear tabla de Pokémon en el bolsillo del usuario
CREATE TABLE usuario_pocket (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    pkm_Id1 INT,
    pkm_Id2 INT,
    pkm_Id3 INT
);

-- ======================
-- CLAVES FORÁNEAS
-- ======================

-- Agregar claves foráneas a Usuario_Rol
ALTER TABLE usuario_rol 
ADD CONSTRAINT fk_IdRol FOREIGN KEY (IdRol) REFERENCES rol(Id);

ALTER TABLE usuario_rol 
ADD CONSTRAINT fk_IdUsuario FOREIGN KEY (IdUsuario) REFERENCES usuario(Id) ON DELETE CASCADE;

-- Agregar claves foráneas a Usuario_Pkm
ALTER TABLE usuario_pkm 
ADD CONSTRAINT fk_usr_Pkm FOREIGN KEY (IdUsuario) REFERENCES usuario(Id) ON DELETE CASCADE;

ALTER TABLE usuario_pkm 
ADD CONSTRAINT fk_estado FOREIGN KEY (Estado) REFERENCES estado(Id);

-- Agregar claves foráneas a Usuario_Pocket
ALTER TABLE usuario_pocket 
ADD CONSTRAINT fk_usr_pkt FOREIGN KEY (IdUsuario) REFERENCES usuario(Id) ON DELETE CASCADE;

-- Modificar las claves foráneas de pkm_Id1, pkm_Id2, pkm_Id3 para evitar ciclos o múltiples rutas de cascada
ALTER TABLE usuario_pocket 
ADD CONSTRAINT fk_usr_pkt_pkm1 FOREIGN KEY (pkm_Id1) REFERENCES usuario_pkm(Id) ON DELETE NO ACTION;

ALTER TABLE usuario_pocket 
ADD CONSTRAINT fk_usr_pkt_pkm2 FOREIGN KEY (pkm_Id2) REFERENCES usuario_pkm(Id) ON DELETE NO ACTION;

ALTER TABLE usuario_pocket 
ADD CONSTRAINT fk_usr_pkt_pkm3 FOREIGN KEY (pkm_Id3) REFERENCES usuario_pkm(Id) ON DELETE NO ACTION;

-- ======================
-- INSERCIONES
-- ======================

-- Insertar roles
INSERT INTO Rol (Descripcion) VALUES ('Admin'), ('User'), ('Guest');

-- Insertar usuarios
INSERT INTO usuario (Nombre, UserName, Password) VALUES ('Administrador', 'Admin', 'pass');

-- Nota: insertar filas en Usuario_Rol después de que existan registros en las tablas Usuario y Rol.
INSERT INTO usuario_rol (IdRol, IdUsuario) VALUES (1, 1000);

-- Insertar estados
INSERT INTO estado (Estado) VALUES ('Activo'), ('Poket'), ('Debilitado');

-- Nota: insertar registros en Usuario_Pkm después de que existan registros en Usuario y Estado.
INSERT INTO usuario_pkm (IdUsuario, pkm_id, Nombre, Estado) 
VALUES (1000, 1, 'Pikachu', 1);

-- Nota: insertar registros en Usuario_Pocket después de que existan registros en Usuario y Usuario_Pkm.
INSERT INTO usuario_pocket (IdUsuario, pkm_Id1, pkm_Id2, pkm_Id3) 
VALUES (1000, 1, NULL, NULL);
