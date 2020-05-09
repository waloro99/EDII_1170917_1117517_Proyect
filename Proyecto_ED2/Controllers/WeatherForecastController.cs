using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Proyecto_ED2.Repository;
using Proyecto_ED2.BD;

namespace Proyecto_ED2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public static IWebHostEnvironment _environment;

        public WeatherForecastController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //object file
        public class FileUploadAPI
        {
            public IFormFile files { get; set; } //name use in postman --> files

        }


        //-------------------------------------------- END POINTS --------------------------------------------------

        //------------------------------------------- Agregar sucursal ---------------------------------------------

        #region AGREGAR SUCURSAL

        private static readonly ISucursalDB SDataBase = new SucursalDB();

        // localhost:51626/weatherforecast/Add/Sucursal
        [HttpPost("Add/Sucursal", Name = "PostAddSucursal")]
        public async Task<string> Post([FromBody]Sucursal sucursal)
        {
            try
            {
                //verificar si no hay otro id igual a sucursal
                if (sucursal.Id_sucursal == "1")
                {
                    //add sucursal
                    SDataBase.AddSucursal(sucursal);
                    return "Add Sucurusal Successful";
                }
                else
                {
                    return "Sucursal with that ID already exists.";
                }           
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
        {
         "Id_sucursal" : "001",
         "Name_sucursal" : "Guate",
         "Adress_sucursal" : "Guatemala City"
        }
        */

        #endregion

        //--------------------------------- Actualizar los datos de una sucursal ------------------------------------

        #region ACTUALIZAR SUCURSAL

        // localhost:51626/weatherforecast/Update/Sucursal
        [HttpPost("Update/Sucursal", Name = "PostUpdateSucursal")]
        public async Task<string> Post([FromBody]Sucursal sucursal, string x)
        {
            try
            {
                //verificar si existe sucursal
                if (sucursal.Id_sucursal == "1")
                {
                    //add sucursal
                    SDataBase.UpdateSucursal(sucursal);
                    return "Update Sucurusal Successful";
                }
                else
                {
                    return "No Sucursal with that ID was found.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
        {
         "Id_sucursal" : "001",
         "Name_sucursal" : "Guate",
         "Adress_sucursal" : "Guatemala City"
        }
        */

        #endregion

        //---------------------------------- Agregar un producto ----------------------------------------------------

        #region AGREGAR PRODUCTO

        // localhost:51626/weatherforecast/Add/Producto
        [HttpPost("Add/Producto", Name = "PostAddProduct")]
        public async Task<string> Post([FromBody]Producto producto)
        {
            try
            {
                //verificar si no hay otro id igual a producto
                if (producto.Id_producto == "1")
                {
                    //add sucursal
                    SDataBase.AddProducto(producto);
                    return "Add Producto Successful";
                }
                else
                {
                    return "Producto with that ID already exists.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
        {
         "Id_producto" : "001",
         "Name_producto" : "Cafe",
         "Adress_producto" : "Guatemala City"
        }
        */

        #endregion

        //-------------------------- Agregar múltiples productos (vía un archivo .csv) --------------------------------

        #region AGREGAR MULTIPLES PRODUCTOS

        // localhost:51626/weatherforecast/Add/Producto/CSV/?Cifrado=caesar
        [HttpPost("Add/Producto/CSV", Name = "PostAddProductMil")]
        public async Task<string> Post(string x,int z)
        {
            try
            {
                return "OK";
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }

        #endregion
        //no tested
        //-------------------------------- Actualizar los datos de un producto -----------------------------------------

        #region ACTUALIZAR PRODUCTO

        // localhost:51626/weatherforecast/Update/Producto
        [HttpPost("Update/Producto", Name = "PostUpdateProducto")]
        public async Task<string> Post([FromBody]Producto producto, string x)
        {
            try
            {
                //verificar si existe sucursal
                if (producto.Id_producto == "1")
                {
                    //add sucursal
                    SDataBase.UpdateProducto(producto);
                    return "Update producto Successful";
                }
                else
                {
                    return "No Producto with that ID was found.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
          {
           "Id_producto" : "001",
           "Name_producto" : "Cafe",
           "Adress_producto" : "Guatemala City"
          }
      */

        #endregion

        //----------------------------- Transferir unidades de una sucursal a otra -------------------------------------

        #region TRASNFERIR UNIDADES

        #endregion
            //no tested

        //---------------------------------- Agregar un producto a una sucursal ----------------------------------------

        #region AGREGAR PRODUCTO EN SUCURSAL

        // localhost:51626/weatherforecast/Add/ProductoPrecio
        [HttpPost("Add/ProductoPrecio", Name = "PostAddProductoPrecio")]
        public async Task<string> Post([FromBody]SucursalPrecio sucursalPrecio)
        {
            try
            {
                //verificar si ese producto ya se agrego a esa sucursal
                if (sucursalPrecio.Id_producto == "1" && sucursalPrecio.Id_sucursal == "1")
                {
                    //add producto in sucursal
                    SDataBase.AddProductoSucursal(sucursalPrecio);
                    return "Add Producto in sucursal Successful";
                }
                else
                {
                    return "This producto has already been added to that sucursal.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
        {
         "Id_sucursal" : "001",
         "Id_producto" : "001",
         "Inv_quantily" : "1"
        }
        */

        #endregion

        //-------------------------- Actualizar cantidad en inventario en la sucursal ---------------------------------

        #region ACTUALIZAR PRODUCTO EN SUCURSAL

        // localhost:51626/weatherforecast/Update/ProductoPrecio
        [HttpPost("Update/ProductoPrecio", Name = "PostUpdateProductoPrecio")]
        public async Task<string> Post([FromBody]SucursalPrecio sucursalPrecio, string x)
        {
            try
            {
                //verificar si ese producto con esa sucursal existen
                if (sucursalPrecio.Id_producto == "1" && sucursalPrecio.Id_sucursal == "1")
                {
                    //add producto in sucursal
                    SDataBase.UpdateProductoSucursal(sucursalPrecio);
                    return "Update quantily Producto in sucursal Successful";
                }
                else
                {
                    return "The product does not match the sucursal.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*
        {
         "Id_sucursal" : "001",
         "Id_producto" : "001",
         "Inv_quantily" : "10"  //tomar en cuenta que este numero se le agregara al inventario existente
        }
        */

        #endregion

        //------------------------------------- Visualizar Sucursal ---------------------------------------------------

        #region VISUALIZAR SUCURSAL

        #endregion
        //NO TESTED
        //------------------------------------- Visualizar Producto ---------------------------------------------------

        #region VISUALIZAR PRODUCTO

        #endregion
        //no tested
        //------------------------------------- Visualizar Producto Sucursal -----------------------------------------

        #region VISUALIZAR PRODUCTO EN SUCURSAL

        #endregion

        //no tested

    }
}
