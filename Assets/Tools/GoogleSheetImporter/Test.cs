using System;
using Unfrozen;
using UnityEngine;

namespace GoogleSheetImporter.Editor
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private string _url;
        private async void Start()
        {
            var dataProvider = new GoogleSheetsDataProvider();
            await dataProvider.InitializeAsync(_url);
            var missionsData = dataProvider.GetMissionsData();
        }
    }
}