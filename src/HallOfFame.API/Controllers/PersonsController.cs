using HallOfFame.Contracts;
using HallOfFame.Core;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using MediatR;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;
using HallOfFame.UseCases.Persons.Commands.DeletePerson;
using HallOfFame.UseCases.Persons.Queries;

namespace HallOfFame.API.Controllers
{
    [Route("api/v1/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IMediator _mediator;

        public PersonsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public IAsyncEnumerable<Person> GetAllPersons()
        {
            var query = new GetAllProfilesQuery();

            return _mediator.CreateStream(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(long id)
        {
            var query = new GetPersonQuery(id);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Person), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> AddPerson([FromBody] PersonRequestDto request)
        {
            var command = _mapper.Map<AddPersonCommand>(request);
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetPersonById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(long id, [FromBody] PersonRequestDto request)
        {
            var command = _mapper.Map<UpdatePersonCommand>(request);
            command.Id = id;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var command = new DeletePersonCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
