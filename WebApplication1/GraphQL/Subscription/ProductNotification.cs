using WebApplication1.Data.Entities;
using HotChocolate.Authorization;
using WebApplication1.Data.Enums;

namespace WebApplication1.GraphQL.Subscription
{
	[ExtendObjectType(typeof(Subscription))]
	public class ProductNotification
	{
		[Subscribe]
		[Topic]
		public Notification AddedToStock([EventMessage]Notification message) => message;
	}
}
