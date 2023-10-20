using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Unfrozen.Tasks;
using UnityEditor;
using UnityEngine;

namespace Unfrozen
{
    [CreateAssetMenu(fileName = nameof(MainConfig), menuName = "Unfrozen/" + nameof(MainConfig), order = 0)]
    public class MainConfig : ScriptableObject
    {
        [SerializeField] private string _url;
        [SerializeField] private List<Mission> _tasks;

        [Button]
        private async void SyncTasks()
        {
            var hasSubMission = false;
            
            foreach (var mission in _tasks)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mission));
            }
            
            _tasks.Clear();

            var dataProvider = new GoogleSheetsDataProvider();
            await dataProvider.InitializeAsync(_url);
            var missionsData = dataProvider.GetMissionsData();
            
            var stack = new Stack<MissionInfo>(missionsData);

            while(stack.Count > 0)
            {
                var data = stack.Pop();
                
                var missionAsset = CreateInstance<Mission>();

                var splitTask = data.Id.Split(".");
                var currentIndex = splitTask[0];
                missionAsset.Id = currentIndex;
                
                missionAsset.AddInfo(data);

                Debug.Log(data.Id + " : " + data.Name + " : " + splitTask.Length);

                if (splitTask.Length > 1)
                {
                    while (true)
                    {
                        var nextInfo = stack.Peek();
                        var nextID = nextInfo.Id.Split(".")[0];
                        
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
                
                _tasks.Add(missionAsset);

                AssetDatabase.CreateAsset(missionAsset, Path.Combine("Assets/Configs", $"{splitTask[0]}.asset"));

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }
        }
    }
}