using System.Collections.Generic;
using UnityEngine;

namespace Unfrozen.Configs
{
    [CreateAssetMenu(fileName = nameof(MissionConfig), menuName = "Unfrozen/" + nameof(MissionConfig), order = 0)]
    public class MissionConfig : ScriptableObject
    {
        [SerializeField] private List<MissionInfo> _missionInfos;
        
        [SerializeField] private string _id;

        public List<MissionInfo> Infos => _missionInfos;
        public Vector2 Position => Infos[0].MissionPosition;

        public string ID => _id;

        public void AddInfo(MissionInfo info)
        {
            _missionInfos ??= new List<MissionInfo>();
            _missionInfos.Add(info);
        }

        public void SetMainID(string id)
        {
            _id = id;
        }
    }
}