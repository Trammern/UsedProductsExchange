namespace UsedProductExchange.Core.Entities
{
    public class RegisterInputModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}