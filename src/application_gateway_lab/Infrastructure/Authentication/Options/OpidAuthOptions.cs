using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace application_gateway_lab.Infrastructure.Authentication.Options;

public class OpidAuthOptions
{
    /// <summary>
    /// Authority Url
    /// </summary>
    public required string Authority { get; init; }

    /// <summary>
    /// ClientId
    /// </summary>
    public required string ClientId { get; init; }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    /// <value>The client secret.</value>
    public required string ClientSecret { get; init; }

    /// <summary>
    /// Gets or sets the audience.
    /// </summary>
    /// <value>The audience.</value>
    public required IEnumerable<string> WebApiAudience { get; init; }

    /// <summary>
    /// Cookies 的名稱
    /// </summary>
    public required string LoginCookieName { get; init; }

    /// <summary>
    /// Cookies 所屬網域
    /// </summary>
    public required string LoginCookieDomain { get; init; }

    /// <summary>
    /// Cookie Secure Policy (default is non)
    /// </summary>
    public CookieSecurePolicy CookieSecurePolicy { get; init; } = CookieSecurePolicy.None;

    /// <summary>
    /// Cookie Same Site Mode (default is Lex)
    /// </summary>
    public SameSiteMode CookieSameSiteMode { get; init; } = SameSiteMode.Lax;

    /// <summary>
    /// ticket store redis server url
    /// </summary>
    public required string TicketStoreRedisServer { get; init; }

    /// <summary>
    /// 登入的服務名稱
    /// </summary>
    public required string LoginApplicationName { get; init; }

    /// <summary>
    /// 是否有設定
    /// </summary>
    public bool IsSettled { get; init; } = true;

    public bool RequireHttpsMetadata { get; init; } = true;

    /// <summary>
    /// opid response type
    /// </summary>
    /// <remarks>code id_token</remarks>
    public string ResponseType { get; init; } = OpenIdConnectResponseType.CodeIdToken;

    public string? RefreshTokenAddress { get; init; }

    internal static OpidAuthOptions? CreateInstance(ConfigurationManager configuration)
    {
        return configuration.GetSection("Auth").Get<OpidAuthOptions>();
    }
}