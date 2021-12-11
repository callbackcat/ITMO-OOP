using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Reports.Commands.CreateReport;
using Reports.Application.Reports.Commands.UpdateReport;
using Reports.Application.Reports.Queries.GetReportDetails;
using Reports.Application.Reports.Queries.GetReportList;
using Reports.WebApi.Models;

namespace Reports.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : BaseController
    {
        private readonly IMapper _mapper;

        public ReportController(IMapper mapper) => _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<ReportListVm>> GetAllReports()
        {
            var query = new GetReportListQuery
            {
                UserId = UserId
            };

            ReportListVm vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReportDetailsVm>> GetReport(Guid id)
        {
            var query = new GetReportDetailsQuery
            {
                UserId = UserId,
                Id = id
            };

            ReportDetailsVm vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateReport([FromBody] CreateReportDto createReportDto)
        {
            var command = _mapper.Map<CreateReportCommand>(createReportDto);
            command.UserId = UserId;
            var reportId = await Mediator.Send(command);

            return Ok(reportId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateReportDto updateReportDto)
        {
            var command = _mapper.Map<UpdateReportCommand>(updateReportDto);
            command.UserId = UserId;
            await Mediator.Send(command);

            return NoContent();
        }
    }
}