using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services.Interfaces;
using api.DTO;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Route("api/tasks")]
[ApiController]
[Authorize]
public class TaskController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> FindAll()
    {
        IEnumerable<TaskModel> tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> FindById(Guid id)
    {
        TaskModel? task = await _taskService.GetTaskByIdAsync(id);
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskModel>> Create(TaskDTO dto)
    {
        TaskModel createdTask = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(FindById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskModel>> Update(Guid id, TaskDTO dto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        TaskModel updatedTask = await _taskService.UpdateTaskAsync(id, dto);
        return Ok(updatedTask);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool _ = await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PaginationResponseDTO<TaskResponseDTO>>> GetTasks([FromQuery] PaginationRequestDTO request)
    {
        PaginationResponseDTO<TaskResponseDTO> result = await _taskService.FindPagedTasksAsync(request);
        return Ok(result);
    }
}