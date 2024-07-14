namespace WebApplication1.Data.Models
{
    public class User
    {
        public int Id { set; get; }
        public Role Role { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public ICollection<Order> Orders { get; set; }

    }
}
