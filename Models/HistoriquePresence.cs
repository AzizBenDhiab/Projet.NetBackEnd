
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Models
{
    public class HistoriquePresence
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Meeting))]
        public Guid MeetingId { get; set; }
        public bool Presence {  get; set; } 
        public String? Cause { get; set; }
    }
}
