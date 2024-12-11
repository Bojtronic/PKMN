create schema PokeDexDb;
USE PokeDexDb;
-- Utilisar solo  Id para todas las tablas dado que genera problemas al llamar en Vs Code 
CREATE TABLE rol(
	Id int PRIMARY KEY AUTO_INCREMENT,
    Descripcion VARCHAR(50)
);

INSERT INTO Rol (Descripcion) VALUES ('Admin');
INSERT INTO Rol (Descripcion) VALUES ('User');
INSERT INTO Rol (Descripcion) VALUES ('Guest');

create table Usuario(
	Id int primary key auto_increment,
    Nombre varchar(100),
    UserName varchar(100),
    Pass varchar(1024)
) auto_increment = 1000;  -- Para aumentar a partir de 1000
insert into usuario (Nombre,UserName,Pass) values ('Administrador','Admin','pass');

create table Usuario_Rol(
	Id int PRIMARY KEY  AUTO_INCREMENT,
    IdRol int,
    IdUsuario int
);
alter table usuario_rol add constraint foreign key fk_IdRol(IdRol) references rol (Id);
-- para que se borre automaticamente al borrar un usuario 
ALTER TABLE usuario_rol ADD CONSTRAINT fk_IdUsuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id) ON DELETE CASCADE;
-- alter table usuario_rol drop constraint fk_IdUsuario;

insert into Usuario_Rol (IdRol,IdUsuario) values (1,1000);

create table estado (
 Id int primary key,
 Estado varchar(50)
 );
 
insert into estado values (1, 'Activo');
insert into estado values (2, 'Poket');
insert into estado values (3, 'Debilitado');


CREATE TABLE usuario_pkm (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdUsuario INT,
    pkm_id INT,
    nombre VARCHAR(100),
    estado int,
    CONSTRAINT fk_usr_Pkm FOREIGN KEY (IdUsuario) 
    REFERENCES Usuario(Id) ON DELETE CASCADE,
    constraint fk_estado foreign key (estado) references estado (Id)
);
CREATE TABLE usuario_pocket (
    id INT AUTO_INCREMENT PRIMARY KEY,
    IdUsuario INT,
    pkm_Id1 INT,
    pkm_Id2 INT,
    pkm_Id3 INT,
    CONSTRAINT fk_usr_pkt FOREIGN KEY (idUsuario) REFERENCES usuario(Id) ON DELETE CASCADE,
    CONSTRAINT fk_usr_pkt_pkm1 FOREIGN KEY (pkm_Id1) REFERENCES usuario_pkm(Id) ON DELETE CASCADE,
    CONSTRAINT fk_usr_pkt_pkm2 FOREIGN KEY (pkm_Id2) REFERENCES usuario_pkm(Id) ON DELETE CASCADE,
    CONSTRAINT fk_usr_pkt_pkm3 FOREIGN KEY (pkm_Id3) REFERENCES usuario_pkm(Id) ON DELETE CASCADE
);

gfh







