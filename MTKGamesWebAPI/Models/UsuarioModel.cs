using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTKGamesWebAPI.Models
{
    public class UsuarioModel
    {
        public int IdUsuario;
        public string Nombre;
        public string NickName;
        public string Email;
        public string Password;
        public DateTime FechaNacimiento;
        public byte FotoPerfil;
        public DateTime UltimaConexion;
        public int Saldo;
        public int Login;
        public int IdBiblioteca;
    }
}