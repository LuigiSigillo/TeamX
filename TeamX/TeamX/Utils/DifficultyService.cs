using System;
using System.Collections.Generic;
using System.Text;

namespace TeamX.Utils
{
    public static class DifficultyService
    {
        public static Dictionary<int, string> Difficulties = new Dictionary<int, string>()
        {
            {1, "Beginner" },
            {2, "Intermediate" },
            {3, "Advanced" },
        };

        public static List<string> GetDifficulties()
        {
            List<string> difficulties = new List<string>(Difficulties.Values);
            return difficulties;
        }

        public static string GetDifficulty(int id)
        {
            return Difficulties[id];
        }
    }
}
