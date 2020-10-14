using Dapper;
using MTKGamesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MTKGamesWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Juego")]
    public class JuegoController : ApiController
    {
        [HttpPost]
        [Route("InsertarJuego")]
        [AllowAnonymous]
        public IHttpActionResult InsertarJuego(JuegoModel juego)
        {
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.Juego (Nombre, Descripcion, EmpresaDesarrollo," +
                    " Nota, Precio, NumeroVentas, IdCategoria) " +
                    $"VALUES ('{juego.Nombre}','{juego.Descripcion}', " +
                    $"'{juego.EmpresaDesarrollo}', {juego.Nota},{juego.Precio}, " +
                    $"{juego.NumeroVentas},{juego.IdCategoria})";

                try
                {
                    con.Execute(sql);
                }
                catch (Exception e)
                {
                    return BadRequest("Error al insertar un juego, " + e.Message);
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("InsertarImagenJuego")]
        [AllowAnonymous]
        public HttpResponseMessage InsertarImagenJuego()
        {
            // https://stackoverflow.com/questions/31839449/how-to-upload-image-via-webapi
            // https://dapper-tutorial.net/knowledge-base/39354100/unable-to-insert-file-stream-into-sql-filetable-using-dapper-net

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count != 1)
                {
                    var message = string.Format("Can't send more than 1 file at a time");

                    dict.Add("error", message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                }

                if (string.IsNullOrEmpty(httpRequest.Params.Get("idJuego")))
                {
                    var message = string.Format("We need to know the id of the game");

                    dict.Add("error", message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                HttpPostedFile postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {

                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {

                        var message = string.Format("Please Upload a file upto 1 mb.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else
                    {
                        using (IDbConnection con = new ApplicationDbContext().Database.Connection)
                        {
                            var imagenJuego = postedFile.InputStream;
                            int idJuego = int.Parse(httpRequest.Params.Get("idJuego"));

                            string sql = "UPDATE dbo.Juego " +
                                        $"SET ImagenJuego = @imagenJuego " +
                                        $"WHERE IdJuego = {idJuego}";
                            var dynParams = new DynamicParameters();
                            dynParams.Add("@imagenJuego", imagenJuego, DbType.Binary);

                            try
                            {
                                con.Execute(sql, dynParams);
                            }
                            catch (Exception e)
                            {
                                var message = string.Format("Error al insertar un la imagen del juego, " + e.Message);

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                        }

                    }
                }
                var message1 = string.Format("Image Updated Successfully.");
                return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpPost]
        [Route("InsertarIconoJuego")]
        [AllowAnonymous]
        public HttpResponseMessage InsertarIconoJuego()
        {
            // https://stackoverflow.com/questions/31839449/how-to-upload-image-via-webapi
            // https://dapper-tutorial.net/knowledge-base/39354100/unable-to-insert-file-stream-into-sql-filetable-using-dapper-net

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count != 1)
                {
                    var message = string.Format("Can't send more than 1 file at a time");

                    dict.Add("error", message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                }

                if (string.IsNullOrEmpty(httpRequest.Params.Get("idJuego")))
                {
                    var message = string.Format("We need to know the id of the game");

                    dict.Add("error", message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                HttpPostedFile postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {

                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {

                        var message = string.Format("Please Upload a file upto 1 mb.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else
                    {
                        using (IDbConnection con = new ApplicationDbContext().Database.Connection)
                        {
                            var iconoJuego = postedFile.InputStream;
                            int idJuego = int.Parse(httpRequest.Params.Get("idJuego"));

                            string sql = "UPDATE dbo.Juego " +
                                        $"SET IconoJuego = @iconoJuego " +
                                        $"WHERE IdJuego = {idJuego}";
                            var dynParams = new DynamicParameters();
                            dynParams.Add("@iconoJuego", iconoJuego, DbType.Binary);

                            try
                            {
                                con.Execute(sql, dynParams);
                            }
                            catch (Exception e)
                            {
                                var message = string.Format("Error al insertar el icono del juego, " + e.Message);

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                        }

                    }
                }
                var message1 = string.Format("Image Updated Successfully.");
                return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpGet]
        [Route("ObtenerImagen/{id}")]
        [AllowAnonymous]
        public byte[] ObtenerImagen(int id)
        {
            byte[] imagenBytes;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT ImagenJuego FROM dbo.Juego where IdJuego = " + id;

                try
                {
                    imagenBytes = con.Query<byte[]>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener todos los juegos " + e.Message);
                }
            }
            return imagenBytes;
        }

        [HttpGet]
        [Route("ObtenerJuegos")]
        [AllowAnonymous]
        public List<JuegoModel> ObtenerJuegos()
        {
            List<JuegoModel> juegos;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT * FROM dbo.Juego";

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

        [HttpGet]
        [Route("ObtenerJuegoId/{id}")]
        [AllowAnonymous]
        public JuegoModel ObtenerJuegoId(int id)
        {
            JuegoModel juego;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT * FROM dbo.Juego where IdJuego = " + id;

                try
                {
                    juego = con.Query<JuegoModel>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener un juego por id " + e.Message);
                }
            }
            return juego;
        }

        [HttpGet]
        [Route("ObtenerJuegoCategoria/{idCategoria}")]
        [AllowAnonymous]
        public List<JuegoModel> ObtenerJuegoCategoria(int idCategoria)
        {
            List<JuegoModel> juegos;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT * FROM dbo.Juego where IdCategoria = " + idCategoria;

                try
                {
                    juegos = con.Query<JuegoModel>(sql).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener juegos por categoria " + e.Message);
                }
            }
            return juegos;
        }

        [HttpGet]
        [Route("ObtenerBestSellers")]
        [AllowAnonymous]
        public List<JuegoModel> ObtenerBestSellers()
        {
            List<JuegoModel> juegos;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT TOP 5 * FROM dbo.Juego ORDER BY NumeroVentas DESC";

                try
                {
                    juegos = con.Query<JuegoModel>(sql).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener los juegos más vendidos " + e.Message);
                }
            }
            return juegos;
        }

        [HttpGet]
        [Route("ObtenerMejorValorados")]
        [AllowAnonymous]
        public List<JuegoModel> ObtenerMejorValorados()
        {
            List<JuegoModel> juegos;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "SELECT TOP 3 * FROM dbo.Juego ORDER BY Nota DESC";

                try
                {
                    juegos = con.Query<JuegoModel>(sql).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener los juegos mejor valorados " + e.Message);
                }
            }
            return juegos;
        }

        [HttpPost]
        [Route("ComprarJuego")]
        [AllowAnonymous]
        public IHttpActionResult ComprarJuego(BibliotecaJuegoModel compra)
        {
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                UsuarioController usuarioController = new UsuarioController();
                UsuarioModel usuario = usuarioController.ObtenerUsuario(compra.IdBiblioteca);
                JuegoModel juego = ObtenerJuegoId(compra.IdJuego);

                if (usuario.Saldo >= juego.Precio)
                {
                    int num = 0;
                    string sqlComprueba = "select count(IdJuego) from dbo.BibliotecaJuego where " +
                        $"IdBiblioteca = {usuario.IdBiblioteca} and IdJuego = {juego.IdJuego}";

                    try
                    {
                        num = con.Query<int>(sqlComprueba).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Error al comprar un juego, " + e.Message);
                    }

                    if (num == 0)
                    {
                        string sql = "INSERT INTO dbo.BibliotecaJuego (IdBiblioteca, IdJuego) VALUES " +
                            $"({usuario.IdBiblioteca},{juego.IdJuego})";

                        int numVentas = juego.NumeroVentas + 1;

                        string sql2 = "UPDATE dbo.Juego SET " +
                            "NumeroVentas = " + numVentas + " WHERE IdJuego = " + juego.IdJuego;

                        int saldo = usuario.Saldo - juego.Precio;

                        string sql3 = "UPDATE dbo.Usuario SET " +
                            "Saldo = " + saldo + " WHERE IdUsuario = " + usuario.IdUsuario;

                        try
                        {
                            con.Execute(sql);
                            con.Execute(sql2);
                            con.Execute(sql3);
                        }
                        catch (Exception e)
                        {
                            return BadRequest("Error al comprar un juego, " + e.Message);
                        }
                        return Ok();
                    } 
                    return Ok<string>("Repetido");
                }
                return Ok<string>("Error");
            }
        }
    }
}