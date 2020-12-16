using System;
using System.Collections.Generic;

namespace TeamX.Utils
{
    public class TeamViewGroup : List<TeamView>
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }


        public TeamViewGroup(string title, string short_title)
        {
            Title = title;
            ShortTitle = short_title;
        }
    }
}
