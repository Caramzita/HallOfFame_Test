using HallOfFame.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace HallOfFame.DataAccess.Configurations;

public class PersonCfg : IEntityTypeConfiguration<Person>
{
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

        builder.Property(p => p.Skills)
            .HasConversion(
                skills => JsonSerializer.Serialize(skills, (JsonSerializerOptions)null),
                skills => JsonSerializer.Deserialize<List<Skill>>(skills, (JsonSerializerOptions)null));
    }
}
