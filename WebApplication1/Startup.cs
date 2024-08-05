using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebApplication1.Services;
using WebApplication1.GraphQL.Mutation;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.Query;
using WebApplication1.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;
using WebApplication1.GraphQL.Subscription;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore;
using WebApplication1.Data.Mapster;
using WebApplication1.Repositories.Contracts;
using WebApplication1.Services.Contracts;

public class Startup
{
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		MapConfiguration.RegisterMapping();
		var key = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey"));

		services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false
			};
		});
		services.AddAuthorization(options =>
		{
			options.AddPolicy("admin access", policy => policy.RequireRole(Role.admin.GetDisplayName()));
			options.AddPolicy("staff access", policy => policy.RequireRole(Role.staff.GetDisplayName()));
			options.AddPolicy("customer access", policy => policy.RequireRole(Role.customer.GetDisplayName()));
		});
		//services.AddDbContext<ApplicationDbContext>(options =>
		//	options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseMySql(Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 23))));
		services.AddHttpContextAccessor();
		services.AddScoped(typeof(ICRUDRepository<>), typeof(CRUDRepository<>));
		services.AddScoped<IOrderCustomerService, OrderCustomerService>();
		services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
		services.AddScoped<IProductService, ProductService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IOrderService, OrderService>();
		services.AddScoped(typeof(IJoinTableRepository<>), typeof(JoinTableRepository<>));
		services.AddScoped<IProductOrderService, ProductOrderService>();
		services.AddSingleton<TokenService>(ts => new TokenService(Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey")));
		services.AddScoped<INotificationService, NotificationService>();
		services
			.AddGraphQLServer()
			.AddAuthorization()
			.AddQueryType<WebApplication1.GraphQL.Query.Query>()
				.AddTypeExtension<OrderQueries>()
				.AddTypeExtension<ProductQueries>()
				.AddTypeExtension<UserQueries>()
			.AddMutationType<Mutation>()
				.AddTypeExtension<ProductMutations>()
				.AddTypeExtension<UserMutations>()
				.AddTypeExtension<OrderMutations>()
			.AddSubscriptionType<Subscription>()
				.AddTypeExtension<ProductNotification>()
			.AddInMemorySubscriptions()
			.AddProjections();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseWebSockets();
		app.UseEndpoints(endpoints =>
		{
			// Map GraphQL endpoint
			endpoints.MapGraphQL();
		});
		app.UseGraphQLPlayground();
	}
}
