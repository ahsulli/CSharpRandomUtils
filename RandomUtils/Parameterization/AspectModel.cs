﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using RandomUtils.Enums;
using Newtonsoft.Json.Converters;

namespace RandomUtils.Parameterization
{
    /// <summary>
    /// Encapsulates the collection of aspects used to read the constraints from the specified resource file.
    /// </summary>
    [Serializable]
    [JsonObject(Id = "Aspects")]
    [XmlRoot("Aspects")]
    public class AspectModelCollection
    {
        /// <summary>
        /// The collection of aspects read from the specified resource file.
        /// </summary>
        [JsonProperty(PropertyName = "Aspect")]
        [XmlElement("Aspect")]
        public List<AspectModel> Models { get; set; }

        /// <summary>
        /// Creates the default setup of class instances prior to the deserialization.
        /// </summary>
        public AspectModelCollection()
        {
            Models = new List<AspectModel>();
        }
    }

    /// <summary>
    /// The object used to describe the constraints of the property whose value will be randomly generated.
    /// </summary>
    public class AspectModel
    {
        /// <summary>
        /// The name of the property whose value will be randomly generated according to the associated constraints.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The name of the class that the specified property name belongs to. 
        /// This is helpful if a property name is used in multiple classes that are being randomly generated by the same call, 
        /// and you need to be more specific with the constraints of the generated value.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ClassName { get; set; }

        /// <summary>
        /// Tells the random generator if the property requires a random value. If the property is optional, 
        /// then the random generator randomly selects whether to generate a random value for the property.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Required { get; set; }

        /// <summary>
        /// Sets the earliest DateTime value for the DateTime type property.
        /// For XML use yyyy-mm-ddThh:mm:ssZ format.
        /// For all others use mm/dd/yyyy hh:mm:ss format.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EarliestDateTime { get; set; }

        /// <summary>
        /// Sets the latest DateTime value for the DateTime type property.
        /// For XML use yyyy-mm-ddThh:mm:ssZ format.
        /// For all others use mm/dd/yyyy hh:mm:ss format.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LatestDateTime { get; set; }

        /// <summary>
        /// Sets the minimum integer value or string length for the property.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Minimum { get; set; }

        /// <summary>
        /// Sets the maximum integer value or string length for the property.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Maximum { get; set; }

        /// <summary>
        /// The maximum number of items that can belong in the collection.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MaximumLength { get; set; }

        /// <summary>
        /// The minimum number of items that can belong in the collection.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MinimumLength { get; set; }

        /// <summary>
        /// Common constraints used when generating a random string value
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public StringType StringType { get; set; }

        /// <summary>
        /// The regex pattern that the randomly generated string must match for strings with a StringType of Regex.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RegexPattern { get; set; }

        /// <summary>
        /// Creates the default setup of class instances prior to the deserialization.
        /// </summary>
        public AspectModel()
        {
            PropertyName = string.Empty;
            Required = false;
            Minimum = DefaultValues.DEFAULTMINIMUM;
            Maximum = DefaultValues.DEFAULTMAXIMUM;
            MinimumLength = DefaultValues.DEFAULTMINIMUMLENGTH;
            MaximumLength = DefaultValues.DEFAULTMAXIMUMLENGTH;
            ClassName = string.Empty;
            StringType = DefaultValues.DEFAULTSTRINGTYPE;
        }
    }
}