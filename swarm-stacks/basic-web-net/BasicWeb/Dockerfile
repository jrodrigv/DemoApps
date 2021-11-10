#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine
COPY . /app
WORKDIR /app
RUN ["dotnet", "build"]
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "BasicWeb.dll","--server.urls http://*:80"]