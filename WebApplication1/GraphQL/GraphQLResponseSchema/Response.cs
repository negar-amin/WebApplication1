namespace WebApplication1.GraphQL.GraphQLResponseSchema
{
    public class Response<T> 
    {
        public List<ResponseError> Errors { get; set; }
        public T? Data { get; set; }
        public Response()
        {
            Errors = new List<ResponseError>();
        }
    }
}
