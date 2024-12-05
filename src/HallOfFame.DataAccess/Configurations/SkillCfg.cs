using HallOfFame.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallOfFame.DataAccess.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Skill"/> для Entity Framework Core.
/// </summary>
public class SkillCfg : IEntityTypeConfiguration<Skill>
{
    /// <summary>
    /// Конфигурирует сущность <see cref="Skill"/> с использованием указанного <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder"> Строитель типа сущности, используемый для настройки сущности. </param>
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.Name);

        builder.Property(s => s.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            new Skill("C#"),
            new Skill("Java"),
            new Skill("Python"),
            new Skill("SQL"),
            new Skill("JavaScript")
        );
    }
}
