using api.DTO;
using api.Models;

namespace api.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<TaskModel> CreateTaskAsync(TaskModel task);
    Task<TaskModel?> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<TaskModel>> GetAllTasksAsync();
    Task<TaskModel> UpdateTaskAsync(TaskModel task);
    Task<bool> DeleteTaskAsync(TaskModel task);
    Task<PaginationResponseDTO<TaskResponseDTO>> FindPagedAsync(PaginationRequestDTO request);
}

