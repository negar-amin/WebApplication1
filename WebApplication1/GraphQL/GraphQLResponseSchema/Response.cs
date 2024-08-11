namespace WebApplication1.GraphQL.GraphQLResponseSchema
{
    public class Response<T> 
    {
        public List<ResponseError> Status { get; set; }
        public T? Data { get; set; }
        public Response()
        {
            Status = new List<ResponseError>();
        }
    }
}
