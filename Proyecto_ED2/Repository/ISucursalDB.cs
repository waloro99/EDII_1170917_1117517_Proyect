using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_ED2.BD;

namespace Proyecto_ED2.Repository
{
    interface ISucursalDB
    {

        //ADD Sucursal
        void AddSucursal(Sucursal sucursal);

        //Update Sucursal
        void UpdateSucursal(Sucursal sucursal);

        //ADD Producto
        void AddProducto(Producto producto);

        //Update Sucursal
        void UpdateProducto(Producto producto);

        //ADD Producto in Sucursal
        void AddProductoSucursal(SucursalPrecio sucursalPrecio);

        //Update Producto in Sucursal
        void UpdateProductoSucursal(SucursalPrecio sucursalPrecio);

        //View sucursal
        List<Sucursal> ViewSucursal();

        //View Product
        List<Producto> ViewProduct( );

        //View producto in sucursal
        List<SucursalPrecio> ViewProductSucursal( );



    }
}
