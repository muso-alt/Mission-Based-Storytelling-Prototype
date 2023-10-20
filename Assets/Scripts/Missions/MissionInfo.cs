using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unfrozen.Tasks
{
    [Serializable]
    public class MissionInfo
    {
        public string Id;
        public string Name;
        public string PreviewText;
        public string MainText;
        public string[] FriendlySideText;
        public string[] OppositeSideText;
        public string[] CharactersToUnlock;
        public Dictionary<string, int> HeroPoints;
        public List<StringList> RequiredMissions;
        public string[] InactiveMissions;
        public Vector2 Position;
    }
}