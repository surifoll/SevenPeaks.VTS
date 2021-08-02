#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SevenPeaks.VTS.Web/SevenPeaks.VTS.Web.csproj", "SevenPeaks.VTS.Web/"]
COPY ["SevenPeaks.VTS.Common/SevenPeaks.VTS.Common.csproj", "SevenPeaks.VTS.Common/"]
COPY ["SevenPeaks.VTS.Domain/SevenPeaks.VTS.Domain.csproj", "SevenPeaks.VTS.Domain/"]
COPY ["SevenPeaks.VTS.Infrastructure/SevenPeaks.VTS.Infrastructure.csproj", "SevenPeaks.VTS.Infrastructure/"]
COPY ["SevenPeaks.VTS.Persistence/SevenPeaks.VTS.Persistence.csproj", "SevenPeaks.VTS.Persistence/"]
COPY ["SevenPeaks.VTS.Application/SevenPeaks.VTS.Application.csproj", "SevenPeaks.VTS.Application/"]
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
