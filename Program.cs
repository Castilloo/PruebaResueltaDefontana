using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pruebaresuelta.Context;
using pruebaresuelta.Interfaces;
using pruebaresuelta.Repositories;
using pruebaresuelta.Services;

namespace pruebaresuelta;
public class Program 
{
    static void Main(string[] args)
    {
        
        var serviceProvider = RunServiceProvider();

        var data = serviceProvider.GetRequiredService<ITiendaService>();
        //Pregunta 2.1
        data.ObtenerDatosUltimos30Dias(30);
        //Pregunta 2.1
        data.ObtenerMontoYCantidadEn30Dias();
        //Pregunta 2.2
        data.ObtenerDiaYHoraMaxMonto();
        //Pregunta 2.3
        data.ObtenerProductoMayorMontoVentas();
        //Pregunta 2.4
        data.ObtenerLocalConMayorMontoDeVentas();
        //Pregunta 2.5
        data.ObtenerMarcaConMayorGanancia();
        //Pregunta 2.6
        data.ObtenerProductosMasVendidosPorLocal();
    } 
    private static IServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        // Agregar DbContext y Repositorio como servicios
        services.AddDbContext<DataContext>(options => 
        {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionDatabase"));
        });
        services.AddTransient<ITiendaRepository, TiendaRepository>();
        services.AddTransient<ITiendaService, TiendaService>();

        return services.BuildServiceProvider();
    }

    private static IServiceProvider RunServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        return ConfigureServices(configuration);
    }
}
