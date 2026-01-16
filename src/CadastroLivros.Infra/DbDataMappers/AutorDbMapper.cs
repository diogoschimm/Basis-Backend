using CadastroLivros.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class AutorDbMapper : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("Autor");

        builder.HasKey(a => a.Codigo);

        builder.Property(a => a.Codigo)
            .HasColumnName("Codigo")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Nome)
            .HasColumnName("Nome")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();
    }
}

