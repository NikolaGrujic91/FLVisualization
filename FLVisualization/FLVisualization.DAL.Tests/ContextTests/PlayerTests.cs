using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FLVisualization.DAL.EF;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Tests.ContextTests
{
    public class PlayerTests : IDisposable
    {
        private readonly FLVisualizationContext db;

        public PlayerTests()
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
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Player");
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Position");
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Team");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Player\", RESEED, 0);");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Position\", RESEED, 0);");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Team\", RESEED, 0);");
        }

        private void SetRequiredEntries()
        {
            db.Positions.Add(new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" });
            db.Teams.Add(new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 });
            db.SaveChanges();
        }

        [Fact]
        public void AddPlayer()
        {
            var player = new Player { FirstName = "Foo", LastName = "Bar", SquadNumber = 11, PositionId = 1, TeamId = 1 };
            db.Players.Add(player);
            Assert.Equal(EntityState.Added, db.Entry(player).State);
            Assert.True(player.Id < 0);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(player).State);
            Assert.Equal(1, player.Id);
            Assert.Equal(1, db.Players.Count());
        }

        [Fact]
        public void GetAllPlayersOrderedByName()
        {
            db.Players.Add(new Player { FirstName = "Foo", LastName = "Bar", SquadNumber = 11, PositionId = 1, TeamId = 1 });
            db.Players.Add(new Player { FirstName = "Coo", LastName = "Bar", SquadNumber = 11, PositionId = 1, TeamId = 1 });
            db.SaveChanges();
            var players = db.Players.OrderBy(t => t.FirstName).ToList();
            Assert.Equal(2, db.Players.Count());
            Assert.Equal("Coo", players[0].FirstName);
            Assert.Equal("Foo", players[1].FirstName);
        }

        [Fact]
        public void UpdatePlayer()
        {
            var players = new Player { FirstName = "Foo", LastName = "Bar", SquadNumber = 11, PositionId = 1, TeamId = 1 };
            db.Players.Add(players);
            db.SaveChanges();
            players.FirstName = "Coo";
            db.Players.Update(players);
            Assert.Equal(EntityState.Modified, db.Entry(players).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(players).State);
            FLVisualizationContext context;
            using (context = new FLVisualizationContext())
            {
                Assert.Equal("Coo", context.Players.First().FirstName);
            }
        }

        [Fact]
        public void DeletePlayer()
        {
            var player = new Player { FirstName = "Foo", LastName = "Bar", SquadNumber = 11, PositionId = 1, TeamId = 1 };
            db.Players.Add(player);
            db.SaveChanges();
            Assert.Equal(1, db.Players.Count());
            db.Players.Remove(player);
            Assert.Equal(EntityState.Deleted, db.Entry(player).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Detached, db.Entry(player).State);
            Assert.Equal(0, db.Players.Count());
        }
    }
}
