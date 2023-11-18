namespace core.Models
{
    public class TaskUser
    {
        public long UserId {get;set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public bool TaskManager {get;set;}
    }
}
