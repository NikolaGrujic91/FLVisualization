using System;
using System.Net.Http;
using FLVisualization.DAL.EF;
using Newtonsoft.Json;
using FLVisualization.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FLVisualization.Populate
{
    internal class DataParser
    {
        private string allDataURL;
        private string playerDataURL;

        public DataParser(string allDataURL, string playerDataURL)
        {
            this.allDataURL = allDataURL;
            this.playerDataURL = playerDataURL;
        }

        internal async void Run()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(this.allDataURL);
                    response.EnsureSuccessStatusCode();

                    using (HttpContent content = response.Content)
                    {
                        FLVisualizationContext context = new FLVisualizationContext();

                        if (!context.Database.EnsureCreated())
                            ClearData(context);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic jsonAll = JsonConvert.DeserializeObject(responseBody);

                        #region Load Teams

                        var teams = jsonAll.teams;
                        foreach (var team in teams)
                        {
                            Console.WriteLine($"{team.id} {team.name } {team.short_name} {team.win} {team.draw} {team.loss}");
                            context.Teams.Add(new Team()
                            {
                                Id = team.id,
                                Name = team.name,
                                ShortName = team.short_name,
                                Win = team.win,
                                Draw = team.draw,
                                Loss = team.loss
                            });
                        }
                        SaveChanges(context, "Team");

                        #endregion

                        #region Load Positions

                        var positions = jsonAll.element_types;
                        foreach (var position in positions)
                        {
                            Console.WriteLine($"{position.id} {position.singular_name} {position.singular_name_short} {position.plural_name} {position.plural_name_short}");
                            context.Positions.Add(new Position()
                            {
                                Id = position.id,
                                SingularName = position.singular_name,
                                SingularNameShort = position.singular_name_short,
                                PluralName = position.plural_name,
                                PluralNameShort = position.plural_name_short
                            });
                        }
                        SaveChanges(context, "Position");

                        #endregion

                        #region Load Players

                        var players = jsonAll.elements;
                        foreach (var player in players)
                        {
                            Console.WriteLine($"{player.id} {player.first_name } {player.second_name} {player.squad_number} {player.team} {player.element_type}");
                            context.Players.Add(new Player()
                            {
                                Id = player.id,
                                FirstName = player.first_name,
                                LastName = player.second_name,
                                SquadNumber = player.squad_number == null ? -1 : player.squad_number,
                                TeamId = player.team,
                                PositionId = player.element_type
                            });
                            SaveChanges(context, "Player");

                            #region Load Player History

                            using (HttpClient clientHistory = new HttpClient())
                            {
                                HttpResponseMessage responseHistory = await client.GetAsync(this.playerDataURL + player.id);
                                responseHistory.EnsureSuccessStatusCode();

                                using (HttpContent contentHistory = responseHistory.Content)
                                {
                                    string responseHistoryBody = await responseHistory.Content.ReadAsStringAsync();
                                    dynamic jsonHistory = JsonConvert.DeserializeObject(responseHistoryBody);

                                    var histories = jsonHistory.history_summary;
                                    foreach (var history in histories)
                                    {
                                        var team_h_score = history.team_h_score;
                                        if (team_h_score == null)
                                            break;

                                        Console.WriteLine($"{history.id} {history.kickoff_time_formatted} {history.team_h_score} {history.team_a_score} {history.was_home} {history.value} {history.round}");

                                        var playerHistory = new PlayerHistory()
                                        {
                                            Id = history.id,
                                            KickoffTimeFormatted = history.kickoff_time_formatted,
                                            TeamHScore = history.team_h_score,
                                            TeamAScore = history.team_a_score,
                                            WasHome = history.was_home,
                                            Value = history.value,
                                            Round = history.round,
                                            TotalPoints = history.total_points,
                                            Minutes = history.minutes,
                                            Goals = history.goals_scored,
                                            Assists = history.assists,
                                            CleanSheet = history.clean_sheets,
                                            GoalsConceded = history.goals_conceded,
                                            OwnGoals = history.own_goals,
                                            PenaltiesSaved = history.penalties_saved,
                                            PenaltiesMissed = history.penalties_missed,
                                            YellowCards = history.yellow_cards,
                                            RedCards = history.red_cards,
                                            Saves = history.saves,
                                            Bonus = history.bonus,
                                            Bps = history.bps,
                                            Influence = history.influence,
                                            Creativity = history.creativity,
                                            Threat = history.threat,
                                            ICTIndex = history.ict_index,
                                            OpenPlayCrosses = history.open_play_crosses,
                                            BigChancesCreated = history.big_chances_created,
                                            CleareancesBlocksInterceptions = history.clearances_blocks_interceptions,
                                            Recoveries = history.recoveries,
                                            KeyPasses = history.key_passes,
                                            Tackles = history.tackles,
                                            WinningGoals = history.winning_goals,
                                            AttemptedPasses = history.attempted_passes,
                                            CompletedPasses = history.completed_passes,
                                            PenaltiesConceded = history.penalties_conceded,
                                            BigChancesMissed = history.big_chances_missed,
                                            ErrorsLeadingToGoal = history.errors_leading_to_goal,
                                            ErrorsLeadingToGoalAttempt = history.errors_leading_to_goal_attempt,
                                            Tackled = history.tackled,
                                            Offside = history.offside,
                                            TargetMissed = history.target_missed,
                                            Fouls = history.fouls,
                                            Dribbles = history.dribbles,
                                            PlayerId = history.element,
                                            OpponentId = history.opponent_team                             
                                        };

                                        playerHistory.Team = context.Teams.Find(playerHistory.OpponentId);
                                        context.History.Add(playerHistory);
                                    }
                                    SaveChanges(context, "PlayerHistory");
                                }
                            }

                            #endregion

                        }

                        #endregion

                    }

                    Console.WriteLine("Database populated successfully!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void ClearData(FLVisualizationContext context)
        {
            ExecuteDeleteSQL(context, "Team");
            ExecuteDeleteSQL(context, "Position");
            ExecuteDeleteSQL(context, "Player");
            ExecuteDeleteSQL(context, "PlayerHistory");
        }

        private static void ExecuteDeleteSQL(FLVisualizationContext context, string tableName)
        {
            context.Database.ExecuteSqlCommand($"Delete from FLVisualization.{tableName}");
        }

        private static void SaveChanges(FLVisualizationContext context, string tableName)
        {
            context.Database.OpenConnection();
            context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT FLVisualization.{tableName} ON;");
            context.SaveChanges();
            context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT FLVisualization.{tableName} OFF;");
        }
    }
}
