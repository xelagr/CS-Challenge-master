using JokeGenerator;
using System;
using Xunit;

namespace JokeGeneratorTests
{
    public class JokeGeneratorTest
    {
        [Fact]
        public async void GetRandomName()
        {
            INameService nameService = new NameService();
            (string firstName, string lastName) = await nameService.GetNamesAsync();
            Assert.NotNull(firstName);
            Assert.NotNull(lastName);
        }

        [Fact]
        public async void ProperNumberOfJokesReturned()
        {
            int jokesNumber = 2;
            IJokeService jokeService = new JokeService();
            string[] jokes = await jokeService.GetRandomJokesAsync(null, null, null, jokesNumber);
            Assert.Equal(jokesNumber, jokes.Length);
        }

        [Fact]
        public async void JokeHasPredefinedName()
        {
            IJokeService jokeService = new JokeService();
            string[] jokes = await jokeService.GetRandomJokesAsync(null, null, null, 1);
            Assert.NotEmpty(jokes);
            Assert.Contains(JokeService.NAME_TO_REPLACE, jokes[0]);
        }

        [Fact]
        public async void IfNamePassed_JokeDoesNotHavePredefinedName()
        {
            IJokeService jokeService = new JokeService();
            const string Firstname = "name";
            const string Lastname = "surname";
            string[] jokes = await jokeService.GetRandomJokesAsync(Firstname, Lastname, null, 1);
            Assert.NotEmpty(jokes);
            Assert.Contains($"{Firstname} {Lastname}", jokes[0]);
        }

        [Fact]
        public async void GetCategories()
        {
            IJokeService jokeService = new JokeService();
            string[] categories = await jokeService.GetCategoriesAsync();
            Assert.NotEmpty(categories);
        }
        
    }
}
