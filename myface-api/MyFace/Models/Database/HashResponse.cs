using System.Collections.Generic;

namespace MyFace.Models.Database
{
    public class HashResponse
    {
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
      
    }
}