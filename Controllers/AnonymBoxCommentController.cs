using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class AnonymBoxCommentController : Controller
    { public readonly AnonymBoxCommentService _service;
        public AnonymBoxCommentController(AnonymBoxCommentService service)
        {
            _service= service;
        }

        [HttpGet("index")]
        public object Index()
        {   List<AnonymBoxComment> comments=_service.display_comments();
            return comments;
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] AnonymBoxCommentForm? formData)
        {   
            string? contenu = formData.Contenu;
            
            _service.create_comment(contenu);
            return (RedirectToAction("Index"));
        }

    }
}
