namespace MauiAppVisit.Model
{
    public class Lugar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FileVRId { get; set; }
        public string Image { get; set; }
        public byte[] ImagemByte { get; set; }
        public string FileName { get; set; }
    }
}
