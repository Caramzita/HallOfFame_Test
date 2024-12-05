using HallOfFame.Core.Exceptions;
using HallOfFame.Core.Interfaces;
using HallOfFame.Core;
using HallOfFame.UseCases.Persons.Commands.DeletePerson;
using Moq;

public class DeletePersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly DeletePersonCommandHandler _handler;

    public DeletePersonCommandHandlerTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _handler = new DeletePersonCommandHandler(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_DeletesPerson()
    {
        // Arrange
        var personId = 1L;
        var person = new Person(personId, "Test", "Test Display", []);
        _personRepositoryMock.Setup(repo => repo.GetPersonById(personId)).ReturnsAsync(person);
        _personRepositoryMock.Setup(repo => repo.DeletePerson(person)).Returns(Task.CompletedTask);

        var command = new DeletePersonCommand(personId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _personRepositoryMock.Verify(repo => repo.GetPersonById(personId), Times.Once);
        _personRepositoryMock.Verify(repo => repo.DeletePerson(person), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        var personId = 1L;
        _personRepositoryMock.Setup(repo => repo.GetPersonById(personId)).ReturnsAsync((Person)null);

        var command = new DeletePersonCommand(personId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal($"Person with ID {personId} was not found.", exception.Message);
    }
}