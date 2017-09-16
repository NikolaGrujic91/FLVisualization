using FLVisualization.Models.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLVisualization.Models.Entities
{
    [Table("Position", Schema = "FLVisualization")]
    public class Position : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string SingularName { get; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        public string SingularNameShort { get; }

        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string PluralName { get; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        public string PluralNameShort { get; }

        [InverseProperty(nameof(Player))]
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
