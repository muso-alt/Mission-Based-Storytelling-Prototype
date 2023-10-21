using System.Collections.Generic;

namespace GoogleSheetImporter.Editor
{
    public static class Parser
    {
        public static (List<List<string>>, bool) ParseTable(string csv, bool normalizeNewLines)
        {
            List<List<string>> lines = new List<List<string>>(32);
            List<string> line = new List<string>(8);
            var quotes = 0;
            var start = 0;
            var cols = -1;
            string val;
            char c;
            var len = csv.Length;
            for (var i = 0; i <= len; i++)
            {
                c = i < len ? csv[i] : '\n';
                if (c == '\"')
                {
                    quotes++;
                }

                if ((c == ',' || c == '\n') && (quotes & 1) == 0)
                {
                    if (i > start)
                    {
                        val = csv.Substring(start, i - start).Trim();
                        if (normalizeNewLines)
                        {
                            val = val.Replace("\\n", "\n");
                        }

                        if (quotes > 0)
                        {
                            val = val
                                .Substring(1, val.Length - 2)
                                .Replace("\"\"", "\"");
                        }
                    }
                    else
                    {
                        val = "";
                    }

                    line.Add(val);
                    quotes = 0;
                    start = i + 1;
                    if (c == '\n')
                    {
                        if (cols == -1)
                        {
                            cols = line.Count;
                        }

                        if (cols != line.Count)
                        {
                            return (default, false);
                        }

                        lines.Add(line);
                        if (i < len)
                        {
                            line = new List<string>(cols);
                        }
                    }
                }
            }

            return quotes != 0 ? (default, false) : (lines, true);
        }
    }
}