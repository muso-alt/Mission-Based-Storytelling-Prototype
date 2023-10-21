using System;
using System.Text.RegularExpressions;

using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace GoogleSheetImporter.Editor
{
    public class GSImporter
    {
        private readonly Regex _regex;

        public GSImporter()
        {
            _regex = new Regex(@"https://docs\.google\.com/spreadsheets/d/(.+)/");
        }

        public async UniTask<(string sheetData, string error)> DownloadAsync(string sheetUrl)
        {
            var match = _regex.Match(sheetUrl);
            var key = match.Groups[1];
            var stringFormat = GetStringFormat(FileFormat.csv);

            var downloadUrl = $"https://docs.google.com/spreadsheets/export?id={key}&exportFormat={stringFormat}";

            using (var client = UnityWebRequest.Get(downloadUrl))
            {
                try
                {
                    await client.SendWebRequest();
                    if (client.error != null)
                        return (null, client.error);

                    var data = client.downloadHandler.text;
                    return (data, default);
                }
                catch (Exception e)
                {
                    return (default, e.Message);
                }
            }
        }

        private string GetStringFormat(FileFormat format)
        {
            return format switch
            {
                FileFormat.csv => "csv",
                FileFormat.tsv => "tsv",
                FileFormat.ods => "ods",
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }
    }
}