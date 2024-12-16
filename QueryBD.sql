-- Eliminar la base de datos existente y crearla nuevamente
DROP DATABASE IF EXISTS PokeDexDb;
CREATE DATABASE PokeDexDb;
USE PokeDexDb;

-- Crear las tablas principales
CREATE TABLE Rol (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Descripcion VARCHAR(50)
);

CREATE TABLE Usuario (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombre VARCHAR(100),
    UserName VARCHAR(100),
    Pass VARCHAR(1024)
) AUTO_INCREMENT = 1000;

CREATE TABLE Usuario_Rol (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdRol INT,
    IdUsuario INT
);

CREATE TABLE Estado (
    Id INT PRIMARY KEY,
    Estado VARCHAR(50)
);

CREATE TABLE Usuario_Pkm (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdUsuario INT,
    Pkm_Id INT,
    Nombre VARCHAR(100),
    Estado INT
);

CREATE TABLE Usuario_Pocket (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdUsuario INT,
    Pkm_Id1 INT,
    Pkm_Id2 INT,
    Pkm_Id3 INT
);

CREATE TABLE Enfermeria (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdUsuario INT NOT NULL,
    IdUsuPkm INT NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Agregar llaves foráneas después de la creación de las tablas
ALTER TABLE Usuario_Rol
ADD CONSTRAINT fk_IdRol FOREIGN KEY (IdRol) REFERENCES Rol(Id),
ADD CONSTRAINT fk_IdUsuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE;

ALTER TABLE Usuario_Pkm
ADD CONSTRAINT fk_usr_Pkm FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_estado FOREIGN KEY (Estado) REFERENCES Estado(Id);

ALTER TABLE Usuario_Pocket
ADD CONSTRAINT fk_usr_pkt FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_usr_pkt_pkm1 FOREIGN KEY (Pkm_Id1) REFERENCES Usuario_Pkm(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_usr_pkt_pkm2 FOREIGN KEY (Pkm_Id2) REFERENCES Usuario_Pkm(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_usr_pkt_pkm3 FOREIGN KEY (Pkm_Id3) REFERENCES Usuario_Pkm(Id) ON DELETE CASCADE;

-- Agregar las llaves foráneas
ALTER TABLE Enfermeria
ADD CONSTRAINT fk_enfermeria_usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_enfermeria_usupkm FOREIGN KEY (IdUsuPkm) REFERENCES Usuario_Pkm(Id) ON DELETE CASCADE;-- Agregar las llaves foráneas
ALTER TABLE Enfermeria
ADD CONSTRAINT fk_enfermeria_usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE,
ADD CONSTRAINT fk_enfermeria_usupkm FOREIGN KEY (IdUsuPkm) REFERENCES Usuario_Pkm(Id) ON DELETE CASCADE;



-- Insertar datos iniciales en las tablas
INSERT INTO Rol (Descripcion) VALUES ('Admin'), ('User'), ('Guest');
INSERT INTO Usuario (Nombre, UserName, Pass) VALUES ('Administrador', 'Admin', 'pass');
INSERT INTO Usuario_Rol (IdRol, IdUsuario) VALUES (1, 1000);

INSERT INTO Estado VALUES 
(1, 'Activo'),
(2, 'Poket'),
(3, 'Debilitado');

-- Crear TRIGGERS y VIEWS
DELIMITER //
CREATE TRIGGER after_usuario_insert
AFTER INSERT ON Usuario
FOR EACH ROW
BEGIN
    INSERT INTO Usuario_Rol (IdRol, IdUsuario) VALUES (2, NEW.Id);
END;
//
DELIMITER ;

CREATE VIEW vista_usuarios_roles AS
SELECT 
    u.Nombre AS Nombre, 
    u.UserName AS UserName, 
    r.Descripcion AS Descripcion, 
    ur.IdRol AS IdRol
FROM 
    Usuario u
    JOIN Usuario_Rol ur ON u.Id = ur.IdUsuario
    JOIN Rol r ON ur.IdRol = r.Id;


