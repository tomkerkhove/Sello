using System;
using System.Text;

namespace Sello.Api.Exceptions
{
    public class ChaosMonkeyException : Exception
    {
        public ChaosMonkeyException()
        {
            var generator = new NamesGenerator();
            MonkeyName = generator.GetRandomName();
        }

        public string MonkeyName { get; }
    }
}