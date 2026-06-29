using Microsoft.Extensions.Caching.Memory;
using System;
 

namespace Nikan.Services.RateLimiter
{
    public interface IMemoryRateLimiterService
    {
        bool IsLimited(string key, int? maxAttempts = 7);
        void RegisterFail(string key);
        void Reset(string key);
    }
    public class MemoryRateLimiterService : IMemoryRateLimiterService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(15);

        public MemoryRateLimiterService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool IsLimited(string key, int? maxAttempts = 7)
        {
            if (_cache.TryGetValue(key, out int attempts))
            {
                return attempts >= maxAttempts;
            }
            return false;
        }

        public void RegisterFail(string key)
        {
            int attempts = 0;
            if (_cache.TryGetValue(key, out int value))
                attempts = value;

            attempts++;

            _cache.Set(key, attempts, Window);
        }

        public void Reset(string key)
        {
            _cache.Remove(key);
        }
    }
}
