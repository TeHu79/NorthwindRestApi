namespace NorthwindRestApi.Models
{
    public class LoggedUser
    {
        public string Username { get; set; }
        public int AccesslevelId { get; set; }
        public string? Token { get; set; }
    }
}
