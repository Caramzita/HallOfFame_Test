using HallOfFame.Contracts;
using HallOfFame.UseCases.Persons.Commands.AddPerson;

public class AddPersonIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public AddPersonIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddPerson_ShouldAddPersonToDatabase()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Display", new List<PersonSkillDto>
        {
            new PersonSkillDto ("C#", 5)
        });

        var handler = new AddPersonCommandHandler(_fixture.PersonRepository, _fixture.SkillRepository);

        // Act
        var person = await handler.Handle(command, default);

        // Assert
        var addedPerson = await _fixture.PersonRepository.GetPersonById(person.Id);
        Assert.NotNull(addedPerson);
        Assert.Equal("Test", addedPerson.Name);
        Assert.Equal("Test Display", addedPerson.DisplayName);
    }
}