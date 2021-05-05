
# Imagen estable de Ubuntu a Febrero del 2021. 
# Siempre es recomendable seleccionar el tag concreto en vez de usar Latest (Latest puede cambiar sin previo aviso)
FROM ubuntu:20.04


RUN apt-get update &&  apt-get -y upgrade \ 
&& apt-get install -y wget \
&& wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
&& dpkg -i packages-microsoft-prod.deb \
&& apt-get update &&  apt-get -y upgrade \
&& apt-get install -y apt-transport-https \
dotnet-sdk-5.0

# Copiando codigo fuente al container
COPY . /src

# Cambiando directorio a src
WORKDIR /src

# Compilando la solucion
RUN dotnet build WebApp.sln -v m

# Ejecutando UnitTests
RUN dotnet test --filter UnitTests -v m

# Ejecutando IntegrationTests
RUN dotnet test --filter IntegrationTests -v m

# Publicando la applicacion
RUN dotnet publish WebApp/WebApp.csproj --output ../Binaries

WORKDIR ../Binaries

# El container expondra al host el puerto 8080
EXPOSE 5000

ENTRYPOINT ["dotnet", "WebApp.dll"]






