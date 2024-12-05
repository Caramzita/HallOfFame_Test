using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Persons.Commands.AddPerson;

/// <summary>
/// Обработчик команды <see cref="AddPersonCommand"/>.
/// </summary>
public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, Person>
{
    private readonly IPersonRepository _personRepository;

    private readonly ISkillRepository _skillRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AddPersonCommandHandler"/>.
    /// </summary>
    /// <param name="personRepository"> Репозиторий сотрудников. </param>
    /// <param name="skillRepository"> Репозиторий навыков. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, если <paramref name="personRepository"/> 
    /// или <paramref name="skillRepository"/> равен <c>null</c>. </exception>
    public AddPersonCommandHandler(IPersonRepository personRepository, ISkillRepository skillRepository)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
    }

    /// <inheritdoc/>
    public async Task<Person> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        var skillNames = request.Skills.Select(ps => ps.Name).Distinct();
        var areSkillsValid = await _skillRepository.AreSkillsValid(skillNames); 

        if (!areSkillsValid)
        {
            throw new ArgumentException("One or more skills do not exist in the database.");
        }

        var newPerson = new Person(request.Name, request.DisplayName);

        await _personRepository.AddPerson(newPerson);

        var personSkills = request.Skills.Select(skillDto =>
            new PersonSkill(newPerson.Id, skillDto.Name, skillDto.Level)).ToList();

        newPerson.UpdatePersonInfo(skills: personSkills);

        await _personRepository.UpdatePerson(newPerson);

        return newPerson;  
    }
}
