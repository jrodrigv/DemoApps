# Demo Docker Stackas

## Prerequisitos

- Haber realizado el tutorial swarm-demo-guia.md y tener el cluster levantando
- Eliminar todos los servicios previamos creados con docker service rm

## Desplegando la stack de ASP.NET + SQL Server

En la carpeta actual disponemos de un fichero **docker-stacks-netapp.yml** todos los ficheros de la carpeta actual son accesibles desde los nodos del cluster.

A continuación nos colocaremos en la ruta donde residen

```shell wrap
m1:~$ cd ..
m1:/home$ cd ..
m1:/$ cd vagrant
m1:/vagrant$ ls
Vagrantfile                  basic-web-net                db_password.txt              db_root_password.txt         docker-stacks-netapp.yml     docker-stacks-wordpress.yml  stacks-demo-guia.md          swarm-demo-guia.md
```

Ahora para crear una stack de .NET usaremos el comando **docker stack deploy** pasandole el fichero compose y añadiremos un nombre para el stack **netapp**

```shell wrap
m1:/vagrant$ docker stack deploy --compose-file "docker-stacks-netapp.yml" netapp
Creating network netapp_frontend
Creating network netapp_backend
Creating service netapp_web
Creating service netapp_db
```

Podemos observar como **netapp** se ha usado como prefijo para la creación de redes y servicios.

Nuestra aplicación estará accesible en **<http://localhost:8080/>**.

Ahora podemos listar la stack con **docker stack ls**

```shell wrap
m1:/vagrant$ docker stack ls
NAME      SERVICES   ORCHESTRATOR
netapp    2          Swarm
```

Nuestro Swarm tiene una stack netapp que tiene dos servicios, ahora listaremos los servicios con **docker service ls**,  nos listará que tenemos el servicio de base datos y las dos replicas del servicio de la web

```shell wrap
m1:/vagrant$ docker service ls
ID             NAME         MODE         REPLICAS   IMAGE                                   PORTS
m1ssgneoar31   netapp_db    replicated   1/1        mcr.microsoft.com/mssql/server:latest
m8wyw21rhpqx   netapp_web   replicated   2/2        jrodrigv/compose-net-web:latest         *:8080->80/tcp
```

Usando **docker stack ps netapp** podemos visualizar donde se estan ejecutando cada uno de los servicios

```shell wrap
m1:/vagrant$ docker stack ps netapp
ID             NAME           IMAGE                                   NODE      DESIRED STATE   CURRENT STATE           ERROR     PORTS
bjjeciujxwxn   netapp_db.1    mcr.microsoft.com/mssql/server:latest   m2        Running         Running 7 minutes ago
wayx0sizn7lm   netapp_web.1   jrodrigv/compose-net-web:latest         w1        Running         Running 7 minutes ago
kt55sfx6zawi   netapp_web.2   jrodrigv/compose-net-web:latest         m1        Running         Running 6 minutes ago
```

## Desplegando la stack de Wordpress

Vamos a añadir otra stack más en el cluster, en esta caso añadiremos la aplicación de wordpress con **docker stack deploy --compose-file "docker-stacks-wordpress.yml" wordpress**

```shell wrap
m1:/vagrant$ docker stack deploy --compose-file "docker-stacks-wordpress.yml" wordpress
Creating network wordpress_default
Creating secret wordpress_db_password
Creating secret wordpress_db_root_password
Creating service wordpress_wordpress
Creating service wordpress_db
```

Esta stack hace uso de docker secrets, disponemos de dos ficheros db_password.txt y db_root_password.txt, en su fichero compose hemos especificado que esos ficheros contienen secretos y que Docker debe leerlos y crearlos en nuestro cluster para ser accesibles.

Si hacemos de nuevo **docker stack ls** podemos comprobar que la stack de wordpress esta lista.

```shell wrap
m1:/vagrant$ docker stack ls
NAME        SERVICES   ORCHESTRATOR
netapp      2          Swarm
wordpress   2          Swarm
```

Podemos acceder a la aplicación en **<http://localhost:8000/>**

Finalmente podemos eliminar las stacks usando el comando **docker stack rm**

```shell wrap
m1:/vagrant$ docker stack rm netapp
Removing service netapp_db
Removing service netapp_web
Removing network netapp_frontend
Removing network netapp_backend
m1:/vagrant$ docker stack rm wordpress
Removing service wordpress_db
Removing service wordpress_wordpress
Removing secret wordpress_db_password
Removing secret wordpress_db_root_password
Removing network wordpress_default
```