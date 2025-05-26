FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CVParserAPI/CVParserAPI.csproj", "CVParserAPI/"]
RUN dotnet restore "CVParserAPI/CVParserAPI.csproj"
COPY . .
WORKDIR "/src/CVParserAPI"
RUN dotnet build "CVParserAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CVParserAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CVParserAPI.dll"] 