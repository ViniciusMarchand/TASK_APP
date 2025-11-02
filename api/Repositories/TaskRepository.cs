using api.DTO;
using api.Models;
using api.Repositories.Interfaces;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    ApplicationDbContext _context = context;
    
    public async Task<TaskModel> CreateTaskAsync(TaskModel task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTaskAsync(TaskModel task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
        return await _context.Tasks.AsNoTracking().Include(t => t.User).ToListAsync();
    }

    public async Task<TaskModel?> GetTaskByIdAsync(Guid id)
    {
        return await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

     public async Task<PaginationResponseDTO<TaskResponseDTO>> FindPagedAsync(PaginationRequestDTO request)
    {
        IOrderedQueryable<TaskModel> query = _context.Tasks
            .AsNoTracking()
            .Include(t => t.User)
            .OrderByDescending(t => t.CreatedAt);

        if(request.StatusFilter.HasValue)
        {
            query = (IOrderedQueryable<TaskModel>)query
                .Where(t => t.Status == request.StatusFilter.Value);
        }

        int totalCount = await query.CountAsync();

        List<TaskResponseDTO> items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(
                t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    UserId = t.UserId,
                    UserName = t.User.FirstName + " " + t.User.LastName,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                }
            )
            .ToListAsync();

        return new PaginationResponseDTO<TaskResponseDTO>
        {
            Items = items,
            Page = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}