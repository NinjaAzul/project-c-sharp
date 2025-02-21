using System;

namespace Project_C_Sharp.Shared.Exceptions;

public abstract class AppException(string message) : Exception(message)
{
}

public class BadRequestException(string message) : AppException(message)
{
}

public class NotFoundException(string message) : AppException(message)
{
}

public class UnauthorizedException(string message) : AppException(message)
{
}