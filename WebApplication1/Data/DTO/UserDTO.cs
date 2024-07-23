using WebApplication1.Data.Enums;

namespace WebApplication1.Data.DTO
{
    public class AddUserDTO
	{
		public Role Role { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
	public class UpdateUserDTO
	{
		public int? Id { set; get; }
		public Role? Role { set; get; }
		public string? FirstName { set; get; }
		public string? LastName { set; get; }
	}
}
