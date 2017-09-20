using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FLVisualization.DAL.EF;
using FLVisualization.DAL.Repos.Base;
using FLVisualization.DAL.Repos.Interfaces;
using FLVisualization.Models.Entities;
using System;

namespace FLVisualization.DAL.Repos
{
    public class PlayerHistoryRepo : RepoBase<PlayerHistory>, IPlayerHistoryRepo
    {
        public PlayerHistoryRepo(DbContextOptions<FLVisualizationContext> options) : base(options)
        {

        }

        public PlayerHistoryRepo()
        {

        }

        public override IEnumerable<PlayerHistory> GetAll() => table.OrderBy(t => t.Id);

        public override IEnumerable<PlayerHistory> GetRange(int skip, int take) => GetRange(table.OrderBy(t => t.Id), skip, take);

        public IEnumerable<PlayerHistory> FindPlayerHistory(int playerId) => table.Where(p => p.PlayerId == playerId).ToList();
    }
}
