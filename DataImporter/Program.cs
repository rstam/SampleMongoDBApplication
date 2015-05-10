using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using MongoDB.Driver;

namespace DataImporter
{
    public static class Program
    {
        private static IMongoClient __client;
        private static IMongoDatabase __database;
        private static IMongoCollection<OnTimePerformance> __collection;

        public static void Main(string[] args)
        {
            try
            {
                MainAsync(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }

            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        private static async Task MainAsync(string[] args)
        {
            var connectionString = "mongodb://localhost";
            __client = new MongoClient(connectionString);
            __database = __client.GetDatabase("Flights");
            __collection = __database.GetCollection<OnTimePerformance>("OnTimePerformance");

            await LoadOnTimePerformanceDataAsync();
        }

        private static async Task LoadOnTimePerformanceDataAsync()
        {
            Console.WriteLine("Loading OnTimePerformance files");

            await __database.DropCollectionAsync(__collection.CollectionNamespace.CollectionName);

            var dataDirectory = FindDataDirectory();
            var onTimePerformanceDataDirectory = Path.Combine(dataDirectory, "OnTimePerformance");

            foreach (var csvFileName in Directory.GetFiles(onTimePerformanceDataDirectory, "*.csv"))
            {
                await LoadOnTimePerformanceDataAsync(csvFileName);
            }
        }

        private static string FindDataDirectory()
        {
            var directory = Directory.GetCurrentDirectory();

            var leaf = Path.GetFileName(directory);
            if (leaf == "Debug" || leaf == "Release")
            {
                directory = Path.GetDirectoryName(directory);
                directory = Path.GetDirectoryName(directory);
            }

            return Path.Combine(directory, "Data");
        }

        private static async Task LoadOnTimePerformanceDataAsync(string csvFilename)
        {
            Console.WriteLine("Loading {0}", csvFilename);

            var batchSize = 1000;
            using (var csvReader = new CsvReader(new StreamReader(csvFilename)))
            {
                var batch = new List<OnTimePerformance>();

                foreach (var document  in csvReader.GetRecords<OnTimePerformance>())
                {
                    if (document.FL_DATE.HasValue)
                    {
                        document.FL_DATE = DateTime.SpecifyKind(document.FL_DATE.Value, DateTimeKind.Utc);
                    }

                    batch.Add(document);
                    if (batch.Count == batchSize)
                    {
                        await __collection.InsertManyAsync(batch);
                        batch.Clear();
                        Console.Write(".");
                    }
                }

                if (batch.Count > 0)
                {
                    await __collection.InsertManyAsync(batch);
                }
            }

            Console.WriteLine();
        }
    }
}
