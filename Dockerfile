# Use a imagem do .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie os arquivos do projeto
COPY . ./

# Restaure as depend�ncias
RUN dotnet restore

# Compile o projeto
RUN dotnet publish -c Release -o out

# Use uma imagem mais leve para execu��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Exponha a porta
EXPOSE 80
EXPOSE 443

# Execute a aplica��o
ENTRYPOINT ["dotnet", "TaskManagement.dll"]
