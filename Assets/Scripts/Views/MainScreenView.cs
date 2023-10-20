using UnityEngine;

namespace Unfrozen.Views
{
    public class MainScreenView : MonoBehaviour
    {
        [SerializeField] private Transform _pointsContent;
        [SerializeField] private MissionPanelView _missionPanel;

        public Transform PointsContent => _pointsContent;
        public MissionPanelView MissionPanel => _missionPanel;
    }
}