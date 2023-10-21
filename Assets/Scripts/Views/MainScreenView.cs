using UnityEngine;

namespace Unfrozen.Views
{
    public class MainScreenView : MonoBehaviour
    {
        [SerializeField] private Transform _pointsContent;
        [SerializeField] private MissionPanelView _missionPanel;
        [SerializeField] private HeroesPanelView _heroesPanelView;

        public Transform PointsContent => _pointsContent;
        public MissionPanelView MissionPanel => _missionPanel;
        public HeroesPanelView HeroesView => _heroesPanelView;
    }
}