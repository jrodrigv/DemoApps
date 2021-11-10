# Tutorial Docker Swarm

## Prerequisitos

Para crear el cluster en local necesitarás instalar:

- Vagrant: https://www.vagrantup.com/downloads
- VirtualBox: https://www.virtualbox.org/wiki/Downloads
- Tener activada la virtualización en la Bios de nuestro PC.

## Creando la infraestructura

Disponemos de un fichero Vagrantfile que define la infraestructura que vamos a usar, en este ejemplo crearemos 3 nodos manager y 3 nodos worker como vimos en la diapositiva 10, Figura 5-1.

Usando el siguiente comando se empezarán a crear todas las maquinas

```shell wrap
vagrant up
```

Despues de unos minutos deberiamos tener las 6 maquinas virtuales ejecutandose en VirtualBox.

## Configurando el cluster

1. Inicializaremos el cluster usando docker swarm init

    El primer paso es inicializar el cluster.

    Para ellos entraremos en la maquina m1 via ssh:

    ```shell wrap
    vagrant ssh m1
    ```

    Inicializamos el cluster con **docker swarm init** y le pasamos la propia ip definida en el Vagrantfile para el nodo m1

    ```shell wrap
    m1:~$ docker swarm init --advertise-addr 10.0.0.10:2377 --listen-addr 10.0.0.10:2377
    Swarm initialized: current node (xicvao0fisc9i64de17lf56rs) is now a manager.

    To add a worker to this swarm, run the following command:

        docker swarm join --token SWMTKN-1-1r3xgytz0ykxa043d2ifjpbs7982mns8q8u6ahtdofcspde89x-8cu1ojpkpnkq6paxil6zl2hm5 10.0.0.10:2377

    To add a manager to this swarm, run 'docker swarm join-token manager' and follow the instructions.
    ```

    Importante tomar nota del comando porque lo usaremos para añadir a los workers w1,w2 y w3.

2. A continuación ejecutamos el siguiente comando para tambien tomar notar del token necesario para añadir un nuevo manager:

    ```shell wrap
    m1:~$ docker swarm join-token manager
    To add a manager to this swarm, run the following command:

        docker swarm join --token SWMTKN-1-1r3xgytz0ykxa043d2ifjpbs7982mns8q8u6ahtdofcspde89x-5mnp0yyk03e6wpekw3tpzy3fc 10.0.0.10:2377
    ```

    Y tambien apuntamos el comando porque lo usaremos para añadir los managers m2 y m3.

3. A continuacion iremos haciendo vagrant ssh en las maquinas m2 y m3, y escribiremos el comando de add a manager.

    ```shell wrap
    m2:~$ docker swarm join --token SWMTKN-1-3xydynj7oy681fheklj5rlng0xzz175dufaown7f4hczmbgrnu-0z49mh0ymamh2hituhwyy60f4 10.0.0.10:2377
    This node joined a swarm as a manager.
    m2:~$ exit
    logout
    Connection to 127.0.0.1 closed.
    ```

4. Luego haremos lo mismo con el w1, w2 e w3.

    ```shell wrap
    w2:~$ docker swarm join --token SWMTKN-1-3xydynj7oy681fheklj5rlng0xzz175dufaown7f4hczmbgrnu-3spzwc7twnqrnh0rktnw0pdn9 10.0.0.10:2377
    This node joined a swarm as a worker.
    w2:~$ exit
    logout
    Connection to 127.0.0.1 closed.
    ```

5. Finalmente, podemos volver a hacer vagrant ssh m1 y ver cuantos nodos tenemos registrados usando el comando **docker node ls**

    ```shell wrap
    vagrant@m1:~$ docker node ls
    ID                            HOSTNAME   STATUS    AVAILABILITY   MANAGER STATUS   ENGINE VERSION
    m1ki6bwz43015eed2h7ryd9bn *   m1         Ready     Active         Leader           20.10.7
    f81ymn73v065b9mhn1tq9c6ed     m2         Ready     Active         Reachable        19.03.5
    1bqq2ml6x546qgb6bls0dbui5     m3         Ready     Active         Reachable        19.03.5
    t75yj87zzrq4c3ncliei3buuy     w1         Ready     Active                          19.03.5
    a8ln8yewlnmlzy52aplatoh63     w2         Ready     Active                          19.03.5
    j21f0grm9r4kt4ul3s47wwbwx     w3         Ready     Active                          19.03.5
    ```

    Vemos como efectivamente tenemos 3 nodos manager, siendo el primero el lider y los otros los seguidores, y 3 nodos worker.

## Añadiendo servicios al cluster

