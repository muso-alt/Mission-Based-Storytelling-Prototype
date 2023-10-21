using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using Unfrozen.Configs;
using UnityEngine;

namespace GoogleSheetImporter.Editor
{
    public class GoogleSheetsDataProvider
    {
        private List<List<string>> _data;
        private List<MissionInfo> _missionsData;

        public async UniTask InitializeAsync(string url)
        {
            var importer = new GSImporter();
            var operationData = await importer.DownloadAsync(url);

            if (string.IsNullOrEmpty(operationData.sheetData))
            {
                throw new Exception($"Не удалось скачать таблицу ошибка {operationData.error}");
            }

            var (data, ok) = Parser.ParseTable(operationData.sheetData, true);

            if (ok)
                _data = data;
            else
                throw new Exception("Не удалось распарсить таблицу");
        }

        public List<MissionInfo> GetMissionsData()
        {
            if (_missionsData != null)
                return _missionsData;

            _missionsData = new List<MissionInfo>();

            for (var row = 1; row < _data.Count; row++)
            {
                var currentRow = _data[row];

                var info = new MissionInfo();

                info.SetID(currentRow[0]);
                info.SetMissionName(currentRow[1]);
                info.SetPreviewText(currentRow[2]);
                info.SetMainText(currentRow[3]);
                info.SetProtagonistSideText(ParseText(currentRow[4], ","));
                info.SetAntagonistSideText(ParseText(currentRow[5], ","));
                info.SetCharactersToUnlock(ParseText(currentRow[6], ","));
                info.SetHeroPoints(ParseHeroPoints(currentRow[7]));
                info.SetRequiredMissions(ParseRequiredMissions(currentRow[8]));
                info.SetInactiveMissions(ParseText(currentRow[9], ","));
                info.SetMissionPosition(ParsePosition(currentRow[10]));
                
                _missionsData.Add(info);
            }

            _missionsData.Reverse();
            return _missionsData;
        }

        private string[] ParseText(string text, string separator)
        {
            var items = text.Split(separator);
            var output = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                output[i] = item.Trim();
            }

            return output;
        }

        private Dictionary<string, int> ParseHeroPoints(string text)
        {
            var separator = "\n";
            var output = new Dictionary<string, int>();
            var values = ParseText(text, separator);
            for (int i = 0; i < values.Length; i++)
            {
                var pair = ParseText(values[i], ":");
                var heroId = pair[0];
                var points = Convert.ToInt32(pair[1]);
                output.Add(heroId, points);
            }

            return output;
        }

        private Vector2 ParsePosition(string text)
        {
            var p = ParseText(text, ",");
            return new Vector2(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]));
        }

        private List<StringList> ParseRequiredMissions(string text)
        {
            var values = ParseText(text, ",");
            var output = new List<StringList> (values.Length);
            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value)) continue;

                if (value.Contains("или"))
                {
                    var pair = ParseText(value, "или");
                    output.Add(new StringList(pair));
                    continue;
                }

                var array = new[] {value};
                
                output.Add(new StringList(array));
            }

            return output;
        }
    }
}