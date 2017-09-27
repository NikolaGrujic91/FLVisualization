using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FLVisualization.DAL.EF;
using FLVisualization.DAL.Repos.Base;
using FLVisualization.DAL.Repos.Interfaces;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.Repos
{
    public class PlayerRepo : RepoBase<Player>, IPlayerRepo
    {
        public PlayerRepo(DbContextOptions<FLVisualizationContext> options) : base(options)
        {

        }

        public PlayerRepo()
        {

        }

        public override IEnumerable<Player> GetAll() => table.OrderBy(t => t.Id);

        public override IEnumerable<Player> GetRange(int skip, int take) => GetRange(table.OrderBy(t => t.Id), skip, take);

        public IEnumerable<Player> FindPlayersByTeam(int teamId) => table.Where(p => p.TeamId == teamId).ToList();
    }
}
