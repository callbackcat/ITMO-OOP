using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Tasks.Commands.CreateTask;
using Reports.Application.Tasks.Commands.UpdateTask;
using Reports.Application.Tasks.Queries.GetTaskDetails;
using Reports.Application.Tasks.Queries.GetTaskList;
using Reports.WebApi.Models;

namespace Reports.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : BaseController
    {
        private readonly IMapper _mapper;

        public TaskController(IMapper mapper) => _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<TaskListVm>> GetAllTasks(Guid userId)
        {
            var query = new GetTaskListQuery
            {
                UserId = userId
            };

            TaskListVm vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDetailsVm>> GetTask(Guid userId, Guid taskId)
        {
            var query = new GetTaskDetailsQuery
            {
                UserId = userId,
                Id = taskId
            };

            TaskDetailsVm vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("GetTaskByCreationTime")]
        public async Task<ActionResult<TaskListVm>> GetTaskByCreationTime(Guid userId, DateTime time)
        {
            var query = GetAllTasks(userId).Result.Value
                .Tasks
                .Where(t => t.CreationTime == time);

            return Ok(query);
        }

        [HttpGet("GetEditedTasks")]
        public async Task<ActionResult<TaskListVm>> GetEditedTasks(Guid userId)
        {
            var query = GetAllTasks(userId).Result.Value
                .Tasks
                .Where(t => t.EditTime != null);

            return Ok(query);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var command = _mapper.Map<CreateTaskCommand>(createTaskDto);
            command.UserId = UserId;
            var taskId = await Mediator.Send(command);

            return Ok(taskId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskDto updateTaskDto)
        {
            var command = _mapper.Map<UpdateTaskCommand>(updateTaskDto);
            command.UserId = UserId;
            await Mediator.Send(command);

            return NoContent();
        }
    }
}