using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class LivroFormaCompraDbMapper : IEntityTypeConfiguration<LivroFormaCompra>
{
    public void Configure(EntityTypeBuilder<LivroFormaCompra> builder)
    {
        builder.ToTable("LivroFormaCompra");

        builder.HasKey(lfc => new { lfc.LivroCodigo, lfc.FormaCompraCodigo });

        builder.Property(lfc => lfc.LivroCodigo)
            .HasColumnName("LivroCodigo")
            .IsRequired();

        builder.Property(lfc => lfc.FormaCompraCodigo)
            .HasColumnName("FormaCompraCodigo")
            .IsRequired();

        builder.Property(lfc => lfc.ValorCompra)
            .HasColumnName("ValorCompra")
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();

        builder.HasOne<FormaCompra>()
            .WithMany()
            .HasForeignKey(lfc => lfc.FormaCompraCodigo);
    }
}

