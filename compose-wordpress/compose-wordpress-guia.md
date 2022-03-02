# Tutorial DockerCompose Wordpress
 
Antes de nada abriremos el docker-compose.yml y leeremos detenidamente todos los comentarios para entender bien la sintaxis docker-compose y para qué sirve cada cosa.
 
Procederemos a levantar los dos servicios de nuestro docker-compose usando el comando:
 
```shell wrap
docker-compose up
 
[+] Running 5/5
 - Network compose-wordpress_backend          Created                                               0.0s
 - Volume "compose-wordpress_wordpress_data"  Create...                                             0.0s
 - Volume "compose-wordpress_db_data"         Created                                               0.0s
 - Container compose-wordpress-db-1           Created                                               0.0s
 - Container compose-wordpress-wordpress-1    Created                                               0.3s
Attaching to compose-wordpress-db-1, compose-wordpress-wordpress-1
compose-wordpress-db-1         | 2022-03-02 19:26:19+00:00 [Note] [Entrypoint]: Entrypoint script for MySQL Server 5.7.37-1debian10 started.
```
Una vez que haya terminado el proceso, deberíamos poder acceder a una web de Wordpress en [localhost:8000](http://localhost:8000/)
 
 
Para comprobar que los contenedores están funcionando correctamente usaremos el comando:
 
```shell wrap
docker-compose ps
 
NAME                            COMMAND                  SERVICE             STATUS              PORTS
compose-wordpress-db-1          "docker-entrypoint.s…"   db                  running (healthy)   33060/tcp
compose-wordpress-wordpress-1   "docker-entrypoint.s…"   wordpress           running             0.0.0.0:8000->80/tcp
```
 
Docker-compose también ofrece la posibilidad de escalar los servicios y poder crear más réplicas para ello podemos ejecutar el comando:
 
```shell wrap
docker-compose up --scale db=2 --scale wordpress=2 --no-recreate
 
```
 
Si ejecutamos este comando,  docker-compose intentará crear una réplica más de cada servicio, pero fallará porque no podemos publicar el puerto 8000 otra vez porque ya estará siendo usado
 
Finalmente podemos eliminar todos los contenedores haciendo
 
```shell wrap
docker-compose down
```