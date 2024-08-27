using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryManagementSolution.Models
{
    public class InternalAuthentication
    {
        public int id { get; set; }

        [Required] public int userid { get; set; }

        [StringLength(255)] public string salt { get; set; }

        [Required] [StringLength(255)] public string passwordhash { get; set; }

        // Navigation property
        [JsonIgnore]
        [ForeignKey("userid")] public AppUser AppUser { get; set; }
    }
}