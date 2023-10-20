using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unfrozen.Views
{
    public class MissionPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _missionText;
        [SerializeField] private TMP_Text _protagonistText;
        [SerializeField] private TMP_Text _antagonistText;
        [SerializeField] private TMP_Text _descriptionText;

        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startButton;
        
        public Button HideButton => _hideButton;
        
        public Button NextButton => _nextButton;
        public Button BackButton => _backButton;

        public Button StartButton => _startButton;

        public void SetMissionText(string text)
        {
            _missionText.text = text;
        }
        
        public void SetProtagonistText(string text)
        {
            _protagonistText.text = text;
        }
        
        public void SetAntagonistText(string text)
        {
            _antagonistText.text = text;
        }
        
        public void SetDescriptionText(string text)
        {
            _descriptionText.text = text;
        }
    }
}