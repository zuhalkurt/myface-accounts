using System;

namespace MyFace.Data
{
    public static class DateGenerator
    {
        private static readonly Random Random = new Random();
        private const int MaxInteractionAge = 10000;

        public static DateTime GetPostDate()
        {
            // Posts happen longer ago than the max interaction age so that we don't risk
            // interacting with a post before it is created.
            var randomMinsAgo = Random.Next(1, MaxInteractionAge);
            return DateTime.Now.AddMinutes(-1 * (MaxInteractionAge + randomMinsAgo));
        }
        
        public static DateTime GetInteractionDate()
        {
            var randomMinsAgo = Random.Next(1, MaxInteractionAge);
            return DateTime.Now.AddMinutes(-1 * randomMinsAgo);
        }
    }
}
