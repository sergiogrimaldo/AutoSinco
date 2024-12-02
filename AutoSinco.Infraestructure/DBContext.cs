using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoSinco.Infraestructure;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acceso> Accesos { get; set; }

    public virtual DbSet<ListaPrecio> ListaPrecios { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoVehiculo> TipoVehiculos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenExpirado> TokenExpirados { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acceso>(entity =>
        {
            entity.HasKey(e => e.IdAcceso).HasName("PK__Acceso__99B2858F0F40E1F9");

            entity.ToTable("Acceso");

            entity.Property(e => e.Contraseña)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Sitio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ListaPrecio>(entity =>
        {
            entity.HasKey(e => e.IdListaPrecios).HasName("PK__ListaPre__2F8C03C5DAAB3AE2");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.TipoVehiculo).WithMany(p => p.ListaPrecios)
                .HasForeignKey(d => d.TipoVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaPrec__TipoV__4D94879B");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__Log__0C54DBC6C0EA3B6C");

            entity.ToTable("Log");

            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Detalle)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C6A62A6B2");

            entity.ToTable("Rol");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdTipoVehiculo).HasName("PK__TipoVehi__DC20741E65C93569");

            entity.ToTable("TipoVehiculo");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("PK__Token__D633244717B17EFA");

            entity.ToTable("Token");

            entity.Property(e => e.IdToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Token__IdUsuario__412EB0B6");
        });

        modelBuilder.Entity<TokenExpirado>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("PK__TokenExp__D6332447B569169C");

            entity.ToTable("TokenExpirado");

            entity.Property(e => e.IdToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TokenExpirados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TokenExpi__IdUsu__440B1D61");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97998DB0B9");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE06866B89B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534DA22C461").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__3E52440B");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__70861215AB7388B6");

            entity.ToTable("Vehiculo");

            entity.HasIndex(e => new { e.TipoVehiculoId, e.Estado }, "IX_Vehiculo_TipoYEstado");

            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Disponible");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Kilometraje).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.TipoVehiculo).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.TipoVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vehiculo__TipoVe__52593CB8");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Venta__BC1240BDB0E75297");

            entity.HasIndex(e => e.FechaVenta, "IX_Venta_Fecha");

            entity.Property(e => e.DocumentoComprador)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuarioVendedor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NombreComprador)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ValorVenta).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdUsuarioVendedorNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuarioVendedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Venta__IdUsuario__59063A47");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Venta__IdVehicul__5812160E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
