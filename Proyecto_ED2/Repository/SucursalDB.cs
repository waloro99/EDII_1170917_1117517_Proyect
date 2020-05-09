using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proyecto_ED2.BD;

namespace Proyecto_ED2.Repository
{
    public class SucursalDB : ISucursalDB
    {

        //method for add new sucursal
        public void AddSucursal(Sucursal sucursal)
        {
            //code add sucursal at the tree B
        }

        //method for update sucursal
        public void UpdateSucursal(Sucursal sucursal)
        {
            //code update sucursal at the tree B
        }

        //method for add new producto
        public void AddProducto(Producto producto)
        {
            //code add producto at the tree B
        }

        //method for update producto
        public void UpdateProducto(Producto producto)
        {
            //code update producto at the tree B
        }

        //method for add new producto in sucursal
        public void AddProductoSucursal(SucursalPrecio sucursalPrecio)
        {
            //code add producto in sucursal at the tree B
        }

        //method for update producto in sucursal
        public void UpdateProductoSucursal(SucursalPrecio sucursalPrecio)
        {
            //code update producto in sucursal at the tree B
        }

        //method for view sucursal
        public List<Sucursal> ViewSucursal()
        {
            //code view sucursal at the tree B
            return null;
        }

        //method for view product
        public List<Producto> ViewProduct()
        {
            //code view producto  at the tree B
            return null;
        }

        //method for view product in sucursal
        public List<SucursalPrecio> ViewProductSucursal()
        {
            //code view product in sucursal at the tree B
            return null;
        }

    }
}
