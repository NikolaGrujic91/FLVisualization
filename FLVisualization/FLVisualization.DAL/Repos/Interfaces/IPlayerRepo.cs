using FLVisualization.DAL.Repos.Base;
using FLVisualization.Models.Entities;
using System.Collections.Generic;

namespace FLVisualization.DAL.Repos.Interfaces
{
    public interface IPlayerRepo : IRepo<Player>
    {
        IEnumerable<Player> FindPlayersByTeam(int teamId);
    }
}
