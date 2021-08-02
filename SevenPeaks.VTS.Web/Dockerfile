FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SevenPeaks.VTS.Web/SevenPeaks.VTS.Web.csproj", "SevenPeaks.VTS.Web/"]
RUN dotnet restore "SevenPeaks.VTS.Web/SevenPeaks.VTS.Web.csproj"
COPY . .
WORKDIR "/src/SevenPeaks.VTS.Web"
RUN dotnet build "SevenPeaks.VTS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SevenPeaks.VTS.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SevenPeaks.VTS.Web.dll"]
