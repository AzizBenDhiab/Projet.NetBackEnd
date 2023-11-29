using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;

namespace ProjetNET.Controllers
{
    [Route("api/Profile")]
    [ApiController]
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
        [HttpGet("/Profile/{id}")]
        public ActionResult<object> GetUserAndHistorique(Guid id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            var historique = _db.HistoriquePresences.Where(h => h.UserId == id).ToList();

            if (user == null)
            {
                return NotFound("User not found");
            }

            var profileData = new
            {
                UserData = user,
                HistoriqueData = historique
            };

            return Ok(profileData);
        }


    }
}
