using HallOfFame.Contracts;
using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using Moq;

public class AddPersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<ISkillRepository> _skillRepositoryMock;
    private readonly AddPersonCommandHandler _handler;

    public AddPersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _skillRepositoryMock = new Mock<ISkillRepository>();
        _handler = new AddPersonCommandHandler(_personRepositoryMock.Object, _skillRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsPerson()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Display", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5),
            new PersonSkillDto ("Skill2", 3)
        });

        _skillRepositoryMock.Setup(repo => repo.AreSkillsValid(It.IsAny<IEnumerable<string>>())).ReturnsAsync(true);
        _personRepositoryMock.Setup(repo => repo.AddPerson(It.IsAny<Person>())).Returns(Task.CompletedTask);
        _personRepositoryMock.Setup(repo => repo.UpdatePerson(It.IsAny<Person>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        Assert.Equal("Test Display", result.DisplayName);
        _personRepositoryMock.Verify(repo => repo.AddPerson(It.IsAny<Person>()), Times.Once);
        _personRepositoryMock.Verify(repo => repo.UpdatePerson(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidSkills_ThrowsArgumentException()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Display", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5)
        });

        _skillRepositoryMock.Setup(repo => repo.AreSkillsValid(It.IsAny<IEnumerable<string>>())).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("One or more skills do not exist in the database.", exception.Message);
    }
}