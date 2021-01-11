using System.Threading.Tasks;

namespace JokeGenerator
{
    public interface IJokeService
    {
        Task<string[]> GetCategoriesAsync();
        Task<string[]> GetRandomJokesAsync(string firstname, string lastname, string category, int jokesNumber);
    }
}