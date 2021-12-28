using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Application.Tasks.Commands.CreateTask;

namespace Reports.WebApi.Models
{
    public class CreateTaskDto : IMapWith<CreateTaskCommand>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateTaskDto, CreateTaskCommand>()
                .ForMember(task => task.Title,
                    opt => opt.MapFrom(taskDto => taskDto.Title))
                .ForMember(task => task.Description,
                    opt => opt.MapFrom(taskDto => taskDto.Description));
        }
    }
}