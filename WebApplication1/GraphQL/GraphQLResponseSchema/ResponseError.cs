namespace WebApplication1.GraphQL.GraphQLResponseSchema
{
    public record ResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public ResponseError(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public  override string ToString()
        {
            return $"{Code}:{Message}:{Detail}";
        }
        public static ResponseError Success = new ResponseError(200, "success");
        public static ResponseError NotFound = new ResponseError(1, "NotFound");
        public static ResponseError PasswordRquirement = new ResponseError(2, "PasswordRequirementViolation");
        public static ResponseError DuplicateUniqueProperty = new ResponseError(3, "ValueAlreadyExists");
    }
}
