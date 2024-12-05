using HallOfFame.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallOfFame.DataAccess.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Person"/> для Entity Framework Core.
/// </summary>
public class PersonCfg : IEntityTypeConfiguration<Person>
{
    /// <summary>
    /// Конфигурирует сущность <see cref="Person"/> с использованием указанного <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder"> Строитель типа сущности, используемый для настройки сущности. </param>
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.DisplayName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(p => p.Skills)
            .WithOne()
            .HasForeignKey(ps => ps.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
