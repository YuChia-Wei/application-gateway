FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Sample.Api/Sample.WebApi.csproj", "Sample.Api/"]
RUN dotnet restore "Sample.Api/Sample.WebApi.csproj"
COPY . .
WORKDIR "/src/Sample.Api"
RUN dotnet build "Sample.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.WebApi.dll"]
