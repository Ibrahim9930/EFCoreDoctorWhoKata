using System;
using DoctorWho.ConsoleApp.Printers;
using DoctorWho.Db;

namespace DoctorWho.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter the season number : ");
            int seasonNumber;
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();

                    seasonNumber = int.Parse(input);
                    break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter a valid number");
                }
            }

            DoctorWhoCoreDbContext context = new DoctorWhoCoreDbContext();

            SeasonInfoPrinter seasonInfoPrinter = new SeasonInfoPrinter(context, Console.WriteLine, seasonNumber);
            seasonInfoPrinter.PrintSeasonInfo();

            GeneralInfoPrinter generalInfoPrinter = new GeneralInfoPrinter(context, Console.WriteLine);
            generalInfoPrinter.PrintActorsAirTime();
        }

    }
}