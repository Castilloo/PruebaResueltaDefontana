
using pruebaresuelta.Interfaces;
using pruebaresuelta.Models;

namespace pruebaresuelta.Services
{
    public class TiendaService : ITiendaService
    {
        private readonly ITiendaRepository _tiendaRepository;
        private List<DataResponse> Datos;
        public TiendaService(ITiendaRepository tiendaRepository)
        {
            _tiendaRepository = tiendaRepository;
            Datos = new List<DataResponse>();
        }

        //Detalle de venta los últimos 30 días
        public void ObtenerDatosUltimos30Dias(int dias)
        {
            Datos = _tiendaRepository.ObtenerDatosUltimos30Dias(dias);
            
            foreach(var registro in Datos) 
            {
                Console.WriteLine($"ID: {registro.IdVenta}, Nombre Producto: {registro.NombreProducto}, Marca: {registro.Marca}, Valor unitario: {registro.PrecioUnitario}, Cantidad: {registro.Cantidad}, Total: {registro.Total}, Fecha: {registro.Fecha}");
            }
        }

        // ----------------El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)---------
        public void ObtenerMontoYCantidadEn30Dias(){
            var montoYCantidadEn30Dias = Datos
                .DistinctBy(p => p.IdVenta)
                .Select(p => new { IdVenta = p.IdVenta, Total = p.Total });

            Console.WriteLine($"Total de ventas: {montoYCantidadEn30Dias.Sum(p => p.Total)}, Cantidad total de ventas: {montoYCantidadEn30Dias.Count()}");
        }

        // //----------------El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).
        public void ObtenerDiaYHoraMaxMonto()
        {
            var diaYHoraMaxMonto = Datos
                .Select(p =>
                    new
                    {
                        Fecha = p.Fecha,
                        Dia = p.Fecha.DayOfWeek,
                        Hora = p.Fecha.ToString("h:mm:ss tt"),
                        Monto = p.Total
                    })
                .MaxBy(p => p.Monto);

            Console.WriteLine($"Fecha: {diaYHoraMaxMonto?.Fecha},Día: {diaYHoraMaxMonto?.Dia}, Hora: {diaYHoraMaxMonto?.Hora}, Monto: {diaYHoraMaxMonto?.Monto}");
        }

        // //----------------Indicar cuál es el producto con mayor monto total de ventas.------------
        public void ObtenerProductoMayorMontoVentas()
        {
            var productoMayorMontoVentas = Datos
                .GroupBy(p => p.NombreProducto)
                .Select(p => new
                {
                    NombreProducto = p.Key,
                    MontoTotal = p.Sum(t => (long)(t.PrecioUnitario * t.Cantidad))
                })
                .OrderByDescending(p => p.MontoTotal)
                .FirstOrDefault();

            Console.WriteLine($"Nombre: {productoMayorMontoVentas?.NombreProducto} - Monto Total: {productoMayorMontoVentas?.MontoTotal}");
        }

        // //----------------Indicar el local con mayor monto de ventas.------------
        public void ObtenerLocalConMayorMontoDeVentas()
        {
            var locales = Datos
                .DistinctBy(p => p.IdVenta)
                .GroupBy(p => p.Local)
                .Select(p => new { Local = p.Key, MontoVenta = p.Sum(t => t.Total) })
                .OrderByDescending(p => p.MontoVenta)
                .FirstOrDefault();

            Console.WriteLine($"Local: {locales?.Local} - MontoTotal: {locales?.MontoVenta}");
        }

        // //----------------¿Cuál es la marca con mayor margen de ganancias?------------
        public void ObtenerMarcaConMayorGanancia()
        {
            var ganaciaDeMarcas = Datos
                .GroupBy(p => p.Marca)
                .Select(p => new
                {
                    Marca = p.Key,
                    Ganancias = p.Sum(t => t.PrecioUnitario * t.Cantidad)
                })
                .OrderByDescending(p => p.Ganancias)
                .FirstOrDefault();

            Console.WriteLine($"Marca: {ganaciaDeMarcas?.Marca}, Ganancias: {ganaciaDeMarcas?.Ganancias}");
        }

        //----------------¿Cómo obtendrías cuál es el producto que más se vende en cada local?------------
        public void ObtenerProductosMasVendidosPorLocal() 
        {
            var productosMasVendidosPorLocal =  (from c in Datos
                                    group c by c.Local into l
                                    orderby l.Key
                                    select new
                                    {
                                        Local = l.Key,
                                        ProductoMasVendido = l
                                            .GroupBy(t => t.NombreProducto)
                                            .Select(t => new
                                            {
                                                NombreProducto = t.Key,
                                                Cantidad = t.Sum(r => r.Cantidad)
                                            })
                                            .OrderByDescending(t => t.Cantidad)
                                            .ThenBy(t => t.NombreProducto)
                                            .FirstOrDefault()
                                    }).Select(p => new {
                                        Local = p.Local, Producto = p.ProductoMasVendido.NombreProducto, Cantidad = p.ProductoMasVendido.Cantidad
                                    });

            foreach(var item in productosMasVendidosPorLocal)
                Console.WriteLine($"Local: {item.Local}, Producto más vendido: {item.Producto}, Cantidad: {item.Cantidad}");
        }
    }
}