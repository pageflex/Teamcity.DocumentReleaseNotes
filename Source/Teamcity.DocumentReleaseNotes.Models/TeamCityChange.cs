// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Teamcity.DocumentReleaseNotes.Models;
//
//    var data = TeamCityChange.FromJson(jsonString);
//
// created using https://quicktype.io/
namespace Teamcity.DocumentReleaseNotes.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TeamCityChange
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("files")]
        public Files Files { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("vcsRootInstance")]
        public VcsRootInstance VcsRootInstance { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class Files
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("file")]
        public List<File> File { get; set; }
    }

    public partial class File
    {
        [JsonProperty("before-revision")]
        public string BeforeRevision { get; set; }

        [JsonProperty("file")]
        public string OtherFile { get; set; }

        [JsonProperty("after-revision")]
        public string AfterRevision { get; set; }

        [JsonProperty("changeType")]
        public string ChangeType { get; set; }

        [JsonProperty("relative-file")]
        public string RelativeFile { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class TeamCityChange
    {
        public static TeamCityChange FromJson(string json) => JsonConvert.DeserializeObject<TeamCityChange>(json, TeamCityChangeConverter.Settings);
    }

    public static class TeamCityChangeSerialize
    {
        public static string ToJson(this TeamCityChange self) => JsonConvert.SerializeObject(self, TeamCityChangeConverter.Settings);
    }

    public class TeamCityChangeConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}

