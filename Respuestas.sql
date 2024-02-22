DECLARE @MaxDate DATE;
SELECT @MaxDate = MAX(Fecha) FROM Venta V;

DECLARE @MinDate DATE = DATEADD(DAY, -30, @MaxDate);

-- Listado de las ventas en los últmos 30 días
SELECT v.ID_Venta, p.Nombre Producto, vt.Precio_Unitario, vt.Cantidad, v.Total ,p.ID_Marca, v.ID_Local
FROM Venta v
INNER JOIN VentaDetalle vt ON vt.ID_Venta = v.ID_Venta
INNER JOIN Producto p ON p.ID_Producto = vt.ID_Producto
INNER JOIN Marca m ON m.ID_Marca = p.ID_Marca
INNER JOIN Local l ON l.ID_Local = v.ID_Local
WHERE v.Fecha >= @MinDate
ORDER BY v.Fecha;

----------------El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)-----------------.
SELECT SUM(Total) "Total de ventas", COUNT(*) "Cantidad de ventas" FROM Venta v WHERE v.Fecha >= @MinDate;

----------------El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).------------
SELECT TOP 1 Total "Monto más alto", CONVERT(NVARCHAR(17), v.Fecha, 113) "Fecha y hora"
FROM Venta v
WHERE v.Fecha >= @MinDate
ORDER BY Total DESC;

----------------Indicar cuál es el producto con mayor monto total de ventas.------------
SELECT TOP 1
p.Nombre AS NombreProducto,
SUM(vd.Precio_Unitario * vd.Cantidad) MontoTotal
FROM Venta v
JOIN VentaDetalle vd ON v.ID_Venta = vd.ID_Venta
JOIN Producto p ON vd.ID_Producto = p.ID_Producto
WHERE v.Fecha >= @MinDate
GROUP BY p.Nombre
ORDER BY MontoTotal DESC;

----------------Indicar el local con mayor monto de ventas.------------
SELECT TOP 1 l.Nombre AS Local, SUM(Total) As MontoTotal
FROM Venta v
INNER JOIN Local l ON l.ID_Local = v.ID_Local
WHERE v.Fecha >= @MinDate
GROUP BY l.Nombre
ORDER BY MontoTotal DESC;

----------------¿Cómo obtendrías cuál es el producto que más se vende en cada local?------------

WITH TABLA AS (
    SELECT l.Nombre AS NombreLocal, p.Nombre AS NombreProducto, SUM(cantidad) AS TotalVendido,
        ROW_NUMBER() OVER (PARTITION BY l.Nombre ORDER BY SUM(cantidad) DESC) AS Ranking
    FROM Venta v
    INNER JOIN VentaDetalle vt ON vt.ID_Venta = v.ID_Venta
    INNER JOIN Producto p ON p.ID_Producto = vt.ID_Producto
    INNER JOIN Local l ON l.ID_Local = v.ID_Local
    WHERE v.Fecha >= @MinDate
    GROUP BY l.Nombre, p.Nombre
)
SELECT NombreLocal, NombreProducto, TotalVendido
FROM TABLA
WHERE Ranking = 1;
