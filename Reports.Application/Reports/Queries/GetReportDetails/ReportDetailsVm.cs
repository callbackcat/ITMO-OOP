using System;
using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Domain.ReportEntities;

namespace Reports.Application.Reports.Queries.GetReportDetails
{
    public class ReportDetailsVm : IMapWith<Report>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
       // public Draft Draft { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Report, ReportDetailsVm>()
                .ForMember(reportVm => reportVm.Id,
                    opt => opt.MapFrom(report => report.Id))
                .ForMember(reportVm => reportVm.Title,
                    opt => opt.MapFrom(report => report.Title))
                .ForMember(reportVm => reportVm.CreationTime,
                    opt => opt.MapFrom(report => report.CreationTime));
            //  .ForMember(reportVm => reportVm.Draft,
            //    opt => opt.MapFrom(report => report.Draft));
        }
    }
}