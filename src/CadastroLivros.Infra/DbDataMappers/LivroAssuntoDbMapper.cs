using CadastroLivros.Core.Entities;
using CadastroLivros.Core.Entities.Ternarias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class LivroAssuntoDbMapper : IEntityTypeConfiguration<LivroAssunto>
{
    public void Configure(EntityTypeBuilder<LivroAssunto> builder)
    {
        builder.ToTable("LivroAssunto");

        builder.HasKey(la => new { la.LivroCodigo, la.AssuntoCodigo });

        builder.Property(la => la.LivroCodigo)
            .HasColumnName("LivroCodigo")
            .IsRequired();

        builder.Property(la => la.AssuntoCodigo)
            .HasColumnName("AssuntoCodigo")
            .IsRequired();

        builder.HasOne<Assunto>()
            .WithMany()
            .HasForeignKey(la => la.AssuntoCodigo);
    }
}

