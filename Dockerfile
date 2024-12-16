# Use a imagem do .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie os arquivos do projeto
COPY . ./

# Restaure as dependências
RUN dotnet restore

# Compile o projeto
RUN dotnet publish -c Release -o out

# Use uma imagem mais leve para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Exponha a porta
EXPOSE 80
EXPOSE 443

# Execute a aplicação
ENTRYPOINT ["dotnet", "TaskManagement.dll"]
