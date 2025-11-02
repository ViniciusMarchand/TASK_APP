using api.DTO;
using api.Models;

namespace api.Services.Interfaces;

public interface ITaskService
{
    Task<TaskModel> CreateTaskAsync(TaskDTO task);
    Task<TaskModel?> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<TaskModel>> GetAllTasksAsync();
    Task<TaskModel> UpdateTaskAsync(Guid id, TaskDTO task);
    Task<bool> DeleteTaskAsync(Guid id);
    Task<PaginationResponseDTO<TaskResponseDTO>> FindPagedTasksAsync(PaginationRequestDTO request);
}