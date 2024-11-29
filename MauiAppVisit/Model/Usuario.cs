namespace MauiAppVisit.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Usuario(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
