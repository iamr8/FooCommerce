#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:latest AS node_base

RUN echo "NODE Version:" && node --version
RUN echo "NPM Version:" && npm --version

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
COPY --from=node_base . .
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY --from=node_base . .
WORKDIR /src
COPY ["FooCommerce.Web/FooCommerce.Web.csproj", "FooCommerce.Web/"]
COPY ["FooCommerce.Infrastructure/FooCommerce.Infrastructure.csproj", "FooCommerce.Infrastructure/"]
COPY ["FooCommerce.Core/FooCommerce.Core.csproj", "FooCommerce.Core/"]
COPY ["FooCommerce.Domain/FooCommerce.Domain.csproj", "FooCommerce.Domain/"]
COPY ["FooCommerce.IdentityAPI/FooCommerce.IdentityAPI.csproj", "FooCommerce.IdentityAPI/"]
COPY ["FooCommerce.NotificationAPI/FooCommerce.NotificationAPI.csproj", "FooCommerce.NotificationAPI/"]
RUN dotnet restore "FooCommerce.Web/FooCommerce.Web.csproj"
COPY . .
WORKDIR "/src/FooCommerce.Web"
RUN dotnet build "FooCommerce.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooCommerce.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false
WORKDIR /app/publish
RUN mkdir logs
RUN mkdir bin
RUN mkdir Keys
RUN chmod 775 bin
RUN chmod 775 logs
RUN chmod 775 Keys
RUN chmod 775 wwwroot
RUN chmod 775 ClientApp

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FooCommerce.Web.dll"]