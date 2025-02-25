using System;
using FluentValidation.Results;

namespace Project_C_Sharp.Shared.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }
}

public class BadRequestException : AppException
{
    public List<ValidationFailure>? Errors { get; }

    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(List<ValidationFailure> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}