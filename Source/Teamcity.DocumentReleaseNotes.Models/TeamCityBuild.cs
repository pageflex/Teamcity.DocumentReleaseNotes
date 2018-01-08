// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Teamcity.DocumentReleaseNotes.Models;
//
//    var data = TeamCityBuild.FromJson(jsonString);
//
// created using https://quicktype.io/
namespace Teamcity.DocumentReleaseNotes.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TeamCityBuild
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("buildTypeId")]
        public string BuildTypeId { get; set; }

        [JsonProperty("artifacts")]
        public Artifacts Artifacts { get; set; }

        [JsonProperty("agent")]
        public Agent Agent { get; set; }

        [JsonProperty("buildType")]
        public BuildType BuildType { get; set; }

        [JsonProperty("finishDate")]
        public string FinishDate { get; set; }

        [JsonProperty("changes")]
        public Artifacts Changes { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("queuedDate")]
        public string QueuedDate { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("lastChanges")]
        public LastChanges LastChanges { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("revisions")]
        public Revisions Revisions { get; set; }

        [JsonProperty("relatedIssues")]
        public Artifacts RelatedIssues { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("triggered")]
        public Triggered Triggered { get; set; }

        [JsonProperty("statistics")]
        public Artifacts Statistics { get; set; }

        [JsonProperty("statusText")]
        public string StatusText { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class Artifacts
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class Agent
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("typeId")]
        public long? TypeId { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class LastChanges
    {
        [JsonProperty("change")]
        public List<Change> Change { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public partial class Change
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("property")]
        public List<Property> Property { get; set; }
    }

    public partial class Revisions
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("revision")]
        public List<Revision> Revision { get; set; }
    }

    public partial class Revision
    {
        [JsonProperty("vcsBranchName")]
        public string VcsBranchName { get; set; }

        [JsonProperty("vcs-root-instance")]
        public VcsRootInstance VcsRootInstance { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class VcsRootInstance
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vcs-root-id")]
        public string VcsRootId { get; set; }
    }

    public partial class Triggered
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("user")]
        public Agent User { get; set; }
    }

    public partial class TeamCityBuild
    {
        public static TeamCityBuild FromJson(string json) => JsonConvert.DeserializeObject<TeamCityBuild>(json, TeamCityBuildConverter.Settings);
    }

    public static class TeamCityBuildSerialize
    {
        public static string ToJson(this TeamCityBuild self) => JsonConvert.SerializeObject(self, TeamCityBuildConverter.Settings);
    }

    public class TeamCityBuildConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
