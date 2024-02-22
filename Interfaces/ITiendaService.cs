namespace pruebaresuelta.Interfaces
{
    public interface ITiendaService
    {
        void ObtenerDatosUltimos30Dias(int dias);
        void ObtenerMontoYCantidadEn30Dias();
        void ObtenerDiaYHoraMaxMonto();
        void ObtenerProductoMayorMontoVentas();
        void ObtenerLocalConMayorMontoDeVentas();
        void ObtenerMarcaConMayorGanancia();
        void ObtenerProductosMasVendidosPorLocal();
    }
}