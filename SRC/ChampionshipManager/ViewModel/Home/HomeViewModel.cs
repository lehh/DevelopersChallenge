using System.Collections.Generic;

namespace ChampionshipManager.ViewModel
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            ChampionshipList = new List<ChampionshipData>();
            TeamList = new List<TeamData>();
        }

        public struct ChampionshipData
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool Active { get; set; }
        }

        public struct TeamData
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public List<ChampionshipData> ChampionshipList { get; set; }
        public List<TeamData> TeamList { get; set; }

    }
}