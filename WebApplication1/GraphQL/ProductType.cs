using HotChocolate.Types;

public class ProductType : ObjectType<Product>
{
	protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
	{
		descriptor.Field(p => p.Id).Type<NonNullType<IdType>>();
		descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
		descriptor.Field(p => p.Description).Type<StringType>();
		descriptor.Field(p => p.Price).Type<DecimalType>();
	}
}

