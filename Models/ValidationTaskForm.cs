using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    [NotMapped]
    public class ValidationTaskForm
    {
        public Guid UserId { get; set; }

        public Guid TaskId { get; set; }
        public string Validation { get; set; }
        public String? Cause { get; set; }

        public ValidationTaskForm(Guid userId, Guid taskId,string validation ,string cause)
        {
            UserId = userId;
            TaskId = taskId;
            Validation = validation;
            Cause = cause;
        }
    }
}
