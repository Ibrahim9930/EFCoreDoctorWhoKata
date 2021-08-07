using System;
using System.Collections.Generic;
using System.Linq;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using Newtonsoft.Json;

namespace DoctorWho.ConsoleApp.Printers
{
    public class SeasonInfoPrinter
    {
        private readonly DoctorWhoCoreDbContext _context;
        private readonly Action<string> _printingMethod;
        public int SeasonNumber { get; set; }

        public SeasonInfoPrinter(DoctorWhoCoreDbContext context, Action<string> printingMethod, int seasonNumber)
        {
            _context = context;
            _printingMethod = printingMethod;

            SeasonNumber = seasonNumber;
        }

        public void PrintSeasonInfo()
        {
            GetInfoAboutSeason(out var numberOfEpisodesInTheSeason, out var enemiesInSeason,
                out var companionsInSeason, out var episodeDetails);

            _printingMethod($"Details for season {SeasonNumber}:-");

            _printingMethod($"{numberOfEpisodesInTheSeason} Episodes");

            if (enemiesInSeason != "")
                _printingMethod($"Enemies in the season : {enemiesInSeason}");

            if (companionsInSeason != "")
                _printingMethod($"Companions in the season : {companionsInSeason}");


            _printingMethod("Season episode details : -");
            foreach (var episodesDetails in episodeDetails)
            {
                _printingMethod(JsonConvert.SerializeObject(episodesDetails, Formatting.Indented));
            }
        }

        private void GetInfoAboutSeason(out int numberOfEpisodesInSeason,
            out string enemiesInSeason, out string companionsInSeason, out List<EpisodeDetails> episodeDetails)
        {

            var seasonEpisodes = GetEpisodesInASeason();
            numberOfEpisodesInSeason = seasonEpisodes.Count();

            enemiesInSeason = GetEnemiesInASeason();

            companionsInSeason = GetCompanionsInASeason();

            episodeDetails = GetEpisodeDetails();
        }

        private IEnumerable<Episode> GetEpisodesInASeason()
        {
            var seasonEpisodes = _context.Episodes
                .Where(ep => ep.SeriesNumber == SeasonNumber)
                .OrderBy(ep => ep.EpisodeNumber);
            return seasonEpisodes;
        }

        private string GetEnemiesInASeason()
        {
            var episodeEnemiesList = _context.Episodes
                .Where(ep => ep.SeriesNumber == SeasonNumber)
                .OrderBy(ep => ep.EpisodeNumber)
                .Select(ep => _context.GetEnemyNamesForEpisode(ep.EpisodeId));
            var nonEmptyEnemyStrings = episodeEnemiesList.Where(s => s != string.Empty);

            return string.Join(',', nonEmptyEnemyStrings);
        }

        private string GetCompanionsInASeason()
        {
            var episodeCompanionsList = _context.Episodes
                .Where(ep => ep.SeriesNumber == SeasonNumber)
                .OrderBy(ep => ep.EpisodeNumber)
                .Select(ep => _context.GetCompanionNamesForEpisode(ep.EpisodeId));
            var nonEmptyCompanionStrings = episodeCompanionsList.Where(s => s != string.Empty);

            return string.Join(',', nonEmptyCompanionStrings);
        }

        private List<EpisodeDetails> GetEpisodeDetails()
        {
            return _context.EpisodeDetails
                .Where(ed => ed.SeriesNumber == SeasonNumber)
                .OrderBy(ed => ed.EpisodeNumber)
                .ToList();
        }
    }
}