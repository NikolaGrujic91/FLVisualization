using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FLVisualization.DAL.EF;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Tests.ContextTests
{
    public class PositionTests : IDisposable
    {
        private readonly FLVisualizationContext db;

        public PositionTests()
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
            db.Database.ExecuteSqlCommand("Delete from FLVisualization.Position");
            db.Database.ExecuteSqlCommand($"DBCC CHECKIDENT (\"FLVisualization.Position\", RESEED, 0);");
        }

        [Fact]
        public void AddPosition()
        {
            var position = new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" };
            db.Positions.Add(position);
            Assert.Equal(EntityState.Added, db.Entry(position).State);
            Assert.True(position.Id < 0);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(position).State);
            Assert.Equal(1, position.Id);
            Assert.Equal(1, db.Positions.Count());
        }

        [Fact]
        public void GetAllPositionsOrderedByName()
        {
            db.Positions.Add(new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" });
            db.Positions.Add(new Position { SingularName = "DEF", SingularNameShort = "DEF", PluralName = "DEF", PluralNameShort = "DEF" });
            db.SaveChanges();
            var positions = db.Positions.OrderBy(t => t.SingularName).ToList();
            Assert.Equal(2, db.Positions.Count());
            Assert.Equal("DEF", positions[0].SingularName);
            Assert.Equal("FWD", positions[1].SingularName);
        }

        [Fact]
        public void UpdatePosition()
        {
            var position = new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" };
            db.Positions.Add(position);
            db.SaveChanges();
            position.SingularName = "DEF";
            db.Positions.Update(position);
            Assert.Equal(EntityState.Modified, db.Entry(position).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, db.Entry(position).State);
            FLVisualizationContext context;
            using (context = new FLVisualizationContext())
            {
                Assert.Equal("DEF", context.Positions.First().SingularName);
            }
        }

        [Fact]
        public void DeletePosition()
        {
            var position = new Position { SingularName = "FWD", SingularNameShort = "FWD", PluralName = "FWD", PluralNameShort = "FWD" };
            db.Positions.Add(position);
            db.SaveChanges();
            Assert.Equal(1, db.Positions.Count());
            db.Positions.Remove(position);
            Assert.Equal(EntityState.Deleted, db.Entry(position).State);
            db.SaveChanges();
            Assert.Equal(EntityState.Detached, db.Entry(position).State);
            Assert.Equal(0, db.Positions.Count());
        }
    }
}
