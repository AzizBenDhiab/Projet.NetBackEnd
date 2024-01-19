using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class ValidationTaskController : Controller
    {
        private readonly ValidationTaskService _service;
        public ValidationTaskController(ValidationTaskService service)
        {
            _service= service;
        }
        public void Index() { }
        [HttpPost("create")]
        public IActionResult CreateValidation([FromBody] ValidationTaskForm validationTaskForm) { 
        _service.CreateValidation(validationTaskForm);
            return (RedirectToAction("Index"));
        }
        
    }
}
