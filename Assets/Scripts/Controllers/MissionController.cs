using System;
using System.Collections.Generic;
using System.Text;
using Unfrozen.Models;
using Unfrozen.Configs;
using Unfrozen.Views;
using UnityEngine;
using VContainer.Unity;

namespace Unfrozen.Controllers
{
    public class MissionController : IInitializable, IDisposable
    {
        private readonly MissionsModel _missionsModel;
        private readonly HeroesModel _heroesModel;
        private readonly MissionPanelView _panelView;

        private int _currentIndex;
        private List<MissionInfo> _infos;

        public MissionController(MissionsModel missionsModel, 
            MainScreenView screenView, HeroesModel heroesModel)
        {
            _missionsModel = missionsModel;
            _heroesModel = heroesModel;
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
            ToggleStartButtonState(true);
            _panelView.gameObject.SetActive(true);
        }

        private void UpdateView()
        {
            _infos = _missionsModel.CurrentSelectedMissionInfos;
            
            _panelView.SetMissionText(_infos[_currentIndex].MissionName);
            SetProtagonistText(_infos[_currentIndex].ProtagonistSideText);
            SetAntagonistText(_infos[_currentIndex].AntagonistSideText);
            _panelView.SetDescriptionText(_infos[_currentIndex].PreviewText);
            
            UpdateMissionVariants();
        }

        private void UpdateStatusAfterPass()
        {
            _panelView.SetDescriptionText(_infos[_currentIndex].MainText);
            ToggleStartButtonState(false);
            _panelView.BackButton.gameObject.SetActive(false);
            _panelView.NextButton.gameObject.SetActive(false);
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
        
        private void SetProtagonistText(IReadOnlyList<string> texts)
        {
            var heroes = GetSideAppend(texts, false);

            _panelView.SetProtagonistText("За кого играем: " + heroes);
        }
        
        private void SetAntagonistText(IReadOnlyList<string> texts)
        {
            var heroes = GetSideAppend(texts, true);

            _panelView.SetAntagonistText("Против кого играем: " + heroes);
        }

        private string GetSideAppend(IReadOnlyList<string> texts, bool isAgainst)
        {
            var builder = new StringBuilder();
            var count = texts.Count;

            for (var i = 0; i < count; i++)
            {
                var fullString = GetSideByPass(texts[i], isAgainst);

                if (string.IsNullOrEmpty(fullString))
                {
                    continue;
                }
                
                if (i < count - 1)
                {
                    fullString += ", ";
                }

                builder.Append(fullString);
            }

            return builder.ToString();
        }

        private string GetSideByPass(string text, bool isAgainst)
        {
            if (!text.Contains(":"))
            {
                return text;
            }

            var sides = text.Split(":");
            var id = sides[1].Replace(" ", "");
            text = sides[0];

            var passed = _missionsModel.PassedMissions.Contains(id);

            //Big brain moment...
            if (isAgainst && passed)
            {
                text = string.Empty;
            }else if (!isAgainst && !passed)
            {
                text = string.Empty;
            }

            return text;
        }

        private void StartMission()
        {
            _missionsModel.InvokeMissionStart(_infos[_currentIndex]);
        }
    }
}