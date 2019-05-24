using System.Collections.Generic;

namespace ChampionshipManager.ViewModel
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            ChampionshipList = new List<ChampionshipData>();
        }

        public struct ChampionshipData
        {
            public string Name { get; set; }
        }

        public List<ChampionshipData> ChampionshipList { get; set; }

        //public int MyProperty { get; set; }
    }
}