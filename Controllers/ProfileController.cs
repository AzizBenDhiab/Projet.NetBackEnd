using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjetNET.Controllers
{
    [Route("api/Profile")]
    [ApiController]
    [Authorize] 
    public class ProfileController : ControllerBase
    {

        AppDbContext _db;
        public ProfileController(AppDbContext db)
        {
            _db = db;
        }
        //[HttpGet]
        //public ActionResult<IEnumerable<HistoriquePresence>> GetHistoriquePresences(Guid id) 
        //{
        //    var historique = _db.HistoriquePresences.Where(h => h.UserId == id)
        //                                            .ToList();
        //    return Ok(historique);
        //}
        //[HttpGet]
        //public ActionResult<IEnumerable<User>> GetUsers (Guid id)
        //{
        //    var user= _db.Users.FirstOrDefault( u => u.Id == id);

        //    return Ok(user);
        //}
        [HttpGet("/Profile")]
        public ActionResult<object> GetUserAndHistorique()
        {
            // Get the user's ID from the claims in the JWT token
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            // Now you have the user ID, and you can use it to fetch user data and historique
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var historique = _db.HistoriquePresences.Where(h => h.UserId == userId).ToList();

            var profileData = new
            {
                UserData = user,
                HistoriqueData = historique
            };

            return Ok(profileData);
        }

        [HttpGet]
        public ActionResult<object> GetUserWarnings(Guid userId)
        {
            var userBlames = _db.Blames.Where(w => w.UserId.Equals( userId)).ToList();
            return Ok(userBlames) ;
        }

        [HttpPost]
        public ActionResult<object> AddUserBlame(Guid userId, string objet, string name, string contention)
        {
            var newBlame = new Blame
            {
                UserId = userId,
                Object = objet,
                Name=name,
                Contention=contention,
           
            };

            _db.Blames.Add(newBlame);
            _db.SaveChanges();

            return Ok("blame added successfully");
        }

        [HttpDelete]

        public ActionResult<object> DeleteBlame(Guid BlameId)
        {
            var BlameToDelete = _db.Blames.Find(BlameId);

            if (BlameToDelete != null)
            {
                _db.Blames.Remove(BlameToDelete);
                _db.SaveChanges();
            }

            
            return Ok("blame deleted successfully");
        }

        //Medal


        [HttpGet("/Profile/Medals")]
        public ActionResult<object> GetUserMedals(Guid userId)
        {
            var userMedals = _db.Medals.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userMedals);
        }

        [HttpPost("/Profile/Medals")]
        public ActionResult<object> AddUserMedal(Guid userId, string type, string name, string description, DateTime date)
        {
            var newMedal = new Medal
            {
                UserId = userId,
                Type = type,
                Name = name,
                Description = description,
                Date = date

            };

            _db.Medals.Add(newMedal);
            _db.SaveChanges();

            return Ok("medal added successfully");
        }

        [HttpDelete("/Profile/Medals")]

        public ActionResult<object> DeleteMedal (Guid MedalId)
        {
            var MedalToDelete = _db.Medals.Find(MedalId);

            if (MedalToDelete != null)
            {
                _db.Medals.Remove(MedalToDelete);
                _db.SaveChanges();
            }


            return Ok("medal deleted successfully");
        }

    }
}
