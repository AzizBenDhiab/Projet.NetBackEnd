using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ProjetNET.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProjetNET.Controllers
{
    [Route("api/historique-presences")]
    [Authorize]
    [ApiController]
    public class HistoriquePresenceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoriquePresenceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/historique-presences/meeting-attendees/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("meeting-attendees/{id}")]
        public ActionResult<IEnumerable<object>> GetMeetingAttendees([FromBody] Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            var isAdmin = HttpContext.User.IsInRole("Admin");

            if (!isAdmin)
            {
                return Forbid();
            }
            // Check if the meeting exists
            var meeting = _context.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound("Meeting not found");
            }

            // Get the users who attended the meeting (presence = true)
            var attendees = _context.HistoriquePresences
            .Where(h => h.MeetingId == id && h.Presence)
            .Include(h => h.User)  
            .Select(h => new
            {
                UserId = h.UserId,
                FirstName = h.User.FirstName,
                LastName = h.User.LastName,
                PhoneNumber = h.User.PhoneNumber
            })
            .ToList();

            return Ok(attendees);
        }


        // GET: api/historique-presences/meeting-confirmation/{id}
        [HttpGet("meeting-confirmation/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<object>> GetMeetingConfirmed([FromBody] Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            var isAdmin = HttpContext.User.IsInRole("Admin");

            if (!isAdmin)
            {
                return Forbid();
            }
            // Check if the meeting exists
            var meeting = _context.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound("Meeting not found");
            }

            // Get the users who attended the meeting (presence = true)
            var confirmed = _context.HistoriquePresences
            .Where(h => h.MeetingId == id && h.Confirmation)
            .Include(h => h.User)  // Eager load the User property
            .Select(h => new
            {
                UserId = h.UserId,
                FirstName = h.User.FirstName,
                LastName = h.User.LastName,
                PhoneNumber = h.User.PhoneNumber
            })
            .ToList();


            return Ok(confirmed);
        }
        
        // PATCH: api/historique-presences/confirm-presence/
        [HttpPatch("confirm-presence")]
        public ActionResult ConfirmPresence([FromBody] Guid meetingId)
        {


            // Get the authenticated user ID
            // Get the user's ID from the claims in the JWT token
            // Get the authenticated user ID
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            string userIdString = userIdClaim.Value;
            Guid userId = new Guid(userIdString);

            // Check if the HistoriquePresence entry exists
            var presenceEntry = _context.HistoriquePresences
                .FirstOrDefault(h => h.UserId == userId && h.MeetingId == meetingId);

            if (presenceEntry == null)
            {
                return NotFound("HistoriquePresence entry not found");
            }

            // Confirm the presence by setting Confirmation to true
            presenceEntry.Confirmation = true;

            // Save changes to the database
            _context.SaveChanges();

            return Ok("Presence confirmed successfully");
           
        }




    }
    public class MeetingObj
    {
        public Guid meetingId { get; set; }
    }

}
