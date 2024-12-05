using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.DataAccess.Repositories;

/// <summary>
/// Реализация репозитория навыков <see cref="ISkillRepository"/>.
/// Обеспечивает доступ к данным навыков с использованием контекста базы данных.
/// </summary>
public class SkillRepository : ISkillRepository
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SkillRepository"/> с заданным контекстом базы данных.
    /// </summary>
    /// <param name="context"> Контекст базы данных для работы с данными навыков. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, если <paramref name="context"/> равен <c>null</c>. </exception>
    public SkillRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Skill> GetSkills()
    {
        return _context.Skills
           .AsNoTracking()
           .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Skill?> GetSkillByName(string name)
    {
        return await _context.Skills.FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <inheritdoc/>
    public async Task<bool> AreSkillsValid(IEnumerable<string> skillNames)
    {
        var existingSkillsCount = await _context.Skills
            .CountAsync(skill => skillNames.Contains(skill.Name));

        return existingSkillsCount == skillNames.Count();
    }

    /// <inheritdoc/>
    public async Task AddSkill(Skill skill)
    {
        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteSkill(Skill skill)
    {
        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateSkill(Skill skill)
    {
        _context.Update(skill);
        await _context.SaveChangesAsync();
    }
}