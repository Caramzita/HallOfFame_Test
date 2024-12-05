using FluentValidation.TestHelper;
using HallOfFame.Contracts;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandValidatorTests
{
    private readonly UpdatePersonCommandValidator _validator;

    public UpdatePersonCommandValidatorTests()
    {
        _validator = new UpdatePersonCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ReturnsNoErrors()
    {
        // Arrange
        var command = new UpdatePersonCommand("Test Dis", "Test Dis", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5)
        });
        command.Id = 1L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyId_ReturnsError()
    {
        // Arrange
        var command = new UpdatePersonCommand("Test Dis", "Test Dis", new List<PersonSkillDto>());
        command.Id = 0L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Id)
              .WithErrorMessage("Id is required");
    }

    [Theory]
    [InlineData("", "Display Name", 1)]
    [InlineData(" ", "Display Name", 1)]
    [InlineData(" A", "   Small", 1)]
    [InlineData("Test Dis", "", 1)]
    [InlineData("Test Dis", " ", 1)]
    [InlineData("Test Dis", "Display Name", 0)]
    [InlineData("Test Dis", "Display Name", 11)]
    public void Validate_InvalidCommand_ReturnsErrors(string name, string displayName, byte level)
    {
        // Arrange
        var command = new UpdatePersonCommand(name, displayName, new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", level)
        });
        command.Id = 1L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_EmptySkillList_ReturnsError()
    {
        // Arrange
        var command = new UpdatePersonCommand("Test Dis", "Test Dis", new List<PersonSkillDto>());
        command.Id = 1L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Skills);
    }

    [Fact]
    public void Validate_DuplicateSkills_ReturnsError()
    {
        // Arrange
        var command = new UpdatePersonCommand("Test Dis", "Test Dis", new List<PersonSkillDto>
        {
            new PersonSkillDto ("Skill1", 5),
            new PersonSkillDto ("Skill1", 3)
        });
        command.Id = 1L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Skills);
    }

    [Theory]
    [InlineData(" ", 1)] // Skill with empty name
    [InlineData("", 1)] // Skill with empty name
    [InlineData("a", 1)] // Skill with name too short
    [InlineData("Skill1", 0)] // Skill level below minimum
    [InlineData("Skill2", 11)] // Skill level above maximum
    public void Validate_InvalidSkill_ReturnsErrors(string skillName, byte skillLevel)
    {
        // Arrange
        var command = new UpdatePersonCommand("Test Dis", "Test Dis", new List<PersonSkillDto>
        {
            new PersonSkillDto (skillName, skillLevel)
        });
        command.Id = 1L;

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveAnyValidationError();
    }
}