using HallOfFame.Contracts;
using HallOfFame.Core;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using MediatR;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;
using HallOfFame.UseCases.Persons.Commands.DeletePerson;
using HallOfFame.UseCases.Persons.Queries;
using Asp.Versioning;

namespace HallOfFame.API.Controllers;

/// <summary>
/// Предоставляет Rest API для работы с сотрудниками.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PersonsController"/>.
    /// </summary>
    /// <param name="mediator"> Интерфейс для отправки команд и запросов через Mediator. </param>
    /// <param name="mapper"> Интерфейс для маппинга данных между моделями. </param>
    /// <exception cref="ArgumentNullException"> Ошибка загрузки интерфейса. </exception>
    public PersonsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить всех сотрудников.
    /// </summary>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public IAsyncEnumerable<Person> GetAllPersons()
    {
        var query = new GetAllPersonsQuery();

        return _mediator.CreateStream(query);
    }

    /// <summary>
    /// Получить сотрудника по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор сотрудника. </param>
    [MapToApiVersion("1.0")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonById(long id)
    {
        var query = new GetPersonQuery(id);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Добавить сотрудника.
    /// </summary>
    /// <param name="request"> Модель данных сотрудника. </param>
    [MapToApiVersion("1.0")]
    [HttpPost]
    [ProducesResponseType(typeof(Person), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> AddPerson([FromBody] PersonRequestDto request)
    {
        var command = _mapper.Map<AddPersonCommand>(request);
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetPersonById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить данные сотрудника.
    /// </summary>
    /// <param name="id"> Идентификатор сотрудника. </param>
    /// <param name="request"> Модель данных сотрудника. </param>
    [MapToApiVersion("1.0")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(long id, [FromBody] PersonRequestDto request)
    {
        var command = _mapper.Map<UpdatePersonCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    /// <summary>
    /// Удалить сотрудника.
    /// </summary>
    /// <param name="id"> Идентификатор сотрудника. </param>
    [MapToApiVersion("1.0")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(long id)
    {
        var command = new DeletePersonCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }
}
