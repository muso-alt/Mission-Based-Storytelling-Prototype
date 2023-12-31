﻿using System;
using System.Collections.Generic;
using Unfrozen.Configs;

namespace Unfrozen.Models
{
    public class MissionsModel
    {
        public readonly List<MissionInfo> CurrentSelectedMissionInfos = new List<MissionInfo>();
        public readonly List<string> PassedMissions = new List<string>();

        public MissionInfo ActiveMissionInfo { get; private set; }
        
        public event Action MissionStarted = delegate { };
        public event Action MissionSelected = delegate { };

        public void InvokeMissionSelect(IEnumerable<MissionInfo> infos)
        {
            CurrentSelectedMissionInfos.Clear();
            CurrentSelectedMissionInfos.AddRange(infos);
            MissionSelected?.Invoke();
        }

        public void InvokeMissionStart(MissionInfo info)
        {
            ActiveMissionInfo = info;
            MissionStarted?.Invoke();
        }
    }
}