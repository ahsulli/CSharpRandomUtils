using RandomUtils.Enums;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RandomUtils.Parameterization
{
    internal class Parser
    {
        private Assembly _callingAssembly { get; set; }
        private string _resourceName { get; set; }

        internal Parser(Assembly assembly, string resourceName)
        {
            _callingAssembly = assembly;
            _resourceName = resourceName;
        }

        internal List<AspectModel> ParseAspects()
        {
            int fileTypeDelimiterIndex = _resourceName.LastIndexOf('.');
            string fileTypeText = _resourceName.Substring(fileTypeDelimiterIndex + 1).Trim().ToUpper();
            DataFileType fileType = (DataFileType)Enum.Parse(typeof(DataFileType), fileTypeText);
            switch (fileType)
            {
                case DataFileType.CSV:
                    return ParseCSV();
                case DataFileType.JSON:
                    return ParseJSON();
                case DataFileType.XML:
                    return ParseXML();
                case DataFileType.YAML:
                    return ParseYAML();
                default:
                    throw new ArgumentException("The specified data source type is not currently supported.");
            }
        }

        private List<AspectModel> ParseCSV()
        {
            List<AspectModel> aspects = new List<AspectModel>();
            using (Stream stream = _callingAssembly.GetManifestResourceStream(_resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                //CSV Parsing logic here
                string text = reader.ReadToEnd();
                List<string> entries = text.Split('\n').ToList();
                entries.RemoveAt(0);
                aspects = ParseCSVEntries(entries).ToList();
            }

            return aspects;
        }

        private IEnumerable<AspectModel> ParseCSVEntries(List<string> entries)
        {
            foreach (string entry in entries)
            {
                List<string> values = entry.Split(';').ToList();
                AspectModel currentAspect = new AspectModel();
                currentAspect.PropertyName = values[0];
                currentAspect.ClassName = values[1];
                if (!string.IsNullOrEmpty(values[2]))
                {
                    currentAspect.Required = Convert.ToBoolean(values[2]);
                }
                if (!string.IsNullOrEmpty(values[3]))
                {
                    currentAspect.Minimum = Convert.ToInt32(values[3]);
                }
                if (!string.IsNullOrEmpty(values[4]))
                {
                    currentAspect.Maximum = Convert.ToInt32(values[4]);
                }
                if (!string.IsNullOrEmpty(values[5]))
                {
                    currentAspect.MinimumLength = Convert.ToInt32(values[5]);
                }
                if (!string.IsNullOrEmpty(values[6]))
                {
                    currentAspect.MaximumLength = Convert.ToInt32(values[6]);
                }
                if (!string.IsNullOrEmpty(values[7]))
                {
                    currentAspect.EarliestDateTime = DateTime.Parse(values[7], null, DateTimeStyles.AllowWhiteSpaces);
                }
                if (!string.IsNullOrEmpty(values[8]))
                {
                    currentAspect.LatestDateTime = DateTime.Parse(values[8], null, DateTimeStyles.AllowWhiteSpaces);
                }
                if (!string.IsNullOrEmpty(values[9]))
                {
                    currentAspect.StringType = (StringType)Enum.Parse(typeof(StringType), values[9]);
                }
                string maxLengthText = values[10];
                if (maxLengthText.IndexOf('\r') != -1)
                {
                    maxLengthText = values[10].Substring(0, values[10].IndexOf('\r'));
                }
                if (!string.IsNullOrEmpty(maxLengthText))
                {
                    currentAspect.RegexPattern = maxLengthText;
                }
                yield return currentAspect;
            }
        }

        private List<AspectModel> ParseJSON()
        {
            List<AspectModel> aspects = new List<AspectModel>();
            using (Stream stream = _callingAssembly.GetManifestResourceStream(_resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                AspectModelCollection collection = JsonConvert.DeserializeObject<AspectModelCollection>(reader.ReadToEnd());
                aspects = collection.Models;
            }
            if (aspects.Count == 0)
            {
                throw new InvalidOperationException("The JSON contents could not be properly loaded. No aspects could be read and loaded.");
            }
            return aspects;
        }

        private List<AspectModel> ParseXML()
        {
            List<AspectModel> aspects = new List<AspectModel>();
            using (Stream stream = _callingAssembly.GetManifestResourceStream(_resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(AspectModelCollection));
                aspects = ((AspectModelCollection)deserializer.Deserialize(reader)).Models;
            }
            if (aspects.Count == 0)
            {
                throw new InvalidOperationException("The XML contents could not be properly loaded. No aspects could be read and loaded.");
            }
            return aspects;
        }

        private List<AspectModel> ParseYAML()
        {
            List<AspectModel> aspects = new List<AspectModel>();
            using (Stream stream = _callingAssembly.GetManifestResourceStream(_resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                text = text.Substring(text.IndexOf(':') + 1);
                List<string> entries = Regex.Split(text, "\n-").ToList();
                aspects = ParseYAMLEntries(entries).ToList();
            }
            return aspects;
        }

        private IEnumerable<AspectModel> ParseYAMLEntries(List<string> entries)
        {
            foreach (string entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Trim()))
                {
                    AspectModel currentAspect = new AspectModel();
                    string aspect = entry.Trim();
                    var aspectProperties = ParseYAMLPairs(aspect);
                    foreach (var aspectProperty in aspectProperties)
                    {
                        PropertyInfo property = currentAspect.GetType().GetProperty(aspectProperty.Key);
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(currentAspect, aspectProperty.Value);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(currentAspect, DateTime.Parse(aspectProperty.Value, null, DateTimeStyles.AllowWhiteSpaces));
                        }
                        else if (property.PropertyType == typeof(StringType))
                        {
                            property.SetValue(currentAspect, (StringType)Enum.Parse(typeof(StringType), aspectProperty.Value));
                        }
                        else
                        {
                            dynamic convertedValue = DynamicConvert(aspectProperty.Value, property.PropertyType);
                            property.SetValue(currentAspect, convertedValue);
                        }
                    }
                    yield return currentAspect;
                }
            }
        }

        private IEnumerable<KeyValuePair<string, string>> ParseYAMLPairs(string entry)
        {
            List<string> aspectProperties = entry.Split('\r').ToList();
            foreach (string aspectProperty in aspectProperties)
            {
                string key = aspectProperty.Substring(0, aspectProperty.IndexOf(':')).Trim();
                string value = aspectProperty.Substring(aspectProperty.IndexOf(':') + 1).Trim();
                yield return new KeyValuePair<string, string>(key, value);
            }
        }

        private dynamic DynamicConvert(string value, Type type)
        {
            if (type == typeof(bool))
            {
                return (bool)Convert.ToBoolean(value);
            }
            else if (type == typeof(int))
            {
                return (int)Convert.ToInt32(value);
            }
            else
            {
                return value;
            }
        }
    }
}
