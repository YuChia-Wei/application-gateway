FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#if need downgrade to tls1.0
#RUN sed -i 's/openssl_conf = openssl_init/#openssl_conf = openssl_init/' /etc/ssl/openssl.cnf
#RUN sed -i '1i openssl_conf = default_conf' /etc/ssl/openssl.cnf && echo -e "\n[ default_conf ]\nssl_conf = ssl_sect\n[ssl_sect]\nsystem_default = system_default_sect\n[system_default_sect]\nMinProtocol = TLSv1\nCipherString = DEFAULT:@SECLEVEL=1" >> /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
#if need downgrade to tls1.0
#RUN sed -i 's/openssl_conf = openssl_init/#openssl_conf = openssl_init/' /etc/ssl/openssl.cnf
#RUN sed -i '1i openssl_conf = default_conf' /etc/ssl/openssl.cnf && echo -e "\n[ default_conf ]\nssl_conf = ssl_sect\n[ssl_sect]\nsystem_default = system_default_sect\n[system_default_sect]\nMinProtocol = TLSv1\nCipherString = DEFAULT:@SECLEVEL=1" >> /etc/ssl/openssl.cnf
ARG BuildConfiguration=Debug
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["application-gateway-lab/application-gateway-lab.csproj", "application-gateway-lab/"]
RUN dotnet restore "application-gateway-lab/application-gateway-lab.csproj"
COPY . .
WORKDIR "/src/application-gateway-lab"
RUN dotnet build "application-gateway-lab.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration=Debug
RUN dotnet publish "application-gateway-lab.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "application-gateway-lab.dll"]
