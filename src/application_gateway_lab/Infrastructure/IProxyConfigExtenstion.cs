using Yarp.ReverseProxy.Configuration;

namespace application_gateway_lab.Infrastructure;

public static class IProxyConfigExtenstion
{
    public static GatewayConfig ToGatewayConfig(this IProxyConfig proxyConfig)
    {
        return new GatewayConfig(proxyConfig);
    }
}