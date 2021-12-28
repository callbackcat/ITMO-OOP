using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Application.Reports.Commands.UpdateReport;
using Reports.Domain;
using Reports.Domain.Enums;

namespace Reports.WebApi.Models
{
    public class UpdateReportDto : IMapWith<UpdateReportCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IList<Task> Tasks { get; set; }
        public ReportState State { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateReportDto, UpdateReportCommand>()
                .ForMember(report => report.Id,
                    opt => opt.MapFrom(reportDto => reportDto.Id))
                .ForMember(report => report.Title,
                    opt => opt.MapFrom(reportDto => reportDto.Title))
                .ForMember(report => report.Tasks,
                    opt => opt.MapFrom(reportDto => reportDto.Tasks))
                .ForMember(report => report.State,
                    opt => opt.MapFrom(reportDto => reportDto.State));
        }
    }
}