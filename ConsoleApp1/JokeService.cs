using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator
{
    public class JokeService : IJokeService
    {
        private const string RANDOM_JOKES_URL = "jokes/random";
        private const string CATEGORIES_URL = "jokes/categories";
        public const string NAME_TO_REPLACE = "Chuck Norris";
        private const int MAX_CONNECTIONS = 4;
        private static readonly HttpClient httpClient;

        static JokeService()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.chucknorris.io/")
            };
            ServicePointManager.DefaultConnectionLimit = MAX_CONNECTIONS;
        }

        public async Task<string[]> GetRandomJokesAsync(string firstname, string lastname, string category, int jokesNumber)
        {
            string url = RANDOM_JOKES_URL;
            if (category != null)
            {
                url += $"?category={category}";
            }

            var jokes = new List<string>();
            var tasks = new List<Task<string>>();
            for (int i = 0; i < jokesNumber; i++)
            {
                tasks.Add(httpClient.GetStringAsync(url));
            }
            await Task.WhenAll(tasks);
            tasks.ForEach(t =>
            {
                string joke = JsonConvert.DeserializeObject<dynamic>(t.Result).value;
                if (firstname != null && lastname != null)
                {
                    joke = joke.Replace(NAME_TO_REPLACE, $"{firstname} {lastname}");
                }
                jokes.Add(joke);
            });

            return jokes.ToArray();
        }

        public async Task<string[]> GetCategoriesAsync()
        {
            string result = await httpClient.GetStringAsync(CATEGORIES_URL);
            return JsonConvert.DeserializeObject<string[]>(result);
        }
    }
}
