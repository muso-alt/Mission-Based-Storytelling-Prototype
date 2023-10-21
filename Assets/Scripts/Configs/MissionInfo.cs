using System;
using System.Collections.Generic;
using Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace Unfrozen.Configs
{
    [Serializable]
    public class MissionInfo
    {
        [SerializeField] private string _missionID;
        [SerializeField] private string _missionName;
        [SerializeField] private string _previewText;
        [SerializeField] private string _mainText;
        [SerializeField] private string[] _protagonistSideText;
        [SerializeField] private string[] _antagonistSideText;
        [SerializeField] private string[] _charactersToUnlock;
        
        [SerializeField] private StringAndIntDictionary _heroPoints;
        [SerializeField] private List<StringList> _requiredMissions;
        [SerializeField] private string[] _inactiveMissions;
        [SerializeField] private Vector2 _missionPosition;

        public string MissionID => _missionID;
        public string MissionName => _missionName;
        public string PreviewText => _previewText;
        public string MainText => _mainText;
        public string[] ProtagonistSideText => _protagonistSideText;
        public string[] AntagonistSideText => _antagonistSideText;
        public string[] CharactersToUnlock => _charactersToUnlock;
        public StringAndIntDictionary HeroPoints => _heroPoints;
        public List<StringList> RequiredMissions => _requiredMissions;
        public string[] InactiveMissions => _inactiveMissions;
        public Vector2 MissionPosition => _missionPosition;

        public void SetID(string id)
        {
            _missionID = id;
        }

        public void SetMissionName(string missionName)
        {
            _missionName = missionName;
        }

        public void SetPreviewText(string previewText)
        {
            _previewText = previewText;
        }

        public void SetMainText(string mainText)
        {
            _mainText = mainText;
        }

        public void SetProtagonistSideText(string[] text)
        {
            _protagonistSideText = text;
        }
        
        public void SetAntagonistSideText(string[] text)
        {
            _antagonistSideText = text;
        }
        
        public void SetCharactersToUnlock(string[] text)
        {
            _charactersToUnlock = text;
        }

        public void SetHeroPoints(Dictionary<string, int> heroPoints)
        {
            _heroPoints = new StringAndIntDictionary();
            HeroPoints.AddRange(heroPoints);
        }

        public void SetRequiredMissions(List<StringList> requiredMissions)
        {
            _requiredMissions = requiredMissions;
        }

        public void SetInactiveMissions(string[] inactiveMissions)
        {
            _inactiveMissions = inactiveMissions;
        }

        public void SetMissionPosition(Vector2 position)
        {
            _missionPosition = position;
        }
    }
}