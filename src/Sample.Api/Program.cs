using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Sample.WebApi.Infrastructure.Options;
using Sample.WebApi.ViewModels;

var builder = WebApplication.CreateBuilder(args);

var authOptions = AuthOptions.CreateInstance(builder.Configuration);

// 開啟 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//OAuth

//join this, if you need to use 'name' to get user name in jwt
// JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add(JwtRegisteredClaimNames.Name, ClaimTypes.Name);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
       {
           options.Authority = authOptions.Authority;
           options.RequireHttpsMetadata = false;
           options.Audience = authOptions.Audience;
       });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/api/v1/sample",
           [Authorize]() => new EmployeeViewModel {EmpId = "sample", EmpName = "sample name " + DateTime.Now});

app.Run();