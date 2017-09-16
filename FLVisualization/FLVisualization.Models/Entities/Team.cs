using FLVisualization.Models.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLVisualization.Models.Entities
{
    [Table("Team", Schema = "FLVisualization")]
    public class Team : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string Name { get; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        public string ShortName { get; }

        [Required]
        public int Win { get; }

        [Required]
        public int Loss { get; }

        [Required]
        public int Draw { get; }

        [InverseProperty(nameof(Player))]
        public List<Player> Players { get; set; } = new List<Player>();

        [InverseProperty(nameof(PlayerHistory))]
        public List<PlayerHistory> History { get; set; } = new List<PlayerHistory>();
    }
}
