using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator
{
    public class NameService : INameService
    {
        private static readonly HttpClient httpClient;
        static NameService()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.names.privserv.com/api/")
            };
        }

        /// <summary>
        /// Fetches a random name and surname from a web service
        /// </summary>
        /// <returns>Returns an object that contains name and surname</returns>
        public async Task<(string firstName, string lastName)> GetNamesAsync()
        {
            var result = await httpClient.GetStringAsync("");
            dynamic json = JsonConvert.DeserializeObject<dynamic>(result);
            return (json.name, json.surname);
        }
    }
}
