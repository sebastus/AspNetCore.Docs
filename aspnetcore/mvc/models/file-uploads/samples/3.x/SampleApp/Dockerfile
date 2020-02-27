#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SampleApp.csproj", ""]
RUN dotnet restore "./SampleApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SampleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleApp.csproj" -c Release -o /app/publish

FROM base AS final
RUN mkdir /files
RUN chmod 664 /files
WORKDIR /app
COPY --from=publish /app/publish .
COPY golive.pfx .
ENTRYPOINT ["dotnet", "SampleApp.dll"]