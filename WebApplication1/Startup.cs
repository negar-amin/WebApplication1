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
using WebApplication1.Data.Models;
using WebApplication1.GraphQL.Query;
using WebApplication1.Repositories;
using WebApplication1.Data.DTO.Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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
		//services.AddDbContext<ApplicationDbContext>(options =>
		//	options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseMySql(Configuration.GetConnectionString("MySQL"), new MySqlServerVersion(new Version(8, 0, 23))));

		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		services.AddScoped<IOrderCustomerService, OrderCustomerService>();
		services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
		services.AddScoped<IProductService, ProductService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IOrderService, OrderService>();
		services.AddScoped(typeof(IJoinTableRepository<>), typeof(JoinTableRepository<>));
		services.AddScoped<IProductOrderService, ProductOrderService>();
		services.AddSingleton<TokenService>(ts => new TokenService(Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey")));
		services
			.AddGraphQLServer()
			.AddAuthorization()
			.AddQueryType(d => d.Name("Query"))
				.AddTypeExtension<ProductQuery>()
				.AddTypeExtension<UserQuery>()
				.AddTypeExtension<OrderQuery>()
			.AddMutationType(d => d.Name("Mutation"))
				.AddTypeExtension<ProductMutation>()
				.AddTypeExtension<UserMutation>()
				.AddTypeExtension<OrderMutation>()
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

		app.UseEndpoints(endpoints =>
		{
			// Map GraphQL endpoint
			endpoints.MapGraphQL();
		});
	}
}
