using HallOfFame.Contracts;
using HallOfFame.Core;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UpdatePersonIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UpdatePerson_ShouldUpdatePersonInDatabase()
    {
        // Arrange
        var existingPerson = new Person(1, "Test", "Test Display", []);
        await _fixture.PersonRepository.AddPerson(existingPerson);

        var command = new UpdatePersonCommand("Test Updated", "Test Display Updated", new List<PersonSkillDto>
        {
            new PersonSkillDto ("SQL", 5)
        });
        command.Id = 1;

        var handler = new UpdatePersonCommandHandler(_fixture.PersonRepository, _fixture.SkillRepository);

        // Act
        var updatedPerson = await handler.Handle(command, default);

        // Assert
        var retrievedPerson = await _fixture.PersonRepository.GetPersonById(updatedPerson.Id);
        Assert.NotNull(retrievedPerson);
        Assert.Equal("Test Updated", retrievedPerson.Name);
        Assert.Equal("Test Display Updated", retrievedPerson.DisplayName);
    }
}