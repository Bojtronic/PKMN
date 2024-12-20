﻿using Api_Pdx_Db_V2.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Pdx_Db_V2.Data
{
    public class DbConexionContext : DbContext
    {
        public DbConexionContext(DbContextOptions<DbConexionContext> options) : base(options) { }

        public DbSet<RolModel> rol { get; set; }
        public DbSet<UsuarioModel> usuario { get; set; }
        public DbSet<UsuarioRolModel> usuario_rol { get; set; }
        public DbSet<EstadoModel> estado { get; set; }
        public DbSet<UsuarioPkmModel> usuario_pkm { get; set; }
        public DbSet<UsuarioPktModel> usuario_pocket {  get; set; }
        public DbSet<RetoModel> reto { get; set; }
        public DbSet<MensajesModel> mensajes { get; set; }
        public DbSet<EnfermeriaModel> enfermeria {  get; set; }
        public DbSet<UsuarioRolViewModel> VistaUsuariosRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolModel>().ToTable("rol");

            modelBuilder.Entity<UsuarioModel>().ToTable("usuario");

            modelBuilder.Entity<UsuarioRolModel>().ToTable("usuario_rol");

            modelBuilder.Entity<EstadoModel>().ToTable("estado");

            modelBuilder.Entity<UsuarioPkmModel>().ToTable("usuario_pkm");

            modelBuilder.Entity<UsuarioPktModel>().ToTable("usuario_pocket");

            modelBuilder.Entity<RetoModel>().ToTable("retos");

            modelBuilder.Entity<MensajesModel>().ToTable("mensajesPred");
         
            modelBuilder.Entity<EnfermeriaModel>().ToTable("enfermeria");

            modelBuilder.Entity<UsuarioRolViewModel>()
                .ToTable("vista_usuarios_roles") // Sin espacio adicional
                .HasNoKey();
        }
    }
}
