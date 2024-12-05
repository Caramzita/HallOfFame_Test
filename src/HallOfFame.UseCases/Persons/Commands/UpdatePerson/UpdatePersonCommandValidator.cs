using FluentValidation;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

/// <summary>
/// Валидатор команды <see cref="UpdatePersonCommand"/>.
/// Наследуется от <see cref="BasePersonCommandValidator{T}"/> для использования общих правил валидации.
/// </summary>
public class UpdatePersonCommandValidator : BasePersonCommandValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Id is required");
    }
}