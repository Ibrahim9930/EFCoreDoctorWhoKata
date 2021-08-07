using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoctorWho.Db;
using DoctorWho.Db.Domain;
using DoctorWho.Db.Utilities;
using Newtonsoft.Json;

namespace DoctorWho.ConsoleApp.Printers
{
    public class GeneralInfoPrinter
    {
        private readonly DoctorWhoCoreDbContext _context;
        private readonly Action<string> _printingMethod;

        public GeneralInfoPrinter(DoctorWhoCoreDbContext context, Action<string> printingMethod)
        {
            _context = context;
            _printingMethod = printingMethod;
        }

        public void PrintActorsAirTime()
        {
            GetActorsAirTime(out var enemiesAirtime, out var companionsAirtime);

            if (enemiesAirtime.Any())
            {
                _printingMethod("Top three enemies that appeared in the show");
                foreach (var enemyRanking in enemiesAirtime)
                {
                    _printingMethod(JsonConvert.SerializeObject(enemyRanking, Formatting.Indented));
                }
            }

            if (companionsAirtime.Any())
            {
                _printingMethod("Top three companions that appeared in the show");
                foreach (var companionRanking in companionsAirtime)
                {
                    _printingMethod(JsonConvert.SerializeObject(companionRanking, Formatting.Indented));
                }
            }
        }

        private void GetActorsAirTime(out List<EnemyRanking> enemiesAirtime,
            out List<CompanionRanking> companionsAirtime)
        {
            var resultSets = _context.GetMultipleResultSets(new List<Type>()
            {
                typeof(CompanionRanking),
                typeof(EnemyRanking)
            });

            companionsAirtime = GetListFromResultSet<CompanionRanking>(resultSets, 0);
            enemiesAirtime = GetListFromResultSet<EnemyRanking>(resultSets, 1);
        }

        private List<T> GetListFromResultSet<T>(List<ArrayList> resultSets, int listIndex)
        {
            return resultSets[listIndex].Cast<T>().ToList();
        }
    }
}