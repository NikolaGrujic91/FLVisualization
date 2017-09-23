using FLVisualization.Models.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLVisualization.Models.Entities
{
    [Table("PlayerHistory", Schema = "FLVisualization")]
    public class PlayerHistory : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(50)]
        public string KickoffTimeFormatted { get; set; }

        [Required]
        public int TeamHScore { get; set; }

        [Required]
        public int TeamAScore { get; set; }

        [Required]
        public int WasHome { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public int Round { get; set; }

        [Required]
        public int TotalPoints { get; set; }

        [Required]
        public int Minutes { get; set; }

        [Required]
        public int Goals { get; set; }

        [Required]
        public int Assists { get; set; }

        [Required]
        public int CleanSheet { get; set; }

        [Required]
        public int GoalsConceded { get; set; }

        [Required]
        public int OwnGoals { get; set; }

        [Required]
        public int PenaltiesSaved { get; set; }

        [Required]
        public int PenaltiesMissed { get; set; }

        [Required]
        public int YellowCards { get; set; }

        [Required]
        public int RedCards { get; set; }

        [Required]
        public int Saves { get; set; }

        [Required]
        public int Bonus { get; set; }

        [Required]
        public int Bps { get; set; }

        [Required]
        public double Influence { get; set; }

        [Required]
        public double Creativity { get; set; }

        [Required]
        public double Threat { get; set; }

        [Required]
        public double ICTIndex { get; set; }

        [Required]
        public int OpenPlayCrosses { get; set; }

        [Required]
        public int BigChancesCreated { get; set; }

        [Required]
        public int CleareancesBlocksInterceptions { get; set; }

        [Required]
        public int Recoveries { get; set; }

        [Required]
        public int KeyPasses { get; set; }

        [Required]
        public int Tackles { get; set; }

        [Required]
        public int WinningGoals { get; set; }

        [Required]
        public int AttemptedPasses { get; set; }

        [Required]
        public int CompletedPasses { get; set; }

        [Required]
        public int PenaltiesConceded { get; set; }

        [Required]
        public int BigChancesMissed { get; set; }

        [Required]
        public int ErrorsLeadingToGoal { get; set; }

        [Required]
        public int ErrorsLeadingToGoalAttempt { get; set; }

        [Required]
        public int Tackled { get; set; }

        [Required]
        public int Offside { get; set; }

        [Required]
        public int TargetMissed { get; set; }

        [Required]
        public int Fouls { get; set; }

        [Required]
        public int Dribbles { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int OpponentId { get; set; }
        public Team Team { get; set; }
    }
}