using System;

namespace Sealegs.Backend.Identity
{
    public interface ISimpleHash
    {
        string Compute(string password, out string salt);

        string Compute(string password, int iterations, out string salt);

        bool Verify(string password, string passwordHashString);

        TimeSpan Estimate(string password, int iterations);
    }
}