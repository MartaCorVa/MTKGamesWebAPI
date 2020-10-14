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
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("RegistrarUsuario")]
        [AllowAnonymous]
        public IHttpActionResult RegistrarUsuario(UsuarioModel usuario)
        {
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                UsuarioModel user;
                string sql = "INSERT INTO dbo.Usuario (Nombre, NickName, Email, Password, FechaNacimiento, " +
                    "UltimaConexion, Saldo, Login) " +
                    $"VALUES ('defecto','{usuario.NickName}', '{usuario.Email}', '{usuario.Password}', " +
                    $"Convert(Datetime2,'{DateTime.Now}',103), Convert(Datetime2,'{DateTime.Now}',103), 0, 1)";
                // Convert(Datetime2,'{DateTime.Now}',103)
                // '{DateTime.Now}'

                try
                {
                    con.Execute(sql);
                    user = ObtenerUsuario(usuario.NickName);
                }
                catch (Exception e)
                {
                    return BadRequest("Error al registrar un usuario ==> " + e.Message);
                }

                string sql2 = "INSERT INTO dbo.Biblioteca (IdBiblioteca, JuegosTotales) VALUES " +
                    $"({user.IdUsuario},0)";

                string sql3 = "UPDATE dbo.Usuario " +
                    $"SET IdBiblioteca = {user.IdUsuario} " +
                    $"WHERE IdUsuario = {user.IdUsuario}";

                try
                {
                    con.Execute(sql2);
                    con.Execute(sql3);
                }
                catch (Exception e)
                {
                    return BadRequest("Error al registrar un usuario ==> " + e.Message);
                }

            }
            return Ok<string>("Registro");
        }

        [HttpGet]
        [Route("ObtenerUsuario/{nick}")]
        [AllowAnonymous]
        public UsuarioModel ObtenerUsuario(string nick)
        {
            UsuarioModel usuario = null;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"select * from dbo.Usuario where NickName = '{nick}'";

                try
                {
                    usuario = con.Query<UsuarioModel>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener un usuario ==> " + e.Message);
                }
            }
            return usuario;
        }

        [HttpGet]
        [Route("ObtenerUsuario/{id}")]
        [AllowAnonymous]
        public UsuarioModel ObtenerUsuario(int id)
        {
            UsuarioModel usuario = null;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"select * from dbo.Usuario where IdUsuario = {id}";

                try
                {
                    usuario = con.Query<UsuarioModel>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al obtener un usuario ==> " + e.Message);
                }
            }
            return usuario;
        }

        [HttpPost]
        [Route("CerrarSesion")]
        [AllowAnonymous]
        public IHttpActionResult CerrarSesion(UsuarioModel usuario)
        {
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "UPDATE dbo.Usuario " +
                    $"SET Login = 0 " +
                    $"WHERE IdUsuario = {usuario.IdUsuario}";

                try
                {
                    con.Execute(sql);
                }
                catch (Exception e)
                {
                    return BadRequest("Error al cerrar sesión ==> " + e.Message);
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("IniciarSesion")]
        [AllowAnonymous]
        public IHttpActionResult IniciarSesion(UsuarioModel usuario)
        {
            UsuarioModel usuarioModel = null;

            if (ComprobarDatos(usuario))
            {
                using (IDbConnection con = new ApplicationDbContext().Database.Connection)
                {
                    usuario = ObtenerUsuario(usuario.NickName);

                    string sql = "UPDATE dbo.Usuario " +
                        $"SET Login = 1 " +
                        $"WHERE IdUsuario = {usuario.IdUsuario}";

                    try
                    {
                        con.Execute(sql);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error al iniciar sesión ==> " + e.Message);
                    }
                    usuarioModel = ObtenerUsuario(usuario.NickName);
                }
                return Ok<UsuarioModel>(usuarioModel);
            }
            return Ok<string>("Error");
        }

        public bool ComprobarDatos(UsuarioModel usuario)
        {
            String password;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"select Password from dbo.Usuario where NickName = '{usuario.NickName}'";

                try
                {
                    password = con.Query<String>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al comprobar datos ==> " + e.Message);
                }
            }
            if ((usuario.Password).Equals(password))
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("CargarMonedero")]
        [AllowAnonymous]
        public UsuarioModel CargarMonedero(UsuarioModel usuario)
        {
            UsuarioModel usuarioModel = null;

            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "UPDATE dbo.Usuario " +
                        $"SET Saldo = {usuario.Saldo} " +
                        $"WHERE IdUsuario = {usuario.IdUsuario}";

                try
                {
                    con.Execute(sql);
                }
                catch (Exception e)
                {
                    throw new Exception("Error al iniciar sesión ==> " + e.Message);
                }
                usuarioModel = ObtenerUsuario(usuario.NickName);
            }
            return usuarioModel;
        }

        [HttpPost]
        [Route("ModificarPerfil")]
        [AllowAnonymous]
        public UsuarioModel ModificarPerfil(UsuarioModel usuario)
        {
            UsuarioModel usuarioModel = null;

            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = "UPDATE dbo.Usuario " +
                        $"SET Nombre = '{usuario.Nombre}', " +
                        $"Email = '{usuario.Email}', " +
                        $"Password = '{usuario.Password}' " +
                        $"WHERE IdUsuario = {usuario.IdUsuario}";

                try
                {
                    con.Execute(sql);
                }
                catch (Exception e)
                {
                    throw new Exception("Error al modificar un usuario ==> " + e.Message);
                }
                usuarioModel = ObtenerUsuario(usuario.NickName);
            }
            return usuarioModel;
        }

        [HttpPost]
        [Route("CompruebaNickName")]
        [AllowAnonymous]
        public IHttpActionResult CompruebaNickName(UsuarioModel user)
        {
            UsuarioModel usuario = null;
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"select * from dbo.Usuario where NickName = '{user.NickName}'";

                try
                {
                    usuario = con.Query<UsuarioModel>(sql).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al comprobar nickname ==> " + e.Message);
                }
            }
            if (usuario == null)
            {
                return Ok<string>("Correcto");
            }
            else
            {
                return Ok<string>("Error");
            }
        }


    }
}
