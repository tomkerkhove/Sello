using System;

namespace Sello.Common.Exceptions
{
    public class SecretNotFoundException : Exception
    {
        public SecretNotFoundException(string secretName) : base($"Secret with name '{secretName}' was not found")
        {
            SecretName = secretName;
        }

        public string SecretName { get; }
    }
}