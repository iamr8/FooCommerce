#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["FooCommerce.Domain/FooCommerce.Domain.csproj", "FooCommerce.Domain/"]
COPY ["FooCommerce.Application/FooCommerce.Application.csproj", "FooCommerce.Application/"]
COPY ["FooCommerce.Core/FooCommerce.Core.csproj", "FooCommerce.Core/"]
COPY ["FooCommerce.Infrastructure/FooCommerce.Infrastructure.csproj", "FooCommerce.Infrastructure/"]
COPY ["FooCommerce.NotificationAPI.Bridge/FooCommerce.NotificationAPI.Bridge.csproj", "FooCommerce.NotificationAPI.Bridge/"]
COPY ["FooCommerce.Web/FooCommerce.Web.csproj", "FooCommerce.Web/"]
RUN dotnet restore "FooCommerce.Web/FooCommerce.Web.csproj"
COPY . .
WORKDIR "/src/FooCommerce.Web"
RUN dotnet build "FooCommerce.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooCommerce.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false
WORKDIR /app/publish
RUN mkdir logs
RUN mkdir bin
RUN chmod 775 bin
RUN chmod 775 logs
# RUN chmod 775 wwwroot/uploads

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FooCommerce.Web.dll"]