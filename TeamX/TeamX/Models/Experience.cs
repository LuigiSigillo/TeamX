using System;
using System.Drawing;
using TeamX.Utils;

namespace TeamX.Models
{
    /// <summary>
    /// Represents the experience of a user in a specific category
    /// </summary>
    public class Experience
    {
        public string Id { get; set; }
        public int Level { get; private set; }
        public int CategoryId { get; private set; }
        public string Category { get; private set; }

        public Experience(int level, int catId, string category)
        {
            Id = Generator.GenerateID();
            Level = level;
            CategoryId = catId;
            Category = category;
        }
    }
}
