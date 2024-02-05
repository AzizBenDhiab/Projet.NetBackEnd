using ProjetNET.Controllers;
using ProjetNET.Models;
  
namespace ProjetNET.Services
{
    public class TaskService
    {
        public readonly AppDbContext _db;
        public TaskService(AppDbContext db)
        {
            _db = db;
        }
        public List<Models.Task> GetAllTasks()
        {
            List<Models.Task> tasks = _db.Tasks.OrderByDescending(obj => obj.DeadLine).ToList();
            return (tasks);
        }
        public List<Models.Task> GetCurrentTasks()
        {
            List<Models.Task> tasks = _db.Tasks.Where(obj => obj.Status == "En cours").OrderByDescending(obj => obj.DeadLine).ToList();
            return (tasks);
        }
        public List<Models.Task> GetCurrentTasksByUserId(Guid userId)
        {
            var taskvalidations = _db.ValidationTasks.Where(c => c.UserId == userId);
            List<Models.Task> tasks = new List<Models.Task>();
            foreach (var taskvalidation in taskvalidations)
            {
                var task = _db.Tasks.FirstOrDefault(c => c.Id == taskvalidation.TaskId);
                if (task!=null && task.Status=="En cours")
                {
                    tasks.Add(task);
                }
            }
            return tasks;
        }
        public void CreateTask(TaskForm taskForm)
        {
            Models.Task task = new Models.Task()
            {
                DeadLine = taskForm.DeadLine,
                Description = taskForm.Description,
                Name = taskForm.Name,
                Status = "En cours"  
            };
            _db.Tasks.Add(task);
            _db.SaveChanges();
            foreach (var userId in taskForm.Users)
            {
                 var user = _db.Users.FirstOrDefault(c=>c.Id==userId);
                if (user!=null)
                {
                    var taskValidation = new Models.ValidationTask()
                    {
                        TaskId = task.Id,
                        UserId = userId,
                        Validation = false,
                    };
                    _db.ValidationTasks.Add(taskValidation);
                }
                
                    
                   
                
            }
            _db.SaveChanges();

        }
        

    }
}
