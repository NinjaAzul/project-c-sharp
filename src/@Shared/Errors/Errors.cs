using System.Collections.Generic;

namespace Project_C_Sharp.Shared.Errors;

public class ApiErrorResponse
{
    public string TraceId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}