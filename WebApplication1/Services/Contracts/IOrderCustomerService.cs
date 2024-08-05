using WebApplication1.Data.DTO;

namespace WebApplication1.Services.Contracts
{
    public interface IOrderCustomerService
    {
        public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(DateTime date);
    }
}
