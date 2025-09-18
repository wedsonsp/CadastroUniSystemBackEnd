# Etapa 1: build com imagem do SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar todos os arquivos do projeto (incluindo projetos dependentes)
COPY . .

# Restaurar dependências usando o arquivo de solução
RUN dotnet restore Sistemaws.Function/Sistemaws.Function.csproj

# Publicar apenas o projeto Function
RUN dotnet publish Sistemaws.Function/Sistemaws.Function.csproj --output /app --configuration Release

# Etapa 2: runtime do Azure Functions
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0

WORKDIR /home/site/wwwroot
COPY --from=build /app .

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

CMD ["func", "start", "--host", "0.0.0.0"]
