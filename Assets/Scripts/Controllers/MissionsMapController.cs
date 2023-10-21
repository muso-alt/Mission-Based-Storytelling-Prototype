using System;
using System.Collections.Generic;
using System.Linq;
using Unfrozen.Configs;
using Unfrozen.Models;
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
        private readonly Dictionary<MissionConfig, MissionView> _missions = new Dictionary<MissionConfig, MissionView>();
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
            //TODO: Must change to action missionConfig end, if add gameplay system
            _missionsModel.MissionStarted += MissionCompleted;
            
            foreach (var mission in _mainConfig.Missions)
            {
                var view = Object.Instantiate(_mainConfig.View, _screenView.PointsContent);
                view.SetId(mission.ID);
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

        private void SetSubMissions(MissionConfig missionConfig)
        {
            foreach (var info in missionConfig.Infos)
            {
                foreach (var inactiveMission in info.InactiveMissions)
                {
                    _subMissions[info.MissionID] = inactiveMission;
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
                        view.SetCompleted();
                        break;
                }
            }
        }

        private MissionState GetMissionState(MissionConfig missionConfig)
        {
            var passed = MissionState.Blocked;
            
            foreach (var info in missionConfig.Infos)
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
                        if (!subId.Equals(info.MissionID))
                        {
                            continue;
                        }

                        passed = _passedMissions.Contains(id) ? MissionState.Active : MissionState.Inactive;
                        break;
                    }
                }

                if (_passedMissions.Contains(info.MissionID))
                {
                    passed = MissionState.Passed;
                    break;
                }
            }

            return passed;
        }

        private void MissionClicked(MissionConfig missionConfig)
        {
            _missionsModel.InvokeMissionSelect(missionConfig.Infos);
        }

        private void MissionCompleted()
        {
            var info = _missionsModel.ActiveMissionInfo;
            _passedMissions.Add(info.MissionID);

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