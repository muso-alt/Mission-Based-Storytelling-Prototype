using System.Collections.Generic;
using UnityEngine;

namespace Unfrozen.Tasks
{
    [CreateAssetMenu(fileName = nameof(Mission), menuName = "Unfrozen/" + nameof(Mission), order = 0)]
    public class Mission : ScriptableObject
    {
        [SerializeField] private List<MissionInfo> _missionInfos;
        
        public string Id;
        
        public MissionState CurrentState { get; set; }
        public int CurrentInfoIndex { get; set; }
        
        public List<MissionInfo> Infos => _missionInfos;
        public Vector2 Position => Infos[0].Position;

        public void AddInfo(MissionInfo info)
        {
            _missionInfos ??= new List<MissionInfo>();
            _missionInfos.Add(info);
        }
    }
}