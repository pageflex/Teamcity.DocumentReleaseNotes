// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Teamcity.DocumentReleaseNotes.Models;
//
//    var data = TeamCityChanges.FromJson(jsonString);
//
// created using https://quicktype.io/
namespace Teamcity.DocumentReleaseNotes.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TeamCityChanges
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("change")]
        public List<Change> Change { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class TeamCityChanges
    {
        public static TeamCityChanges FromJson(string json) => JsonConvert.DeserializeObject<TeamCityChanges>(json, TeamCityChangesConverter.Settings);
    }

    public static class TeamCityChangesSerialize
    {
        public static string ToJson(this TeamCityChanges self) => JsonConvert.SerializeObject(self, TeamCityChangesConverter.Settings);
    }

    public class TeamCityChangesConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
