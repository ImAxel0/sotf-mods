using Alt.Json;

namespace UpdatesChecker;

public class Mod
{
    [JsonProperty("mod_id")]
    public string ModId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; }

    [JsonProperty("isNSFW")]
    public bool IsNSFW { get; set; }

    [JsonProperty("isApproved")]
    public bool IsApproved { get; set; }

    [JsonProperty("isFeatured")]
    public bool IsFeatured { get; set; }

    [JsonProperty("category_slug")]
    public string CategorySlug { get; set; }

    [JsonProperty("category_name")]
    public string CategoryName { get; set; }

    [JsonProperty("user_name")]
    public string UserName { get; set; }

    [JsonProperty("user_slug")]
    public string UserSlug { get; set; }

    [JsonProperty("user_image_url")]
    public string UserImageUrl { get; set; }

    [JsonProperty("thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    [JsonProperty("primary_image_url")]
    public string PrimaryImageUrl { get; set; }

    [JsonProperty("dependencies")]
    public List<string> Dependencies { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("latest_version")]
    public string LatestVersion { get; set; }

    [JsonProperty("downloads")]
    public int Downloads { get; set; }

    [JsonProperty("favorites")]
    public int Favorites { get; set; }

    [JsonProperty("total_downloads")]
    public int TotalDownloads { get; set; }

    [JsonProperty("last_week_downloads")]
    public int LastWeekDownloads { get; set; }

    [JsonProperty("time_ago")]
    public string TimeAgo { get; set; }
}

public class Meta
{
    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("pages")]
    public int Pages { get; set; }

    [JsonProperty("next_page")]
    public int NextPage { get; set; }

    [JsonProperty("prev_page")]
    public int PrevPage { get; set; }
}

public class Root
{
    [JsonProperty("mods")]
    public List<Mod> Mods { get; set; }

    [JsonProperty("meta")]
    public Meta Meta { get; set; }
}
