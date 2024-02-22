## Instrucciones:

- Esta aplicación se realizó con .Net 6 y Dotnet CLI en un ambiente Linux.

- Para poner a funcionar la aplicación se necesita ejecutar el siguiente script desde la carpeta actual:
~~~
    dotnet run
~~~

- En el archivo *TiendaService.cs* se encuentran los métodos de cada una de los puntos.

- Comentar cada uno de los métodos puntos para visualizar cada uno de los resultados.

- Los scripts sql se encuentra en el archivo *Respuestas.sql*.

- Crear un archivo appsettings.json con el contenido: 
~~~
{
    "ConnectionStrings": {
        "ConnectionDatabase": "Server={NOMBRE_SERVIDOR},{PUERTO};Database={NOMBRE_DB};User Id={USUARIO};Password={CONTRASEÑA}"
    }
}
~~~
