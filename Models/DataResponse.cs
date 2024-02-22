namespace pruebaresuelta.Models
{
    public class DataResponse
    {
        public long IdDetalle { get; set; }
        public long IdVenta { get; set; }
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }
        public int Total { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Local { get; set; } = null!;
    }
}