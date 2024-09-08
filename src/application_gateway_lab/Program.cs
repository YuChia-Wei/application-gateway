using application_gateway_lab.Infrastructure;
using application_gateway_lab.Infrastructure.Authentication;
using application_gateway_lab.Infrastructure.Authentication.Options;
using application_gateway_lab.Infrastructure.YarpComponents.TransformProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddJsonFile("ReverseProxy-ClustersSetting.json", true, true)
       .AddJsonFile("ReverseProxy-RoutesSetting.json", true, true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddHealthChecks();

builder.Services.AddW3CLogging(logging =>
{
    // Log all W3C fields
    logging.LoggingFields = W3CLoggingFields.All;

    logging.FileSizeLimit = 5 * 1024 * 1024;
    logging.RetainedFileCountLimit = 2;
    logging.FileName = AppDomain.CurrentDomain.FriendlyName + Environment.MachineName;
    logging.FlushInterval = TimeSpan.FromSeconds(2);

    //.net 7 new feature
    logging.AdditionalRequestHeaders.Add("x-forwarded-for");
});

//OAuth
var opidAuthOptions = OpidAuthOptions.CreateInstance(builder.Configuration);

builder.Services.AddAuthentication(options =>
       {
           options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
       })
       .AddOpenIdConnectWithCookie(opidAuthOptions);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GatewayManager", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

// Add the reverse proxy capability to the server
builder.Services
       .AddReverseProxy()
       .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
       .AddTransforms<AuthenticationTokenTransformProvider>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto |
                               ForwardedHeaders.XForwardedHost;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost
});

app.UseHealthChecks("/health");

// 攔截 favicon.ico 的路徑，因為 Proxy 的 Route 設定中尚未發現排除特定路徑的設定方式
app.Map("/favicon.ico", () => "");

app.UseW3CLogging();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/gateway-config",
           [Authorize("GatewayManager")]([FromServices] IProxyConfigProvider proxyConfig) =>
           proxyConfig.GetConfig().ToGatewayConfig());

app.MapReverseProxy();

app.Run();