namespace HallOfFame.Contracts;

/// <summary>
/// Представляет навык сотрудника с уровнем владения.
/// </summary>
/// <param name="Name"> Название навыка. </param>
/// <param name="Level"> Уровень владения навыком (от 1 до 10). </param>
public record PersonSkillDto(string Name, byte Level);
