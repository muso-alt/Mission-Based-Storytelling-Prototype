using System.Collections.Generic;

namespace Unfrozen.Models
{
    public class HeroesModel
    {
        public List<string> HeroesInPool = new List<string>();
        public string ActiveHero { get; private set; }

        public void AddHeroToPool(string heroId)
        {
            if (HeroesInPool.Contains(heroId))
            {
                return;
            }
            
            HeroesInPool.Add(heroId);
        }
        
        public void SetActiveHero(string heroID)
        {
            ActiveHero = heroID;
        }
    }
}