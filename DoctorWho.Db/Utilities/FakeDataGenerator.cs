using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DoctorWho.Db.Domain;
using System.Text.Json;
using Bogus;

namespace DoctorWho.Db.Utilities
{
    public static class FakeDataGenerator
    {
        private const string FAKES_PATH =
            "C:\\Users\\IbrhaimMasri\\Documents\\Foothill Internship\\EF\\DoctorWho\\fakes";

        public static string GetFilePathForTypeFakes<T>() where T : class
        {
            return $"{FAKES_PATH}/{typeof(T).Name}.json";
        }

        public static string GetFilePathForJoinTypeFakes<TEntityType1, TEntityType2>()
        {
            return $"{FAKES_PATH}/{typeof(TEntityType1).Name}_{typeof(TEntityType2).Name}.json";
        }

        public static void GenerateData(int itemCount)
        {
            GenerateFakeEntities(itemCount);
            GenerateFakeJoinEntities(itemCount);
        }

        private static void GenerateFakeEntities(int itemCount)
        {
            GenerateFakeEntity<Author>(itemCount);
            GenerateFakeEntity<Doctor>(itemCount);
            GenerateFakeEntity<Enemy>(itemCount);
            GenerateFakeEntity<Companion>(itemCount);
            GenerateFakeEntity<Episode>(itemCount);
        }

        private static void GenerateFakeEntity<T>(int itemCount) where T : class
        {
            List<T> entities = FakerGenerator.GenerateFakeList<T>(itemCount);

            var output = JsonSerializer.Serialize(entities);

            WriteToFile(GetFilePathForTypeFakes<T>(), output);
        }

        private static void GenerateFakeJoinEntities(int itemCount)
        {
            ArrayList episodeEnemyRelations = new ArrayList();
            ArrayList episodeCompanionRelations = new ArrayList();

            Faker faker = new Faker();
            for (int i = 0; i < itemCount; i++)
            {
                episodeCompanionRelations.Add(new
                {
                    EpisodesEpisodeId = faker.PickRandom(Enumerable.Range(1, itemCount)),
                    CompanionsCompanionId = faker.PickRandom(Enumerable.Range(1, itemCount)),
                    EpisodeCompanionId = i + 1
                });
            }

            for (int i = 0; i < itemCount; i++)
            {
                episodeEnemyRelations.Add(new
                {
                    EpisodesEpisodeId = faker.PickRandom(Enumerable.Range(1, itemCount)),
                    EnemiesEnemyId = faker.PickRandom(Enumerable.Range(1, itemCount)),
                    EpisodeEnemyId = i + 1
                });
            }

            WriteToFile(GetFilePathForJoinTypeFakes<Enemy, Episode>(), JsonSerializer.Serialize(episodeEnemyRelations));
            WriteToFile(GetFilePathForJoinTypeFakes<Companion, Episode>(),
                JsonSerializer.Serialize(episodeCompanionRelations));
        }

        private static void WriteToFile(string filePath, string output)
        {
            var fileStream = File.Create(filePath);
            StreamWriter outputStream = new StreamWriter(fileStream);

            outputStream.Write(output);

            outputStream.Close();
        }
    }
}