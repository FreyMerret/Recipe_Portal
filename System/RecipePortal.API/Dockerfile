#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["System/RecipePortal.API/RecipePortal.API.csproj", "System/RecipePortal.API/"]
COPY ["Shared/RecipePortal.Settings/RecipePortal.Settings.csproj", "Shared/RecipePortal.Settings/"]
COPY ["Services/RecipePortal.RecipeService/RecipePortal.RecipeService.csproj", "Services/RecipePortal.RecipeService/"]
COPY ["Shared/RecipePortal.Common/RecipePortal.Common.csproj", "Shared/RecipePortal.Common/"]
COPY ["Data/RecipePortal.Db.Context/RecipePortal.Db.Context.csproj", "Data/RecipePortal.Db.Context/"]
COPY ["Data/RecipePortal.Db.Entities/RecipePortal.Db.Entities.csproj", "Data/RecipePortal.Db.Entities/"]
COPY ["Services/RecipePortal.RabbitMqService/RecipePortal.RabbitMqService.csproj", "Services/RecipePortal.RabbitMqService/"]
COPY ["Services/RecipePortal.UserAccountService/RecipePortal.UserAccountService.csproj", "Services/RecipePortal.UserAccountService/"]
COPY ["Services/RecipePortal.EmailService/RecipePortal.EmailService.csproj", "Services/RecipePortal.EmailService/"]
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