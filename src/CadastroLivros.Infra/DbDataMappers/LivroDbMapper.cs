using CadastroLivros.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class LivroDbMapper : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("Livro");

        builder.HasKey(l => l.Codigo);

        builder.Property(l => l.Codigo)
            .HasColumnName("Codigo")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Titulo)
            .HasColumnName("Titulo")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();

        builder.Property(l => l.Editora)
            .HasColumnName("Editora")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();

        builder.Property(l => l.Edicao)
            .HasColumnName("Edicao")
            .IsRequired();

        builder.Property(l => l.AnoPublicacao)
            .HasColumnName("AnoPublicacao")
            .HasColumnType("VARCHAR(4)")
            .IsRequired();

        builder.HasMany(l => l.LivroAutores)
            .WithOne()
            .HasForeignKey(la => la.LivroCodigo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.LivroAssuntos)
            .WithOne()
            .HasForeignKey(la => la.LivroCodigo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.LivroFormasCompra)
            .WithOne()
            .HasForeignKey(lfc => lfc.LivroCodigo)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

