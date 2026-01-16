using CadastroLivros.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadastroLivros.Infra.DbDataMappers;

public class AssuntoDbMapper : IEntityTypeConfiguration<Assunto>
{
    public void Configure(EntityTypeBuilder<Assunto> builder)
    {
        builder.ToTable("Assunto");

        builder.HasKey(a => a.Codigo);

        builder.Property(a => a.Codigo)
            .HasColumnName("Codigo")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Descricao)
            .HasColumnName("Descricao")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();
    }
} 