6. En esta demo vamos a crear un docker service que es una web simple en ASP.Net Core con 3 replicas.

    ```shell wrap
    vagrant@m1:~$ docker service create --name web --publish published=8080,target=80 --replicas 3 jrodrigv/swarm-web-app:latest
    er88pukrljai1acsqzmdhyyi4
    overall progress: 3 out of 3 tasks
    1/3: running   [==================================================>]
    2/3: running   [==================================================>]
    3/3: running   [==================================================>]
    verify: Service converged
    ```

    El puerto 8080 quedará abierto en todos los nodos del cluster y será redireccionado automaticamente al puerto 80 de una de las replicas del servicio.

7. Podemos listar los servicios del Swarm con:

    ```shell wrap
    vagrant@m1:~$ docker service ls
    ID                  NAME                MODE                REPLICAS            IMAGE                           PORTS
    er88pukrljai        web                 replicated          3/3                 jrodrigv/swarm-web-app:latest   *:8080->80/tcp
    ```

8. Tambien podemos ver donde se ejecutan cada de una la replicas del servicio con:

    ```shell wrap
    vagrant@m1:~$ docker service ps web
    ID                  NAME                IMAGE                           NODE                DESIRED STATE       CURRENT STATE            ERROR               PORTS
    lx4ni1i9z70z        web.1               jrodrigv/swarm-web-app:latest   m3                  Running             Running 20 minutes ago
    1bh6h8k3l80n        web.2               jrodrigv/swarm-web-app:latest   m1                  Running             Running 20 minutes ago
    2g1mmmswmrpl        web.3               jrodrigv/swarm-web-app:latest   m2                  Running             Running 20 minutes ago
    ```

9. Vemos que la replica **1bh6h8k3l80n...** se esta ejecutando en nuestro nodo **m1**. Si ejecutamos **docker ps** podemos ver los contenedores que tenemos en ejecución en nuestro nodo, y observamos en el nombre que efectivamente es la replica web.2.**1bh6h8k3l80n**rud0b3mkqqj8x

    ```shell wrap
    vagrant@m1:~$ docker ps
    CONTAINER ID        IMAGE                           COMMAND                  CREATED             STATUS              PORTS               NAMES
    64552c2b29f3        jrodrigv/swarm-web-app:latest   "dotnet BasicWeb.dll…"   32 minutes ago      Up 32 minutes       80/tcp              web.2.1bh6h8k3l80nrud0b3mkqqj8x
    ```

10. Tambien podemos acceder a los logs generados por esta replica usando **docker logs** y el id del contenedor.

    ```shell wrap
    vagrant@m1:~$ docker logs 64552c2b29f3
    warn: Microsoft.AspNetCore.DataProtection.Repositories.FileSystemXmlRepository[60]
        Storing keys in a directory '/root/.aspnet/DataProtection-Keys' that may not be persisted outside of the container. Protected data will be unavailable when container is destroyed.
    warn: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[35]
        No XML encryptor configured. Key {4bfa38e0-7e17-4812-80b9-a9d3669bdc86} may be persisted to storage in unencrypted form.
    info: Microsoft.Hosting.Lifetime[0]
        Now listening on: http://0.0.0.0:80
    info: Microsoft.Hosting.Lifetime[0]
        Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
        Hosting environment: Production
    info: Microsoft.Hosting.Lifetime[0]
        Content root path: /app
    ```

11. Finalmente para comprobar que los nodos estan funcionando simplemente accediendo a "localhost:8080" desde nuestro PC, tambien podemos escribir el siguiente comando desde dentro de M1:

    ```shell wrap
        vagrant@m1:~$ watch -n 1 'curl http://m1:8080 | grep -F "Welcome from "'
    % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current
                                    Dload  Upload   Total   Spent    Left  Speed
    0     0    0     0    0     0      0      0 --:--:-- --:--:-- --:--:--     0 100  2234    0  2234    0     0   545k      0 --:--:-- --:--:-- --:--:--  545k
        <h1 class="display-4">Welcome from ba0e6ad26239</h1>                                                          436                                     436
                                        07cd473c5a88
    ```

    Podemos ver como el hostname va variando en cada curl y a veces repitiendose! Todas las instancias estan funcionando correctamente.

## Escalando un servicio

Escalar un servicio es algo bastante sencillo, en el ejemplo siguiente incrementamos el numero de replicas de 3 a 6.

```shell wrap
    vagrant@m1:~$ docker service scale web=6
    web scaled to 6
    overall progress: 6 out of 6 tasks
    1/6: running   [==================================================>]
    2/6: running   [==================================================>]
    3/6: running   [==================================================>]
    4/6: running   [==================================================>]
    5/6: running   [==================================================>]
    6/6: running   [==================================================>]
    verify: Service converged
```

## Eliminando el servicio creado

Para el eliminar el servicio creado usaremos el comando **docker service rm web**

## Eliminando el cluster

Puedes eliminar todas las VMs del Swarm haciendo **vagrant destroy**
