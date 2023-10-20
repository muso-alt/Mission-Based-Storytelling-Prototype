using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unfrozen.Views
{
    public class MissionView : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] private Button _missionButton;
        [SerializeField] private Image _missionImage;

        public Button MissionButton => _missionButton;
        
        public void SetId(string id)
        {
            if (id.Contains('.'))
            {
                _tmp.text = id.Split('.')[0];
                return;
            }

            _tmp.text = id;
        }

        public void SetCompleted()
        {
            _missionImage.color = Color.green;
        }

        public void SetActive(bool state)
        {
            _content.SetActive(state);
        }
        
        public void SetLockState(bool state)
        {
            _missionButton.interactable = !state;
        }
    }
}