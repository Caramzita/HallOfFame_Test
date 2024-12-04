using FluentValidation;
using HallOfFame.Core;

namespace HallOfFame.UseCases.Persons.Commands;

public class SkillValidator : AbstractValidator<Skill>
{
    public SkillValidator()
    {
        RuleFor(skill => skill.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Skill name is required.")
            .Matches(@"\A\S+\z").WithMessage("Skill name must contain meaningful characters.")
            .MinimumLength(2).WithMessage("Skill name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Skill name must not exceed 50 characters.");

        RuleFor(skill => skill.Level)
            .InclusiveBetween((byte)1, (byte)10).WithMessage("Skill level must be between 1 and 10.");
    }
}
