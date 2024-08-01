using WebApplication1.GraphQL.Graphql_schema;
using WebApplication1.Data.Models;
using HotChocolate.Authorization;
using WebApplication1.Data.Enums;

namespace WebApplication1.GraphQL.Subscription
{
	[ExtendObjectType(Name ="Subscription")]
	public class ProductNotification
	{
		[Subscribe]
		[Topic]
		public Notification AddedToStock([EventMessage]Notification message) => message;
	}
}
