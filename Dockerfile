# Pull down an image from Docker Hub that includes the .NET core SDK: 
# https://hub.docker.com/_/microsoft-dotnet-core-sdk
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
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore?tabs=netcore2x
RUN dotnet restore "frankfund/REST/REST.csproj"

# Compile, then pack the compiled app and dependencies into a deployable unit.
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21
RUN dotnet publish "frankfund/REST/REST.csproj" -c Release -o /app/publish

# Pull down an image from Docker Hub that includes only the ASP.NET core runtime:
# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# We don't need the SDK anymore, so this will produce a lighter-weight image
# that can still run the app.
FROM gcr.io/google-appengine/aspnetcore:3.1

# Expose port 80 to your local machine so you can access the app.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Copy the published app to this new runtime-only container.
COPY --from=build /app/publish .

# To run the app, run `dotnet sample-app.dll`, which we just copied over.
ENTRYPOINT ["dotnet", "REST.dll"]