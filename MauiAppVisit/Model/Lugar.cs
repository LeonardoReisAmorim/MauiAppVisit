using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppVisit.Model
{
    public class Lugar
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string imagem { get; set; }
        public byte[] ImagemByte { get; set; }
    }
}
