using FluentValidation.TestHelper;
using HallOfFame.Contracts;
using HallOfFame.UseCases.Persons.Commands.AddPerson;

public class AddPersonCommandValidatorTests
{
    private readonly AddPersonCommandValidator _validator;

    public AddPersonCommandValidatorTests()
    {
        _validator = new AddPersonCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ReturnsNoErrors()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Display", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Display", 1)]
    [InlineData("A", "Display", 1)]
    [InlineData("Test", "", 1)]
    [InlineData("Test", "D", 1)]
    [InlineData("Test", "Display", 0)]
    [InlineData("Test", "Display", 11)]
    public void Validate_InvalidCommand_ReturnsErrors(string name, string displayName, byte level)
    {
        // Arrange
        var command = new AddPersonCommand(name, displayName, new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", level)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_EmptySkillList_ReturnsError()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Disp", new List<PersonSkillDto>());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Skills);
    }

    [Fact]
    public void Validate_DuplicateSkills_ReturnsError()
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Disp", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5),
            new PersonSkillDto ("Skill1", 3)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Skills);
    }

    [Theory]
    [InlineData(" ", 1)]
    [InlineData("", 1)]
    [InlineData("a", 1)]
    [InlineData("Skill1", 0)]
    [InlineData("Skill2", 11)]
    public void Validate_InvalidSkill_ReturnsErrors(string skillName, byte skillLevel)
    {
        // Arrange
        var command = new AddPersonCommand("Test", "Test Dis", new List<PersonSkillDto>
        {
            new PersonSkillDto (skillName, skillLevel)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveAnyValidationError();
    }
}