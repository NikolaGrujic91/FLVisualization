using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FLVisualization.DAL.EF;
using FLVisualization.DAL.Repos.Base;
using FLVisualization.DAL.Repos.Interfaces;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Repos
{
    public class PositionRepo : RepoBase<Position>, IPositionRepo
    {
        public PositionRepo(DbContextOptions<FLVisualizationContext> options) : base(options)
        {

        }

        public PositionRepo()
        {

        }

        public override IEnumerable<Position> GetAll() => table.OrderBy(t => t.Id);

        public override IEnumerable<Position> GetRange(int skip, int take) => GetRange(table.OrderBy(t => t.Id), skip, take);
    }
}
