using System;
using System.Threading.Tasks;
using AutoMapper;
using Reports.Application.Common.Mappings;
using Task = Reports.Domain.Task;

namespace Reports.Application.Tasks.Queries.GetTaskDetails
{
    public class TaskDetailsVm : IMapWith<Task>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? EditTime { get; set; }
        public TaskStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Task, TaskDetailsVm>()
                .ForMember(taskVm => taskVm.Id,
                    opt => opt.MapFrom(task => task.Id))
                .ForMember(taskVm => taskVm.Title,
                    opt => opt.MapFrom(task => task.Title))
                .ForMember(taskVm => taskVm.Description,
                    opt => opt.MapFrom(task => task.Description))
                .ForMember(taskVm => taskVm.CreationTime,
                    opt => opt.MapFrom(task => task.CreationTime))
                .ForMember(taskVm => taskVm.EditTime,
                    opt => opt.MapFrom(task => task.EditTime))
                .ForMember(taskVm => taskVm.Status,
                    opt => opt.MapFrom(task => task.Status));
        }
    }
}