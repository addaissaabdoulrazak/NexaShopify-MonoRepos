# Étape 1 : Build (Construction)
# On utilise l'image SDK officielle de Microsoft pour compiler le projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie du fichier de solution et des fichiers projets pour restaurer les dépendances
# Cette étape est séparée pour optimiser le cache Docker (si les dépendances ne changent pas, cette étape est sautée)
COPY ["NexaShopify.API.sln", "./"]
COPY ["Web/NexaShopify.API/NexaShopify.API.csproj", "Web/NexaShopify.API/"]
COPY ["Core/NexaShopify.Core/NexaShopify.Core.csproj", "Core/NexaShopify.Core/"]
COPY ["Core/NexaShopify.Core.Common/NexaShopify.Core.Common.csproj", "Core/NexaShopify.Core.Common/"]
COPY ["Core/NexaShopify.Core.Identity/NexaShopify.Core.Identity.csproj", "Core/NexaShopify.Core.Identity/"]
COPY ["Core/NexaShopify.Core.SharedKernel/NexaShopify.Core.SharedKernel.csproj", "Core/NexaShopify.Core.SharedKernel/"]
COPY ["Core/NexaShopify.Core.Shop/NexaShopify.Core.Shop.csproj", "Core/NexaShopify.Core.Shop/"]
COPY ["Infrastructure/Infrastructure.Data/Infrastructure.Data.csproj", "Infrastructure/Infrastructure.Data/"]
COPY ["Infrastructure/Infrastructure.Service/Infrastructure.Service.csproj", "Infrastructure/Infrastructure.Service/"]
COPY ["Tests/NexaShopify.Core.Tests/NexaShopify.Core.Tests.csproj", "Tests/NexaShopify.Core.Tests/"]

# Restauration des packages NuGet
RUN dotnet restore "Web/NexaShopify.API/NexaShopify.API.csproj"

# Copie du reste du code source
COPY . .

# Compilation du projet en mode Release
WORKDIR "/src/Web/NexaShopify.API"
RUN dotnet build "NexaShopify.API.csproj" -c Release -o /app/build

# Étape 2 : Publish (Publication)
# On publie l'application dans un dossier final optimisé
FROM build AS publish
RUN dotnet publish "NexaShopify.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 3 : Final (Image d'exécution)
# On utilise une image plus légère (ASP.NET Core Runtime) pour l'exécution
# Cela réduit la taille de l'image finale et améliore la sécurité
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NexaShopify.API.dll"]