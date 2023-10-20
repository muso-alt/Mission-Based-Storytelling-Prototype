using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GoogleSheetImporter.Editor;
using Unfrozen.Tasks;
using UnityEngine;

namespace Unfrozen
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
                
                var info = new MissionInfo
                {
                    Id = currentRow[0],
                    Name = currentRow[1],
                    PreviewText = currentRow[2],
                    MainText = currentRow[3],
                    FriendlySideText = ParseText(currentRow[4], ","),
                    OppositeSideText = ParseText(currentRow[5], ","),
                    CharactersToUnlock = ParseText(currentRow[6], ","),
                    HeroPoints = ParseHeroPoints(currentRow[7]),
                    RequiredMissions = ParseRequiredMissions(currentRow[8]),
                    InactiveMissions = ParseText(currentRow[9], ","),
                    Position = ParsePosition(currentRow[10])
                };
                
                _missionsData.Add(info);
            }

            _missionsData.Reverse();
            return _missionsData;
        }

        public string[] ParseText(string text, string separator)
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
            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrEmpty(values[i])) continue;

                if (values[i].Contains("или"))
                {
                    var pair = ParseText(values[i], "или");
                    output.Add(new StringList(pair));
                    continue;
                }

                var array = new[] {values[i]};
                
                output.Add(new StringList(array));
            }

            return output;
        }
    }
}