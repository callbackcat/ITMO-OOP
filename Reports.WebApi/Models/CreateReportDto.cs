using AutoMapper;
using Reports.Application.Common.Mappings;
using Reports.Application.Reports.Commands.CreateReport;

namespace Reports.WebApi.Models
{
    public class CreateReportDto : IMapWith<CreateReportCommand>
    {
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateReportDto, CreateReportCommand>()
                .ForMember(report => report.Title,
                    opt => opt.MapFrom(reportDto => reportDto.Title));
        }
    }
}