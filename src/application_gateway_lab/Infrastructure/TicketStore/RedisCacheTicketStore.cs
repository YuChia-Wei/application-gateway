using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace application_gateway_lab.Infrastructure.TicketStore;

/// <summary>
/// Redis Auth Ticket Store
/// ref: https://mikerussellnz.github.io/.NET-Core-Auth-Ticket-Redis/
/// </summary>
public class RedisCacheTicketStore : ITicketStore
{
    private const string KeyPrefix = "application-gateway-lab_Sample:LoginSession:";

    private readonly IDistributedCache _distributedCache;

    public RedisCacheTicketStore(RedisCacheOptions redisCacheOptions)
    {
        this._distributedCache = new RedisCache(redisCacheOptions);
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var guid = Guid.NewGuid();
        var key = KeyPrefix + guid.ToString();
        await this.RenewAsync(key, ticket);
        return key;
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var options = new DistributedCacheEntryOptions();
        var expiresUtc = ticket.Properties.ExpiresUtc;
        if (expiresUtc.HasValue)
        {
            options.SetAbsoluteExpiration(expiresUtc.Value);
        }

        var val = SerializeToBytes(ticket);
        this._distributedCache.Set(key, val, options);
        return Task.FromResult(0);
    }

    public Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        var bytes = this._distributedCache.Get(key);
        var ticket = DeserializeFromBytes(bytes);
        return Task.FromResult(ticket);
    }

    public Task RemoveAsync(string key)
    {
        this._distributedCache.Remove(key);
        return Task.FromResult(0);
    }

    private static AuthenticationTicket? DeserializeFromBytes(byte[]? source)
    {
        return source == null ? null : TicketSerializer.Default.Deserialize(source);
    }

    private static byte[] SerializeToBytes(AuthenticationTicket source)
    {
        return TicketSerializer.Default.Serialize(source);
    }
}