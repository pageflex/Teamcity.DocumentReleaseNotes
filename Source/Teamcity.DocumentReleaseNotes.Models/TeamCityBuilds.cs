// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Teamcity.DocumentReleaseNotes.Models;
//
//    var data = TeamCityBuilds.FromJson(jsonString);
//
// created using https://quicktype.io/
namespace Teamcity.DocumentReleaseNotes.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TeamCityBuilds
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("build")]
        public List<Build> Build { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class Build
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("buildTypeId")]
        public string BuildTypeId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class TeamCityBuilds
    {
        public static TeamCityBuilds FromJson(string json) => JsonConvert.DeserializeObject<TeamCityBuilds>(json, TeamCityBuildsConverter.Settings);
    }

    public static class TeamCityBuildsSerialize
    {
        public static string ToJson(this TeamCityBuilds self) => JsonConvert.SerializeObject(self, TeamCityBuildsConverter.Settings);
    }

    public class TeamCityBuildsConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
