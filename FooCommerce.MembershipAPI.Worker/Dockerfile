#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FooCommerce.MembershipAPI.Worker/FooCommerce.MembershipAPI.Worker.csproj", "FooCommerce.MembershipAPI.Worker/"]
COPY ["FooCommerce.MembershipAPI/FooCommerce.MembershipAPI.csproj", "FooCommerce.MembershipAPI/"]
COPY ["FooCommerce.Core/FooCommerce.Core.csproj", "FooCommerce.Core/"]
COPY ["FooCommerce.Application/FooCommerce.Application.csproj", "FooCommerce.Application/"]
COPY ["FooCommerce.Domain/FooCommerce.Domain.csproj", "FooCommerce.Domain/"]
RUN dotnet restore "FooCommerce.MembershipAPI.Worker/FooCommerce.MembershipAPI.Worker.csproj"
COPY . .
WORKDIR "/src/FooCommerce.MembershipAPI.Worker"
RUN dotnet build "FooCommerce.MembershipAPI.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooCommerce.MembershipAPI.Worker.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false
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
ENTRYPOINT ["dotnet", "FooCommerce.MembershipAPI.Worker.dll"]