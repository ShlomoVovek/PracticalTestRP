using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PracticalTestRP.Models;
using PracticalTestRP.Enums;
using PracticalTestRP.Interfaces;
using PracticalTestRP.Services;


namespace PracticalTestRP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("User Data Sync Application - Practical Test For RP");

            using var httpClient = new HttpClient();

            var userSources = InitializeUserSources(httpClient);
            var exporters = InitializeExporters();

            List<User> allUsers = await FetchAllUsersAsync(userSources);

            if (allUsers.Count == 0)
            {
                Console.WriteLine("No users fetched. Exiting application.");
                ExitApplication();
                return;
            }

            string folderPath = GetFolderPathFromUser();
            FileFormat desiredFormat = GetFileFormatFromUser();

            await SaveUserDataAsync(allUsers, folderPath, desiredFormat, exporters);

            Console.WriteLine("\nApplication finished. Press any key to exit.");
            Console.ReadKey();
        }

        // Initializes and returns a list of user data sources.
        private static List<IUserSource> InitializeUserSources(HttpClient httpClient)
        {
            Console.WriteLine("Initializing user data sources...");
            return new List<IUserSource>
            {
                new RandomUserSource(httpClient),
                new JsonPlaceholderSource(httpClient),
                new DummyJsonSource(httpClient),
                new ReqresInSource(httpClient)
            };
        }


        // Initializes and returns a dictionary of data exporters.
        private static Dictionary<FileFormat, IDataExporter> InitializeExporters()
        {
            Console.WriteLine("Initializing data exporters...");
            return new Dictionary<FileFormat, IDataExporter>
            {
                { FileFormat.Json, new JsonExporter() },
                { FileFormat.Csv, new CsvExporter() }
            };
        }

        // Fetches users from all configured sources concurrently.
        private static async Task<List<User>> FetchAllUsersAsync(List<IUserSource> userSources)
        {
            Console.WriteLine("Fetching user data from sources...");
            var allUsers = new List<User>();
            var tasks = userSources.Select(async source =>
            {
                try
                {
                    var users = await source.GetUsersAsync();
                    lock (allUsers) // Thread-safe addition to the shared list
                    {
                        allUsers.AddRange(users);
                    }
                    Console.WriteLine($"Fetched {users.Count()} users from {source.SourceName}.");
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"Error fetching from {source.SourceName} (HTTP Error): {httpEx.Message}");
                    // Consider logging httpEx.StackTrace to a file for detailed debugging
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred while fetching from {source.SourceName}: {ex.Message}");
                    // Consider logging ex.StackTrace to a file for detailed debugging
                }
            }).ToList();

            await Task.WhenAll(tasks); // Wait for all parallel fetches to complete
            Console.WriteLine($"\nTotal users fetched from all sources: {allUsers.Count}");
            return allUsers;
        }

        // Prompts the user to enter a folder path and ensures it's valid or can be created.
        private static string GetFolderPathFromUser()
        {
            string folderPath;
            while (true)
            {
                Console.Write("Enter the folder path to save the file: ");
                folderPath = Console.ReadLine()?.Trim(); // Trim whitespace

                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    Console.WriteLine("Folder path cannot be empty. Please try again.");
                }
                else if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine($"Folder '{folderPath}' does not exist. Attempting to create it...");
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        Console.WriteLine("Folder created successfully.");
                        break;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Access denied to create folder. Please provide a valid path with write permissions.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating folder: {ex.Message}. Please try again.");
                    }
                }
                else
                {
                    break; // Folder exists
                }
            }
            return folderPath;
        }

        // Prompts the user to enter a desired file format (JSON/CSV) and validates the input.
        private static FileFormat GetFileFormatFromUser()
        {
            FileFormat desiredFormat;
            while (true)
            {
                Console.Write("Enter the desired file format (JSON/CSV): ");
                string formatInput = Console.ReadLine()?.Trim();
                if (Enum.TryParse(formatInput, true, out desiredFormat) && Enum.IsDefined(typeof(FileFormat), desiredFormat))
                {
                    break; // Valid format
                }
                else
                {
                    Console.WriteLine("Invalid format. Please enter 'JSON' or 'CSV'.");
                }
            }
            return desiredFormat;
        }

        // Saves the list of users to a file in the specified format.        
        private static async Task SaveUserDataAsync(
            List<User> users,
            string folderPath,
            FileFormat format,
            Dictionary<FileFormat, IDataExporter> exporters)
        {
            Console.WriteLine($"\nSaving users to {format.ToString().ToUpper()} format...");
            string fileName = $"users_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}"; // Unique filename

            try
            {
                if (exporters.TryGetValue(format, out var exporter))
                {
                    await exporter.ExportAsync(folderPath, fileName, users);
                }
                else
                {
                    Console.WriteLine($"Error: No exporter found for format: {format}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
                // Consider logging ex.StackTrace to a file for detailed debugging
            }
        }

        private static void ExitApplication()
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
