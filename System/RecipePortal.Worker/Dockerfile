#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["System/RecipePortal.Worker/RecipePortal.Worker.csproj", "System/RecipePortal.Worker/"]
COPY ["Shared/RecipePortal.Settings/RecipePortal.Settings.csproj", "Shared/RecipePortal.Settings/"]
COPY ["Services/RecipePortal.RabbitMqService/RecipePortal.RabbitMqService.csproj", "Services/RecipePortal.RabbitMqService/"]
COPY ["Services/RecipePortal.EmailService/RecipePortal.EmailService.csproj", "Services/RecipePortal.EmailService/"]
COPY ["Data/RecipePortal.Db.Context/RecipePortal.Db.Context.csproj", "Data/RecipePortal.Db.Context/"]
COPY ["Data/RecipePortal.Db.Entities/RecipePortal.Db.Entities.csproj", "Data/RecipePortal.Db.Entities/"]
RUN dotnet restore "System/RecipePortal.Worker/RecipePortal.Worker.csproj"
COPY . .
WORKDIR "/src/System/RecipePortal.Worker"
RUN dotnet build "RecipePortal.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipePortal.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipePortal.Worker.dll"]