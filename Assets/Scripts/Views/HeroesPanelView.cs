using UnityEngine;
using UnityEngine.UI;

namespace Unfrozen.Views
{
    public class HeroesPanelView : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private RectTransform _content;

        public ToggleGroup MainToggle => _toggleGroup;
        public RectTransform Content => _content;
    }
}