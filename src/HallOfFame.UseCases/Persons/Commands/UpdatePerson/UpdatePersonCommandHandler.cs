using HallOfFame.Core;
using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.UpdatePerson;

/// <summary>
/// Обработчик команды <see cref="UpdatePersonCommand"/>.
/// </summary>
public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Person>
{
    private readonly IPersonRepository _personRepository;

    private readonly ISkillRepository _skillRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UpdatePersonCommandHandler"/>.
    /// </summary>
    /// <param name = "personRepository" > Репозиторий сотрудников. </param>
    /// <param name="skillRepository"> Репозиторий навыков. </param>
    /// <exception cref="ArgumentNullException">  Выбрасывается, если <paramref name="personRepository"/> 
    /// или <paramref name="skillRepository"/> равен <c>null</c>. </exception>
    public UpdatePersonCommandHandler(IPersonRepository personRepository, ISkillRepository skillRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
    }

    /// <inheritdoc/>
    public async Task<Person> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonById(request.Id);

        if (person == null)
        {
            throw new NotFoundException($"Person with ID {request.Id} was not found.");
        }

        var skillNames = request.Skills.Select(ps => ps.Name).Distinct();
        var areSkillsValid = await _skillRepository.AreSkillsValid(skillNames);

        if (!areSkillsValid)
        {
            throw new ArgumentException("One or more skills do not exist in the database.");
        }

        var updatedSkills = request.Skills?.Select(skillDto =>
            new PersonSkill(person.Id, skillDto.Name, skillDto.Level)).ToList();

        person.UpdatePersonInfo(
            name: request.Name,
            displayName: request.DisplayName,
            skills: updatedSkills
        );

        await _personRepository.UpdatePerson(person);

        return person;
    }
}
