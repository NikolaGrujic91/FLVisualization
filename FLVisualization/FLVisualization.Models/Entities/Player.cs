using FLVisualization.Models.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLVisualization.Models.Entities
{
    [Table("Player", Schema = "FLVisualization")]
    public class Player : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public int SquadNumber { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }

        public List<PlayerHistory> PlayerHistory { get; set; } = new List<PlayerHistory>();
    }
}
