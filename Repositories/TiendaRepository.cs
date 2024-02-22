using pruebaresuelta.Context;
using pruebaresuelta.Interfaces;
using pruebaresuelta.Models;

namespace pruebaresuelta.Repositories
{
    public class TiendaRepository : ITiendaRepository
    {
        private readonly DataContext _dbContext;

        public TiendaRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<DataResponse> ObtenerDatosUltimos30Dias(int dias)
        {
            var fechaVentaMasReciente = _dbContext.Venta.Max(p => p.Fecha);
            var FechaLimite = fechaVentaMasReciente.AddDays(- dias);

            return (from v in _dbContext.Venta
                    join d in _dbContext.VentaDetalles on v.IdVenta equals d.IdVenta
                    join p in _dbContext.Productos on d.IdProducto equals p.IdProducto
                    join m in _dbContext.Marcas on p.IdMarca equals m.IdMarca
                    join l in _dbContext.Locals on v.IdLocal equals l.IdLocal
                    where v.Fecha >= FechaLimite
                    orderby v.Fecha
                    select new DataResponse
                    {
                        IdDetalle = d.IdVentaDetalle,
                        IdVenta = v.IdVenta,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Total = v.Total,
                        Fecha = v.Fecha,
                        NombreProducto = p.Nombre,
                        Marca = m.Nombre,
                        Local = l.Nombre
                    }).ToList();
        }
    }
}