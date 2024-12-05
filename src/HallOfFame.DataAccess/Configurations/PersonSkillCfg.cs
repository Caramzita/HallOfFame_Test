using HallOfFame.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallOfFame.DataAccess.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="PersonSkill"/> для Entity Framework Core.
/// </summary>
public class PersonSkillCfg : IEntityTypeConfiguration<PersonSkill>
{
    /// <summary>
    /// Конфигурирует сущность <see cref="PersonSkill"/> с использованием указанного <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder"> Строитель типа сущности, используемый для настройки сущности. </param>
    public void Configure(EntityTypeBuilder<PersonSkill> builder)
    {
        builder.HasKey(ps => new { ps.PersonId, ps.SkillName });

        builder.Property(ps => ps.Level)
            .IsRequired();

        builder.HasOne<Person>()
            .WithMany(p => p.Skills)
            .HasForeignKey(ps => ps.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Skill>()
            .WithMany()
            .HasForeignKey(ps => ps.SkillName)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
