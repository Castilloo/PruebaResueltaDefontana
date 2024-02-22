using pruebaresuelta.Models;

namespace pruebaresuelta.Interfaces
{
    public interface ITiendaRepository
    {
        List<DataResponse> ObtenerDatosUltimos30Dias(int dias);
    }
}