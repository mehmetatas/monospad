using System;

namespace Monospad.Core.Models.Database
{
    public class Note
    {
        public long Id { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public Guid AccessToken { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
