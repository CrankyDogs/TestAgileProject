using System;

namespace TestProjLarge.Entities
{
    public class User
    {
        public string Username{ get; set; }

        public DateTime? expiresAt { get; set; }

        public string token { get; set; }
    }
}
