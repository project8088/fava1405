using System;

namespace Nikan.DomainClasses
{
    public class UserToken
    {
        public int Id { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTime AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTime RefreshTokenExpiresDateTime { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}