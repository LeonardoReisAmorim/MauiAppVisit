﻿namespace MauiAppVisit.Model
{
    public class Lugar
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public int arquivoId { get; set; }
        public string imagem { get; set; }
        public byte[] ImagemByte { get; set; }
    }
}
