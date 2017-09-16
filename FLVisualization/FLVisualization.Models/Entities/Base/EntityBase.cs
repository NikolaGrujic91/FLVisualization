using System.ComponentModel.DataAnnotations;

namespace FLVisualization.Models.Entities.Base
{
    public abstract class EntityBase
    {
        [Required]
        [Key]
        public int Id { get; }
    }
}
