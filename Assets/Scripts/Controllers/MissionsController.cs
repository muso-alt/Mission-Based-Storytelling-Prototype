using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<Mission, MissionView> _missions = new Dictionary<Mission, MissionView>();
        private readonly Dictionary<string, string> _subMissions = new Dictionary<string, string>();
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
                SetSubMissions(mission);

                view.MissionButton.onClick.AddListener(() => MissionClicked(mission));
                
                _missions.Add(mission, view);
            }
            
            UpdateMissionsState();
        }

        private void SetSubMissions(Mission mission)
        {
            foreach (var info in mission.Infos)
            {
                foreach (var inactiveMission in info.InactiveMissions)
                {
                    _subMissions[info.Id] = inactiveMission;
                }
            }
        }
        
        private void UpdateMissionsState()
        {
            foreach (var (mission, view) in _missions)
            {
                var state = GetMissionState(mission);
                
                switch (state)
                {
                    case MissionState.Active:
                        view.SetActive(true);
                        view.SetLockState(false);
                        break;
                    case MissionState.Inactive:
                        view.SetActive(true);
                        view.SetLockState(true);
                        break;
                    case MissionState.Blocked:
                        view.SetActive(false);
                        view.SetLockState(true);
                        break;
                    case MissionState.Passed:
                        view.SetActive(true);
                        view.SetLockState(false);
                        view.SetCompleted();
                        break;
                }
            }
        }

        private MissionState GetMissionState(Mission mission)
        {
            var passed = MissionState.Blocked;
            
            foreach (var info in mission.Infos)
            {
                passed = info.RequiredMissions.Count == 0 ? MissionState.Active : MissionState.Blocked;

                foreach (var stringList in info.RequiredMissions)
                {
                    passed = stringList.Items.Any(item =>
                        _passedMissions.Contains(item)) ? MissionState.Active : MissionState.Blocked;

                    if (passed == MissionState.Blocked)
                    {
                        break;
                    }
                }
                
                if(passed == MissionState.Active)
                {
                    foreach (var (id, subId) in _subMissions)
                    {
                        if (!subId.Equals(info.Id))
                        {
                            continue;
                        }

                        passed = _passedMissions.Contains(id) ? MissionState.Active : MissionState.Inactive;
                        break;
                    }
                }

                if (_passedMissions.Contains(info.Id))
                {
                    passed = MissionState.Passed;
                    break;
                }
            }

            return passed;
        }

        private void MissionClicked(Mission mission)
        {
            _passedMissions.Add(mission.Infos[0].Id);
            _missions[mission].SetCompleted();
            _missions[mission].SetLockState(true);
            UpdateMissionsState();
        }
    }
}