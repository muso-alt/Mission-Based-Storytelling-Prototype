using Unfrozen.Views;
using UnityEngine;

namespace Unfrozen.Configs
{
    [CreateAssetMenu(fileName = nameof(HeroesConfig), menuName = "Unfrozen/" + nameof(HeroesConfig), order = 0)]
    public class HeroesConfig : ScriptableObject
    {
        [SerializeField] private HeroView _heroView;
        [SerializeField] private string[] _defaultHeroes;

        public HeroView View => _heroView;
        public string[] DefaultHeroes => _defaultHeroes;
    }
}