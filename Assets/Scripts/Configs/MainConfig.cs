using System.Collections.Generic;
using System.IO;
using GoogleSheetImporter.Editor;
using Sirenix.OdinInspector;
using Unfrozen.Views;
using UnityEditor;
using UnityEngine;

namespace Unfrozen.Configs
{
    [CreateAssetMenu(fileName = nameof(MainConfig), menuName = "Unfrozen/" + nameof(MainConfig), order = 0)]
    public class MainConfig : ScriptableObject
    {
        [SerializeField] private string _url;
        [SerializeField] private MissionView _view;
        [SerializeField] private List<MissionConfig> _tasks;

        private bool _isLoading;
        public List<MissionConfig> Missions => _tasks;

        public MissionView View => _view;

        [Button]
        private async void SyncTasks()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            
            foreach (var mission in Missions)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mission));
            }
            
            Missions.Clear();

            var dataProvider = new GoogleSheetsDataProvider();
            await dataProvider.InitializeAsync(_url);
            var missionsData = dataProvider.GetMissionsData();
            
            var stack = new Stack<MissionInfo>(missionsData);

            while(stack.Count > 0)
            {
                var data = stack.Pop();
                
                var missionAsset = CreateInstance<MissionConfig>();

                var splitTask = data.MissionID.Split(".");
                var currentIndex = splitTask[0];
                missionAsset.SetMainID(currentIndex);
                
                missionAsset.AddInfo(data);

                if (splitTask.Length > 1)
                {
                    while (true)
                    {
                        var nextInfo = stack.Peek();
                        var nextID = nextInfo.MissionID.Split(".")[0];
                        
                        if (nextID == currentIndex)
                        {
                            stack.Pop();
                            missionAsset.AddInfo(nextInfo);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                
                Missions.Add(missionAsset);

                AssetDatabase.CreateAsset(missionAsset, Path.Combine("Assets/Configs", $"{splitTask[0]}.asset"));

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }

            _isLoading = false;
        }
    }
}