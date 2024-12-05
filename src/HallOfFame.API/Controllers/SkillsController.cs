using Asp.Versioning;
using HallOfFame.Core;
using HallOfFame.UseCases.Skills.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.API.Controllers;

/// <summary>
/// Предоставляет Rest API для работы с навыками.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="SkillsController"/>.
    /// </summary>
    /// <param name="mediator"> Интерфейс для отправки команд и запросов через Mediator. </param>
    /// <exception cref="ArgumentNullException"> Ошибка загрузки интерфейса. </exception>
    public SkillsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить все навыки.
    /// </summary>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public IAsyncEnumerable<Skill> GetAllSkills()
    {
        var query = new GetSkillsQuery();

        return _mediator.CreateStream(query);
    }
}
