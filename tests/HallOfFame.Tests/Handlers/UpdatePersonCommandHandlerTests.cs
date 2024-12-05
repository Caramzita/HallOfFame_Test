using HallOfFame.Contracts;
using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using HallOfFame.Core;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;
using Moq;

public class UpdatePersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<ISkillRepository> _skillRepositoryMock;
    private readonly UpdatePersonCommandHandler _handler;

    public UpdatePersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _skillRepositoryMock = new Mock<ISkillRepository>();
        _handler = new UpdatePersonCommandHandler(_personRepositoryMock.Object, _skillRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesPerson()
    {
        // Arrange
        var personId = 1L;
        var existingPerson = new Person(personId, "Old Name", "Old Display Name", []);
        var command = new UpdatePersonCommand("New Name", "New Display Name", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5),
            new PersonSkillDto ("Skill2", 3)
        });
        command.Id = personId;

        _personRepositoryMock.Setup(repo => repo.GetPersonById(personId)).ReturnsAsync(existingPerson);
        _skillRepositoryMock.Setup(repo => repo.AreSkillsValid(It.IsAny<IEnumerable<string>>())).ReturnsAsync(true);
        _personRepositoryMock.Setup(repo => repo.UpdatePerson(existingPerson)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("New Display Name", result.DisplayName);
        _personRepositoryMock.Verify(repo => repo.UpdatePerson(existingPerson), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        var personId = 1L;
        _personRepositoryMock.Setup(repo => repo.GetPersonById(personId)).ReturnsAsync((Person)null);
        var command = new UpdatePersonCommand("Name", "Display Name", new List<PersonSkillDto>());
        command.Id = personId;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal($"Person with ID {personId} was not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_InvalidSkills_ThrowsArgumentException()
    {
        // Arrange
        var personId = 1L;
        var existingPerson = new Person(personId, "Old Name", "Old Display Name", []);
        var command = new UpdatePersonCommand("New Name", "New Display Name", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5)
        });
        command.Id = personId;

        _personRepositoryMock.Setup(repo => repo.GetPersonById(personId)).ReturnsAsync(existingPerson);
        _skillRepositoryMock.Setup(repo => repo.AreSkillsValid(It.IsAny<IEnumerable<string>>())).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("One or more skills do not exist in the database.", exception.Message);
    }
}