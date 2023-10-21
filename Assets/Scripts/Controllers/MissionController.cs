using System;
using System.Collections.Generic;
using System.Text;
using Unfrozen.Models;
using Unfrozen.Tasks;
using Unfrozen.Views;
using UnityEngine;
using VContainer.Unity;

namespace Unfrozen.Controllers
{
    public class MissionController : IInitializable, IDisposable
    {
        private readonly MissionsModel _missionsModel;
        private readonly MissionPanelView _panelView;

        private int _currentIndex;
        private List<MissionInfo> _infos;

        public MissionController(MissionsModel missionsModel, MainScreenView screenView)
        {
            _missionsModel = missionsModel;
            _panelView = screenView.MissionPanel;
        }
        
        public void Initialize()
        {
            _missionsModel.MissionSelected += Show;
            _missionsModel.MissionStarted += UpdateStatusAfterPass;
            _panelView.StartButton.onClick.AddListener(StartMission);
            _panelView.HideButton.onClick.AddListener(Hide);
            
            _panelView.BackButton.onClick.AddListener(PreviousVariant);
            _panelView.NextButton.onClick.AddListener(NextVariant);
        }
        
        public void Dispose()
        {
            _missionsModel.MissionSelected -= Show;
            _missionsModel.MissionStarted -= UpdateStatusAfterPass;
            _panelView.StartButton.onClick.RemoveAllListeners();
            _panelView.HideButton.onClick.RemoveAllListeners();
            _panelView.BackButton.onClick.RemoveAllListeners();
            _panelView.NextButton.onClick.RemoveAllListeners();
        }

        private void Show()
        {
            UpdateView();
            _panelView.gameObject.SetActive(true);
        }

        private void UpdateView()
        {
            _infos = _missionsModel.CurrentSelectedMissionInfos;
            
            _panelView.SetMissionText(_infos[_currentIndex].Name);
            SetProtagonistText(_infos[_currentIndex].ProtagonistSideText);
            SetAntagonistText(_infos[_currentIndex].AntagonistSideText);
            _panelView.SetDescriptionText(_infos[_currentIndex].PreviewText);
            UpdateMissionVariants();
            
            ToggleStartButtonState(true);
        }

        private void UpdateStatusAfterPass()
        {
            _panelView.SetDescriptionText(_infos[_currentIndex].MainText);
            ToggleStartButtonState(false);
        }

        private void ToggleStartButtonState(bool value)
        {
            _panelView.StartButton.interactable = value;
            _panelView.StartButton.image.color = value ? Color.white : Color.green;
        }
        
        private void Hide()
        {
            _panelView.gameObject.SetActive(false);
            _currentIndex = 0;
            _infos = null;
        }

        private void UpdateMissionVariants()
        {
            if (_infos.Count <= 1)
            {
                _panelView.BackButton.gameObject.SetActive(false);
                _panelView.NextButton.gameObject.SetActive(false);
                return;
            }

            _panelView.BackButton.gameObject.SetActive(_currentIndex > 0);
            _panelView.NextButton.gameObject.SetActive(_currentIndex < _infos.Count - 1);
        }

        private void NextVariant()
        {
            if (_currentIndex >= _infos.Count)
            {
                return;
            }
            
            _currentIndex++;
            UpdateView();
        }

        private void PreviousVariant()
        {
            if (_currentIndex <= 0)
            {
                return;
            }
            
            _currentIndex--;
            UpdateView();
        }
        
        private void SetProtagonistText(string[] texts)
        {
            var heroes = GetHeroesAppend(texts);

            _panelView.SetProtagonistText("За кого играем: " + heroes);
        }
        
        private void SetAntagonistText(string[] texts)
        {
            var heroes = GetHeroesAppend(texts);

            _panelView.SetAntagonistText("Против кого играем: " + heroes);
        }

        private string GetHeroesAppend(string[] texts)
        {
            var builder = new StringBuilder();
            foreach (var text in texts)
            {
                builder.Append(text + ", ");
            }

            return builder.ToString();
        }

        private void StartMission()
        {
            _missionsModel.InvokeMissionStart(_infos[_currentIndex]);
        }
    }
}