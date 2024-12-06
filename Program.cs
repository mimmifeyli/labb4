using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Labb4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hämtar information från GitHub API...");

            // URL till GitHub API
            string url = "https://api.github.com/orgs/dotnet/repos";

            try
            {
                // Skapa en HttpClient-instans
                using HttpClient client = new HttpClient();

                // Ange en User-Agent (krävs av GitHub API)
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                // Skicka en HTTP GET-förfrågan
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Kontrollera om statuskoden är 200 OK

                // Läs svaret som en JSON-sträng
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialisera JSON-strängen till en lista av Repo-objekt
                List<Repo> repositories = JsonSerializer.Deserialize<List<Repo>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Gör det oberoende av stor bokstav
                });

                // Skriv ut informationen till konsolen
                Console.WriteLine("\nResultat:");
                foreach (var repo in repositories)
                {
                    Console.WriteLine($"Name: {repo.Name}");
                    Console.WriteLine($"Description: {repo.Description}");
                    Console.WriteLine($"HTML URL: {repo.HtmlUrl}");
                    Console.WriteLine($"Homepage: {repo.Homepage}");
                    Console.WriteLine($"Watchers: {repo.Watchers}");
                    Console.WriteLine($"Pushed At: {repo.PushedAt}");
                    Console.WriteLine(new string('-', 50));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade: {ex.Message}");
            }
        }
    }

    // Definiera en klass för att representera ett repository
    public class Repo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }

        [JsonPropertyName("homepage")]
        public string Homepage { get; set; }

        [JsonPropertyName("watchers")]
        public int Watchers { get; set; }

        [JsonPropertyName("pushed_at")]
        public DateTime PushedAt { get; set; }
    }
}

