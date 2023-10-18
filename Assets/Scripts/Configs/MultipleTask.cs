using UnityEngine;

namespace Unfrozen.Configs
{
    [CreateAssetMenu(fileName = "Multiple Task", menuName = "Unfrozen/Multiple Task", order = 0)]
    public class MultipleTask : BaseTask
    {
        [SerializeField] private BaseTask[] _subTasks;

        public BaseTask[] SubTasks => _subTasks;
    }
}