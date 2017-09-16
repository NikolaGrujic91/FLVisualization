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
        public string FirstName { get; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string LastName { get; }

        [Required]
        public int SquadNumber { get; }

        [Required]
        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }

        [Required]
        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; }

        [InverseProperty(nameof(PlayerHistory))]
        public List<PlayerHistory> History { get; set; } = new List<PlayerHistory>();
    }
}
