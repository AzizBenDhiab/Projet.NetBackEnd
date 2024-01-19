using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.Services
{
    public class ValidationTaskService
    {
        public readonly AppDbContext _db;
        public ValidationTaskService(AppDbContext db)
        {
            _db = db;
        }
        public void CreateValidation(ValidationTaskForm validationTaskForm) {
        var  valid= validationTaskForm.Validation== "valide";
            var validation = new ValidationTask()
            {
                UserId = validationTaskForm.UserId,
                TaskId = validationTaskForm.TaskId,
                Validation = valid,
                Cause = validationTaskForm.Cause,
            };

        _db.ValidationTasks.Add(validation);
        _db.SaveChanges();

        }
    }
}
