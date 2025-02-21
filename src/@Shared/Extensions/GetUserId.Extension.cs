
using Project_C_Sharp.Shared.Exceptions;

namespace Project_C_Sharp.Shared.GetUserId.Extensions;

public static class GetUserIdExtension
{
    public static Guid GetUserId(this HttpContext context)
    {
        // AQUI apenas recupera o ID que foi salvo anteriormente
        var userId = context.Items["UserId"]?.ToString();

        return Guid.Parse(userId!);
    }
}