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
            var idText = id.Split('.')[0];

            _tmp.text = idText;
        }

        public void SetCompleted()
        {
            _missionImage.color = Color.green;
            _missionButton.interactable = false;
        }

        public void SetActive(bool state)
        {
            _content.SetActive(state);
        }
        
        public void SetLockState(bool state)
        {
            _missionImage.color = state ? Color.gray : Color.white;
            _missionButton.interactable = !state;
        }
    }
}