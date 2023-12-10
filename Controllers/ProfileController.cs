using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

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



    }
}
