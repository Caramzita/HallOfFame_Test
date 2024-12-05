using AutoMapper;
using HallOfFame.Contracts;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;

namespace HallOfFame.API.Profiles;

/// <summary>
/// Профиль AutoMapper для маппинга объектов запросов из контроллера на команды.
/// </summary>
public class ControllerMappingProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ControllerMappingProfile"/> и задает конфигурации маппинга.
    /// </summary>
    public ControllerMappingProfile()
    {
        CreateMap<PersonRequestDto, AddPersonCommand>();

        CreateMap<PersonRequestDto, UpdatePersonCommand>();
    }
}
