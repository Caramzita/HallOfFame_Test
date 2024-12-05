using HallOfFame.Core;
using HallOfFame.UseCases.Persons.Commands.DeletePerson;

public class DeletePersonIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DeletePersonIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DeletePerson_ShouldRemovePersonFromDatabase()
    {
        // Arrange
        var person = new Person(1, "Test", "Test Display", []);
        await _fixture.PersonRepository.AddPerson(person);

        var command = new DeletePersonCommand(1);
        var handler = new DeletePersonCommandHandler(_fixture.PersonRepository);

        // Act
        await handler.Handle(command, default);

        // Assert
        var deletedPerson = await _fixture.PersonRepository.GetPersonById(1);
        Assert.Null(deletedPerson);
    }
}