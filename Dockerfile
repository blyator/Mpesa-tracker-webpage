FROM node:20-alpine AS css
WORKDIR /src
COPY package*.json ./
RUN npm install
COPY wwwroot/css/input.css ./wwwroot/css/
COPY tailwind.config.js* ./
COPY Views ./Views
RUN npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/site.css

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
COPY --from=css /src/wwwroot/css/site.css ./wwwroot/css/site.css
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MpesaDashboard.dll"]