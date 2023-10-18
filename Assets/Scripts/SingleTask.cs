using UnityEngine;

namespace Unfrozen
{
    [CreateAssetMenu(fileName = "Single Task", menuName = "Unfrozen/Single Task", order = 0)]
    public class SingleTask : BaseTask
    {
        [SerializeField] private BaseTask _subTask;

        public BaseTask SubTask => _subTask;
    }
}