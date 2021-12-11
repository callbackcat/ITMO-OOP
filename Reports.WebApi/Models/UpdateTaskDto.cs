using System;
using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Application.Tasks.Commands.UpdateTask;

namespace Reports.WebApi.Models
{
    public class UpdateTaskDto : IMapWith<UpdateTaskCommand>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateTaskDto, UpdateTaskCommand>()
                .ForMember(task => task.Id,
                    opt => opt.MapFrom(taskDto => taskDto.Id))
                .ForMember(task => task.Title,
                    opt => opt.MapFrom(taskDto => taskDto.Title))
                .ForMember(task => task.Description,
                    opt => opt.MapFrom(taskDto => taskDto.Description))
                .ForMember(task => task.UserId,
                    opt => opt.MapFrom(taskDto => taskDto.UserId));
        }
    }
}