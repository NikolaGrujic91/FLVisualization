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
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        public string ShortName { get; set; }

        [Required]
        public int Win { get; set; }

        [Required]
        public int Loss { get; set; }

        [Required]
        public int Draw { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();

        public List<PlayerHistory> PlayerHistory { get; set; } = new List<PlayerHistory>();
    }
}
