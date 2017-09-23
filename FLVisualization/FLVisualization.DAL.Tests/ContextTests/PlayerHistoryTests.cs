using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FLVisualization.DAL.EF;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Tests.ContextTests
{
    public class PlayerHistoryTests : IDisposable
    {
        private readonly FLVisualizationContext db;

        public PlayerHistoryTests()
        {
            db = new FLVisualizationContext();
            CleanDatabase();
            SetRequiredEntries();
        }

        public void Dispose()
        {
            CleanDatabase();
            db.Dispose();
        }

        private void CleanDatabase()
        {
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.PlayerHistory");
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Player");
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Position");
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Team");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.PlayerHistory\", RESEED, 0);");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Player\", RESEED, 0);");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Position\", RESEED, 0);");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Team\", RESEED, 0);");
        }

        private void SetRequiredEntries()
        {
            db.Positions.Add(new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" });
            db.Teams.Add(new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 });
            db.SaveChanges();
            db.Players.Add(new Player { FirstName = "Foo1", LastName = "Bar1", SquadNumber = 11, PositionId = 1, TeamId = 1 });
            db.Players.Add(new Player { FirstName = "Foo2", LastName = "Bar2", SquadNumber = 7, PositionId = 1, TeamId = 1 });
            db.SaveChanges();
        }

        PlayerHistory Instantiate(int playerId)
        {
            return new PlayerHistory()
            {
                KickoffTimeFormatted = "20-09-2017",
                TeamHScore = 0,
                TeamAScore = 0,
                WasHome = 0,
                Value = 0,
                Round = 0,
                TotalPoints = 0,
                Minutes = 0,
                Goals = 0,
                Assists = 0,
                CleanSheet = 0,
                GoalsConceded = 0,
                OwnGoals = 0,
                PenaltiesSaved = 0,
                PenaltiesMissed = 0,
                YellowCards = 0,
                RedCards = 0,
                Saves = 0,
                Bonus = 0,
                Bps = 0,
                Influence = 0,
                Creativity = 0,
                Threat = 0,
                ICTIndex = 0,
                OpenPlayCrosses = 0,
                BigChancesCreated = 0,
                CleareancesBlocksInterceptions = 0,
                Recoveries = 0,
                KeyPasses = 0,
                Tackles = 0,
                WinningGoals = 0,
                AttemptedPasses = 0,
                CompletedPasses = 0,
                PenaltiesConceded = 0,
                BigChancesMissed = 0,
                ErrorsLeadingToGoal = 0,
                ErrorsLeadingToGoalAttempt = 0,
                Tackled = 0,
                Offside = 0,
                TargetMissed = 0,
                Fouls = 0,
                Dribbles = 0,
                PlayerId = playerId,
                OpponentId = 1
            };
        }

        [Fact]
        public void AddPlayerHistory()
        {
            var playerHistory = Instantiate(1);
            db.History.Add(playerHistory);
            Assert.Equal(EntityState.Added, db.Entry(playerHistory).State);
            Assert.True(playerHistory.Id < 0);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(playerHistory).State);
            Assert.Equal(1, playerHistory.Id);
            Assert.Equal(1, db.History.Count());
        }

        [Fact]
        public void GetAllHistoryOrderedByPlayerId()
        {
            var playerHistory = Instantiate(1);
            var playerHistory2 = Instantiate(2);
            db.History.Add(playerHistory);
            db.History.Add(playerHistory2);
            db.SaveChanges();
            var history = db.History.OrderBy(h => h.PlayerId).ToList();
            Assert.Equal(2, db.Players.Count());
            Assert.Equal(1, history[0].PlayerId);
            Assert.Equal(2, history[1].PlayerId);
        }


        [Fact]
        public void UpdatePlayerHistory()
        {
            var playerHistory = Instantiate(1);
            db.History.Add(playerHistory);
            db.SaveChanges();
            playerHistory.TeamHScore = 3;
            db.History.Update(playerHistory);
            Assert.Equal(EntityState.Modified, db.Entry(playerHistory).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(playerHistory).State);
            FLVisualizationContext context;
            using (context = new FLVisualizationContext())
            {
                Assert.Equal(3, context.History.First().TeamHScore);
            }
        }

        [Fact]
        public void DeletePlayerHistory()
        {
            var playerHistory = Instantiate(1);
            db.History.Add(playerHistory);
            db.SaveChanges();
            Assert.Equal(1, db.History.Count());
            db.History.Remove(playerHistory);
            Assert.Equal(EntityState.Deleted, db.Entry(playerHistory).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Detached, db.Entry(playerHistory).State);
            Assert.Equal(0, db.History.Count());
        }
    }
}
