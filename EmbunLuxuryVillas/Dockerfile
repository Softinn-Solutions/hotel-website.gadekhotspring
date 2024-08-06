FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# nuget restore
# Install Credential Provider and set env variables to enable Nuget restore with auth
ARG PAT
RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | bash
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS "{\"endpointCredentials\": [{\"endpoint\":\"https://nuget.pkg.github.com/softinn-solutions/index.json\", \"password\":\"${PAT}\"}]}"

# Copy csproj and restore as distinct layers
COPY *.csproj ./

# restore packge with softinn nuget, last parameter expected to be your project csproj
RUN dotnet restore -s "https://nuget.pkg.github.com/softinn-solutions/index.json" -s "https://api.nuget.org/v3/index.json" "EmbunLuxuryVillas.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY /database.db /app/database.db

# override the request to specific port
ENV ASPNETCORE_URLS=http://+:80

# override to specific port
EXPOSE 80

# expected to be your project dll.
ENTRYPOINT ["dotnet", "EmbunLuxuryVillas.dll"] 