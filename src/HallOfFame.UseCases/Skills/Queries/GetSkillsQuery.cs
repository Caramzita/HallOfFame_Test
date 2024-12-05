using HallOfFame.Core;
using HallOfFame.Core.Interfaces;
using MediatR;

namespace HallOfFame.UseCases.Skills.Queries;

/// <summary>
/// Запрос для получения всех навыков.
/// </summary>
public record GetSkillsQuery : IStreamRequest<Skill>;

/// <summary>
/// Обработчик запроса <see cref="GetSkillsQuery"/>.
/// Отвечает за извлечение всех навыков из репозитория.
/// </summary>
public class GetSkillsQueryHandler : IStreamRequestHandler<GetSkillsQuery, Skill>
{
    private readonly ISkillRepository _skillRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GetSkillsQueryHandler"/>.
    /// </summary>
    /// <param name="skillRepository"> Репозиторий навыков. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, 
    /// если <paramref name="skillRepository"/> равен <c>null</c>. </exception>
    public GetSkillsQueryHandler(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Skill> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        await foreach (var skill in _skillRepository.GetSkills())
        {
            yield return skill;
        }
    }
}
