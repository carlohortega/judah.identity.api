FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
# EXPOSE 7000:80
# EXPOSE 7001:443

# ENV ASPNETCORE_URLS=http://+:80

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Judah.Identity.Api.csproj", "./"]
RUN dotnet restore "Judah.Identity.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Judah.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Judah.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Judah.Identity.Api.dll"]
