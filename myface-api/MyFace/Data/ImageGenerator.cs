using System.Collections.Generic;
using System.Linq;

namespace MyFace.Data
{
    public static class ImageGenerator
    {
        private static readonly IEnumerable<int> BrokenImages = new List<int>();
        
        public static string GetPostImage(int index)
        {
            return GetImage(100 + index, 1600, 900);
        }

        public static string GetProfileImage(string username)
        {
            return $"https://robohash.org/{username}?set=any&bgset=any";
        }

        public static string GetCoverImage(int index)
        {
            return GetImage(600 + index, 2400, 900);
        }

        private static string GetImage(int index, int width, int height)
        {
            return BrokenImages.Contains(index)
                ? null 
                : $"https://picsum.photos/id/{index}/{width}/{height}.jpg";
        }
    }
}
