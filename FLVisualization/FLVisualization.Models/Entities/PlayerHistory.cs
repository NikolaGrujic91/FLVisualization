using FLVisualization.Models.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLVisualization.Models.Entities
{
    [Table("PlayerHistory", Schema = "FLVisualization")]
    public class PlayerHistory : EntityBase
    {
        [Required]
        [DataType(DataType.Text), MaxLength(20)]
        public string KickoffTimeFormatted { get; }

        [Required]
        public int TeamHScore { get; }

        [Required]
        public int TeamAScore { get; }

        [Required]
        public int WasHome { get; }

        [Required]
        public int Value { get; }

        [Required]
        public int Round { get; }

        [Required]
        public int TotalPoints { get; }

        [Required]
        public int Minutes { get; }

        [Required]
        public int Goals { get; }

        [Required]
        public int Assists { get; }

        [Required]
        public int CleanSheet { get; }

        [Required]
        public int GoalsConceded { get; }

        [Required]
        public int OwnGoals { get; }

        [Required]
        public int PenaltiesSaved { get; }

        [Required]
        public int PenaltiesMissed { get; }

        [Required]
        public int YellowCards { get; }

        [Required]
        public int RedCards { get; }

        [Required]
        public int Saves { get; }

        [Required]
        public int Bonus { get; }

        [Required]
        public int Bps { get; }

        [Required]
        public double Influence { get; }

        [Required]
        public double Creativity { get; }

        [Required]
        public double Threat { get; }

        [Required]
        public double ICTIndex { get; }

        [Required]
        public int OpenPlayCrosses { get; }

        [Required]
        public int BigChancesCreated { get; }

        [Required]
        public int CleareancesBlocksInterceptions { get; }

        [Required]
        public int Recoveries { get; }

        [Required]
        public int KeyPasses { get; }

        [Required]
        public int Tackles { get; }

        [Required]
        public int WinningGoals { get; }

        [Required]
        public int AttemptedPasses { get; }

        [Required]
        public int CompletedPasses { get; }

        [Required]
        public int PenaltiesConceded { get; }

        [Required]
        public int BigChancesMissed { get; }

        [Required]
        public int ErrorsLeadingToGoal { get; }

        [Required]
        public int ErrorsLeadingToGoalAttempt { get; }

        [Required]
        public int Tackled { get; }

        [Required]
        public int Offside { get; }

        [Required]
        public int TargetMissed { get; }

        [Required]
        public int Fouls { get; }

        [Required]
        public int Dribbles { get; }

        [Required]
        public int PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        [Required]
        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team OpponentTeam { get; set; }
    }
}
