# Tutorial DockerCompose .NET Core MVC + SQL Server

## 1. Instalando Docker-compose

Si vas a realizar el tutoral con https://www.docker.com/play-with-docker necesitarás actualizar la version de Docker-Compose

(recuerda que para pegar en PlayWithDocker se hace con Ctrl+Shift+v)

```shell wrap
pip install --upgrade docker-compose
```

Si vas a realizarlo con una instalación local de Docker, seguiremos la guia oficial para instalar docker compose:
https://docs.docker.com/compose/install/

## 2. Levantando docker-compose

Compilaremos nuestro servicio web usandodo el dockerfile definido en la ruta actual y despues procederemos a levantar el Compose con los dos contenedores

```shell wrap
docker-compose build
docker-compose up
```

Una vez que haya terminado el proceso, deberiamos poder acceder a la web en localhost:8000, tenemos una opción para registrar un nuevo usuario, las nuevas identidades se almacenan en el base de datos y podremos hacer log in y log out!

Finalmente podemos eliminar todos los contenedores haciendo

```shell wrap
docker-compose down
```
