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
                                Loss = team.loss,
                                ImageURL = ClubLogoUrl(team.id)
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

                            string imageUrl = CreatePlayerImageUrl(player.photo);
                            Player newPlayer = new Player()
                            {
                                Id = player.id,
                                FirstName = player.first_name,
                                LastName = player.second_name,
                                SquadNumber = player.squad_number == null ? -1 : player.squad_number,
                                TeamId = player.team,
                                PositionId = player.element_type,
                                ImageURL = imageUrl
                            };

                            newPlayer.Position = context.Positions.Find(newPlayer.PositionId);
                            newPlayer.Team = context.Teams.Find(newPlayer.TeamId);
                            context.Players.Add(newPlayer);
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

                                    var histories = jsonHistory.history;
                                    foreach (var history in histories)
                                    {
                                        var team_h_score = history.team_h_score;
                                        if (team_h_score == null)
                                            break;

                                        Console.WriteLine($"    {history.id} {history.kickoff_time_formatted} {history.team_h_score} {history.team_a_score} {history.was_home} {history.value} {history.round}");

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

        private static string CreatePlayerImageUrl(dynamic imageName)
        {
            string urlPrefix = "https://platform-static-files.s3.amazonaws.com/premierleague/photos/players/250x250/p";
            string urlSufix = ".png";
            string[] list = ((string)imageName).Split('.');

            return $"{urlPrefix}{list[0]}{urlSufix}";
        }

        private static string ClubLogoUrl(dynamic teamId)
        {
            switch(Convert.ToInt32(teamId.Value))
            {
                case 1: //Arsenal
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/5/53/Arsenal_FC.svg/180px-Arsenal_FC.svg.png";
                case 2: //Bournemouth
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/e/e5/AFC_Bournemouth_%282013%29.svg/170px-AFC_Bournemouth_%282013%29.svg.png";
                case 3: //Brighton
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/f/fd/Brighton_%26_Hove_Albion_logo.svg/200px-Brighton_%26_Hove_Albion_logo.svg.png";
                case 4: //Burnley
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/0/02/Burnley_FC_badge.png/200px-Burnley_FC_badge.png";
                case 5: //Chelsea
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/c/cc/Chelsea_FC.svg/200px-Chelsea_FC.svg.png";
                case 6: //Crystal Palace
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/0/0c/Crystal_Palace_FC_logo.svg/170px-Crystal_Palace_FC_logo.svg.png";
                case 7: //Everton
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/7/7c/Everton_FC_logo.svg/220px-Everton_FC_logo.svg.png";
                case 8: //Huddersfield
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/5/5a/Huddersfield_Town_A.F.C._logo.svg/150px-Huddersfield_Town_A.F.C._logo.svg.png";
                case 9: //Leicester
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/2/2d/Leicester_City_crest.svg/220px-Leicester_City_crest.svg.png";
                case 10: //Liverpool
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/0/0c/Liverpool_FC.svg/170px-Liverpool_FC.svg.png";
                case 11: //Man City
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/e/eb/Manchester_City_FC_badge.svg/200px-Manchester_City_FC_badge.svg.png";
                case 12: //Man Utd
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/7/7a/Manchester_United_FC_crest.svg/220px-Manchester_United_FC_crest.svg.png";
                case 13: //Newcastle
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/5/56/Newcastle_United_Logo.svg/200px-Newcastle_United_Logo.svg.png";
                case 14: //Southampton
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/c/c9/FC_Southampton.svg/180px-FC_Southampton.svg.png";
                case 15: //Stoke
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/2/29/Stoke_City_FC.svg/220px-Stoke_City_FC.svg.png";
                case 16: //Swansea
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/f/f9/Swansea_City_AFC_logo.svg/220px-Swansea_City_AFC_logo.svg.png";
                case 17: //Spurs
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/b/b4/Tottenham_Hotspur.svg/125px-Tottenham_Hotspur.svg.png";
                case 18: //Watford
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/e/e2/Watford.svg/185px-Watford.svg.png";
                case 19: //West Brom
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/8/8b/West_Bromwich_Albion.svg/160px-West_Bromwich_Albion.svg.png";
                case 20: //West Ham
                    return "https://upload.wikimedia.org/wikipedia/en/thumb/c/c2/West_Ham_United_FC_logo.svg/185px-West_Ham_United_FC_logo.svg.png";
                default:
                    return string.Empty;
            }
        }
    }
}
