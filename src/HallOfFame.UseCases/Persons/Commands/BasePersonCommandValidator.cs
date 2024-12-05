using FluentValidation;

namespace HallOfFame.UseCases.Persons.Commands;

/// <summary>
/// Базовый валидатор для команд, связанных с сущностью <see cref="IPersonCommand"/>.
/// Реализует общие правила валидации для команд, таких как название и навыки.
/// </summary>
/// <typeparam name="T">Тип команды, наследуемый от <see cref="IPersonCommand"/>.</typeparam>
public class BasePersonCommandValidator<T> : AbstractValidator<T>
    where T : IPersonCommand
{
    /// <summary>
    /// Инициализирует новый экземпляр базового валидатора.
    /// </summary>
    protected BasePersonCommandValidator()
    {
        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Name is required.")
            .Matches(@"^(?! )[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$").WithMessage("Name must contain meaningful characters " +
                "and no leading or trailing spaces, and only single spaces between words.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(command => command.DisplayName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Display name is required.")
            .Matches(@"^(?! )[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$").WithMessage("Name must contain meaningful characters " +
                "and no leading or trailing spaces, and only single spaces between words.")
            .MinimumLength(2).WithMessage("Display name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Display name must not exceed 50 characters.");

        RuleFor(command => command.Skills)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Skills cannot be null.")
            .Must(skills => skills.Count != 0).WithMessage("At least one skill is required.")
            .Must(skills => skills.Select(skill => skill.Name.ToLower()).Distinct().Count() == skills.Count)
                .WithMessage("Duplicate skills are not allowed.")
            .ForEach(skill => skill.SetValidator(new PersonSkillValidator()));
    }
}
