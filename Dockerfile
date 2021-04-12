# Pull down an image from Docker Hub that includes the .NET core SDK: 
# This is so we have all the tools necessary to compile the app.
FROM gcr.io/google-appengine/aspnetcore:3.1 AS build

# Fetch and install Node 10. Make sure to include the --yes parameter 
# to automatically accept prompts during install, or it'll fail.
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

# Copy the source from your machine onto the container.
WORKDIR /src
COPY . .

# Install dependencies. 
RUN dotnet restore "frankfund/WebServer/WebServer.csproj"

# Compile, then pack the compiled app and dependencies into a deployable unit.
RUN dotnet publish "frankfund/WebServer/WebServer.csproj" -c Release -o /app/publish

# Pull down an image from GCP that includes only the ASP.NET core runtime:
FROM gcr.io/google-appengine/aspnetcore:3.1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Copy the published app to this new runtime-only container.
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebServer.dll"]