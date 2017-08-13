using RandomUtils.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace RandomUtils.Parameterization
{
    internal class StringConstants
    {
        private static Dictionary<int, char> _characterCodeMapping = ReadFile();

        private static Dictionary<int, char> ReadFile()
        {
            Dictionary<int, char> aspects = new Dictionary<int, char>();
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RandomUtils.Parameterization.UnicodeChart.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                List<string> entries = text.Split('\n').ToList();
                entries.Remove(entries.Last());
                entries.RemoveAt(0);
                aspects = new StringConstants().ParseEntry(entries).ToDictionary(entry => entry.Key, entry => entry.Value);
            }
            return aspects;
        }

        private IEnumerable<KeyValuePair<int, char>> ParseEntry(List<string> entries)
        {
            int dataCodeValueStartIndex = 11;
            foreach (string entry in entries)
            {
                if (!string.IsNullOrWhiteSpace(entry))
                {
                    string dataCodeEntry = entry.Substring(entry.IndexOf("data-code"));
                    int dataCodeValueEndIndex = dataCodeEntry.IndexOf('>') - 1;
                    int key = Convert.ToInt32(dataCodeEntry.Substring(dataCodeValueStartIndex, dataCodeValueEndIndex - dataCodeValueStartIndex));
                    string characterEntry = dataCodeEntry.Substring(dataCodeEntry.IndexOf('>') + 1, 1);
                    char character = Convert.ToChar(characterEntry);
                    yield return new KeyValuePair<int, char>(key, character);
                }
            }
        }

        internal Dictionary<int,char> GetMapping(StringType type)
        {
            switch (type)
            {
                case StringType.All:
                    return _characterCodeMapping;
                case StringType.Alphabetic:
                    return FilterMapping("[A-Za-z]");
                case StringType.AlphaNumeric:
                    return FilterMapping("[A-Za-z0-9]");
            }
            return new Dictionary<int, char>();
        }

        private Dictionary<int,char> FilterMapping(string pattern)
        {
            Regex regex = new Regex(pattern);
            return _characterCodeMapping
                .Where(entry => regex.Match(entry.Value.ToString()).Success)
                .ToDictionary(entry => entry.Key, entry=> entry.Value);
        }
    }
}
