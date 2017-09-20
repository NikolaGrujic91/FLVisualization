using FLVisualization.DAL.Repos.Base;
using FLVisualization.Models.Entities;
using System.Collections.Generic;

namespace FLVisualization.DAL.Repos.Interfaces
{
    public interface IPlayerHistoryRepo : IRepo<PlayerHistory>
    {
        IEnumerable<PlayerHistory> FindPlayerHistory(int playerId);
    }
}
