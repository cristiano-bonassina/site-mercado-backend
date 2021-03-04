FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["LogicArt.SiteMercado.Hosting/LogicArt.SiteMercado.Hosting.csproj", "LogicArt.SiteMercado.Hosting/"]
COPY ["LogicArt.SiteMercado.Presentation/LogicArt.SiteMercado.Presentation.csproj", "LogicArt.SiteMercado.Presentation/"]
COPY ["LogicArt.SiteMercado.Infrastructure.Persistence/LogicArt.SiteMercado.Infrastructure.Persistence.csproj", "LogicArt.SiteMercado.Infrastructure.Persistence/"]
COPY ["LogicArt.SiteMercado.Domain.Entities/LogicArt.SiteMercado.Domain.Entities.csproj", "LogicArt.SiteMercado.Domain.Entities/"]
COPY ["LogicArt.SiteMercado.Application.Repositories.Abstractions/LogicArt.SiteMercado.Application.Repositories.Abstractions.csproj", "LogicArt.SiteMercado.Application.Repositories.Abstractions/"]
COPY ["LogicArt.SiteMercado.Application.Data/LogicArt.SiteMercado.Application.Data.csproj", "LogicArt.SiteMercado.Application.Data/"]
COPY ["LogicArt.SiteMercado.Application.Assemblers/LogicArt.SiteMercado.Application.Adapters.csproj", "LogicArt.SiteMercado.Application.Assemblers/"]
COPY ["LogicArt.SiteMercado.Application.Adapters.Abstractions/LogicArt.SiteMercado.Application.Adapters.Abstractions.csproj", "LogicArt.SiteMercado.Application.Adapters.Abstractions/"]
COPY ["LogicArt.SiteMercado.Application.Services.Abstractions/LogicArt.SiteMercado.Application.Services.Abstractions.csproj", "LogicArt.SiteMercado.Application.Services.Abstractions/"]
COPY ["LogicArt.SiteMercado.Application.Services/LogicArt.SiteMercado.Application.Services.csproj", "LogicArt.SiteMercado.Application.Services/"]
COPY ["LogicArt.SiteMercado.Application.Events/LogicArt.SiteMercado.Application.Events.csproj", "LogicArt.SiteMercado.Application.Events/"]
COPY NuGet.config ./
RUN dotnet restore --configfile NuGet.config "LogicArt.SiteMercado.Hosting/LogicArt.SiteMercado.Hosting.csproj"
COPY . .
WORKDIR "/src/LogicArt.SiteMercado.Hosting"
RUN dotnet build "LogicArt.SiteMercado.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogicArt.SiteMercado.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "LogicArt.SiteMercado.Hosting.dll"]
