using System;
using System.Collections.Generic;
using Unfrozen.Configs;
using Unfrozen.Models;
using Unfrozen.Views;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Unfrozen.Controllers
{
    public class HeroesController : IInitializable, IDisposable
    {
        private readonly MissionsModel _missionsModel;
        private readonly HeroesModel _heroesModel;
        private readonly HeroesConfig _heroesConfig;
        private readonly HeroesPanelView _heroesView;
        
        private readonly Dictionary<string, HeroView> _createdHeroes
            = new Dictionary<string, HeroView>();

        private Dictionary<string, int> _preHeroScores = new Dictionary<string, int>();

        private string _activeHeroId;

        public HeroesController(MissionsModel missionsModel, 
            MainScreenView screenView, HeroesModel heroesModel,
            HeroesConfig heroesConfig)
        {
            _missionsModel = missionsModel;
            _heroesModel = heroesModel;
            _heroesConfig = heroesConfig;
            _heroesView = screenView.HeroesView;
        }

        public void Initialize()
        {
            _missionsModel.MissionSelected += UpdateHero;
            
            //Change to MissionEnd
            _missionsModel.MissionStarted += TryAddNewHero;
            
            InitHeroes();
            UpdateHeroPanel();
            SetActiveHero(_heroesConfig.DefaultHeroes[0], true);
        }

        public void Dispose()
        {
            _missionsModel.MissionSelected -= UpdateHero;
            _missionsModel.MissionStarted -= TryAddNewHero;

            _createdHeroes.Clear();
        }

        private void InitHeroes()
        {
            foreach (var defaultHero in _heroesConfig.DefaultHeroes)
            {
                _heroesModel.AddHeroToPool(defaultHero);
            }
            
            //Here set heroes from storage
        }

        private void TryAddNewHero()
        {
            var info = _missionsModel.ActiveMissionInfo;
            
            foreach (var hero in info.CharactersToUnlock)
            {
                if (!string.IsNullOrEmpty(hero))
                {
                    _heroesModel.AddHeroToPool(hero);
                }
            }

            foreach (var (id, score) in info.HeroPoints)
            {
                if (_createdHeroes.ContainsKey(id))
                {
                    _createdHeroes[id].SetScore(score);
                }
                
                //TODO: Fix later
                else if (id.Equals("Текущий"))
                {
                    _createdHeroes[_activeHeroId].SetScore(score);
                }
                else
                {
                    _preHeroScores.Add(id, score);
                }
            }

            UpdateHeroPanel();
        }

        private void UpdateHeroPanel()
        {
            foreach (var hero in _heroesModel.HeroesInPool)
            {
                if (_createdHeroes.ContainsKey(hero))
                {
                    continue;
                }

                var newHero = Object.Instantiate(_heroesConfig.View, _heroesView.Content);
                newHero.ToggleElement.group = _heroesView.MainToggle;
                newHero.SetHeroName(hero);

                _preHeroScores.TryGetValue(hero, out var heroScore);

                newHero.SetScore(heroScore);
                
                newHero.ToggleElement.onValueChanged.RemoveAllListeners();
                newHero.ToggleElement.onValueChanged.AddListener(value => SetActiveHero(hero, value));
                _createdHeroes.Add(hero, newHero);
            }
        }
        
        private void SetActiveHero(string heroID, bool toggleValue)
        {
            if (toggleValue)
            {
                _activeHeroId = heroID;
            }
        }

        private void UpdateHero()
        {
            _heroesModel.SetActiveHero(_activeHeroId);
        }
    }
}