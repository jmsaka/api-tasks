# Usar a imagem base do ASP.NET para execu��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usar a imagem SDK do .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos os arquivos do projeto, incluindo o TaskManagement.Api.csproj e outros projetos dependentes
COPY src/TaskManagement.Api/TaskManagement.Api.csproj src/TaskManagement.Api/
COPY src/TaskManagement.Application/TaskManagement.Application.csproj src/TaskManagement.Application/
COPY src/TaskManagement.Domain/TaskManagement.Domain.csproj src/TaskManagement.Domain/
COPY src/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj src/TaskManagement.Infrastructure/
COPY src/TaskManagement.CrossCutting/TaskManagement.CrossCutting.csproj src/TaskManagement.CrossCutting/

# Restaurar as depend�ncias
RUN dotnet restore "src/TaskManagement.Api/TaskManagement.Api.csproj"

# Copiar o restante do c�digo-fonte para o container
COPY src/ ./

# Construir o projeto no diret�rio correto
WORKDIR /src/TaskManagement.Api
RUN dotnet build "TaskManagement.Api.csproj" -c Release -o /app/build

# Publicar a aplica��o
FROM build AS publish
RUN dotnet publish "TaskManagement.Api.csproj" -c Release -o /app/publish

# Usar a imagem base para a execu��o
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "TaskManagement.Api.dll"]
# Adicionar comando para aplicar migra��es durante a inicializa��o
CMD ["sh", "-c", "dotnet ef database update && dotnet TaskManagement.Api.dll"]
