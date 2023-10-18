using UnityEngine;

namespace Unfrozen
{
    public class BaseTask : ScriptableObject
    {
        [SerializeField] private string _taskHeader;
        [SerializeField] private string _taskDescription;

        public string TaskHeader => _taskHeader;
        public string TaskDescription => _taskDescription;
    }
}