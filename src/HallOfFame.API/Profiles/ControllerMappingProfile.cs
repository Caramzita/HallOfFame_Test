using AutoMapper;
using HallOfFame.Contracts;
using HallOfFame.UseCases.Persons.Commands.AddPerson;
using HallOfFame.UseCases.Persons.Commands.UpdatePerson;

namespace HallOfFame.API.Profiles;

public class ControllerMappingProfile : Profile
{
    public ControllerMappingProfile()
    {
        CreateMap<PersonRequestDto, AddPersonCommand>();

        CreateMap<PersonRequestDto, UpdatePersonCommand>();
    }
}
