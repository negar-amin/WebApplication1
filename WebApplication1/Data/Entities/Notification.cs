namespace WebApplication1.Data.Entities
{
	public class Notification
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public bool IsSeen { get; set; }
	}
}
