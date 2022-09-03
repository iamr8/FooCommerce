#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FooCommerce.NotificationAPI.Worker/FooCommerce.NotificationAPI.Worker.csproj", "FooCommerce.NotificationAPI.Worker/"]
COPY ["FooCommerce.NotificationAPI/FooCommerce.NotificationAPI.csproj", "FooCommerce.NotificationAPI/"]
COPY ["FooCommerce.Core/FooCommerce.Core.csproj", "FooCommerce.Core/"]
COPY ["FooCommerce.Domain/FooCommerce.Domain.csproj", "FooCommerce.Domain/"]
RUN dotnet restore "FooCommerce.NotificationAPI.Worker/FooCommerce.NotificationAPI.Worker.csproj"
COPY . .
WORKDIR "/src/FooCommerce.NotificationAPI.Worker"
RUN dotnet build "FooCommerce.NotificationAPI.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooCommerce.NotificationAPI.Worker.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false
WORKDIR /app/publish
RUN mkdir logs
RUN mkdir bin
RUN mkdir Keys
RUN chmod 775 bin
RUN chmod 775 logs
RUN chmod 775 Keys

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FooCommerce.NotificationAPI.Worker.dll"]