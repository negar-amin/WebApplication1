using WebApplication1.GraphQL.Graphql_schema;

namespace WebApplication1.GraphQL.Graphql_Schema_Configuration
{
	public class MessageConfiguration : ObjectType<Message>
	{
		protected override void Configure(IObjectTypeDescriptor<Message> descriptor)
		{
			descriptor.Field(m => m.Id).Type<NonNullType<IdType>>();
			descriptor.Field(m => m.Content).Type<NonNullType<StringType>>();
		}
	}
}
