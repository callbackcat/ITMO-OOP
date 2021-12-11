using System;
using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Application.Reports.Queries.GetReportList;
using Reports.Domain;
using Reports.Domain.ReportEntities;

namespace Reports.Application.Tasks.Queries.GetTaskList
{
    public class TaskLookupDto : IMapWith<Task>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? EditTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Task, TaskLookupDto>()
                .ForMember(taskDto => taskDto.Id,
                    opt => opt.MapFrom(report => report.Id))
                .ForMember(taskDto => taskDto.Title,
                    opt => opt.MapFrom(report => report.Title))
                .ForMember(taskDto => taskDto.Description,
                    opt => opt.MapFrom(report => report.Description))
                .ForMember(taskDto => taskDto.CreationTime,
                    opt => opt.MapFrom(report => report.CreationTime))
                .ForMember(taskDto => taskDto.EditTime,
                    opt => opt.MapFrom(report => report.EditTime));
        }
    }
}