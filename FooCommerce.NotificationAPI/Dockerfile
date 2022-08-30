#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FooCommerce.Domain/FooCommerce.Domain.csproj", "FooCommerce.Domain/"]
COPY ["FooCommerce.Application/FooCommerce.Application.csproj", "FooCommerce.Application/"]
COPY ["FooCommerce.Core/FooCommerce.Core.csproj", "FooCommerce.Core/"]
COPY ["FooCommerce.NotificationAPI.Bridge/FooCommerce.NotificationAPI.Bridge.csproj", "FooCommerce.NotificationAPI.Bridge/"]
COPY ["FooCommerce.NotificationAPI/FooCommerce.NotificationAPI.csproj", "FooCommerce.NotificationAPI/"]
RUN dotnet restore "FooCommerce.NotificationAPI/FooCommerce.NotificationAPI.csproj"
COPY . .
WORKDIR "/src/FooCommerce.NotificationAPI"
RUN dotnet build "FooCommerce.NotificationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooCommerce.NotificationAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false
WORKDIR /app/publish
RUN mkdir logs
RUN mkdir bin
RUN chmod 775 bin
RUN chmod 775 logs
# RUN chmod 775 wwwroot/uploads

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FooCommerce.NotificationAPI.dll"]