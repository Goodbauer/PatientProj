FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PatientTest.Web/PatientTest.Web.csproj", "PatientTest.Web/"]
COPY ["PatientTest.Core/PatientTest.Core.csproj", "PatientTest.Core/"]
COPY ["PatientTest.Infrastructure/PatientTest.Infrastructure.csproj", "PatientTest.Infrastructure/"]
RUN dotnet restore "PatientTest.Web/PatientTest.Web.csproj"
COPY . .
WORKDIR "/src/PatientTest.Web"
RUN dotnet build "PatientTest.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientTest.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientTest.Web.dll"]
