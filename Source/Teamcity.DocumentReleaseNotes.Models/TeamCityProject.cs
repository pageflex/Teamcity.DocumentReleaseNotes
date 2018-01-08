// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Teamcity.DocumentReleaseNotes.Models;
//
//    var data = TeamCityProject.FromJson(jsonString);
//
// created using https://quicktype.io/
namespace Teamcity.DocumentReleaseNotes.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class TeamCityProject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("parentProjectId")]
        public string ParentProjectId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("buildTypes")]
        public BuildTypes BuildTypes { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parentProject")]
        public ParentProject ParentProject { get; set; }

        [JsonProperty("projects")]
        public Projects Projects { get; set; }

        [JsonProperty("vcsRoots")]
        public ProjectFeatures VcsRoots { get; set; }

        [JsonProperty("projectFeatures")]
        public ProjectFeatures ProjectFeatures { get; set; }

        [JsonProperty("templates")]
        public BuildTypes Templates { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class BuildTypes
    {
        [JsonProperty("buildType")]
        public List<BuildType> BuildType { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public partial class BuildType
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("templateFlag")]
        public bool? TemplateFlag { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class Parameters
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("property")]
        public List<Property> Property { get; set; }
    }

    public partial class Property
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inherited")]
        public bool Inherited { get; set; }

        [JsonProperty("type")]
        public OtherType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class OtherType
    {
        [JsonProperty("rawValue")]
        public string RawValue { get; set; }
    }

    public partial class ParentProject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("parentProjectId")]
        public string ParentProjectId { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }

    public partial class Projects
    {
        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public partial class ProjectFeatures
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class TeamCityProject
    {
        public static TeamCityProject FromJson(string json) => JsonConvert.DeserializeObject<TeamCityProject>(json, TeamCityProjectConverter.Settings);
    }

    public static class TeamCityProjectSerialize
    {
        public static string ToJson(this TeamCityProject self) => JsonConvert.SerializeObject(self, TeamCityProjectConverter.Settings);
    }

    public class TeamCityProjectConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
