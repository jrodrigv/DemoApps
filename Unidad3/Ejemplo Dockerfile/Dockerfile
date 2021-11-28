FROM ubuntu:20.04
#La instrucción FROM sirve para que Docker sepa que imagen base vamos a usar. Esta será la primera capa de nuestra imagen

ENV DEBIAN_FRONTEND="noninteractive" TZ="America/Toronto"
#Crea una variable de entorno para pasar el locale- que necesitamos en Ubuntu
	
RUN apt-get update
RUN apt-get install -y apache2
#Ejecutamos apt-get e instalamos la ultima version de Apache.

ADD index.html /var/www/html/index.html
#Copia el fichero index.html desde el directorio actual al directorio web raíz de la imagen.

CMD /usr/sbin/apache2ctl -D FOREGROUND

#Cuando se levante un container de esta imagen, automáticamente ejecutará apache2ctl para dejar el servidor web ejecutándose en primer plano.

EXPOSE 80
#Informa que los containers que se levanten usan el puerto 80 y necesitan que se publique este puerto haciendo docker run -p
