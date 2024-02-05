using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Bogus.DataSets;

namespace ProjetNET.Controllers
{
    [Route("blames")]
    [ApiController]
    [Authorize]
    public class BlameController : Controller
    {
        AppDbContext _db;
        public BlameController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public ActionResult<object> GetUserWarnings()
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            var userBlames = _db.Blames.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userBlames);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> GetAllWarnings()
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            var userBlames = _db.Blames.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userBlames);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> AddUserBlame([FromBody] Blame b)
        {
            if (b == null)
            {
                return BadRequest();

            }
            b.Id = Guid.NewGuid();

            _db.Blames.Add(b);
            _db.SaveChanges();

            return Ok("blame added successfully");
        }

        [HttpPatch("{blameId}")]
        [Authorize(Roles = "User")]
        public ActionResult<object> AddContest(Guid blameId,[FromBody] string? contestation)
        {
            var blame = _db.Blames.Where(w => w.Id.Equals(blameId)).FirstOrDefault();
            if (blame == null) { return NotFound(); }
            blame.Contention = contestation;
            _db.SaveChanges();

            return Ok("contention added successfully");
        }



        [HttpDelete("{blameId:Guid}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBlame(Guid blameId)
        {
            if (blameId == Guid.Empty)
            {
                return BadRequest();
            }
            var b = _db.Blames.FirstOrDefault(u => u.Id == blameId);
            if (b == null)
            {
                return NotFound();
            }

                _db.Blames.Remove(b);
                _db.SaveChanges();
            
            return NoContent();
        }
    }
}
