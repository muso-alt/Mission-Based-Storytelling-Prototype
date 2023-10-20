using System.Collections.Generic;
using Unfrozen.Tasks;
using Unfrozen.Views;
using UnityEngine;
using VContainer.Unity;

namespace Unfrozen.Controllers
{
    public class MissionsController : IInitializable
    {
        private readonly MainConfig _mainConfig;
        private readonly MainScreenView _screenView;
        private readonly List<MissionView> _views = new List<MissionView>();
        private readonly List<string> _passedMissions = new List<string>();

        public MissionsController(MainConfig mainConfig, MainScreenView screenView)
        {
            _mainConfig = mainConfig;
            _screenView = screenView;
        }
        
        public void Initialize()
        {
            foreach (var mission in _mainConfig.Missions)
            {
                var view = Object.Instantiate(_mainConfig.View, _screenView.PointsContent);
                view.SetId(mission.Id);
                view.transform.localPosition = mission.Position;

                var canShow = RequiredMissionPassed(mission);
                view.SetActive(canShow);
            }
        }

        private bool RequiredMissionPassed(Mission mission)
        {
            var passed = false;
            
            foreach (var info in mission.Infos)
            {
                passed = info.RequiredMissions.Count == 0;
                
                foreach (var stringList in info.RequiredMissions)
                {
                    foreach (var item in stringList.Items)
                    {
                        if (_passedMissions.Contains(item))
                        {
                            passed = true;
                        }
                    }
                }
            }

            return passed;
        }
    }
}