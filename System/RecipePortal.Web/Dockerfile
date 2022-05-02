FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["System/RecipePortal.Web/RecipePortal.Web.csproj", "System/RecipePortal.Web/RecipePortal.Web.csproj"]
RUN dotnet restore "System/RecipePortal.Web/RecipePortal.Web.csproj"
COPY . .
WORKDIR "/src/System/RecipePortal.Web"
RUN dotnet build "RecipePortal.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipePortal.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /var/www/web

COPY --from=publish /app/publish/wwwroot .
COPY System/RecipePortal.Web/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80