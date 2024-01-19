using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles ="Admin")]
    public class TaskController : Controller
    {
        public readonly TaskService _service;
        public TaskController(TaskService service)
        {
            _service = service;
        }
        [HttpGet("index")]
        public object Index()
        {
            List<Models.Task> tasks = _service.GetAllTasks();
            return (tasks);
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] TaskForm taskform) {
            var task = _service.CreateTask(taskform);
            _service.AddTask(task);
            return(RedirectToAction("Index"));
        }
    }
}
