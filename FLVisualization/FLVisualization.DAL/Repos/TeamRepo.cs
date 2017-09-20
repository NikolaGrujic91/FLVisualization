using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FLVisualization.DAL.EF;
using FLVisualization.DAL.Repos.Base;
using FLVisualization.DAL.Repos.Interfaces;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Repos
{
    public class TeamRepo : RepoBase<Team>, ITeamRepo
    {
        public TeamRepo(DbContextOptions<FLVisualizationContext> options) : base(options)
        {

        }

        public TeamRepo()
        {

        }

        public override IEnumerable<Team> GetAll() => table.OrderBy(t => t.Id);

        public override IEnumerable<Team> GetRange(int skip, int take) => GetRange(table.OrderBy(t => t.Id), skip, take);
    }
}
