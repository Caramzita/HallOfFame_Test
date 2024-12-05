namespace HallOfFame.UseCases.Persons.Commands.AddPerson;

/// <summary>
/// Валидатор команды <see cref="AddPersonCommand"/>.
/// Наследуется от <see cref="BasePersonCommandValidator{T}"/> для использования общих правил валидации.
/// </summary>
public class AddPersonCommandValidator : BasePersonCommandValidator<AddPersonCommand>;
