using System;
using System.Linq;
using System.Threading.Tasks;

namespace JokeGenerator
{
    class Program
    {
        private static readonly ConsolePrinter printer = new ConsolePrinter();
        private static readonly IJokeService jokeService = new JokeService();
        private static readonly INameService nameService = new NameService();

        private const int MIN_JOKES = 1;
        private const int MAX_JOKES = 9;

        static async Task Main(string[] args)
        {
            printer.print("This app shows random Chuck Norris jokes");
            printer.print("Press ? to get instructions or any other key to exit.");
            if (Console.ReadLine() == "?")
            {
                bool endApp = false;
                while (!endApp)
                {
                    try
                    {
                        printer.print("");
                        printer.print("Press c to get categories");
                        printer.print("Press r to get random jokes");
                        switch (Console.ReadLine())
                        {
                            case "c":
                                string[] categories = await jokeService.GetCategoriesAsync();
                                PrintResults(categories);
                                break;

                            case "r":
                                string[] jokes = await GetRandomJokes();
                                PrintResults(jokes);
                                break;
                            default:
                                printer.print("Unknown option");
                                break;
                        }
                        printer.print("Press 'n' to close the app or any other key to continue: ");
                        if (Console.ReadLine() == "n") endApp = true;
                    }
                    catch(BusinessException e)
                    {
                        printer.print(e.Message);
                    }
                    catch(Exception e)
                    {
                        printer.print($"An exception occured during execution.\n - Details: {e.Message}");
                    }
                }
            }

        }

        private async static Task<string[]> GetRandomJokes()
        {
            (string firstName, string lastName) = await AskIfUseRandomName();
            string category = await AskIfUseCategory();
            int jokesNumber = AskIfUseJokesNumber();

            return await jokeService.GetRandomJokesAsync(firstName, lastName, category, jokesNumber);
        }

        private static async Task<(string firstName, string lastName)> AskIfUseRandomName()
        {
            printer.print("Want to use a random name? y/n");
            if (Console.ReadLine() == "y")
            {
                return await nameService.GetNamesAsync();
            }
            return (null, null);
        }

        private static async Task<string> AskIfUseCategory()
        {
            printer.print("Want to specify a category? y/n");
            if (Console.ReadLine() == "y")
            {
                printer.print("Enter a category;");
                string category = Console.ReadLine();
                string[] categories = await jokeService.GetCategoriesAsync();
                if (!categories.Contains(category))
                {
                    throw new BusinessException("There is no such category");
                }
                return category;
            }
            return null;
        }

        private static int AskIfUseJokesNumber()
        {
            printer.print($"How many jokes do you want? ({MIN_JOKES}-{MAX_JOKES})");
            try
            {
                int jokesNumber = Int32.Parse(Console.ReadLine());
                if (jokesNumber >= MIN_JOKES && jokesNumber <= MAX_JOKES)
                {
                    return jokesNumber;
                }
                throw new BusinessException("Wrong number of jokes entered");
            }
            catch(FormatException e)
            {
                throw new BusinessException("Number of jokes entered has incorrect format");
            }
            
        }       

        private static void PrintResults(string[] results)
        {
            if (results.Length > 0)
            {
                printer.print("Here are your results:");
                for (int i = 0; i < results.Length; i++)
                {
                    printer.print($"{i + 1}: {results[i]}");
                }
            }
        }        

    }
}
