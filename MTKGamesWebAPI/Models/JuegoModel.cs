using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTKGamesWebAPI.Models
{
    public class JuegoModel
    {
        public int IdJuego;
        public string Nombre;
        public string Descripcion;
        public byte[] ImagenJuego;
        public byte[] IconoJuego;
        public string EmpresaDesarrollo;
        public decimal Nota;
        public int Precio;
        public int NumeroVentas;
        public int IdCategoria;
    }
}