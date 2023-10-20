using UnityEngine;

namespace Unfrozen.Views
{
    public class MainScreenView : MonoBehaviour
    {
        [SerializeField] private Transform _pointsContent;

        public Transform PointsContent => _pointsContent;
    }
}