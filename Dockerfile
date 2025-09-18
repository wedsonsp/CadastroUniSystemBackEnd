# Use a imagem oficial do .NET 8 SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["Sistemaws.WebApi/Sistemaws.WebApi.csproj", "Sistemaws.WebApi/"]
COPY ["Sistemaws.Application/Sistemaws.Application.csproj", "Sistemaws.Application/"]
COPY ["Sistemaws.Domain/Sistemaws.Domain.csproj", "Sistemaws.Domain/"]
COPY ["Sistemaws.Infrastructure/Sistemaws.Infrastructure.csproj", "Sistemaws.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "Sistemaws.WebApi/Sistemaws.WebApi.csproj"

# Copiar todo o código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/Sistemaws.WebApi"
RUN dotnet build "Sistemaws.WebApi.csproj" -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish "Sistemaws.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expor a porta
EXPOSE 80
EXPOSE 443

# Comando de entrada
ENTRYPOINT ["dotnet", "Sistemaws.WebApi.dll"]
