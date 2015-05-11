using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using MongoDB.Driver;

namespace DataImporter
{
    public static class Program
    {
        private static IMongoClient __client;
        private static IMongoDatabase __database;
        private static HashSet<int> __seenAirlineIds = new HashSet<int>();
        private static HashSet<int> __seenAirportIds = new HashSet<int>();

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
        }

        private static async Task MainAsync(string[] args)
        {
            var connectionString = "mongodb://localhost";
            __client = new MongoClient(connectionString);
            __database = __client.GetDatabase("flights");

            await DropCollectionsAsync();
            await LoadCollectionsAsync();
        }

        private static async Task DropCollectionsAsync()
        {
            await __database.DropCollectionAsync("flights");
            await __database.DropCollectionAsync("airlines");
            await __database.DropCollectionAsync("airports");
        }

        private static async Task LoadCollectionsAsync()
        {
            Console.WriteLine("Loading data");
            await LoadFlightsAsync();
            await LoadAirlinesAsync();
            await LoadAirportsAsync();
        }

        private static async Task LoadFlightsAsync()
        {
            var dataDirectory = FindDataDirectory();
            var flightsDirectory = Path.Combine(dataDirectory, "Flights");
            foreach (var csvFileName in Directory.GetFiles(flightsDirectory, "*.csv"))
            {
                await LoadFlightsAsync(csvFileName);
            }
        }

        private static async Task LoadFlightsAsync(string csvFilename)
        {
            Console.WriteLine("Loading {0}", Path.GetFileName(csvFilename));

            var collection = __database.GetCollection<Flight>("flights");

            var batchSize = 1000;
            using (var csvReader = new CsvReader(new StreamReader(csvFilename)))
            {
                var flights = new List<Flight>();

                foreach (var flight  in csvReader.GetRecords<Flight>())
                {
                    if (flight.FL_DATE.HasValue)
                    {
                        flight.FL_DATE = DateTime.SpecifyKind(flight.FL_DATE.Value, DateTimeKind.Utc);
                    }

                    if (flight.AIRLINE_ID.HasValue) { __seenAirlineIds.Add(flight.AIRLINE_ID.Value); }
                    if (flight.ORIGIN_AIRPORT_ID.HasValue) { __seenAirportIds.Add(flight.ORIGIN_AIRPORT_ID.Value); }
                    if (flight.DEST_AIRPORT_ID.HasValue) { __seenAirportIds.Add(flight.DEST_AIRPORT_ID.Value); }

                    flights.Add(flight);
                    if (flights.Count == batchSize)
                    {
                        await collection.InsertManyAsync(flights);
                        flights.Clear();
                        Console.Write(".");
                    }
                }

                if (flights.Count > 0)
                {
                    await collection.InsertManyAsync(flights);
                }
            }

            Console.WriteLine();
        }

        private static async Task LoadAirlinesAsync()
        {
            var dataDirectory = FindDataDirectory();
            var csvFilename = Path.Combine(dataDirectory, "airlines.csv");
            await LoadAirlinesAsync(csvFilename);
        }

        private static async Task LoadAirlinesAsync(string csvFilename)
        {
            Console.WriteLine("Loading {0}", Path.GetFileName(csvFilename));
            
            var airlines = new List<Airline>();
            using (var csvReader = new CsvReader(new StreamReader(csvFilename)))
            {
                foreach (var airline in csvReader.GetRecords<Airline>())
                {
                    if (__seenAirlineIds.Contains(airline.Code))
                    {
                        airlines.Add(airline);
                    }
                }
            }

            var collection = __database.GetCollection<Airline>("airlines");
            await collection.InsertManyAsync(airlines);
        }

        private static async Task LoadAirportsAsync()
        {
            var dataDirectory = FindDataDirectory();
            var csvFilename = Path.Combine(dataDirectory, "airports.csv");
            await LoadAirportsAsync(csvFilename);
        }

        private static async Task LoadAirportsAsync(string csvFilename)
        {
            Console.WriteLine("Loading {0}", Path.GetFileName(csvFilename));

            var airports = new List<Airport>();
            using (var csvReader = new CsvReader(new StreamReader(csvFilename)))
            {
                foreach (var airport in csvReader.GetRecords<Airport>())
                {
                    if (__seenAirportIds.Contains(airport.Code))
                    {
                        airports.Add(airport);
                    }
                }
            }

            var collection = __database.GetCollection<Airport>("airports");
            await collection.InsertManyAsync(airports);
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
    }
}
