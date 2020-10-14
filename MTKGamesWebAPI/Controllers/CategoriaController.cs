using Dapper;
using MTKGamesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MTKGamesWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Categoria")]
    public class CategoriaController : ApiController
    {
        [HttpPost]
        [Route("InsertarCategoria")]
        [AllowAnonymous]
        public IHttpActionResult InsertarCategoria(CategoriaModel categoria)
        {
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.Categoria (Nombre) " +
                    $"VALUES ('{categoria.Nombre}')";

                try
                {
                    con.Execute(sql);
                }
                catch (Exception e)
                {
                    return BadRequest("Error al insertar una categoria, " + e.Message);
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("ObtenerCategorias")]
        [AllowAnonymous]
        public List<CategoriaModel> ObtenerJuegos()
        {
            List<CategoriaModel> categorias;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT * FROM dbo.Categoria";

                try
                {
                    categorias = con.Query<CategoriaModel>(sql).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener todas las categorias " + e.Message);
                }
            }
            return categorias;
        }
    }
}
