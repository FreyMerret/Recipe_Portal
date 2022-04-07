#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["System/RecipePortal.API/RecipePortal.API.csproj", "System/RecipePortal.API/"]
COPY ["Shared/RecipePortal.Settings/RecipePortal.Settings.csproj", "Shared/RecipePortal.Settings/"]
RUN dotnet restore "System/RecipePortal.API/RecipePortal.API.csproj"
COPY . .
WORKDIR "/src/System/RecipePortal.API"
RUN dotnet build "RecipePortal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipePortal.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipePortal.API.dll"]