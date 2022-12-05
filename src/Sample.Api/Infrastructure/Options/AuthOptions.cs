namespace Sample.WebApi.Infrastructure.Options;

/// <summary>
/// OAuth 設定物件
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Config 常數
    /// </summary>
    public const string Auth = "Auth";

    /// <summary>
    /// Authority Url
    /// </summary>
    public string Authority { get; set; }

    /// <summary>
    /// Gets or sets the audience.
    /// </summary>
    /// <value>The audience.</value>
    public string Audience { get; set; }

    public static AuthOptions CreateInstance(ConfigurationManager configurationSection)
    {
        return configurationSection.GetSection(Auth).Get<AuthOptions>();
    }
}