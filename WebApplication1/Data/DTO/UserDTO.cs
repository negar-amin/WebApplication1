using WebApplication1.Data.Models;

namespace WebApplication1.Data.DTO
{
	public class AddUserDTO
	{
		public Role Role { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
	}
	public class UpdateUserDTO
	{
		public int? Id { set; get; }
		public Role? Role { set; get; }
		public string? FirstName { set; get; }
		public string? LastName { set; get; }
	}
}
