using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JokeGenerator
{
    public interface INameService
    {
        Task<(string firstName, string lastName)> GetNamesAsync();
    }
}
