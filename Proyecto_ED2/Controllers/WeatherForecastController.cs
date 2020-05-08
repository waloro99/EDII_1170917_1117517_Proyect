using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

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

        // localhost:51626/weatherforecast/Add/Sucursal/?Cifrado=caesar
        [HttpPost("Add/Sucursal", Name = "PostAddSucursal")]
        public async Task<string> Post()
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

        //--------------------------------- Actualizar los datos de una sucursal ------------------------------------

        //---------------------------------- Agregar un producto ----------------------------------------------------

        // localhost:51626/weatherforecast/Add/Producto/?Cifrado=caesar
        [HttpPost("Add/Producto", Name = "PostAddProduct")]
        public async Task<string> Post(string x)
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

        //-------------------------- Agregar múltiples productos (vía un archivo .csv) --------------------------------

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

        //-------------------------------- Actualizar los datos de un producto -----------------------------------------

        //----------------------------- Transferir unidades de una sucursal a otra -------------------------------------

        //---------------------------------- Agregar un producto a una sucursal ----------------------------------------

        // localhost:51626/weatherforecast/Add/ProductoPrecio/?Cifrado=caesar
        [HttpPost("Add/ProductoPrecio", Name = "PostAddProductPrecio")]
        public async Task<string> Post(string x, int z,bool t)
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

        //-------------------------- Actualizar cantidad en inventario en la sucursal ---------------------------------






    }
}
