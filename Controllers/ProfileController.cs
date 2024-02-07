using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Bogus.DataSets;

namespace ProjetNET.Controllers
{
    [Route("api/Profile")]
    [ApiController]
   // [Authorize] 
    public class ProfileController : ControllerBase
    {

        AppDbContext _db;
        public ProfileController(AppDbContext db)
        {
            _db = db;
        }


        


        [HttpGet("/Profile")]
        public ActionResult<object> GetUserAndHistorique()
        {
            // Get the user's ID from the claims in the JWT token
            var userIdClaim = HttpContext.User.FindFirst("id");
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


                 var histoPresencesData = _db.HistoriquePresences
                .Where(h => h.UserId == userId)
                .Select(h => new 
                {
                    Name = h.Meeting.Name,
                    Description = h.Meeting.Description,
                    Date = h.Meeting.Date,
                    Validation = h.Presence,
                    Cause = h.Cause,
                    EventType = "Meeting"

                })
                .ToList();

            var histoTasksData = _db.ValidationTasks
               .Where(h => h.UserId == userId)
               .Select(h => new {
                   Name = h.Task.Name,
                   Description = h.Task.Description,
                   Date=h.Task.DeadLine,
                   Validation = h.Validation,
                   Cause = h.Cause,
                   EventType = "Task"
               })
               .ToList();


            var combinedData = histoPresencesData
       .Concat(histoTasksData)
       .OrderBy(e => e.Date)
       .ToList();


            

            var profileData = new
            {
                UserData = user,
                histoData= combinedData,
            };

            return Ok(profileData);
        }

<<<<<<< HEAD
        [HttpGet("Blames")]
        public ActionResult<object> GetUserWarnings(Guid userId)
        {
            var userBlames = _db.Blames.Where(w => w.UserId.Equals( userId)).ToList();
            return Ok(userBlames) ;
        }

        [HttpPost("Blames")]
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

        [HttpDelete("Blames")]

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
=======
>>>>>>> blames


        [HttpGet("Medals")]
        public ActionResult<object> GetUserMedals(Guid userId)
        {
            var userMedals = _db.Medals.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userMedals);
        }

        [HttpPost("Medals")]
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

        [HttpDelete("Medals")]

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



//      var combinedData = histoPresencesData
//.Select(e => new
//{
//    Name = e.Name,
//    Description = e.Description,
//    Date = e.Date,
//    Validation = e.Validation,
//    Cause = e.Cause,
//    EventType = e.EventType
//})
//.Concat(histoTasksData
//    .Select(e => new
//    {
//        Name = e.Name,
//        Description = e.Description,
//        Date = e.Date,
//        Validation = e.Validation,
//        Cause = e.Cause,
//        EventType = e.EventType
//    })
//)
//.OrderBy(e => e.Date)
//.ToList();
