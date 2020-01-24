using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Overwatch
{
    public class MyUserStats : NayuModule
    {        
        [Subject(Categories.Overwatch)]
        [Command("myowstats")]
        [Summary("Get your Overwatch statistics. NOTE: You must first register your Battle.net Username and ID with n!owaccount")]
        [Alias("myows", "myoverwatchstats")]
        [Remarks("n!myows")]
        [Cooldown(5)]
        public async Task GetOwStats()
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var username = config.OverwatchID;
                var platform = config.OverwatchPlatform;
                var region = config.OverwatchRegion;

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/complete");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string compcards = dataObject.competitiveStats.awards.cards.ToString();
                string compmedal = dataObject.competitiveStats.awards.medals.ToString();
                string compmedalGold = dataObject.competitiveStats.awards.medalsGold.ToString();
                string compmedalSilver = dataObject.competitiveStats.awards.medalsSilver.ToString();
                string compmedalBronze = dataObject.competitiveStats.awards.medalsBronze.ToString();

                string qpcards = dataObject.quickPlayStats.awards.cards.ToString();
                string qpmedal = dataObject.quickPlayStats.awards.medals.ToString();
                string qpmedalGold = dataObject.quickPlayStats.awards.medalsGold.ToString();
                string qpmedalSilver = dataObject.quickPlayStats.awards.medalsSilver.ToString();
                string qpmedalBronze = dataObject.quickPlayStats.awards.medalsBronze.ToString();
                string qpgames = dataObject.quickPlayStats.games.played.ToString();
                string qpwon = dataObject.quickPlayStats.games.won.ToString();

                //comp
                string CompdefensiveAssists = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string ComphealingDone = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string CompoffensiveAssists = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string CompbarrierDamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.barrierDamageDone.ToString();
                string CompdamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.damageDone.ToString();
                string Compdeaths = dataObject.competitiveStats.careerStats.allHeroes.combat.deaths.ToString();
                string Compeliminations = dataObject.competitiveStats.careerStats.allHeroes.combat.eliminations.ToString();
                string CompenvironmentalKills = dataObject.competitiveStats.careerStats.allHeroes.combat.environmentalKills.ToString();
                string CompfinalBlows = dataObject.competitiveStats.careerStats.allHeroes.combat.finalBlows.ToString();
                string CompheroDamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.heroDamageDone.ToString();
                string CompmeleeFinalBlows = dataObject.competitiveStats.careerStats.allHeroes.combat.meleeFinalBlows.ToString();
                string Compmultikills = dataObject.competitiveStats.careerStats.allHeroes.combat.multikills.ToString();
                string CompobjectiveKills = dataObject.competitiveStats.careerStats.allHeroes.combat.objectiveKills.ToString();
                string CompobjectiveTime = dataObject.competitiveStats.careerStats.allHeroes.combat.objectiveTime.ToString();
                string CompsoloKills = dataObject.competitiveStats.careerStats.allHeroes.combat.soloKills.ToString();
                string ComptimeSpentOnFire = dataObject.competitiveStats.careerStats.allHeroes.combat.timeSpentOnFire.ToString();

                string CompallDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.allDamageDoneAvgPer10Min.ToString();
                string CompbarrierDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.barrierDamageDoneAvgPer10Min.ToString();
                string CompdeathsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.deathsAvgPer10Min.ToString();
                string CompeliminationsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.eliminationsAvgPer10Min.ToString();
                string CompfinalBlowsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.finalBlowsAvgPer10Min.ToString();
                string ComphealingDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.healingDoneAvgPer10Min.ToString();
                string CompheroDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.heroDamageDoneAvgPer10Min.ToString();
                string CompobjectiveKillsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.objectiveKillsAvgPer10Min.ToString();
                string CompobjectiveTimeAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.objectiveTimeAvgPer10Min.ToString();
                string CompsoloKillsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.soloKillsAvgPer10Min.ToString();
                string ComptimeSpentOnFireAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.timeSpentOnFireAvgPer10Min.ToString();

                string CompallDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.allDamageDoneMostInGame.ToString();
                string CompbarrierDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.barrierDamageDoneMostInGame.ToString();
                string CompdefensiveAssistsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.defensiveAssistsMostInGame.ToString();
                string CompeliminationsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.eliminationsMostInGame.ToString();
                string CompenvironmentalKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.environmentalKillsMostInGame.ToString();
                string CompfinalBlowsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.finalBlowsMostInGame.ToString();
                string ComphealingDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.healingDoneMostInGame.ToString();
                string CompheroDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.heroDamageDoneMostInGame.ToString();
                string CompkillsStreakBest = dataObject.competitiveStats.careerStats.allHeroes.best.killsStreakBest.ToString();
                string CompmeleeFinalBlowsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.meleeFinalBlowsMostInGame.ToString();
                string CompmultikillsBest = dataObject.competitiveStats.careerStats.allHeroes.best.multikillsBest.ToString();
                string CompobjectiveKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.objectiveKillsMostInGame.ToString();
                string CompobjectiveTimeMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.objectiveTimeMostInGame.ToString();
                string CompoffensiveAssistsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.offensiveAssistsMostInGame.ToString();
                string CompsoloKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.soloKillsMostInGame.ToString();
                string ComptimeSpentOnFireMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.timeSpentOnFireMostInGame.ToString();

                string CompgamesLost = dataObject.competitiveStats.careerStats.allHeroes.game.gamesLost.ToString();
                string CompgamesPlayed = dataObject.competitiveStats.careerStats.allHeroes.game.gamesPlayed.ToString();
                string CompgamesTied = dataObject.competitiveStats.careerStats.allHeroes.game.gamesTied.ToString();
                string CompgamesWon = dataObject.competitiveStats.careerStats.allHeroes.game.gamesWon.ToString();
                string ComptimePlayed = dataObject.competitiveStats.careerStats.allHeroes.game.timePlayed.ToString();

                //qp
                string QpdefensiveAssists = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QphealingDone = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QpoffensiveAssists = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QpbarrierDamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.barrierDamageDone.ToString();
                string QpdamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.damageDone.ToString();
                string Qpdeaths = dataObject.quickPlayStats.careerStats.allHeroes.combat.deaths.ToString();
                string Qpeliminations = dataObject.quickPlayStats.careerStats.allHeroes.combat.eliminations.ToString();
                string QpenvironmentalKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.environmentalKills.ToString();
                string QpfinalBlows = dataObject.quickPlayStats.careerStats.allHeroes.combat.finalBlows.ToString();
                string QpheroDamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.heroDamageDone.ToString();
                string QpmeleeFinalBlows = dataObject.quickPlayStats.careerStats.allHeroes.combat.meleeFinalBlows.ToString();
                string Qpmultikills = dataObject.quickPlayStats.careerStats.allHeroes.combat.multikills.ToString();
                string QpobjectiveKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.objectiveKills.ToString();
                string QpobjectiveTime = dataObject.quickPlayStats.careerStats.allHeroes.combat.objectiveTime.ToString();
                string QpsoloKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.soloKills.ToString();
                string QptimeSpentOnFire = dataObject.quickPlayStats.careerStats.allHeroes.combat.timeSpentOnFire.ToString();

                string QpallDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.allDamageDoneAvgPer10Min.ToString();
                string QpbarrierDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.barrierDamageDoneAvgPer10Min.ToString();
                string QpdeathsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.deathsAvgPer10Min.ToString();
                string QpeliminationsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.eliminationsAvgPer10Min.ToString();
                string QpfinalBlowsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.finalBlowsAvgPer10Min.ToString();
                string QphealingDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.healingDoneAvgPer10Min.ToString();
                string QpheroDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.heroDamageDoneAvgPer10Min.ToString();
                string QpobjectiveKillsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.objectiveKillsAvgPer10Min.ToString();
                string QpobjectiveTimeAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.objectiveTimeAvgPer10Min.ToString();
                string QpsoloKillsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.soloKillsAvgPer10Min.ToString();
                string QptimeSpentOnFireAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.timeSpentOnFireAvgPer10Min.ToString();

                string QpallDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.allDamageDoneMostInGame.ToString();
                string QpbarrierDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.barrierDamageDoneMostInGame.ToString();
                string QpdefensiveAssistsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.defensiveAssistsMostInGame.ToString();
                string QpeliminationsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.eliminationsMostInGame.ToString();
                string QpenvironmentalKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.environmentalKillsMostInGame.ToString();
                string QpfinalBlowsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.finalBlowsMostInGame.ToString();
                string QphealingDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.healingDoneMostInGame.ToString();
                string QpheroDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.heroDamageDoneMostInGame.ToString();
                string QpkillsStreakBest = dataObject.quickPlayStats.careerStats.allHeroes.best.killsStreakBest.ToString();
                string QpmeleeFinalBlowsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.meleeFinalBlowsMostInGame.ToString();
                string QpmultikillsBest = dataObject.quickPlayStats.careerStats.allHeroes.best.multikillsBest.ToString();
                string QpobjectiveKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.objectiveKillsMostInGame.ToString();
                string QpobjectiveTimeMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.objectiveTimeMostInGame.ToString();
                string QpoffensiveAssistsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.offensiveAssistsMostInGame.ToString();
                string QpsoloKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.soloKillsMostInGame.ToString();
                string QptimeSpentOnFireMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.timeSpentOnFireMostInGame.ToString();

                string QpgamesWon = dataObject.quickPlayStats.careerStats.allHeroes.game.gamesWon.ToString();
                string QptimePlayed = dataObject.quickPlayStats.careerStats.allHeroes.game.timePlayed.ToString();

                string Qpcards = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.cards.ToString();
                string Qpmedals = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medals.ToString();
                string QpmedalsBronze = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsBronze.ToString();
                string QpmedalsGold = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsGold.ToString();
                string QpmedalsSilver = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsSilver.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                string sr = dataObject.rating.ToString();
                string srIcon = dataObject.ratingIcon.ToString();

                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OW-API",
                    IconUrl = srIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Overwatch Profile",
                    IconUrl = endorsementIcon
                };

                var embed = new EmbedBuilder()
                {
                    Author = top,
                    Footer = bottom
                };
                embed.WithThumbnailUrl(playerIcon);
                embed.WithColor(Global.NayuColor);
                embed.AddField("Quickplay All Hero Stats Total", $"Defensive Assists: **{QpdefensiveAssists}**\nOffensive Assists: **{QpoffensiveAssists}**\nDamage Done: **{QpdamageDone}**\nBarrier Damage Done: **{QpbarrierDamageDone}**\nHero Damage Done: **{QpheroDamageDone}**\nHealing Done: **{QphealingDone}**\nEliminations: **{Qpeliminations}**\nDeaths: **{Qpdeaths}**\nEnvironmental Kills: **{QpenvironmentalKills}**\nFinal Blows: **{QpfinalBlows}**\nMelee Final Blows: **{QpmeleeFinalBlows}**\nMulti Kills: **{Qpmultikills}**\nObjective Kills: **{QpobjectiveKills}**\nObjective Time: **{QpobjectiveTime}**\nSolo Kills: **{QpsoloKills}**\nTime Spent On Fire: **{QptimeSpentOnFire}**", true);
                embed.AddField("Quickplay All Hero Stats Averages Per 10 Minutes", $"Barrier Damage Done: **{QpbarrierDamageDoneAvgPer10Min}**\nHero Damage Done: **{QpheroDamageDoneAvgPer10Min}**\nHealing Done: **{QphealingDoneAvgPer10Min}**\nEliminations: **{QpeliminationsAvgPer10Min}**\nDeaths: **{QpdeathsAvgPer10Min}**\nFinal Blows: **{QpfinalBlowsAvgPer10Min}**\nObjective Kills: **{QpobjectiveKillsAvgPer10Min}**\nObjective Time: **{QpobjectiveTimeAvgPer10Min}**\nSolo Kills: **{QpsoloKillsAvgPer10Min}**\nTime Spent On Fire: **{QptimeSpentOnFireAvgPer10Min}**", true);
                embed.AddField("Quickplay All Hero Stats Best In Game", $"Defensive Assists: **{QpdefensiveAssistsMostInGame}**\nOffensive Assists: **{QpoffensiveAssistsMostInGame}**\nDamage Done: **{QpallDamageDoneMostInGame}**\nBarrier Damage Done: **{QpbarrierDamageDoneMostInGame}**\nHero Damage Done: **{QpheroDamageDoneMostInGame}**\nHealing Done: **{QphealingDoneMostInGame}**\nEliminations: **{QpeliminationsMostInGame}**\nEnvironmental Kills: **{QpenvironmentalKillsMostInGame}**\nFinal Blows: **{QpfinalBlowsMostInGame}**\nMelee Final Blows: **{QpmeleeFinalBlowsMostInGame}**\nObjective Kills: **{QpobjectiveKillsMostInGame}**\nObjective Time: **{QpobjectiveTimeMostInGame}**\nSolo Kills: **{QpsoloKillsMostInGame}**\nTime Spent On Fire: **{QptimeSpentOnFireMostInGame}**", true);
                embed.AddField("Quickplay Game Stats", $"Games Played: **{qpgames}**\nGames Won: **{QpgamesWon}**\nTime Played: **{QptimePlayed}**", true);
                embed.AddField("Quickplay Awards", $"Total Medals: **{qpmedal}**\n:first_place: Gold Medals: **{qpmedalGold}**\n:second_place: Silver Medals: **{qpmedalSilver}**\n:third_place: Bronze Medals: **{qpmedalBronze}**\nCards: **{Qpcards}**", true);
                embed.AddField("Competitive All Hero Stats Total", $"Defensive Assists: **{CompdefensiveAssists}**\nOffensive Assists: **{CompoffensiveAssists}**\nDamage Done: **{CompdamageDone}**\nBarrier Damage Done: **{CompbarrierDamageDone}**\nHero Damage Done: **{CompheroDamageDone}**\nHealing Done: **{ComphealingDone}**\nEliminations: **{Compeliminations}**\nDeaths: **{Compdeaths}**\nEnvironmental Kills: **{CompenvironmentalKills}**\nFinal Blows: **{CompfinalBlows}**\nMelee Final Blows: **{CompmeleeFinalBlows}**\nMulti Kills: **{Compmultikills}**\nObjective Kills: **{CompobjectiveKills}**\nObjective Time: **{CompobjectiveTime}**\nSolo Kills: **{CompsoloKills}**\nTime Spent On Fire: **{ComptimeSpentOnFire}**", true);
                embed.AddField("Competitive All Hero Stats Averages Per 10 Minutes", $"Barrier Damage Done: **{CompbarrierDamageDoneAvgPer10Min}**\nHero Damage Done: **{CompheroDamageDoneAvgPer10Min}**\nHealing Done: **{ComphealingDoneAvgPer10Min}**\nEliminations: **{CompeliminationsAvgPer10Min}**\nDeaths: **{CompdeathsAvgPer10Min}**\nFinal Blows: **{CompfinalBlowsAvgPer10Min}**\nObjective Kills: **{CompobjectiveKillsAvgPer10Min}**\nObjective Time: **{CompobjectiveTimeAvgPer10Min}**\nSolo Kills: **{CompsoloKillsAvgPer10Min}**\nTime Spent On Fire: **{ComptimeSpentOnFireAvgPer10Min}**", true);
                embed.AddField("Competitive All Hero Stats Best In Game", $"Defensive Assists: **{CompdefensiveAssistsMostInGame}**\nOffensive Assists: **{CompoffensiveAssistsMostInGame}**\nDamage Done: **{CompallDamageDoneMostInGame}**\nBarrier Damage Done: **{CompbarrierDamageDoneMostInGame}**\nHero Damage Done: **{CompheroDamageDoneMostInGame}**\nHealing Done: **{ComphealingDoneMostInGame}**\nEliminations: **{CompeliminationsMostInGame}**\nEnvironmental Kills: **{CompenvironmentalKillsMostInGame}**\nFinal Blows: **{CompfinalBlowsMostInGame}**\nMelee Final Blows: **{CompmeleeFinalBlowsMostInGame}**\nObjective Kills: **{CompobjectiveKillsMostInGame}**\nObjective Time: **{CompobjectiveTimeMostInGame}**\nSolo Kills: **{CompsoloKillsMostInGame}**\nTime Spent On Fire: **{ComptimeSpentOnFireMostInGame}**", true);
                embed.AddField("Competitive Game Stats", $"Games Played: **{CompgamesPlayed}**\nGames Won: **{CompgamesWon}**\nGames Tied: **{CompgamesTied}**\nGames Lost: **{CompgamesLost}**\nTime Played: **{ComptimePlayed}**", true);
                embed.AddField("Competitive Awards", $"Total Medals: **{compmedal}**\n:first_place: Gold Medals: **{compmedalGold}**\n:second_place: Silver Medals: **{compmedalSilver}**\n:third_place: Bronze Medals: **{compmedalBronze}**\nCards: **{compcards}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nSR: **{sr}**\nEndorsement Level: **{endorsement}**", true);

                await SendMessage(Context, embed.Build());
            }
            catch
            {
                await SendMessage(Context, null, "Oops! Are you sure that your Overwatch career profile is set to public and you already set your account information?\n**n!myows**\nNote that you must have completed your placement matches in competetive for this to show up, otherwise use n!owsqp");
            }
        }

        [Command("myowstatsqp")]
        [Summary("Get your Overwatch Quickplay statistics. NOTE: You must first register your Battle.net Username and ID with n!owaccount")]
        [Alias("myowsqp", "myoverwatchstatsqp", "myowsquickplay")]
        [Remarks("n!myowsqp")]
        [Cooldown(5)]
        public async Task GetOwQpStats()
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var username = config.OverwatchID;
                var platform = config.OverwatchPlatform;
                var region = config.OverwatchRegion;

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/complete");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string qpcards = dataObject.quickPlayStats.awards.cards.ToString();
                string qpmedal = dataObject.quickPlayStats.awards.medals.ToString();
                string qpmedalGold = dataObject.quickPlayStats.awards.medalsGold.ToString();
                string qpmedalSilver = dataObject.quickPlayStats.awards.medalsSilver.ToString();
                string qpmedalBronze = dataObject.quickPlayStats.awards.medalsBronze.ToString();
                string qpgames = dataObject.quickPlayStats.games.played.ToString();
                string qpwon = dataObject.quickPlayStats.games.won.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string levelIcon = dataObject.levelIcon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                string QpdefensiveAssists = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QphealingDone = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QpoffensiveAssists = dataObject.quickPlayStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string QpbarrierDamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.barrierDamageDone.ToString();
                string QpdamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.damageDone.ToString();
                string Qpdeaths = dataObject.quickPlayStats.careerStats.allHeroes.combat.deaths.ToString();
                string Qpeliminations = dataObject.quickPlayStats.careerStats.allHeroes.combat.eliminations.ToString();
                string QpenvironmentalKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.environmentalKills.ToString();
                string QpfinalBlows = dataObject.quickPlayStats.careerStats.allHeroes.combat.finalBlows.ToString();
                string QpheroDamageDone = dataObject.quickPlayStats.careerStats.allHeroes.combat.heroDamageDone.ToString();
                string QpmeleeFinalBlows = dataObject.quickPlayStats.careerStats.allHeroes.combat.meleeFinalBlows.ToString();
                string Qpmultikills = dataObject.quickPlayStats.careerStats.allHeroes.combat.multikills.ToString();
                string QpobjectiveKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.objectiveKills.ToString();
                string QpobjectiveTime = dataObject.quickPlayStats.careerStats.allHeroes.combat.objectiveTime.ToString();
                string QpsoloKills = dataObject.quickPlayStats.careerStats.allHeroes.combat.soloKills.ToString();
                string QptimeSpentOnFire = dataObject.quickPlayStats.careerStats.allHeroes.combat.timeSpentOnFire.ToString();

                string QpallDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.allDamageDoneAvgPer10Min.ToString();
                string QpbarrierDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.barrierDamageDoneAvgPer10Min.ToString();
                string QpdeathsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.deathsAvgPer10Min.ToString();
                string QpeliminationsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.eliminationsAvgPer10Min.ToString();
                string QpfinalBlowsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.finalBlowsAvgPer10Min.ToString();
                string QphealingDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.healingDoneAvgPer10Min.ToString();
                string QpheroDamageDoneAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.heroDamageDoneAvgPer10Min.ToString();
                string QpobjectiveKillsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.objectiveKillsAvgPer10Min.ToString();
                string QpobjectiveTimeAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.objectiveTimeAvgPer10Min.ToString();
                string QpsoloKillsAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.soloKillsAvgPer10Min.ToString();
                string QptimeSpentOnFireAvgPer10Min = dataObject.quickPlayStats.careerStats.allHeroes.average.timeSpentOnFireAvgPer10Min.ToString();

                string QpallDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.allDamageDoneMostInGame.ToString();
                string QpbarrierDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.barrierDamageDoneMostInGame.ToString();
                string QpdefensiveAssistsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.defensiveAssistsMostInGame.ToString();
                string QpeliminationsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.eliminationsMostInGame.ToString();
                string QpenvironmentalKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.environmentalKillsMostInGame.ToString();
                string QpfinalBlowsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.finalBlowsMostInGame.ToString();
                string QphealingDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.healingDoneMostInGame.ToString();
                string QpheroDamageDoneMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.heroDamageDoneMostInGame.ToString();
                string QpkillsStreakBest = dataObject.quickPlayStats.careerStats.allHeroes.best.killsStreakBest.ToString();
                string QpmeleeFinalBlowsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.meleeFinalBlowsMostInGame.ToString();
                string QpmultikillsBest = dataObject.quickPlayStats.careerStats.allHeroes.best.multikillsBest.ToString();
                string QpobjectiveKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.objectiveKillsMostInGame.ToString();
                string QpobjectiveTimeMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.objectiveTimeMostInGame.ToString();
                string QpoffensiveAssistsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.offensiveAssistsMostInGame.ToString();
                string QpsoloKillsMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.soloKillsMostInGame.ToString();
                string QptimeSpentOnFireMostInGame = dataObject.quickPlayStats.careerStats.allHeroes.best.timeSpentOnFireMostInGame.ToString();

                string QpgamesWon = dataObject.quickPlayStats.careerStats.allHeroes.game.gamesWon.ToString();
                string QptimePlayed = dataObject.quickPlayStats.careerStats.allHeroes.game.timePlayed.ToString();

                string Qpcards = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.cards.ToString();
                string Qpmedals = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medals.ToString();
                string QpmedalsBronze = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsBronze.ToString();
                string QpmedalsGold = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsGold.ToString();
                string QpmedalsSilver = dataObject.quickPlayStats.careerStats.allHeroes.matchAwards.medalsSilver.ToString();
                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OW-API",
                    IconUrl = levelIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Quickplay Overwatch Profile",
                    IconUrl = endorsementIcon
                };

                var embed = new EmbedBuilder()
                {
                    Author = top,
                    Footer = bottom
                };
                embed.WithThumbnailUrl(playerIcon);
                embed.WithColor(Global.NayuColor);
                embed.AddField("Quickplay All Hero Stats Total", $"Defensive Assists: **{QpdefensiveAssists}**\nOffensive Assists: **{QpoffensiveAssists}**\nDamage Done: **{QpdamageDone}**\nBarrier Damage Done: **{QpbarrierDamageDone}**\nHero Damage Done: **{QpheroDamageDone}**\nHealing Done: **{QphealingDone}**\nEliminations: **{Qpeliminations}**\nDeaths: **{Qpdeaths}**\nEnvironmental Kills: **{QpenvironmentalKills}**\nFinal Blows: **{QpfinalBlows}**\nMelee Final Blows: **{QpmeleeFinalBlows}**\nMulti Kills: **{Qpmultikills}**\nObjective Kills: **{QpobjectiveKills}**\nObjective Time: **{QpobjectiveTime}**\nSolo Kills: **{QpsoloKills}**\nTime Spent On Fire: **{QptimeSpentOnFire}**", true);
                embed.AddField("Quickplay All Hero Stats Averages Per 10 Minutes", $"Barrier Damage Done: **{QpbarrierDamageDoneAvgPer10Min}**\nHero Damage Done: **{QpheroDamageDoneAvgPer10Min}**\nHealing Done: **{QphealingDoneAvgPer10Min}**\nEliminations: **{QpeliminationsAvgPer10Min}**\nDeaths: **{QpdeathsAvgPer10Min}**\nFinal Blows: **{QpfinalBlowsAvgPer10Min}**\nObjective Kills: **{QpobjectiveKillsAvgPer10Min}**\nObjective Time: **{QpobjectiveTimeAvgPer10Min}**\nSolo Kills: **{QpsoloKillsAvgPer10Min}**\nTime Spent On Fire: **{QptimeSpentOnFireAvgPer10Min}**", true);
                embed.AddField("Quickplay All Hero Stats Best In Game", $"Defensive Assists: **{QpdefensiveAssistsMostInGame}**\nOffensive Assists: **{QpoffensiveAssistsMostInGame}**\nDamage Done: **{QpallDamageDoneMostInGame}**\nBarrier Damage Done: **{QpbarrierDamageDoneMostInGame}**\nHero Damage Done: **{QpheroDamageDoneMostInGame}**\nHealing Done: **{QphealingDoneMostInGame}**\nEliminations: **{QpeliminationsMostInGame}**\nEnvironmental Kills: **{QpenvironmentalKillsMostInGame}**\nFinal Blows: **{QpfinalBlowsMostInGame}**\nMelee Final Blows: **{QpmeleeFinalBlowsMostInGame}**\nObjective Kills: **{QpobjectiveKillsMostInGame}**\nObjective Time: **{QpobjectiveTimeMostInGame}**\nSolo Kills: **{QpsoloKillsMostInGame}**\nTime Spent On Fire: **{QptimeSpentOnFireMostInGame}**", true);
                embed.AddField("Quickplay Game Stats", $"Games Played: **{qpgames}**\nGames Won: **{QpgamesWon}**\nTime Played: **{QptimePlayed}**", true);
                embed.AddField("Quickplay Awards", $"Total Medals: **{qpmedal}**\n:first_place: Gold Medals: **{qpmedalGold}**\n:second_place: Silver Medals: **{qpmedalSilver}**\n:third_place: Bronze Medals: **{qpmedalBronze}**\nCards: **{Qpcards}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nEndorsement Level: **{endorsement}**", true);

                await SendMessage(Context, embed.Build());
            }
            catch
            {
                await SendMessage(Context, null, "Oops! Are you sure that your Overwatch career profile is set to public and you already set your account information?\n**n!myowsqp**");
            }
        }

        [Command("myowstatscomp")]
        [Summary("Get your Overwatch Competitive statistics. NOTE: You must first register your Battle.net Username and ID with n!owaccount")]
        [Alias("myowsc", "myoverwatchstatscomp", "myowscompetitive")]
        [Remarks("n!myowsc")]
        [Cooldown(5)]
        public async Task GetOwCompStats()
        {
            try
            {
                var config = GlobalUserAccounts.GetUserAccount(Context.User);

                var username = config.OverwatchID;
                var platform = config.OverwatchPlatform;
                var region = config.OverwatchRegion;

                var json = await Global.SendWebRequest($"https://ow-api.com/v1/stats/{platform}/{region}/{username}/complete");

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string CompdefensiveAssists = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string ComphealingDone = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string CompoffensiveAssists = dataObject.competitiveStats.careerStats.allHeroes.assists.defensiveAssists.ToString();
                string CompbarrierDamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.barrierDamageDone.ToString();
                string CompdamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.damageDone.ToString();
                string Compdeaths = dataObject.competitiveStats.careerStats.allHeroes.combat.deaths.ToString();
                string Compeliminations = dataObject.competitiveStats.careerStats.allHeroes.combat.eliminations.ToString();
                string CompenvironmentalKills = dataObject.competitiveStats.careerStats.allHeroes.combat.environmentalKills.ToString();
                string CompfinalBlows = dataObject.competitiveStats.careerStats.allHeroes.combat.finalBlows.ToString();
                string CompheroDamageDone = dataObject.competitiveStats.careerStats.allHeroes.combat.heroDamageDone.ToString();
                string CompmeleeFinalBlows = dataObject.competitiveStats.careerStats.allHeroes.combat.meleeFinalBlows.ToString();
                string Compmultikills = dataObject.competitiveStats.careerStats.allHeroes.combat.multikills.ToString();
                string CompobjectiveKills = dataObject.competitiveStats.careerStats.allHeroes.combat.objectiveKills.ToString();
                string CompobjectiveTime = dataObject.competitiveStats.careerStats.allHeroes.combat.objectiveTime.ToString();
                string CompsoloKills = dataObject.competitiveStats.careerStats.allHeroes.combat.soloKills.ToString();
                string ComptimeSpentOnFire = dataObject.competitiveStats.careerStats.allHeroes.combat.timeSpentOnFire.ToString();

                string CompallDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.allDamageDoneAvgPer10Min.ToString();
                string CompbarrierDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.barrierDamageDoneAvgPer10Min.ToString();
                string CompdeathsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.deathsAvgPer10Min.ToString();
                string CompeliminationsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.eliminationsAvgPer10Min.ToString();
                string CompfinalBlowsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.finalBlowsAvgPer10Min.ToString();
                string ComphealingDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.healingDoneAvgPer10Min.ToString();
                string CompheroDamageDoneAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.heroDamageDoneAvgPer10Min.ToString();
                string CompobjectiveKillsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.objectiveKillsAvgPer10Min.ToString();
                string CompobjectiveTimeAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.objectiveTimeAvgPer10Min.ToString();
                string CompsoloKillsAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.soloKillsAvgPer10Min.ToString();
                string ComptimeSpentOnFireAvgPer10Min = dataObject.competitiveStats.careerStats.allHeroes.average.timeSpentOnFireAvgPer10Min.ToString();

                string CompallDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.allDamageDoneMostInGame.ToString();
                string CompbarrierDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.barrierDamageDoneMostInGame.ToString();
                string CompdefensiveAssistsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.defensiveAssistsMostInGame.ToString();
                string CompeliminationsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.eliminationsMostInGame.ToString();
                string CompenvironmentalKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.environmentalKillsMostInGame.ToString();
                string CompfinalBlowsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.finalBlowsMostInGame.ToString();
                string ComphealingDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.healingDoneMostInGame.ToString();
                string CompheroDamageDoneMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.heroDamageDoneMostInGame.ToString();
                string CompkillsStreakBest = dataObject.competitiveStats.careerStats.allHeroes.best.killsStreakBest.ToString();
                string CompmeleeFinalBlowsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.meleeFinalBlowsMostInGame.ToString();
                string CompmultikillsBest = dataObject.competitiveStats.careerStats.allHeroes.best.multikillsBest.ToString();
                string CompobjectiveKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.objectiveKillsMostInGame.ToString();
                string CompobjectiveTimeMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.objectiveTimeMostInGame.ToString();
                string CompoffensiveAssistsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.offensiveAssistsMostInGame.ToString();
                string CompsoloKillsMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.soloKillsMostInGame.ToString();
                string ComptimeSpentOnFireMostInGame = dataObject.competitiveStats.careerStats.allHeroes.best.timeSpentOnFireMostInGame.ToString();

                string CompgamesLost = dataObject.competitiveStats.careerStats.allHeroes.game.gamesLost.ToString();
                string CompgamesPlayed = dataObject.competitiveStats.careerStats.allHeroes.game.gamesPlayed.ToString();
                string CompgamesTied = dataObject.competitiveStats.careerStats.allHeroes.game.gamesTied.ToString();
                string CompgamesWon = dataObject.competitiveStats.careerStats.allHeroes.game.gamesWon.ToString();
                string ComptimePlayed = dataObject.competitiveStats.careerStats.allHeroes.game.timePlayed.ToString();

                string compcards = dataObject.competitiveStats.awards.cards.ToString();
                string compmedal = dataObject.competitiveStats.awards.medals.ToString();
                string compmedalGold = dataObject.competitiveStats.awards.medalsGold.ToString();
                string compmedalSilver = dataObject.competitiveStats.awards.medalsSilver.ToString();
                string compmedalBronze = dataObject.competitiveStats.awards.medalsBronze.ToString();
                string compgames = dataObject.competitiveStats.games.played.ToString();
                string compwon = dataObject.competitiveStats.games.won.ToString();

                string endorsement = dataObject.endorsement.ToString();
                string endorsementIcon = dataObject.endorsementIcon.ToString();
                string playerIcon = dataObject.icon.ToString();
                string gamesWon = dataObject.gamesWon.ToString();
                string level = dataObject.level.ToString();
                string prestige = dataObject.prestige.ToString();

                string sr = dataObject.rating.ToString();
                string srIcon = dataObject.ratingIcon.ToString();

                var bottom = new EmbedFooterBuilder()
                {
                    Text = "Powered by the OW-API",
                    IconUrl = srIcon
                };

                var top = new EmbedAuthorBuilder()
                {
                    Name = $"{username}'s Competitive Overwatch Profile",
                    IconUrl = endorsementIcon
                };

                var embed = new EmbedBuilder()
                {
                    Author = top,
                    Footer = bottom
                };
                embed.WithThumbnailUrl(playerIcon);
                embed.WithColor(Global.NayuColor);
                embed.AddField("Competitive All Hero Stats Total", $"Defensive Assists: **{CompdefensiveAssists}**\nOffensive Assists: **{CompoffensiveAssists}**\nDamage Done: **{CompdamageDone}**\nBarrier Damage Done: **{CompbarrierDamageDone}**\nHero Damage Done: **{CompheroDamageDone}**\nHealing Done: **{ComphealingDone}**\nEliminations: **{Compeliminations}**\nDeaths: **{Compdeaths}**\nEnvironmental Kills: **{CompenvironmentalKills}**\nFinal Blows: **{CompfinalBlows}**\nMelee Final Blows: **{CompmeleeFinalBlows}**\nMulti Kills: **{Compmultikills}**\nObjective Kills: **{CompobjectiveKills}**\nObjective Time: **{CompobjectiveTime}**\nSolo Kills: **{CompsoloKills}**\nTime Spent On Fire: **{ComptimeSpentOnFire}**", true);
                embed.AddField("Competitive All Hero Stats Averages Per 10 Minutes", $"Barrier Damage Done: **{CompbarrierDamageDoneAvgPer10Min}**\nHero Damage Done: **{CompheroDamageDoneAvgPer10Min}**\nHealing Done: **{ComphealingDoneAvgPer10Min}**\nEliminations: **{CompeliminationsAvgPer10Min}**\nDeaths: **{CompdeathsAvgPer10Min}**\nFinal Blows: **{CompfinalBlowsAvgPer10Min}**\nObjective Kills: **{CompobjectiveKillsAvgPer10Min}**\nObjective Time: **{CompobjectiveTimeAvgPer10Min}**\nSolo Kills: **{CompsoloKillsAvgPer10Min}**\nTime Spent On Fire: **{ComptimeSpentOnFireAvgPer10Min}**", true);
                embed.AddField("Competitive All Hero Stats Best In Game", $"Defensive Assists: **{CompdefensiveAssistsMostInGame}**\nOffensive Assists: **{CompoffensiveAssistsMostInGame}**\nDamage Done: **{CompallDamageDoneMostInGame}**\nBarrier Damage Done: **{CompbarrierDamageDoneMostInGame}**\nHero Damage Done: **{CompheroDamageDoneMostInGame}**\nHealing Done: **{ComphealingDoneMostInGame}**\nEliminations: **{CompeliminationsMostInGame}**\nEnvironmental Kills: **{CompenvironmentalKillsMostInGame}**\nFinal Blows: **{CompfinalBlowsMostInGame}**\nMelee Final Blows: **{CompmeleeFinalBlowsMostInGame}**\nObjective Kills: **{CompobjectiveKillsMostInGame}**\nObjective Time: **{CompobjectiveTimeMostInGame}**\nSolo Kills: **{CompsoloKillsMostInGame}**\nTime Spent On Fire: **{ComptimeSpentOnFireMostInGame}**", true);
                embed.AddField("Competitive Game Stats", $"Games Played: **{CompgamesPlayed}**\nGames Won: **{CompgamesWon}**\nGames Tied: **{CompgamesTied}**\nGames Lost: **{CompgamesLost}**\nTime Played: **{ComptimePlayed}**", true);
                embed.AddField("Competitive Awards", $"Total Medals: **{compmedal}**\n:first_place: Gold Medals: **{compmedalGold}**\n:second_place: Silver Medals: **{compmedalSilver}**\n:third_place: Bronze Medals: **{compmedalBronze}**\nCards: **{compcards}**", true);
                embed.AddField("Overall", $"Level: **{level}**\nPrestige: **{prestige}**\nSR: **{sr}**\nEndorsement Level: **{endorsement}**", true);

                await SendMessage(Context, embed.Build());
            }
            catch
            {
                await SendMessage(Context, null, "Oops! Are you sure that your Overwatch career profile is set to public and you already set your account information?\n**n!myowscomp**");
            }
        }
    }
}
