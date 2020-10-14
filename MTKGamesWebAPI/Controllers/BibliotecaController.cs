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
    [RoutePrefix("api/Biblioteca")]
    public class BibliotecaController : ApiController
    {
        [HttpGet]
        [Route("ObtenerNumJuegos/{id}")]
        [AllowAnonymous]
        public int ObtenerNumJuegos(int id)
        {
            int juegos = 0;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT count(IdJuego) FROM dbo.BibliotecaJuego where " +
                    "IdBiblioteca = " + id;

                try
                {
                    juegos = con.Query<int>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener todos los juegos " + e.Message);
                }
            }
            return juegos;
        }

        [HttpGet]
        [Route("ObtenerJuegos/{id}")]
        [AllowAnonymous]
        public List<JuegoModel> ObtenerJuegos(int id)
        {
            List<JuegoModel> juegos;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT * FROM dbo.Juego as j, dbo.BibliotecaJuego as b where " +
                    "b.IdBiblioteca = " + id + " and b.IdJuego = j.IdJuego";

                try
                {
                    juegos = con.Query<JuegoModel>(sql).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener todos los juegos " + e.Message);
                }
            }
            return juegos;
        }

    }
}
