﻿using HotChocolate.Authorization;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(typeof(Mutation))]
	public class OrderMutation
	{
		public async Task<Order> AddOrder(int userId, List<AddOrderRequestDTO> productsInfo,[Service] IOrderService OrderService)
		{
			var order = await OrderService.AddOrderAsync(userId,productsInfo);
			return order;
		}
		public async Task<bool> DeleteOrder(int id, [Service]IOrderService orderService)
		{
			await orderService.DeleteOrderAsync(id);
			return true;
		}
	}
}
