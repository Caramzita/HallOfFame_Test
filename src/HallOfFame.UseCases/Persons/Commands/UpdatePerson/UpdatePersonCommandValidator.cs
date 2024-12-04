using FluentValidation;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : BasePersonCommandValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Id is required");
    }
}
