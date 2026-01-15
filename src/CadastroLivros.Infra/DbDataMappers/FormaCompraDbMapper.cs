using CadastroLivros.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class FormaCompraDbMapper : IEntityTypeConfiguration<FormaCompra>
{
    public void Configure(EntityTypeBuilder<FormaCompra> builder)
    {
        builder.ToTable("FormaCompra");

        builder.HasKey(fc => fc.Codigo);

        builder.Property(fc => fc.Codigo)
            .HasColumnName("Codigo")
            .IsRequired();

        builder.Property(fc => fc.Descricao)
            .HasColumnName("Descricao")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();
    }
}

