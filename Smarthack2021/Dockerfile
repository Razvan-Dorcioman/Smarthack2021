FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Smarthack2021/Smarthack2021.csproj", "Smarthack2021/"]
RUN dotnet restore "Smarthack2021/Smarthack2021.csproj"
COPY . .
WORKDIR "/src/Smarthack2021"
RUN dotnet build "Smarthack2021.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Smarthack2021.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Smarthack2021.dll"]
