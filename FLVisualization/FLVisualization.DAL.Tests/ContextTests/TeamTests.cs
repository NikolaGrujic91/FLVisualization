using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FLVisualization.DAL.EF;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Tests.ContextTests
{
    public class TeamTests : IDisposable
    {
        private readonly FLVisualizationContext db;

        public TeamTests()
        {
            db = new FLVisualizationContext();
            CleanDatabase();
        }

        public void Dispose()
        {
            CleanDatabase();
            db.Dispose();
        }

        private void CleanDatabase()
        {
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Team");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Team\", RESEED, 0);");
        }

        [Fact]
        public void AddTeam()
        {
            var team = new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 };
            db.Teams.Add(team);
            Assert.Equal(EntityState.Added, db.Entry(team).State);
            Assert.True(team.Id < 0);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(team).State);
            Assert.Equal(1, team.Id);
            Assert.Equal(1, db.Teams.Count());
        }

        [Fact]
        public void GetAllTeamsOrderedByName()
        {
            db.Teams.Add(new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 });
            db.Teams.Add(new Team { Name = "Bar Team", ShortName = "BAR", Draw = 10, Win = 12, Loss = 13 });
            db.SaveChanges();
            var teams = db.Teams.OrderBy(t => t.Name).ToList();
            Assert.Equal(2, db.Teams.Count());
            Assert.Equal("Bar Team", teams[0].Name);
            Assert.Equal("Foo Team", teams[1].Name);
        }

        [Fact]
        public void UpdateTeam()
        {
            var team = new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 };
            db.Teams.Add(team);
            db.SaveChanges();
            team.Name = "Bar Team";
            db.Teams.Update(team);
            Assert.Equal(EntityState.Modified, db.Entry(team).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(team).State);
            FLVisualizationContext context;
            using (context = new FLVisualizationContext())
            {
                Assert.Equal("Bar Team", context.Teams.First().Name);
            }
        }

        [Fact]
        public void DeleteTeam()
        {
            var team = new Team { Name = "Foo Team", ShortName = "FOO", Draw = 10, Win = 12, Loss = 13 };
            db.Teams.Add(team);
            db.SaveChanges();
            Assert.Equal(1, db.Teams.Count());
            db.Teams.Remove(team);
            Assert.Equal(EntityState.Deleted, db.Entry(team).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Detached, db.Entry(team).State);
            Assert.Equal(0, db.Teams.Count());
        }
    }
}
