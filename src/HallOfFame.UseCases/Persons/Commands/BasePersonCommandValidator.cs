using FluentValidation;

namespace HallOfFame.UseCases.Persons.Commands;

public class BasePersonCommandValidator<T> : AbstractValidator<T>
    where T : IPersonCommand
{
    protected BasePersonCommandValidator()
    {
        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Name is required.")
            .Matches(@"\A\S+\z").WithMessage("Name must contain meaningful characters.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(command => command.DisplayName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Display name is required.")
            .Matches(@"\A\S+\z").WithMessage("Display name must contain meaningful characters.")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Display name must not exceed 50 characters.");

        RuleFor(command => command.Skills)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Skills cannot be null.")
            .Must(skills => skills.Count != 0).WithMessage("At least one skill is required.")
            .Must(skills => skills.Select(skill => skill.Name.ToLower()).Distinct().Count() == skills.Count)
            .WithMessage("Duplicate skills are not allowed.")
            .ForEach(skill => skill.SetValidator(new SkillValidator()));
    }
}
