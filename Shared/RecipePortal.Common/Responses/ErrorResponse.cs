namespace RecipePortal.Common.Responses;

public class ErrorResponse
{
    public int ErrorCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ErrorResponseFieldInfo> FieldErrors { get; set; } = Enumerable.Empty<ErrorResponseFieldInfo>();
}

public class ErrorResponseFieldInfo
{
    public string FieldName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty ;
}
