using FluentValidation;
using HallOfFame.Contracts;

namespace HallOfFame.UseCases.Persons.Commands;

/// <summary>
/// Валидатор для DTO навыков сотрудников <see cref="PersonSkillDto"/>.
/// Определяет правила валидации для свойств, связанных с навыками сотрудника.
/// </summary>
public class PersonSkillValidator : AbstractValidator<PersonSkillDto>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PersonSkillValidator"/>.
    /// </summary>
    public PersonSkillValidator()
    {
        RuleFor(skill => skill.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Skill name is required.")
            .MinimumLength(2).WithMessage("Skill name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Skill name must not exceed 50 characters.");

        RuleFor(skill => skill.Level)
            .InclusiveBetween((byte)1, (byte)10).WithMessage("Skill level must be between 1 and 10.");
    }
}
