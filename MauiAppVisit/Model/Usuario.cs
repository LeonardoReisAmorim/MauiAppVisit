namespace MauiAppVisit.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Usuario(string nome, string email, string password)
        {
            Nome = nome;
            Email = email;
            Password = password;
        }
    }
}
