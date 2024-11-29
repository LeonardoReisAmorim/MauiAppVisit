namespace MauiAppVisit.Model
{
    public class Lugar
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int fileVRId { get; set; }
        public string image { get; set; }
        public byte[] ImagemByte { get; set; }
    }
}
