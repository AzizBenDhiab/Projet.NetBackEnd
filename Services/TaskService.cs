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
            List<Models.Task> tasks = _db.Tasks.ToList();
            return (tasks);
        }
        public Models.Task CreateTask(TaskForm taskForm)
        {
            Models.Task task = new Models.Task()
            {
                DeadLine = taskForm.DeadLine,
                Description = taskForm.Description,
                Name = taskForm.Name,
                Status = "En cours",
                Users = new List<Models.User>()
            };
            foreach (var userId in taskForm.Users)
            {
                var User = _db.Users.FirstOrDefault(c => c.Id == userId);
                if(User != null) {
                    task.Users.Add(User);
                } 
            }
            return task;
        }
        public void AddTask(Models.Task task)
        {
            _db.Tasks.Add(task);
            _db.SaveChanges();
        }

    }
}
