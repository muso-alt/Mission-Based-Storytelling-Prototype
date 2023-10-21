using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unfrozen.Views
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleElement;
        [SerializeField] private TMP_Text _heroName;
        [SerializeField] private TMP_Text _score;

        public Toggle ToggleElement => _toggleElement;
        private int _heroScore = 0;

        public void SetHeroName(string text)
        {
            _heroName.text = text;
        }

        public void SetScore(int score)
        {
            _heroScore += score;
            _score.text = "Очки: " + _heroScore;
        }
    }
}