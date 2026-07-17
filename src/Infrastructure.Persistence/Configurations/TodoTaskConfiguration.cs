// Infrastructure.Persistence/Configurations/TodoTaskConfiguration.cs
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Guardar Enums como Strings en SQLite facilita lecturas directas en DB
        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        // Filtro Global: Por defecto, jamás retorna registros eliminados lógicamente
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}