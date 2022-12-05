namespace application_gateway_lab.Infrastructure.Options;

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
    /// ClientId
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    /// <value>The client secret.</value>
    public string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the audience.
    /// </summary>
    /// <value>The audience.</value>
    public IEnumerable<string> WebApiAudience { get; set; }

    public static AuthOptions CreateInstance(ConfigurationManager configurationSection)
    {
        return configurationSection.GetSection(Auth).Get<AuthOptions>();
    }
}