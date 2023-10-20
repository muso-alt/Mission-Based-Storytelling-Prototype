using System;
using System.Collections.Generic;
using System.Linq;
using Unfrozen.Models;
using Unfrozen.Tasks;
using Unfrozen.Views;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Unfrozen.Controllers
{
    public class MissionsMapController : IInitializable, IDisposable
    {
        private readonly MainConfig _mainConfig;
        private readonly MainScreenView _screenView;
        private readonly MissionsModel _missionsModel;
        private readonly Dictionary<Mission, MissionView> _missions = new Dictionary<Mission, MissionView>();
        private readonly Dictionary<string, string> _subMissions = new Dictionary<string, string>();
        private readonly List<string> _passedMissions = new List<string>();

        public MissionsMapController(MainConfig mainConfig,
            MainScreenView screenView, MissionsModel missionsModel)
        {
            _mainConfig = mainConfig;
            _screenView = screenView;
            _missionsModel = missionsModel;
        }
        
        public void Initialize()
        {
            //TODO: Must change to action mission end, if add gameplay system
            _missionsModel.MissionStarted += MissionCompleted;
            
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
        
        public void Dispose()
        {
            _missionsModel.MissionStarted -= MissionCompleted;
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
            _missionsModel.InvokeMissionSelect(mission.Infos);
        }

        private void MissionCompleted()
        {
            var info = _missionsModel.ActiveMissionInfo;
            _passedMissions.Add(info.Id);

            foreach (var (mission, view) in _missions)
            {
                if (!mission.Infos.Contains(info))
                {
                    continue;
                }
                
                view.SetCompleted();
                view.SetLockState(true);
                break;
            }
            
            UpdateMissionsState();
        }
    }
}