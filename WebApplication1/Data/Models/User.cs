using WebApplication1.Data.Enums;

namespace WebApplication1.Data.Models
{
    public class User
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        [GraphQLIgnore]
		public string PasswordHash { get; set; }
        [GraphQLIgnore]
		public string PasswordSalt { get; set; }
		public Role Role { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
