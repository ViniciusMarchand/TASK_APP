using System.Security.Claims;
using api.DTO;
using api.Exceptions;
using api.Models;
using api.Repositories.Interfaces;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TaskService(ITaskRepository taskRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : ITaskService
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /*
    * EM PROJETOS MAIORES EU USARIA O AUTOMAPPER PARA FAZER O MAPEAMENTO ENTRE DTO E MODEL
    */

    public async Task<TaskModel> CreateTaskAsync(TaskDTO task)
    {

        string? userId = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId)
            ?? throw new EntityNotFoundException($"User not found."); 

        TaskModel newTask = new()
        {
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            UserId = user.Id,
            User = user
        };

        return await _taskRepository.CreateTaskAsync(newTask);
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        TaskModel? existingTask = await _taskRepository.GetTaskByIdAsync(id)
            ?? throw new EntityNotFoundException($"Task with id {id} not found.");

        string role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        string userIdString = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        Guid userId = Guid.Parse(userIdString);

            
        if(!existingTask.CanDeleteTask(userId, role))
            throw new ForbiddenException("You do not have permission to delete this task.");
        

        return await _taskRepository.DeleteTaskAsync(existingTask);
    }

    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllTasksAsync();
    }

    public async Task<TaskModel?> GetTaskByIdAsync(Guid id)
    {
        TaskModel? existingTask = await _taskRepository.GetTaskByIdAsync(id) 
            ?? throw new EntityNotFoundException($"Task with id {id} not found.");

        return existingTask;
    }

    public async Task<TaskModel> UpdateTaskAsync(Guid id, TaskDTO task)
    {
        TaskModel? existingTask = await _taskRepository.GetTaskByIdAsync(id)
            ?? throw new EntityNotFoundException($"Task with id {id} not found.");

        string role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        string userIdString = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        Guid userId = Guid.Parse(userIdString);

        if (!existingTask.CanEditTask(userId, role))
            throw new ForbiddenException("You do not have permission to edit this task.");

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Status = task.Status;
        existingTask.UpdatedAt = DateTime.UtcNow;

        return await _taskRepository.UpdateTaskAsync(existingTask);
    }
    
    public async Task<PaginationResponseDTO<TaskResponseDTO>> FindPagedTasksAsync(PaginationRequestDTO request)
    {
        return await _taskRepository.FindPagedAsync(request);
    }
}